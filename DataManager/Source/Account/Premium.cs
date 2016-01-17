using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zDBManager.Account
{
    public partial class Premium : Form
    {
        public string AccountName;

        public Premium(string Account)
        {
            InitializeComponent();
            AccountName = Account;
        }

        void RefreshData(string Account)
        {
            DBLite.dbMu.Read("select * from PremiumData where AccountID = '" + Account +"'");
            DBLite.dbMu.Fetch();

            int Type = DBLite.dbMu.GetAsInteger("PayCode");
            comboBox2.SelectedIndex = Type;

            textBox1.Text = DBLite.dbMu.GetAsString("ExpireDate");
            DBLite.dbMu.Close();

            textBox2.Text = "0";

            if (textBox1.Text != "")
            {
                DateTime TimeNow = DateTime.Now;
                DateTime TimeExpire = DateTime.Parse(textBox1.Text);

                textBox1.Text = TimeExpire.ToString("yyyy-MM-dd HH:mm:ss");

                TimeSpan LeftDays = TimeExpire - TimeNow;
                if (LeftDays.TotalDays > 0.0)
                {
                    int Days = (int)Math.Abs(LeftDays.TotalDays);
                    textBox2.Text = Days.ToString();
                }
            }
        }

        private void Premium_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            DBLite.dbMe.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DBLite.dbMe.Fetch())
            {
                comboBox1.Items.Add(DBLite.dbMe.GetAsString("memb___id"));
            }
            DBLite.dbMe.Close();

            if (AccountName == "")
            {
                return;
            }
            comboBox1.Text = AccountName;
            //RefreshData(AccountName);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                return;
            }
            RefreshData(comboBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime TimeExpire = DateTime.Now;

            if (textBox1.Text != "")
            {
                TimeExpire = Convert.ToDateTime(textBox1.Text);
            }

            if (numericUpDown1.Value > 0)
            {
                TimeExpire = TimeExpire.AddHours((double)numericUpDown1.Value);
            }

            if (numericUpDown2.Value > 0)
            {
                TimeExpire = TimeExpire.AddDays((double)numericUpDown2.Value);
            }

            if (numericUpDown3.Value > 0)
            {
                TimeExpire = TimeExpire.AddMonths((int)numericUpDown3.Value);
            }

            if (numericUpDown4.Value > 0)
            {
                TimeExpire = TimeExpire.AddYears((int)numericUpDown4.Value);
            }

            textBox1.Text = TimeExpire.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime TimeNow = DateTime.Now;
            TimeSpan LeftDays = TimeExpire - TimeNow;

            if (LeftDays.TotalDays > 0.0)
            {
                int Days = (int)Math.Abs(LeftDays.TotalDays);
                textBox2.Text = Days.ToString();
            }
            else
            {
                textBox2.Text = "0";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime TimeExpire = DateTime.Now;

            if (textBox1.Text != "")
            {
                TimeExpire = Convert.ToDateTime(textBox1.Text);
            }

            if (numericUpDown1.Value > 0)
            {
                TimeExpire = TimeExpire.AddHours(-(double)numericUpDown1.Value);
            }

            if (numericUpDown2.Value > 0)
            {
                TimeExpire = TimeExpire.AddDays(-(double)numericUpDown2.Value);
            }

            if (numericUpDown3.Value > 0)
            {
                TimeExpire = TimeExpire.AddMonths(-(int)numericUpDown3.Value);
            }

            if (numericUpDown4.Value > 0)
            {
                TimeExpire = TimeExpire.AddYears(-(int)numericUpDown4.Value);
            }

            textBox1.Text = TimeExpire.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime TimeNow = DateTime.Now;
            TimeSpan LeftDays = TimeExpire - TimeNow;

            if (LeftDays.TotalDays > 0.0)
            {
                int Days = (int)Math.Abs(LeftDays.TotalDays);
                textBox2.Text = Days.ToString();
            }
            else
            {
                textBox2.Text = "0";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                return;
            }

            if (comboBox1.Text == "")
            {
                return;
            }

            Int32 Result = DBLite.dbMu.ExecWithResult("select count(*) from PremiumData where AccountID = '"+ comboBox1.Text +"'");
            DBLite.dbMu.Close();

            if (Result > 0)
            {
                DBLite.dbMu.Exec("update PremiumData set PayCode = " + comboBox2.SelectedIndex + ", ExpireDate = '" + textBox1.Text + "' where AccountID = '" + comboBox1.Text +"'");
            }
            else
            {
                DBLite.dbMu.Exec("insert into PremiumData (AccountID, PayCode, ExpireDate) values('"+comboBox1.Text+"', "+ comboBox2.SelectedIndex +", '"+ textBox1.Text +"')");
            }

            DBLite.dbMu.Close();

            MessageBox.Show("Premium information has been changed", "zDBManager", MessageBoxButtons.OK);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                return;
            }

            if (MessageBox.Show("You sure want delete " + comboBox1.Text + "'s premium information?", "zDBManager", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DBLite.dbMu.Exec("delete from PremiumData where AccountID = '" + comboBox1.Text + "'");
                DBLite.dbMu.Close();
                RefreshData(comboBox1.Text);
            }
        }
    }
}
