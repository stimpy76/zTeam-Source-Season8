using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zDBManager
{
    public partial class Find : Form
    {
        public Find()
        {
            InitializeComponent();
        }

        public bool isAccount = false;

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (isAccount == false)
            {
                DBLite.dbMu.Read("select AccountID,Name from Character where Name Like '" + textBox1.Text + "%' order by Name");
                while (DBLite.dbMu.Fetch())
                {
                    listView1.Items.Add(DBLite.dbMu.GetAsString("AccountID")).SubItems.Add(DBLite.dbMu.GetAsString("Name"));
                }
                DBLite.dbMu.Close();
            }
            else
            {
                DBLite.dbMe.Read("select memb___id from MEMB_INFO where memb___id Like '" + textBox1.Text + "%' order by memb___id");
                while (DBLite.dbMe.Fetch())
                {
                    listView1.Items.Add(DBLite.dbMe.GetAsString("memb___id"));
                }
                DBLite.dbMe.Close();
            }
        }

        private void Find_Load(object sender, EventArgs e)
        {
            Extra.FindResult.AccountID = "";
            Extra.FindResult.Character = "";

            if (isAccount == false)
                label1.Text += "Character";
            else
                label1.Text += "Account";
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                Extra.FindResult.AccountID = listView1.FocusedItem.Text;
                if (isAccount == false)
                    Extra.FindResult.Character = listView1.FocusedItem.SubItems[1].Text;

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
