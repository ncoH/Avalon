using MediaPortal.Configuration;

namespace ProcessPlugins.AvalonEditor
{
    partial class AvalonConfig
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
            this.btSave = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tbEditorStartParamsOutput = new System.Windows.Forms.TextBox();
            this.add_submenu_button = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.update_submenu_button = new System.Windows.Forms.Button();
            this.used_list_submenu = new System.Windows.Forms.ListBox();
            this.remove_submenu_button = new System.Windows.Forms.Button();
            this.down_submenu_button = new System.Windows.Forms.Button();
            this.up_submenu_button = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.add_button = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_test = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tbEditorSearchExpression = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbEditorLayout = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbEditorViewValues = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbEditorViews = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbEditorConfigs = new System.Windows.Forms.ComboBox();
            this.lblMovieCategories = new System.Windows.Forms.Label();
            this.movPicsCategoryCombo = new Cornerstone.GUI.Controls.FilterComboBox();
            this.rB_Recordings = new System.Windows.Forms.RadioButton();
            this.rB_Music = new System.Windows.Forms.RadioButton();
            this.rB_Series = new System.Windows.Forms.RadioButton();
            this.rB_Movies = new System.Windows.Forms.RadioButton();
            this.cB_RecentMedia = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bt_browse = new System.Windows.Forms.Button();
            this.cB_FanartHandler = new System.Windows.Forms.CheckBox();
            this.cB_singleImage = new System.Windows.Forms.CheckBox();
            this.new_bgimage = new System.Windows.Forms.ComboBox();
            this.combo_FanartHandler = new System.Windows.Forms.ComboBox();
            this.cB_onlinevideosOption = new System.Windows.Forms.CheckBox();
            this.lblParameter = new System.Windows.Forms.Label();
            this.cboParameterViews = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.new_name = new System.Windows.Forms.TextBox();
            this.new_windowid = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.update_button = new System.Windows.Forms.Button();
            this.used_list = new System.Windows.Forms.ListBox();
            this.remove_button = new System.Windows.Forms.Button();
            this.down_button = new System.Windows.Forms.Button();
            this.up_button = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.avail_list = new System.Windows.Forms.ListBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.rB_MyFilms = new System.Windows.Forms.RadioButton();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Location = new System.Drawing.Point(766, 585);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 8;
            this.btSave.Text = "Save";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.Location = new System.Drawing.Point(847, 585);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 18;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 615);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(941, 22);
            this.statusStrip1.TabIndex = 26;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(0, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(922, 569);
            this.tabControl1.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbEditorStartParamsOutput);
            this.tabPage1.Controls.Add(this.add_submenu_button);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.groupBox6);
            this.tabPage1.Controls.Add(this.add_button);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(914, 543);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "BasicHome menu";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbEditorStartParamsOutput
            // 
            this.tbEditorStartParamsOutput.Location = new System.Drawing.Point(552, 428);
            this.tbEditorStartParamsOutput.Name = "tbEditorStartParamsOutput";
            this.tbEditorStartParamsOutput.Size = new System.Drawing.Size(248, 20);
            this.tbEditorStartParamsOutput.TabIndex = 36;
            this.tbEditorStartParamsOutput.Visible = false;
            // 
            // add_submenu_button
            // 
            this.add_submenu_button.Location = new System.Drawing.Point(537, 330);
            this.add_submenu_button.Name = "add_submenu_button";
            this.add_submenu_button.Size = new System.Drawing.Size(119, 23);
            this.add_submenu_button.TabIndex = 35;
            this.add_submenu_button.Text = "Add to Submenu >>";
            this.add_submenu_button.UseVisualStyleBackColor = true;
            this.add_submenu_button.Click += new System.EventHandler(this.btnSubAdd_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.update_submenu_button);
            this.groupBox7.Controls.Add(this.used_list_submenu);
            this.groupBox7.Controls.Add(this.remove_submenu_button);
            this.groupBox7.Controls.Add(this.down_submenu_button);
            this.groupBox7.Controls.Add(this.up_submenu_button);
            this.groupBox7.Location = new System.Drawing.Point(662, 205);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(246, 189);
            this.groupBox7.TabIndex = 34;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Modules in Submenu";
            // 
            // update_submenu_button
            // 
            this.update_submenu_button.Location = new System.Drawing.Point(172, 132);
            this.update_submenu_button.Name = "update_submenu_button";
            this.update_submenu_button.Size = new System.Drawing.Size(68, 23);
            this.update_submenu_button.TabIndex = 26;
            this.update_submenu_button.Text = "UPDATE";
            this.update_submenu_button.UseVisualStyleBackColor = true;
            this.update_submenu_button.Click += new System.EventHandler(this.btnSubUpdate_Click);
            // 
            // used_list_submenu
            // 
            this.used_list_submenu.Enabled = false;
            this.used_list_submenu.FormattingEnabled = true;
            this.used_list_submenu.Location = new System.Drawing.Point(15, 19);
            this.used_list_submenu.Name = "used_list_submenu";
            this.used_list_submenu.Size = new System.Drawing.Size(151, 147);
            this.used_list_submenu.TabIndex = 4;
            this.used_list_submenu.SelectedIndexChanged += new System.EventHandler(this.used_list_submenu_SelectedIndexChanged);
            // 
            // remove_submenu_button
            // 
            this.remove_submenu_button.Location = new System.Drawing.Point(172, 103);
            this.remove_submenu_button.Name = "remove_submenu_button";
            this.remove_submenu_button.Size = new System.Drawing.Size(68, 23);
            this.remove_submenu_button.TabIndex = 25;
            this.remove_submenu_button.Text = "REMOVE";
            this.remove_submenu_button.UseVisualStyleBackColor = true;
            this.remove_submenu_button.Click += new System.EventHandler(this.btnSubRemove_Click);
            // 
            // down_submenu_button
            // 
            this.down_submenu_button.Location = new System.Drawing.Point(172, 74);
            this.down_submenu_button.Name = "down_submenu_button";
            this.down_submenu_button.Size = new System.Drawing.Size(68, 23);
            this.down_submenu_button.TabIndex = 3;
            this.down_submenu_button.Text = "DOWN";
            this.down_submenu_button.UseVisualStyleBackColor = true;
            this.down_submenu_button.Click += new System.EventHandler(this.btSubMoveDown_Click);
            // 
            // up_submenu_button
            // 
            this.up_submenu_button.Location = new System.Drawing.Point(172, 45);
            this.up_submenu_button.Name = "up_submenu_button";
            this.up_submenu_button.Size = new System.Drawing.Size(68, 23);
            this.up_submenu_button.TabIndex = 2;
            this.up_submenu_button.Text = "UP";
            this.up_submenu_button.UseVisualStyleBackColor = true;
            this.up_submenu_button.Click += new System.EventHandler(this.btSubMoveUp_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.radioButton3);
            this.groupBox6.Controls.Add(this.radioButton1);
            this.groupBox6.Controls.Add(this.radioButton2);
            this.groupBox6.Location = new System.Drawing.Point(5, 348);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(231, 86);
            this.groupBox6.TabIndex = 29;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Select modules";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(15, 63);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(147, 17);
            this.radioButton3.TabIndex = 3;
            this.radioButton3.Text = "Show only plugin-modules";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.rBut3_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(15, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(86, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Show all files";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.rBut1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(15, 40);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(145, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Show only home-modules";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.rBut2_CheckedChanged);
            // 
            // add_button
            // 
            this.add_button.Location = new System.Drawing.Point(537, 135);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(119, 23);
            this.add_button.TabIndex = 33;
            this.add_button.Text = "Add to Mainmenu >>";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rB_MyFilms);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.lblMovieCategories);
            this.groupBox4.Controls.Add(this.movPicsCategoryCombo);
            this.groupBox4.Controls.Add(this.rB_Recordings);
            this.groupBox4.Controls.Add(this.rB_Music);
            this.groupBox4.Controls.Add(this.rB_Series);
            this.groupBox4.Controls.Add(this.rB_Movies);
            this.groupBox4.Controls.Add(this.cB_RecentMedia);
            this.groupBox4.Controls.Add(this.groupBox2);
            this.groupBox4.Controls.Add(this.cB_onlinevideosOption);
            this.groupBox4.Controls.Add(this.lblParameter);
            this.groupBox4.Controls.Add(this.cboParameterViews);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.new_name);
            this.groupBox4.Controls.Add(this.new_windowid);
            this.groupBox4.Location = new System.Drawing.Point(242, 10);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(289, 526);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Add selected module to Menu";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_test);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.tbEditorSearchExpression);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.cbEditorLayout);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.cbEditorViewValues);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.cbEditorViews);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.cbEditorConfigs);
            this.groupBox5.Location = new System.Drawing.Point(9, 327);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(274, 193);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "MyFilms Start Parameter";
            // 
            // button_test
            // 
            this.button_test.Location = new System.Drawing.Point(6, 164);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(262, 23);
            this.button_test.TabIndex = 37;
            this.button_test.Text = "Create Parameter";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(150, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Search Expression";
            // 
            // tbEditorSearchExpression
            // 
            this.tbEditorSearchExpression.Location = new System.Drawing.Point(146, 135);
            this.tbEditorSearchExpression.Name = "tbEditorSearchExpression";
            this.tbEditorSearchExpression.Size = new System.Drawing.Size(121, 20);
            this.tbEditorSearchExpression.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(150, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Layout";
            // 
            // cbEditorLayout
            // 
            this.cbEditorLayout.FormattingEnabled = true;
            this.cbEditorLayout.Items.AddRange(new object[] {
            "List View",
            "Small Thumbs",
            "Big Thumbs",
            "Filmstrip",
            "CoverFlow"});
            this.cbEditorLayout.Location = new System.Drawing.Point(146, 40);
            this.cbEditorLayout.Name = "cbEditorLayout";
            this.cbEditorLayout.Size = new System.Drawing.Size(121, 21);
            this.cbEditorLayout.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Value to Filter Movies";
            // 
            // cbEditorViewValues
            // 
            this.cbEditorViewValues.FormattingEnabled = true;
            this.cbEditorViewValues.Location = new System.Drawing.Point(6, 135);
            this.cbEditorViewValues.Name = "cbEditorViewValues";
            this.cbEditorViewValues.Size = new System.Drawing.Size(121, 21);
            this.cbEditorViewValues.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Select View";
            // 
            // cbEditorViews
            // 
            this.cbEditorViews.FormattingEnabled = true;
            this.cbEditorViews.Location = new System.Drawing.Point(6, 85);
            this.cbEditorViews.Name = "cbEditorViews";
            this.cbEditorViews.Size = new System.Drawing.Size(121, 21);
            this.cbEditorViews.TabIndex = 10;
            this.cbEditorViews.SelectedIndexChanged += new System.EventHandler(this.cbEditorViews_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Select Config";
            // 
            // cbEditorConfigs
            // 
            this.cbEditorConfigs.FormattingEnabled = true;
            this.cbEditorConfigs.Location = new System.Drawing.Point(6, 40);
            this.cbEditorConfigs.Name = "cbEditorConfigs";
            this.cbEditorConfigs.Size = new System.Drawing.Size(121, 21);
            this.cbEditorConfigs.TabIndex = 7;
            this.cbEditorConfigs.SelectedIndexChanged += new System.EventHandler(this.cbEditorConfigs_SelectedIndexChanged);
            // 
            // lblMovieCategories
            // 
            this.lblMovieCategories.AutoSize = true;
            this.lblMovieCategories.Location = new System.Drawing.Point(6, 110);
            this.lblMovieCategories.Name = "lblMovieCategories";
            this.lblMovieCategories.Size = new System.Drawing.Size(52, 13);
            this.lblMovieCategories.TabIndex = 42;
            this.lblMovieCategories.Text = "Category:";
            // 
            // movPicsCategoryCombo
            // 
            this.movPicsCategoryCombo.DropDownHeight = 200;
            this.movPicsCategoryCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.movPicsCategoryCombo.FormattingEnabled = true;
            this.movPicsCategoryCombo.IntegralHeight = false;
            this.movPicsCategoryCombo.Location = new System.Drawing.Point(70, 107);
            this.movPicsCategoryCombo.Name = "movPicsCategoryCombo";
            this.movPicsCategoryCombo.RestrictSelectionToLeafNodes = false;
            this.movPicsCategoryCombo.Size = new System.Drawing.Size(159, 21);
            this.movPicsCategoryCombo.TabIndex = 30;
            this.movPicsCategoryCombo.DropDown += new System.EventHandler(this.movPicsCategoryCombo_DropDown);
            // 
            // rB_Recordings
            // 
            this.rB_Recordings.AutoSize = true;
            this.rB_Recordings.Location = new System.Drawing.Point(109, 304);
            this.rB_Recordings.Name = "rB_Recordings";
            this.rB_Recordings.Size = new System.Drawing.Size(79, 17);
            this.rB_Recordings.TabIndex = 41;
            this.rB_Recordings.TabStop = true;
            this.rB_Recordings.Text = "Recordings";
            this.rB_Recordings.UseVisualStyleBackColor = true;
            // 
            // rB_Music
            // 
            this.rB_Music.AutoSize = true;
            this.rB_Music.Location = new System.Drawing.Point(39, 304);
            this.rB_Music.Name = "rB_Music";
            this.rB_Music.Size = new System.Drawing.Size(53, 17);
            this.rB_Music.TabIndex = 40;
            this.rB_Music.TabStop = true;
            this.rB_Music.Text = "Music";
            this.rB_Music.UseVisualStyleBackColor = true;
            // 
            // rB_Series
            // 
            this.rB_Series.AutoSize = true;
            this.rB_Series.Location = new System.Drawing.Point(109, 280);
            this.rB_Series.Name = "rB_Series";
            this.rB_Series.Size = new System.Drawing.Size(71, 17);
            this.rB_Series.TabIndex = 39;
            this.rB_Series.TabStop = true;
            this.rB_Series.Text = "TV Series";
            this.rB_Series.UseVisualStyleBackColor = true;
            // 
            // rB_Movies
            // 
            this.rB_Movies.AutoSize = true;
            this.rB_Movies.Location = new System.Drawing.Point(39, 280);
            this.rB_Movies.Name = "rB_Movies";
            this.rB_Movies.Size = new System.Drawing.Size(59, 17);
            this.rB_Movies.TabIndex = 38;
            this.rB_Movies.TabStop = true;
            this.rB_Movies.Text = "Movies";
            this.rB_Movies.UseVisualStyleBackColor = true;
            // 
            // cB_RecentMedia
            // 
            this.cB_RecentMedia.AutoSize = true;
            this.cB_RecentMedia.Location = new System.Drawing.Point(21, 260);
            this.cB_RecentMedia.Name = "cB_RecentMedia";
            this.cB_RecentMedia.Size = new System.Drawing.Size(203, 17);
            this.cB_RecentMedia.TabIndex = 36;
            this.cB_RecentMedia.Text = "Enable Latest Media for selected item";
            this.cB_RecentMedia.UseVisualStyleBackColor = true;
            this.cB_RecentMedia.CheckedChanged += new System.EventHandler(this.cB_RecentMedia_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.bt_browse);
            this.groupBox2.Controls.Add(this.cB_FanartHandler);
            this.groupBox2.Controls.Add(this.cB_singleImage);
            this.groupBox2.Controls.Add(this.new_bgimage);
            this.groupBox2.Controls.Add(this.combo_FanartHandler);
            this.groupBox2.Location = new System.Drawing.Point(9, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(274, 100);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Background Options";
            // 
            // bt_browse
            // 
            this.bt_browse.Location = new System.Drawing.Point(186, 42);
            this.bt_browse.Name = "bt_browse";
            this.bt_browse.Size = new System.Drawing.Size(82, 23);
            this.bt_browse.TabIndex = 9;
            this.bt_browse.Text = "browse";
            this.bt_browse.UseVisualStyleBackColor = true;
            this.bt_browse.Click += new System.EventHandler(this.bt_browse_Click);
            // 
            // cB_FanartHandler
            // 
            this.cB_FanartHandler.AutoSize = true;
            this.cB_FanartHandler.Location = new System.Drawing.Point(12, 69);
            this.cB_FanartHandler.Name = "cB_FanartHandler";
            this.cB_FanartHandler.Size = new System.Drawing.Size(115, 17);
            this.cB_FanartHandler.TabIndex = 8;
            this.cB_FanartHandler.Text = "Use FanartHandler";
            this.cB_FanartHandler.UseVisualStyleBackColor = true;
            this.cB_FanartHandler.CheckedChanged += new System.EventHandler(this.cB_FanartHandler_CheckedChanged);
            // 
            // cB_singleImage
            // 
            this.cB_singleImage.AutoSize = true;
            this.cB_singleImage.Location = new System.Drawing.Point(12, 19);
            this.cB_singleImage.Name = "cB_singleImage";
            this.cB_singleImage.Size = new System.Drawing.Size(109, 17);
            this.cB_singleImage.TabIndex = 7;
            this.cB_singleImage.Text = "Use Single Image";
            this.cB_singleImage.UseVisualStyleBackColor = true;
            this.cB_singleImage.CheckedChanged += new System.EventHandler(this.cB_singleImage_CheckedChanged);
            // 
            // new_bgimage
            // 
            this.new_bgimage.FormattingEnabled = true;
            this.new_bgimage.Location = new System.Drawing.Point(6, 42);
            this.new_bgimage.Name = "new_bgimage";
            this.new_bgimage.Size = new System.Drawing.Size(165, 21);
            this.new_bgimage.TabIndex = 6;
            // 
            // combo_FanartHandler
            // 
            this.combo_FanartHandler.FormattingEnabled = true;
            this.combo_FanartHandler.Items.AddRange(new object[] {
            "games",
            "movies",
            "music",
            "pictures",
            "plugins",
            "scorecenter",
            "tv",
            "series"});
            this.combo_FanartHandler.Location = new System.Drawing.Point(15, 222);
            this.combo_FanartHandler.Name = "combo_FanartHandler";
            this.combo_FanartHandler.Size = new System.Drawing.Size(135, 21);
            this.combo_FanartHandler.TabIndex = 9;
            // 
            // cB_onlinevideosOption
            // 
            this.cB_onlinevideosOption.AutoSize = true;
            this.cB_onlinevideosOption.Location = new System.Drawing.Point(109, 107);
            this.cB_onlinevideosOption.Name = "cB_onlinevideosOption";
            this.cB_onlinevideosOption.Size = new System.Drawing.Size(120, 17);
            this.cB_onlinevideosOption.TabIndex = 37;
            this.cB_onlinevideosOption.Text = "Return to root menu";
            this.cB_onlinevideosOption.UseVisualStyleBackColor = true;
            // 
            // lblParameter
            // 
            this.lblParameter.AutoSize = true;
            this.lblParameter.Location = new System.Drawing.Point(6, 84);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(58, 13);
            this.lblParameter.TabIndex = 9;
            this.lblParameter.Text = "Parameter:";
            // 
            // cboParameterViews
            // 
            this.cboParameterViews.FormattingEnabled = true;
            this.cboParameterViews.Location = new System.Drawing.Point(70, 81);
            this.cboParameterViews.Name = "cboParameterViews";
            this.cboParameterViews.Size = new System.Drawing.Size(159, 21);
            this.cboParameterViews.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Window ID:";
            // 
            // new_name
            // 
            this.new_name.Location = new System.Drawing.Point(94, 55);
            this.new_name.Name = "new_name";
            this.new_name.Size = new System.Drawing.Size(135, 20);
            this.new_name.TabIndex = 2;
            // 
            // new_windowid
            // 
            this.new_windowid.Location = new System.Drawing.Point(94, 26);
            this.new_windowid.Name = "new_windowid";
            this.new_windowid.ReadOnly = true;
            this.new_windowid.Size = new System.Drawing.Size(135, 20);
            this.new_windowid.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.update_button);
            this.groupBox3.Controls.Add(this.used_list);
            this.groupBox3.Controls.Add(this.remove_button);
            this.groupBox3.Controls.Add(this.down_button);
            this.groupBox3.Controls.Add(this.up_button);
            this.groupBox3.Location = new System.Drawing.Point(662, 10);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(246, 189);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Modules in Mainmenu";
            // 
            // update_button
            // 
            this.update_button.Location = new System.Drawing.Point(172, 132);
            this.update_button.Name = "update_button";
            this.update_button.Size = new System.Drawing.Size(68, 23);
            this.update_button.TabIndex = 26;
            this.update_button.Text = "UPDATE";
            this.update_button.UseVisualStyleBackColor = true;
            this.update_button.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // used_list
            // 
            this.used_list.Enabled = false;
            this.used_list.FormattingEnabled = true;
            this.used_list.Location = new System.Drawing.Point(15, 29);
            this.used_list.Name = "used_list";
            this.used_list.Size = new System.Drawing.Size(151, 147);
            this.used_list.TabIndex = 4;
            this.used_list.SelectedIndexChanged += new System.EventHandler(this.used_list_SelectedIndexChanged);
            // 
            // remove_button
            // 
            this.remove_button.Location = new System.Drawing.Point(172, 103);
            this.remove_button.Name = "remove_button";
            this.remove_button.Size = new System.Drawing.Size(68, 23);
            this.remove_button.TabIndex = 25;
            this.remove_button.Text = "REMOVE";
            this.remove_button.UseVisualStyleBackColor = true;
            this.remove_button.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // down_button
            // 
            this.down_button.Location = new System.Drawing.Point(172, 74);
            this.down_button.Name = "down_button";
            this.down_button.Size = new System.Drawing.Size(68, 23);
            this.down_button.TabIndex = 3;
            this.down_button.Text = "DOWN";
            this.down_button.UseVisualStyleBackColor = true;
            this.down_button.Click += new System.EventHandler(this.btMoveDown_Click);
            // 
            // up_button
            // 
            this.up_button.Location = new System.Drawing.Point(172, 45);
            this.up_button.Name = "up_button";
            this.up_button.Size = new System.Drawing.Size(68, 23);
            this.up_button.TabIndex = 2;
            this.up_button.Text = "UP";
            this.up_button.UseVisualStyleBackColor = true;
            this.up_button.Click += new System.EventHandler(this.btMoveUp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.avail_list);
            this.groupBox1.Location = new System.Drawing.Point(5, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 332);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available modules";
            // 
            // avail_list
            // 
            this.avail_list.Enabled = false;
            this.avail_list.FormattingEnabled = true;
            this.avail_list.Location = new System.Drawing.Point(6, 26);
            this.avail_list.Name = "avail_list";
            this.avail_list.Size = new System.Drawing.Size(213, 290);
            this.avail_list.TabIndex = 0;
            this.avail_list.SelectedIndexChanged += new System.EventHandler(this.avail_list_SelectedIndexChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(111, 59);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(113, 17);
            this.radioButton5.TabIndex = 30;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Fanart / Titles only";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(6, 59);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(41, 17);
            this.radioButton4.TabIndex = 29;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Full";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 41);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(236, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Enable RecentlyAdded Series to BasicHome";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(681, 19);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(106, 13);
            this.linkLabel1.TabIndex = 19;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "What is InfoService?";
            // 
            // rB_MyFilms
            // 
            this.rB_MyFilms.AutoSize = true;
            this.rB_MyFilms.Location = new System.Drawing.Point(186, 280);
            this.rB_MyFilms.Name = "rB_MyFilms";
            this.rB_MyFilms.Size = new System.Drawing.Size(62, 17);
            this.rB_MyFilms.TabIndex = 43;
            this.rB_MyFilms.TabStop = true;
            this.rB_MyFilms.Text = "MyFilms";
            this.rB_MyFilms.UseVisualStyleBackColor = true;
            // 
            // AvalonConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 637);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.tabControl1);
            this.Name = "AvalonConfig";
            this.Text = "Avalon Configuration";
            this.Load += new System.EventHandler(this.frmAvalonEditor_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button add_submenu_button;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button update_submenu_button;
        private System.Windows.Forms.ListBox used_list_submenu;
        private System.Windows.Forms.Button remove_submenu_button;
        private System.Windows.Forms.Button down_submenu_button;
        private System.Windows.Forms.Button up_submenu_button;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox new_name;
        private System.Windows.Forms.TextBox new_windowid;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button update_button;
        private System.Windows.Forms.ListBox used_list;
        private System.Windows.Forms.Button remove_button;
        private System.Windows.Forms.Button down_button;
        private System.Windows.Forms.Button up_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox avail_list;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ComboBox new_bgimage;
        private System.Windows.Forms.Label lblParameter;
        private System.Windows.Forms.ComboBox cboParameterViews;
        private System.Windows.Forms.CheckBox cB_onlinevideosOption;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rB_Recordings;
        private System.Windows.Forms.RadioButton rB_Music;
        private System.Windows.Forms.RadioButton rB_Series;
        private System.Windows.Forms.RadioButton rB_Movies;
        private System.Windows.Forms.CheckBox cB_RecentMedia;
        private System.Windows.Forms.ComboBox combo_FanartHandler;
        private System.Windows.Forms.CheckBox cB_FanartHandler;
        private System.Windows.Forms.CheckBox cB_singleImage;
        private System.Windows.Forms.Button bt_browse;
        private Cornerstone.GUI.Controls.FilterComboBox movPicsCategoryCombo;
        private System.Windows.Forms.Label lblMovieCategories;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbEditorConfigs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbEditorViews;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbEditorViewValues;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbEditorLayout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbEditorSearchExpression;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbEditorStartParamsOutput;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.RadioButton rB_MyFilms;

    }
}