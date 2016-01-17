using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Text;
using zFileMgr.MuGeneric;

namespace zFileMgr.MuBMD
{
    class NPCDialog : BmdFile
    {
        const int cTotalStructCount = 400;

        //nothing to do here, just pass data to generic constructor
        public NPCDialog(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = new string[58];
            for (int i = 1; i <= 58; i++)
            {
                col_names[i - 1] = i.ToString();
            }


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_NPCDialog Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_NPCDialog)items[i];

                    object[] rowparams = new object[58];
                    for (int j = 0; j < 58; j++) rowparams[j] = Item.Values[j];

                    dgv.Rows.Add(rowparams);
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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_NPCDialog));
            MuDef.MUFile_NPCDialog CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {



                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_NPCDialog));

                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_NPCDialog)Item;

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

                List<MuDef.MUFile_NPCDialog> TmpList = new List<MuDef.MUFile_NPCDialog>(dgv.Rows.Count);
                MuDef.MUFile_NPCDialog CurrentItem = new MuDef.MUFile_NPCDialog();


                
                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Values = new byte[58];
                        for (int j = 0; j < 58; j++)
                        {
                            CurrentItem.Values[j] = byte.Parse(dgv.Rows[i].Cells[j].Value.ToString());
                        }
                       
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_NPCDialog));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_NPCDialog)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }

}
