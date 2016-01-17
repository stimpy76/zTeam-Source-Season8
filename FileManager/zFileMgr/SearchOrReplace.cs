using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using zFileMgr.Utils;

namespace zFileMgr
{
    public partial class SearchOrReplace : Form
    {
        private DataGridView m_Data;
        private int m_LastRow;
        private int m_LastCol;
        bool m_DoActualReplace;
        // ----
        public SearchOrReplace(DataGridView Data, string Title)
        {
            m_LastRow   = 0;
            m_LastCol = 0;
            m_Data      = Data;
            m_DoActualReplace = false;

            InitializeComponent();

            KeyPreview  = true;
            Text        = Title;
        }
        // ----

        private void OnFocusEnter(object sender, EventArgs e)
        {
            Opacity = 100.0;
        }
        // ----
        private void OnFocusLeave(object sender, EventArgs e)
        {
            Opacity = 90.0;
        }
        // ----

        /// <summary>
        /// Todo: fix replacing (sometimes gets bugged for some reason)
        /// both single-item replace and replace all option
        /// </summary>
        /// <param name="bWhatToDo">find, replace one item, replace all items</param>
        /// <param name="bDoActualReplace"></param>
        /// <returns></returns>
        int Exec(byte bWhatToDo, bool bDoActualReplace)
        {
            bool DoBreak = false;
            int FindResult = -1;

            //last row in our dgv is null
            for (int nRow = m_LastRow; nRow < m_Data.Rows.Count - 1; nRow++)
            {
                for (int nCol = 0; nCol < m_Data.Columns.Count; nCol++)
                {
      
                    bool tmpres = false;

                    if (cbox_match_case.Checked)
                    {
                        if (m_Data[nCol, nRow].Value != null)
                        {
                            if (Misc.StrContainsValue(m_Data[nCol, nRow].Value.ToString(), textBox1.Text, false))
                                tmpres = true;
                        }
                      
                    }
                    else
                    {
                        if (m_Data[nCol, nRow].Value != null)
                        {
                            if (Misc.StrContainsValue(m_Data[nCol, nRow].Value.ToString(), textBox1.Text, true))
                                tmpres = true;
                        }
                    }
                   

                    //found something 
                    if (tmpres)
                    {

                         if(m_LastRow < nRow || m_LastCol < nCol)
                        {
                            
                            m_Data.FirstDisplayedScrollingRowIndex = nRow;

                            switch (bWhatToDo)
                            {
                                case 0:
                                    {
                                        
                                        m_Data[nCol, nRow].Selected = true;
                                        m_LastRow = nRow;
                                        m_LastCol = nCol;
                                        DoBreak = true;
                                    }
                                    break;

                                case 1:
                                    {
                                        m_Data[nCol, nRow].Selected = true;

                                        if (bDoActualReplace)
                                        {
                                            if (cbox_match_case.Checked)
                                            {
                                                m_Data[nCol, nRow].Value = Misc.StrReplace(m_Data[nCol, nRow].Value.ToString(), textBox1.Text, textBox2.Text, false);
                                            }
                                            else
                                            {

                                                m_Data[nCol, nRow].Value = Misc.StrReplace(m_Data[nCol, nRow].Value.ToString(), textBox1.Text, textBox2.Text, true);
                                               
                                            }
                                            m_LastRow = nRow;
                                            m_LastCol = nCol;
                                        }
                                       
                                        
                                    }
                                    break;
                                case 2:
                                    {
                                        if (cbox_match_case.Checked)
                                        {
                                            m_Data[nCol, nRow].Value = Misc.StrReplace(m_Data[nCol, nRow].Value.ToString(), textBox1.Text, textBox2.Text, false);
                                        }
                                        else
                                        {

                                            m_Data[nCol, nRow].Value = Misc.StrReplace(m_Data[nCol, nRow].Value.ToString(), textBox1.Text, textBox2.Text, true);

                                        }
                                        m_LastRow = nRow;
                                        m_LastCol = nCol;
                                    }
                                    break;
                            }

                            //assign result
                            FindResult = nRow;

                            DoBreak = true;
                            lbl_status.Text = "Found at line " + nRow.ToString();
                            break;
                        }

                    }
                }

                if (DoBreak) break;
            }

            return FindResult;
        }

        /// <summary>
        /// Find item in table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int found = Exec(0, false);

            if (found == -1)
            {
                m_LastRow = 0;
                lbl_status.Text = "Search: Not found";
            }
        }
        // ----
        /// <summary>
        /// Replace single item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Exec(1, m_DoActualReplace);

            //switch true / false
            m_DoActualReplace = !(m_DoActualReplace);
        }
        // ----

        /// <summary>
        /// Replace all items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //replac all
            int found = Exec(2, false);
            if (found == -1)
            {
                m_LastRow = 0;
                lbl_status.Text = "Replace all: nothing found";
            }

            else
            {
                
                //just loop until nothing to replace
                int nCounter = 0;
                while ((found = Exec(2, false)) != -1)
                {
                    nCounter++;
                }
                lbl_status.Text = "Replace all: replaced " + nCounter.ToString() + " items";
            }
        }



        void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    {
                        button1_Click(null, null);
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
