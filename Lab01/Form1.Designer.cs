namespace Lab01
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
            this.btLine = new System.Windows.Forms.Button();
            this.btCircle = new System.Windows.Forms.Button();
            this.btMauNen = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.openGLControl = new SharpGL.OpenGLControl();
            this.time = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_hinh_chu_nhat = new System.Windows.Forms.Button();
            this.btEllipse = new System.Windows.Forms.Button();
            this.bt_tam_giac_deu = new System.Windows.Forms.Button();
            this.bt_ngu_giac_deu = new System.Windows.Forms.Button();
            this.bt_luc_giac_deu = new System.Windows.Forms.Button();
            this.bt_fill = new System.Windows.Forms.Button();
            this.bt_DoDay = new System.Windows.Forms.ComboBox();
            this.btMauVien = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Keo_Gian = new System.Windows.Forms.Button();
            this.bt_select = new System.Windows.Forms.Button();
            this.bt_scanline = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).BeginInit();
            this.SuspendLayout();
            // 
            // btLine
            // 
            this.btLine.Location = new System.Drawing.Point(12, 12);
            this.btLine.Name = "btLine";
            this.btLine.Size = new System.Drawing.Size(75, 23);
            this.btLine.TabIndex = 0;
            this.btLine.Text = "Line";
            this.btLine.UseVisualStyleBackColor = true;
            this.btLine.Click += new System.EventHandler(this.btLine_Click);
            // 
            // btCircle
            // 
            this.btCircle.Location = new System.Drawing.Point(93, 12);
            this.btCircle.Name = "btCircle";
            this.btCircle.Size = new System.Drawing.Size(75, 23);
            this.btCircle.TabIndex = 1;
            this.btCircle.Text = "Circle";
            this.btCircle.UseVisualStyleBackColor = true;
            this.btCircle.Click += new System.EventHandler(this.btCircle_Click);
            // 
            // btMauNen
            // 
            this.btMauNen.Location = new System.Drawing.Point(1053, 12);
            this.btMauNen.Name = "btMauNen";
            this.btMauNen.Size = new System.Drawing.Size(73, 23);
            this.btMauNen.TabIndex = 2;
            this.btMauNen.Text = "Màu nền";
            this.btMauNen.UseVisualStyleBackColor = true;
            this.btMauNen.Click += new System.EventHandler(this.btMauNen_Click);
            // 
            // openGLControl
            // 
            this.openGLControl.DrawFPS = false;
            this.openGLControl.FrameRate = 60;
            this.openGLControl.Location = new System.Drawing.Point(12, 69);
            this.openGLControl.Name = "openGLControl";
            this.openGLControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.openGLControl.RenderContextType = SharpGL.RenderContextType.FBO;
            this.openGLControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.openGLControl.Size = new System.Drawing.Size(1114, 435);
            this.openGLControl.TabIndex = 3;
            this.openGLControl.OpenGLInitialized += new System.EventHandler(this.openGLControl_OpenGLInitialized);
            this.openGLControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.openGLControl_OpenGLDraw);
            this.openGLControl.Resized += new System.EventHandler(this.openGLControl_Resized);
            this.openGLControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseClick);
            this.openGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseDown);
            this.openGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseMove);
            this.openGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.openGLControl_MouseUp);
            // 
            // time
            // 
            this.time.Location = new System.Drawing.Point(81, 510);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(100, 20);
            this.time.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 513);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Time:";
            // 
            // bt_hinh_chu_nhat
            // 
            this.bt_hinh_chu_nhat.Location = new System.Drawing.Point(174, 12);
            this.bt_hinh_chu_nhat.Name = "bt_hinh_chu_nhat";
            this.bt_hinh_chu_nhat.Size = new System.Drawing.Size(95, 23);
            this.bt_hinh_chu_nhat.TabIndex = 6;
            this.bt_hinh_chu_nhat.Text = "Hinh Chu Nhat";
            this.bt_hinh_chu_nhat.UseVisualStyleBackColor = true;
            this.bt_hinh_chu_nhat.Click += new System.EventHandler(this.bt_hinh_chu_nhat_Click);
            // 
            // btEllipse
            // 
            this.btEllipse.Location = new System.Drawing.Point(275, 12);
            this.btEllipse.Name = "btEllipse";
            this.btEllipse.Size = new System.Drawing.Size(75, 23);
            this.btEllipse.TabIndex = 7;
            this.btEllipse.Text = "Ellipse";
            this.btEllipse.UseVisualStyleBackColor = true;
            this.btEllipse.Click += new System.EventHandler(this.btEllipse_Click);
            // 
            // bt_tam_giac_deu
            // 
            this.bt_tam_giac_deu.Location = new System.Drawing.Point(356, 12);
            this.bt_tam_giac_deu.Name = "bt_tam_giac_deu";
            this.bt_tam_giac_deu.Size = new System.Drawing.Size(88, 23);
            this.bt_tam_giac_deu.TabIndex = 8;
            this.bt_tam_giac_deu.Text = "Tam Giac Deu";
            this.bt_tam_giac_deu.UseVisualStyleBackColor = true;
            this.bt_tam_giac_deu.Click += new System.EventHandler(this.bt_tam_giac_deu_Click);
            // 
            // bt_ngu_giac_deu
            // 
            this.bt_ngu_giac_deu.Location = new System.Drawing.Point(450, 12);
            this.bt_ngu_giac_deu.Name = "bt_ngu_giac_deu";
            this.bt_ngu_giac_deu.Size = new System.Drawing.Size(84, 23);
            this.bt_ngu_giac_deu.TabIndex = 9;
            this.bt_ngu_giac_deu.Text = "Ngu Giac Deu";
            this.bt_ngu_giac_deu.UseVisualStyleBackColor = true;
            this.bt_ngu_giac_deu.Click += new System.EventHandler(this.bt_ngu_giac_deu_Click);
            // 
            // bt_luc_giac_deu
            // 
            this.bt_luc_giac_deu.Location = new System.Drawing.Point(541, 12);
            this.bt_luc_giac_deu.Name = "bt_luc_giac_deu";
            this.bt_luc_giac_deu.Size = new System.Drawing.Size(87, 23);
            this.bt_luc_giac_deu.TabIndex = 10;
            this.bt_luc_giac_deu.Text = "Luc Giac Deu";
            this.bt_luc_giac_deu.UseVisualStyleBackColor = true;
            this.bt_luc_giac_deu.Click += new System.EventHandler(this.bt_luc_giac_deu_Click);
            // 
            // bt_fill
            // 
            this.bt_fill.Location = new System.Drawing.Point(93, 41);
            this.bt_fill.Name = "bt_fill";
            this.bt_fill.Size = new System.Drawing.Size(75, 23);
            this.bt_fill.TabIndex = 12;
            this.bt_fill.Text = "To Mau";
            this.bt_fill.UseVisualStyleBackColor = true;
            this.bt_fill.Click += new System.EventHandler(this.bt_fill_Click);
            // 
            // bt_DoDay
            // 
            this.bt_DoDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bt_DoDay.FormattingEnabled = true;
            this.bt_DoDay.Items.AddRange(new object[] {
            "Small",
            "Medium",
            "Big"});
            this.bt_DoDay.Location = new System.Drawing.Point(857, 14);
            this.bt_DoDay.Name = "bt_DoDay";
            this.bt_DoDay.Size = new System.Drawing.Size(87, 21);
            this.bt_DoDay.TabIndex = 13;
            this.bt_DoDay.SelectedIndex = 0;
            this.bt_DoDay.SelectedIndexChanged += new System.EventHandler(this.bt_DoDay_SelectedIndexChanged);
            // 
            // btMauVien
            // 
            this.btMauVien.Location = new System.Drawing.Point(973, 12);
            this.btMauVien.Name = "btMauVien";
            this.btMauVien.Size = new System.Drawing.Size(74, 23);
            this.btMauVien.TabIndex = 14;
            this.btMauVien.Text = "Màu viền";
            this.btMauVien.UseVisualStyleBackColor = true;
            this.btMauVien.Click += new System.EventHandler(this.btMauVien_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Keo_Gian
            // 
            this.Keo_Gian.Location = new System.Drawing.Point(275, 40);
            this.Keo_Gian.Name = "Keo_Gian";
            this.Keo_Gian.Size = new System.Drawing.Size(75, 23);
            this.Keo_Gian.TabIndex = 15;
            this.Keo_Gian.Text = "Keo Gian";
            this.Keo_Gian.UseVisualStyleBackColor = true;
            this.Keo_Gian.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Keo_Gian_MouseDown);
            // 
            // bt_select
            // 
            this.bt_select.Location = new System.Drawing.Point(12, 41);
            this.bt_select.Name = "bt_select";
            this.bt_select.Size = new System.Drawing.Size(75, 23);
            this.bt_select.TabIndex = 16;
            this.bt_select.Text = "Select";
            this.bt_select.UseVisualStyleBackColor = true;
            this.bt_select.Click += new System.EventHandler(this.bt_select_Click);
            // 
            // bt_scanline
            // 
            this.bt_scanline.Location = new System.Drawing.Point(174, 41);
            this.bt_scanline.Name = "bt_scanline";
            this.bt_scanline.Size = new System.Drawing.Size(95, 23);
            this.bt_scanline.TabIndex = 17;
            this.bt_scanline.Text = "Scanline Fill";
            this.bt_scanline.UseVisualStyleBackColor = true;
            this.bt_scanline.Click += new System.EventHandler(this.bt_scanline_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 542);
            this.Controls.Add(this.bt_scanline);
            this.Controls.Add(this.bt_select);
            this.Controls.Add(this.Keo_Gian);
            this.Controls.Add(this.btMauVien);
            this.Controls.Add(this.bt_DoDay);
            this.Controls.Add(this.bt_fill);
            this.Controls.Add(this.bt_luc_giac_deu);
            this.Controls.Add(this.bt_ngu_giac_deu);
            this.Controls.Add(this.bt_tam_giac_deu);
            this.Controls.Add(this.btEllipse);
            this.Controls.Add(this.bt_hinh_chu_nhat);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.time);
            this.Controls.Add(this.openGLControl);
            this.Controls.Add(this.btMauNen);
            this.Controls.Add(this.btCircle);
            this.Controls.Add(this.btLine);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.bt_luc_giac_deu_Click);
            ((System.ComponentModel.ISupportInitialize)(this.openGLControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btLine;
        private System.Windows.Forms.Button btCircle;
        private System.Windows.Forms.Button btMauNen;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private SharpGL.OpenGLControl openGLControl;
        private System.Windows.Forms.TextBox time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_hinh_chu_nhat;
        private System.Windows.Forms.Button btEllipse;
        private System.Windows.Forms.Button bt_tam_giac_deu;
        private System.Windows.Forms.Button bt_ngu_giac_deu;
        private System.Windows.Forms.Button bt_luc_giac_deu;
        private System.Windows.Forms.Button bt_fill;
        private System.Windows.Forms.ComboBox bt_DoDay;
        private System.Windows.Forms.Button btMauVien;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Keo_Gian;
        private System.Windows.Forms.Button bt_select;
        private System.Windows.Forms.Button bt_scanline;
    }
}

