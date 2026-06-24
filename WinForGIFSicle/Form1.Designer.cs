namespace WinForGIFSicle
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAnimation();
                if (origImage != null) { origImage.Dispose(); origImage = null; }
                if (origStream != null) { origStream.Dispose(); origStream = null; }
                if (newImage != null) { newImage.Dispose(); newImage = null; }
                if (newStream != null) { newStream.Dispose(); newStream = null; }
                try { string tmp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "__gifsicle_preview__.gif"); if (System.IO.File.Exists(tmp)) System.IO.File.Delete(tmp); } catch { }
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            this.previewDebounceTimer = new System.Windows.Forms.Timer(this.components);

            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnClearTxt = new System.Windows.Forms.Button();
            this.labelHint = new System.Windows.Forms.Label();

            this.grpCompression = new System.Windows.Forms.GroupBox();
            this.btnSimple = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.lblScale = new System.Windows.Forms.Label(); this.numScale = new System.Windows.Forms.NumericUpDown(); this.btnResetScale = new System.Windows.Forms.Button(); this.lblScalePct = new System.Windows.Forms.Label();
            this.lblLossy = new System.Windows.Forms.Label(); this.numLossy = new System.Windows.Forms.NumericUpDown(); this.btnResetLossy = new System.Windows.Forms.Button();
            this.lblColors = new System.Windows.Forms.Label(); this.numColors = new System.Windows.Forms.NumericUpDown(); this.btnResetColors = new System.Windows.Forms.Button(); this.lblColorsHint = new System.Windows.Forms.Label();
            this.chkNoComments = new System.Windows.Forms.CheckBox(); this.chkNoExtensions = new System.Windows.Forms.CheckBox();
            this.lblOptLevel = new System.Windows.Forms.Label(); this.cmbOptLevel = new System.Windows.Forms.ComboBox();
            this.chkInterlace = new System.Windows.Forms.CheckBox(); this.chkNoWarnings = new System.Windows.Forms.CheckBox();
            this.lblLoopCount = new System.Windows.Forms.Label(); this.numLoopCount = new System.Windows.Forms.NumericUpDown(); this.lblLoopHint = new System.Windows.Forms.Label();
            this.lblCatFrame = new System.Windows.Forms.Label(); this.lblAdvDelay = new System.Windows.Forms.Label(); this.numDelay = new System.Windows.Forms.NumericUpDown(); this.lblAdvCs = new System.Windows.Forms.Label();
            this.lblAdvDispose = new System.Windows.Forms.Label(); this.cmbDispose = new System.Windows.Forms.ComboBox();
            this.btnResetAll = new System.Windows.Forms.Button();

            this.grpSave = new System.Windows.Forms.GroupBox();
            this.panelSaveDir = new System.Windows.Forms.Panel(); this.lblSaveTo = new System.Windows.Forms.Label();
            this.radOriginalDir = new System.Windows.Forms.RadioButton(); this.radDesktop = new System.Windows.Forms.RadioButton();
            this.radCustomDir = new System.Windows.Forms.RadioButton(); this.txtCustomDir = new System.Windows.Forms.TextBox(); this.btnBrowseDir = new System.Windows.Forms.Button();
            this.panelSaveName = new System.Windows.Forms.Panel(); this.lblFileName = new System.Windows.Forms.Label();
            this.radOrigName = new System.Windows.Forms.RadioButton(); this.radTimeStamp = new System.Windows.Forms.RadioButton();
            this.radCustomName = new System.Windows.Forms.RadioButton(); this.radReplaceName = new System.Windows.Forms.RadioButton(); this.radAppendName = new System.Windows.Forms.RadioButton(); this.txtCustomName = new System.Windows.Forms.TextBox();
            this.btnCompress = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();

            // 预览区控件
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblOrigTitle = new System.Windows.Forms.Label(); this.chkPreviewOrig = new System.Windows.Forms.CheckBox();
            this.picOriginal = new System.Windows.Forms.PictureBox(); this.lblOrigInfo = new System.Windows.Forms.Label();
            this.lblNewTitle = new System.Windows.Forms.Label(); this.chkPreviewNew = new System.Windows.Forms.CheckBox();
            this.picCompressed = new System.Windows.Forms.PictureBox(); this.lblNewInfo = new System.Windows.Forms.Label();
            this.btnPrevLeft = new System.Windows.Forms.Button(); this.btnNextLeft = new System.Windows.Forms.Button(); this.lblLeftIndex = new System.Windows.Forms.Label();
            this.btnPrevRight = new System.Windows.Forms.Button(); this.btnNextRight = new System.Windows.Forms.Button(); this.lblRightIndex = new System.Windows.Forms.Label();
            this.chkSync = new System.Windows.Forms.CheckBox();
            this.pnlPreviewLeft = new System.Windows.Forms.Panel();
            this.pnlPreviewRight = new System.Windows.Forms.Panel();

            this.grpResults = new System.Windows.Forms.GroupBox();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn(); this.colOrigSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewSize = new System.Windows.Forms.DataGridViewTextBoxColumn(); this.colRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.grpCompression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLossy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoopCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.grpSave.SuspendLayout();
            this.panelSaveDir.SuspendLayout();
            this.panelSaveName.SuspendLayout();
            this.grpPreview.SuspendLayout();
            this.pnlPreviewLeft.SuspendLayout();
            this.pnlPreviewRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCompressed)).BeginInit();
            this.grpResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.SuspendLayout();

            // ===== 布局常量 =====
            // 窗口 ClientSize: 900 x 720
            // 布局从上到下：文件选择区 → 压缩选项+保存选项(并排) → 压缩按钮+状态 → 预览区 → 结果区

            // ===== txtFilePath =====
            this.txtFilePath.Location = new System.Drawing.Point(12, 12);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFilePath.Size = new System.Drawing.Size(876, 60);
            this.txtFilePath.WordWrap = false;

            this.btnSelectFile.Location = new System.Drawing.Point(12, 78); this.btnSelectFile.Size = new System.Drawing.Size(80, 25); this.btnSelectFile.Text = "选择文件"; this.btnSelectFile.UseVisualStyleBackColor = true; this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            this.btnClearTxt.Location = new System.Drawing.Point(98, 78); this.btnClearTxt.Size = new System.Drawing.Size(80, 25); this.btnClearTxt.Text = "清空选择"; this.btnClearTxt.UseVisualStyleBackColor = true; this.btnClearTxt.Click += new System.EventHandler(this.btnClearTxt_Click);
            this.labelHint.AutoSize = true; this.labelHint.ForeColor = System.Drawing.SystemColors.Highlight; this.labelHint.Location = new System.Drawing.Point(190, 83); this.labelHint.Text = "*多个GIF可直接选择文件后，拖拽至程序即可";

            // ===== grpCompression =====
            this.grpCompression.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.btnSimple, this.btnAdvanced, this.btnResetAll,
                this.lblScale, this.numScale, this.btnResetScale, this.lblScalePct,
                this.lblLossy, this.numLossy, this.btnResetLossy,
                this.lblColors, this.numColors, this.btnResetColors, this.lblColorsHint,
                this.chkNoComments, this.chkNoExtensions,
                this.lblOptLevel, this.cmbOptLevel, this.chkInterlace, this.chkNoWarnings,
                this.lblLoopCount, this.numLoopCount, this.lblLoopHint,
                this.lblCatFrame, this.lblAdvDelay, this.numDelay, this.lblAdvCs, this.lblAdvDispose, this.cmbDispose});
            this.grpCompression.Location = new System.Drawing.Point(12, 110);
            this.grpCompression.Size = new System.Drawing.Size(440, 175);
            this.grpCompression.Text = "压缩选项";

            this.btnSimple.Location = new System.Drawing.Point(10, 18); this.btnSimple.Size = new System.Drawing.Size(55, 23); this.btnSimple.Text = "简单"; this.btnSimple.UseVisualStyleBackColor = true; this.btnSimple.Click += new System.EventHandler(this.btnSimple_Click);
            this.btnAdvanced.Location = new System.Drawing.Point(70, 18); this.btnAdvanced.Size = new System.Drawing.Size(55, 23); this.btnAdvanced.Text = "高级"; this.btnAdvanced.UseVisualStyleBackColor = true; this.btnAdvanced.Click += new System.EventHandler(this.btnAdvanced_Click);
            this.btnResetAll.Location = new System.Drawing.Point(345, 18); this.btnResetAll.Size = new System.Drawing.Size(85, 23); this.btnResetAll.Text = "全部重置"; this.btnResetAll.UseVisualStyleBackColor = true; this.btnResetAll.Click += new System.EventHandler(this.btnResetAll_Click);

            this.lblScale.AutoSize = true; this.lblScale.Location = new System.Drawing.Point(10, 52); this.lblScale.Text = "缩放比例";
            this.numScale.Location = new System.Drawing.Point(75, 50); this.numScale.Maximum = new decimal(new int[] { 500, 0, 0, 0 }); this.numScale.Minimum = new decimal(new int[] { 1, 0, 0, 0 }); this.numScale.Size = new System.Drawing.Size(50, 21); this.numScale.Value = new decimal(new int[] { 100, 0, 0, 0 }); this.numScale.ValueChanged += new System.EventHandler(this.SettingChanged);
            this.lblScalePct.AutoSize = true; this.lblScalePct.Location = new System.Drawing.Point(128, 52); this.lblScalePct.Text = "%";
            this.btnResetScale.Location = new System.Drawing.Point(145, 48); this.btnResetScale.Size = new System.Drawing.Size(40, 23); this.btnResetScale.Text = "重置"; this.btnResetScale.UseVisualStyleBackColor = true; this.btnResetScale.Click += new System.EventHandler(this.btnResetScale_Click);

            this.lblLossy.AutoSize = true; this.lblLossy.Location = new System.Drawing.Point(10, 82); this.lblLossy.Text = "Lossy值";
            this.numLossy.Location = new System.Drawing.Point(75, 80); this.numLossy.Maximum = new decimal(new int[] { 200, 0, 0, 0 }); this.numLossy.Size = new System.Drawing.Size(55, 21); this.numLossy.Value = new decimal(new int[] { 0, 0, 0, 0 }); this.numLossy.ValueChanged += new System.EventHandler(this.SettingChanged);
            this.btnResetLossy.Location = new System.Drawing.Point(137, 78); this.btnResetLossy.Size = new System.Drawing.Size(40, 23); this.btnResetLossy.Text = "重置"; this.btnResetLossy.UseVisualStyleBackColor = true; this.btnResetLossy.Click += new System.EventHandler(this.btnResetLossy_Click);

            this.lblColors.AutoSize = true; this.lblColors.Location = new System.Drawing.Point(10, 112); this.lblColors.Text = "色板限制";
            this.numColors.Location = new System.Drawing.Point(75, 110); this.numColors.Maximum = new decimal(new int[] { 256, 0, 0, 0 }); this.numColors.Minimum = new decimal(new int[] { 2, 0, 0, 0 }); this.numColors.Size = new System.Drawing.Size(55, 21); this.numColors.Value = new decimal(new int[] { 256, 0, 0, 0 }); this.numColors.ValueChanged += new System.EventHandler(this.SettingChanged);
            this.btnResetColors.Location = new System.Drawing.Point(137, 108); this.btnResetColors.Size = new System.Drawing.Size(40, 23); this.btnResetColors.Text = "重置"; this.btnResetColors.UseVisualStyleBackColor = true; this.btnResetColors.Click += new System.EventHandler(this.btnResetColors_Click);
            this.lblColorsHint.AutoSize = true; this.lblColorsHint.ForeColor = System.Drawing.SystemColors.GrayText; this.lblColorsHint.Location = new System.Drawing.Point(185, 112); this.lblColorsHint.Text = "(2-256)";

            this.chkNoComments.AutoSize = true; this.chkNoComments.Location = new System.Drawing.Point(12, 142); this.chkNoComments.Text = "去除注释"; this.chkNoComments.UseVisualStyleBackColor = true; this.chkNoComments.CheckedChanged += new System.EventHandler(this.SettingChanged);
            this.chkNoExtensions.AutoSize = true; this.chkNoExtensions.Location = new System.Drawing.Point(100, 142); this.chkNoExtensions.Text = "去除扩展"; this.chkNoExtensions.UseVisualStyleBackColor = true; this.chkNoExtensions.CheckedChanged += new System.EventHandler(this.SettingChanged);

            this.lblOptLevel.AutoSize = true; this.lblOptLevel.Location = new System.Drawing.Point(10, 52); this.lblOptLevel.Text = "优化级别"; this.lblOptLevel.Visible = false;
            this.cmbOptLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbOptLevel.Items.AddRange(new object[] { "O1", "O2", "O3" }); this.cmbOptLevel.Location = new System.Drawing.Point(75, 50); this.cmbOptLevel.Size = new System.Drawing.Size(55, 20); this.cmbOptLevel.Visible = false; this.cmbOptLevel.SelectedIndexChanged += new System.EventHandler(this.SettingChanged);
            this.chkInterlace.AutoSize = true; this.chkInterlace.Location = new System.Drawing.Point(10, 82); this.chkInterlace.Text = "隔行扫描"; this.chkInterlace.UseVisualStyleBackColor = true; this.chkInterlace.Visible = false; this.chkInterlace.CheckedChanged += new System.EventHandler(this.SettingChanged);
            this.chkNoWarnings.AutoSize = true; this.chkNoWarnings.Location = new System.Drawing.Point(100, 82); this.chkNoWarnings.Text = "无警告"; this.chkNoWarnings.UseVisualStyleBackColor = true; this.chkNoWarnings.Visible = false;
            this.lblLoopCount.AutoSize = true; this.lblLoopCount.Location = new System.Drawing.Point(10, 112); this.lblLoopCount.Text = "循环次数"; this.lblLoopCount.Visible = false;
            this.numLoopCount.Location = new System.Drawing.Point(75, 110); this.numLoopCount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 }); this.numLoopCount.Size = new System.Drawing.Size(55, 21); this.numLoopCount.Value = new decimal(new int[] { 0, 0, 0, 0 }); this.numLoopCount.Visible = false; this.numLoopCount.ValueChanged += new System.EventHandler(this.SettingChanged);
            this.lblLoopHint.AutoSize = true; this.lblLoopHint.ForeColor = System.Drawing.SystemColors.GrayText; this.lblLoopHint.Location = new System.Drawing.Point(135, 112); this.lblLoopHint.Text = "(0=无限)"; this.lblLoopHint.Visible = false;
            this.lblCatFrame.AutoSize = true; this.lblCatFrame.Font = new System.Drawing.Font("Microsoft YaHei", 8F, System.Drawing.FontStyle.Bold); this.lblCatFrame.Location = new System.Drawing.Point(10, 144); this.lblCatFrame.Text = "帧控制:"; this.lblCatFrame.Visible = false;
            this.lblAdvDelay.AutoSize = true; this.lblAdvDelay.Location = new System.Drawing.Point(80, 146); this.lblAdvDelay.Text = "帧延迟"; this.lblAdvDelay.Visible = false;
            this.numDelay.Location = new System.Drawing.Point(130, 143); this.numDelay.Maximum = new decimal(new int[] { 10000, 0, 0, 0 }); this.numDelay.Size = new System.Drawing.Size(50, 21); this.numDelay.Visible = false; this.numDelay.ValueChanged += new System.EventHandler(this.SettingChanged);
            this.lblAdvCs.AutoSize = true; this.lblAdvCs.Location = new System.Drawing.Point(183, 146); this.lblAdvCs.Text = "cs"; this.lblAdvCs.Visible = false;
            this.lblAdvDispose.AutoSize = true; this.lblAdvDispose.Location = new System.Drawing.Point(210, 146); this.lblAdvDispose.Text = "帧清除"; this.lblAdvDispose.Visible = false;
            this.cmbDispose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbDispose.Items.AddRange(new object[] { "不设置", "none", "background", "previous" }); this.cmbDispose.Location = new System.Drawing.Point(260, 143); this.cmbDispose.Size = new System.Drawing.Size(90, 20); this.cmbDispose.Visible = false; this.cmbDispose.SelectedIndexChanged += new System.EventHandler(this.SettingChanged);

            // ===== grpSave =====
            this.grpSave.Controls.AddRange(new System.Windows.Forms.Control[] { this.panelSaveDir, this.panelSaveName, this.radReplaceName, this.radAppendName, this.txtCustomName });
            this.grpSave.Location = new System.Drawing.Point(460, 110); this.grpSave.Size = new System.Drawing.Size(428, 135); this.grpSave.Text = "保存选项";

            this.panelSaveDir.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblSaveTo, this.radOriginalDir, this.radDesktop, this.radCustomDir, this.txtCustomDir, this.btnBrowseDir }); this.panelSaveDir.Location = new System.Drawing.Point(6, 18); this.panelSaveDir.Size = new System.Drawing.Size(416, 22);
            this.lblSaveTo.AutoSize = true; this.lblSaveTo.Location = new System.Drawing.Point(0, 3); this.lblSaveTo.Text = "保存至";
            this.radOriginalDir.AutoSize = true; this.radOriginalDir.Checked = true; this.radOriginalDir.Location = new System.Drawing.Point(48, 2); this.radOriginalDir.Text = "原目录"; this.radOriginalDir.UseVisualStyleBackColor = true; this.radOriginalDir.TabStop = true;
            this.radDesktop.AutoSize = true; this.radDesktop.Location = new System.Drawing.Point(112, 2); this.radDesktop.Text = "桌面"; this.radDesktop.UseVisualStyleBackColor = true;
            this.radCustomDir.AutoSize = true; this.radCustomDir.Location = new System.Drawing.Point(162, 2); this.radCustomDir.Text = "自选目录"; this.radCustomDir.UseVisualStyleBackColor = true; this.radCustomDir.CheckedChanged += new System.EventHandler(this.radCustomDir_CheckedChanged);
            this.txtCustomDir.Enabled = false; this.txtCustomDir.Location = new System.Drawing.Point(230, 0); this.txtCustomDir.Size = new System.Drawing.Size(155, 21);
            this.btnBrowseDir.Location = new System.Drawing.Point(390, 0); this.btnBrowseDir.Size = new System.Drawing.Size(25, 22); this.btnBrowseDir.Text = "..."; this.btnBrowseDir.UseVisualStyleBackColor = true; this.btnBrowseDir.Click += new System.EventHandler(this.btnBrowseDir_Click);

            this.panelSaveName.Controls.AddRange(new System.Windows.Forms.Control[] { this.lblFileName, this.radOrigName, this.radTimeStamp, this.radCustomName }); this.panelSaveName.Location = new System.Drawing.Point(6, 46); this.panelSaveName.Size = new System.Drawing.Size(416, 22);
            this.lblFileName.AutoSize = true; this.lblFileName.Location = new System.Drawing.Point(0, 3); this.lblFileName.Text = "文件名";
            this.radOrigName.AutoSize = true; this.radOrigName.Location = new System.Drawing.Point(48, 2); this.radOrigName.Text = "原文件名"; this.radOrigName.UseVisualStyleBackColor = true;
            this.radTimeStamp.AutoSize = true; this.radTimeStamp.Checked = true; this.radTimeStamp.Location = new System.Drawing.Point(118, 2); this.radTimeStamp.Text = "添加时间戳"; this.radTimeStamp.UseVisualStyleBackColor = true; this.radTimeStamp.TabStop = true;
            this.radCustomName.AutoSize = true; this.radCustomName.Location = new System.Drawing.Point(210, 2); this.radCustomName.Text = "自定义名称"; this.radCustomName.UseVisualStyleBackColor = true; this.radCustomName.CheckedChanged += new System.EventHandler(this.radCustomName_CheckedChanged);
            this.txtCustomName.Enabled = false; this.txtCustomName.Location = new System.Drawing.Point(10, 104); this.txtCustomName.Size = new System.Drawing.Size(200, 21);

            // "完全改名"和"增加名字"单选，默认隐藏，选自定义名称后显示
            this.radReplaceName.AutoSize = true; this.radReplaceName.Checked = true; this.radReplaceName.Location = new System.Drawing.Point(210, 74); this.radReplaceName.Text = "完全改名"; this.radReplaceName.UseVisualStyleBackColor = true; this.radReplaceName.Visible = false; this.radReplaceName.TabStop = true;
            this.radAppendName.AutoSize = true; this.radAppendName.Location = new System.Drawing.Point(290, 74); this.radAppendName.Text = "增加名字"; this.radAppendName.UseVisualStyleBackColor = true; this.radAppendName.Visible = false;

            // ===== btnCompress =====
            this.btnCompress.BackColor = System.Drawing.Color.FromArgb(0, 120, 215); this.btnCompress.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCompress.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold); this.btnCompress.ForeColor = System.Drawing.Color.White;
            this.btnCompress.Location = new System.Drawing.Point(460, 252); this.btnCompress.Size = new System.Drawing.Size(428, 28); this.btnCompress.Text = "开始压缩"; this.btnCompress.UseVisualStyleBackColor = false; this.btnCompress.Click += new System.EventHandler(this.btnCompress_Click);

            // ===== lblStatus =====
            this.lblStatus.Location = new System.Drawing.Point(460, 284); this.lblStatus.Size = new System.Drawing.Size(428, 28); this.lblStatus.Text = "就绪"; this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            // ===== grpPreview =====
            // 预览区: y=290, 高度=250, 导航按钮在底部 y=220(相对) 即绝对 y=510
            // 结果区从 y=560 开始，留 50px 给导航按钮+信息标签
            this.grpPreview.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblOrigTitle, this.chkPreviewOrig,
                this.lblNewTitle, this.chkPreviewNew,
                this.pnlPreviewLeft, this.pnlPreviewRight,
                this.btnPrevLeft, this.btnNextLeft, this.lblLeftIndex,
                this.btnPrevRight, this.btnNextRight, this.lblRightIndex,
                this.chkSync,
                this.lblOrigInfo, this.lblNewInfo});
            this.grpPreview.Location = new System.Drawing.Point(12, 320);
            this.grpPreview.Size = new System.Drawing.Size(876, 260);
            this.grpPreview.Text = "图片预览对比";

            // 左侧标题和动画开关
            this.lblOrigTitle.AutoSize = true; this.lblOrigTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold); this.lblOrigTitle.Location = new System.Drawing.Point(10, 18); this.lblOrigTitle.Text = "原图";
            this.chkPreviewOrig.AutoSize = true; this.chkPreviewOrig.Checked = true; this.chkPreviewOrig.CheckState = System.Windows.Forms.CheckState.Checked; this.chkPreviewOrig.Location = new System.Drawing.Point(50, 19); this.chkPreviewOrig.Text = "显示动画"; this.chkPreviewOrig.UseVisualStyleBackColor = true; this.chkPreviewOrig.CheckedChanged += new System.EventHandler(this.chkPreviewOrig_CheckedChanged);

            // 右侧标题和动画开关
            this.lblNewTitle.AutoSize = true; this.lblNewTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold); this.lblNewTitle.Location = new System.Drawing.Point(450, 18); this.lblNewTitle.Text = "压缩后";
            this.chkPreviewNew.AutoSize = true; this.chkPreviewNew.Checked = true; this.chkPreviewNew.CheckState = System.Windows.Forms.CheckState.Checked; this.chkPreviewNew.Location = new System.Drawing.Point(490, 19); this.chkPreviewNew.Text = "显示动画"; this.chkPreviewNew.UseVisualStyleBackColor = true; this.chkPreviewNew.CheckedChanged += new System.EventHandler(this.chkPreviewNew_CheckedChanged);

            // 同步切换 checkbox 放在原图"显示动画"后面，压缩后区域之前
            this.chkSync.AutoSize = true; this.chkSync.Checked = true; this.chkSync.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSync.Location = new System.Drawing.Point(155, 19); this.chkSync.Text = "同步切换"; this.chkSync.UseVisualStyleBackColor = true;

            // 左侧图片面板
            this.pnlPreviewLeft.Location = new System.Drawing.Point(5, 42); this.pnlPreviewLeft.Size = new System.Drawing.Size(415, 180);
            this.pnlPreviewLeft.Controls.Add(this.picOriginal);
            this.picOriginal.BackColor = System.Drawing.Color.FromArgb(240, 240, 240); this.picOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picOriginal.Dock = System.Windows.Forms.DockStyle.Fill; this.picOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; this.picOriginal.TabStop = false;
            this.picOriginal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOriginal.Click += new System.EventHandler(this.picOriginal_Click);

            // 右侧图片面板
            this.pnlPreviewRight.Location = new System.Drawing.Point(450, 42); this.pnlPreviewRight.Size = new System.Drawing.Size(415, 180);
            this.pnlPreviewRight.Controls.Add(this.picCompressed);
            this.picCompressed.BackColor = System.Drawing.Color.FromArgb(240, 240, 240); this.picCompressed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCompressed.Dock = System.Windows.Forms.DockStyle.Fill; this.picCompressed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom; this.picCompressed.TabStop = false;
            this.picCompressed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCompressed.Click += new System.EventHandler(this.picCompressed_Click);

            // 左侧导航按钮行 (y=228 相对 grpPreview)
            this.btnPrevLeft.Location = new System.Drawing.Point(5, 230); this.btnPrevLeft.Size = new System.Drawing.Size(28, 25); this.btnPrevLeft.Text = "<"; this.btnPrevLeft.UseVisualStyleBackColor = true; this.btnPrevLeft.Click += new System.EventHandler(this.btnPrevLeft_Click);
            this.lblLeftIndex.Location = new System.Drawing.Point(37, 233); this.lblLeftIndex.Size = new System.Drawing.Size(40, 16); this.lblLeftIndex.Text = "0/0"; this.lblLeftIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNextLeft.Location = new System.Drawing.Point(81, 230); this.btnNextLeft.Size = new System.Drawing.Size(28, 25); this.btnNextLeft.Text = ">"; this.btnNextLeft.UseVisualStyleBackColor = true; this.btnNextLeft.Click += new System.EventHandler(this.btnNextLeft_Click);
            this.lblOrigInfo.ForeColor = System.Drawing.SystemColors.GrayText; this.lblOrigInfo.Location = new System.Drawing.Point(115, 233); this.lblOrigInfo.Size = new System.Drawing.Size(300, 16); this.lblOrigInfo.Text = "请选择文件..."; this.lblOrigInfo.AutoEllipsis = true;

            // 右侧导航按钮行
            this.btnPrevRight.Location = new System.Drawing.Point(450, 230); this.btnPrevRight.Size = new System.Drawing.Size(28, 25); this.btnPrevRight.Text = "<"; this.btnPrevRight.UseVisualStyleBackColor = true; this.btnPrevRight.Click += new System.EventHandler(this.btnPrevRight_Click);
            this.lblRightIndex.Location = new System.Drawing.Point(482, 233); this.lblRightIndex.Size = new System.Drawing.Size(40, 16); this.lblRightIndex.Text = "0/0"; this.lblRightIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNextRight.Location = new System.Drawing.Point(526, 230); this.btnNextRight.Size = new System.Drawing.Size(28, 25); this.btnNextRight.Text = ">"; this.btnNextRight.UseVisualStyleBackColor = true; this.btnNextRight.Click += new System.EventHandler(this.btnNextRight_Click);
            this.lblNewInfo.ForeColor = System.Drawing.SystemColors.GrayText; this.lblNewInfo.Location = new System.Drawing.Point(560, 233); this.lblNewInfo.Size = new System.Drawing.Size(300, 16); this.lblNewInfo.Text = "等待压缩..."; this.lblNewInfo.AutoEllipsis = true;

            // ===== grpResults =====
            // 结果区从 y=560 开始，高度 150，到 y=710
            this.grpResults.Controls.Add(this.dgvResults);
            this.grpResults.Location = new System.Drawing.Point(12, 590);
            this.grpResults.Size = new System.Drawing.Size(876, 150);
            this.grpResults.Text = "压缩结果";
            this.dgvResults.AllowUserToAddRows = false; this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.BackgroundColor = System.Drawing.SystemColors.Window; this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.colFileName, this.colOrigSize, this.colNewSize, this.colRatio, this.colStatus });
            this.dgvResults.Location = new System.Drawing.Point(3, 18); this.dgvResults.ReadOnly = true; this.dgvResults.RowHeadersWidth = 25; this.dgvResults.Size = new System.Drawing.Size(870, 129);
            this.colFileName.HeaderText = "文件名"; this.colFileName.Name = "colFileName"; this.colFileName.Width = 300;
            this.colOrigSize.HeaderText = "原始"; this.colOrigSize.Name = "colOrigSize"; this.colOrigSize.Width = 90;
            this.colNewSize.HeaderText = "压缩后"; this.colNewSize.Name = "colNewSize"; this.colNewSize.Width = 90;
            this.colRatio.HeaderText = "压缩率"; this.colRatio.Name = "colRatio"; this.colRatio.Width = 90;
            this.colStatus.HeaderText = "状态"; this.colStatus.Name = "colStatus"; this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            // ===== timers =====
            this.animTimer.Interval = 80; this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            this.previewDebounceTimer.Interval = 500; this.previewDebounceTimer.Tick += new System.EventHandler(this.previewDebounceTimer_Tick);

            // ===== tooltips =====
            this.toolTip1.AutoPopDelay = 10000; this.toolTip1.InitialDelay = 300; this.toolTip1.ReshowDelay = 100;
            this.toolTip1.SetToolTip(this.cmbOptLevel, "O1快速/O2普通/O3最强"); this.toolTip1.SetToolTip(this.numScale, "缩放百分比，100=不缩放"); this.toolTip1.SetToolTip(this.btnResetScale, "重置为100%");
            this.toolTip1.SetToolTip(this.numLossy, "有损压缩，推荐30-80，0=无损"); this.toolTip1.SetToolTip(this.btnResetLossy, "重置为0");
            this.toolTip1.SetToolTip(this.numColors, "限制颜色数，256=不限"); this.toolTip1.SetToolTip(this.btnResetColors, "重置为256");
            this.toolTip1.SetToolTip(this.chkNoComments, "删除注释减小体积"); this.toolTip1.SetToolTip(this.chkNoExtensions, "删除扩展块减小体积");
            this.toolTip1.SetToolTip(this.chkInterlace, "逐行渐进显示"); this.toolTip1.SetToolTip(this.chkNoWarnings, "屏蔽gifsicle警告"); this.toolTip1.SetToolTip(this.numLoopCount, "0=无限循环");
            this.toolTip1.SetToolTip(this.btnResetAll, "重置所有压缩选项为默认值");
            this.toolTip1.SetToolTip(this.chkSync, "同步切换：勾选后左右两边同步切换图片");

            // ===== Form1 =====
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 760);
            this.MinimumSize = new System.Drawing.Size(680, 540); // 最小尺寸，允许缩小但不至于太挤
            this.Controls.Add(this.txtFilePath); this.Controls.Add(this.btnSelectFile); this.Controls.Add(this.btnClearTxt); this.Controls.Add(this.labelHint);
            this.Controls.Add(this.grpCompression); this.Controls.Add(this.grpSave);
            this.Controls.Add(this.btnCompress); this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.grpResults);
            // 允许窗口缩放，不锁定大小
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "Form1";
            this.Text = "GIFSicle GUI - GIF压缩工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.grpCompression.ResumeLayout(false); this.grpCompression.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.numLossy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numColors)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.numLoopCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.grpSave.ResumeLayout(false); this.grpSave.PerformLayout();
            this.panelSaveDir.ResumeLayout(false); this.panelSaveDir.PerformLayout();
            this.panelSaveName.ResumeLayout(false); this.panelSaveName.PerformLayout();
            this.grpPreview.ResumeLayout(false); this.grpPreview.PerformLayout();
            this.pnlPreviewLeft.ResumeLayout(false); this.pnlPreviewRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.picCompressed)).EndInit();
            this.grpResults.ResumeLayout(false); ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnClearTxt;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.GroupBox grpCompression;
        private System.Windows.Forms.Button btnSimple;
        private System.Windows.Forms.Button btnAdvanced;
        private System.Windows.Forms.Label lblScale;
        private System.Windows.Forms.NumericUpDown numScale;
        private System.Windows.Forms.Button btnResetScale;
        private System.Windows.Forms.Label lblScalePct;
        private System.Windows.Forms.Label lblLossy;
        private System.Windows.Forms.NumericUpDown numLossy;
        private System.Windows.Forms.Button btnResetLossy;
        private System.Windows.Forms.Label lblColors;
        private System.Windows.Forms.NumericUpDown numColors;
        private System.Windows.Forms.Button btnResetColors;
        private System.Windows.Forms.Label lblColorsHint;
        private System.Windows.Forms.CheckBox chkNoComments;
        private System.Windows.Forms.CheckBox chkNoExtensions;
        private System.Windows.Forms.Label lblOptLevel;
        private System.Windows.Forms.ComboBox cmbOptLevel;
        private System.Windows.Forms.CheckBox chkInterlace;
        private System.Windows.Forms.CheckBox chkNoWarnings;
        private System.Windows.Forms.Label lblLoopCount;
        private System.Windows.Forms.NumericUpDown numLoopCount;
        private System.Windows.Forms.Label lblLoopHint;
        private System.Windows.Forms.Label lblCatFrame;
        private System.Windows.Forms.Label lblAdvDelay;
        private System.Windows.Forms.NumericUpDown numDelay;
        private System.Windows.Forms.Label lblAdvCs;
        private System.Windows.Forms.Label lblAdvDispose;
        private System.Windows.Forms.ComboBox cmbDispose;
        private System.Windows.Forms.Button btnResetAll;
        private System.Windows.Forms.GroupBox grpSave;
        private System.Windows.Forms.Panel panelSaveDir;
        private System.Windows.Forms.Label lblSaveTo;
        private System.Windows.Forms.RadioButton radOriginalDir;
        private System.Windows.Forms.RadioButton radDesktop;
        private System.Windows.Forms.RadioButton radCustomDir;
        private System.Windows.Forms.TextBox txtCustomDir;
        private System.Windows.Forms.Button btnBrowseDir;
        private System.Windows.Forms.Panel panelSaveName;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.RadioButton radOrigName;
        private System.Windows.Forms.RadioButton radTimeStamp;
        private System.Windows.Forms.RadioButton radCustomName;
        private System.Windows.Forms.RadioButton radReplaceName;
        private System.Windows.Forms.RadioButton radAppendName;
        private System.Windows.Forms.TextBox txtCustomName;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.Panel pnlPreviewLeft;
        private System.Windows.Forms.Panel pnlPreviewRight;
        private System.Windows.Forms.Label lblOrigTitle;
        private System.Windows.Forms.CheckBox chkPreviewOrig;
        private System.Windows.Forms.PictureBox picOriginal;
        private System.Windows.Forms.Label lblOrigInfo;
        private System.Windows.Forms.Label lblNewTitle;
        private System.Windows.Forms.CheckBox chkPreviewNew;
        private System.Windows.Forms.PictureBox picCompressed;
        private System.Windows.Forms.Label lblNewInfo;
        private System.Windows.Forms.Button btnPrevLeft;
        private System.Windows.Forms.Button btnNextLeft;
        private System.Windows.Forms.Label lblLeftIndex;
        private System.Windows.Forms.Button btnPrevRight;
        private System.Windows.Forms.Button btnNextRight;
        private System.Windows.Forms.Label lblRightIndex;
        private System.Windows.Forms.CheckBox chkSync;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrigSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer animTimer;
        private System.Windows.Forms.Timer previewDebounceTimer;
    }
}
