using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Npgsql;

namespace SqlQuery.Db
{
    public class PostgresProvider : DbProviderBase
    {
        public PostgresProvider(ConnectionInfo info)
            : base(info)
        {
        }

        public override string DisplayName
        {
            get { return "PostgreSQL"; }
        }

        public override DbConnection CreateConnection()
        {
            return new NpgsqlConnection(Info.BuildConnectionString());
        }

        public override string QuoteIdentifier(string name)
        {
            return "\"" + name.Replace("\"", "\"\"") + "\"";
        }

        public override DataTable GetTables()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT table_schema, table_name, " +
                    "CASE table_type WHEN 'BASE TABLE' THEN 'TABLE' ELSE 'VIEW' END AS table_type " +
                    "FROM information_schema.tables " +
                    "WHERE table_schema NOT IN ('pg_catalog','information_schema') " +
                    "ORDER BY table_schema, table_name", null);
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
                    "SELECT a.attname AS column_name, " +
                    "format_type(a.atttypid, a.atttypmod) AS data_type, " +
                    "CASE WHEN a.attnotnull THEN 'NO' ELSE 'YES' END AS is_nullable, " +
                    "COALESCE(pg_get_expr(ad.adbin, ad.adrelid), '') AS default_value, " +
                    "'NO' AS is_identity, " +
                    "a.attnum AS ordinal " +
                    "FROM pg_attribute a " +
                    "JOIN pg_class cls ON cls.oid = a.attrelid " +
                    "JOIN pg_namespace ns ON ns.oid = cls.relnamespace " +
                    "LEFT JOIN pg_attrdef ad ON ad.adrelid = a.attrelid AND ad.adnum = a.attnum " +
                    "WHERE ns.nspname = @schema AND cls.relname = @table " +
                    "AND a.attnum > 0 AND NOT a.attisdropped " +
                    "ORDER BY a.attnum", parameters);
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
                    "SELECT kcu.column_name, kcu.ordinal_position AS ordinal " +
                    "FROM information_schema.table_constraints tc " +
                    "JOIN information_schema.key_column_usage kcu " +
                    "ON tc.constraint_name = kcu.constraint_name AND tc.table_schema = kcu.table_schema " +
                    "WHERE tc.constraint_type = 'PRIMARY KEY' AND tc.table_schema = @schema AND tc.table_name = @table " +
                    "ORDER BY kcu.ordinal_position", parameters);
            });
        }

        public override DataTable GetIndexes(string schema, string table)
        {
            return WithConnection(delegate(DbConnection connection)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@schema", schema);
                parameters.Add("@table", table);
                DataTable raw = Fill(connection,
                    "SELECT indexname AS index_name, indexdef AS index_def " +
                    "FROM pg_indexes WHERE schemaname = @schema AND tablename = @table " +
                    "ORDER BY indexname", parameters);
                DataTable result = new DataTable();
                result.Columns.Add("index_name", typeof(string));
                result.Columns.Add("is_unique", typeof(bool));
                result.Columns.Add("columns", typeof(string));
                foreach (DataRow row in raw.Rows)
                {
                    string definition = Convert.ToString(row["index_def"]);
                    result.Rows.Add(
                        Convert.ToString(row["index_name"]),
                        definition.ToUpperInvariant().Contains("UNIQUE"),
                        definition);
                }
                return result;
            });
        }

        public override DataTable GetProcedures()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT n.nspname AS table_schema, p.proname AS table_name " +
                    "FROM pg_proc p JOIN pg_namespace n ON n.oid = p.pronamespace " +
                    "WHERE n.nspname NOT IN ('pg_catalog','information_schema') AND p.prokind = 'f' " +
                    "ORDER BY table_schema, table_name", null);
            });
        }

        public override List<string> GetDatabases()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                List<string> names = new List<string>();
                foreach (DataRow row in Fill(connection,
                    "SELECT datname FROM pg_database WHERE NOT datistemplate AND datallowconn ORDER BY datname", null).Rows)
                {
                    names.Add(Convert.ToString(row["datname"]));
                }
                return names;
            });
        }

        public override DataTable GetTableRights()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT n.nspname || '.' || c.relname AS object_name, " +
                    "perm.permission AS permission, " +
                    "CURRENT_USER AS grantee, " +
                    "CASE WHEN has_table_privilege(quote_ident(n.nspname) || '.' || quote_ident(c.relname), perm.permission) " +
                    "THEN 'GRANTED' ELSE 'DENIED' END AS state " +
                    "FROM pg_class c " +
                    "JOIN pg_namespace n ON n.oid = c.relnamespace " +
                    "CROSS JOIN (VALUES ('SELECT'),('INSERT'),('UPDATE'),('DELETE'),('REFERENCES'),('TRIGGER')) AS perm(permission) " +
                    "WHERE c.relkind IN ('r','v') AND n.nspname NOT IN ('pg_catalog','information_schema') " +
                    "ORDER BY object_name, permission", null);
            });
        }

        public override DataTable GetColumnRights()
        {
            return WithConnection(delegate(DbConnection connection)
            {
                return Fill(connection,
                    "SELECT COALESCE(gr.rolname, 'PUBLIC') AS grantee, " +
                    "pr.type AS permission, " +
                    "n.nspname || '.' || c.relname || '.' || a.attname AS object_name, " +
                    "CASE WHEN pr.grantable THEN 'YES' ELSE 'NO' END AS is_grantable " +
                    "FROM pg_attribute a " +
                    "JOIN pg_class c ON c.oid = a.attrelid " +
                    "JOIN pg_namespace n ON n.oid = c.relnamespace " +
                    "CROSS JOIN LATERAL aclexplode(COALESCE(a.attacl, '{}'::aclitem[])) AS pr " +
                    "LEFT JOIN pg_roles gr ON gr.oid = pr.grantee " +
                    "WHERE n.nspname NOT IN ('pg_catalog','information_schema') AND a.attnum > 0 AND NOT a.attisdropped " +
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
            string type = dataType.ToLowerInvariant();
            int paren = type.IndexOf('(');
            if (paren >= 0)
            {
                type = type.Substring(0, paren);
            }
            switch (type)
            {
                case "smallint":
                case "int2":
                    return "short";
                case "integer":
                case "int":
                case "int4":
                case "serial":
                    return "int";
                case "bigint":
                case "int8":
                case "bigserial":
                    return "long";
                case "boolean":
                case "bool":
                    return "bool";
                case "real":
                case "float4":
                    return "float";
                case "double precision":
                case "float8":
                    return "double";
                case "numeric":
                case "decimal":
                case "money":
                    return "decimal";
                case "character varying":
                case "character":
                case "char":
                case "varchar":
                case "text":
                case "json":
                case "jsonb":
                case "xml":
                    return "string";
                case "date":
                case "timestamp":
                case "timestamp without time zone":
                    return "DateTime";
                case "timestamp with time zone":
                case "timestamptz":
                    return "DateTimeOffset";
                case "time":
                case "time without time zone":
                case "time with time zone":
                case "interval":
                    return "TimeSpan";
                case "uuid":
                    return "Guid";
                case "bytea":
                    return "byte[]";
                default:
                    return "object";
            }
        }

        public override string BuildSelectTop(TableRef table, int limit)
        {
            return "SELECT * FROM " + QuoteFullName(table.Schema, table.Name) + " LIMIT " + limit;
        }
    }
}
