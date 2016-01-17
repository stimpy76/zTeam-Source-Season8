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
    class Gate : BmdFile
    {
        const int cTotalStructCount = 1000;

        //nothing to do here, just pass data to generic constructor
        public Gate(string _path, FileType _type) : 
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "Type", "MapNumber", "X1", "Y1", "X2", "Y2", "Target", "Dir", "Level", "MaxLevel" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_Gate Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_Gate)items[i];
                    dgv.Rows.Add(
                                    Item.Type,
                                    Item.MapNumber,
                                    Item.X1,
                                    Item.Y1,
                                    Item.X2,
                                    Item.Y2,
                                    Item.Target,
                                    Item.Dir,
                                    Item.Level,
                                    Item.MaxLevel

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Gate));
            MuDef.MUFile_Gate CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_Gate));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_Gate)Item;

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

                List<MuDef.MUFile_Gate> TmpList = new List<MuDef.MUFile_Gate>(dgv.Rows.Count);
                MuDef.MUFile_Gate CurrentItem = new MuDef.MUFile_Gate();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Type = byte.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.MapNumber = byte.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.X1 = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.Y1 = byte.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.X2 = byte.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.Y2 = byte.Parse(dgv.Rows[i].Cells[5].Value.ToString());

                        CurrentItem.Target = ushort.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.Dir = ushort.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        CurrentItem.Level = ushort.Parse(dgv.Rows[i].Cells[8].Value.ToString());
                        CurrentItem.MaxLevel = ushort.Parse(dgv.Rows[i].Cells[9].Value.ToString());
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_Credit));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_Credit)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file."); }
        }


        bool AllCellsAreZero(int nRow, DataGridView dgv)
        {
            int n = 0;
            for (int i = 0; i < dgv.Columns.Count - 1; i++)
            {
                if (dgv.Rows[nRow].Cells[i].Value.ToString() == "0")
                    n++;
                    
            }
            if (n == dgv.Columns.Count - 1) return true;
            return false;

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

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<movegate>\r\n");

                OutputStream.Write(buf, 0, buf.Length);

                int n = 1;
                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {


                    if (AllCellsAreZero(i, dgv))
                        continue;

                    line =
                        string.Format("\t<gate id=\"{0:d}\" flag=\"{1:d}\" map=\"{2:d}\" x1=\"{3:d}\" y1=\"{4:d}\" x2=\"{5:d}\" y2=\"{6:d}\" target=\"{7:d}\" dir=\"{8:d}\" minlvl=\"{9:d}\" maxlvl=\"{10:d}\" />\r\n",
                        n++,
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString(),
                        dgv.Rows[i].Cells[2].Value.ToString(),
                        dgv.Rows[i].Cells[3].Value.ToString(),
                        dgv.Rows[i].Cells[4].Value.ToString(),
                        dgv.Rows[i].Cells[5].Value.ToString(),
                        dgv.Rows[i].Cells[6].Value.ToString(),
                        dgv.Rows[i].Cells[7].Value.ToString(),
                        dgv.Rows[i].Cells[8].Value.ToString(),
                        dgv.Rows[i].Cells[9].Value.ToString());


                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</movegate>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
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



                int n = 1;
                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {


                    if (AllCellsAreZero(i, dgv))
                        continue;

                    line =
                        string.Format("{0:d}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\r\n",
                        n++,
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString(),
                        dgv.Rows[i].Cells[2].Value.ToString(),
                        dgv.Rows[i].Cells[3].Value.ToString(),
                        dgv.Rows[i].Cells[4].Value.ToString(),
                        dgv.Rows[i].Cells[5].Value.ToString(),
                        dgv.Rows[i].Cells[6].Value.ToString(),
                        dgv.Rows[i].Cells[7].Value.ToString(),
                        dgv.Rows[i].Cells[8].Value.ToString(),
                        dgv.Rows[i].Cells[9].Value.ToString());


                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
