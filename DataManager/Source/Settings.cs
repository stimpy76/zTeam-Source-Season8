using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace zDBManager
{
    public partial class Settings : Form
    {
        public byte g_ConnectType;
        public bool g_UseMD5;
        public string g_CharacterDBHost;
        public string g_CharacterDBName;


        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = Properties.Settings.Default.ConnectionType;
            comboBox2.SelectedIndex = Properties.Settings.Default.MD5;

            textBox7.Text = Properties.Settings.Default.CharacterDBHost;
            textBox4.Text = Properties.Settings.Default.AccountDBHost;

            textBox1.Text = Properties.Settings.Default.CharacterDB;
            textBox8.Text = Properties.Settings.Default.AccountDB;

            textBox2.Text = Properties.Settings.Default.CharacterDBLogin;
            textBox6.Text = Properties.Settings.Default.AccountDBLogin;

            textBox3.Text = Properties.Settings.Default.CharacterDBPassword;
            textBox5.Text = Properties.Settings.Default.AccountDBPassword;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConnectionType = (byte)comboBox1.SelectedIndex;
            Properties.Settings.Default.MD5 = (byte)comboBox2.SelectedIndex;

            Properties.Settings.Default.CharacterDBHost = textBox7.Text;
            Properties.Settings.Default.AccountDBHost = textBox4.Text;

            Properties.Settings.Default.CharacterDB = textBox1.Text;
            Properties.Settings.Default.AccountDB = textBox8.Text;

            Properties.Settings.Default.CharacterDBLogin = textBox2.Text;
            Properties.Settings.Default.AccountDBLogin = textBox6.Text;

            Properties.Settings.Default.CharacterDBPassword = textBox3.Text;
            Properties.Settings.Default.AccountDBPassword = textBox5.Text;

            Properties.Settings.Default.Save();

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
