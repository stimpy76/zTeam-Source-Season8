using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zDBManager.Account
{
    public partial class AccountEditor : Form
    {
        private bool ReloadAccount = true;
        private int MemberGuid = 0;

        public AccountEditor(string Account, string Character)
        {
            InitializeComponent();

            if (Account != "")
            {
                comboBox1.Text = Account;
            }

            if (Character != "")
            {
                comboBox2.Text = Character;
            }
        }

        public void Account_Reload()
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            DBLite.dbMe.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DBLite.dbMe.Fetch())
            {
                comboBox1.Items.Add(DBLite.dbMe.GetAsString("memb___id"));
            }
            DBLite.dbMe.Close();
        }

        public void Character_Reload()
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            if (comboBox1.SelectedItem != null)
            {
                DBLite.dbMu.Read("select Name from Character where AccountID = '" + comboBox1.Text + "' order by Name");
                while (DBLite.dbMu.Fetch())
                {
                    comboBox2.Items.Add(DBLite.dbMu.GetAsString("Name"));
                }
                DBLite.dbMu.Close();
            }
        }

        public void AccountInformation_Load()
        {
            if (Properties.Settings.Default.MD5 == 0)
            {
                DBLite.dbMe.Read("select memb_guid, memb__pwd,mail_addr,sno__numb from MEMB_INFO where memb___id = '" + comboBox1.Text + "'");
                DBLite.dbMe.Fetch();
                textBox7.Text = DBLite.dbMe.GetAsString("memb__pwd");
                textBox6.Text = DBLite.dbMe.GetAsString("mail_addr");
                textBox5.Text = DBLite.dbMe.GetAsString("sno__numb");
                MemberGuid = DBLite.dbMe.GetAsInteger("memb_guid");
            }
            else
            {
                DBLite.dbMe.Read("select memb_guid, mail_addr,sno__numb from MEMB_INFO where memb___id = '" + comboBox1.Text + "'");
                DBLite.dbMe.Fetch();
                textBox7.Text = "Encrypted";
                textBox7.ReadOnly = true;
                textBox6.Text = DBLite.dbMe.GetAsString("mail_addr");
                textBox5.Text = DBLite.dbMe.GetAsString("sno__numb");
                MemberGuid = DBLite.dbMe.GetAsInteger("memb_guid");
            }
            DBLite.dbMe.Close();

            DBLite.dbMu.Read("select * from MEMB_STAT where memb___id = '" + comboBox1.Text + "'");
            DBLite.dbMu.Fetch();

            int Status = DBLite.dbMu.GetAsInteger("ConnectStat");

            if (Status == 1)
            {
                label18.Text = "ONLINE";
                label18.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                label18.Text = "OFFLINE";
                label18.ForeColor = System.Drawing.Color.Red;
            }

            textBox8.Text = DBLite.dbMu.GetAsString("ServerName");
            textBox9.Text = DBLite.dbMu.GetAsString("IP");
            textBox11.Text = DBLite.dbMu.GetAsString("ConnectTM");
            textBox10.Text = DBLite.dbMu.GetAsString("DisConnectTM");
            DBLite.dbMu.Close();

            DBLite.dbMu.Read("select WCoinC, WCoinP, WCoinG from GameShop_Data where MemberGuid = " + MemberGuid);
            DBLite.dbMu.Fetch();

            numericUpDown1.Value = DBLite.dbMu.GetAsInteger("WCoinC");
            numericUpDown2.Value = DBLite.dbMu.GetAsInteger("WCoinP");
            numericUpDown3.Value = DBLite.dbMu.GetAsInteger("WCoinG");
            DBLite.dbMu.Close();
        }

        public void AccountInformation_Save()
        {
            if (Properties.Settings.Default.MD5 == 0)
            {
                DBLite.dbMu.Exec("update MEMB_INFO set memb__pwd = '" + textBox7.Text + "', mail_addr = '" + textBox6.Text + "', sno__numb = '" + textBox5.Text + "' where memb___id = '" + comboBox1.Text + "'");                
            }
            else
            {
                DBLite.dbMu.Exec("update MEMB_INFO set mail_addr = '" + textBox6.Text + "', sno__numb = '" + textBox5.Text + "' where memb___id = '" + comboBox1.Text + "'");
            }
            DBLite.dbMu.Close();

            DBLite.dbMu.Exec("update GameShop_Data set WCoinC = '" + numericUpDown1.Value + "', WCoinP = '" + numericUpDown2.Value + "', WCoinG = '" + numericUpDown3.Value + "' where MemberGuid = " + MemberGuid);
            DBLite.dbMu.Close();

            MessageBox.Show("Account has been saved", "zDBManager", MessageBoxButtons.OK);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character_Reload();
            AccountInformation_Load();
        }

        private void AccountEditor_Load(object sender, EventArgs e)
        {
           /* DBLite.mdb = new DBLite(Application.StartupPath + @"\TitanEditor.mdb", "");
            if (DBLite.mdb.Connect() == false)
            {
                MessageBox.Show("Error on connection!1");
                Application.Exit();
            }*/
            if (ReloadAccount)
                Account_Reload();
            //th.Abort();
            this.Focus();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                if (comboBox2.SelectedItem != null)
                {
                    if (MessageBox.Show("You sure want delete " + comboBox2.Text + "?", "zDBManager", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DBLite.dbMu.Exec("delete from Character where Name = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        DBLite.dbMu.Exec("delete from Ertel_Inventory where UserName = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        DBLite.dbMu.Exec("delete from GensMainInfo where memb_char = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        DBLite.dbMu.Exec("delete from GuildMatching_OfferList where Master = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        DBLite.dbMu.Exec("delete from GuildMatching_RequestList where Sender = '" + comboBox2.Text + "' OR Recipient = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        DBLite.dbMu.Exec("delete from GuildMatching_RequestList where Sender = '" + comboBox2.Text + "' OR Recipient = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        for (int i = 1; i <= 5; i++)
                        {
                            DBLite.dbMu.Exec("update AccountCharacter set GameID" + i + "=NULL where GameID" + i + " = '" + comboBox2.Text + "'");
                            DBLite.dbMu.Close();
                        }
                        DBLite.dbMu.Exec("update AccountCharacter set GameIDC=NULL where GameIDC = '" + comboBox2.Text + "'");
                        DBLite.dbMu.Close();

                        comboBox2.Items.Remove(comboBox2.SelectedItem);
                        comboBox2.Text = "";
                    }
                }
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                if (comboBox1.SelectedItem != null)
                {
                    if (MessageBox.Show("You sure want delete " + comboBox1.Text + "?", "zDBManager", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DBLite.dbMe.Exec("delete from MEMB_INFO where memb___id = '" + comboBox1.Text + "'");
                        DBLite.dbMe.Close();
                        DBLite.dbMu.Exec("delete from Character where AccountID = '" + comboBox1.Text + "'");
                        DBLite.dbMu.Close();
                        DBLite.dbMu.Exec("delete from AccountCharacter where Id = '" + comboBox1.Text + "'");
                        DBLite.dbMu.Close();
                        DBLite.dbMu.Exec("delete from warehouse where AccountID = '" + comboBox1.Text + "'");
                        DBLite.dbMu.Close();
                        comboBox1.Items.Remove(comboBox1.SelectedItem);
                        comboBox1.Text = "";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                AccountInformation_Save();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                Premium PremiumForm = new Premium(comboBox1.Text);
                PremiumForm.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                Warehouse WarehouseForm = new Warehouse();
                WarehouseForm.AccountName = comboBox1.Text;
                WarehouseForm.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                if (comboBox2.Items.Count >= 5)
                {
                    MessageBox.Show("Create error, you have reached max. characters slot count", "zDBManager", MessageBoxButtons.OK);
                    return;
                }

                Character.Create CreateForm = new Character.Create(comboBox1.Text);
                DialogResult Result = new DialogResult();
                Result = CreateForm.ShowDialog();

                if (Result == DialogResult.OK)
                {
                    Character_Reload();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 CheckResult = DBLite.dbMe.ExecWithResult("select count(*) from MEMB_INFO where memb___id = '" + textBox1.Text +"'");

            if (textBox1.Text.Length < 2)
            {
                MessageBox.Show("Create error, please check entered account name", "zDBManager", MessageBoxButtons.OK);
                DBLite.dbMe.Close();
                return;
            }

            if (textBox2.Text.Length < 2)
            {
                MessageBox.Show("Create error, please check entered password", "zDBManager", MessageBoxButtons.OK);
                DBLite.dbMe.Close();
                return;
            }

            if( CheckResult > 0 )
            {
                MessageBox.Show("Create error, account already exist", "zDBManager", MessageBoxButtons.OK);
                DBLite.dbMe.Close();
                return;
            }

            DBLite.dbMe.Close();

            if (Properties.Settings.Default.MD5 == 0)
            {
                DBLite.dbMe.Exec("insert into MEMB_INFO (memb___id,memb__pwd,memb_name,sno__numb,mail_addr,fpas_ques,fpas_answ,appl_days,modi_days,out__days,true_days,mail_chek,bloc_code,ctl1_code) values ('"+ textBox1.Text +"','"+ textBox2.Text +"','','1','"+ textBox3.Text +"','','','20140101','20140101','20140101','20140101','1','0','0')");
                DBLite.dbMe.Close();
                MessageBox.Show("Account has been created", "zDBManager", MessageBoxButtons.OK);
                Account_Reload();
                comboBox1.Text = textBox1.Text;
                AccountInformation_Load();
            }
            else
            {
                MessageBox.Show("Create error, MD5 temporarily disabled", "zDBManager", MessageBoxButtons.OK);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                Character.Editor CharacterEditorForm = new Character.Editor(comboBox1.Text, comboBox2.Text);
                CharacterEditorForm.Show();
            }
        }
    }
}
