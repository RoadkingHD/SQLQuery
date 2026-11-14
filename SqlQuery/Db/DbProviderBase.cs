using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace SqlQuery.Db
{
    public abstract class DbProviderBase
    {
        protected readonly ConnectionInfo Info;

        protected DbProviderBase(ConnectionInfo info)
        {
            Info = info;
        }

        public abstract string DisplayName { get; }

        public abstract DbConnection CreateConnection();

        public abstract string QuoteIdentifier(string name);

        public abstract DataTable GetTables();

        public abstract DataTable GetColumns(string schema, string table);

        public abstract DataTable GetPrimaryKeys(string schema, string table);

        public abstract DataTable GetIndexes(string schema, string table);

        public abstract string GetObjectDefinition(string schema, string name, string type);

        public abstract DataTable GetTableRights();

        public abstract DataTable GetColumnRights();

        public abstract DataTable GetProcedures();

        public abstract List<string> GetDatabases();

        public abstract string GetSchemaDdl();

        public abstract string BuildSelectTop(TableRef table, int limit);

        public virtual void TestConnection()
        {
            using (DbConnection connection = CreateConnection())
            {
                connection.Open();
            }
        }

        public virtual List<string> GetCompletionTables()
        {
            List<string> names = new List<string>();
            foreach (DataRow row in GetTables().Rows)
            {
                string schema = Convert.ToString(row["table_schema"]);
                string name = Convert.ToString(row["table_name"]);
                names.Add(string.IsNullOrEmpty(schema) ? name : schema + "." + name);
            }
            return names;
        }

        public static DbProviderBase Create(ConnectionInfo info)
        {
            switch (info.ServerType)
            {
                case ServerType.SqlServer:
                    return new SqlServerProvider(info);
                case ServerType.Sqlite:
                    return new SqliteProvider(info);
                case ServerType.PostgreSql:
                    return new PostgresProvider(info);
                default:
                    throw new NotSupportedException("Unsupported server type.");
            }
        }

        protected T WithConnection<T>(Func<DbConnection, T> action)
        {
            using (DbConnection connection = CreateConnection())
            {
                connection.Open();
                return action(connection);
            }
        }

        protected static DataTable Fill(DbConnection connection, string sql, Dictionary<string, object> parameters)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> pair in parameters)
                    {
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = pair.Key;
                        parameter.Value = pair.Value ?? DBNull.Value;
                        command.Parameters.Add(parameter);
                    }
                }
                using (DbDataReader reader = command.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }

        public abstract string GetClrType(string dataType);

        public string BuildCSharpClass(TableRef table)
        {
            DataTable columns = GetColumns(table.Schema, table.Name);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public class " + ToPascalIdentifier(table.Name));
            sb.AppendLine("{");
            foreach (DataRow row in columns.Rows)
            {
                string columnName = Convert.ToString(row["column_name"]);
                string dataType = Convert.ToString(row["data_type"]);
                bool nullable = IsNullableValue(row["is_nullable"]);
                string clrType = GetClrType(dataType);
                if (nullable && IsClrValueType(clrType))
                {
                    clrType = clrType + "?";
                }
                string propertyName = ToPascalIdentifier(columnName);
                sb.Append("    public ").Append(clrType).Append(' ').Append(propertyName).Append(" { get; set; }");
                if (!string.Equals(propertyName, columnName, StringComparison.Ordinal))
                {
                    sb.Append(" // source column: ").Append(columnName);
                }
                sb.AppendLine();
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static bool IsClrValueType(string clrType)
        {
            switch (clrType)
            {
                case "int":
                case "long":
                case "short":
                case "byte":
                case "bool":
                case "double":
                case "float":
                case "decimal":
                case "DateTime":
                case "DateTimeOffset":
                case "TimeSpan":
                case "Guid":
                    return true;
                default:
                    return false;
            }
        }

        public static string ToPascalIdentifier(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return "Column";
            }
            StringBuilder sb = new StringBuilder();
            bool upperNext = true;
            foreach (char c in name)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(upperNext ? char.ToUpperInvariant(c) : c);
                    upperNext = false;
                }
                else
                {
                    upperNext = true;
                }
            }
            string result = sb.ToString();
            if (result.Length == 0)
            {
                return "Column";
            }
            if (char.IsDigit(result[0]))
            {
                result = "_" + result;
            }
            return result;
        }

        protected string QuoteFullName(string schema, string table)
        {
            return string.IsNullOrEmpty(schema) ? QuoteIdentifier(table) : QuoteIdentifier(schema) + "." + QuoteIdentifier(table);
        }

        protected string BuildTableDdl(string schema, string table, DataTable columns, DataTable primaryKeys)
        {
            StringBuilder sb = new StringBuilder();
            string target = QuoteFullName(schema, table);
            sb.AppendLine("-- " + target);
            sb.Append("CREATE TABLE ").Append(target).AppendLine(" (");
            List<string> defs = new List<string>();
            foreach (DataRow row in columns.Rows)
            {
                StringBuilder def = new StringBuilder();
                def.Append("    ").Append(QuoteIdentifier(Convert.ToString(row["column_name"])));
                def.Append(' ').Append(Convert.ToString(row["data_type"]));
                if (!IsNullableValue(row["is_nullable"]))
                {
                    def.Append(" NOT NULL");
                }
                string defaultValue = Convert.ToString(row["default_value"]);
                if (!string.IsNullOrEmpty(defaultValue))
                {
                    def.Append(" DEFAULT ").Append(defaultValue);
                }
                defs.Add(def.ToString());
            }
            if (primaryKeys.Rows.Count > 0)
            {
                List<string> keyColumns = new List<string>();
                List<DataRow> keyRows = new List<DataRow>(primaryKeys.Rows.Count);
                foreach (DataRow row in primaryKeys.Rows)
                {
                    keyRows.Add(row);
                }
                keyRows.Sort(delegate(DataRow a, DataRow b)
                {
                    return Convert.ToInt32(a["ordinal"]).CompareTo(Convert.ToInt32(b["ordinal"]));
                });
                foreach (DataRow row in keyRows)
                {
                    keyColumns.Add(QuoteIdentifier(Convert.ToString(row["column_name"])));
                }
                defs.Add("    PRIMARY KEY (" + string.Join(", ", keyColumns.ToArray()) + ")");
            }
            sb.AppendLine(string.Join("," + Environment.NewLine, defs.ToArray()));
            sb.AppendLine(");");
            sb.AppendLine();
            return sb.ToString();
        }

        public static bool IsNullableValue(object value)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            if (value == null || value == DBNull.Value)
            {
                return true;
            }
            string text = Convert.ToString(value);
            return text == "1" || string.Equals(text, "YES", StringComparison.OrdinalIgnoreCase)
                || string.Equals(text, "True", StringComparison.OrdinalIgnoreCase);
        }
    }
}
