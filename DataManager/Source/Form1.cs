using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace zDBManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool ConnectionReload()
        {
            bool ConnectionStatus = true;

            if (Properties.Settings.Default.ConnectionType == 0)
            {
                DBLite.dbMe = new DBLite(1);
                DBLite.dbMu = new DBLite(1);
                if (DBLite.dbMe.Connect(Properties.Settings.Default.AccountDB,
                    Properties.Settings.Default.CharacterDBLogin,
                    Properties.Settings.Default.CharacterDBPassword) == false)
                {
                    MessageBox.Show("(AccountDB) ODBC connection error", "zDBManager", MessageBoxButtons.OK);
                    toolStripStatusLabel5.Text = "Disconnected";
                    ConnectionStatus = false;
                }
                else
                {
                    toolStripStatusLabel5.Text = "Connected";
                }

                if (DBLite.dbMu.Connect(Properties.Settings.Default.CharacterDB,
                    Properties.Settings.Default.CharacterDBLogin,
                    Properties.Settings.Default.CharacterDBPassword) == false)
                {
                    MessageBox.Show("(CharacterDB) ODBC connection error", "zDBManager", MessageBoxButtons.OK);
                    toolStripStatusLabel2.Text = "Disconnected";
                    ConnectionStatus = false;
                }
                else
                {
                    toolStripStatusLabel2.Text = "Connected";
                }
            }
            else
            {
                DBLite.dbMe = new DBLite(3);
                DBLite.dbMu = new DBLite(3);
                if (DBLite.dbMe.Connect(Properties.Settings.Default.AccountDBHost,
                    Properties.Settings.Default.AccountDB,
                    Properties.Settings.Default.AccountDBLogin,
                    Properties.Settings.Default.AccountDBPassword) == false)
                {
                    MessageBox.Show("(AccountDB) Direct connection error", "zDBManager", MessageBoxButtons.OK);
                    toolStripStatusLabel5.Text = "Disconnected";
                    ConnectionStatus = false;
                }
                else
                {
                    toolStripStatusLabel5.Text = "Connected";
                }

                if (DBLite.dbMu.Connect(Properties.Settings.Default.CharacterDBHost,
                    Properties.Settings.Default.CharacterDB,
                    Properties.Settings.Default.CharacterDBLogin,
                    Properties.Settings.Default.CharacterDBPassword) == false)
                {
                    MessageBox.Show("(CharacterDB) Direct connection error", "zDBManager", MessageBoxButtons.OK);
                    toolStripStatusLabel2.Text = "Disconnected";
                    ConnectionStatus = false;
                }
                else
                {
                    toolStripStatusLabel2.Text = "Connected";
                }
            }

            if (!ConnectionStatus)
            {
                this.usersToolStripMenuItem.Enabled = false;
                this.accountEditorToolStripMenuItem.ShortcutKeys = Keys.None;
                this.characterEditorToolStripMenuItem.ShortcutKeys = Keys.None;
                this.skillsEditorToolStripMenuItem.ShortcutKeys = Keys.None;
                this.premiumControlToolStripMenuItem.ShortcutKeys = Keys.None;
            }
            else
            {
                this.usersToolStripMenuItem.Enabled = true;
                this.accountEditorToolStripMenuItem.ShortcutKeys = Keys.F1;
                this.characterEditorToolStripMenuItem.ShortcutKeys = Keys.F2;
                this.skillsEditorToolStripMenuItem.ShortcutKeys = Keys.F3;
                this.premiumControlToolStripMenuItem.ShortcutKeys = Keys.F4;
            }

            return ConnectionStatus;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*EquipItemInfo.g_ItemInfo = new EquipItemInfo();
            EquipItemInfo.g_ItemInfo.Load();*/

            if (!ConnectionReload())
            {
                Settings CreateForm = new Settings();
                DialogResult Result = new DialogResult();
                Result = CreateForm.ShowDialog();

                if (Result == DialogResult.OK)
                {
                    Form1_Load(sender, e);
                    return;
                }
            }

            this.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                Hide();
                ShowInTaskbar = false;

                notifyIcon1.ShowBalloonTip(1);
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Show();
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                notifyIcon1.ContextMenuStrip.Hide();
                Focus();
            }
            /*else if (e.Button == MouseButtons.Right)
            {
                notifyIcon1.Visible = false;
                Application.Exit();
            }*/
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void accountEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Account.AccountEditor AccountEditorForm = new Account.AccountEditor("", "");
            AccountEditorForm.Show();
        }

        private void sQLLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogWindow.SqlLog.m_objSender = sender;

            if (sQLLogToolStripMenuItem.Checked)
            {
                LogWindow.SqlLog.Show();
            }
            else
            {
                LogWindow.SqlLog.Hide();
            }
        }

        private void premiumControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Account.Premium PremiumEditorForm = new Account.Premium("");
            PremiumEditorForm.Show();
        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings CreateForm = new Settings();
            DialogResult Result = new DialogResult();
            Result = CreateForm.ShowDialog();

            if (Result == DialogResult.OK)
            {
                ConnectionReload();
            }
        }

        private void characterEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character.Editor CharacterEditorForm = new Character.Editor("", "");
            CharacterEditorForm.Show();
        }

        private void skillsEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character.SkillsEditor SkillsForm = new Character.SkillsEditor("", "");
            SkillsForm.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Account.AccountEditor AccountEditorForm = new Account.AccountEditor("", "");
            AccountEditorForm.Show();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Character.Editor CharacterEditorForm = new Character.Editor("", "");
            CharacterEditorForm.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Character.SkillsEditor SkillsForm = new Character.SkillsEditor("", "");
            SkillsForm.Show();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Account.Premium PremiumEditorForm = new Account.Premium("");
            PremiumEditorForm.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            notifyIcon1.ContextMenuStrip.Hide();
            Focus();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            Settings CreateForm = new Settings();
            DialogResult Result = new DialogResult();
            Result = CreateForm.ShowDialog();

            if (Result == DialogResult.OK)
            {
                ConnectionReload();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About AboutForm = new About();
            AboutForm.Show();
        }
    }
}
