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
    class HelpData : BmdFile
    {
        const int cTotalStructCount = 520;

        //nothing to do here, just pass data to generic constructor
        public HelpData(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "ID", "bt1", "bt2", "Text1", "Text2", "Text3" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_HelpData Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_HelpData)items[i];
                    dgv.Rows.Add(
                      Item.ID,
                      Item.bt1,
                      Item.bt2,
                      Item.Text1,
                      Item.Text2,
                      Item.Text3

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
            object[] Res = new object[cTotalStructCount];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_HelpData));
            MuDef.MUFile_HelpData CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {



                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_HelpData));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_HelpData)Item;

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
                int FilledRowCount = dgv.Rows.Count;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_HelpData> TmpList = new List<MuDef.MUFile_HelpData>(dgv.Rows.Count);
                MuDef.MUFile_HelpData CurrentItem = new MuDef.MUFile_HelpData();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.ID = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.bt1 = byte.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.bt2 = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.Text1 = dgv.Rows[i].Cells[3].Value.ToString();
                        CurrentItem.Text2 = dgv.Rows[i].Cells[4].Value.ToString();
                        CurrentItem.Text3 = dgv.Rows[i].Cells[5].Value.ToString();

                   
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_HelpData));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_HelpData)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);

                byte[] crc = new byte[4];
                crc = BitConverter.GetBytes(GetCRC(FileBuffer, FileBuffer.Length));

                OutputStream.Write(crc, 0, crc.Length);
                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
