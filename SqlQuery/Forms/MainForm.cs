using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SqlQuery.Db;
using SqlQuery.Editing;

namespace SqlQuery.Forms
{
    public partial class MainForm : Form
    {
        private const int IconTable = 0;
        private const int IconView = 1;
        private const int IconProcedure = 2;

        private DbProviderBase provider;
        private ConnectionInfo currentConnection;
        private bool tablesLoaded;
        private bool rightsLoaded;
        private bool schemaLoaded;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tvSchema.ImageList = CreateSchemaImageList();
            SetStatus("Not connected. Click \"New Connection...\" to choose a database.");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F5 && provider != null)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ExecuteQuery();
            }
        }

        private static ImageList CreateSchemaImageList()
        {
            ImageList images = new ImageList();
            images.ImageSize = new Size(16, 16);
            images.ColorDepth = ColorDepth.Depth32Bit;
            images.Images.Add(DrawObjectIcon(Color.FromArgb(0, 102, 204), "T"));
            images.Images.Add(DrawObjectIcon(Color.FromArgb(0, 153, 51), "V"));
            images.Images.Add(DrawObjectIcon(Color.FromArgb(153, 51, 204), "P"));
            return images;
        }

        private static Bitmap DrawObjectIcon(Color color, string letter)
        {
            Bitmap bitmap = new Bitmap(16, 16);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, 1, 2, 14, 12);
                }
                using (Font font = new Font(FontFamily.GenericSansSerif, 7f, FontStyle.Bold))
                {
                    using (StringFormat format = new StringFormat())
                    {
                        format.Alignment = StringAlignment.Center;
                        format.LineAlignment = StringAlignment.Center;
                        using (SolidBrush textBrush = new SolidBrush(Color.White))
                        {
                            RectangleF bounds = new RectangleF(1f, 2f, 14f, 12f);
                            graphics.DrawString(letter, font, textBrush, bounds, format);
                        }
                    }
                }
            }
            return bitmap;
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private void btnNewConnection_Click(object sender, EventArgs e)
        {
            using (ConnectionDialog dialog = new ConnectionDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                Connect(dialog.Connection);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            provider = null;
            currentConnection = null;
            tvSchema.Nodes.Clear();
            editor.Completion.ClearSchema();
            gridResults.DataSource = null;
            gridTables.DataSource = null;
            gridColumns.DataSource = null;
            gridRights.DataSource = null;
            gridColumnRights.DataSource = null;
            txtSchema.Clear();
            txtCode.Clear();
            tablesLoaded = false;
            rightsLoaded = false;
            schemaLoaded = false;
            Text = "SqlQuery";
            SetStatus("Disconnected.");
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ExecuteQuery();
        }

        private void btnRefreshSchema_Click(object sender, EventArgs e)
        {
            if (provider == null)
            {
                MessageBox.Show(this, "Not connected.", "SqlQuery", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                BuildSchemaTree();
                editor.Completion.SetTables(provider.GetCompletionTables());
                tablesLoaded = false;
                rightsLoaded = false;
                schemaLoaded = false;
                SetStatus("Schema refreshed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopyCode_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtCode.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not copy to clipboard: " + ex.Message,
                    "SqlQuery", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Connect(ConnectionInfo info)
        {
            try
            {
                SetStatus("Connecting to \"" + info.Name + "\"...");
                Cursor = Cursors.WaitCursor;
                provider = DbProviderBase.Create(info);
                provider.TestConnection();
                currentConnection = info;
                tablesLoaded = false;
                rightsLoaded = false;
                schemaLoaded = false;
                BuildSchemaTree();
                LoadCompletionColumns();
                editor.Completion.SetTables(provider.GetCompletionTables());
                Text = "SqlQuery - " + info.Name + " (" + provider.DisplayName + ")";
                SetStatus("Connected to \"" + info.Name + "\" (" + provider.DisplayName + ").");
            }
            catch (Exception ex)
            {
                provider = null;
                currentConnection = null;
                tvSchema.Nodes.Clear();
                editor.Completion.ClearSchema();
                MessageBox.Show(this, ex.ToString(), "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("Connection failed.");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BuildSchemaTree()
        {
            tvSchema.BeginUpdate();
            try
            {
                tvSchema.Nodes.Clear();
                TreeNode root = new TreeNode(currentConnection.Name + "  [" + provider.DisplayName + "]");
                root.ImageIndex = -1;
                root.SelectedImageIndex = -1;

                TreeNode tablesNode = new TreeNode("Tables");
                tablesNode.ImageIndex = -1;
                tablesNode.SelectedImageIndex = -1;
                TreeNode viewsNode = new TreeNode("Views");
                viewsNode.ImageIndex = -1;
                viewsNode.SelectedImageIndex = -1;
                TreeNode proceduresNode = new TreeNode("Stored Procedures");
                proceduresNode.ImageIndex = -1;
                proceduresNode.SelectedImageIndex = -1;

                foreach (DataRow row in provider.GetTables().Rows)
                {
                    TableRef tableRef = new TableRef();
                    tableRef.Schema = Convert.ToString(row["table_schema"]);
                    tableRef.Name = Convert.ToString(row["table_name"]);
                    tableRef.Type = Convert.ToString(row["table_type"]);
                    bool isView = tableRef.Type == "VIEW";
                    TreeNode node = new TreeNode(tableRef.FullName, isView ? IconView : IconTable, isView ? IconView : IconTable);
                    node.Tag = tableRef;
                    if (isView)
                    {
                        viewsNode.Nodes.Add(node);
                    }
                    else
                    {
                        tablesNode.Nodes.Add(node);
                    }
                }

                foreach (DataRow row in provider.GetProcedures().Rows)
                {
                    TableRef tableRef = new TableRef();
                    tableRef.Schema = Convert.ToString(row["table_schema"]);
                    tableRef.Name = Convert.ToString(row["table_name"]);
                    tableRef.Type = "PROCEDURE";
                    TreeNode node = new TreeNode(tableRef.FullName, IconProcedure, IconProcedure);
                    node.Tag = tableRef;
                    proceduresNode.Nodes.Add(node);
                }

                if (tablesNode.Nodes.Count > 0)
                {
                    root.Nodes.Add(tablesNode);
                }
                if (viewsNode.Nodes.Count > 0)
                {
                    root.Nodes.Add(viewsNode);
                }
                if (proceduresNode.Nodes.Count > 0)
                {
                    root.Nodes.Add(proceduresNode);
                }
                tvSchema.Nodes.Add(root);
                tvSchema.ExpandAll();
            }
            finally
            {
                tvSchema.EndUpdate();
            }
        }

        private void LoadCompletionColumns()
        {
            foreach (DataRow row in provider.GetTables().Rows)
            {
                string schema = Convert.ToString(row["table_schema"]);
                string name = Convert.ToString(row["table_name"]);
                string type = Convert.ToString(row["table_type"]);
                if (type == "VIEW")
                {
                    continue;
                }
                try
                {
                    DataTable columns = provider.GetColumns(schema, name);
                    List<string> names = new List<string>();
                    foreach (DataRow columnRow in columns.Rows)
                    {
                        names.Add(Convert.ToString(columnRow["column_name"]));
                    }
                    TableRef tableRef = new TableRef();
                    tableRef.Schema = schema;
                    tableRef.Name = name;
                    tableRef.Type = type;
                    editor.Completion.AddTableColumns(tableRef.FullName, names);
                }
                catch (Exception)
                {
                    // Column metadata is optional for completion; ignore per-table failures.
                }
            }
        }

        private void cmsTable_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeNode node = tvSchema.GetNodeAt(tvSchema.PointToClient(Cursor.Position));
            if (node == null || !(node.Tag is TableRef))
            {
                e.Cancel = true;
                return;
            }
            tvSchema.SelectedNode = node;
        }

        private void tvSchema_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TableRef tableRef = e.Node.Tag as TableRef;
            if (tableRef == null || tableRef.Type == "PROCEDURE")
            {
                return;
            }
            tabMain.SelectedTab = tpColumns;
            LoadColumns(tableRef);
        }

        private void LoadColumns(TableRef tableRef)
        {
            try
            {
                DataTable columns = provider.GetColumns(tableRef.Schema, tableRef.Name);
                gridColumns.DataSource = columns;
                List<string> names = new List<string>();
                foreach (DataRow row in columns.Rows)
                {
                    names.Add(Convert.ToString(row["column_name"]));
                }
                editor.Completion.AddTableColumns(tableRef.FullName, names);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (provider == null)
            {
                return;
            }
            try
            {
                if (tabMain.SelectedTab == tpTables && !tablesLoaded)
                {
                    gridTables.DataSource = provider.GetTables();
                    tablesLoaded = true;
                }
                else if (tabMain.SelectedTab == tpRights && !rightsLoaded)
                {
                    gridRights.DataSource = provider.GetTableRights();
                    gridColumnRights.DataSource = provider.GetColumnRights();
                    rightsLoaded = true;
                }
                else if (tabMain.SelectedTab == tpSchema && !schemaLoaded)
                {
                    txtSchema.Text = provider.GetSchemaDdl();
                    schemaLoaded = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteQuery()
        {
            if (provider == null)
            {
                MessageBox.Show(this, "Connect to a database first.", "SqlQuery",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string sql = editor.Text.Trim();
            if (sql.Length == 0)
            {
                return;
            }
            UseWaitCursor = true;
            SetStatus("Executing query...");
            StringBuilder messages = new StringBuilder();
            Stopwatch stopwatch = Stopwatch.StartNew();
            DataTable result = null;
            int statements = 0;
            try
            {
                using (System.Data.Common.DbConnection connection = provider.CreateConnection())
                {
                    connection.Open();
                    using (System.Data.Common.DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandTimeout = 300;
                        using (System.Data.Common.DbDataReader reader = command.ExecuteReader())
                        {
                            while (true)
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                statements++;
                                messages.AppendLine("Statement " + statements + ": " + table.Rows.Count + " row(s) returned.");
                                if (table.Columns.Count > 0)
                                {
                                    result = table;
                                }
                                if (!reader.NextResult())
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                gridResults.DataSource = result;
                lblRows.Text = result == null ? string.Empty : result.Rows.Count + " row(s)";
                if (result != null)
                {
                    tabResults.SelectedTab = tpData;
                }
                messages.AppendLine("Total time: " + stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
                txtMessages.Text = messages.ToString();
                SetStatus("Done in " + stopwatch.ElapsedMilliseconds + " ms. " + statements + " statement(s).");
            }
            catch (Exception ex)
            {
                messages.AppendLine("Error: " + ex.ToString());
                txtMessages.Text = messages.ToString();
                tabResults.SelectedTab = tpMessages;
                SetStatus("Query failed.");
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private void mnuSelectTop_Click(object sender, EventArgs e)
        {
            if (provider == null || tvSchema.SelectedNode == null)
            {
                return;
            }
            TableRef tableRef = tvSchema.SelectedNode.Tag as TableRef;
            if (tableRef == null || tableRef.Type == "PROCEDURE")
            {
                return;
            }
            tabMain.SelectedTab = tpQuery;
            editor.Clear();
            editor.AppendText(provider.BuildSelectTop(tableRef, 1000));
            editor.Focus();
        }

        private void mnuDumpClass_Click(object sender, EventArgs e)
        {
            if (provider == null || tvSchema.SelectedNode == null)
            {
                return;
            }
            TableRef tableRef = tvSchema.SelectedNode.Tag as TableRef;
            if (tableRef == null || tableRef.Type == "PROCEDURE")
            {
                return;
            }
            try
            {
                txtCode.Text = provider.BuildCSharpClass(tableRef);
                tabMain.SelectedTab = tpCode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
