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
    class BuffEffect : BmdFile
    {
          const int cTotalStructCount = 1000;

        //nothing to do here, just pass data to generic constructor
        public BuffEffect(string _path, FileType _type) : base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }

       

        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "Index", "Group", "ItemIndex", "ItemNumber", "Name", "State1", "State2", "State3", "Description" };

            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                col.Resizable = DataGridViewTriState.True;

                dgv.Columns.Add(col);
            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_BuffEffect Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_BuffEffect)items[i];
                    dgv.Rows.Add(
                        Item.Index,
                        Item.Group,
                        Item.ItemIndex,
                        Item.ItemNumber,
                        Item.Name,
                        Item.State1,
                        Item.State2,
                        Item.State3,
                        Item.Description
                            
                        );
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
            object[] Res = new object[cTotalStructCount];
            
            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_BuffEffect));
            MuDef.MUFile_BuffEffect CurrentItem = new MuDef.MUFile_BuffEffect();

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
              //6  ReadInt(m_FileStream);
                ReadInt(m_FileStream);

                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_BuffEffect));
                    XorFilter(ref Item);
                    
                    CurrentItem = (MuDef.MUFile_BuffEffect)Item;
                    
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
                List<MuDef.MUFile_BuffEffect> TmpList = new List<MuDef.MUFile_BuffEffect>(dgv.Rows.Count);
                MuDef.MUFile_BuffEffect CurrentItem = new MuDef.MUFile_BuffEffect();

                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Index = ushort.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.Group = byte.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.ItemIndex = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.ItemNumber = byte.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.Name = dgv.Rows[i].Cells[4].Value.ToString();
                        CurrentItem.State1 = byte.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.State2 = byte.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.State3 = byte.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        CurrentItem.Description = dgv.Rows[i].Cells[8].Value.ToString();

                        TmpList.Add(CurrentItem);
                    }

                }
                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_BuffEffect));

                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_BuffEffect)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(BitConverter.GetBytes(FilledRowCount), 0, 4);
                OutputStream.Write(FileBuffer, 0, TotalSize);

                byte[] crc_bytes = BitConverter.GetBytes(GetCRC(FileBuffer, TotalSize));
                OutputStream.Write(crc_bytes, 0, sizeof(int));

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file."); }
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
                        string.Format("{0:d}	{1:d}	{2:d}	{3:d}	\"{4}\"	{5:d}	{6:d}	{7:d}	\"{8}\"\r\n",
                    dgv.Rows[i].Cells[0].Value.ToString(),
                    dgv.Rows[i].Cells[1].Value.ToString(),
                    dgv.Rows[i].Cells[2].Value.ToString(),
                    dgv.Rows[i].Cells[3].Value.ToString(),
                    dgv.Rows[i].Cells[4].Value.ToString(),
                    dgv.Rows[i].Cells[5].Value.ToString(),
                    dgv.Rows[i].Cells[6].Value.ToString(),
                    dgv.Rows[i].Cells[7].Value.ToString(),
                    dgv.Rows[i].Cells[8].Value.ToString());

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

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<buffeffect>\r\n");

                OutputStream.Write(buf, 0, buf.Length);

               
                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {



                    line =
                        string.Format("\t<effect index=\"{0:d}\" group=\"{1}\" ItemIndex=\"{2}\" ItemNumber=\"{3}\" Name=\"{4}\" State1=\"{5}\" State2=\"{6}\" State3=\"{7}\" Description=\"{8}\"></effect>\r\n",
                       
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString(),
                        dgv.Rows[i].Cells[2].Value.ToString(),
                        dgv.Rows[i].Cells[3].Value.ToString(),
                        dgv.Rows[i].Cells[4].Value.ToString(),
                        dgv.Rows[i].Cells[5].Value.ToString(),
                        dgv.Rows[i].Cells[6].Value.ToString(),
                        dgv.Rows[i].Cells[7].Value.ToString(),
                        dgv.Rows[i].Cells[8].Value.ToString());


                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</buffeffect>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
