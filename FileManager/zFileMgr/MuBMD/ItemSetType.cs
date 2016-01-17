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
    class ItemSetType : BmdFile
    {
        const int cTotalStructCount = 512 * 15;

        //nothing to do here, just pass data to generic constructor
        public ItemSetType(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "Group", "Set1", "Set2", "Level1", "Level2" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_ItemSetType Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemSetType)items[i];
                    dgv.Rows.Add(i / 512,
                                   Item.Set1,
                                   Item.Set2,
                                   Item.Level1,
                                   Item.Level2

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetType));
            MuDef.MUFile_ItemSetType CurrentItem;

            // int LineCount = ReadInt(m_FileStream);
            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];



            m_FileStream.Read(m_FileBuffer, 0, m_FileBuffer.Length);

            XorFilter(ref m_FileBuffer, m_FileBuffer.Length);

            MemoryStream ms = new MemoryStream(m_FileBuffer);
            byte[] item_buf = new byte[SizeOfItem];

            try
            {
                while ((ms.Read(item_buf, 0, item_buf.Length) == SizeOfItem))
                {
                    object Item = Marshal.PtrToStructure(
                           Marshal.UnsafeAddrOfPinnedArrayElement(item_buf, 0),
                           typeof(MuDef.MUFile_ItemSetType));

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
                //last row is null
                int FilledRowCount = dgv.Rows.Count;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_ItemSetType> TmpList = new List<MuDef.MUFile_ItemSetType>(dgv.Rows.Count);
                MuDef.MUFile_ItemSetType CurrentItem = new MuDef.MUFile_ItemSetType();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Set1 = byte.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.Set2 = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.Level1 = byte.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.Level2 = byte.Parse(dgv.Rows[i].Cells[4].Value.ToString());

                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetType));
                int TotalSize = cTotalStructCount * ItemSize;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    // XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemAddOption)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                XorFilter(ref FileBuffer, FileBuffer.Length);
                OutputStream.Write(FileBuffer, 0, TotalSize);


                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }


    }
}
