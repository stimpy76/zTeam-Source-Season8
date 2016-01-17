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
    class MonsterSkill : BmdFile
    {
        const int cTotalStructCount = 400;

        //nothing to do here, just pass data to generic constructor
        public MonsterSkill(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "MonsterID", "Skill1", "Skill2", "Skill3", "Skill4", "Skill5", "Skill6", "Skill7", "Skill8", "Skill9", "Skill10", "Unknown1" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_MonsterSkill Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_MonsterSkill)items[i];
                    dgv.Rows.Add(
                            Item.MonsterID,
                            Item.MonsterSkill[0],
                            Item.MonsterSkill[1],
                            Item.MonsterSkill[2],
                            Item.MonsterSkill[3],
                            Item.MonsterSkill[4],
                            Item.MonsterSkill[5],
                            Item.MonsterSkill[6],
                            Item.MonsterSkill[7],
                            Item.MonsterSkill[8],
                            Item.MonsterSkill[9],
                            Item.Unknown1
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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_MonsterSkill));
            MuDef.MUFile_MonsterSkill CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                int LineCount = ReadInt(m_FileStream);


                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_MonsterSkill));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_MonsterSkill)Item;

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

                List<MuDef.MUFile_MonsterSkill> TmpList = new List<MuDef.MUFile_MonsterSkill>(dgv.Rows.Count);
                MuDef.MUFile_MonsterSkill CurrentItem = new MuDef.MUFile_MonsterSkill();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.MonsterID = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.MonsterSkill = new int[10];
                        for (int j = 1; j <= 10; j++)
                        {
                            CurrentItem.MonsterSkill[j - 1] = int.Parse(dgv.Rows[i].Cells[j].Value.ToString());
                        }
                        CurrentItem.Unknown1 = int.Parse(dgv.Rows[i].Cells[11].Value.ToString());
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_MonsterSkill));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_MonsterSkill)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                byte[] row_count_bytes = BitConverter.GetBytes(FilledRowCount);

                OutputStream.Write(row_count_bytes, 0, sizeof(int));
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
