using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using MySqlConnector;

namespace SqlQuery.Db
{
    public class MySqlProvider : DbProviderBase
    {
        public MySqlProvider(ConnectionInfo info)
            : base(info)
        {
        }

        public override string DisplayName
        {
            get { return "MySQL"; }
        }

        public override DbConnection CreateConnection()
        {
            return new MySqlConnection(Info.BuildConnectionString());
        }

        public override string QuoteIdentifier(string name)
        {
            return "`" + name.Replace("`", "``") + "`";
        }

        private static string ExcludeSystemSchemas
        {
            get { return "('information_schema','mysql','performance_schema','sys')"; }
        }

        public override DataTable GetTables()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT TABLE_SCHEMA AS table_schema, TABLE_NAME AS table_name, 'TABLE' AS table_type " +
                    "FROM information_schema.TABLES " +
                    "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA NOT IN " + ExcludeSystemSchemas + " " +
                    "UNION ALL " +
                    "SELECT TABLE_SCHEMA, TABLE_NAME, 'VIEW' " +
                    "FROM information_schema.TABLES " +
                    "WHERE TABLE_TYPE = 'VIEW' AND TABLE_SCHEMA NOT IN " + ExcludeSystemSchemas + " " +
                    "ORDER BY table_schema, table_name", null);
            });
        }

        public override DataTable GetProcedures()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT ROUTINE_SCHEMA AS table_schema, ROUTINE_NAME AS table_name " +
                    "FROM information_schema.ROUTINES " +
                    "WHERE ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_SCHEMA NOT IN " + ExcludeSystemSchemas + " " +
                    "ORDER BY table_schema, table_name", null);
            });
        }

        public override List<string> GetDatabases()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                List<string> names = new List<string>();
                foreach (DataRow row in Fill(connection, "SHOW DATABASES", null).Rows)
                {
                    string name = Convert.ToString(row[0]);
                    if (name != "information_schema" && name != "mysql" && name != "performance_schema" && name != "sys")
                    {
                        names.Add(name);
                    }
                }
                return names;
            });
        }

        public override DataTable GetColumns(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@schema", schema);
                parameters.Add("@table", table);
                return Fill(connection,
                    "SELECT COLUMN_NAME AS column_name, COLUMN_TYPE AS data_type, " +
                    "IS_NULLABLE AS is_nullable, " +
                    "COALESCE(COLUMN_DEFAULT, '') AS default_value, " +
                    "'NO' AS is_identity, " +
                    "ORDINAL_POSITION AS ordinal " +
                    "FROM information_schema.COLUMNS " +
                    "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table " +
                    "ORDER BY ORDINAL_POSITION", parameters);
            });
        }

        public override DataTable GetPrimaryKeys(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@schema", schema);
                parameters.Add("@table", table);
                return Fill(connection,
                    "SELECT COLUMN_NAME AS column_name, ORDINAL_POSITION AS ordinal " +
                    "FROM information_schema.KEY_COLUMN_USAGE " +
                    "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table AND CONSTRAINT_NAME = 'PRIMARY' " +
                    "ORDER BY ORDINAL_POSITION", parameters);
            });
        }

        public override DataTable GetIndexes(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@schema", schema);
                parameters.Add("@table", table);
                return Fill(connection,
                    "SELECT INDEX_NAME AS index_name, MAX(NON_UNIQUE = 0) AS is_unique, " +
                    "GROUP_CONCAT(COLUMN_NAME ORDER BY SEQ_IN_INDEX) AS columns " +
                    "FROM information_schema.STATISTICS " +
                    "WHERE TABLE_SCHEMA = @schema AND TABLE_NAME = @table AND INDEX_NAME <> 'PRIMARY' " +
                    "GROUP BY INDEX_NAME " +
                    "ORDER BY index_name", parameters);
            });
        }

        public override DataTable GetTableRights()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT CONCAT(TABLE_SCHEMA, '.', TABLE_NAME) AS object_name, " +
                    "PRIVILEGE_TYPE AS permission, " +
                    "GRANTEE AS grantee, " +
                    "'GRANTED' AS state " +
                    "FROM information_schema.table_privileges " +
                    "WHERE TABLE_SCHEMA NOT IN " + ExcludeSystemSchemas + " " +
                    "ORDER BY object_name, permission", null);
            });
        }

        public override string GetObjectDefinition(string schema, string name, string type)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                string quoted = "`" + schema.Replace("`", "``") + "`.`" + name.Replace("`", "``") + "`";
                string sql = type == "PROCEDURE"
                    ? "SHOW CREATE PROCEDURE " + quoted
                    : "SHOW CREATE VIEW " + quoted;
                DataTable result = Fill(connection, sql, null);
                if (result.Rows.Count == 0)
                {
                    return "-- Definition not found for " + schema + "." + name;
                }
                return Convert.ToString(result.Rows[0][2]);
            });
        }

        public override DataTable GetColumnRights()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT CONCAT(c.TABLE_SCHEMA, '.', c.TABLE_NAME, '.', c.COLUMN_NAME) AS object_name, " +
                    "tp.PRIVILEGE_TYPE AS permission, " +
                    "tp.GRANTEE AS grantee, " +
                    "'GRANTED' AS state, " +
                    "c.COLUMN_NAME AS column_name " +
                    "FROM information_schema.COLUMNS c " +
                    "JOIN information_schema.TABLE_PRIVILEGES tp " +
                    "ON tp.TABLE_SCHEMA = c.TABLE_SCHEMA AND tp.TABLE_NAME = c.TABLE_NAME " +
                    "AND tp.PRIVILEGE_TYPE IN ('SELECT','UPDATE') " +
                    "WHERE c.TABLE_SCHEMA NOT IN " + ExcludeSystemSchemas + " " +
                    "UNION " +
                    "SELECT CONCAT(TABLE_SCHEMA, '.', TABLE_NAME, '.', COLUMN_NAME), " +
                    "PRIVILEGE_TYPE, " +
                    "GRANTEE, " +
                    "'GRANTED', " +
                    "COLUMN_NAME " +
                    "FROM information_schema.column_privileges " +
                    "WHERE TABLE_SCHEMA NOT IN " + ExcludeSystemSchemas + " " +
                    "ORDER BY object_name, column_name, permission", null);
            });
        }

        public override string GetSchemaDdl()
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in GetTables().Rows)
            {
                if (Convert.ToString(row["table_type"]) != "TABLE")
                {
                    continue;
                }
                string schema = Convert.ToString(row["table_schema"]);
                string table = Convert.ToString(row["table_name"]);
                sb.Append(BuildTableDdl(schema, table, GetColumns(schema, table), GetPrimaryKeys(schema, table)));
            }
            if (sb.Length == 0)
            {
                return "-- No tables found.";
            }
            return sb.ToString();
        }

        public override string BuildSelectTop(TableRef table, int limit)
        {
            return "SELECT * FROM " + QuoteFullName(table.Schema, table.Name) + " LIMIT " + limit;
        }

        public override string GetClrType(string dataType)
        {
            string type = dataType.ToLowerInvariant();
            int paren = type.IndexOf('(');
            if (paren >= 0)
            {
                type = type.Substring(0, paren).Trim();
            }
            switch (type)
            {
                case "tinyint":
                    return "byte";
                case "smallint":
                    return "short";
                case "mediumint":
                case "int":
                case "integer":
                    return "int";
                case "bigint":
                    return "long";
                case "bit":
                    return "bool";
                case "float":
                    return "float";
                case "double":
                    return "double";
                case "decimal":
                case "numeric":
                    return "decimal";
                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                case "enum":
                case "set":
                case "json":
                    return "string";
                case "date":
                case "datetime":
                case "timestamp":
                    return "DateTime";
                case "time":
                    return "TimeSpan";
                case "year":
                    return "int";
                case "binary":
                case "varbinary":
                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblob":
                    return "byte[]";
                default:
                    return "object";
            }
        }
    }
}
