namespace TFLabelTool
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            //this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.nextListBoxFile = new System.Windows.Forms.Button();
            this.preListBoxFile = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.clearBox = new System.Windows.Forms.Button();
            this.genFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDownImportHeight = new System.Windows.Forms.NumericUpDown();
            this.buttonOpenImageFolder = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonImport = new System.Windows.Forms.Button();
            this.listBoxLable = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();

            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.AdjustX = new System.Windows.Forms.NumericUpDown();
            this.AdjustY = new System.Windows.Forms.NumericUpDown();
            this.AdjustHeight = new System.Windows.Forms.NumericUpDown();
            this.AdjustWidth = new System.Windows.Forms.NumericUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportHeight)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustWidth)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 

            this.tabPage1.Controls.Add(this.nextListBoxFile);
            this.tabPage1.Controls.Add(this.preListBoxFile);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.reset);
            this.tabPage1.Controls.Add(this.clearBox);
            this.tabPage1.Controls.Add(this.genFile);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.numericUpDownImportHeight);
            this.tabPage1.Controls.Add(this.buttonOpenImageFolder);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.buttonImport);
            this.tabPage1.Controls.Add(this.listBoxLable);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.radioButton4);
            this.tabPage1.Controls.Add(this.checkBox5);
            this.tabPage1.Controls.Add(this.checkBox6);
            this.tabPage1.Controls.Add(this.checkBox7);
            this.tabPage1.Controls.Add(this.checkBox8);
            this.tabPage1.Controls.Add(this.checkBox9);
            this.tabPage1.Controls.Add(this.checkBox10);
            this.tabPage1.Controls.Add(this.listBoxFiles);
            this.tabPage1.Controls.Add(this.AdjustX);
            this.tabPage1.Controls.Add(this.AdjustY);
            this.tabPage1.Controls.Add(this.AdjustHeight);
            this.tabPage1.Controls.Add(this.AdjustWidth);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1184, 523);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "标注";
            this.tabPage1.UseVisualStyleBackColor = true;


            // 
            // nextListBoxFile
            // 
            this.nextListBoxFile.Location = new System.Drawing.Point(172, 23);
            this.nextListBoxFile.Name = "nextListBoxFile";
            this.nextListBoxFile.Size = new System.Drawing.Size(46, 19);
            this.nextListBoxFile.TabIndex = 53;
            this.nextListBoxFile.Text = "next";
            this.nextListBoxFile.UseVisualStyleBackColor = true;
            this.nextListBoxFile.Click += new System.EventHandler(this.nextListBoxFile_Click);
            // 
            // preListBoxFile
            // 
            this.preListBoxFile.Location = new System.Drawing.Point(120, 22);
            this.preListBoxFile.Name = "preListBoxFile";
            this.preListBoxFile.Size = new System.Drawing.Size(46, 19);
            this.preListBoxFile.TabIndex = 52;
            this.preListBoxFile.Text = "pre";
            this.preListBoxFile.UseVisualStyleBackColor = true;
            this.preListBoxFile.Click += new System.EventHandler(this.preListBoxFile_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1085, 22);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(43, 21);
            this.button3.TabIndex = 41;
            this.button3.Text = "缩小";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.reduce_Click);
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(1133, 22);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(43, 21);
            this.reset.TabIndex = 40;
            this.reset.Text = "重置";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            //
            //clear Box 清除框选
            //
            this.clearBox.Location = new System.Drawing.Point(1181, 22);
            this.clearBox.Name = "clearBox";
            this.clearBox.Size = new System.Drawing.Size(43, 21);
            this.clearBox.TabIndex = 40;
            this.clearBox.Text = "去框";
            this.clearBox.UseVisualStyleBackColor = true;
            this.clearBox.Click += new System.EventHandler(this.clearBox_Click);
            //
            //genFile 生成文件
            //
            this.genFile.Location = new System.Drawing.Point(1229, 22);
            this.genFile.Name = "genFile";
            this.genFile.Size = new System.Drawing.Size(86, 21);
            this.genFile.TabIndex = 40;
            this.genFile.Text = "文件生成";
            this.genFile.UseVisualStyleBackColor = true;
            this.genFile.Visible = false;
            this.genFile.Click += new System.EventHandler(this.genFile_Click);

            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(550, 21);
            this.label8.TabIndex = 59;
            this.label8.Text = "提示：上/下/左/右键选择图片 S/Z/X/C键控制标框大小 8/4/5/6(小键盘)键控制标框位置 Q/W控制标框旋转";
            this.label8.Visible = true;

            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1036, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 21);
            this.button1.TabIndex = 39;
            this.button1.Text = "放大";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.enlarge_Click);
            // 
            // numericUpDownImportHeight
            // 
            this.numericUpDownImportHeight.Location = new System.Drawing.Point(825, 19);
            this.numericUpDownImportHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownImportHeight.Name = "numericUpDownImportHeight";
            this.numericUpDownImportHeight.Size = new System.Drawing.Size(45, 21);
            this.numericUpDownImportHeight.TabIndex = 28;
            this.numericUpDownImportHeight.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownImportHeight.Visible = false;
            // 
            // buttonOpenImageFolder
            // 
            this.buttonOpenImageFolder.Location = new System.Drawing.Point(4, 22);
            this.buttonOpenImageFolder.Name = "buttonOpenImageFolder";
            this.buttonOpenImageFolder.Size = new System.Drawing.Size(110, 19);
            this.buttonOpenImageFolder.TabIndex = 26;
            this.buttonOpenImageFolder.Text = "打开图片文件夹";
            this.buttonOpenImageFolder.UseVisualStyleBackColor = true;
            this.buttonOpenImageFolder.Click += new System.EventHandler(this.buttonOpenImageFolder_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1034, 312);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "标注信息";
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(985, 23);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(43, 21);
            this.buttonImport.TabIndex = 21;
            this.buttonImport.Text = "open";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);

            // 
            // listBoxLable
            // 
            this.listBoxLable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxLable.FormattingEnabled = true;
            this.listBoxLable.HorizontalScrollbar = true;
            this.listBoxLable.ItemHeight = 12;
            this.listBoxLable.Location = new System.Drawing.Point(1036, 327);
            this.listBoxLable.Name = "listBoxLable";
            this.listBoxLable.Size = new System.Drawing.Size(142, 184);
            this.listBoxLable.TabIndex = 5;
            this.listBoxLable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxLable_MouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(230, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 459);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(30, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(30, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(745, 333);
            this.label7.TabIndex = 1;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(1039, 45);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(59, 16);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.TabStop = true;
            this.checkBox1.Text = "无遮挡";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkbox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(1039, 67);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(71, 16);
            this.checkBox2.TabIndex = 42;
            this.checkBox2.TabStop = true;
            this.checkBox2.Text = "同队遮挡";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(1039, 89);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(71, 16);
            this.checkBox3.TabIndex = 43;
            this.checkBox3.TabStop = true;
            this.checkBox3.Text = "异队遮挡";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(1039, 212);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(71, 16);
            this.radioButton4.TabIndex = 44;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "多人遮挡";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Visible = false;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            
            // checkBox5
            this.checkBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(1039, 111);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(71, 16);
            this.checkBox5.TabIndex = 44;
            this.checkBox5.TabStop = true;
            this.checkBox5.Text = "部分遮挡";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);

            // checkBox6
            this.checkBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(1039, 133);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(71, 16);
            this.checkBox6.TabIndex = 44;
            this.checkBox6.TabStop = true;
            this.checkBox6.Text = "完全遮挡";
            this.checkBox6.UseVisualStyleBackColor = true;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);

            // checkBox7
            this.checkBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(1039, 155);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(71, 16);
            this.checkBox7.TabIndex = 44;
            this.checkBox7.TabStop = true;
            this.checkBox7.Text = "运动模糊";
            this.checkBox7.UseVisualStyleBackColor = true;
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);

            // checkBox8
            this.checkBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(1039, 177);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(71, 16);
            this.checkBox8.TabIndex = 44;
            this.checkBox8.TabStop = true;
            this.checkBox8.Text = "球员旋转";
            this.checkBox8.UseVisualStyleBackColor = true;
            this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);

            // checkBox9
            this.checkBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox9.AutoSize = true;
            this.checkBox9.Location = new System.Drawing.Point(1039, 199);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(71, 16);
            this.checkBox9.TabIndex = 44;
            this.checkBox9.TabStop = true;
            this.checkBox9.Text = "球员形变";
            this.checkBox9.UseVisualStyleBackColor = true;
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);

            // checkBox10
            this.checkBox10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(1039, 221);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(71, 16);
            this.checkBox10.TabIndex = 44;
            this.checkBox10.TabStop = true;
            this.checkBox10.Text = "球员消失";
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged);

            // 
            // listBoxFiles
            // 
            this.listBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.HorizontalScrollbar = true;
            this.listBoxFiles.ItemHeight = 12;
            this.listBoxFiles.Location = new System.Drawing.Point(8, 49);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(216, 460);
            this.listBoxFiles.TabIndex = 2;
            this.listBoxFiles.SelectedIndexChanged += new System.EventHandler(this.listBoxFiles_SelectedIndexChanged);
            //this.listBoxFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxFiles_KeyUp);
            

            // 
            // AdjustX
            // 
            this.AdjustX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjustX.Location = new System.Drawing.Point(1036, 64);
            this.AdjustX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AdjustX.Name = "AdjustX";
            this.AdjustX.Size = new System.Drawing.Size(45, 21);
            this.AdjustX.TabIndex = 48;
            this.AdjustX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AdjustX.Visible = false;
            this.AdjustX.ValueChanged += new System.EventHandler(this.AdjustX_ValueChanged);
            // 
            // AdjustY
            // 
            this.AdjustY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjustY.Location = new System.Drawing.Point(1036, 91);
            this.AdjustY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AdjustY.Name = "AdjustY";
            this.AdjustY.Size = new System.Drawing.Size(45, 21);
            this.AdjustY.TabIndex = 51;
            this.AdjustY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AdjustY.Visible = false;
            this.AdjustY.ValueChanged += new System.EventHandler(this.AdjustY_ValueChanged);
            // 
            // AdjustHeight
            // 
            this.AdjustHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjustHeight.Location = new System.Drawing.Point(1036, 118);
            this.AdjustHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AdjustHeight.Name = "AdjustHeight";
            this.AdjustHeight.Size = new System.Drawing.Size(45, 21);
            this.AdjustHeight.TabIndex = 50;
            this.AdjustHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AdjustHeight.Visible = false;
            this.AdjustHeight.ValueChanged += new System.EventHandler(this.AdjustHeight_ValueChanged);
            // 
            // AdjustWidth
            // 
            this.AdjustWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdjustWidth.Location = new System.Drawing.Point(1036, 146);
            this.AdjustWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.AdjustWidth.Name = "AdjustWidth";
            this.AdjustWidth.Size = new System.Drawing.Size(45, 21);
            this.AdjustWidth.TabIndex = 49;
            this.AdjustWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AdjustWidth.Visible = false;
            this.AdjustWidth.ValueChanged += new System.EventHandler(this.AdjustWidth_ValueChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1192, 549);
            this.tabControl1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 549);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.Text = "TrackingPlayer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportHeight)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AdjustWidth)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonOpenImageFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.ListBox listBoxLable;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.NumericUpDown numericUpDownImportHeight;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.Button clearBox; // 去框
        private System.Windows.Forms.Button genFile; // 生成两个目标文件
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox1; // 无遮挡
        private System.Windows.Forms.CheckBox checkBox2; // 同队遮挡
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.CheckBox checkBox3; // 异队遮挡
        private System.Windows.Forms.CheckBox checkBox5; // 部分遮挡
        private System.Windows.Forms.CheckBox checkBox6; // 完全遮挡
        private System.Windows.Forms.CheckBox checkBox7; // 运动模糊
        private System.Windows.Forms.CheckBox checkBox8; // 球员旋转
        private System.Windows.Forms.CheckBox checkBox9; // 球员形变
        private System.Windows.Forms.CheckBox checkBox10; // 球员从画面边缘消失
        private System.Windows.Forms.NumericUpDown AdjustX;
        private System.Windows.Forms.NumericUpDown AdjustY;
        private System.Windows.Forms.NumericUpDown AdjustHeight;
        private System.Windows.Forms.NumericUpDown AdjustWidth;
        private System.Windows.Forms.Button nextListBoxFile;
        private System.Windows.Forms.Button preListBoxFile;
        private System.Windows.Forms.Label label8;
    }
}