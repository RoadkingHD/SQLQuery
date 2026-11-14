using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlQuery.Editing
{
    public static class SqlSyntaxHighlighter
    {
        public static readonly Color CommentColor = Color.FromArgb(0, 128, 0);
        public static readonly Color KeywordColor = Color.Blue;
        public static readonly Color StringColor = Color.FromArgb(163, 21, 21);
        public static readonly Color NumberColor = Color.FromArgb(0, 128, 128);

        private const int WM_SETREDRAW = 0x000B;

        private static readonly Regex TokenRegex = new Regex(
            @"(?<comment>--[^\r\n]*|/\*.*?\*/)" +
            @"|(?<string>'(''|[^'])*')" +
            @"|(?<word>[A-Za-z_@#][A-Za-z0-9_@#$]*)" +
            @"|(?<number>\d+(\.\d+)?)",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        public static readonly HashSet<string> Keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        static SqlSyntaxHighlighter()
        {
            string[] words = new string[]
            {
                "SELECT","FROM","WHERE","INSERT","INTO","VALUES","UPDATE","SET","DELETE","CREATE","ALTER","DROP",
                "TABLE","VIEW","INDEX","DATABASE","SCHEMA","JOIN","INNER","LEFT","RIGHT","FULL","OUTER","CROSS",
                "ON","AS","AND","OR","NOT","NULL","IS","IN","EXISTS","BETWEEN","LIKE","ILIKE","UNION","ALL",
                "DISTINCT","TOP","LIMIT","OFFSET","FETCH","FIRST","NEXT","ONLY","CASE","WHEN","THEN","ELSE","END",
                "DECLARE","BEGIN","IF","ELSEIF","WHILE","RETURN","BREAK","CONTINUE","EXEC","EXECUTE","USE","GO",
                "GRANT","REVOKE","DENY","WITH","RECURSIVE","PRIMARY","FOREIGN","KEY","REFERENCES","CONSTRAINT",
                "UNIQUE","CHECK","DEFAULT","IDENTITY","AUTOINCREMENT","CASCADE","RESTRICT","ACTION","NO","MATCH",
                "TRANSACTION","COMMIT","ROLLBACK","SAVEPOINT","WORK","ISOLATION","LEVEL","READ","WRITE",
                "SERIALIZABLE","COMMITTED","UNCOMMITTED","REPEATABLE","TRUNCATE","MERGE","MATCHED","SOURCE",
                "TARGET","OUTPUT","PIVOT","UNPIVOT","OVER","PARTITION","ROWS","RANGE","PRECEDING","FOLLOWING",
                "CURRENT","UNBOUNDED","ASC","DESC","COLLATE","CAST","CONVERT","COALESCE","NULLIF","ISNULL",
                "IFNULL","NVL","ANALYZE","EXPLAIN","VACUUM","PRAGMA","ATTACH","DETACH","RETURNS","LANGUAGE",
                "FUNCTION","PROCEDURE","TRIGGER","SEQUENCE","NEXTVAL","CURRVAL","CURSOR","OPEN","CLOSE",
                "DEALLOCATE","INTO","VALUES","USING","GROUP","BY","HAVING","ORDER","ADD","COLUMN","RENAME","TO",
                "MODIFY","COMMENT","ON","DELETE","CASCADE","TEMP","TEMPORARY","IF","CLUSTER","CONFLICT","DO",
                "NOTHING","RETURNING","WINDOW","FILTER","WITHIN","NATURAL","ANY","SOME","INTERSECT","EXCEPT",
                "MINUS","APPLY","LOCK","UNLOCK","ENABLE","DISABLE","VALIDATE","OWNER","TO","AUTHORIZATION",
                "INT","INTEGER","BIGINT","SMALLINT","TINYINT","MEDIUMINT","BIT","BOOLEAN","BOOL","REAL","FLOAT",
                "DOUBLE","PRECISION","DECIMAL","NUMERIC","CHAR","CHARACTER","VARCHAR","NVARCHAR","NCHAR",
                "VARCHAR2","TEXT","NTEXT","CLOB","BLOB","IMAGE","BINARY","VARBINARY","DATE","TIME","DATETIME",
                "DATETIME2","SMALLDATETIME","TIMESTAMP","TIMESTAMPTZ","INTERVAL","YEAR","MONTH","DAY","HOUR",
                "MINUTE","SECOND","ZONE","SERIAL","BIGSERIAL","UUID","JSON","JSONB","XML","MONEY",
                "UNIQUEIDENTIFIER","ROWVERSION","BYTEA","ENUM","ARRAY"
            };
            foreach (string word in words)
            {
                Keywords.Add(word);
            }
        }

        public static bool IsKeyword(string word)
        {
            return Keywords.Contains(word);
        }

        public static void Apply(RichTextBox box)
        {
            if (box.TextLength == 0)
            {
                return;
            }
            int selectionStart = box.SelectionStart;
            int selectionLength = box.SelectionLength;
            string text = box.Text;
            SendMessage(box.Handle, WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
            try
            {
                foreach (Match match in TokenRegex.Matches(text))
                {
                    Color color = GetColor(match);
                    if (color.IsEmpty)
                    {
                        continue;
                    }
                    box.SelectionStart = match.Index;
                    box.SelectionLength = match.Length;
                    box.SelectionColor = color;
                }
                box.SelectionStart = selectionStart;
                box.SelectionLength = selectionLength;
                if (selectionLength == 0)
                {
                    box.SelectionColor = box.ForeColor;
                }
            }
            finally
            {
                SendMessage(box.Handle, WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                box.Invalidate();
            }
        }

        private static Color GetColor(Match match)
        {
            if (match.Groups["comment"].Success)
            {
                return CommentColor;
            }
            if (match.Groups["string"].Success)
            {
                return StringColor;
            }
            if (match.Groups["number"].Success)
            {
                return NumberColor;
            }
            if (match.Groups["word"].Success && Keywords.Contains(match.Value))
            {
                return KeywordColor;
            }
            return Color.Empty;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
    }
}
