using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;
using System.IO;

using zFileMgr.MuGeneric;

namespace zFileMgr.MuBMD
{
    class SocketItem : BmdFile
    {
        const int cMaxGroups = 3;
        const int cMaxOptions = 50;

        //nothing to do here, just pass data to generic constructor
        public SocketItem(string _path, FileType _type) : base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "Index", "Type", "Name", "Option1", "Option2", "Option3", "Option4", "Option5", "Option6",
                                     "SetOption1", "SetOption2", "SetOption3", "SetOption4", "SetOption5", "SetOption6"};


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_SocketItem Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_SocketItem)items[i];
                    dgv.Rows.Add(
                                   Item.Index,
                                   Item.Type,
                                   Item.Name,
                                   Item.Option1,
                                   Item.Option2,
                                   Item.Option3,
                                   Item.Option4,
                                   Item.Option5,
                                   Item.Option6,
                                   Item.SetOption1,
                                   Item.SetOption2,
                                   Item.SetOption3,
                                   Item.SetOption4,
                                   Item.SetOption5
                                  // Item.SetOption6
                                  

                        );
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
            object[] Res = new object[cMaxOptions * cMaxGroups];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_SocketItem));
            MuDef.MUFile_SocketItem CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cMaxOptions * cMaxGroups];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_SocketItem));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_SocketItem)Item;

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

                List<MuDef.MUFile_Gate> TmpList = new List<MuDef.MUFile_Gate>(dgv.Rows.Count);
                MuDef.MUFile_Gate CurrentItem = new MuDef.MUFile_Gate();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Type = byte.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.MapNumber = byte.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.X1 = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.Y1 = byte.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.X2 = byte.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.Y2 = byte.Parse(dgv.Rows[i].Cells[5].Value.ToString());

                        CurrentItem.Target = ushort.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.Dir = ushort.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        CurrentItem.Level = ushort.Parse(dgv.Rows[i].Cells[8].Value.ToString());
                        CurrentItem.MaxLevel = ushort.Parse(dgv.Rows[i].Cells[9].Value.ToString());
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_Credit));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_Credit)));
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
