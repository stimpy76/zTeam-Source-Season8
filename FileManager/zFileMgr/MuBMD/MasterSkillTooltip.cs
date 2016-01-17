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
    class MasterSkillTooltip : BmdFile
    {
        const int cTotalStructCount = 512;

        //nothing to do here, just pass data to generic constructor
        public MasterSkillTooltip(string _path, FileType _type) : base(_path, _type, BmdConversationType.BmdToText , BmdConversationType.BmdToXml) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "SkillID", "Type", "Info1", "Info2", "Info3", "Info4", "Info5", "Info6" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_MasterSkillTooltip Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_MasterSkillTooltip)items[i];
                    dgv.Rows.Add(
                                    Item.SkillID,
                                    Item.Type,
                                    Item.Info1,
                                    Item.Info2,
                                    Item.Info3,
                                    Item.Info4,
                                    Item.Info5,
                                    Item.Info6

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_MasterSkillTooltip));
            MuDef.MUFile_MasterSkillTooltip CurrentItem;

            
            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                for (int i = 0; i < cTotalStructCount; i++)
                {
                    m_FileStream.Read(m_FileBuffer, SizeOfItem * i, SizeOfItem);

                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * i),
                        typeof(MuDef.MUFile_MasterSkillTooltip));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_MasterSkillTooltip)Item;

                    Res[i] = (object)CurrentItem;
                }

              
                //has CRC
            }
            //todo: prevent
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
                
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                List<MuDef.MUFile_MasterSkillTooltip> TmpList = new List<MuDef.MUFile_MasterSkillTooltip>(cTotalStructCount);
                MuDef.MUFile_MasterSkillTooltip CurrentItem = new MuDef.MUFile_MasterSkillTooltip();

                //last row is null
                for (int i = 0; i < cTotalStructCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                            CurrentItem.SkillID = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                            CurrentItem.Type = ushort.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                            CurrentItem.Info1 = dgv.Rows[i].Cells[2].Value.ToString();
                            CurrentItem.Info2 = dgv.Rows[i].Cells[3].Value.ToString();
                            CurrentItem.Info3 = dgv.Rows[i].Cells[4].Value.ToString();
                            CurrentItem.Info4 = dgv.Rows[i].Cells[5].Value.ToString();
                            CurrentItem.Info5 = dgv.Rows[i].Cells[6].Value.ToString();
                            CurrentItem.Info6 = dgv.Rows[i].Cells[7].Value.ToString();

                            TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_MasterSkillTooltip));
                int TotalSize = ItemSize * cTotalStructCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < cTotalStructCount; i++)
                {
        
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, ItemSize);
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                //write data
                OutputStream.Write(FileBuffer, 0, TotalSize);

                //write crc
                byte[] crc_bytes = BitConverter.GetBytes(GetCRC(FileBuffer, TotalSize));

                OutputStream.Write(crc_bytes, 0, sizeof(int));

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file "); }
        }

        public override void SaveAsText(string OutputPath, DataGridView dgv)
        {
            try
            {
                //base.SaveAsText(OutputPath, dgv);
                int FilledRowCount = dgv.Rows.Count - 1;

                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                string line = string.Empty;
                byte[] buf = new byte[1000];
                int tmp = 0;

                for (int i = 0; i < FilledRowCount; i++)
                {
                    tmp = int.Parse(dgv.Rows[i].Cells[1].Value.ToString());


                    line =
                        string.Format("{0:d}	{1:d}	\"{2}\"	\"{3}\"	\"{4}\"	\"{5}\"	\"{6}\"	\"{7}\"\r\n",
                    dgv.Rows[i].Cells[0].Value.ToString(), //skill id
                    (tmp == 255) ? "-1" : tmp.ToString(),
                    dgv.Rows[i].Cells[1].Value.ToString(),
                    dgv.Rows[i].Cells[2].Value.ToString(),
                    dgv.Rows[i].Cells[3].Value.ToString(),
                    dgv.Rows[i].Cells[4].Value.ToString(),
                    dgv.Rows[i].Cells[5].Value.ToString(),
                    dgv.Rows[i].Cells[6].Value.ToString());


                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }

        public override void SaveAsXml(string OutputPath, DataGridView dgv)
        {
            try
            {
                //base.SaveAsText(OutputPath, dgv);
                int FilledRowCount = dgv.Rows.Count - 1;

                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                string line = string.Empty;
                byte[] buf = new byte[1000];

                //  "SkillID", "Type", "Info1", "Info2", "Info3", "Info4", "Info5", "Info6" };

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<masterskilltooltip>\r\n");

                OutputStream.Write(buf, 0, buf.Length);


                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {



                    line =
                        string.Format("\t<skill id=\"{0:d}\" type=\"{1}\" Info1=\"{2}\" Info2=\"{3}\" Info3=\"{4}\" Info4=\"{5}\" Info5=\"{6}\" Info6=\"{7}\"></skill>\r\n",
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString(),
                        dgv.Rows[i].Cells[2].Value.ToString(),
                        dgv.Rows[i].Cells[3].Value.ToString(),
                        dgv.Rows[i].Cells[4].Value.ToString(),
                        dgv.Rows[i].Cells[5].Value.ToString(),
                        dgv.Rows[i].Cells[6].Value.ToString(),
                        dgv.Rows[i].Cells[7].Value.ToString());




                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</masterskilltooltip>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
