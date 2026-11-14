using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace SqlQuery.Db
{
    public class SqlServerProvider : DbProviderBase
    {
        public SqlServerProvider(ConnectionInfo info)
            : base(info)
        {
        }

        public override string DisplayName
        {
            get { return "SQL Server"; }
        }

        public override DbConnection CreateConnection()
        {
            return new SqlConnection(Info.BuildConnectionString());
        }

        public override string QuoteIdentifier(string name)
        {
            return "[" + name.Replace("]", "]]") + "]";
        }

        public override DataTable GetTables()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT s.name AS table_schema, t.name AS table_name, 'TABLE' AS table_type " +
                    "FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id " +
                    "UNION ALL " +
                    "SELECT s.name, v.name, 'VIEW' " +
                    "FROM sys.views v JOIN sys.schemas s ON s.schema_id = v.schema_id " +
                    "ORDER BY table_schema, table_name", null);
            });
        }

        public override DataTable GetColumns(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@table", QuoteFullName(schema, table));
                return Fill(connection,
                    "SELECT c.name AS column_name, ty.name AS data_type, " +
                    "CAST(c.is_nullable AS bit) AS is_nullable, " +
                    "ISNULL(dc.definition, '') AS default_value, " +
                    "CAST(c.is_identity AS bit) AS is_identity, " +
                    "c.column_id AS ordinal " +
                    "FROM sys.columns c " +
                    "JOIN sys.types ty ON ty.user_type_id = c.user_type_id " +
                    "LEFT JOIN sys.default_constraints dc ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id " +
                    "WHERE c.object_id = OBJECT_ID(@table) " +
                    "ORDER BY c.column_id", parameters);
            });
        }

        public override DataTable GetPrimaryKeys(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@table", QuoteFullName(schema, table));
                return Fill(connection,
                    "SELECT c.name AS column_name, ic.key_ordinal AS ordinal " +
                    "FROM sys.indexes i " +
                    "JOIN sys.index_columns ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id " +
                    "JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id " +
                    "WHERE i.is_primary_key = 1 AND i.object_id = OBJECT_ID(@table) " +
                    "ORDER BY ic.key_ordinal", parameters);
            });
        }

        public override DataTable GetIndexes(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@table", QuoteFullName(schema, table));
                return Fill(connection,
                    "SELECT i.name AS index_name, CAST(i.is_unique AS bit) AS is_unique, " +
                    "i.type_desc AS index_type, " +
                    "STUFF((SELECT ', ' + c.name FROM sys.index_columns ic " +
                    "JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id " +
                    "WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id " +
                    "ORDER BY ic.key_ordinal FOR XML PATH('')), 1, 2, '') AS columns " +
                    "FROM sys.indexes i " +
                    "WHERE i.object_id = OBJECT_ID(@table) AND i.name IS NOT NULL " +
                    "ORDER BY i.name", parameters);
            });
        }

        public override DataTable GetTableRights()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT USER_NAME(p.grantee_principal_id) AS grantee, " +
                    "p.permission_name AS permission, " +
                    "p.state_desc AS state, " +
                    "s.name + '.' + o.name AS object_name, " +
                    "'' AS column_name " +
                    "FROM sys.database_permissions p " +
                    "JOIN sys.objects o ON o.object_id = p.major_id " +
                    "JOIN sys.schemas s ON s.schema_id = o.schema_id " +
                    "WHERE p.class = 1 AND p.minor_id = 0 AND o.type IN ('U','V') " +
                    "ORDER BY object_name, grantee, permission", null);
            });
        }

        public override DataTable GetColumnRights()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT USER_NAME(p.grantee_principal_id) AS grantee, " +
                    "p.permission_name AS permission, " +
                    "p.state_desc AS state, " +
                    "s.name + '.' + o.name + '.' + c.name AS object_name, " +
                    "c.name AS column_name " +
                    "FROM sys.database_permissions p " +
                    "JOIN sys.objects o ON o.object_id = p.major_id " +
                    "JOIN sys.schemas s ON s.schema_id = o.schema_id " +
                    "JOIN sys.columns c ON c.object_id = p.major_id AND c.column_id = p.minor_id " +
                    "WHERE p.class = 1 AND p.minor_id > 0 AND o.type IN ('U','V') " +
                    "ORDER BY object_name, grantee, permission", null);
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

        public override string GetClrType(string dataType)
        {
            switch (dataType.ToLowerInvariant())
            {
                case "bigint":
                    return "long";
                case "binary":
                case "image":
                case "rowversion":
                case "timestamp":
                case "varbinary":
                    return "byte[]";
                case "bit":
                    return "bool";
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                case "xml":
                    return "string";
                case "date":
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                    return "DateTime";
                case "datetimeoffset":
                    return "DateTimeOffset";
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                    return "decimal";
                case "float":
                    return "double";
                case "int":
                    return "int";
                case "real":
                    return "float";
                case "smallint":
                    return "short";
                case "time":
                    return "TimeSpan";
                case "tinyint":
                    return "byte";
                case "uniqueidentifier":
                    return "Guid";
                default:
                    return "object";
            }
        }

        public override string BuildSelectTop(TableRef table, int limit)
        {
            return "SELECT TOP " + limit + " * FROM " + QuoteFullName(table.Schema, table.Name);
        }
    }
}
