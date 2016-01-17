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
    class ItemLevelTooltip : BmdFile
    {
        const int cTotalStructCount = 400;

        //nothing to do here, just pass data to generic constructor
        public ItemLevelTooltip(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "ID", "Name", "Unknown1", "Unknown2", "Unknown3", "Unknown4",
                                     "Unknown5", "Unknown6",  "Unknown7", "Unknown8",  "Unknown9", "Unknown10", 
                                      "Unknown11", "Unknown12",  "Unknown13", "Unknown14",  "Unknown15", "Unknown16", 
                                       "Unknown17", "Unknown18",  "Unknown19", "Unknown20"};


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_ItemLevelTooltip Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemLevelTooltip)items[i];
                    dgv.Rows.Add(
                      Item.ID,
                      Item.Name,

                      //4
                      Item.Unknown1[0],
                      Item.Unknown1[1],
                      Item.Unknown1[2],
                      Item.Unknown1[3],

                      //16

                      Item.Unknown2[0],
                      Item.Unknown2[1],
                      Item.Unknown2[2],
                      Item.Unknown2[3],
                      Item.Unknown2[4],
                      Item.Unknown2[5],
                      Item.Unknown2[6],
                      Item.Unknown2[7],
                      Item.Unknown2[8],
                      Item.Unknown2[9],
                      Item.Unknown2[10],
                      Item.Unknown2[11],
                      Item.Unknown2[12],
                      Item.Unknown2[13],
                      Item.Unknown2[14],
                      Item.Unknown2[15]

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemLevelTooltip));
            MuDef.MUFile_ItemLevelTooltip CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {



                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_ItemLevelTooltip));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_ItemLevelTooltip)Item;

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
                int FilledRowCount = dgv.Rows.Count; //;- 1;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_ItemLevelTooltip> TmpList = new List<MuDef.MUFile_ItemLevelTooltip>(dgv.Rows.Count);
                MuDef.MUFile_ItemLevelTooltip CurrentItem = new MuDef.MUFile_ItemLevelTooltip();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Unknown1 = new byte[4];
                        CurrentItem.Unknown2 = new short[16];

                        CurrentItem.ID = short.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.Name = dgv.Rows[i].Cells[1].Value.ToString();

                        for (int j = 0; j < 4; j++)
                        {
                            CurrentItem.Unknown1[j] =
                                byte.Parse(dgv.Rows[i].Cells[2 + j].Value.ToString());
                        }

                        for (int j = 0; j < 16; j++)
                        {
                            CurrentItem.Unknown2[j] =
                                short.Parse(dgv.Rows[i].Cells[6 + j].Value.ToString());
                        }

                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_ItemLevelTooltip));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemLevelTooltip)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);


                byte[] crc = new byte[4];
                crc = BitConverter.GetBytes(GetCRC(FileBuffer, TotalSize));

                OutputStream.Write(crc, 0, crc.Length);

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }

}
