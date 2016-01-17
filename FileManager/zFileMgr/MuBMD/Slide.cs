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
    class Slide : BmdFile
    {
        
        const int cTotalStructCount = 400;

        //nothing to do here, just pass data to generic constructor
        public Slide(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;

            DataGridViewColumn col;
            string[] col_names = { "Delay", "Slide count" };

            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            //add slide 1 - 32
            for (int i = 0; i < 32; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = (i + 1).ToString();
                dgv.Columns.Add(col);
            }

        
            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_Slide Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_Slide)items[i];
                    dgv.Rows.Add(

                            Item.Delay,
                            Item.SlideCount,
                            Item.Slide1,
                            Item.Slide2,
                            Item.Slide3,
                            Item.Slide4,
                            Item.Slide5,
                            Item.Slide6,
                            Item.Slide7,
                            Item.Slide8,
                            Item.Slide9,
                            Item.Slide10,
                            Item.Slide11,
                            Item.Slide12,
                            Item.Slide13,
                            Item.Slide14,
                            Item.Slide15,
                            Item.Slide16,
                            Item.Slide17,
                            Item.Slide18,
                            Item.Slide19,
                            Item.Slide20,
                            Item.Slide21,
                            Item.Slide22,
                            Item.Slide23,
                            Item.Slide24,
                            Item.Slide25,
                            Item.Slide26,
                            Item.Slide27,
                            Item.Slide28,
                            Item.Slide29,
                            Item.Slide30,
                            Item.Slide31,
                            Item.Slide32

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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Slide));
            MuDef.MUFile_Slide CurrentItem;

            try
            {


                m_FileBuffer = new byte[(int)m_FileStream.Length];

                m_FileStream.Read(m_FileBuffer, 0, (int)m_FileStream.Length);

                XorFilter(ref m_FileBuffer, m_FileBuffer.Length);
                
                //---
                ReadInt(m_FileStream);
                ReadInt(m_FileStream);
                //---

                int BlockCount = (m_FileBuffer.Length - 8) / SizeOfItem;

                byte[] tmp = new byte[BlockCount * SizeOfItem];

                Buffer.BlockCopy(m_FileBuffer, 8, tmp, 0, tmp.Length);
               
               

                MemoryStream ms = new MemoryStream(m_FileBuffer);
                ms.Seek(8, SeekOrigin.Begin);

                while (ms.Read(tmp, 0, SizeOfItem) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                           Marshal.UnsafeAddrOfPinnedArrayElement(tmp, SizeOfItem * m_CurrentLine++),
                           typeof(MuDef.MUFile_Slide));
                    CurrentItem = (MuDef.MUFile_Slide)Item;
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

                List<MuDef.MUFile_Slide> TmpList = new List<MuDef.MUFile_Slide>(dgv.Rows.Count);
                MuDef.MUFile_Slide CurrentItem = new MuDef.MUFile_Slide();



                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.Delay = int.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.SlideCount = int.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.Slide1 = dgv.Rows[i].Cells[2].Value.ToString();
                        CurrentItem.Slide2 = dgv.Rows[i].Cells[3].Value.ToString();
                        CurrentItem.Slide3 = dgv.Rows[i].Cells[4].Value.ToString();
                        CurrentItem.Slide4 = dgv.Rows[i].Cells[5].Value.ToString();
                        CurrentItem.Slide5 = dgv.Rows[i].Cells[6].Value.ToString();
                        CurrentItem.Slide6 = dgv.Rows[i].Cells[7].Value.ToString();
                        CurrentItem.Slide7 = dgv.Rows[i].Cells[8].Value.ToString();
                        CurrentItem.Slide8 = dgv.Rows[i].Cells[9].Value.ToString();
                        CurrentItem.Slide9 = dgv.Rows[i].Cells[10].Value.ToString();
                        CurrentItem.Slide10 = dgv.Rows[i].Cells[11].Value.ToString();
                        CurrentItem.Slide11 = dgv.Rows[i].Cells[12].Value.ToString();
                        CurrentItem.Slide12 = dgv.Rows[i].Cells[13].Value.ToString();
                        CurrentItem.Slide13 = dgv.Rows[i].Cells[14].Value.ToString();
                        CurrentItem.Slide14 = dgv.Rows[i].Cells[15].Value.ToString();
                        CurrentItem.Slide15 = dgv.Rows[i].Cells[16].Value.ToString();
                        CurrentItem.Slide16 = dgv.Rows[i].Cells[17].Value.ToString();
                        CurrentItem.Slide17 = dgv.Rows[i].Cells[18].Value.ToString();
                        CurrentItem.Slide18 = dgv.Rows[i].Cells[19].Value.ToString();
                        CurrentItem.Slide19 = dgv.Rows[i].Cells[20].Value.ToString();
                        CurrentItem.Slide20 = dgv.Rows[i].Cells[21].Value.ToString();
                        CurrentItem.Slide21 = dgv.Rows[i].Cells[22].Value.ToString();
                        CurrentItem.Slide22 = dgv.Rows[i].Cells[23].Value.ToString();
                        CurrentItem.Slide23 = dgv.Rows[i].Cells[24].Value.ToString();
                        CurrentItem.Slide24 = dgv.Rows[i].Cells[25].Value.ToString();
                        CurrentItem.Slide25 = dgv.Rows[i].Cells[26].Value.ToString();
                        CurrentItem.Slide26 = dgv.Rows[i].Cells[27].Value.ToString();
                        CurrentItem.Slide27 = dgv.Rows[i].Cells[28].Value.ToString();
                        CurrentItem.Slide28 = dgv.Rows[i].Cells[29].Value.ToString();
                        CurrentItem.Slide29 = dgv.Rows[i].Cells[30].Value.ToString();
                        CurrentItem.Slide30 = dgv.Rows[i].Cells[31].Value.ToString();
                        CurrentItem.Slide31 = dgv.Rows[i].Cells[32].Value.ToString();
                        CurrentItem.Slide32 = dgv.Rows[i].Cells[33].Value.ToString();
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_Slide));
                //8 bytes of header allocated also
                int TotalSize = (ItemSize  * FilledRowCount) + 8 ;

                byte[] FileBuffer = new byte[TotalSize];

     
                MemoryStream ms = new MemoryStream(FileBuffer, true);

                ms.Seek(8, SeekOrigin.Begin);
                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    ms.Write(buf, 0, ItemSize);

                }

                //xor whole buffer
                ms.Flush();

                byte[] head = new byte[8] { 0xC0, 0xCF, 0xAB, 0xFC, 0xCF, 0xAB, 0xDC, 0x8F };

                XorFilter(ref FileBuffer, FileBuffer.Length);

                //head not xor'ed
                Buffer.BlockCopy(head, 0, FileBuffer, 0, 8);
         
                OutputStream.Write(FileBuffer, 0, FileBuffer.Length);

                ms.Close();

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }

}
