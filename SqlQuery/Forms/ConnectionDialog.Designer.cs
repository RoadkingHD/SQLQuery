namespace SqlQuery.Forms
{
    partial class ConnectionDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblServerType = new System.Windows.Forms.Label();
            this.cmbServerType = new System.Windows.Forms.ComboBox();
            this.grpSqlServer = new System.Windows.Forms.GroupBox();
            this.lblServer2 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.chkWindowsAuth = new System.Windows.Forms.CheckBox();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.chkEncrypt = new System.Windows.Forms.CheckBox();
            this.grpSqlite = new System.Windows.Forms.GroupBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.grpPostgres = new System.Windows.Forms.GroupBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.lblPgUser = new System.Windows.Forms.Label();
            this.txtPgUser = new System.Windows.Forms.TextBox();
            this.lblPgPassword = new System.Windows.Forms.Label();
            this.txtPgPassword = new System.Windows.Forms.TextBox();
            this.lblPgDb = new System.Windows.Forms.Label();
            this.cmbPgDatabase = new System.Windows.Forms.ComboBox();
            this.grpMySql = new System.Windows.Forms.GroupBox();
            this.lblMySqlHost = new System.Windows.Forms.Label();
            this.txtMySqlHost = new System.Windows.Forms.TextBox();
            this.lblMySqlPort = new System.Windows.Forms.Label();
            this.numMySqlPort = new System.Windows.Forms.NumericUpDown();
            this.lblMySqlUser = new System.Windows.Forms.Label();
            this.txtMySqlUser = new System.Windows.Forms.TextBox();
            this.lblMySqlPassword = new System.Windows.Forms.Label();
            this.txtMySqlPassword = new System.Windows.Forms.TextBox();
            this.lblMySqlDb = new System.Windows.Forms.Label();
            this.cmbMySqlDatabase = new System.Windows.Forms.ComboBox();
            this.lblConnString = new System.Windows.Forms.Label();
            this.txtConnString = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpSqlServer.SuspendLayout();
            this.grpSqlite.SuspendLayout();
            this.grpPostgres.SuspendLayout();
            this.grpMySql.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMySqlPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServerType
            // 
            this.lblServerType.Location = new System.Drawing.Point(12, 16);
            this.lblServerType.Name = "lblServerType";
            this.lblServerType.Size = new System.Drawing.Size(90, 20);
            this.lblServerType.TabIndex = 0;
            this.lblServerType.Text = "Server type:";
            this.lblServerType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbServerType
            // 
            this.cmbServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServerType.FormattingEnabled = true;
            this.cmbServerType.Items.AddRange(new object[] {
            "SQL Server",
            "SQLite",
            "PostgreSQL",
            "MySQL"});
            this.cmbServerType.Location = new System.Drawing.Point(110, 13);
            this.cmbServerType.Name = "cmbServerType";
            this.cmbServerType.Size = new System.Drawing.Size(200, 21);
            this.cmbServerType.TabIndex = 1;
            this.cmbServerType.SelectedIndexChanged += new System.EventHandler(this.cmbServerType_SelectedIndexChanged);
            // 
            // grpSqlServer
            // 
            this.grpSqlServer.Controls.Add(this.lblServer2);
            this.grpSqlServer.Controls.Add(this.txtServer);
            this.grpSqlServer.Controls.Add(this.chkWindowsAuth);
            this.grpSqlServer.Controls.Add(this.lblUser);
            this.grpSqlServer.Controls.Add(this.txtUser);
            this.grpSqlServer.Controls.Add(this.lblPassword);
            this.grpSqlServer.Controls.Add(this.txtPassword);
            this.grpSqlServer.Controls.Add(this.lblDatabase);
            this.grpSqlServer.Controls.Add(this.cmbDatabase);
            this.grpSqlServer.Controls.Add(this.chkEncrypt);
            this.grpSqlServer.Location = new System.Drawing.Point(12, 44);
            this.grpSqlServer.Name = "grpSqlServer";
            this.grpSqlServer.Size = new System.Drawing.Size(516, 180);
            this.grpSqlServer.TabIndex = 2;
            this.grpSqlServer.TabStop = false;
            this.grpSqlServer.Text = "SQL Server";
            // 
            // lblServer2
            // 
            this.lblServer2.Location = new System.Drawing.Point(10, 28);
            this.lblServer2.Name = "lblServer2";
            this.lblServer2.Size = new System.Drawing.Size(95, 20);
            this.lblServer2.TabIndex = 0;
            this.lblServer2.Text = "Server:";
            this.lblServer2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(110, 25);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(280, 20);
            this.txtServer.TabIndex = 1;
            this.txtServer.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // chkWindowsAuth
            // 
            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Checked = true;
            this.chkWindowsAuth.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWindowsAuth.Location = new System.Drawing.Point(110, 55);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(280, 17);
            this.chkWindowsAuth.TabIndex = 2;
            this.chkWindowsAuth.Text = "Use Windows Authentication (Integrated Security)";
            this.chkWindowsAuth.UseVisualStyleBackColor = true;
            this.chkWindowsAuth.CheckedChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(10, 88);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(95, 20);
            this.lblUser.TabIndex = 3;
            this.lblUser.Text = "User name:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(110, 85);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(180, 20);
            this.txtUser.TabIndex = 4;
            this.txtUser.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(10, 118);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(95, 20);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(110, 115);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(180, 20);
            this.txtPassword.TabIndex = 6;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblDatabase
            // 
            this.lblDatabase.Location = new System.Drawing.Point(10, 148);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(95, 20);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database:";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(110, 145);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(220, 21);
            this.cmbDatabase.TabIndex = 8;
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // chkEncrypt
            // 
            this.chkEncrypt.AutoSize = true;
            this.chkEncrypt.Location = new System.Drawing.Point(350, 148);
            this.chkEncrypt.Name = "chkEncrypt";
            this.chkEncrypt.Size = new System.Drawing.Size(130, 17);
            this.chkEncrypt.TabIndex = 9;
            this.chkEncrypt.Text = "Encrypt connection";
            this.chkEncrypt.UseVisualStyleBackColor = true;
            this.chkEncrypt.CheckedChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // grpSqlite
            // 
            this.grpSqlite.Controls.Add(this.lblFile);
            this.grpSqlite.Controls.Add(this.txtFilePath);
            this.grpSqlite.Controls.Add(this.btnBrowse);
            this.grpSqlite.Location = new System.Drawing.Point(12, 44);
            this.grpSqlite.Name = "grpSqlite";
            this.grpSqlite.Size = new System.Drawing.Size(516, 70);
            this.grpSqlite.TabIndex = 3;
            this.grpSqlite.TabStop = false;
            this.grpSqlite.Text = "SQLite";
            this.grpSqlite.Visible = false;
            // 
            // lblFile
            // 
            this.lblFile.Location = new System.Drawing.Point(10, 30);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(95, 20);
            this.lblFile.TabIndex = 0;
            this.lblFile.Text = "Database file:";
            this.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(110, 27);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(290, 20);
            this.txtFilePath.TabIndex = 1;
            this.txtFilePath.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(410, 25);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(90, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // grpPostgres
            // 
            this.grpPostgres.Controls.Add(this.lblHost);
            this.grpPostgres.Controls.Add(this.txtHost);
            this.grpPostgres.Controls.Add(this.lblPort);
            this.grpPostgres.Controls.Add(this.numPort);
            this.grpPostgres.Controls.Add(this.lblPgUser);
            this.grpPostgres.Controls.Add(this.txtPgUser);
            this.grpPostgres.Controls.Add(this.lblPgPassword);
            this.grpPostgres.Controls.Add(this.txtPgPassword);
            this.grpPostgres.Controls.Add(this.lblPgDb);
            this.grpPostgres.Controls.Add(this.cmbPgDatabase);
            this.grpPostgres.Location = new System.Drawing.Point(12, 44);
            this.grpPostgres.Name = "grpPostgres";
            this.grpPostgres.Size = new System.Drawing.Size(516, 150);
            this.grpPostgres.TabIndex = 4;
            this.grpPostgres.TabStop = false;
            this.grpPostgres.Text = "PostgreSQL";
            this.grpPostgres.Visible = false;
            // 
            // lblHost
            // 
            this.lblHost.Location = new System.Drawing.Point(10, 28);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(95, 20);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(110, 25);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(180, 20);
            this.txtHost.TabIndex = 1;
            this.txtHost.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(300, 28);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(45, 20);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(350, 25);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(80, 20);
            this.numPort.TabIndex = 3;
            this.numPort.Value = new decimal(new int[] {
            5432,
            0,
            0,
            0});
            this.numPort.ValueChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblPgUser
            // 
            this.lblPgUser.Location = new System.Drawing.Point(10, 58);
            this.lblPgUser.Name = "lblPgUser";
            this.lblPgUser.Size = new System.Drawing.Size(95, 20);
            this.lblPgUser.TabIndex = 4;
            this.lblPgUser.Text = "User name:";
            this.lblPgUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPgUser
            // 
            this.txtPgUser.Location = new System.Drawing.Point(110, 55);
            this.txtPgUser.Name = "txtPgUser";
            this.txtPgUser.Size = new System.Drawing.Size(180, 20);
            this.txtPgUser.TabIndex = 5;
            this.txtPgUser.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblPgPassword
            // 
            this.lblPgPassword.Location = new System.Drawing.Point(10, 88);
            this.lblPgPassword.Name = "lblPgPassword";
            this.lblPgPassword.Size = new System.Drawing.Size(95, 20);
            this.lblPgPassword.TabIndex = 6;
            this.lblPgPassword.Text = "Password:";
            this.lblPgPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPgPassword
            // 
            this.txtPgPassword.Location = new System.Drawing.Point(110, 85);
            this.txtPgPassword.Name = "txtPgPassword";
            this.txtPgPassword.Size = new System.Drawing.Size(180, 20);
            this.txtPgPassword.TabIndex = 7;
            this.txtPgPassword.UseSystemPasswordChar = true;
            this.txtPgPassword.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblPgDb
            // 
            this.lblPgDb.Location = new System.Drawing.Point(10, 118);
            this.lblPgDb.Name = "lblPgDb";
            this.lblPgDb.Size = new System.Drawing.Size(95, 20);
            this.lblPgDb.TabIndex = 8;
            this.lblPgDb.Text = "Database:";
            this.lblPgDb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbPgDatabase
            // 
            this.cmbPgDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPgDatabase.FormattingEnabled = true;
            this.cmbPgDatabase.Location = new System.Drawing.Point(110, 115);
            this.cmbPgDatabase.Name = "cmbPgDatabase";
            this.cmbPgDatabase.Size = new System.Drawing.Size(220, 21);
            this.cmbPgDatabase.TabIndex = 9;
            this.cmbPgDatabase.SelectedIndexChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // grpMySql
            // 
            this.grpMySql.Controls.Add(this.lblMySqlHost);
            this.grpMySql.Controls.Add(this.txtMySqlHost);
            this.grpMySql.Controls.Add(this.lblMySqlPort);
            this.grpMySql.Controls.Add(this.numMySqlPort);
            this.grpMySql.Controls.Add(this.lblMySqlUser);
            this.grpMySql.Controls.Add(this.txtMySqlUser);
            this.grpMySql.Controls.Add(this.lblMySqlPassword);
            this.grpMySql.Controls.Add(this.txtMySqlPassword);
            this.grpMySql.Controls.Add(this.lblMySqlDb);
            this.grpMySql.Controls.Add(this.cmbMySqlDatabase);
            this.grpMySql.Location = new System.Drawing.Point(12, 44);
            this.grpMySql.Name = "grpMySql";
            this.grpMySql.Size = new System.Drawing.Size(516, 150);
            this.grpMySql.TabIndex = 5;
            this.grpMySql.TabStop = false;
            this.grpMySql.Text = "MySQL";
            this.grpMySql.Visible = false;
            // 
            // lblMySqlHost
            // 
            this.lblMySqlHost.Location = new System.Drawing.Point(10, 28);
            this.lblMySqlHost.Name = "lblMySqlHost";
            this.lblMySqlHost.Size = new System.Drawing.Size(95, 20);
            this.lblMySqlHost.TabIndex = 0;
            this.lblMySqlHost.Text = "Host:";
            this.lblMySqlHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMySqlHost
            // 
            this.txtMySqlHost.Location = new System.Drawing.Point(110, 25);
            this.txtMySqlHost.Name = "txtMySqlHost";
            this.txtMySqlHost.Size = new System.Drawing.Size(180, 20);
            this.txtMySqlHost.TabIndex = 1;
            this.txtMySqlHost.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblMySqlPort
            // 
            this.lblMySqlPort.Location = new System.Drawing.Point(300, 28);
            this.lblMySqlPort.Name = "lblMySqlPort";
            this.lblMySqlPort.Size = new System.Drawing.Size(45, 20);
            this.lblMySqlPort.TabIndex = 2;
            this.lblMySqlPort.Text = "Port:";
            this.lblMySqlPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numMySqlPort
            // 
            this.numMySqlPort.Location = new System.Drawing.Point(350, 25);
            this.numMySqlPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numMySqlPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMySqlPort.Name = "numMySqlPort";
            this.numMySqlPort.Size = new System.Drawing.Size(80, 20);
            this.numMySqlPort.TabIndex = 3;
            this.numMySqlPort.Value = new decimal(new int[] {
            3306,
            0,
            0,
            0});
            this.numMySqlPort.ValueChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblMySqlUser
            // 
            this.lblMySqlUser.Location = new System.Drawing.Point(10, 58);
            this.lblMySqlUser.Name = "lblMySqlUser";
            this.lblMySqlUser.Size = new System.Drawing.Size(95, 20);
            this.lblMySqlUser.TabIndex = 4;
            this.lblMySqlUser.Text = "User name:";
            this.lblMySqlUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMySqlUser
            // 
            this.txtMySqlUser.Location = new System.Drawing.Point(110, 55);
            this.txtMySqlUser.Name = "txtMySqlUser";
            this.txtMySqlUser.Size = new System.Drawing.Size(180, 20);
            this.txtMySqlUser.TabIndex = 5;
            this.txtMySqlUser.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblMySqlPassword
            // 
            this.lblMySqlPassword.Location = new System.Drawing.Point(10, 88);
            this.lblMySqlPassword.Name = "lblMySqlPassword";
            this.lblMySqlPassword.Size = new System.Drawing.Size(95, 20);
            this.lblMySqlPassword.TabIndex = 6;
            this.lblMySqlPassword.Text = "Password:";
            this.lblMySqlPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMySqlPassword
            // 
            this.txtMySqlPassword.Location = new System.Drawing.Point(110, 85);
            this.txtMySqlPassword.Name = "txtMySqlPassword";
            this.txtMySqlPassword.Size = new System.Drawing.Size(180, 20);
            this.txtMySqlPassword.TabIndex = 7;
            this.txtMySqlPassword.UseSystemPasswordChar = true;
            this.txtMySqlPassword.TextChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblMySqlDb
            // 
            this.lblMySqlDb.Location = new System.Drawing.Point(10, 118);
            this.lblMySqlDb.Name = "lblMySqlDb";
            this.lblMySqlDb.Size = new System.Drawing.Size(95, 20);
            this.lblMySqlDb.TabIndex = 8;
            this.lblMySqlDb.Text = "Database:";
            this.lblMySqlDb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMySqlDatabase
            // 
            this.cmbMySqlDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMySqlDatabase.FormattingEnabled = true;
            this.cmbMySqlDatabase.Location = new System.Drawing.Point(110, 115);
            this.cmbMySqlDatabase.Name = "cmbMySqlDatabase";
            this.cmbMySqlDatabase.Size = new System.Drawing.Size(220, 21);
            this.cmbMySqlDatabase.TabIndex = 9;
            this.cmbMySqlDatabase.SelectedIndexChanged += new System.EventHandler(this.OnAnySettingChanged);
            // 
            // lblConnString
            // 
            this.lblConnString.Location = new System.Drawing.Point(12, 232);
            this.lblConnString.Name = "lblConnString";
            this.lblConnString.Size = new System.Drawing.Size(120, 20);
            this.lblConnString.TabIndex = 6;
            this.lblConnString.Text = "Connection string:";
            // 
            // txtConnString
            // 
            this.txtConnString.BackColor = System.Drawing.Color.White;
            this.txtConnString.Location = new System.Drawing.Point(12, 252);
            this.txtConnString.Multiline = true;
            this.txtConnString.Name = "txtConnString";
            this.txtConnString.ReadOnly = true;
            this.txtConnString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConnString.Size = new System.Drawing.Size(516, 56);
            this.txtConnString.TabIndex = 7;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 320);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(110, 30);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test Connection";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(340, 320);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 30);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "Connect";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(438, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConnectionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(540, 362);
            this.Controls.Add(this.lblServerType);
            this.Controls.Add(this.cmbServerType);
            this.Controls.Add(this.grpSqlServer);
            this.Controls.Add(this.grpSqlite);
            this.Controls.Add(this.grpPostgres);
            this.Controls.Add(this.grpMySql);
            this.Controls.Add(this.lblConnString);
            this.Controls.Add(this.txtConnString);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Connection";
            this.Load += new System.EventHandler(this.ConnectionDialog_Load);
            this.grpSqlServer.ResumeLayout(false);
            this.grpSqlServer.PerformLayout();
            this.grpSqlite.ResumeLayout(false);
            this.grpSqlite.PerformLayout();
            this.grpPostgres.ResumeLayout(false);
            this.grpPostgres.PerformLayout();
            this.grpMySql.ResumeLayout(false);
            this.grpMySql.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMySqlPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblServerType;
        private System.Windows.Forms.ComboBox cmbServerType;
        private System.Windows.Forms.GroupBox grpSqlServer;
        private System.Windows.Forms.Label lblServer2;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.CheckBox chkWindowsAuth;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.CheckBox chkEncrypt;
        private System.Windows.Forms.GroupBox grpSqlite;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.GroupBox grpPostgres;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label lblPgUser;
        private System.Windows.Forms.TextBox txtPgUser;
        private System.Windows.Forms.Label lblPgPassword;
        private System.Windows.Forms.TextBox txtPgPassword;
        private System.Windows.Forms.Label lblPgDb;
        private System.Windows.Forms.ComboBox cmbPgDatabase;
        private System.Windows.Forms.GroupBox grpMySql;
        private System.Windows.Forms.Label lblMySqlHost;
        private System.Windows.Forms.TextBox txtMySqlHost;
        private System.Windows.Forms.Label lblMySqlPort;
        private System.Windows.Forms.NumericUpDown numMySqlPort;
        private System.Windows.Forms.Label lblMySqlUser;
        private System.Windows.Forms.TextBox txtMySqlUser;
        private System.Windows.Forms.Label lblMySqlPassword;
        private System.Windows.Forms.TextBox txtMySqlPassword;
        private System.Windows.Forms.Label lblMySqlDb;
        private System.Windows.Forms.ComboBox cmbMySqlDatabase;
        private System.Windows.Forms.Label lblConnString;
        private System.Windows.Forms.TextBox txtConnString;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}
