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
    class QuestProgress : BmdFile
    {
        const int cTotalStructCount = 3000;

        //nothing to do here, just pass data to generic constructor
        public QuestProgress(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = {   "ID1", "ID2", "ID3",
                                     "Param1", "Param2", "Param3", "Param4",
                                     "Param5", "Param6", "Param7", "Param8",
                                     "Param9", "Param10", "Param11", "Param12",
                                     "Param13", "Param14", "Param15", "Param16",
                                     "Param17", "Param18" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_QuestProgress Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_QuestProgress)items[i];

                    dgv.Rows.Add(
                            Item.ID1,
                            Item.ID2,
                            Item.ID3,

                            Item.ID4[0],
                            Item.ID4[1],
                            Item.ID4[2],
                            Item.ID4[3],
                            Item.ID4[4],
                            Item.ID4[5],
                            Item.ID4[6],
                            Item.ID4[7],
                            Item.ID4[8],
                            Item.ID4[9],
                            Item.ID4[10],
                            Item.ID4[11],
                            Item.ID4[12],
                            Item.ID4[13],
                            Item.ID4[14],
                            Item.ID4[15],
                            Item.ID4[16],
                            Item.ID4[17]
         

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_QuestProgress));
            MuDef.MUFile_QuestProgress CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {



                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_QuestProgress));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_QuestProgress)Item;

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

                List<MuDef.MUFile_QuestProgress> TmpList = new List<MuDef.MUFile_QuestProgress>(dgv.Rows.Count);
                MuDef.MUFile_QuestProgress CurrentItem = new MuDef.MUFile_QuestProgress();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.ID1 = ushort.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.ID2 = ushort.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.ID3 = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.ID4 = new ushort[18];

                        for (int j = 0; j < 18; j++)
                        {
                            CurrentItem.ID4[j] = ushort.Parse(dgv.Rows[i].Cells[j + 2].Value.ToString());
                        }
                        
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_QuestProgress));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_QuestProgress)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
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
                for (int i = 0; i < FilledRowCount; i++)
                {
                    line =
                        string.Format("{0:d}    {1}\r\n",
                    dgv.Rows[i].Cells[0].Value.ToString(),
                    dgv.Rows[i].Cells[1].Value.ToString());

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

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<credits>\r\n");

                OutputStream.Write(buf, 0, buf.Length);


                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {



                    line =
                        string.Format("\t<credit Position=\"{0:d}\">{1}</credit>\r\n",

                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString());



                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</credits>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }

}
