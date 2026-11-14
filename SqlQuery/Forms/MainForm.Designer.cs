namespace SqlQuery.Forms
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.btnNewConnection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.btnRefreshSchema = new System.Windows.Forms.ToolStripButton();
            this.btnDisconnect = new System.Windows.Forms.ToolStripButton();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tvSchema = new System.Windows.Forms.TreeView();
            this.cmsTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSelectTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDumpClass = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tpQuery = new System.Windows.Forms.TabPage();
            this.splitEditor = new System.Windows.Forms.SplitContainer();
            this.editor = new SqlQuery.Editing.SqlEditor();
            this.tabResults = new System.Windows.Forms.TabControl();
            this.tpData = new System.Windows.Forms.TabPage();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.tpMessages = new System.Windows.Forms.TabPage();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.tpTables = new System.Windows.Forms.TabPage();
            this.gridTables = new System.Windows.Forms.DataGridView();
            this.tpColumns = new System.Windows.Forms.TabPage();
            this.gridColumns = new System.Windows.Forms.DataGridView();
            this.tpDefinition = new System.Windows.Forms.TabPage();
            this.txtDefinition = new System.Windows.Forms.RichTextBox();
            this.tpRights = new System.Windows.Forms.TabPage();
            this.splitRights = new System.Windows.Forms.SplitContainer();
            this.lblTableRights = new System.Windows.Forms.Label();
            this.gridRights = new System.Windows.Forms.DataGridView();
            this.lblColumnRights = new System.Windows.Forms.Label();
            this.gridColumnRights = new System.Windows.Forms.DataGridView();
            this.tpSchema = new System.Windows.Forms.TabPage();
            this.txtSchema = new System.Windows.Forms.TextBox();
            this.tpCode = new System.Windows.Forms.TabPage();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.panelCodeTop = new System.Windows.Forms.Panel();
            this.btnCopyCode = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRows = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.cmsTable.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tpQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitEditor)).BeginInit();
            this.splitEditor.Panel1.SuspendLayout();
            this.splitEditor.Panel2.SuspendLayout();
            this.splitEditor.SuspendLayout();
            this.tabResults.SuspendLayout();
            this.tpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.tpMessages.SuspendLayout();
            this.tpTables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
            this.tpColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
            this.tpDefinition.SuspendLayout();
            this.tpRights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRights)).BeginInit();
            this.splitRights.Panel1.SuspendLayout();
            this.splitRights.Panel2.SuspendLayout();
            this.splitRights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnRights)).BeginInit();
            this.tpSchema.SuspendLayout();
            this.tpCode.SuspendLayout();
            this.panelCodeTop.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStripMain
            //
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewConnection,
            this.toolStripSeparator1,
            this.btnExecute,
            this.btnRefreshSchema,
            this.btnDisconnect});
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(984, 25);
            //
            // btnNewConnection
            //
            this.btnNewConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNewConnection.Name = "btnNewConnection";
            this.btnNewConnection.Size = new System.Drawing.Size(107, 22);
            this.btnNewConnection.Text = "New Connection...";
            this.btnNewConnection.Click += new System.EventHandler(this.btnNewConnection_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            //
            // btnExecute
            //
            this.btnExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(83, 22);
            this.btnExecute.Text = "Execute (F5)";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            //
            // btnRefreshSchema
            //
            this.btnRefreshSchema.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefreshSchema.Name = "btnRefreshSchema";
            this.btnRefreshSchema.Size = new System.Drawing.Size(99, 22);
            this.btnRefreshSchema.Text = "Refresh Schema";
            this.btnRefreshSchema.Click += new System.EventHandler(this.btnRefreshSchema_Click);
            //
            // btnDisconnect
            //
            this.btnDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(71, 22);
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            //
            // splitMain
            //
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 25);
            this.splitMain.Name = "splitMain";
            this.splitMain.Panel1.Controls.Add(this.tvSchema);
            this.splitMain.Panel2.Controls.Add(this.tabMain);
            this.splitMain.Size = new System.Drawing.Size(984, 514);
            this.splitMain.SplitterDistance = 260;
            this.splitMain.TabIndex = 0;
            //
            // tvSchema
            //
            this.tvSchema.ContextMenuStrip = this.cmsTable;
            this.tvSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSchema.HideSelection = false;
            this.tvSchema.Location = new System.Drawing.Point(0, 0);
            this.tvSchema.Name = "tvSchema";
            this.tvSchema.Size = new System.Drawing.Size(260, 514);
            this.tvSchema.TabIndex = 0;
            this.tvSchema.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSchema_AfterSelect);
            //
            // cmsTable
            //
            this.cmsTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSelectTop,
            this.mnuDumpClass});
            this.cmsTable.Name = "cmsTable";
            this.cmsTable.Size = new System.Drawing.Size(181, 48);
            this.cmsTable.Opening += new System.ComponentModel.CancelEventHandler(this.cmsTable_Opening);
            //
            // mnuSelectTop
            //
            this.mnuSelectTop.Name = "mnuSelectTop";
            this.mnuSelectTop.Size = new System.Drawing.Size(180, 22);
            this.mnuSelectTop.Text = "Select Top 1000 Rows";
            this.mnuSelectTop.Click += new System.EventHandler(this.mnuSelectTop_Click);
            //
            // mnuDumpClass
            //
            this.mnuDumpClass.Name = "mnuDumpClass";
            this.mnuDumpClass.Size = new System.Drawing.Size(180, 22);
            this.mnuDumpClass.Text = "Dump as C# Class";
            this.mnuDumpClass.Click += new System.EventHandler(this.mnuDumpClass_Click);
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tpQuery);
            this.tabMain.Controls.Add(this.tpTables);
            this.tabMain.Controls.Add(this.tpColumns);
            this.tabMain.Controls.Add(this.tpDefinition);
            this.tabMain.Controls.Add(this.tpRights);
            this.tabMain.Controls.Add(this.tpSchema);
            this.tabMain.Controls.Add(this.tpCode);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(720, 514);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            //
            // tpQuery
            //
            this.tpQuery.Controls.Add(this.splitEditor);
            this.tpQuery.Location = new System.Drawing.Point(4, 22);
            this.tpQuery.Name = "tpQuery";
            this.tpQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tpQuery.Size = new System.Drawing.Size(712, 488);
            this.tpQuery.Text = "Query";
            this.tpQuery.UseVisualStyleBackColor = true;
            //
            // splitEditor
            //
            this.splitEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitEditor.Location = new System.Drawing.Point(3, 3);
            this.splitEditor.Name = "splitEditor";
            this.splitEditor.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitEditor.Panel1.Controls.Add(this.editor);
            this.splitEditor.Panel2.Controls.Add(this.tabResults);
            this.splitEditor.Size = new System.Drawing.Size(706, 482);
            this.splitEditor.SplitterDistance = 220;
            this.splitEditor.TabIndex = 0;
            //
            // editor
            //
            this.editor.AcceptsTab = true;
            this.editor.DetectUrls = false;
            this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editor.HideSelection = false;
            this.editor.Location = new System.Drawing.Point(0, 0);
            this.editor.Name = "editor";
            this.editor.Size = new System.Drawing.Size(706, 220);
            this.editor.TabIndex = 0;
            this.editor.Text = "";
            this.editor.WordWrap = false;
            //
            // tabResults
            //
            this.tabResults.Controls.Add(this.tpData);
            this.tabResults.Controls.Add(this.tpMessages);
            this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabResults.Location = new System.Drawing.Point(0, 0);
            this.tabResults.Name = "tabResults";
            this.tabResults.SelectedIndex = 0;
            this.tabResults.Size = new System.Drawing.Size(706, 258);
            this.tabResults.TabIndex = 0;
            //
            // tpData
            //
            this.tpData.Controls.Add(this.gridResults);
            this.tpData.Location = new System.Drawing.Point(4, 22);
            this.tpData.Name = "tpData";
            this.tpData.Padding = new System.Windows.Forms.Padding(3);
            this.tpData.Size = new System.Drawing.Size(698, 232);
            this.tpData.Text = "Results";
            this.tpData.UseVisualStyleBackColor = true;
            //
            // gridResults
            //
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.AllowUserToOrderColumns = true;
            this.gridResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResults.Location = new System.Drawing.Point(3, 3);
            this.gridResults.Name = "gridResults";
            this.gridResults.ReadOnly = true;
            this.gridResults.Size = new System.Drawing.Size(692, 226);
            this.gridResults.TabIndex = 0;
            //
            // tpMessages
            //
            this.tpMessages.Controls.Add(this.txtMessages);
            this.tpMessages.Location = new System.Drawing.Point(4, 22);
            this.tpMessages.Name = "tpMessages";
            this.tpMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tpMessages.Size = new System.Drawing.Size(698, 232);
            this.tpMessages.Text = "Messages";
            this.tpMessages.UseVisualStyleBackColor = true;
            //
            // txtMessages
            //
            this.txtMessages.BackColor = System.Drawing.Color.White;
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(3, 3);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessages.Size = new System.Drawing.Size(692, 226);
            this.txtMessages.TabIndex = 0;
            //
            // tpTables
            //
            this.tpTables.Controls.Add(this.gridTables);
            this.tpTables.Location = new System.Drawing.Point(4, 22);
            this.tpTables.Name = "tpTables";
            this.tpTables.Padding = new System.Windows.Forms.Padding(3);
            this.tpTables.Size = new System.Drawing.Size(712, 488);
            this.tpTables.Text = "Tables";
            this.tpTables.UseVisualStyleBackColor = true;
            //
            // gridTables
            //
            this.gridTables.AllowUserToAddRows = false;
            this.gridTables.AllowUserToDeleteRows = false;
            this.gridTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTables.Location = new System.Drawing.Point(3, 3);
            this.gridTables.Name = "gridTables";
            this.gridTables.ReadOnly = true;
            this.gridTables.Size = new System.Drawing.Size(706, 482);
            this.gridTables.TabIndex = 0;
            //
            // tpColumns
            //
            this.tpColumns.Controls.Add(this.gridColumns);
            this.tpColumns.Location = new System.Drawing.Point(4, 22);
            this.tpColumns.Name = "tpColumns";
            this.tpColumns.Padding = new System.Windows.Forms.Padding(3);
            this.tpColumns.Size = new System.Drawing.Size(712, 488);
            this.tpColumns.Text = "Columns";
            this.tpColumns.UseVisualStyleBackColor = true;
            //
            // gridColumns
            //
            this.gridColumns.AllowUserToAddRows = false;
            this.gridColumns.AllowUserToDeleteRows = false;
            this.gridColumns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridColumns.Location = new System.Drawing.Point(3, 3);
            this.gridColumns.Name = "gridColumns";
            this.gridColumns.ReadOnly = true;
            this.gridColumns.Size = new System.Drawing.Size(706, 482);
            this.gridColumns.TabIndex = 0;
            // 
            // tpDefinition
            // 
            this.tpDefinition.Controls.Add(this.txtDefinition);
            this.tpDefinition.Location = new System.Drawing.Point(4, 22);
            this.tpDefinition.Name = "tpDefinition";
            this.tpDefinition.Padding = new System.Windows.Forms.Padding(3);
            this.tpDefinition.Size = new System.Drawing.Size(712, 488);
            this.tpDefinition.Text = "Definition";
            this.tpDefinition.UseVisualStyleBackColor = true;
            // 
            // txtDefinition
            // 
            this.txtDefinition.BackColor = System.Drawing.Color.White;
            this.txtDefinition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDefinition.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtDefinition.Location = new System.Drawing.Point(3, 3);
            this.txtDefinition.Multiline = true;
            this.txtDefinition.Name = "txtDefinition";
            this.txtDefinition.ReadOnly = true;
            this.txtDefinition.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both;
            this.txtDefinition.Size = new System.Drawing.Size(706, 482);
            this.txtDefinition.TabIndex = 0;
            this.txtDefinition.WordWrap = false;
            //
            // tpRights
            //
            this.tpRights.Controls.Add(this.splitRights);
            this.tpRights.Location = new System.Drawing.Point(4, 22);
            this.tpRights.Name = "tpRights";
            this.tpRights.Padding = new System.Windows.Forms.Padding(3);
            this.tpRights.Size = new System.Drawing.Size(712, 488);
            this.tpRights.Text = "Rights";
            this.tpRights.UseVisualStyleBackColor = true;
            //
            // splitRights
            //
            this.splitRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitRights.Location = new System.Drawing.Point(3, 3);
            this.splitRights.Name = "splitRights";
            this.splitRights.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitRights.Panel1.Controls.Add(this.gridRights);
            this.splitRights.Panel1.Controls.Add(this.lblTableRights);
            this.splitRights.Panel2.Controls.Add(this.gridColumnRights);
            this.splitRights.Panel2.Controls.Add(this.lblColumnRights);
            this.splitRights.Size = new System.Drawing.Size(706, 482);
            this.splitRights.SplitterDistance = 200;
            this.splitRights.TabIndex = 0;
            //
            // lblTableRights
            //
            this.lblTableRights.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTableRights.Location = new System.Drawing.Point(0, 0);
            this.lblTableRights.Name = "lblTableRights";
            this.lblTableRights.Padding = new System.Windows.Forms.Padding(2, 4, 0, 2);
            this.lblTableRights.Size = new System.Drawing.Size(706, 22);
            this.lblTableRights.Text = "Table-level rights";
            //
            // gridRights
            //
            this.gridRights.AllowUserToAddRows = false;
            this.gridRights.AllowUserToDeleteRows = false;
            this.gridRights.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridRights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRights.Location = new System.Drawing.Point(0, 22);
            this.gridRights.Name = "gridRights";
            this.gridRights.ReadOnly = true;
            this.gridRights.Size = new System.Drawing.Size(706, 178);
            this.gridRights.TabIndex = 0;
            //
            // lblColumnRights
            //
            this.lblColumnRights.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblColumnRights.Location = new System.Drawing.Point(0, 0);
            this.lblColumnRights.Name = "lblColumnRights";
            this.lblColumnRights.Padding = new System.Windows.Forms.Padding(2, 4, 0, 2);
            this.lblColumnRights.Size = new System.Drawing.Size(706, 22);
            this.lblColumnRights.Text = "Column-level (field) rights";
            //
            // gridColumnRights
            //
            this.gridColumnRights.AllowUserToAddRows = false;
            this.gridColumnRights.AllowUserToDeleteRows = false;
            this.gridColumnRights.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridColumnRights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridColumnRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridColumnRights.Location = new System.Drawing.Point(0, 22);
            this.gridColumnRights.Name = "gridColumnRights";
            this.gridColumnRights.ReadOnly = true;
            this.gridColumnRights.Size = new System.Drawing.Size(706, 256);
            this.gridColumnRights.TabIndex = 0;
            //
            // tpSchema
            //
            this.tpSchema.Controls.Add(this.txtSchema);
            this.tpSchema.Location = new System.Drawing.Point(4, 22);
            this.tpSchema.Name = "tpSchema";
            this.tpSchema.Padding = new System.Windows.Forms.Padding(3);
            this.tpSchema.Size = new System.Drawing.Size(712, 488);
            this.tpSchema.Text = "Schema DDL";
            this.tpSchema.UseVisualStyleBackColor = true;
            //
            // txtSchema
            //
            this.txtSchema.BackColor = System.Drawing.Color.White;
            this.txtSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSchema.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtSchema.Location = new System.Drawing.Point(3, 3);
            this.txtSchema.Multiline = true;
            this.txtSchema.Name = "txtSchema";
            this.txtSchema.ReadOnly = true;
            this.txtSchema.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSchema.Size = new System.Drawing.Size(706, 482);
            this.txtSchema.TabIndex = 0;
            this.txtSchema.WordWrap = false;
            //
            // tpCode
            //
            this.tpCode.Controls.Add(this.txtCode);
            this.tpCode.Controls.Add(this.panelCodeTop);
            this.tpCode.Location = new System.Drawing.Point(4, 22);
            this.tpCode.Name = "tpCode";
            this.tpCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpCode.Size = new System.Drawing.Size(712, 488);
            this.tpCode.Text = "C# code";
            this.tpCode.UseVisualStyleBackColor = true;
            //
            // txtCode
            //
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCode.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtCode.Location = new System.Drawing.Point(3, 3);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(706, 452);
            this.txtCode.TabIndex = 0;
            this.txtCode.WordWrap = false;
            //
            // panelCodeTop
            //
            this.panelCodeTop.Controls.Add(this.btnCopyCode);
            this.panelCodeTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCodeTop.Location = new System.Drawing.Point(3, 3);
            this.panelCodeTop.Name = "panelCodeTop";
            this.panelCodeTop.Size = new System.Drawing.Size(706, 36);
            this.panelCodeTop.TabIndex = 1;
            //
            // btnCopyCode
            //
            this.btnCopyCode.Location = new System.Drawing.Point(4, 5);
            this.btnCopyCode.Name = "btnCopyCode";
            this.btnCopyCode.Size = new System.Drawing.Size(140, 26);
            this.btnCopyCode.TabIndex = 0;
            this.btnCopyCode.Text = "Copy to Clipboard";
            this.btnCopyCode.UseVisualStyleBackColor = true;
            this.btnCopyCode.Click += new System.EventHandler(this.btnCopyCode_Click);
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblRows});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(900, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblRows
            //
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(69, 17);
            this.lblRows.Text = "";
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStripMain);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SqlQuery";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.cmsTable.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tpQuery.ResumeLayout(false);
            this.splitEditor.Panel1.ResumeLayout(false);
            this.splitEditor.Panel1.PerformLayout();
            this.splitEditor.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitEditor)).EndInit();
            this.splitEditor.ResumeLayout(false);
            this.tabResults.ResumeLayout(false);
            this.tpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.tpMessages.ResumeLayout(false);
            this.tpMessages.PerformLayout();
            this.tpTables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
            this.tpColumns.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
            this.tpDefinition.ResumeLayout(false);
            this.tpDefinition.PerformLayout();
            this.tpRights.ResumeLayout(false);
            this.splitRights.Panel1.ResumeLayout(false);
            this.splitRights.Panel1.PerformLayout();
            this.splitRights.Panel2.ResumeLayout(false);
            this.splitRights.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitRights)).EndInit();
            this.splitRights.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnRights)).EndInit();
            this.tpSchema.ResumeLayout(false);
            this.tpSchema.PerformLayout();
            this.tpCode.ResumeLayout(false);
            this.tpCode.PerformLayout();
            this.panelCodeTop.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton btnNewConnection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private System.Windows.Forms.ToolStripButton btnRefreshSchema;
        private System.Windows.Forms.ToolStripButton btnDisconnect;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.TreeView tvSchema;
        private System.Windows.Forms.ContextMenuStrip cmsTable;
        private System.Windows.Forms.ToolStripMenuItem mnuSelectTop;
        private System.Windows.Forms.ToolStripMenuItem mnuDumpClass;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpQuery;
        private System.Windows.Forms.SplitContainer splitEditor;
        private SqlQuery.Editing.SqlEditor editor;
        private System.Windows.Forms.TabControl tabResults;
        private System.Windows.Forms.TabPage tpData;
        private System.Windows.Forms.DataGridView gridResults;
        private System.Windows.Forms.TabPage tpMessages;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.TabPage tpTables;
        private System.Windows.Forms.DataGridView gridTables;
        private System.Windows.Forms.TabPage tpColumns;
        private System.Windows.Forms.DataGridView gridColumns;
        private System.Windows.Forms.TabPage tpDefinition;
        private System.Windows.Forms.RichTextBox txtDefinition;
        private System.Windows.Forms.TabPage tpRights;
        private System.Windows.Forms.SplitContainer splitRights;
        private System.Windows.Forms.Label lblTableRights;
        private System.Windows.Forms.DataGridView gridRights;
        private System.Windows.Forms.Label lblColumnRights;
        private System.Windows.Forms.DataGridView gridColumnRights;
        private System.Windows.Forms.TabPage tpSchema;
        private System.Windows.Forms.TextBox txtSchema;
        private System.Windows.Forms.TabPage tpCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Panel panelCodeTop;
        private System.Windows.Forms.Button btnCopyCode;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRows;
    }
}
