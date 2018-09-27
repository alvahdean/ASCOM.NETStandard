namespace xAscom.App.ControlPanel
{
    partial class frmDomeControl
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.btnPark = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnSyncAz = new System.Windows.Forms.Button();
            this.btnSlewAltAz = new System.Windows.Forms.Button();
            this.btnSetPark = new System.Windows.Forms.Button();
            this.txtSlewAlt = new System.Windows.Forms.TextBox();
            this.lblSlewAlt = new System.Windows.Forms.Label();
            this.lblSlewAz = new System.Windows.Forms.Label();
            this.txtSlewAz = new System.Windows.Forms.TextBox();
            this.lblEndpoint = new System.Windows.Forms.Label();
            this.lblDriverName = new System.Windows.Forms.Label();
            this.grpControl = new System.Windows.Forms.GroupBox();
            this.btnFindHome = new System.Windows.Forms.Button();
            this.ddlActionName = new System.Windows.Forms.ListBox();
            this.lblExecActionArgs = new System.Windows.Forms.Label();
            this.txtActionArgs = new System.Windows.Forms.TextBox();
            this.lblExecActionName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.grpChooser = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.ddlEndpoint = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.tabDeviceViewCapabilties = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabDeviceViewStateRender = new System.Windows.Forms.TabPage();
            this.tabDeviceViewStateTable = new System.Windows.Forms.TabPage();
            this.tblSession = new System.Windows.Forms.TableLayoutPanel();
            this.txtSessionState = new System.Windows.Forms.TextBox();
            this.txtControlUser = new System.Windows.Forms.TextBox();
            this.txtSessionUser = new System.Windows.Forms.TextBox();
            this.txtSessionMode = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.grpState = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStateAlt = new System.Windows.Forms.TextBox();
            this.txtStateAz = new System.Windows.Forms.TextBox();
            this.txtStateMotion = new System.Windows.Forms.TextBox();
            this.lblStateAlt = new System.Windows.Forms.Label();
            this.txtStateShutter = new System.Windows.Forms.TextBox();
            this.txtStateSlaved = new System.Windows.Forms.TextBox();
            this.lblStateAz = new System.Windows.Forms.Label();
            this.txtStateAtHome = new System.Windows.Forms.TextBox();
            this.txtStateParked = new System.Windows.Forms.TextBox();
            this.txtStateUser = new System.Windows.Forms.TextBox();
            this.lblStateLastAction = new System.Windows.Forms.Label();
            this.txtStateLastAction = new System.Windows.Forms.TextBox();
            this.txtStateLastOpTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tabDeviceView = new System.Windows.Forms.TabControl();
            this.grpControl.SuspendLayout();
            this.grpChooser.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabDeviceViewCapabilties.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabDeviceViewStateTable.SuspendLayout();
            this.tblSession.SuspendLayout();
            this.grpState.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabDeviceView.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonConnect.Location = new System.Drawing.Point(503, 3);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Padding = new System.Windows.Forms.Padding(10);
            this.buttonConnect.Size = new System.Drawing.Size(382, 55);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.OnConnectClick);
            // 
            // btnPark
            // 
            this.btnPark.Location = new System.Drawing.Point(20, 42);
            this.btnPark.Name = "btnPark";
            this.btnPark.Size = new System.Drawing.Size(200, 62);
            this.btnPark.TabIndex = 3;
            this.btnPark.Text = "Park";
            this.btnPark.UseVisualStyleBackColor = true;
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(20, 240);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(200, 62);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            // 
            // btnSyncAz
            // 
            this.btnSyncAz.Location = new System.Drawing.Point(20, 175);
            this.btnSyncAz.Name = "btnSyncAz";
            this.btnSyncAz.Size = new System.Drawing.Size(200, 62);
            this.btnSyncAz.TabIndex = 6;
            this.btnSyncAz.Text = "Sync Az";
            this.btnSyncAz.UseVisualStyleBackColor = true;
            // 
            // btnSlewAltAz
            // 
            this.btnSlewAltAz.Location = new System.Drawing.Point(20, 110);
            this.btnSlewAltAz.Name = "btnSlewAltAz";
            this.btnSlewAltAz.Size = new System.Drawing.Size(200, 62);
            this.btnSlewAltAz.TabIndex = 7;
            this.btnSlewAltAz.Text = "Slew Alt/Az";
            this.btnSlewAltAz.UseVisualStyleBackColor = true;
            // 
            // btnSetPark
            // 
            this.btnSetPark.Location = new System.Drawing.Point(470, 42);
            this.btnSetPark.Name = "btnSetPark";
            this.btnSetPark.Size = new System.Drawing.Size(200, 62);
            this.btnSetPark.TabIndex = 8;
            this.btnSetPark.Text = "SetPark";
            this.btnSetPark.UseVisualStyleBackColor = true;
            // 
            // txtSlewAlt
            // 
            this.txtSlewAlt.Location = new System.Drawing.Point(244, 141);
            this.txtSlewAlt.Name = "txtSlewAlt";
            this.txtSlewAlt.Size = new System.Drawing.Size(200, 31);
            this.txtSlewAlt.TabIndex = 9;
            this.txtSlewAlt.Text = "0.0";
            // 
            // lblSlewAlt
            // 
            this.lblSlewAlt.AutoSize = true;
            this.lblSlewAlt.Location = new System.Drawing.Point(244, 110);
            this.lblSlewAlt.Name = "lblSlewAlt";
            this.lblSlewAlt.Size = new System.Drawing.Size(84, 25);
            this.lblSlewAlt.TabIndex = 10;
            this.lblSlewAlt.Text = "Altitude";
            // 
            // lblSlewAz
            // 
            this.lblSlewAz.AutoSize = true;
            this.lblSlewAz.Location = new System.Drawing.Point(470, 110);
            this.lblSlewAz.Name = "lblSlewAz";
            this.lblSlewAz.Size = new System.Drawing.Size(89, 25);
            this.lblSlewAz.TabIndex = 12;
            this.lblSlewAz.Text = "Azimuth";
            // 
            // txtSlewAz
            // 
            this.txtSlewAz.Location = new System.Drawing.Point(470, 141);
            this.txtSlewAz.Name = "txtSlewAz";
            this.txtSlewAz.Size = new System.Drawing.Size(200, 31);
            this.txtSlewAz.TabIndex = 11;
            this.txtSlewAz.Text = "0.0";
            // 
            // lblEndpoint
            // 
            this.lblEndpoint.AutoSize = true;
            this.lblEndpoint.Location = new System.Drawing.Point(3, 0);
            this.lblEndpoint.Name = "lblEndpoint";
            this.lblEndpoint.Size = new System.Drawing.Size(97, 25);
            this.lblEndpoint.TabIndex = 13;
            this.lblEndpoint.Text = "Endpoint";
            // 
            // lblDriverName
            // 
            this.lblDriverName.AutoSize = true;
            this.lblDriverName.Location = new System.Drawing.Point(3, 61);
            this.lblDriverName.Name = "lblDriverName";
            this.lblDriverName.Size = new System.Drawing.Size(69, 25);
            this.lblDriverName.TabIndex = 16;
            this.lblDriverName.Text = "Driver";
            // 
            // grpControl
            // 
            this.grpControl.Controls.Add(this.btnFindHome);
            this.grpControl.Controls.Add(this.ddlActionName);
            this.grpControl.Controls.Add(this.lblExecActionArgs);
            this.grpControl.Controls.Add(this.txtActionArgs);
            this.grpControl.Controls.Add(this.lblExecActionName);
            this.grpControl.Controls.Add(this.label1);
            this.grpControl.Controls.Add(this.textBox1);
            this.grpControl.Controls.Add(this.btnPark);
            this.grpControl.Controls.Add(this.btnExecute);
            this.grpControl.Controls.Add(this.btnSyncAz);
            this.grpControl.Controls.Add(this.btnSlewAltAz);
            this.grpControl.Controls.Add(this.lblSlewAz);
            this.grpControl.Controls.Add(this.btnSetPark);
            this.grpControl.Controls.Add(this.txtSlewAz);
            this.grpControl.Controls.Add(this.txtSlewAlt);
            this.grpControl.Controls.Add(this.lblSlewAlt);
            this.grpControl.Location = new System.Drawing.Point(12, 199);
            this.grpControl.Name = "grpControl";
            this.grpControl.Size = new System.Drawing.Size(905, 323);
            this.grpControl.TabIndex = 17;
            this.grpControl.TabStop = false;
            this.grpControl.Text = "Operations";
            // 
            // btnFindHome
            // 
            this.btnFindHome.Location = new System.Drawing.Point(244, 42);
            this.btnFindHome.Name = "btnFindHome";
            this.btnFindHome.Size = new System.Drawing.Size(200, 62);
            this.btnFindHome.TabIndex = 20;
            this.btnFindHome.Text = "Find Home";
            this.btnFindHome.UseVisualStyleBackColor = true;
            // 
            // ddlActionName
            // 
            this.ddlActionName.FormattingEnabled = true;
            this.ddlActionName.ItemHeight = 25;
            this.ddlActionName.Location = new System.Drawing.Point(244, 273);
            this.ddlActionName.Name = "ddlActionName";
            this.ddlActionName.Size = new System.Drawing.Size(200, 29);
            this.ddlActionName.TabIndex = 19;
            // 
            // lblExecActionArgs
            // 
            this.lblExecActionArgs.AutoSize = true;
            this.lblExecActionArgs.Location = new System.Drawing.Point(475, 240);
            this.lblExecActionArgs.Name = "lblExecActionArgs";
            this.lblExecActionArgs.Size = new System.Drawing.Size(115, 25);
            this.lblExecActionArgs.TabIndex = 18;
            this.lblExecActionArgs.Text = "Arguments";
            // 
            // txtActionArgs
            // 
            this.txtActionArgs.Location = new System.Drawing.Point(475, 271);
            this.txtActionArgs.Name = "txtActionArgs";
            this.txtActionArgs.Size = new System.Drawing.Size(200, 31);
            this.txtActionArgs.TabIndex = 17;
            this.txtActionArgs.Text = "arg1, arg2, arg3";
            // 
            // lblExecActionName
            // 
            this.lblExecActionName.AutoSize = true;
            this.lblExecActionName.Location = new System.Drawing.Point(249, 240);
            this.lblExecActionName.Name = "lblExecActionName";
            this.lblExecActionName.Size = new System.Drawing.Size(72, 25);
            this.lblExecActionName.TabIndex = 16;
            this.lblExecActionName.Text = "Action";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 25);
            this.label1.TabIndex = 14;
            this.label1.Text = "Azimuth";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(244, 206);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(200, 31);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "0.0";
            // 
            // grpChooser
            // 
            this.grpChooser.Controls.Add(this.tableLayoutPanel3);
            this.grpChooser.Location = new System.Drawing.Point(23, 12);
            this.grpChooser.Name = "grpChooser";
            this.grpChooser.Size = new System.Drawing.Size(894, 152);
            this.grpChooser.TabIndex = 32;
            this.grpChooser.TabStop = false;
            this.grpChooser.Text = "Chooser";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.buttonConnect, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblEndpoint, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.lblDriverName, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.ddlEndpoint, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBox2, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(888, 122);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(503, 64);
            this.btnClose.Name = "btnClose";
            this.btnClose.Padding = new System.Windows.Forms.Padding(10);
            this.btnClose.Size = new System.Drawing.Size(382, 55);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.OnCloseClick);
            // 
            // ddlEndpoint
            // 
            this.ddlEndpoint.FormattingEnabled = true;
            this.ddlEndpoint.Location = new System.Drawing.Point(106, 3);
            this.ddlEndpoint.Name = "ddlEndpoint";
            this.ddlEndpoint.Size = new System.Drawing.Size(391, 33);
            this.ddlEndpoint.TabIndex = 34;
            this.ddlEndpoint.ValueMember = "ServiceRoot";
            this.ddlEndpoint.SelectedValueChanged += new System.EventHandler(this.ddlEndpoint_SelectedValueChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.DisplayMember = "Name";
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(106, 64);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(391, 33);
            this.comboBox2.TabIndex = 35;
            this.comboBox2.ValueMember = "Name";
            // 
            // tabDeviceViewCapabilties
            // 
            this.tabDeviceViewCapabilties.Controls.Add(this.groupBox1);
            this.tabDeviceViewCapabilties.Location = new System.Drawing.Point(8, 39);
            this.tabDeviceViewCapabilties.Name = "tabDeviceViewCapabilties";
            this.tabDeviceViewCapabilties.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeviceViewCapabilties.Size = new System.Drawing.Size(895, 624);
            this.tabDeviceViewCapabilties.TabIndex = 2;
            this.tabDeviceViewCapabilties.Text = "Capabilities";
            this.tabDeviceViewCapabilties.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 50, 0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.groupBox1.Size = new System.Drawing.Size(889, 618);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Capabilities";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.textBox3, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 34);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(889, 584);
            this.tableLayoutPanel2.TabIndex = 28;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(3, 3);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(182, 31);
            this.textBox3.TabIndex = 35;
            this.textBox3.Text = "Not Connected";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabDeviceViewStateRender
            // 
            this.tabDeviceViewStateRender.Location = new System.Drawing.Point(8, 39);
            this.tabDeviceViewStateRender.Name = "tabDeviceViewStateRender";
            this.tabDeviceViewStateRender.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeviceViewStateRender.Size = new System.Drawing.Size(895, 624);
            this.tabDeviceViewStateRender.TabIndex = 1;
            this.tabDeviceViewStateRender.Text = "Render";
            this.tabDeviceViewStateRender.UseVisualStyleBackColor = true;
            // 
            // tabDeviceViewStateTable
            // 
            this.tabDeviceViewStateTable.Controls.Add(this.tblSession);
            this.tabDeviceViewStateTable.Controls.Add(this.grpState);
            this.tabDeviceViewStateTable.Location = new System.Drawing.Point(8, 39);
            this.tabDeviceViewStateTable.Name = "tabDeviceViewStateTable";
            this.tabDeviceViewStateTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeviceViewStateTable.Size = new System.Drawing.Size(895, 624);
            this.tabDeviceViewStateTable.TabIndex = 0;
            this.tabDeviceViewStateTable.Text = "State";
            this.tabDeviceViewStateTable.UseVisualStyleBackColor = true;
            // 
            // tblSession
            // 
            this.tblSession.AutoSize = true;
            this.tblSession.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblSession.ColumnCount = 4;
            this.tblSession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tblSession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tblSession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tblSession.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 289F));
            this.tblSession.Controls.Add(this.txtSessionState, 0, 1);
            this.tblSession.Controls.Add(this.txtControlUser, 0, 1);
            this.tblSession.Controls.Add(this.txtSessionUser, 0, 1);
            this.tblSession.Controls.Add(this.txtSessionMode, 0, 1);
            this.tblSession.Controls.Add(this.label11, 0, 0);
            this.tblSession.Controls.Add(this.label14, 1, 0);
            this.tblSession.Controls.Add(this.label15, 2, 0);
            this.tblSession.Controls.Add(this.label16, 3, 0);
            this.tblSession.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tblSession.Location = new System.Drawing.Point(3, 559);
            this.tblSession.Name = "tblSession";
            this.tblSession.RowCount = 2;
            this.tblSession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblSession.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblSession.Size = new System.Drawing.Size(889, 62);
            this.tblSession.TabIndex = 33;
            // 
            // txtSessionState
            // 
            this.txtSessionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSessionState.Location = new System.Drawing.Point(3, 28);
            this.txtSessionState.Name = "txtSessionState";
            this.txtSessionState.ReadOnly = true;
            this.txtSessionState.Size = new System.Drawing.Size(194, 31);
            this.txtSessionState.TabIndex = 50;
            this.txtSessionState.Text = "Not Connected";
            this.txtSessionState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtControlUser
            // 
            this.txtControlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtControlUser.Location = new System.Drawing.Point(203, 28);
            this.txtControlUser.Name = "txtControlUser";
            this.txtControlUser.ReadOnly = true;
            this.txtControlUser.Size = new System.Drawing.Size(194, 31);
            this.txtControlUser.TabIndex = 49;
            this.txtControlUser.Text = "Not Connected";
            this.txtControlUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSessionUser
            // 
            this.txtSessionUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSessionUser.Location = new System.Drawing.Point(403, 28);
            this.txtSessionUser.Name = "txtSessionUser";
            this.txtSessionUser.ReadOnly = true;
            this.txtSessionUser.Size = new System.Drawing.Size(194, 31);
            this.txtSessionUser.TabIndex = 48;
            this.txtSessionUser.Text = "Not Connected";
            this.txtSessionUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSessionMode
            // 
            this.txtSessionMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSessionMode.Location = new System.Drawing.Point(603, 28);
            this.txtSessionMode.Name = "txtSessionMode";
            this.txtSessionMode.ReadOnly = true;
            this.txtSessionMode.Size = new System.Drawing.Size(283, 31);
            this.txtSessionMode.TabIndex = 47;
            this.txtSessionMode.Text = "Not Connected";
            this.txtSessionMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(194, 25);
            this.label11.TabIndex = 29;
            this.label11.Text = "State";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(203, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(194, 25);
            this.label14.TabIndex = 42;
            this.label14.Text = "Mode";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(403, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(194, 25);
            this.label15.TabIndex = 43;
            this.label15.Text = "Session User";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(603, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(283, 25);
            this.label16.TabIndex = 44;
            this.label16.Text = "Control User";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpState
            // 
            this.grpState.AutoSize = true;
            this.grpState.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpState.Controls.Add(this.tableLayoutPanel1);
            this.grpState.Location = new System.Drawing.Point(19, 13);
            this.grpState.Name = "grpState";
            this.grpState.Size = new System.Drawing.Size(366, 437);
            this.grpState.TabIndex = 27;
            this.grpState.TabStop = false;
            this.grpState.Text = "State";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtStateAlt, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtStateAz, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtStateMotion, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblStateAlt, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtStateShutter, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtStateSlaved, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblStateAz, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtStateAtHome, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtStateParked, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtStateUser, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblStateLastAction, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtStateLastAction, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.txtStateLastOpTime, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.textBox2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(360, 407);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 296);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(132, 25);
            this.label9.TabIndex = 34;
            this.label9.Text = "Control User";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 259);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 33;
            this.label8.Text = "Parked";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 25);
            this.label7.TabIndex = 32;
            this.label7.Text = "At Home";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 185);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 25);
            this.label6.TabIndex = 31;
            this.label6.Text = "Slave State";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(148, 25);
            this.label5.TabIndex = 30;
            this.label5.Text = "Shutter Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 25);
            this.label4.TabIndex = 29;
            this.label4.Text = "Motion State";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 25);
            this.label3.TabIndex = 28;
            this.label3.Text = "Connection";
            // 
            // txtStateAlt
            // 
            this.txtStateAlt.Location = new System.Drawing.Point(175, 40);
            this.txtStateAlt.Name = "txtStateAlt";
            this.txtStateAlt.ReadOnly = true;
            this.txtStateAlt.Size = new System.Drawing.Size(69, 31);
            this.txtStateAlt.TabIndex = 6;
            this.txtStateAlt.Text = "0.0";
            this.txtStateAlt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStateAz
            // 
            this.txtStateAz.Location = new System.Drawing.Point(175, 77);
            this.txtStateAz.Name = "txtStateAz";
            this.txtStateAz.ReadOnly = true;
            this.txtStateAz.Size = new System.Drawing.Size(69, 31);
            this.txtStateAz.TabIndex = 7;
            this.txtStateAz.Text = "0.0";
            this.txtStateAz.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStateMotion
            // 
            this.txtStateMotion.Location = new System.Drawing.Point(175, 114);
            this.txtStateMotion.Name = "txtStateMotion";
            this.txtStateMotion.ReadOnly = true;
            this.txtStateMotion.Size = new System.Drawing.Size(182, 31);
            this.txtStateMotion.TabIndex = 4;
            this.txtStateMotion.Text = "Not Moving";
            this.txtStateMotion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStateAlt
            // 
            this.lblStateAlt.AutoSize = true;
            this.lblStateAlt.Location = new System.Drawing.Point(3, 37);
            this.lblStateAlt.Name = "lblStateAlt";
            this.lblStateAlt.Size = new System.Drawing.Size(161, 25);
            this.lblStateAlt.TabIndex = 21;
            this.lblStateAlt.Text = "Current Altitude";
            // 
            // txtStateShutter
            // 
            this.txtStateShutter.Location = new System.Drawing.Point(175, 151);
            this.txtStateShutter.Name = "txtStateShutter";
            this.txtStateShutter.ReadOnly = true;
            this.txtStateShutter.Size = new System.Drawing.Size(182, 31);
            this.txtStateShutter.TabIndex = 3;
            this.txtStateShutter.Text = "Shutter Closed";
            this.txtStateShutter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStateSlaved
            // 
            this.txtStateSlaved.Location = new System.Drawing.Point(175, 188);
            this.txtStateSlaved.Name = "txtStateSlaved";
            this.txtStateSlaved.ReadOnly = true;
            this.txtStateSlaved.Size = new System.Drawing.Size(182, 31);
            this.txtStateSlaved.TabIndex = 1;
            this.txtStateSlaved.Text = "Not Slaved";
            this.txtStateSlaved.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStateAz
            // 
            this.lblStateAz.AutoSize = true;
            this.lblStateAz.Location = new System.Drawing.Point(3, 74);
            this.lblStateAz.Name = "lblStateAz";
            this.lblStateAz.Size = new System.Drawing.Size(166, 25);
            this.lblStateAz.TabIndex = 22;
            this.lblStateAz.Text = "Current Azimuth";
            // 
            // txtStateAtHome
            // 
            this.txtStateAtHome.Location = new System.Drawing.Point(175, 225);
            this.txtStateAtHome.Name = "txtStateAtHome";
            this.txtStateAtHome.ReadOnly = true;
            this.txtStateAtHome.Size = new System.Drawing.Size(182, 31);
            this.txtStateAtHome.TabIndex = 2;
            this.txtStateAtHome.Text = "Not At Home";
            this.txtStateAtHome.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStateParked
            // 
            this.txtStateParked.Location = new System.Drawing.Point(175, 262);
            this.txtStateParked.Name = "txtStateParked";
            this.txtStateParked.ReadOnly = true;
            this.txtStateParked.Size = new System.Drawing.Size(182, 31);
            this.txtStateParked.TabIndex = 5;
            this.txtStateParked.Text = "Parked";
            this.txtStateParked.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStateUser
            // 
            this.txtStateUser.Location = new System.Drawing.Point(175, 299);
            this.txtStateUser.Name = "txtStateUser";
            this.txtStateUser.ReadOnly = true;
            this.txtStateUser.Size = new System.Drawing.Size(182, 31);
            this.txtStateUser.TabIndex = 23;
            this.txtStateUser.Text = "Username";
            this.txtStateUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblStateLastAction
            // 
            this.lblStateLastAction.AutoSize = true;
            this.lblStateLastAction.Location = new System.Drawing.Point(3, 333);
            this.lblStateLastAction.Name = "lblStateLastAction";
            this.lblStateLastAction.Size = new System.Drawing.Size(119, 25);
            this.lblStateLastAction.TabIndex = 26;
            this.lblStateLastAction.Text = "Last Action";
            this.lblStateLastAction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtStateLastAction
            // 
            this.txtStateLastAction.Location = new System.Drawing.Point(175, 336);
            this.txtStateLastAction.Name = "txtStateLastAction";
            this.txtStateLastAction.ReadOnly = true;
            this.txtStateLastAction.Size = new System.Drawing.Size(182, 31);
            this.txtStateLastAction.TabIndex = 24;
            this.txtStateLastAction.Text = "Last Action";
            this.txtStateLastAction.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStateLastOpTime
            // 
            this.txtStateLastOpTime.Location = new System.Drawing.Point(175, 373);
            this.txtStateLastOpTime.Name = "txtStateLastOpTime";
            this.txtStateLastOpTime.ReadOnly = true;
            this.txtStateLastOpTime.Size = new System.Drawing.Size(182, 31);
            this.txtStateLastOpTime.TabIndex = 25;
            this.txtStateLastOpTime.Text = "1969-Apr-12 08:55:18";
            this.txtStateLastOpTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 370);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 25);
            this.label2.TabIndex = 27;
            this.label2.Text = "Last Action";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(175, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(182, 31);
            this.textBox2.TabIndex = 35;
            this.textBox2.Text = "Not Connected";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabDeviceView
            // 
            this.tabDeviceView.Controls.Add(this.tabDeviceViewStateTable);
            this.tabDeviceView.Controls.Add(this.tabDeviceViewStateRender);
            this.tabDeviceView.Controls.Add(this.tabDeviceViewCapabilties);
            this.tabDeviceView.Location = new System.Drawing.Point(12, 537);
            this.tabDeviceView.Name = "tabDeviceView";
            this.tabDeviceView.SelectedIndex = 0;
            this.tabDeviceView.Size = new System.Drawing.Size(911, 671);
            this.tabDeviceView.TabIndex = 31;
            // 
            // frmDomeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 1219);
            this.Controls.Add(this.grpChooser);
            this.Controls.Add(this.tabDeviceView);
            this.Controls.Add(this.grpControl);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "frmDomeControl";
            this.Text = "Dome Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.grpControl.ResumeLayout(false);
            this.grpControl.PerformLayout();
            this.grpChooser.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabDeviceViewCapabilties.ResumeLayout(false);
            this.tabDeviceViewCapabilties.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabDeviceViewStateTable.ResumeLayout(false);
            this.tabDeviceViewStateTable.PerformLayout();
            this.tblSession.ResumeLayout(false);
            this.tblSession.PerformLayout();
            this.grpState.ResumeLayout(false);
            this.grpState.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabDeviceView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button btnPark;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnSyncAz;
        private System.Windows.Forms.Button btnSlewAltAz;
        private System.Windows.Forms.Button btnSetPark;
        private System.Windows.Forms.TextBox txtSlewAlt;
        private System.Windows.Forms.Label lblSlewAlt;
        private System.Windows.Forms.Label lblSlewAz;
        private System.Windows.Forms.TextBox txtSlewAz;
        private System.Windows.Forms.Label lblEndpoint;
        private System.Windows.Forms.Label lblDriverName;
        private System.Windows.Forms.GroupBox grpControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnFindHome;
        private System.Windows.Forms.ListBox ddlActionName;
        private System.Windows.Forms.Label lblExecActionArgs;
        private System.Windows.Forms.TextBox txtActionArgs;
        private System.Windows.Forms.Label lblExecActionName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox grpChooser;
        private System.Windows.Forms.TabPage tabDeviceViewCapabilties;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TabPage tabDeviceViewStateRender;
        private System.Windows.Forms.TabPage tabDeviceViewStateTable;
        private System.Windows.Forms.TableLayoutPanel tblSession;
        private System.Windows.Forms.TextBox txtSessionState;
        private System.Windows.Forms.TextBox txtControlUser;
        private System.Windows.Forms.TextBox txtSessionUser;
        private System.Windows.Forms.TextBox txtSessionMode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox grpState;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStateAlt;
        private System.Windows.Forms.TextBox txtStateAz;
        private System.Windows.Forms.TextBox txtStateMotion;
        private System.Windows.Forms.Label lblStateAlt;
        private System.Windows.Forms.TextBox txtStateShutter;
        private System.Windows.Forms.TextBox txtStateSlaved;
        private System.Windows.Forms.Label lblStateAz;
        private System.Windows.Forms.TextBox txtStateAtHome;
        private System.Windows.Forms.TextBox txtStateParked;
        private System.Windows.Forms.TextBox txtStateUser;
        private System.Windows.Forms.Label lblStateLastAction;
        private System.Windows.Forms.TextBox txtStateLastAction;
        private System.Windows.Forms.TextBox txtStateLastOpTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TabControl tabDeviceView;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox ddlEndpoint;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.BindingSource DriverUIStateBindingSource;
    }
}

