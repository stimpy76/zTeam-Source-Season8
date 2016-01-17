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
    class ItemTRSData : BmdFile
    {
        const int cTotalStructCount = 512 * 5;

        //nothing to do here, just pass data to generic constructor
        public ItemTRSData(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = true;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "ItemCode", "TranslationX", "TranslationY", "TranslationZ", "Rotation",
                                 "ScaleX", "ScaleY", "ScaleZ" };

            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);
            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_ItemTRSData Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemTRSData)items[i];
                    dgv.Rows.Add(Item.ItemCode, Item.TranslationX, Item.TranslationY, Item.TranslationZ,
                        Item.Rotation, Item.ScaleX, Item.ScaleY, Item.ScaleZ);
                }
            }

            EnumerateLines(dgv);
        }

        public override object GetStructure()
        {
            return 0;
        }

        public override object[] GetStructures()
        {
            int RealLineCount = ReadInt(m_FileStream);
            int LineCount = cTotalStructCount;
            object[] Res = new object[LineCount];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemTRSData));
            MuDef.MUFile_ItemTRSData CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * LineCount];
            m_FileStream.Read(m_FileBuffer, 0, m_FileBuffer.Length);

            MemoryStream ms = new MemoryStream(m_FileBuffer);
            byte[] item_buf = new byte[SizeOfItem];

            try
            {
                while ((ms.Read(item_buf, 0, item_buf.Length) == SizeOfItem))
                {
                    XorFilter(ref item_buf, SizeOfItem);
                    object Item;

                    if (m_CurrentLine < RealLineCount)
                    {
                        Item = Marshal.PtrToStructure(
                               Marshal.UnsafeAddrOfPinnedArrayElement(item_buf, 0),
                               typeof(MuDef.MUFile_ItemTRSData));
                    }
                    else
                    {
                        Item = new MuDef.MUFile_ItemTRSData();
                    }
                    Res[m_CurrentLine++] = (object)Item;
                }
            }
            
            catch (ArgumentException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }
            catch// (Exception e)
            {
                MessageBox.Show("Failed to read file structures.");
            }
            CloseSourceFile();
            return Res;
        }

        public override void SaveAsBmd(string OutputPath, DataGridView dgv)
        {
            //try
            //{
                int FilledRowCount = dgv.Rows.Count;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_ItemTRSData> TmpList = new List<MuDef.MUFile_ItemTRSData>(dgv.Rows.Count);
                MuDef.MUFile_ItemTRSData CurrentItem = new MuDef.MUFile_ItemTRSData();

                for (int i = 0; i < FilledRowCount; i++)
                {
                    if (dgv.Rows[i].Cells[0].Value != null && 
                        (int.Parse(dgv.Rows[i].Cells[0].Value.ToString()) > 0 
                        || float.Parse(dgv.Rows[i].Cells[4].Value.ToString()) > 0.0f))
                    {
                        CurrentItem.ItemCode = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.TranslationX = float.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.TranslationY = float.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.TranslationZ = float.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.Rotation = float.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.ScaleX = float.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.ScaleY = float.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.ScaleZ = float.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_ItemTRSData));
                int TotalSize = TmpList.Count * ItemSize;

                byte[] FileBuffer = new byte[TotalSize];

                for (int i = 0; i < TmpList.Count; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemTRSData)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(BitConverter.GetBytes(TmpList.Count), 0, sizeof(int));
                OutputStream.Write(FileBuffer, 0, TotalSize);
                OutputStream.Write(BitConverter.GetBytes(GetCRC(FileBuffer, FileBuffer.Length)), 0, sizeof(uint));

                OutputStream.Flush();
                OutputStream.Close();
          //  }
           // catch (Exception e)
           // { MessageBox.Show("Failed to save file"); }
        }
    }
}
