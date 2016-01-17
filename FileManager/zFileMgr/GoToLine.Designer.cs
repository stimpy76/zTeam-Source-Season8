namespace zFileMgr
{
    partial class GoToLine
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
            this.tb_linenenr = new System.Windows.Forms.TextBox();
            this.btn_gotonr = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_linenenr
            // 
            this.tb_linenenr.Location = new System.Drawing.Point(1, 10);
            this.tb_linenenr.Name = "tb_linenenr";
            this.tb_linenenr.Size = new System.Drawing.Size(63, 20);
            this.tb_linenenr.TabIndex = 0;
            this.tb_linenenr.Text = "1";
            // 
            // btn_gotonr
            // 
            this.btn_gotonr.Location = new System.Drawing.Point(70, 8);
            this.btn_gotonr.Name = "btn_gotonr";
            this.btn_gotonr.Size = new System.Drawing.Size(77, 23);
            this.btn_gotonr.TabIndex = 1;
            this.btn_gotonr.Text = "Go";
            this.btn_gotonr.UseVisualStyleBackColor = true;
            this.btn_gotonr.Click += new System.EventHandler(this.btn_gotonr_Click);
            // 
            // GoToLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(151, 37);
            this.Controls.Add(this.btn_gotonr);
            this.Controls.Add(this.tb_linenenr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GoToLine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GoToLine";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_linenenr;
        private System.Windows.Forms.Button btn_gotonr;
    }
}