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
    class ServerList : BmdFile
    {
        const int cTotalStructCount = 1000;

        //nothing to do here, just pass data to generic constructor
        public ServerList(string _path, FileType _type) : 
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = {   "ID",
                                     "Name",
                                     "ServerPos",
                                     "1",
                                     "2",
                                     "3",
                                     "4",
                                     "5",
                                     "6",
                                     "7",
                                     "8",
                                     "9",
                                     "10",
                                     "11",
                                     "12",
                                     "13",
                                     "14",
                                     "15",
                                     "16",
                                     "17",
                                     "18",
                                     "19",
                                     "20",
                                     "21",
                                      };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MuFile_ServerListManaged Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MuFile_ServerListManaged)items[i];

                  
                    dgv.Rows.Add(
                                   Item.ServerID,
                                   Item.Name,
                                   Item.Unknown[0],
                                   Item.Unknown[1],
                                   Item.Unknown[2],
                                   Item.Unknown[3],
                                   Item.Unknown[4],
                                   Item.Unknown[5],
                                   Item.Unknown[6],
                                   Item.Unknown[7],
                                   Item.Unknown[8],
                                   Item.Unknown[9],
                                   Item.Unknown[10],
                                   Item.Unknown[11],
                                   Item.Unknown[12],
                                   Item.Unknown[13],
                                   Item.Unknown[14],
                                   Item.Unknown[15],
                                   Item.Unknown[16],
                                   Item.Unknown[17],
                                   Item.Unknown[18],
                                   Item.Unknown[19],
                                   Item.Unknown[20],
                                   Item.Unknown[21]

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
            
            try
            {
                
                int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ServerListItem));
                MuDef.MUFile_ServerListItem CurrentItem;

                m_FileBuffer = new byte[SizeOfItem * 100];

                while (m_FileStream.Read(m_FileBuffer, 0, SizeOfItem) == SizeOfItem)
                {
                    XorFilter(ref m_FileBuffer, SizeOfItem);

                    object Item = Marshal.PtrToStructure(
                      Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, 0),
                      typeof(MuDef.MUFile_ServerListItem));

                    CurrentItem = (MuDef.MUFile_ServerListItem)Item;
                 

                 

                    byte[] text_bytes = new byte[CurrentItem.DescLen];
                    m_FileStream.Read(text_bytes, 0, CurrentItem.DescLen);

                    Res[m_CurrentLine] =
                           new MuDef.MuFile_ServerListManaged
                           {
                               ServerID = CurrentItem.ServerID,
                               Name = CurrentItem.Name,
                               Unknown = new byte[22] 
                               {   
                                   CurrentItem.Unknown1, 
                                   CurrentItem.Unknown2,
                                   CurrentItem.Unknown3, 
                                   CurrentItem.Unknown4,
                                   CurrentItem.Unknown5, 
                                   CurrentItem.Unknown6,
                                   CurrentItem.Unknown7, 
                                   CurrentItem.Unknown8,
                                   CurrentItem.Unknown9, 
                                   CurrentItem.Unknown10,
                                   CurrentItem.Unknown11, 
                                   CurrentItem.Unknown12,
                                   CurrentItem.Unknown13, 
                                   CurrentItem.Unknown14,
                                   CurrentItem.Unknown15, 
                                   CurrentItem.Unknown16,
                                   CurrentItem.Unknown17, 
                                   CurrentItem.Unknown18,
                                   CurrentItem.Unknown19,
                                   CurrentItem.Unknown20,
                                   CurrentItem.Unknown21,
                                   CurrentItem.Unknown22
                               },
                               
                               
                           };
                    m_CurrentLine++;
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

                int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ServerListItem));
                MuDef.MUFile_ServerListItem CurrentItem;

                //should be enough for any description length
                m_FileBuffer = new byte[SizeOfItem + 512];
           

                byte[] DescBuf;
                const int cDescLen = 2;

                for (int i = 0; i < FilledRowCount; i++)
                {
                    CurrentItem = new MuDef.MUFile_ServerListItem()
                    {
                        ServerID = ushort.Parse(dgv.Rows[i].Cells[0].Value.ToString()),
                        Name = dgv.Rows[i].Cells[1].Value.ToString(),
                        Unknown1 = byte.Parse(dgv.Rows[i].Cells[2].Value.ToString()),
                        Unknown2 = byte.Parse(dgv.Rows[i].Cells[3].Value.ToString()),
                        Unknown3 = byte.Parse(dgv.Rows[i].Cells[4].Value.ToString()),
                        Unknown4 = byte.Parse(dgv.Rows[i].Cells[5].Value.ToString()),
                        Unknown5 = byte.Parse(dgv.Rows[i].Cells[6].Value.ToString()),
                        Unknown6 = byte.Parse(dgv.Rows[i].Cells[7].Value.ToString()),
                        Unknown7 = byte.Parse(dgv.Rows[i].Cells[8].Value.ToString()),
                        Unknown8 = byte.Parse(dgv.Rows[i].Cells[9].Value.ToString()),
                        Unknown9 = byte.Parse(dgv.Rows[i].Cells[10].Value.ToString()),
                        Unknown10 = byte.Parse(dgv.Rows[i].Cells[11].Value.ToString()),
                        Unknown11 = byte.Parse(dgv.Rows[i].Cells[12].Value.ToString()),
                        Unknown12 = byte.Parse(dgv.Rows[i].Cells[13].Value.ToString()),
                        Unknown13 = byte.Parse(dgv.Rows[i].Cells[14].Value.ToString()),
                        Unknown14 = byte.Parse(dgv.Rows[i].Cells[15].Value.ToString()),
                        Unknown15 = byte.Parse(dgv.Rows[i].Cells[16].Value.ToString()),
                        Unknown16 = byte.Parse(dgv.Rows[i].Cells[17].Value.ToString()),
                        Unknown17 = byte.Parse(dgv.Rows[i].Cells[18].Value.ToString()),
                        Unknown18 = byte.Parse(dgv.Rows[i].Cells[19].Value.ToString()),
                        Unknown19 = byte.Parse(dgv.Rows[i].Cells[20].Value.ToString()),
                        Unknown20 = byte.Parse(dgv.Rows[i].Cells[21].Value.ToString()),
                        Unknown21 = byte.Parse(dgv.Rows[i].Cells[22].Value.ToString()),
                        Unknown22 = byte.Parse(dgv.Rows[i].Cells[23].Value.ToString()),
                        DescLen = (ushort)cDescLen

                    };


                    DescBuf = new byte[cDescLen] { 0x20, 0x00 };
               
                    XorFilter(ref DescBuf, DescBuf.Length);
                  
                    byte[] struct_bytes = StructureToByteArray(CurrentItem);

                    
                    XorFilter(ref struct_bytes, struct_bytes.Length);
                    
                    OutputStream.Write(struct_bytes, 0, struct_bytes.Length);

                    OutputStream.Write(DescBuf, 0, DescBuf.Length);
                   
                }

                


                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }


    }
}
