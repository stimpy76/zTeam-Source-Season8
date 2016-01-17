namespace zFileMgr
{
    partial class AboutBox
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_product = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_ver = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_desc = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_build = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_copyright = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "# Product";
            // 
            // lbl_product
            // 
            this.lbl_product.AutoSize = true;
            this.lbl_product.Location = new System.Drawing.Point(95, 9);
            this.lbl_product.Name = "lbl_product";
            this.lbl_product.Size = new System.Drawing.Size(70, 13);
            this.lbl_product.TabIndex = 1;
            this.lbl_product.Text = "zFileManager";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "# Version";
            // 
            // lbl_ver
            // 
            this.lbl_ver.AutoSize = true;
            this.lbl_ver.Location = new System.Drawing.Point(95, 35);
            this.lbl_ver.Name = "lbl_ver";
            this.lbl_ver.Size = new System.Drawing.Size(40, 13);
            this.lbl_ver.TabIndex = 3;
            this.lbl_ver.Text = "1.0.0.0";
            this.lbl_ver.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "# Description";
            // 
            // lbl_desc
            // 
            this.lbl_desc.AutoSize = true;
            this.lbl_desc.Location = new System.Drawing.Point(95, 62);
            this.lbl_desc.Name = "lbl_desc";
            this.lbl_desc.Size = new System.Drawing.Size(114, 13);
            this.lbl_desc.TabIndex = 5;
            this.lbl_desc.Text = "MU Online client editor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "# Build ";
            // 
            // lbl_build
            // 
            this.lbl_build.AutoSize = true;
            this.lbl_build.Location = new System.Drawing.Point(95, 92);
            this.lbl_build.Name = "lbl_build";
            this.lbl_build.Size = new System.Drawing.Size(29, 13);
            this.lbl_build.TabIndex = 7;
            this.lbl_build.Text = "build";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(17, 155);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(413, 34);
            this.button1.TabIndex = 8;
            this.button1.Text = "Website";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_copyright
            // 
            this.lbl_copyright.AutoSize = true;
            this.lbl_copyright.Location = new System.Drawing.Point(139, 126);
            this.lbl_copyright.Name = "lbl_copyright";
            this.lbl_copyright.Size = new System.Drawing.Size(125, 13);
            this.lbl_copyright.TabIndex = 9;
            this.lbl_copyright.Text = "Copyright © 2014 zTeam";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(158, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "www.z-team.pro";
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 193);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_copyright);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_build);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_desc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_ver);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_product);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "zFileManager :: About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_product;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_ver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_desc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_build;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_copyright;
        private System.Windows.Forms.Label label4;

    }
}