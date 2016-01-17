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
    class Text : BmdFile
    {
        //nothing to do here, just pass data to generic constructor
        public Text(string _path, FileType _type) : base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }


        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col = new DataGridViewTextBoxColumn();

            string[] col_names = { "Line nr", "Text" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            
            MuDef.MUFile_TextManaged Item;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_TextManaged)items[i];

                    if (items[i] != null)
                    {
                        dgv.Rows.Add(
                                        
                                        Item.LineNr,
                                        Item.Text
                                    );
                    }
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
            object[] Res = new object[0];

            int SizeOfTextHeader = Marshal.SizeOf(typeof(MuDef.MUFile_TextItemHead));
            int SizeOfTextBody = Marshal.SizeOf(typeof(MuDef.MUFile_Text));

            m_FileBuffer = new byte[SizeOfTextBody];

            try
            {
                //read BMD head
                m_FileStream.Read(m_FileBuffer, 0, Marshal.SizeOf(typeof(MuDef.TBmdHead)));

                object objBmdHeader = Marshal.PtrToStructure(
                    Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, 0), typeof(MuDef.TBmdHead));

                MuDef.TBmdHead BmdHead = (MuDef.TBmdHead)objBmdHeader;

                //initialize Res
                Res = new object[BmdHead.LineCount];

                MuDef.MUFile_TextItemHead TextHead;
                MuDef.MUFile_TextManaged TextObj;

                for (int i = 0; i < BmdHead.LineCount; i++)
                {
                    m_FileStream.Read(m_FileBuffer, 0, SizeOfTextHeader);
                    object objItemHeader = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, 0), typeof(MuDef.MUFile_TextItemHead));


                    TextHead = (MuDef.MUFile_TextItemHead)objItemHeader;

                    m_FileStream.Read(m_FileBuffer, 0, TextHead.LineLen);
                    XorFilter(ref m_FileBuffer, TextHead.LineLen);

                    string Tmp = ASCIIEncoding.ASCII.GetString(m_FileBuffer, 0, TextHead.LineLen);

                    TextObj = new MuDef.MUFile_TextManaged()
                    {
                        LineNr = TextHead.LineNr,
                        Text = Tmp
                    };
                    Res[i] = TextObj;

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
                int FilledRowCount = dgv.Rows.Count - 1;

                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);

                MuDef.TBmdHead BmdHeader = new MuDef.TBmdHead()
                {
                    Version = MuDef.cBmdVersion,
                    LineCount = FilledRowCount
                };

                MuDef.MUFile_TextItemHead TextItemHeader;


                //write bmd header
                int nBmdHeaderSize = Marshal.SizeOf(typeof(MuDef.TBmdHead));
                int nTextHeaderSize = Marshal.SizeOf(typeof(MuDef.MUFile_TextItemHead));

                OutputStream.Write(StructureToByteArray(BmdHeader), 0, nBmdHeaderSize);


                //write text header
                for (int i = 0; i < FilledRowCount; i++)
                {
         
                    TextItemHeader = new MuDef.MUFile_TextItemHead()
                    {
                        LineNr = int.Parse(dgv.Rows[i].Cells[0].Value.ToString()),
                        LineLen = dgv.Rows[i].Cells[1].Value.ToString().Length
                    };


                    OutputStream.Write(StructureToByteArray(TextItemHeader), 0, nTextHeaderSize);

                    if (TextItemHeader.LineLen > 0)
                    {
                        byte[] text_bytes = ASCIIEncoding.ASCII.GetBytes(dgv.Rows[i].Cells[1].Value.ToString());


                        XorFilter(ref text_bytes, text_bytes.Length);

                        OutputStream.Write(text_bytes, 0, TextItemHeader.LineLen);
                    }

                }

                OutputStream.Flush();
                OutputStream.Close();

            }
            catch
            { MessageBox.Show("Failed to save file."); }
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
                        string.Format("{0:d} \"{1}\"\r\n",
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

                //   string[] col_names = { "Index", "Group", "ItemIndex", "ItemNumber", "Name", "State1", "State2", "State3", "Description" };

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<text>\r\n");

                OutputStream.Write(buf, 0, buf.Length);


                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {



                    line =
                        string.Format("\t<line nr=\"{0:d}\">{1}</line>\r\n",
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString());



                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</text>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
