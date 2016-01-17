using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using zFileMgr.MuBMD;
using zFileMgr.MuGeneric;
using zFileMgr.Utils;

namespace zFileMgr
{
    public partial class ClientEditor : Form
    {
        BmdFile m_File;
        bool m_IsSaved;
        bool m_NeedsSaving;
        string m_SaveFolderPath;

        struct CellEditInfo
        {
            public int ColIndex;
            public int RowIndex;
            public string Value;
        }


        //for item.bmd
        int CurrentGroupID = 0;
        bool m_IsGroupedMode = false;
        int nCurItemID = 0;

        object[] m_AllStructures;

        Stack<CellEditInfo> m_ColumnActions = new Stack<CellEditInfo>();
        
        public ClientEditor(MuBMD.BmdFile file, bool IsGroupedMode)
        {
            InitializeComponent();
            m_File = file;
            m_IsSaved = false;
            m_NeedsSaving = false;

            m_IsGroupedMode = IsGroupedMode;

            m_SaveFolderPath = Properties.Settings.Default.SaveFolder;
            Properties.Settings.Default.Save();

            dataGridView1.RowHeadersWidth = 50;

            if (m_IsGroupedMode)
            {
                m_AllStructures = file.GetStructures();
                object[] items = Item.GetItemsByGroupID(m_AllStructures, 0);
                file.BuildTable(dataGridView1, items);
            }
            else
            {
                groupbox_grouping.Visible = false;
                groupbox_grouping.Dock = DockStyle.None;
                dataGridView1.Dock = DockStyle.Fill;
                file.BuildTable(dataGridView1, file.GetStructures());
                dataGridView1.EditMode = DataGridViewEditMode.EditOnF2;
            }
        }

        private void DoSave()
        {
            string path = SaveBmdDialog();

            if (string.IsNullOrEmpty(path))
                return;


            if (m_File.MyType == BmdFile.FileType.Item)
            {
                m_File.SaveAsBmd(path, m_AllStructures);
            }
            else
            {
                m_File.SaveAsBmd(path, dataGridView1);
            }


                Main.Log("File {0} saved", path);

                m_IsSaved = true;

                Properties.Settings.Default.SaveFolder = Path.GetDirectoryName(path);
                Properties.Settings.Default.Save();

              //  Close();

            
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSave();
        }

        void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_IsSaved && m_NeedsSaving)
            {
                DialogResult res = MessageBox.Show("Do you want to save this file ?\nAll unsaved content will be lost.", "Closing file", MessageBoxButtons.YesNoCancel);


                switch (res)
                {
                    case DialogResult.Yes:
                        {
                            DoSave();
                        }
                        break;
                    case DialogResult.Cancel:
                        {
                            e.Cancel = true;
                        }
                        break;
                }
            }

        }

        private string SaveBmdDialog()
        {
            SaveFileDialog Dialog = new SaveFileDialog();

            Dialog.InitialDirectory = m_SaveFolderPath;
            Dialog.Filter = "MU Config File (*.bmd)|*.bmd";
            // ----
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                m_SaveFolderPath = Path.GetDirectoryName(Dialog.FileName);
                Properties.Settings.Default.SaveFolder = m_SaveFolderPath;
                return Dialog.FileName;
            }
            // ----
            return "";
        }

        private string SaveAsTextDialog()
        {
            SaveFileDialog Dialog = new SaveFileDialog();

            Dialog.InitialDirectory = m_SaveFolderPath;
            Dialog.Filter = "MU Config File (*.txt)|*.txt";
            // ----
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                m_SaveFolderPath = Path.GetDirectoryName(Dialog.FileName);
                Properties.Settings.Default.SaveFolder = m_SaveFolderPath;
                return Dialog.FileName;
            }
            // ----
            return "";
        }


        private string SaveAsXmlDialog()
        {
            SaveFileDialog Dialog = new SaveFileDialog();

            Dialog.InitialDirectory = m_SaveFolderPath;
            Dialog.Filter = "MU Config File (*.xml)|*.xml";
            // ----
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                m_SaveFolderPath = Path.GetDirectoryName(Dialog.FileName);
                Properties.Settings.Default.SaveFolder = m_SaveFolderPath;
                return Dialog.FileName;
            }
            // ----
            return "";
        }


        //warning: will have to modify if we will add Insert row feature
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].HeaderCell.Value = e.RowIndex.ToString();
           // m_NeedsSaving = true;

  
        }

        

        //re-enumerates lines
        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            m_File.EnumerateLines(dataGridView1);
            m_NeedsSaving = true;
        }


        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoToLine frm = new GoToLine(dataGridView1, "Go to line");
            frm.ShowDialog();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchOrReplace frm = new SearchOrReplace(dataGridView1, "Search or replace");
            frm.ShowDialog();
        }

        private void dataGridView1_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                new AboutBox().ShowDialog();
            }

            if (e.Control && e.KeyCode == Keys.F)
            {
                new SearchOrReplace(dataGridView1, "Search or replace").ShowDialog();
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {

                DoSave();
            }
            else if (e.KeyCode == Keys.F5)
            {
                MessageBox.Show("Convert");
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                new GoToLine(dataGridView1, "Go to line").ShowDialog();

            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (dataGridView1.AllowUserToDeleteRows)
                {
                    if (dataGridView1.SelectedRows.Count > 0
                        && dataGridView1.SelectedRows[0].Index != dataGridView1.Rows.Count - 1)
                    {
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                    }
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                if (dataGridView1.AllowUserToAddRows)
                {
                    try
                    {
                        

                        if (m_File.MyType == BmdFile.FileType.Item)
                        {

                            // Item tmp = (Item)m_File;
                            int n = dataGridView1.Rows.Count - 1;
                            dataGridView1.Rows.Insert(n, new DataGridViewRow());
                            DataGridViewRow prev_row = dataGridView1.Rows[n - 1];

                            dataGridView1.Rows[n].Cells[0].Value = (int.Parse(prev_row.Cells[0].Value.ToString()) + 1).ToString();
                            dataGridView1.Rows[n].Cells[1].Value = prev_row.Cells[1].Value;
                            dataGridView1.Rows[n].Cells[2].Value = (int.Parse(prev_row.Cells[2].Value.ToString()) + 1).ToString();
                            Misc.HighlightRow(dataGridView1, n, false);
                        }
                        else
                        {
                            int nRow = dataGridView1.SelectedCells[0].RowIndex;
                            dataGridView1.Rows.Insert(nRow, new DataGridViewRow());

                            m_File.EnumerateLines(dataGridView1);

                            //highlight newly added row
                            Misc.HighlightRow(dataGridView1, nRow, false);
                            
                        }

                        m_NeedsSaving = true;

                      
                    }
                    catch (Exception ex)
                    {
                        //...
                        Console.WriteLine(ex.ToString());
                    }
                }
             
            }
            else if (e.KeyCode == Keys.PageUp)
            {

            }
            else if (e.KeyCode == Keys.PageDown)
            {

            }



            //copy/paste
            if (e.Control && e.KeyCode == Keys.A)
            {
                dataGridView1.MultiSelect = true;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Selected = true;
                    }
                }
              //  dataGridView1.MultiSelect = false;
            }


            if (e.Control && e.KeyCode == Keys.C)
            {
                //Copy to clipboard
                dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                DataObject dataObj = dataGridView1.GetClipboardContent();
                if (dataObj != null)
                    Clipboard.SetDataObject(dataObj);
            }

            if (e.Control && e.KeyCode == Keys.Z)
            {

                if (m_ColumnActions.Count > 0)
                {
                    CellEditInfo a = m_ColumnActions.Pop();
                    dataGridView1[a.ColIndex, a.RowIndex].Value = a.Value;
                }
            }
        }

        
        void OnCellEditEnd(object sender, DataGridViewCellEventArgs e)
        {
            m_NeedsSaving = true;
            bool flag = false;
            //TODO: mov ethis to item.bmd (made in hurry)
            if (m_File.MyType == BmdFile.FileType.Item)
            {
                try
                {
                    //find structure with matching index
                    int index = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                    MuDef.MUFile_Item tmp_item = new MuDef.MUFile_Item();

                    for (int i = 0; i < m_AllStructures.Length; i++)
                    {
                        if (m_AllStructures[i] != null)
                        {
                            tmp_item = ((MuDef.MUFile_Item)(m_AllStructures[i]));

                          
                            if (tmp_item.ItemID == nCurItemID)
                            {
                                flag = true;
                                tmp_item = new MuDef.MUFile_Item();
                                tmp_item.ItemInfo = new MuDef.MUFile_ItemInfo()
                                {
                                    Classes = new byte[7],
                                    Resistances = new byte[7],
                                };

                                tmp_item.ItemID = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                                tmp_item.Index = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                                tmp_item.Number = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                                tmp_item.TexturePath = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                                tmp_item.ModelName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();


                                tmp_item.ItemInfo.Name = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                                tmp_item.ItemInfo.Type1 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                                tmp_item.ItemInfo.Type2 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                                tmp_item.ItemInfo.Type3 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());

                                tmp_item.ItemInfo.TwoHands = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());

                                tmp_item.ItemInfo.ItemLvl = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString());
                                tmp_item.ItemInfo.ItemSlot = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString());
                                tmp_item.ItemInfo.ItemSkill = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString());

                                tmp_item.ItemInfo.X = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString());
                                tmp_item.ItemInfo.Y = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString());

                                tmp_item.ItemInfo.DmgMin = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[15].Value.ToString());
                                tmp_item.ItemInfo.DmgMax = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[16].Value.ToString());
                                tmp_item.ItemInfo.DefRate = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[17].Value.ToString());
                                tmp_item.ItemInfo.Defence = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[18].Value.ToString());
                                tmp_item.ItemInfo.Unknown1 = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[19].Value.ToString());

                                tmp_item.ItemInfo.AtkSpeed = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[20].Value.ToString());
                                tmp_item.ItemInfo.WalkSpeed = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[21].Value.ToString());
                                tmp_item.ItemInfo.Durability = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[22].Value.ToString());
                                tmp_item.ItemInfo.MagicDur = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[23].Value.ToString());
                                tmp_item.ItemInfo.MagicPwr = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[24].Value.ToString());
                                tmp_item.ItemInfo.Unknown2 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[25].Value.ToString());


                                tmp_item.ItemInfo.ReqStr = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[26].Value.ToString());
                                tmp_item.ItemInfo.ReqDex = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[27].Value.ToString());
                                tmp_item.ItemInfo.ReqEne = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[28].Value.ToString());
                                tmp_item.ItemInfo.ReqVit = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[29].Value.ToString());
                                tmp_item.ItemInfo.ReqLea = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[30].Value.ToString());
                                tmp_item.ItemInfo.ReqLvl = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[31].Value.ToString());
                                tmp_item.ItemInfo.ItemValue = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[32].Value.ToString());

                                tmp_item.ItemInfo.Zen = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[33].Value.ToString());
                                tmp_item.ItemInfo.SetOption = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[34].Value.ToString());

                                //clasees
                                tmp_item.ItemInfo.Classes[0] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[35].Value.ToString());
                                tmp_item.ItemInfo.Classes[1] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[36].Value.ToString());
                                tmp_item.ItemInfo.Classes[2] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[37].Value.ToString());
                                tmp_item.ItemInfo.Classes[3] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[38].Value.ToString());
                                tmp_item.ItemInfo.Classes[4] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[39].Value.ToString());
                                tmp_item.ItemInfo.Classes[5] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[40].Value.ToString());
                                tmp_item.ItemInfo.Classes[6] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[41].Value.ToString());

                                //resist
                                tmp_item.ItemInfo.Resistances[0] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[42].Value.ToString());
                                tmp_item.ItemInfo.Resistances[1] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[43].Value.ToString());
                                tmp_item.ItemInfo.Resistances[2] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[44].Value.ToString());
                                tmp_item.ItemInfo.Resistances[3] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[45].Value.ToString());
                                tmp_item.ItemInfo.Resistances[4] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[46].Value.ToString());
                                tmp_item.ItemInfo.Resistances[5] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[47].Value.ToString());
                                tmp_item.ItemInfo.Resistances[6] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[48].Value.ToString());

                                //unknown
                                tmp_item.ItemInfo.IsApplyToDrop = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[49].Value.ToString());
                                tmp_item.ItemInfo.Unknown3 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[50].Value.ToString());
                                tmp_item.ItemInfo.Unknown4 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[51].Value.ToString());
                                tmp_item.ItemInfo.Unknown5 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[52].Value.ToString());
                                tmp_item.ItemInfo.Unknown6 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[53].Value.ToString());
                                tmp_item.ItemInfo.IsExpensive = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[54].Value.ToString());
                                tmp_item.ItemInfo.Unknown7 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[55].Value.ToString());
                                tmp_item.ItemInfo.StackMax = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[56].Value.ToString());

                                //???
                                tmp_item.ItemInfo.Pad = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[57].Value.ToString());

                                m_AllStructures[i] = tmp_item;
                            }
                        }







                    }


                    if (!flag)
                    {
                        int n = Item.GetFirstFreeItemPlace(m_AllStructures, CurrentGroupID);
                        bool success = true;
                     


                        tmp_item = new MuDef.MUFile_Item();
                        tmp_item.ItemInfo = new MuDef.MUFile_ItemInfo()
                        {
                            Classes = new byte[7],
                            Resistances = new byte[7],
                        };

                        try
                        {
                            tmp_item.ItemID = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            tmp_item.Index = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                            tmp_item.Number = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                            tmp_item.TexturePath = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                            tmp_item.ModelName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();


                            tmp_item.ItemInfo.Name = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                            tmp_item.ItemInfo.Type1 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                            tmp_item.ItemInfo.Type2 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                            tmp_item.ItemInfo.Type3 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString());

                            tmp_item.ItemInfo.TwoHands = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString());

                            tmp_item.ItemInfo.ItemLvl = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString());
                            tmp_item.ItemInfo.ItemSlot = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString());
                            tmp_item.ItemInfo.ItemSkill = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString());

                            tmp_item.ItemInfo.X = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[13].Value.ToString());
                            tmp_item.ItemInfo.Y = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[14].Value.ToString());

                            tmp_item.ItemInfo.DmgMin = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[15].Value.ToString());
                            tmp_item.ItemInfo.DmgMax = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[16].Value.ToString());
                            tmp_item.ItemInfo.DefRate = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[17].Value.ToString());
                            tmp_item.ItemInfo.Defence = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[18].Value.ToString());
                            tmp_item.ItemInfo.Unknown1 = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[19].Value.ToString());

                            tmp_item.ItemInfo.AtkSpeed = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[20].Value.ToString());
                            tmp_item.ItemInfo.WalkSpeed = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[21].Value.ToString());
                            tmp_item.ItemInfo.Durability = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[22].Value.ToString());
                            tmp_item.ItemInfo.MagicDur = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[23].Value.ToString());
                            tmp_item.ItemInfo.MagicPwr = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[24].Value.ToString());
                            tmp_item.ItemInfo.Unknown2 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[25].Value.ToString());


                            tmp_item.ItemInfo.ReqStr = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[26].Value.ToString());
                            tmp_item.ItemInfo.ReqDex = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[27].Value.ToString());
                            tmp_item.ItemInfo.ReqEne = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[28].Value.ToString());
                            tmp_item.ItemInfo.ReqVit = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[29].Value.ToString());
                            tmp_item.ItemInfo.ReqLea = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[30].Value.ToString());
                            tmp_item.ItemInfo.ReqLvl = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[31].Value.ToString());
                            tmp_item.ItemInfo.ItemValue = ushort.Parse(dataGridView1.Rows[e.RowIndex].Cells[32].Value.ToString());

                            tmp_item.ItemInfo.Zen = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[33].Value.ToString());
                            tmp_item.ItemInfo.SetOption = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[34].Value.ToString());

                            //clasees
                            tmp_item.ItemInfo.Classes[0] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[35].Value.ToString());
                            tmp_item.ItemInfo.Classes[1] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[36].Value.ToString());
                            tmp_item.ItemInfo.Classes[2] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[37].Value.ToString());
                            tmp_item.ItemInfo.Classes[3] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[38].Value.ToString());
                            tmp_item.ItemInfo.Classes[4] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[39].Value.ToString());
                            tmp_item.ItemInfo.Classes[5] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[40].Value.ToString());
                            tmp_item.ItemInfo.Classes[6] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[41].Value.ToString());

                            //resist
                            tmp_item.ItemInfo.Resistances[0] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[42].Value.ToString());
                            tmp_item.ItemInfo.Resistances[1] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[43].Value.ToString());
                            tmp_item.ItemInfo.Resistances[2] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[44].Value.ToString());
                            tmp_item.ItemInfo.Resistances[3] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[45].Value.ToString());
                            tmp_item.ItemInfo.Resistances[4] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[46].Value.ToString());
                            tmp_item.ItemInfo.Resistances[5] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[47].Value.ToString());
                            tmp_item.ItemInfo.Resistances[6] = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[48].Value.ToString());

                            tmp_item.ItemInfo.IsApplyToDrop = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[49].Value.ToString());
                            tmp_item.ItemInfo.Unknown3 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[50].Value.ToString());
                            tmp_item.ItemInfo.Unknown4 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[51].Value.ToString());
                            tmp_item.ItemInfo.Unknown5 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[52].Value.ToString());
                            tmp_item.ItemInfo.Unknown6 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[53].Value.ToString());
                            tmp_item.ItemInfo.IsExpensive = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[54].Value.ToString());
                            tmp_item.ItemInfo.Unknown7 = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[55].Value.ToString());
                            tmp_item.ItemInfo.StackMax = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[56].Value.ToString());

                            //???
                            tmp_item.ItemInfo.Pad = byte.Parse(dataGridView1.Rows[e.RowIndex].Cells[57].Value.ToString());
                        }
                        catch { success = false; }

                        if (success)
                        {
                            m_AllStructures[n] = new MuDef.MUFile_Item()
                            {
                                ItemInfo = new MuDef.MUFile_ItemInfo()
                                {
                                    Classes = new byte[7],
                                    Resistances = new byte[7],
                                }
                            };

                            m_AllStructures[n] = tmp_item;
                        }
                    }

                   
                }
                catch { }
            }
            /*else if (m_File.MyType == BmdFile.FileType.ItemTRSData)
            {
                try
                {
                    //find structure with matching index
                    int index = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                    MuDef.MUFile_ItemTRSData tmp_item = new MuDef.MUFile_ItemTRSData();

                    for (int i = 0; i < m_AllStructures.Length; i++)
                    {
                        if (m_AllStructures[i] != null)
                        {
                            tmp_item = ((MuDef.MUFile_ItemTRSData)(m_AllStructures[i]));


                            if (tmp_item.ItemCode == nCurItemID)
                            {
                                flag = true;
                                tmp_item = new MuDef.MUFile_ItemTRSData();
                                tmp_item.ItemCode = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                                tmp_item.TranslationX = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                                tmp_item.TranslationY = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                                tmp_item.TranslationZ = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                                tmp_item.Rotation = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                                tmp_item.ScaleX = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                                tmp_item.ScaleY = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                                tmp_item.ScaleZ = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                                m_AllStructures[i] = tmp_item;
                            }
                        }
                    }


                    if (!flag)
                    {
                        int n = Item.GetFirstFreeItemPlace(m_AllStructures, CurrentGroupID);
                        bool success = true;

                        tmp_item = new MuDef.MUFile_ItemTRSData();


                        try
                        {
                            tmp_item.ItemCode = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                            tmp_item.TranslationX = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                            tmp_item.TranslationY = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                            tmp_item.TranslationZ = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                            tmp_item.Rotation = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
                            tmp_item.ScaleX = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString());
                            tmp_item.ScaleY = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                            tmp_item.ScaleZ = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
                        }
                        catch { success = false; }

                        if (success)
                        {
                            m_AllStructures[n] = new MuDef.MUFile_ItemTRSData();
                            m_AllStructures[n] = tmp_item;
                        }
                    }


                }
                catch { }
            }*/

        }

        

        void OnCellClicked(object sender, DataGridViewCellEventArgs e)
        {
           // dataGridView1.BeginEdit(false);

            if (m_File.MyType == BmdFile.FileType.Item || m_File.MyType == BmdFile.FileType.ItemTRSData)
            {
                try
                {
                    nCurItemID = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
                catch (NullReferenceException)
                { }
            }

            if (dataGridView1.MultiSelect)
            {
                dataGridView1.MultiSelect = false;
            }
        }

        void OnCellEditBegin(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_NeedsSaving = true;

            
            CellEditInfo action = new CellEditInfo();

            action.ColIndex = e.ColumnIndex;
            action.RowIndex = e.RowIndex;

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                action.Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            else action.Value = "";

            m_ColumnActions.Push(action);

        }

        private void asTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_File.AllowedConversationModes.Contains(BmdFile.BmdConversationType.BmdToText))
            {
                string fpath = SaveAsTextDialog();
                if (!string.IsNullOrEmpty(fpath))
                {
                    m_File.SaveAsText(fpath, dataGridView1);
                }
            }
            else
            {
                MessageBox.Show("This BMD file cannot be saved as text.");
            }
        }

        private void xmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_File.AllowedConversationModes.Contains(BmdFile.BmdConversationType.BmdToXml))
            {
                string fpath = SaveAsXmlDialog();
                if (!string.IsNullOrEmpty(fpath))
                {
                    m_File.SaveAsXml(fpath, dataGridView1);
                }
            }
            else
            {
                MessageBox.Show("This BMD file cannot be saved as xml.");
            }
        }

        private void btn_prevgroup_Click(object sender, EventArgs e)
        {
            if (m_File.MyType == BmdFile.FileType.Item)
            {
                CurrentGroupID--;

                int n = CurrentGroupID;
                if (CurrentGroupID == 16 || CurrentGroupID < 0)
                {
                    n = 0;
                    CurrentGroupID = n;
                }

                object[] items = Item.GetItemsByGroupID(m_AllStructures, n);

                m_File.BuildTable(dataGridView1, items);
                label_groupid.Text = n.ToString();

            }
        }

        private void btn_nextgroup_Click(object sender, EventArgs e)
        {
            if (m_File.MyType == BmdFile.FileType.Item)
            {
                CurrentGroupID++;

                int n = CurrentGroupID;
                if (CurrentGroupID == 16 || CurrentGroupID < 0)
                {
                    n = 0;
                    CurrentGroupID = n;
                }

                object[] items = Item.GetItemsByGroupID(m_AllStructures, n);

                m_File.BuildTable(dataGridView1, items);
                label_groupid.Text = n.ToString();
            }
        }


    }
}
