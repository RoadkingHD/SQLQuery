using System;
using System.Collections.Generic;

namespace SqlQuery.Editing
{
    public class SqlCodeCompletion
    {
        private readonly List<string> tables = new List<string>();
        private readonly Dictionary<string, List<string>> columnsByTable =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> functions;

        public SqlCodeCompletion()
        {
            functions = new List<string>();
            functions.Add("COUNT(");
            functions.Add("SUM(");
            functions.Add("AVG(");
            functions.Add("MIN(");
            functions.Add("MAX(");
            functions.Add("COALESCE(");
            functions.Add("NULLIF(");
            functions.Add("CAST(");
            functions.Add("CONVERT(");
            functions.Add("SUBSTRING(");
            functions.Add("UPPER(");
            functions.Add("LOWER(");
            functions.Add("LEN(");
            functions.Add("LENGTH(");
            functions.Add("TRIM(");
            functions.Add("REPLACE(");
            functions.Add("CONCAT(");
            functions.Add("ROUND(");
            functions.Add("ABS(");
            functions.Add("GETDATE()");
            functions.Add("NOW()");
            functions.Add("CURRENT_TIMESTAMP");
            functions.Add("ROW_NUMBER() OVER (");
            functions.Add("RANK() OVER (");
            functions.Add("DENSE_RANK() OVER (");
        }

        public void SetTables(IEnumerable<string> tableNames)
        {
            tables.Clear();
            tables.AddRange(tableNames);
        }

        public void AddTableColumns(string tableName, IEnumerable<string> columnNames)
        {
            columnsByTable[tableName] = new List<string>(columnNames);
        }

        public void ClearSchema()
        {
            tables.Clear();
            columnsByTable.Clear();
        }

        public List<string> GetCompletions(string prefix, int max)
        {
            List<string> result = new List<string>();
            HashSet<string> used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Collect(tables, prefix, used, result, max);
            foreach (List<string> columns in columnsByTable.Values)
            {
                Collect(columns, prefix, used, result, max);
            }
            Collect(functions, prefix, used, result, max);
            Collect(SqlSyntaxHighlighter.Keywords, prefix, used, result, max);
            return result;
        }

        public List<string> GetAll(int max)
        {
            List<string> result = new List<string>();
            result.AddRange(tables);
            foreach (List<string> columns in columnsByTable.Values)
            {
                result.AddRange(columns);
            }
            result.AddRange(functions);
            result.AddRange(SqlSyntaxHighlighter.Keywords);
            if (result.Count > max)
            {
                return result.GetRange(0, max);
            }
            return result;
        }

        private static void Collect(IEnumerable<string> source, string prefix, HashSet<string> used, List<string> result, int max)
        {
            foreach (string item in source)
            {
                if (result.Count >= max)
                {
                    return;
                }
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                if (!item.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (!used.Add(item))
                {
                    continue;
                }
                result.Add(item);
            }
        }
    }
}
