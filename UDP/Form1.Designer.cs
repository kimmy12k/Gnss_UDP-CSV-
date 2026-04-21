namespace UDPMode
{
    partial class Form1
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
            grNetwork = new DevExpress.XtraEditors.GroupControl();
            picStatus = new DevExpress.XtraEditors.PictureEdit();
            btnDisconnect = new DevExpress.XtraEditors.SimpleButton();
            btnConnect = new DevExpress.XtraEditors.SimpleButton();
            txtUdpPort = new DevExpress.XtraEditors.TextEdit();
            lblUDPPrt = new DevExpress.XtraEditors.LabelControl();
            txtScpiPort = new DevExpress.XtraEditors.TextEdit();
            lblSCPIPort = new DevExpress.XtraEditors.LabelControl();
            txtIP = new DevExpress.XtraEditors.TextEdit();
            lblIp = new DevExpress.XtraEditors.LabelControl();
            btnGnssOn = new DevExpress.XtraEditors.SimpleButton();
            btnInitialize = new DevExpress.XtraEditors.SimpleButton();
            grControl = new DevExpress.XtraEditors.GroupControl();
            btnConfig = new DevExpress.XtraEditors.SimpleButton();
            btnRfOff = new DevExpress.XtraEditors.SimpleButton();
            btnHilStart = new DevExpress.XtraEditors.SimpleButton();
            btnLoadCsv = new DevExpress.XtraEditors.SimpleButton();
            btnHilStop = new DevExpress.XtraEditors.SimpleButton();
            btnRfOn = new DevExpress.XtraEditors.SimpleButton();
            btnGnssOff = new DevExpress.XtraEditors.SimpleButton();
            btnClear = new DevExpress.XtraEditors.SimpleButton();
            grStatus = new DevExpress.XtraEditors.GroupControl();
            lblPdop = new DevExpress.XtraEditors.LabelControl();
            lblSimInfo = new DevExpress.XtraEditors.LabelControl();
            lblTestMode = new DevExpress.XtraEditors.LabelControl();
            lblGnssState = new DevExpress.XtraEditors.LabelControl();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            grHilMonitor = new DevExpress.XtraEditors.GroupControl();
            lblHilStatus = new DevExpress.XtraEditors.LabelControl();
            lblPacketCount = new DevExpress.XtraEditors.LabelControl();
            lblUdpPort = new DevExpress.XtraEditors.LabelControl();
            labelControl9 = new DevExpress.XtraEditors.LabelControl();
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            grGnssConfig = new DevExpress.XtraEditors.GroupControl();
            txtLevel = new DevExpress.XtraEditors.TextEdit();
            labelControl10 = new DevExpress.XtraEditors.LabelControl();
            txtAlt = new DevExpress.XtraEditors.TextEdit();
            txtLon = new DevExpress.XtraEditors.TextEdit();
            txtLat = new DevExpress.XtraEditors.TextEdit();
            comboPosition = new DevExpress.XtraEditors.ComboBoxEdit();
            comboTestMode = new DevExpress.XtraEditors.ComboBoxEdit();
            lblAlt = new DevExpress.XtraEditors.LabelControl();
            lblLon = new DevExpress.XtraEditors.LabelControl();
            lblLat = new DevExpress.XtraEditors.LabelControl();
            lblPosition = new DevExpress.XtraEditors.LabelControl();
            lblTest = new DevExpress.XtraEditors.LabelControl();
            grLog = new DevExpress.XtraEditors.GroupControl();
            btnLogClear = new DevExpress.XtraEditors.SimpleButton();
            memoLog = new DevExpress.XtraEditors.MemoEdit();
            grSetTime = new DevExpress.XtraEditors.GroupControl();
            DateEdits = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            btnSetCurrentDate = new DevExpress.XtraEditors.SimpleButton();
            btnSetDate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)grNetwork).BeginInit();
            grNetwork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picStatus.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtUdpPort.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtScpiPort.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtIP.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grControl).BeginInit();
            grControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grStatus).BeginInit();
            grStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grHilMonitor).BeginInit();
            grHilMonitor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grGnssConfig).BeginInit();
            grGnssConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtLevel.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtAlt.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtLon.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtLat.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comboPosition.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comboTestMode.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grLog).BeginInit();
            grLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)memoLog.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grSetTime).BeginInit();
            grSetTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DateEdits.Properties).BeginInit();
            SuspendLayout();
            // 
            // grNetwork
            // 
            grNetwork.Controls.Add(picStatus);
            grNetwork.Controls.Add(btnDisconnect);
            grNetwork.Controls.Add(btnConnect);
            grNetwork.Controls.Add(txtUdpPort);
            grNetwork.Controls.Add(lblUDPPrt);
            grNetwork.Controls.Add(txtScpiPort);
            grNetwork.Controls.Add(lblSCPIPort);
            grNetwork.Controls.Add(txtIP);
            grNetwork.Controls.Add(lblIp);
            grNetwork.Location = new System.Drawing.Point(5, 13);
            grNetwork.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            grNetwork.Name = "grNetwork";
            grNetwork.Size = new System.Drawing.Size(346, 120);
            grNetwork.TabIndex = 0;
            grNetwork.Text = "NETWORK";
            // 
            // picStatus
            // 
            picStatus.Location = new System.Drawing.Point(276, 83);
            picStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            picStatus.Name = "picStatus";
            picStatus.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            picStatus.Size = new System.Drawing.Size(48, 29);
            picStatus.TabIndex = 8;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new System.Drawing.Point(206, 83);
            btnDisconnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new System.Drawing.Size(50, 30);
            btnDisconnect.TabIndex = 7;
            btnDisconnect.Text = "해제";
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Location = new System.Drawing.Point(151, 83);
            btnConnect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new System.Drawing.Size(51, 30);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "연결";
            btnConnect.Click += btnConnect_Click;
            // 
            // txtUdpPort
            // 
            txtUdpPort.Location = new System.Drawing.Point(14, 86);
            txtUdpPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtUdpPort.Name = "txtUdpPort";
            txtUdpPort.Size = new System.Drawing.Size(114, 20);
            txtUdpPort.TabIndex = 5;
            // 
            // lblUDPPrt
            // 
            lblUDPPrt.Location = new System.Drawing.Point(26, 69);
            lblUDPPrt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lblUDPPrt.Name = "lblUDPPrt";
            lblUDPPrt.Size = new System.Drawing.Size(50, 14);
            lblUDPPrt.TabIndex = 4;
            lblUDPPrt.Text = "UDP Port";
            // 
            // txtScpiPort
            // 
            txtScpiPort.Location = new System.Drawing.Point(151, 43);
            txtScpiPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtScpiPort.Name = "txtScpiPort";
            txtScpiPort.Size = new System.Drawing.Size(107, 20);
            txtScpiPort.TabIndex = 3;
            // 
            // lblSCPIPort
            // 
            lblSCPIPort.Location = new System.Drawing.Point(165, 27);
            lblSCPIPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lblSCPIPort.Name = "lblSCPIPort";
            lblSCPIPort.Size = new System.Drawing.Size(52, 14);
            lblSCPIPort.TabIndex = 2;
            lblSCPIPort.Text = "SCPI Port";
            // 
            // txtIP
            // 
            txtIP.Location = new System.Drawing.Point(15, 43);
            txtIP.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtIP.Name = "txtIP";
            txtIP.Size = new System.Drawing.Size(114, 20);
            txtIP.TabIndex = 1;
            // 
            // lblIp
            // 
            lblIp.Location = new System.Drawing.Point(27, 27);
            lblIp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            lblIp.Name = "lblIp";
            lblIp.Size = new System.Drawing.Size(51, 14);
            lblIp.TabIndex = 0;
            lblIp.Text = "Device IP";
            // 
            // btnGnssOn
            // 
            btnGnssOn.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnGnssOn.Appearance.Options.UseFont = true;
            btnGnssOn.Location = new System.Drawing.Point(8, 69);
            btnGnssOn.Name = "btnGnssOn";
            btnGnssOn.Size = new System.Drawing.Size(76, 32);
            btnGnssOn.TabIndex = 25;
            btnGnssOn.Text = "GNSS ON";
            btnGnssOn.Click += btnGnssOn_Click;
            // 
            // btnInitialize
            // 
            btnInitialize.Appearance.BackColor = System.Drawing.Color.Yellow;
            btnInitialize.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnInitialize.Appearance.ForeColor = System.Drawing.Color.Black;
            btnInitialize.Appearance.Options.UseBackColor = true;
            btnInitialize.Appearance.Options.UseFont = true;
            btnInitialize.Appearance.Options.UseForeColor = true;
            btnInitialize.AppearanceDisabled.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnInitialize.AppearanceDisabled.Options.UseFont = true;
            btnInitialize.Location = new System.Drawing.Point(9, 29);
            btnInitialize.Name = "btnInitialize";
            btnInitialize.Size = new System.Drawing.Size(76, 32);
            btnInitialize.TabIndex = 24;
            btnInitialize.Text = "Initialize";
            btnInitialize.Click += btnInitialize_Click;
            // 
            // grControl
            // 
            grControl.Controls.Add(btnConfig);
            grControl.Controls.Add(btnRfOff);
            grControl.Controls.Add(btnHilStart);
            grControl.Controls.Add(btnLoadCsv);
            grControl.Controls.Add(btnHilStop);
            grControl.Controls.Add(btnRfOn);
            grControl.Controls.Add(btnGnssOff);
            grControl.Controls.Add(btnInitialize);
            grControl.Controls.Add(btnGnssOn);
            grControl.Location = new System.Drawing.Point(5, 354);
            grControl.Name = "grControl";
            grControl.Size = new System.Drawing.Size(346, 120);
            grControl.TabIndex = 24;
            grControl.Text = "Control";
            // 
            // btnConfig
            // 
            btnConfig.Appearance.BackColor = System.Drawing.Color.White;
            btnConfig.Appearance.BorderColor = System.Drawing.Color.FromArgb(128, 255, 255);
            btnConfig.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnConfig.Appearance.Options.UseBackColor = true;
            btnConfig.Appearance.Options.UseBorderColor = true;
            btnConfig.Appearance.Options.UseFont = true;
            btnConfig.Location = new System.Drawing.Point(90, 29);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new System.Drawing.Size(76, 32);
            btnConfig.TabIndex = 34;
            btnConfig.Text = "Config";
            btnConfig.Click += btnConfig_Click;
            // 
            // btnRfOff
            // 
            btnRfOff.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnRfOff.Appearance.Options.UseFont = true;
            btnRfOff.Location = new System.Drawing.Point(258, 69);
            btnRfOff.Name = "btnRfOff";
            btnRfOff.Size = new System.Drawing.Size(76, 32);
            btnRfOff.TabIndex = 33;
            btnRfOff.Text = "RF OFF";
            btnRfOff.Click += btnRfOff_Click;
            // 
            // btnHilStart
            // 
            btnHilStart.Appearance.BackColor = System.Drawing.Color.Aqua;
            btnHilStart.Appearance.BorderColor = System.Drawing.Color.FromArgb(128, 255, 255);
            btnHilStart.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnHilStart.Appearance.Options.UseBackColor = true;
            btnHilStart.Appearance.Options.UseBorderColor = true;
            btnHilStart.Appearance.Options.UseFont = true;
            btnHilStart.Location = new System.Drawing.Point(177, 29);
            btnHilStart.Name = "btnHilStart";
            btnHilStart.Size = new System.Drawing.Size(76, 32);
            btnHilStart.TabIndex = 30;
            btnHilStart.Text = "HIL Start";
            btnHilStart.Click += btnHilStart_Click;
            // 
            // btnLoadCsv
            // 
            btnLoadCsv.Appearance.BackColor = System.Drawing.Color.FromArgb(128, 255, 128);
            btnLoadCsv.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnLoadCsv.Appearance.Options.UseBackColor = true;
            btnLoadCsv.Appearance.Options.UseFont = true;
            btnLoadCsv.Location = new System.Drawing.Point(268, 1);
            btnLoadCsv.Name = "btnLoadCsv";
            btnLoadCsv.Size = new System.Drawing.Size(65, 23);
            btnLoadCsv.TabIndex = 32;
            btnLoadCsv.Text = "CSV Load";
            btnLoadCsv.Click += btnLoadCsv_Click;
            // 
            // btnHilStop
            // 
            btnHilStop.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnHilStop.Appearance.Options.UseFont = true;
            btnHilStop.Location = new System.Drawing.Point(259, 29);
            btnHilStop.Name = "btnHilStop";
            btnHilStop.Size = new System.Drawing.Size(76, 32);
            btnHilStop.TabIndex = 31;
            btnHilStop.Text = "HIL Stop";
            btnHilStop.Click += btnHilStop_Click;
            // 
            // btnRfOn
            // 
            btnRfOn.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnRfOn.Appearance.Options.UseFont = true;
            btnRfOn.Location = new System.Drawing.Point(176, 69);
            btnRfOn.Name = "btnRfOn";
            btnRfOn.Size = new System.Drawing.Size(76, 32);
            btnRfOn.TabIndex = 29;
            btnRfOn.Text = "RF ON";
            btnRfOn.Click += btnRfOn_Click;
            // 
            // btnGnssOff
            // 
            btnGnssOff.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnGnssOff.Appearance.Options.UseFont = true;
            btnGnssOff.Location = new System.Drawing.Point(91, 69);
            btnGnssOff.Name = "btnGnssOff";
            btnGnssOff.Size = new System.Drawing.Size(76, 32);
            btnGnssOff.TabIndex = 26;
            btnGnssOff.Text = "GNSS OFF";
            btnGnssOff.Click += btnGnssOff_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new System.Drawing.Point(177, 1);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(75, 23);
            btnClear.TabIndex = 35;
            btnClear.Text = "Clear";
            btnClear.Click += btnClear_Click;
            // 
            // grStatus
            // 
            grStatus.Controls.Add(lblPdop);
            grStatus.Controls.Add(lblSimInfo);
            grStatus.Controls.Add(lblTestMode);
            grStatus.Controls.Add(lblGnssState);
            grStatus.Controls.Add(labelControl4);
            grStatus.Controls.Add(labelControl3);
            grStatus.Controls.Add(labelControl1);
            grStatus.Location = new System.Drawing.Point(5, 480);
            grStatus.Name = "grStatus";
            grStatus.Size = new System.Drawing.Size(166, 117);
            grStatus.TabIndex = 34;
            grStatus.Text = "Status";
            // 
            // lblPdop
            // 
            lblPdop.Location = new System.Drawing.Point(0, 0);
            lblPdop.Name = "lblPdop";
            lblPdop.Size = new System.Drawing.Size(0, 14);
            lblPdop.TabIndex = 0;
            // 
            // lblSimInfo
            // 
            lblSimInfo.Location = new System.Drawing.Point(103, 91);
            lblSimInfo.Name = "lblSimInfo";
            lblSimInfo.Size = new System.Drawing.Size(4, 14);
            lblSimInfo.TabIndex = 44;
            lblSimInfo.Text = "-";
            // 
            // lblTestMode
            // 
            lblTestMode.Location = new System.Drawing.Point(101, 67);
            lblTestMode.Name = "lblTestMode";
            lblTestMode.Size = new System.Drawing.Size(4, 14);
            lblTestMode.TabIndex = 43;
            lblTestMode.Text = "-";
            // 
            // lblGnssState
            // 
            lblGnssState.Location = new System.Drawing.Point(102, 35);
            lblGnssState.Name = "lblGnssState";
            lblGnssState.Size = new System.Drawing.Size(4, 14);
            lblGnssState.TabIndex = 41;
            lblGnssState.Text = "-";
            // 
            // labelControl4
            // 
            labelControl4.ImeMode = System.Windows.Forms.ImeMode.On;
            labelControl4.Location = new System.Drawing.Point(5, 90);
            labelControl4.Name = "labelControl4";
            labelControl4.Size = new System.Drawing.Size(45, 14);
            labelControl4.TabIndex = 39;
            labelControl4.Text = "Sim Info";
            // 
            // labelControl3
            // 
            labelControl3.Location = new System.Drawing.Point(5, 62);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new System.Drawing.Size(59, 14);
            labelControl3.TabIndex = 38;
            labelControl3.Text = "Test Mode";
            // 
            // labelControl1
            // 
            labelControl1.Location = new System.Drawing.Point(5, 32);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(59, 14);
            labelControl1.TabIndex = 36;
            labelControl1.Text = "Gnss State";
            // 
            // grHilMonitor
            // 
            grHilMonitor.Controls.Add(lblHilStatus);
            grHilMonitor.Controls.Add(lblPacketCount);
            grHilMonitor.Controls.Add(lblUdpPort);
            grHilMonitor.Controls.Add(labelControl9);
            grHilMonitor.Controls.Add(labelControl6);
            grHilMonitor.Controls.Add(labelControl2);
            grHilMonitor.Location = new System.Drawing.Point(185, 481);
            grHilMonitor.Name = "grHilMonitor";
            grHilMonitor.Size = new System.Drawing.Size(166, 116);
            grHilMonitor.TabIndex = 35;
            grHilMonitor.Text = "HIL Monitor";
            // 
            // lblHilStatus
            // 
            lblHilStatus.Location = new System.Drawing.Point(93, 91);
            lblHilStatus.Name = "lblHilStatus";
            lblHilStatus.Size = new System.Drawing.Size(4, 14);
            lblHilStatus.TabIndex = 55;
            lblHilStatus.Text = "-";
            // 
            // lblPacketCount
            // 
            lblPacketCount.Location = new System.Drawing.Point(91, 65);
            lblPacketCount.Name = "lblPacketCount";
            lblPacketCount.Size = new System.Drawing.Size(4, 14);
            lblPacketCount.TabIndex = 52;
            lblPacketCount.Text = "-";
            // 
            // lblUdpPort
            // 
            lblUdpPort.Location = new System.Drawing.Point(93, 35);
            lblUdpPort.Name = "lblUdpPort";
            lblUdpPort.Size = new System.Drawing.Size(4, 14);
            lblUdpPort.TabIndex = 51;
            lblUdpPort.Text = "-";
            // 
            // labelControl9
            // 
            labelControl9.Location = new System.Drawing.Point(14, 91);
            labelControl9.Name = "labelControl9";
            labelControl9.Size = new System.Drawing.Size(35, 14);
            labelControl9.TabIndex = 50;
            labelControl9.Text = "Status";
            // 
            // labelControl6
            // 
            labelControl6.Location = new System.Drawing.Point(14, 63);
            labelControl6.Name = "labelControl6";
            labelControl6.Size = new System.Drawing.Size(37, 14);
            labelControl6.TabIndex = 47;
            labelControl6.Text = "Packet";
            // 
            // labelControl2
            // 
            labelControl2.Location = new System.Drawing.Point(14, 36);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(50, 14);
            labelControl2.TabIndex = 46;
            labelControl2.Text = "UDP Port";
            // 
            // grGnssConfig
            // 
            grGnssConfig.Controls.Add(txtLevel);
            grGnssConfig.Controls.Add(labelControl10);
            grGnssConfig.Controls.Add(txtAlt);
            grGnssConfig.Controls.Add(txtLon);
            grGnssConfig.Controls.Add(txtLat);
            grGnssConfig.Controls.Add(comboPosition);
            grGnssConfig.Controls.Add(comboTestMode);
            grGnssConfig.Controls.Add(lblAlt);
            grGnssConfig.Controls.Add(lblLon);
            grGnssConfig.Controls.Add(lblLat);
            grGnssConfig.Controls.Add(lblPosition);
            grGnssConfig.Controls.Add(lblTest);
            grGnssConfig.Location = new System.Drawing.Point(5, 234);
            grGnssConfig.Name = "grGnssConfig";
            grGnssConfig.Size = new System.Drawing.Size(346, 114);
            grGnssConfig.TabIndex = 9;
            grGnssConfig.Text = "Mode and Location";
            // 
            // txtLevel
            // 
            txtLevel.Location = new System.Drawing.Point(244, 46);
            txtLevel.Name = "txtLevel";
            txtLevel.Size = new System.Drawing.Size(93, 20);
            txtLevel.TabIndex = 11;
            // 
            // labelControl10
            // 
            labelControl10.Location = new System.Drawing.Point(244, 32);
            labelControl10.Name = "labelControl10";
            labelControl10.Size = new System.Drawing.Size(56, 14);
            labelControl10.TabIndex = 10;
            labelControl10.Text = "Level dBm";
            // 
            // txtAlt
            // 
            txtAlt.Location = new System.Drawing.Point(244, 89);
            txtAlt.Name = "txtAlt";
            txtAlt.Size = new System.Drawing.Size(93, 20);
            txtAlt.TabIndex = 9;
            // 
            // txtLon
            // 
            txtLon.Location = new System.Drawing.Point(130, 89);
            txtLon.Name = "txtLon";
            txtLon.Size = new System.Drawing.Size(89, 20);
            txtLon.TabIndex = 8;
            // 
            // txtLat
            // 
            txtLat.Location = new System.Drawing.Point(17, 89);
            txtLat.Name = "txtLat";
            txtLat.Size = new System.Drawing.Size(92, 20);
            txtLat.TabIndex = 7;
            // 
            // comboPosition
            // 
            comboPosition.EditValue = "";
            comboPosition.Location = new System.Drawing.Point(130, 47);
            comboPosition.Name = "comboPosition";
            comboPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            comboPosition.Properties.Items.AddRange(new object[] { "STAT", "MOV", "HIL" });
            comboPosition.Size = new System.Drawing.Size(89, 20);
            comboPosition.TabIndex = 6;
            comboPosition.SelectedIndexChanged += comboPosition_SelectedIndexChanged;
            // 
            // comboTestMode
            // 
            comboTestMode.Location = new System.Drawing.Point(17, 47);
            comboTestMode.Name = "comboTestMode";
            comboTestMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            comboTestMode.Properties.Items.AddRange(new object[] { "NAV", "TRAC", "SING" });
            comboTestMode.Size = new System.Drawing.Size(92, 20);
            comboTestMode.TabIndex = 5;
            // 
            // lblAlt
            // 
            lblAlt.Location = new System.Drawing.Point(244, 73);
            lblAlt.Name = "lblAlt";
            lblAlt.Size = new System.Drawing.Size(43, 14);
            lblAlt.TabIndex = 4;
            lblAlt.Text = "Altitude";
            // 
            // lblLon
            // 
            lblLon.Location = new System.Drawing.Point(130, 73);
            lblLon.Name = "lblLon";
            lblLon.Size = new System.Drawing.Size(48, 14);
            lblLon.TabIndex = 3;
            lblLon.Text = "Logitude";
            // 
            // lblLat
            // 
            lblLat.Location = new System.Drawing.Point(20, 73);
            lblLat.Name = "lblLat";
            lblLat.Size = new System.Drawing.Size(45, 14);
            lblLat.TabIndex = 2;
            lblLat.Text = "Latitude";
            // 
            // lblPosition
            // 
            lblPosition.Location = new System.Drawing.Point(130, 32);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new System.Drawing.Size(42, 14);
            lblPosition.TabIndex = 1;
            lblPosition.Text = "Position";
            // 
            // lblTest
            // 
            lblTest.Location = new System.Drawing.Point(19, 32);
            lblTest.Name = "lblTest";
            lblTest.Size = new System.Drawing.Size(59, 14);
            lblTest.TabIndex = 0;
            lblTest.Text = "Test Mode";
            // 
            // grLog
            // 
            grLog.Controls.Add(btnLogClear);
            grLog.Controls.Add(memoLog);
            grLog.Location = new System.Drawing.Point(378, 15);
            grLog.Name = "grLog";
            grLog.Size = new System.Drawing.Size(320, 590);
            grLog.TabIndex = 36;
            grLog.Text = "LOG";
            // 
            // btnLogClear
            // 
            btnLogClear.Location = new System.Drawing.Point(221, 5);
            btnLogClear.Name = "btnLogClear";
            btnLogClear.Size = new System.Drawing.Size(89, 19);
            btnLogClear.TabIndex = 37;
            btnLogClear.Text = "Clear";
            btnLogClear.Click += btnLogClear_Click;
            // 
            // memoLog
            // 
            memoLog.Dock = System.Windows.Forms.DockStyle.Fill;
            memoLog.Location = new System.Drawing.Point(2, 27);
            memoLog.Name = "memoLog";
            memoLog.Properties.Appearance.BackColor = System.Drawing.Color.Black;
            memoLog.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            memoLog.Properties.Appearance.ForeColor = System.Drawing.Color.Lime;
            memoLog.Properties.Appearance.Options.UseBackColor = true;
            memoLog.Properties.Appearance.Options.UseFont = true;
            memoLog.Properties.Appearance.Options.UseForeColor = true;
            memoLog.Properties.ReadOnly = true;
            memoLog.Size = new System.Drawing.Size(316, 561);
            memoLog.TabIndex = 0;
            // 
            // grSetTime
            // 
            grSetTime.Controls.Add(btnClear);
            grSetTime.Controls.Add(DateEdits);
            grSetTime.Controls.Add(btnSetCurrentDate);
            grSetTime.Controls.Add(btnSetDate);
            grSetTime.Location = new System.Drawing.Point(5, 139);
            grSetTime.Name = "grSetTime";
            grSetTime.Size = new System.Drawing.Size(346, 89);
            grSetTime.TabIndex = 35;
            grSetTime.Text = "Set Time";
            // 
            // DateEdits
            // 
            DateEdits.EditValue = null;
            DateEdits.Location = new System.Drawing.Point(5, 46);
            DateEdits.Name = "DateEdits";
            DateEdits.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            DateEdits.Size = new System.Drawing.Size(234, 20);
            DateEdits.TabIndex = 15;
            // 
            // btnSetCurrentDate
            // 
            btnSetCurrentDate.Appearance.Font = new System.Drawing.Font("Candara", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            btnSetCurrentDate.Appearance.Options.UseFont = true;
            btnSetCurrentDate.Location = new System.Drawing.Point(266, 0);
            btnSetCurrentDate.Name = "btnSetCurrentDate";
            btnSetCurrentDate.Size = new System.Drawing.Size(75, 23);
            btnSetCurrentDate.TabIndex = 14;
            btnSetCurrentDate.Text = "Set Start Time";
            btnSetCurrentDate.Click += btnSetCurrentDate_Click;
            // 
            // btnSetDate
            // 
            btnSetDate.Location = new System.Drawing.Point(251, 43);
            btnSetDate.Name = "btnSetDate";
            btnSetDate.Size = new System.Drawing.Size(84, 23);
            btnSetDate.TabIndex = 13;
            btnSetDate.Text = "apply";
            btnSetDate.Click += btnSetDate_Click;
            // 
            // Form1
            // 
            Appearance.BackColor = System.Drawing.Color.Linen;
            Appearance.Options.UseBackColor = true;
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(715, 608);
            Controls.Add(grSetTime);
            Controls.Add(grLog);
            Controls.Add(grGnssConfig);
            Controls.Add(grHilMonitor);
            Controls.Add(grStatus);
            Controls.Add(grControl);
            Controls.Add(grNetwork);
            ImeMode = System.Windows.Forms.ImeMode.Disable;
            Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            Name = "Form1";
            Text = "Gnss Remote Controller";
            Load += Form_Load;
            ((System.ComponentModel.ISupportInitialize)grNetwork).EndInit();
            grNetwork.ResumeLayout(false);
            grNetwork.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picStatus.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtUdpPort.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtScpiPort.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtIP.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)grControl).EndInit();
            grControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grStatus).EndInit();
            grStatus.ResumeLayout(false);
            grStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grHilMonitor).EndInit();
            grHilMonitor.ResumeLayout(false);
            grHilMonitor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)grGnssConfig).EndInit();
            grGnssConfig.ResumeLayout(false);
            grGnssConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)txtLevel.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtAlt.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtLon.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtLat.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)comboPosition.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)comboTestMode.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)grLog).EndInit();
            grLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)memoLog.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)grSetTime).EndInit();
            grSetTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DateEdits.Properties).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grNetwork;
        private DevExpress.XtraEditors.TextEdit txtUdpPort;
        private DevExpress.XtraEditors.LabelControl lblUDPPrt;
        private DevExpress.XtraEditors.TextEdit txtScpiPort;
        private DevExpress.XtraEditors.LabelControl lblSCPIPort;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private DevExpress.XtraEditors.LabelControl lblIp;
        private DevExpress.XtraEditors.SimpleButton btnDisconnect;
        private DevExpress.XtraEditors.SimpleButton btnConnect;
        private DevExpress.XtraEditors.PictureEdit picStatus;
        private DevExpress.XtraEditors.SimpleButton btnGnssOn;
        private DevExpress.XtraEditors.SimpleButton btnInitialize;
        private DevExpress.XtraEditors.GroupControl grControl;
        private DevExpress.XtraEditors.SimpleButton btnRfOn;
        private DevExpress.XtraEditors.SimpleButton btnGnssOff;
        private DevExpress.XtraEditors.SimpleButton btnRfOff;
        private DevExpress.XtraEditors.SimpleButton btnLoadCsv;
        private DevExpress.XtraEditors.SimpleButton btnHilStop;
        private DevExpress.XtraEditors.SimpleButton btnHilStart;
        private DevExpress.XtraEditors.GroupControl grStatus;
        private DevExpress.XtraEditors.GroupControl grHilMonitor;
        private DevExpress.XtraEditors.GroupControl grGnssConfig;
        private DevExpress.XtraEditors.TextEdit txtLat;
        private DevExpress.XtraEditors.ComboBoxEdit comboPosition;
        private DevExpress.XtraEditors.ComboBoxEdit comboTestMode;
        private DevExpress.XtraEditors.LabelControl lblAlt;
        private DevExpress.XtraEditors.LabelControl lblLon;
        private DevExpress.XtraEditors.LabelControl lblLat;
        private DevExpress.XtraEditors.LabelControl lblPosition;
        private DevExpress.XtraEditors.LabelControl lblTest;
        private DevExpress.XtraEditors.TextEdit txtAlt;
        private DevExpress.XtraEditors.TextEdit txtLon;
        private DevExpress.XtraEditors.LabelControl lblPdop;
        private DevExpress.XtraEditors.LabelControl lblSimInfo;
        private DevExpress.XtraEditors.LabelControl lblTestMode;
        private DevExpress.XtraEditors.LabelControl lblGnssState;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lblHilStatus;
        private DevExpress.XtraEditors.LabelControl lblPacketCount;
        private DevExpress.XtraEditors.LabelControl lblUdpPort;
        private DevExpress.XtraEditors.GroupControl grLog;
        private DevExpress.XtraEditors.MemoEdit memoLog;
        private DevExpress.XtraEditors.SimpleButton btnLogClear;
        private DevExpress.XtraEditors.SimpleButton btnConfig;
        private DevExpress.XtraEditors.TextEdit txtLevel;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.GroupControl grSetTime;
        private DevExpress.XtraEditors.SimpleButton btnSetDate;
        private DevExpress.XtraEditors.SimpleButton btnSetCurrentDate;
        private DevExpress.XtraEditors.DateTimeOffsetEdit DateEdits;
        private DevExpress.XtraEditors.SimpleButton btnClear;
    }
}

