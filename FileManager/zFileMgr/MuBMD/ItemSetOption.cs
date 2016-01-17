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
    class ItemSetOption : BmdFile
    {
        const int cTotalStructCount = 512 * 15;

        public ItemSetOption(string _path, FileType _type) :
            base(_path, _type) { }

        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "Name", 
                                     "OptT00", "OptT01", "OptT02", "OptT03", "OptT04", "OptT05",
                                     "OptT10", "OptT11", "OptT12", "OptT13", "OptT14", "OptT15",
                                     "OptV00", "OptV01", "OptV02", "OptV03", "OptV04", "OptV05",
                                     "OptV10", "OptV11", "OptV12", "OptV13", "OptV14", "OptV15",
                                     "ExOptT00", "ExOptT01",  "ExOptV00", "ExOptV01",
                                     "SetOptCnt",
                                     "SetOptT0", "SetOptT1", "SetOptT2", "SetOptT3", "SetOptT4",
                                     "SetOptV0", "SetOptV1", "SetOptV2", "SetOptV3", "SetOptV4",
                                     "Wizard", "Knight", "Elf", "Gladiator", "Lord", "Summoner", "Fighter"
                                 };

            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);
            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_ItemSetOption Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemSetOption)items[i];
                    dgv.Rows.Add(Item.Name,
                        Item.OptionTable0[0], Item.OptionTable0[1], Item.OptionTable0[2],
                        Item.OptionTable0[3], Item.OptionTable0[4], Item.OptionTable0[5],
                        Item.OptionTable1[0], Item.OptionTable1[1], Item.OptionTable1[2],
                        Item.OptionTable1[3], Item.OptionTable1[4], Item.OptionTable1[5],
                        Item.OptionTableValue0[0], Item.OptionTableValue0[1], Item.OptionTableValue0[2],
                        Item.OptionTableValue0[3], Item.OptionTableValue0[4], Item.OptionTableValue0[5],
                        Item.OptionTableValue1[0], Item.OptionTableValue1[1], Item.OptionTableValue1[2],
                        Item.OptionTableValue1[3], Item.OptionTableValue1[4], Item.OptionTableValue1[5],
                        Item.ExOptionTable[0], Item.ExOptionTable[1], Item.ExOptionTableValue[0], 
                        Item.ExOptionTableValue[1], Item.FullOptionCount,
                        Item.FullOptionTable[0], Item.FullOptionTable[1], Item.FullOptionTable[2],
                        Item.FullOptionTable[3], Item.FullOptionTable[4],
                        Item.FullOptionTableValue[0], Item.FullOptionTableValue[1], Item.FullOptionTableValue[2],
                        Item.FullOptionTableValue[3], Item.FullOptionTableValue[4],
                        Item.ReqClassTable[0], Item.ReqClassTable[1], Item.ReqClassTable[2],
                        Item.ReqClassTable[3], Item.ReqClassTable[4], Item.ReqClassTable[5],
                        Item.ReqClassTable[6]);
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
            int LineCount = 255;
            object[] Res = new object[LineCount];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetOption));
            MuDef.MUFile_ItemSetOption CurrentItem;

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
                           typeof(MuDef.MUFile_ItemSetOption));
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
           // {
                int FilledRowCount = dgv.Rows.Count;
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_ItemSetOption> TmpList = new List<MuDef.MUFile_ItemSetOption>(dgv.Rows.Count);
                MuDef.MUFile_ItemSetOption CurrentItem = new MuDef.MUFile_ItemSetOption();
                CurrentItem.OptionTable0 = new sbyte[6];
                CurrentItem.OptionTable1 = new sbyte[6];
                CurrentItem.OptionTableValue0 = new sbyte[6];
                CurrentItem.OptionTableValue1 = new sbyte[6];
                CurrentItem.ExOptionTable = new sbyte[2];
                CurrentItem.ExOptionTableValue = new sbyte[2];
                CurrentItem.FullOptionTable = new sbyte[5];
                CurrentItem.FullOptionTableValue = new sbyte[5];
                CurrentItem.ReqClassTable = new sbyte[7];

                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        int CurrentCell = 0;
                        CurrentItem.Name = dgv.Rows[i].Cells[CurrentCell++].Value.ToString();

                        for (int j = 0; j < 6; j++)
                        {
                            CurrentItem.OptionTable0[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 6; j++)
                        {
                            CurrentItem.OptionTable1[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 6; j++)
                        {
                            CurrentItem.OptionTableValue0[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 6; j++)
                        {
                            CurrentItem.OptionTableValue1[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 2; j++)
                        {
                            CurrentItem.ExOptionTable[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 2; j++)
                        {
                            CurrentItem.ExOptionTableValue[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        CurrentItem.FullOptionCount = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());

                        for (int j = 0; j < 5; j++)
                        {
                            CurrentItem.FullOptionTable[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 5; j++)
                        {
                            CurrentItem.FullOptionTableValue[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        for (int j = 0; j < 7; j++)
                        {
                            CurrentItem.ReqClassTable[j] = sbyte.Parse(dgv.Rows[i].Cells[CurrentCell++].Value.ToString());
                        }

                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetOption));
                int TotalSize = FilledRowCount * ItemSize;

                byte[] FileBuffer = new byte[TotalSize];

                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemSetOption)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);
                OutputStream.Write(BitConverter.GetBytes(GetCRC(FileBuffer, FileBuffer.Length)), 0, sizeof(uint));

                OutputStream.Flush();
                OutputStream.Close();
            //}
            //catch (Exception e)
            //{ MessageBox.Show("Failed to save file" + e.Message); }
        }
    }
}
