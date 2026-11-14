using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SqlQuery.Db;

namespace SqlQuery.Forms
{
    public partial class ConnectionDialog : Form
    {
        private bool loading;

        public ConnectionDialog()
        {
            InitializeComponent();
        }

        public ConnectionInfo Connection { get; private set; }

        private void ConnectionDialog_Load(object sender, EventArgs e)
        {
            if (!loading)
            {
                cmbServerType.SelectedIndex = 0;
                UpdateProviderPanels();
                UpdatePreview();
            }
        }

        private ConnectionInfo BuildFromUi()
        {
            ConnectionInfo info = new ConnectionInfo();
            info.ServerType = (ServerType)cmbServerType.SelectedIndex;
            switch (info.ServerType)
            {
                case ServerType.SqlServer:
                    info.Server = txtServer.Text.Trim();
                    info.WindowsAuth = chkWindowsAuth.Checked;
                    info.UserName = txtUser.Text.Trim();
                    info.Password = txtPassword.Text;
                    info.Database = cmbDatabase.Text.Trim();
                    info.Encrypt = chkEncrypt.Checked;
                    info.Name = info.Server + "/" + info.Database;
                    break;
                case ServerType.Sqlite:
                    info.FilePath = txtFilePath.Text.Trim();
                    info.Name = Path.GetFileName(info.FilePath);
                    break;
                case ServerType.PostgreSql:
                    info.Server = txtHost.Text.Trim();
                    info.Port = (int)numPort.Value;
                    info.UserName = txtPgUser.Text.Trim();
                    info.Password = txtPgPassword.Text;
                    info.Database = cmbPgDatabase.Text.Trim();
                    info.Name = info.Server + "/" + info.Database;
                    break;
                case ServerType.MySql:
                    info.Server = txtMySqlHost.Text.Trim();
                    info.Port = (int)numMySqlPort.Value;
                    info.UserName = txtMySqlUser.Text.Trim();
                    info.Password = txtMySqlPassword.Text;
                    info.Database = cmbMySqlDatabase.Text.Trim();
                    info.Name = info.Server + "/" + info.Database;
                    break;
            }
            return info;
        }

        private void UpdateProviderPanels()
        {
            ServerType type = (ServerType)cmbServerType.SelectedIndex;
            grpSqlServer.Visible = type == ServerType.SqlServer;
            grpSqlite.Visible = type == ServerType.Sqlite;
            grpPostgres.Visible = type == ServerType.PostgreSql;
            grpMySql.Visible = type == ServerType.MySql;
        }

        private void UpdatePreview()
        {
            if (loading)
            {
                return;
            }
            try
            {
                txtConnString.Text = BuildFromUi().BuildConnectionString();
            }
            catch (Exception ex)
            {
                txtConnString.Text = "(" + ex.Message + ")";
            }
        }

        private void cmbServerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading)
            {
                return;
            }
            ServerType type = (ServerType)cmbServerType.SelectedIndex;
            if (type == ServerType.PostgreSql)
            {
                numPort.Value = 5432;
            }
            else if (type == ServerType.MySql)
            {
                numMySqlPort.Value = 3306;
            }
            UpdateProviderPanels();
            UpdatePreview();
        }

        private void OnAnySettingChanged(object sender, EventArgs e)
        {
            txtUser.Enabled = !chkWindowsAuth.Checked;
            txtPassword.Enabled = !chkWindowsAuth.Checked;
            UpdatePreview();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "SQLite databases (*.db;*.sqlite;*.db3;*.sqlite3)|*.db;*.sqlite;*.db3;*.sqlite3|All files (*.*)|*.*";
                dialog.CheckFileExists = false;
                dialog.Title = "Select SQLite database file";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFilePath.Text = dialog.FileName;
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                ConnectionInfo info = BuildFromUi();
                DbProviderBase testProvider = DbProviderBase.Create(info);
                testProvider.TestConnection();
                List<string> databases = testProvider.GetDatabases();
                FillDatabaseCombo(cmbDatabase, databases, info.Database);
                FillDatabaseCombo(cmbPgDatabase, databases, info.Database);
                FillDatabaseCombo(cmbMySqlDatabase, databases, info.Database);
                string message = databases.Count > 0
                    ? "Connection successful. " + databases.Count + " database(s) loaded - pick one below."
                    : "Connection successful.";
                MessageBox.Show(this, message, "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private static void FillDatabaseCombo(ComboBox combo, List<string> databases, string selected)
        {
            combo.Items.Clear();
            foreach (string database in databases)
            {
                combo.Items.Add(database);
            }
            int index = -1;
            for (int i = 0; i < databases.Count; i++)
            {
                if (string.Equals(databases[i], selected, StringComparison.OrdinalIgnoreCase))
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                combo.SelectedIndex = index;
            }
            else if (combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ConnectionInfo info = BuildFromUi();
            string error = ValidateInfo(info);
            if (error != null)
            {
                MessageBox.Show(this, error, "Connect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }
            Connection = info;
        }

        private static string ValidateInfo(ConnectionInfo info)
        {
            switch (info.ServerType)
            {
                case ServerType.SqlServer:
                    if (info.Server.Length == 0)
                    {
                        return "Enter a server name.";
                    }
                    if (info.Database.Length == 0)
                    {
                        return "Click \"Test Connection\" and select a database.";
                    }
                    break;
                case ServerType.Sqlite:
                    if (info.FilePath.Length == 0)
                    {
                        return "Choose a SQLite database file.";
                    }
                    break;
                case ServerType.PostgreSql:
                    if (info.Server.Length == 0)
                    {
                        return "Enter a host name.";
                    }
                    if (info.Database.Length == 0)
                    {
                        return "Click \"Test Connection\" and select a database.";
                    }
                    break;
                case ServerType.MySql:
                    if (info.Server.Length == 0)
                    {
                        return "Enter a host name.";
                    }
                    if (info.Database.Length == 0)
                    {
                        return "Click \"Test Connection\" and select a database.";
                    }
                    break;
            }
            return null;
        }
    }
}
