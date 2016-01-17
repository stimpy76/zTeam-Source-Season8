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
    class QuestWords : BmdFile
    {
        const int cTotalStructCount = 3000;

        //nothing to do here, just pass data to generic constructor
        public QuestWords(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "ID", "Word" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_QuestWord_NoMarshal Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_QuestWord_NoMarshal)items[i];
                    dgv.Rows.Add(
                      Item.ID,
                      Item.Word

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_QuestWordHead));
            MuDef.MUFile_QuestWordHead CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];
            byte[] word_buffer = new byte[1024];
            MuDef.MUFile_QuestWord_NoMarshal NoMarshalItem = new MuDef.MUFile_QuestWord_NoMarshal();
            try
            {
               
              //  m_CurrentLine = 1;
                
                while((m_FileStream.Read(m_FileBuffer, 0, SizeOfItem) == SizeOfItem))
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, 0),
                        typeof(MuDef.MUFile_QuestWordHead));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_QuestWordHead)Item;
                    //read word

                    m_FileStream.Read(word_buffer,  0, CurrentItem.Length);

                    XorFilter(ref word_buffer, CurrentItem.Length);


                    NoMarshalItem.ID = CurrentItem.ID;
                    //skip null terminator
                    NoMarshalItem.Word = ASCIIEncoding.ASCII.GetString(word_buffer, 0, CurrentItem.Length - 1);
                    Res[m_CurrentLine++] = (object)NoMarshalItem;

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


                MuDef.MUFile_QuestWordHead TmpHeader = new MuDef.MUFile_QuestWordHead();
                MuDef.MUFile_QuestWord_NoMarshal TmpNoMarshal = new MuDef.MUFile_QuestWord_NoMarshal();

                for (int i = 0; i < FilledRowCount; i++)
                {
                    TmpNoMarshal.ID = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                    TmpNoMarshal.Word = dgv.Rows[i].Cells[1].Value.ToString() + "\0";

                    //make unnative native
                    TmpHeader.ID = TmpNoMarshal.ID;
                    TmpHeader.Length = (ushort)TmpNoMarshal.Word.Length;


                    //header
                    byte[] header_bytes = StructureToByteArray(TmpHeader);
                    XorFilter(ref header_bytes, header_bytes.Length);
                    byte[] text = ASCIIEncoding.ASCII.GetBytes(TmpNoMarshal.Word);
                    XorFilter(ref text, text.Length);

                    OutputStream.Write(header_bytes, 0, header_bytes.Length);
                    OutputStream.Write(text, 0, text.Length);

                }


               // OutputStream.Write(FileBuffer, 0, TotalSize);

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }

}
