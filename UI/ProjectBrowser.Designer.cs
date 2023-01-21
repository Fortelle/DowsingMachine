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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.projectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddProject = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelpGitHub = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiHelpAppData = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvwProjects = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lstExtract = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstDump = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstTest = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstData = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiProjectOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiProjectRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tssProjectActions = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiProjectVersions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmsProject.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(833, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(833, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
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
            this.splitContainer1.Location = new System.Drawing.Point(0, 53);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvwProjects);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(833, 491);
            this.splitContainer1.SplitterDistance = 201;
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
            this.lvwProjects.Size = new System.Drawing.Size(201, 491);
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
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(1, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(628, 491);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lstExtract);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(160, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(151, 485);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Extract";
            // 
            // lstExtract
            // 
            this.lstExtract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstExtract.FormattingEnabled = true;
            this.lstExtract.ItemHeight = 20;
            this.lstExtract.Location = new System.Drawing.Point(3, 21);
            this.lstExtract.Name = "lstExtract";
            this.lstExtract.Size = new System.Drawing.Size(145, 461);
            this.lstExtract.TabIndex = 0;
            this.lstExtract.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstExtract_MouseDoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstDump);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(317, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(151, 485);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dump";
            // 
            // lstDump
            // 
            this.lstDump.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDump.FormattingEnabled = true;
            this.lstDump.ItemHeight = 20;
            this.lstDump.Location = new System.Drawing.Point(3, 21);
            this.lstDump.Name = "lstDump";
            this.lstDump.Size = new System.Drawing.Size(145, 461);
            this.lstDump.TabIndex = 0;
            this.lstDump.DoubleClick += new System.EventHandler(this.lstDump_DoubleClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstTest);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(474, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(151, 485);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Test";
            // 
            // lstTest
            // 
            this.lstTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTest.FormattingEnabled = true;
            this.lstTest.ItemHeight = 20;
            this.lstTest.Location = new System.Drawing.Point(3, 21);
            this.lstTest.Name = "lstTest";
            this.lstTest.Size = new System.Drawing.Size(145, 461);
            this.lstTest.TabIndex = 0;
            this.lstTest.DoubleClick += new System.EventHandler(this.lstTest_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstData);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 485);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Browse";
            // 
            // lstData
            // 
            this.lstData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstData.FormattingEnabled = true;
            this.lstData.ItemHeight = 20;
            this.lstData.Location = new System.Drawing.Point(3, 21);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(145, 461);
            this.lstData.TabIndex = 0;
            this.lstData.DoubleClick += new System.EventHandler(this.lstData_DoubleClick);
            this.lstData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstData_MouseDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(833, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // cmsProject
            // 
            this.cmsProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiProjectOptions,
            this.tsmiProjectRemove,
            this.tssProjectActions,
            this.tsmiProjectVersions});
            this.cmsProject.Name = "cmsProject";
            this.cmsProject.Size = new System.Drawing.Size(141, 82);
            // 
            // tsmiProjectOptions
            // 
            this.tsmiProjectOptions.Name = "tsmiProjectOptions";
            this.tsmiProjectOptions.Size = new System.Drawing.Size(180, 24);
            this.tsmiProjectOptions.Text = "&Options...";
            this.tsmiProjectOptions.Click += new System.EventHandler(this.tsmiProjectOptions_Click);
            // 
            // tsmiProjectRemove
            // 
            this.tsmiProjectRemove.Name = "tsmiProjectRemove";
            this.tsmiProjectRemove.Size = new System.Drawing.Size(180, 24);
            this.tsmiProjectRemove.Text = "&Remove";
            this.tsmiProjectRemove.Click += new System.EventHandler(this.tsmiProjectRemove_Click);
            // 
            // tssProjectActions
            // 
            this.tssProjectActions.Name = "tssProjectActions";
            this.tssProjectActions.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiProjectVersions
            // 
            this.tsmiProjectVersions.Name = "tsmiProjectVersions";
            this.tsmiProjectVersions.Size = new System.Drawing.Size(180, 24);
            this.tsmiProjectVersions.Text = "Version";
            // 
            // ProjectBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 566);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ProjectBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProjectBrowser";
            this.Load += new System.EventHandler(this.ProjectBrowser_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsProject.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ToolStrip toolStrip1;
        private MenuStrip menuStrip1;
        private SplitContainer splitContainer1;
        private ListView lvwProjects;
        private ImageList imageList1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox2;
        private ListBox lstDump;
        private GroupBox groupBox3;
        private ListBox lstTest;
        private GroupBox groupBox1;
        private ListBox lstData;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar toolStripProgressBar1;
        private ContextMenuStrip contextMenuStrip1;
        private ContextMenuStrip cmsProject;
        private ToolStripMenuItem tsmiProjectVersions;
        private GroupBox groupBox4;
        private ListBox lstExtract;
        private ToolStripMenuItem projectsToolStripMenuItem;
        private ToolStripMenuItem tsmiAddProject;
        private ToolStripMenuItem tsmiProjectOptions;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem tsmiHelpGitHub;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiHelpAppData;
        private ToolStripMenuItem tsmiProjectRemove;
        private ToolStripSeparator tssProjectActions;
    }
}