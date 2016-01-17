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
    class Credits : BmdFile
    {
        const int cTotalStructCount = 400;

        //nothing to do here, just pass data to generic constructor
        public Credits(string _path, FileType _type) : 
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "Position", "Text" };

          
            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);
                
            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_Credit Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_Credit)items[i];
                    dgv.Rows.Add(
                      Item.Position, 
                      Item.Text

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Credit));
            MuDef.MUFile_Credit CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {



                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_Credit));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_Credit)Item;

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

                List<MuDef.MUFile_Credit> TmpList = new List<MuDef.MUFile_Credit>(dgv.Rows.Count);
                MuDef.MUFile_Credit CurrentItem = new MuDef.MUFile_Credit();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Position = byte.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.Text = dgv.Rows[i].Cells[1].Value.ToString();
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
