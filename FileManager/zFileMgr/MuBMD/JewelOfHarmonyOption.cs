using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.IO;

using zFileMgr.Utils;
using zFileMgr.MuGeneric;

namespace zFileMgr.MuBMD
{
    class JewelOfHarmonyOption : BmdFile
    {
        const int cTotalStructCount = 30;

        //nothing to do here, just pass data to generic constructor
        public JewelOfHarmonyOption(string _path, FileType _type) : base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            

            DataGridViewColumn col;
            string[] col_names = {
                                     "Group",
                                     "Index",
                                     "Name",
                                     "Level1",
                                     "Level2",
                                     "Level3",
                                     "Level4",
                                     "Level5",
                                     "Level6",
                                     "Level7",
                                     "Level8",
                                     "Level9",
                                     "Level10",
                                     "Level11",
                                     "Level12",
                                     "Level13",
                                     "Level14",
                                     "Zen1",
                                     "Zen2",
                                     "Zen3",
                                     "Zen4",
                                     "Zen5",
                                     "Zen6",
                                     "Zen7",
                                     "Zen8",
                                     "Zen9",
                                     "Zen10",
                                     "Zen11",
                                     "Zen12",
                                     "Zen13",
                                     "Zen14"
                                 };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                if (i == 0)
                    
                    col.ReadOnly = true;

                dgv.Columns.Add(col);

            }

            int nGroup = 0;
           
            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_JewelOfHarmonyOption Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_JewelOfHarmonyOption)items[i];
                    if (i == 0 || ((i -1 ) % 10  == 0))
                    {
                       
                        dgv.Rows.Add(
                                       ++nGroup,
                                       Item.Index,
                                       Item.Name,
                                       Item.Level1,
                                       Item.Level2,
                                       Item.Level3,
                                       Item.Level4,
                                       Item.Level5,
                                       Item.Level6,
                                       Item.Level7,
                                       Item.Level8,
                                       Item.Level9,
                                       Item.Level10,
                                       Item.Level11,
                                       Item.Level12,
                                       Item.Level13,
                                       Item.Level14,

                                       Item.Zen1,
                                       Item.Zen2,
                                       Item.Zen3,
                                       Item.Zen4,
                                       Item.Zen5,
                                       Item.Zen6,
                                       Item.Zen7,
                                       Item.Zen8,
                                       Item.Zen9,
                                       Item.Zen10,
                                       Item.Zen11,
                                       Item.Zen12,
                                       Item.Zen13,
                                       Item.Zen14

                            );

                        Misc.HighlightRow(dgv, i - 1, true);
                    }
                    else
                    {
                        dgv.Rows.Add(
                                       "",
                                       Item.Index,
                                       Item.Name,
                                       Item.Level1,
                                       Item.Level2,
                                       Item.Level3,
                                       Item.Level4,
                                       Item.Level5,
                                       Item.Level6,
                                       Item.Level7,
                                       Item.Level8,
                                       Item.Level9,
                                       Item.Level10,
                                       Item.Level11,
                                       Item.Level12,
                                       Item.Level13,
                                       Item.Level14,

                                       Item.Zen1,
                                       Item.Zen2,
                                       Item.Zen3,
                                       Item.Zen4,
                                       Item.Zen5,
                                       Item.Zen6,
                                       Item.Zen7,
                                       Item.Zen8,
                                       Item.Zen9,
                                       Item.Zen10,
                                       Item.Zen11,
                                       Item.Zen12,
                                       Item.Zen13,
                                       Item.Zen14
                                       );
                        Misc.HighlightRow(dgv, i - 1, false);
                        
                    }
                }
            }

            //enumerate rows
            EnumerateLines(dgv);

        }

        public override object GetStructure()
        {
            return 0;
        }

        public override object[] GetStructures()
        {
            object[] Res = new object[cTotalStructCount];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_JewelOfHarmonyOption));
            MuDef.MUFile_JewelOfHarmonyOption CurrentItem;


            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_JewelOfHarmonyOption));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_JewelOfHarmonyOption)Item;

                    Res[m_CurrentLine] = (object)CurrentItem;


                }
            }
            //todo: prevent
            catch (IndexOutOfRangeException)
            {

            }
            catch (Exception)
            {
                MessageBox.Show("Failed to read file structures.");
            }
            CloseSourceFile();
            return Res;
        }

        public override void SaveAsBmd(string OutputPath, DataGridView dgv)
        {
            try
            {
                //last row is null
                int FilledRowCount = dgv.Rows.Count - 1;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_JewelOfHarmonyOption> TmpList = new List<MuDef.MUFile_JewelOfHarmonyOption>(dgv.Rows.Count);
                MuDef.MUFile_JewelOfHarmonyOption CurrentItem = new MuDef.MUFile_JewelOfHarmonyOption();

                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Index = int.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.Name = dgv.Rows[i].Cells[2].Value.ToString();

                        CurrentItem.Level1 = int.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.Level2 = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.Level3 = int.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.Level4 = int.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.Level5 = int.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        CurrentItem.Level7 = int.Parse(dgv.Rows[i].Cells[8].Value.ToString());
                        CurrentItem.Level8 = int.Parse(dgv.Rows[i].Cells[9].Value.ToString());
                        CurrentItem.Level9 = int.Parse(dgv.Rows[i].Cells[10].Value.ToString());
                        CurrentItem.Level10 = int.Parse(dgv.Rows[i].Cells[11].Value.ToString());
                        CurrentItem.Level11 = int.Parse(dgv.Rows[i].Cells[12].Value.ToString());
                        CurrentItem.Level12 = int.Parse(dgv.Rows[i].Cells[13].Value.ToString());
                        CurrentItem.Level13 = int.Parse(dgv.Rows[i].Cells[14].Value.ToString());
                        CurrentItem.Level14 = int.Parse(dgv.Rows[i].Cells[15].Value.ToString());

                        CurrentItem.Zen1 = int.Parse(dgv.Rows[i].Cells[16].Value.ToString());
                        CurrentItem.Zen2 = int.Parse(dgv.Rows[i].Cells[17].Value.ToString());
                        CurrentItem.Zen3 = int.Parse(dgv.Rows[i].Cells[18].Value.ToString());
                        CurrentItem.Zen4 = int.Parse(dgv.Rows[i].Cells[19].Value.ToString());
                        CurrentItem.Zen5 = int.Parse(dgv.Rows[i].Cells[20].Value.ToString());
                        CurrentItem.Zen6 = int.Parse(dgv.Rows[i].Cells[21].Value.ToString());
                        CurrentItem.Zen7 = int.Parse(dgv.Rows[i].Cells[22].Value.ToString());
                        CurrentItem.Zen8 = int.Parse(dgv.Rows[i].Cells[23].Value.ToString());
                        CurrentItem.Zen9 = int.Parse(dgv.Rows[i].Cells[24].Value.ToString());
                        CurrentItem.Zen10 = int.Parse(dgv.Rows[i].Cells[25].Value.ToString());
                        CurrentItem.Zen11 = int.Parse(dgv.Rows[i].Cells[26].Value.ToString());
                        CurrentItem.Zen12 = int.Parse(dgv.Rows[i].Cells[27].Value.ToString());
                        CurrentItem.Zen13 = int.Parse(dgv.Rows[i].Cells[28].Value.ToString());
                        CurrentItem.Zen14 = int.Parse(dgv.Rows[i].Cells[29].Value.ToString());

                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_JewelOfHarmonyOption));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_JewelOfHarmonyOption)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                // OutputStream.Write(BitConverter.GetBytes(FilledRowCount), 0, sizeof(int));
                OutputStream.Write(FileBuffer, 0, TotalSize);


                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
