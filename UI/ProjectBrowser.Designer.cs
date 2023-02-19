namespace PBT.DowsingMachine.UI
{
    partial class ProjectBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpGitHub = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiHelpAppData = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvwProjects = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvwReferences = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvwExports = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.cmsExports = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExportSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lvwTests = new System.Windows.Forms.ListView();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.projectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddProject = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.cmsReferences = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiReferencesRead = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReferencesDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReferenceSteps = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReferenceExport = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiProjectOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiReload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProjectRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tssProjectActions = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiProjectVersions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.cmsExports.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmsReferences.SuspendLayout();
            this.cmsProject.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpGitHub,
            this.toolStripSeparator1,
            this.tsmiHelpAppData});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // tsmiHelpGitHub
            // 
            this.tsmiHelpGitHub.Name = "tsmiHelpGitHub";
            this.tsmiHelpGitHub.Size = new System.Drawing.Size(223, 24);
            this.tsmiHelpGitHub.Text = "View in GitHub";
            this.tsmiHelpGitHub.Click += new System.EventHandler(this.tsmiHelpGitHub_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(220, 6);
            // 
            // tsmiHelpAppData
            // 
            this.tsmiHelpAppData.Name = "tsmiHelpAppData";
            this.tsmiHelpAppData.Size = new System.Drawing.Size(223, 24);
            this.tsmiHelpAppData.Text = "Open AppData folder";
            this.tsmiHelpAppData.Click += new System.EventHandler(this.tsmiHelpAppData_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwProjects);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 679);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 2;
            // 
            // lvwProjects
            // 
            this.lvwProjects.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvwProjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwProjects.FullRowSelect = true;
            this.lvwProjects.Location = new System.Drawing.Point(0, 0);
            this.lvwProjects.Name = "lvwProjects";
            this.lvwProjects.Size = new System.Drawing.Size(243, 679);
            this.lvwProjects.SmallImageList = this.imageList1;
            this.lvwProjects.TabIndex = 0;
            this.lvwProjects.UseCompatibleStateImageBehavior = false;
            this.lvwProjects.View = System.Windows.Forms.View.Details;
            this.lvwProjects.ItemActivate += new System.EventHandler(this.lvwProjects_ItemActivate);
            this.lvwProjects.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwProjects_MouseDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Project";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 89;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(1, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(761, 679);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvwReferences);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(753, 646);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "References";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvwReferences
            // 
            this.lvwReferences.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader10,
            this.columnHeader4});
            this.lvwReferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwReferences.FullRowSelect = true;
            this.lvwReferences.Location = new System.Drawing.Point(3, 3);
            this.lvwReferences.Name = "lvwReferences";
            this.lvwReferences.Size = new System.Drawing.Size(747, 640);
            this.lvwReferences.TabIndex = 0;
            this.lvwReferences.UseCompatibleStateImageBehavior = false;
            this.lvwReferences.View = System.Windows.Forms.View.Details;
            this.lvwReferences.ItemActivate += new System.EventHandler(this.lvwReferences_ItemActivate);
            this.lvwReferences.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwReferences_MouseDown);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 300;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Ref";
            this.columnHeader10.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Output type";
            this.columnHeader4.Width = 200;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lvwExports);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(753, 646);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lvwExports
            // 
            this.lvwExports.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvwExports.ContextMenuStrip = this.cmsExports;
            this.lvwExports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwExports.FullRowSelect = true;
            this.lvwExports.Location = new System.Drawing.Point(3, 3);
            this.lvwExports.Name = "lvwExports";
            this.lvwExports.Size = new System.Drawing.Size(747, 640);
            this.lvwExports.TabIndex = 0;
            this.lvwExports.UseCompatibleStateImageBehavior = false;
            this.lvwExports.View = System.Windows.Forms.View.Details;
            this.lvwExports.ItemActivate += new System.EventHandler(this.lvwExports_ItemActivate);
            this.lvwExports.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvwExports_MouseDown);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 300;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Output type";
            this.columnHeader6.Width = 200;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Output path";
            this.columnHeader7.Width = 200;
            // 
            // cmsExports
            // 
            this.cmsExports.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExportSelected});
            this.cmsExports.Name = "cmsOutput";
            this.cmsExports.Size = new System.Drawing.Size(221, 28);
            // 
            // tsmiExportSelected
            // 
            this.tsmiExportSelected.Name = "tsmiExportSelected";
            this.tsmiExportSelected.Size = new System.Drawing.Size(220, 24);
            this.tsmiExportSelected.Text = "Export selected items";
            this.tsmiExportSelected.Click += new System.EventHandler(this.tsmiExportSelected_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lvwTests);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(753, 646);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Test";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lvwTests
            // 
            this.lvwTests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9});
            this.lvwTests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwTests.FullRowSelect = true;
            this.lvwTests.Location = new System.Drawing.Point(3, 3);
            this.lvwTests.Name = "lvwTests";
            this.lvwTests.Size = new System.Drawing.Size(747, 640);
            this.lvwTests.TabIndex = 1;
            this.lvwTests.UseCompatibleStateImageBehavior = false;
            this.lvwTests.View = System.Windows.Forms.View.Details;
            this.lvwTests.ItemActivate += new System.EventHandler(this.lvwTests_ItemActivate);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Name";
            this.columnHeader8.Width = 300;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Output type";
            this.columnHeader9.Width = 200;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pnlActions);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(753, 646);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Actions";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pnlActions
            // 
            this.pnlActions.AutoScroll = true;
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActions.Location = new System.Drawing.Point(3, 3);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(747, 640);
            this.pnlActions.TabIndex = 0;
            // 
            // projectsToolStripMenuItem
            // 
            this.projectsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddProject});
            this.projectsToolStripMenuItem.Name = "projectsToolStripMenuItem";
            this.projectsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.projectsToolStripMenuItem.Text = "&Projects";
            // 
            // tsmiAddProject
            // 
            this.tsmiAddProject.Name = "tsmiAddProject";
            this.tsmiAddProject.Size = new System.Drawing.Size(115, 24);
            this.tsmiAddProject.Text = "&Add...";
            this.tsmiAddProject.Click += new System.EventHandler(this.tsmiAddProject_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 707);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // cmsReferences
            // 
            this.cmsReferences.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiReferencesRead,
            this.tsmiReferencesDebug,
            this.tsmiReferenceSteps,
            this.tsmiReferenceExport});
            this.cmsReferences.Name = "contextMenuStrip1";
            this.cmsReferences.Size = new System.Drawing.Size(181, 122);
            // 
            // tsmiReferencesRead
            // 
            this.tsmiReferencesRead.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tsmiReferencesRead.Name = "tsmiReferencesRead";
            this.tsmiReferencesRead.Size = new System.Drawing.Size(180, 24);
            this.tsmiReferencesRead.Text = "&Read";
            this.tsmiReferencesRead.Click += new System.EventHandler(this.tsmiReferencesRead_Click);
            // 
            // tsmiReferencesDebug
            // 
            this.tsmiReferencesDebug.Name = "tsmiReferencesDebug";
            this.tsmiReferencesDebug.Size = new System.Drawing.Size(180, 24);
            this.tsmiReferencesDebug.Text = "&Debug";
            this.tsmiReferencesDebug.Click += new System.EventHandler(this.tsmiReferencesDebug_Click);
            // 
            // tsmiReferenceSteps
            // 
            this.tsmiReferenceSteps.Name = "tsmiReferenceSteps";
            this.tsmiReferenceSteps.Size = new System.Drawing.Size(180, 24);
            this.tsmiReferenceSteps.Text = "&Steps";
            // 
            // tsmiReferenceExport
            // 
            this.tsmiReferenceExport.Name = "tsmiReferenceExport";
            this.tsmiReferenceExport.Size = new System.Drawing.Size(180, 24);
            this.tsmiReferenceExport.Text = "&Export";
            this.tsmiReferenceExport.Click += new System.EventHandler(this.tsmiReferenceExport_Click);
            // 
            // cmsProject
            // 
            this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProjectOptions,
            this.tsmiReload,
            this.tsmiProjectRemove,
            this.tssProjectActions,
            this.tsmiProjectVersions});
            this.cmsProject.Name = "cmsProject";
            this.cmsProject.Size = new System.Drawing.Size(141, 106);
            // 
            // tsmiProjectOptions
            // 
            this.tsmiProjectOptions.Name = "tsmiProjectOptions";
            this.tsmiProjectOptions.Size = new System.Drawing.Size(140, 24);
            this.tsmiProjectOptions.Text = "&Options...";
            this.tsmiProjectOptions.Click += new System.EventHandler(this.tsmiProjectOptions_Click);
            // 
            // tsmiReload
            // 
            this.tsmiReload.Name = "tsmiReload";
            this.tsmiReload.Size = new System.Drawing.Size(140, 24);
            this.tsmiReload.Text = "&Reload";
            this.tsmiReload.Click += new System.EventHandler(this.tsmiReload_Click);
            // 
            // tsmiProjectRemove
            // 
            this.tsmiProjectRemove.Name = "tsmiProjectRemove";
            this.tsmiProjectRemove.Size = new System.Drawing.Size(140, 24);
            this.tsmiProjectRemove.Text = "&Remove";
            this.tsmiProjectRemove.Click += new System.EventHandler(this.tsmiProjectRemove_Click);
            // 
            // tssProjectActions
            // 
            this.tssProjectActions.Name = "tssProjectActions";
            this.tssProjectActions.Size = new System.Drawing.Size(137, 6);
            // 
            // tsmiProjectVersions
            // 
            this.tsmiProjectVersions.Name = "tsmiProjectVersions";
            this.tsmiProjectVersions.Size = new System.Drawing.Size(140, 24);
            this.tsmiProjectVersions.Text = "Version";
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.projectsToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1008, 28);
            this.menuMain.TabIndex = 4;
            this.menuMain.Text = "menuStrip3";
            // 
            // ProjectBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuMain);
            this.Name = "ProjectBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProjectBrowser";
            this.Load += new System.EventHandler(this.ProjectBrowser_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.cmsExports.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsReferences.ResumeLayout(false);
            this.cmsProject.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private SplitContainer splitContainer1;
        private ListView lvwProjects;
        private ImageList imageList1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ContextMenuStrip cmsReferences;
        private ContextMenuStrip cmsProject;
        private ToolStripMenuItem tsmiProjectVersions;
        private ToolStripMenuItem tsmiProjectOptions;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem tsmiHelpGitHub;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiHelpAppData;
        private ToolStripMenuItem tsmiProjectRemove;
        private ToolStripSeparator tssProjectActions;
        private ListView lvwReferences;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ListView lvwExports;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ColumnHeader columnHeader7;
        private ToolStripMenuItem tsmiReferencesRead;
        private ToolStripMenuItem projectsToolStripMenuItem;
        private ToolStripMenuItem tsmiAddProject;
        private ContextMenuStrip cmsExports;
        private ToolStripMenuItem tsmiExportSelected;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ListView lvwTests;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader9;
        private MenuStrip menuMain;
        private TabPage tabPage4;
        private FlowLayoutPanel flpActions;
        private Panel pnlActions;
        private ColumnHeader columnHeader10;
        private ToolStripMenuItem tsmiReload;
        private ToolStripMenuItem tsmiReferenceExport;
        private ToolStripMenuItem tsmiReferenceSteps;
        private ToolStripMenuItem tsmiReferencesDebug;
    }
}