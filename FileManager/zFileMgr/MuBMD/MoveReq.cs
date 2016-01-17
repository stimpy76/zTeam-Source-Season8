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
    class MoveReq : BmdFile
    {
        const int cTotalStructCount = 1000;

        //nothing to do here, just pass data to generic constructor
        public MoveReq(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText,BmdConversationType.BmdToXml) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "ID", "Text_1", "Text_2", "MinLevel", "MaxLevel", "Money", "Gate" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_MoveReq Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_MoveReq)items[i];
                    dgv.Rows.Add(
                                    Item.ID,
                                    Item.Text1,
                                    Item.Text2,
                                    Item.MinLevel,
                                    Item.MaxLevel,
                                    Item.Money,
                                    Item.Gate

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_MoveReq));
            MuDef.MUFile_MoveReq CurrentItem;

            int LineCount = ReadInt(m_FileStream);
            m_FileBuffer = new byte[SizeOfItem * LineCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_MoveReq));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_MoveReq)Item;

                    Res[m_CurrentLine] = (object)CurrentItem;


                }
            }
            catch (ArgumentException)
            {

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

                List<MuDef.MUFile_MoveReq> TmpList = new List<MuDef.MUFile_MoveReq>(dgv.Rows.Count);
                MuDef.MUFile_MoveReq CurrentItem = new MuDef.MUFile_MoveReq();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.ID = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.Text1 = dgv.Rows[i].Cells[1].Value.ToString();
                        CurrentItem.Text2 = dgv.Rows[i].Cells[2].Value.ToString();
                        CurrentItem.MinLevel = int.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.MaxLevel = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.Money = int.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.Gate = int.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_MoveReq));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_MoveReq)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(BitConverter.GetBytes(FilledRowCount), 0, sizeof(int));
                OutputStream.Write(FileBuffer, 0, TotalSize);


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


                for (int i = 0; i < FilledRowCount; i++)
                {




                    line =
                        string.Format("{0:d}    {1}     {2}     {3}     {4}     {5}     {6}\r\n",
                        dgv.Rows[i].Cells[0].Value.ToString(),
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

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<movelist>\r\n");

                OutputStream.Write(buf, 0, buf.Length);

                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {

                    //   string[] col_names = { "ID", "Text_1", "Text_2", "MinLevel", "MaxLevel", "Money", "Gate" };

                    line =
                        string.Format("\t<move id=\"{0:d}\" money=\"{1:d}\" level=\"{2:d}\" gate=\"{3:d}\">{4}</move>\r\n",
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[5].Value.ToString(), //money
                        dgv.Rows[i].Cells[3].Value.ToString(),
                        dgv.Rows[i].Cells[6].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString());



                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</movelist>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            } catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
