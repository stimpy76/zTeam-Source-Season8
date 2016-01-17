using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace zDBManager
{
    public partial class LogWindow : Form
    {
        public static LogWindow SqlLog = new LogWindow("Sql execution log");
        public object m_objSender;

        public LogWindow(string Title)
        {
            m_objSender = new object();
            this.Visible = false;
            InitializeComponent();
            this.Text = "zDBManager :: " + Title;
        }

        public void LogAdd(string Text)
        {
            this.textBox1.AppendText("["+DateTime.Now.ToString("HH:mm:ss")+"] "+Text + "\r\n");
        }

        private void LogWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            if (m_objSender != null)
            {
                (m_objSender as System.Windows.Forms.ToolStripMenuItem).Checked = false;
            }
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog Dialog = new SaveFileDialog();
            Dialog.InitialDirectory = Directory.GetCurrentDirectory();

            Dialog.Filter = "Log file (*.log)|*.log";

            if (Dialog.ShowDialog() == DialogResult.OK && Dialog.FileName.Length > 0)
            {
                File.WriteAllText(Dialog.FileName, textBox1.Text);
            }
        }
    }
}
