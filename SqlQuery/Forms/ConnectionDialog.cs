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

        public ConnectionDialog(ConnectionInfo existing)
            : this()
        {
            LoadFrom(existing);
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

        private void LoadFrom(ConnectionInfo info)
        {
            loading = true;
            try
            {
                txtName.Text = info.Name;
                cmbServerType.SelectedIndex = (int)info.ServerType;
                txtServer.Text = info.Server;
                chkWindowsAuth.Checked = info.WindowsAuth;
                txtUser.Text = info.UserName;
                txtPassword.Text = info.Password;
                txtDatabase.Text = info.Database;
                chkEncrypt.Checked = info.Encrypt;
                txtFilePath.Text = info.FilePath;
                txtHost.Text = info.Server;
                numPort.Value = info.Port <= 0 ? 5432 : info.Port;
                txtPgDatabase.Text = info.Database;
                txtPgUser.Text = info.UserName;
                txtPgPassword.Text = info.Password;
                UpdateProviderPanels();
                UpdatePreview();
            }
            finally
            {
                loading = false;
            }
        }

        private ConnectionInfo BuildFromUi()
        {
            ConnectionInfo info = new ConnectionInfo();
            info.Name = txtName.Text.Trim();
            info.ServerType = (ServerType)cmbServerType.SelectedIndex;
            switch (info.ServerType)
            {
                case ServerType.SqlServer:
                    info.Server = txtServer.Text.Trim();
                    info.WindowsAuth = chkWindowsAuth.Checked;
                    info.UserName = txtUser.Text.Trim();
                    info.Password = txtPassword.Text;
                    info.Database = txtDatabase.Text.Trim();
                    info.Encrypt = chkEncrypt.Checked;
                    if (info.Name.Length == 0)
                    {
                        info.Name = info.Server + "/" + info.Database;
                    }
                    break;
                case ServerType.Sqlite:
                    info.FilePath = txtFilePath.Text.Trim();
                    if (info.Name.Length == 0)
                    {
                        info.Name = Path.GetFileName(info.FilePath);
                    }
                    break;
                case ServerType.PostgreSql:
                    info.Server = txtHost.Text.Trim();
                    info.Port = (int)numPort.Value;
                    info.UserName = txtPgUser.Text.Trim();
                    info.Password = txtPgPassword.Text;
                    info.Database = txtPgDatabase.Text.Trim();
                    if (info.Name.Length == 0)
                    {
                        info.Name = info.Server + "/" + info.Database;
                    }
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
            UpdateProviderPanels();
            UpdatePreview();
        }

        private void OnAnySettingChanged(object sender, EventArgs e)
        {
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
                DbProviderBase testProvider = DbProviderBase.Create(BuildFromUi());
                testProvider.TestConnection();
                MessageBox.Show(this, "Connection successful.", "Test Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Connection Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ConnectionInfo info = BuildFromUi();
            if (info.Name.Length == 0)
            {
                MessageBox.Show(this, "Enter a connection name before saving.", "Save Connection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<ConnectionInfo> all = ConnectionStore.Load();
            ConnectionInfo existing = null;
            foreach (ConnectionInfo candidate in all)
            {
                if (string.Equals(candidate.Name, info.Name, StringComparison.OrdinalIgnoreCase))
                {
                    existing = candidate;
                    break;
                }
            }
            if (existing != null)
            {
                DialogResult choice = MessageBox.Show(this,
                    "A connection named \"" + info.Name + "\" already exists. Overwrite it?",
                    "Save Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (choice != DialogResult.Yes)
                {
                    return;
                }
                all.Remove(existing);
            }
            all.Add(info);
            ConnectionStore.Save(all);
            MessageBox.Show(this, "Connection \"" + info.Name + "\" saved.", "Save Connection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ConnectionInfo info = BuildFromUi();
            string error = ValidateInfo(info);
            if (error != null)
            {
                MessageBox.Show(this, error, "SqlQuery", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    break;
            }
            return null;
        }
    }
}
