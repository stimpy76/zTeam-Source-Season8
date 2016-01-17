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
    class ItemAddOption : BmdFile
    {
        const int cTotalStructCount = 512 * 15;

        //nothing to do here, just pass data to generic constructor
        public ItemAddOption(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "Group", "Opt1", "Val1", "Opt2", "Val2", "Index", "Time" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_ItemAddOption Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemAddOption)items[i];
                    dgv.Rows.Add( i / 512,
                                   Item.ItemOpt1,
                                   Item.ItemVal1,
                                   Item.ItemOpt2,
                                   Item.ItemVal2,
                                   Item.Index,
                                   Item.Time

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemAddOption));
            MuDef.MUFile_ItemAddOption CurrentItem;

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
                           typeof(MuDef.MUFile_ItemAddOption));

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

                List<MuDef.MUFile_ItemAddOption> TmpList = new List<MuDef.MUFile_ItemAddOption>(dgv.Rows.Count);
                MuDef.MUFile_ItemAddOption CurrentItem = new MuDef.MUFile_ItemAddOption();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.ItemOpt1 = ushort.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.ItemVal1 = ushort.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.ItemOpt2 = ushort.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.ItemVal2 = ushort.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.Index = int.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.Time = int.Parse(dgv.Rows[i].Cells[6].Value.ToString());

                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_ItemAddOption));
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
