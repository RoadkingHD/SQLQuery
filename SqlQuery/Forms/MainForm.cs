using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlQuery.Db;
using SqlQuery.Editing;

namespace SqlQuery.Forms
{
    public partial class MainForm : Form
    {
        private readonly List<ConnectionInfo> connections = new List<ConnectionInfo>();
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
            LoadConnectionList();
            SetStatus("Not connected. Click \"Connect...\" to choose a database.");
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

        private void LoadConnectionList()
        {
            connections.Clear();
            connections.AddRange(ConnectionStore.Load());
            ComboBox combo = cmbConnections.ComboBox;
            combo.DataSource = null;
            combo.DataSource = connections;
            combo.DisplayMember = "Name";
        }

        private void SetStatus(string text)
        {
            lblStatus.Text = text;
        }

        private ConnectionInfo GetSelectedConnection()
        {
            return cmbConnections.SelectedItem as ConnectionInfo;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectionInfo selected = GetSelectedConnection();
            if (selected == null)
            {
                using (ConnectionDialog dialog = new ConnectionDialog())
                {
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                    LoadConnectionList();
                    selected = dialog.Connection;
                }
            }
            Connect(selected);
        }

        private void btnNewConnection_Click(object sender, EventArgs e)
        {
            using (ConnectionDialog dialog = new ConnectionDialog())
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                LoadConnectionList();
                Connect(dialog.Connection);
            }
        }

        private void btnEditConnection_Click(object sender, EventArgs e)
        {
            ConnectionInfo selected = GetSelectedConnection();
            if (selected == null)
            {
                MessageBox.Show(this, "Select a saved connection first, or create a new one.",
                    "SqlQuery", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string name = selected.Name;
            using (ConnectionDialog dialog = new ConnectionDialog(selected))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
                LoadConnectionList();
                foreach (ConnectionInfo item in connections)
                {
                    if (string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
                    {
                        cmbConnections.SelectedItem = item;
                        break;
                    }
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
                MessageBox.Show(this, ex.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                TreeNode tablesNode = new TreeNode("Tables");
                TreeNode viewsNode = new TreeNode("Views");
                foreach (DataRow row in provider.GetTables().Rows)
                {
                    TableRef tableRef = new TableRef();
                    tableRef.Schema = Convert.ToString(row["table_schema"]);
                    tableRef.Name = Convert.ToString(row["table_name"]);
                    tableRef.Type = Convert.ToString(row["table_type"]);
                    TreeNode node = new TreeNode(tableRef.FullName);
                    node.Tag = tableRef;
                    node.Nodes.Add(NewPlaceholder(tableRef, "Columns"));
                    node.Nodes.Add(NewPlaceholder(tableRef, "Primary Key"));
                    node.Nodes.Add(NewPlaceholder(tableRef, "Indexes"));
                    if (tableRef.Type == "VIEW")
                    {
                        viewsNode.Nodes.Add(node);
                    }
                    else
                    {
                        tablesNode.Nodes.Add(node);
                    }
                }
                if (viewsNode.Nodes.Count > 0)
                {
                    root.Nodes.Add(viewsNode);
                }
                root.Nodes.Add(tablesNode);
                tvSchema.Nodes.Add(root);
                tvSchema.ExpandAll();
            }
            finally
            {
                tvSchema.EndUpdate();
            }
        }

        private static TreeNode NewPlaceholder(TableRef tableRef, string kind)
        {
            TreeNode node = new TreeNode(kind);
            node.Name = kind;
            node.Tag = tableRef;
            node.Nodes.Add(new TreeNode("..."));
            return node;
        }

        private void tvSchema_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            TableRef tableRef = node.Tag as TableRef;
            if (tableRef == null)
            {
                return;
            }
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "...")
            {
                try
                {
                    if (node.Name == "Columns")
                    {
                        FillColumnNodes(node, tableRef);
                    }
                    else if (node.Name == "Primary Key")
                    {
                        FillPrimaryKeys(node, tableRef);
                    }
                    else if (node.Name == "Indexes")
                    {
                        FillIndexes(node, tableRef);
                    }
                    else
                    {
                        node.Nodes.Clear();
                    }
                }
                catch (Exception ex)
                {
                    node.Nodes.Clear();
                    node.Nodes.Add(new TreeNode("Error: " + ex.Message));
                }
            }
        }

        private void FillColumnNodes(TreeNode node, TableRef tableRef)
        {
            DataTable columns = provider.GetColumns(tableRef.Schema, tableRef.Name);
            node.Nodes.Clear();
            List<string> names = new List<string>();
            foreach (DataRow row in columns.Rows)
            {
                string name = Convert.ToString(row["column_name"]);
                names.Add(name);
                string type = Convert.ToString(row["data_type"]);
                string flags = DbProviderBase.IsNullableValue(row["is_nullable"]) ? "NULL" : "NOT NULL";
                node.Nodes.Add(new TreeNode(name + "  " + type + "  (" + flags + ")"));
            }
            editor.Completion.AddTableColumns(tableRef.FullName, names);
        }

        private void FillPrimaryKeys(TreeNode node, TableRef tableRef)
        {
            DataTable keys = provider.GetPrimaryKeys(tableRef.Schema, tableRef.Name);
            node.Nodes.Clear();
            if (keys.Rows.Count == 0)
            {
                node.Nodes.Add(new TreeNode("(none)"));
                return;
            }
            foreach (DataRow row in keys.Rows)
            {
                node.Nodes.Add(new TreeNode(Convert.ToString(row["column_name"])));
            }
        }

        private void FillIndexes(TreeNode node, TableRef tableRef)
        {
            DataTable indexes = provider.GetIndexes(tableRef.Schema, tableRef.Name);
            node.Nodes.Clear();
            if (indexes.Rows.Count == 0)
            {
                node.Nodes.Add(new TreeNode("(none)"));
                return;
            }
            foreach (DataRow row in indexes.Rows)
            {
                string label = Convert.ToString(row["index_name"]);
                if (indexes.Columns.Contains("columns"))
                {
                    string columns = Convert.ToString(row["columns"]);
                    if (columns.Length > 0)
                    {
                        label += "  (" + columns + ")";
                    }
                }
                node.Nodes.Add(new TreeNode(label));
            }
        }

        private void tvSchema_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TableRef tableRef = e.Node.Tag as TableRef;
            if (tableRef == null)
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
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                messages.AppendLine("Error: " + ex.Message);
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
            if (tableRef == null)
            {
                return;
            }
            editor.Text = provider.BuildSelectTop(tableRef, 1000);
            tabMain.SelectedTab = tpQuery;
            editor.Focus();
        }

        private void mnuDumpClass_Click(object sender, EventArgs e)
        {
            if (provider == null || tvSchema.SelectedNode == null)
            {
                return;
            }
            TableRef tableRef = tvSchema.SelectedNode.Tag as TableRef;
            if (tableRef == null)
            {
                return;
            }
            try
            {
                string code = provider.BuildCSharpClass(tableRef);
                using (CodeDialog dialog = new CodeDialog())
                {
                    dialog.DialogTitle = "C# class - " + tableRef.FullName;
                    dialog.CodeText = code;
                    dialog.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
