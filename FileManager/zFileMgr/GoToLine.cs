using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace zFileMgr
{
    public partial class GoToLine : Form
    {
        DataGridView m_Data;

        public GoToLine(DataGridView dgv, string title)
        {
            m_Data = dgv;
            InitializeComponent();

            tb_linenenr.Focus();
            KeyPreview = true;
            Text = title;
        }

        void SelectRow()
        {
            if (string.IsNullOrEmpty(tb_linenenr.Text))
            {
                return;
            }

            int row = 0;
            try
            {
                row = Convert.ToInt32(tb_linenenr.Text);
                m_Data.FirstDisplayedScrollingRowIndex = row;
                m_Data.Rows[row].Selected = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unhandled ex at GoToline::SelectRow: " + e.Message);
            }
        }

        private void btn_gotonr_Click(object sender, EventArgs e)
        {
            int n = -1;

            try
            {
                n = int.Parse(tb_linenenr.Text);

                if (n > m_Data.Rows.Count || n < 0)
                {
                    MessageBox.Show("line number > line count or line number is negative");
                    return;
                }
                
                SelectRow();
               
            }
            catch (Exception)
            {
                //handle
                //throw new Exception();
                MessageBox.Show("Unknown error at go to line");
            }
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        btn_gotonr_Click(null, null);
                    }
                    break;
                case Keys.Escape:
                    {
                        Close();
                    }
                    break;
            }
        }
    }
}
