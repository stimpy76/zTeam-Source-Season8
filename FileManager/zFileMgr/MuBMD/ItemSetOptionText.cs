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
    class ItemSetOptionText : BmdFile
    {
        const int cTotalStructCount = 512 * 15;

        public ItemSetOptionText(string _path, FileType _type) :
            base(_path, _type) { }

        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "Index", "Text" };

            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);
            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_ItemSetOptionText Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemSetOptionText)items[i];
                    dgv.Rows.Add(Item.Index, Item.Text);
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
            int LineCount = ReadInt(m_FileStream);
            object[] Res = new object[LineCount];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetOptionText));
            MuDef.MUFile_ItemSetOptionText CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * LineCount];
            m_FileStream.Read(m_FileBuffer, 0, m_FileBuffer.Length);

            MemoryStream ms = new MemoryStream(m_FileBuffer);
            byte[] item_buf = new byte[SizeOfItem];

            try
            {
                while ((ms.Read(item_buf, 0, item_buf.Length) == SizeOfItem))
                {
                    XorFilter(ref item_buf, SizeOfItem);
                    object Item = Marshal.PtrToStructure(
                           Marshal.UnsafeAddrOfPinnedArrayElement(item_buf, 0),
                           typeof(MuDef.MUFile_ItemSetOptionText));
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
            try
            {
                int FilledRowCount = dgv.Rows.Count;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_ItemSetOptionText> TmpList = new List<MuDef.MUFile_ItemSetOptionText>(dgv.Rows.Count);
                MuDef.MUFile_ItemSetOptionText CurrentItem = new MuDef.MUFile_ItemSetOptionText();

                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Index = byte.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.Text = dgv.Rows[i].Cells[1].Value.ToString();
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetOptionText));
                int TotalSize = FilledRowCount * ItemSize;

                byte[] FileBuffer = new byte[TotalSize];

                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetOptionText)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(BitConverter.GetBytes(FilledRowCount), 0, sizeof(int));
                OutputStream.Write(FileBuffer, 0, TotalSize);
                OutputStream.Write(BitConverter.GetBytes(GetCRC(FileBuffer, FileBuffer.Length)), 0, sizeof(uint));

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch (Exception e)
            { MessageBox.Show("Failed to save file"); }
        }
    }
}
