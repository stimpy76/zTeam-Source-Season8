namespace zFileMgr
{
    partial class NormalShopEditor
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
            this.combobox_item_cat = new System.Windows.Forms.ComboBox();
            this.combobox_item_name = new System.Windows.Forms.ComboBox();
            this.checkbox_skill = new System.Windows.Forms.CheckBox();
            this.checkbox_luck = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.group_newitem = new System.Windows.Forms.GroupBox();
            this.picturebox_inventory = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            this.group_newitem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_inventory)).BeginInit();
            this.SuspendLayout();
            // 
            // combobox_item_cat
            // 
            this.combobox_item_cat.FormattingEnabled = true;
            this.combobox_item_cat.Location = new System.Drawing.Point(6, 19);
            this.combobox_item_cat.Name = "combobox_item_cat";
            this.combobox_item_cat.Size = new System.Drawing.Size(121, 21);
            this.combobox_item_cat.TabIndex = 0;
            // 
            // combobox_item_name
            // 
            this.combobox_item_name.FormattingEnabled = true;
            this.combobox_item_name.Location = new System.Drawing.Point(6, 46);
            this.combobox_item_name.Name = "combobox_item_name";
            this.combobox_item_name.Size = new System.Drawing.Size(121, 21);
            this.combobox_item_name.TabIndex = 1;
            // 
            // checkbox_skill
            // 
            this.checkbox_skill.AutoSize = true;
            this.checkbox_skill.Location = new System.Drawing.Point(6, 73);
            this.checkbox_skill.Name = "checkbox_skill";
            this.checkbox_skill.Size = new System.Drawing.Size(45, 17);
            this.checkbox_skill.TabIndex = 2;
            this.checkbox_skill.Text = "Skill";
            this.checkbox_skill.UseVisualStyleBackColor = true;
            // 
            // checkbox_luck
            // 
            this.checkbox_luck.AutoSize = true;
            this.checkbox_luck.Location = new System.Drawing.Point(57, 73);
            this.checkbox_luck.Name = "checkbox_luck";
            this.checkbox_luck.Size = new System.Drawing.Size(50, 17);
            this.checkbox_luck.TabIndex = 3;
            this.checkbox_luck.Text = "Luck";
            this.checkbox_luck.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1292, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xMLToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // xMLToolStripMenuItem
            // 
            this.xMLToolStripMenuItem.Name = "xMLToolStripMenuItem";
            this.xMLToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.xMLToolStripMenuItem.Text = "XML";
            this.xMLToolStripMenuItem.Click += new System.EventHandler(this.xMLToolStripMenuItem_Click);
            // 
            // group_newitem
            // 
            this.group_newitem.Controls.Add(this.combobox_item_cat);
            this.group_newitem.Controls.Add(this.checkbox_luck);
            this.group_newitem.Controls.Add(this.combobox_item_name);
            this.group_newitem.Controls.Add(this.checkbox_skill);
            this.group_newitem.Location = new System.Drawing.Point(30, 28);
            this.group_newitem.Name = "group_newitem";
            this.group_newitem.Size = new System.Drawing.Size(200, 100);
            this.group_newitem.TabIndex = 5;
            this.group_newitem.TabStop = false;
            this.group_newitem.Text = "New item";
            // 
            // picturebox_inventory
            // 
            this.picturebox_inventory.Image = global::zFileMgr.Properties.Resources.PcVJzDSV3g8;
            this.picturebox_inventory.Location = new System.Drawing.Point(479, 28);
            this.picturebox_inventory.Name = "picturebox_inventory";
            this.picturebox_inventory.Size = new System.Drawing.Size(258, 480);
            this.picturebox_inventory.TabIndex = 6;
            this.picturebox_inventory.TabStop = false;
            // 
            // NormalShopEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 592);
            this.Controls.Add(this.picturebox_inventory);
            this.Controls.Add(this.group_newitem);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "NormalShopEditor";
            this.Text = "NormalShop";
            this.Load += new System.EventHandler(this.NormalShopEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.group_newitem.ResumeLayout(false);
            this.group_newitem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturebox_inventory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox combobox_item_cat;
        private System.Windows.Forms.ComboBox combobox_item_name;
        private System.Windows.Forms.CheckBox checkbox_skill;
        private System.Windows.Forms.CheckBox checkbox_luck;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.GroupBox group_newitem;
        private System.Windows.Forms.PictureBox picturebox_inventory;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xMLToolStripMenuItem;
    }
}