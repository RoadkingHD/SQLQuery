using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace SqlQuery.Db
{
    public class SqliteProvider : DbProviderBase
    {
        public SqliteProvider(ConnectionInfo info)
            : base(info)
        {
        }

        public override string DisplayName
        {
            get { return "SQLite"; }
        }

        public override DbConnection CreateConnection()
        {
            return new SQLiteConnection(Info.BuildConnectionString());
        }

        public override string QuoteIdentifier(string name)
        {
            return "\"" + name.Replace("\"", "\"\"") + "\"";
        }

        private static string QuoteLiteral(string name)
        {
            return "'" + name.Replace("'", "''") + "'";
        }

        public override DataTable GetTables()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT '' AS table_schema, name AS table_name, " +
                    "CASE type WHEN 'view' THEN 'VIEW' ELSE 'TABLE' END AS table_type " +
                    "FROM sqlite_master WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%' " +
                    "ORDER BY table_name", null);
            });
        }

        public override DataTable GetColumns(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                DataTable raw = Fill(connection, "PRAGMA table_info(" + QuoteLiteral(table) + ")", null);
                DataTable result = new DataTable();
                result.Columns.Add("column_name", typeof(string));
                result.Columns.Add("data_type", typeof(string));
                result.Columns.Add("is_nullable", typeof(bool));
                result.Columns.Add("default_value", typeof(string));
                result.Columns.Add("is_identity", typeof(bool));
                result.Columns.Add("ordinal", typeof(int));
                foreach (DataRow row in raw.Rows)
                {
                    string type = Convert.ToString(row["type"]);
                    if (type.Length == 0)
                    {
                        type = "BLOB";
                    }
                    result.Rows.Add(
                        Convert.ToString(row["name"]),
                        type,
                        Convert.ToInt64(row["notnull"]) == 0,
                        row["dflt_value"] == DBNull.Value ? string.Empty : Convert.ToString(row["dflt_value"]),
                        false,
                        Convert.ToInt32(row["cid"]));
                }
                return result;
            });
        }

        public override DataTable GetPrimaryKeys(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                DataTable raw = Fill(connection, "PRAGMA table_info(" + QuoteLiteral(table) + ")", null);
                DataTable result = new DataTable();
                result.Columns.Add("column_name", typeof(string));
                result.Columns.Add("ordinal", typeof(int));
                List<DataRow> keyRows = new List<DataRow>();
                foreach (DataRow row in raw.Rows)
                {
                    if (Convert.ToInt32(row["pk"]) > 0)
                    {
                        keyRows.Add(row);
                    }
                }
                keyRows.Sort(delegate(DataRow a, DataRow b)
                {
                    return Convert.ToInt32(a["pk"]).CompareTo(Convert.ToInt32(b["pk"]));
                });
                foreach (DataRow row in keyRows)
                {
                    result.Rows.Add(Convert.ToString(row["name"]), Convert.ToInt32(row["pk"]));
                }
                return result;
            });
        }

        public override DataTable GetIndexes(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                DataTable raw = Fill(connection, "PRAGMA index_list(" + QuoteLiteral(table) + ")", null);
                DataTable result = new DataTable();
                result.Columns.Add("index_name", typeof(string));
                result.Columns.Add("is_unique", typeof(bool));
                result.Columns.Add("columns", typeof(string));
                foreach (DataRow row in raw.Rows)
                {
                    string indexName = Convert.ToString(row["name"]);
                    DataTable info = Fill(connection, "PRAGMA index_info(" + QuoteLiteral(indexName) + ")", null);
                    List<string> columns = new List<string>();
                    foreach (DataRow infoRow in info.Rows)
                    {
                        columns.Add(Convert.ToString(infoRow["name"]));
                    }
                    result.Rows.Add(indexName, Convert.ToInt64(row["unique"]) != 0, string.Join(", ", columns.ToArray()));
                }
                return result;
            });
        }

        public override DataTable GetProcedures()
        {
            DataTable result = new DataTable();
            result.Columns.Add("table_schema", typeof(string));
            result.Columns.Add("table_name", typeof(string));
            return result;
        }

        public override List<string> GetDatabases()
        {
            return new List<string>();
        }

        private static DataTable RightsNotSupported(string note)
        {
            DataTable result = new DataTable();
            result.Columns.Add("grantee", typeof(string));
            result.Columns.Add("permission", typeof(string));
            result.Columns.Add("object_name", typeof(string));
            result.Columns.Add("state", typeof(string));
            result.Rows.Add("(SQLite)", "n/a", note, string.Empty);
            return result;
        }

        public override DataTable GetTableRights()
        {
            return RightsNotSupported("SQLite has no per-user privileges; access control is file-level.");
        }

        public override string GetObjectDefinition(string schema, string name, string type)
        {
            if (type == "PROCEDURE")
            {
                return "-- SQLite has no stored procedures.";
            }
            return WithConnection(delegate(DbConnection connection)
            {
                DataTable result = Fill(connection,
                    "SELECT sql FROM sqlite_master WHERE name = @name",
                    new Dictionary<string, object> { { "@name", name } });
                if (result.Rows.Count == 0)
                {
                    return "-- Definition not found for " + name;
                }
                return Convert.ToString(result.Rows[0][0]);
            });
        }

        public override DataTable GetColumnRights()
        {
            return RightsNotSupported("SQLite has no per-column privileges.");
        }

        public override string GetSchemaDdl()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                DataTable raw = Fill(connection,
                    "SELECT type, name, sql FROM sqlite_master WHERE sql IS NOT NULL " +
                    "ORDER BY CASE type WHEN 'table' THEN 0 WHEN 'index' THEN 1 WHEN 'trigger' THEN 2 WHEN 'view' THEN 3 END, name", null);
                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in raw.Rows)
                {
                    sb.AppendLine("-- " + Convert.ToString(row["type"]) + ": " + Convert.ToString(row["name"]));
                    sb.AppendLine(Convert.ToString(row["sql"]) + ";");
                    sb.AppendLine();
                }
                if (sb.Length == 0)
                {
                    sb.AppendLine("-- No schema objects found.");
                }
                return sb.ToString();
            });
        }

        public override string GetClrType(string dataType)
        {
            switch (dataType.ToLowerInvariant())
            {
                case "text":
                case "clob":
                case "char":
                case "character":
                case "nchar":
                case "nvarchar":
                case "varchar":
                    return "string";
                case "int":
                case "integer":
                case "bigint":
                case "smallint":
                case "mediumint":
                    return "long";
                case "real":
                case "float":
                case "double":
                    return "double";
                case "numeric":
                case "decimal":
                    return "decimal";
                case "boolean":
                case "bit":
                    return "bool";
                case "datetime":
                case "date":
                case "timestamp":
                    return "DateTime";
                case "blob":
                    return "byte[]";
                default:
                    return "object";
            }
        }

        public override string BuildSelectTop(TableRef table, int limit)
        {
            return "SELECT * FROM " + QuoteIdentifier(table.Name) + " LIMIT " + limit;
        }
    }
}
