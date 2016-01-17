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
    class Minimap : BmdFile
    {
        const int cTotalStructCount = 1000;

        //nothing to do here, just pass data to generic constructor
        public Minimap(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }


        public List<int> GetAllMapIndexes(DataGridView dgv)
        {
            List<int> res = new List<int>();

            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                if (!res.Contains(int.Parse(dgv.Rows[i].Cells[0].Value.ToString())))
                {
                    res.Add(int.Parse(dgv.Rows[i].Cells[0].Value.ToString()));
                }
            }

            return res;
        }

        public List<object> GetParamsByMapIndex(object[] all_items, int map_index)
        {
            List<object> res = new List<object>();
            for (int i = 0; i < all_items.Length; i++)
            {
                if (((MuDef.MUFile_Minimap_NoMarshal)(all_items[i])).MapIndex == map_index)
                {
                    res.Add(all_items[i]);
                }
            }
            return res;
        }

        public List<MuDef.MUFile_Minimap> GetStructuresByMapIndex(DataGridView dgv, int map_index)
        {
            List<MuDef.MUFile_Minimap> res = new List<MuDef.MUFile_Minimap>();

            MuDef.MUFile_Minimap temp = new MuDef.MUFile_Minimap();

            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                if (int.Parse(dgv.Rows[i].Cells[0].Value.ToString()) == map_index)
                {
                    temp.Params = new int[5];
                    temp.Params[0] = int.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                    temp.Params[1] = int.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                    temp.Params[2] = int.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                    temp.Params[3] = int.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                    temp.Params[4] = int.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                    temp.Name = dgv.Rows[i].Cells[6].Value.ToString();
                    res.Add(temp);

                }
            }
            return res;
        }

        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "MapIndex", "Type", "X", "Y", "RotationAngle", "Param5", "Name" };

            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);
            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_Minimap_NoMarshal Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_Minimap_NoMarshal)items[i];
                    dgv.Rows.Add(
                                    Item.MapIndex,
                                    Item.MinimapStruct.Params[0], 
                                    Item.MinimapStruct.Params[1],
                                    Item.MinimapStruct.Params[2],
                                    Item.MinimapStruct.Params[3],
                                    Item.MinimapStruct.Params[4],
                                    Item.MinimapStruct.Name
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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Minimap));
            MuDef.MUFile_Minimap CurrentItem;


            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];
            object nLines = ReadInt(m_FileStream);
            XorFilter(ref nLines);

            m_FileStream.Read(m_FileBuffer, 0, (int)m_FileStream.Length);

            //XorFilter(m_FileBuffer, m_FileBuffer.Length);

            MemoryStream ms = new MemoryStream(m_FileBuffer);

            byte[] item_buf = new byte[SizeOfItem];

            

            for (int i = 0; i < (int)nLines; i++)
            {
                byte[] buf = new byte[4];

                ms.Read(buf, 0, 4);
                XorFilter(ref buf, 4);
                int mapindex = BitConverter.ToInt32(buf, 0);
                Console.WriteLine("map: {0}", mapindex);

                //count


               byte[] count_buf = new byte[1];
               count_buf[0] = (byte)ms.ReadByte();
               XorFilter(ref count_buf, 1);
               byte count = count_buf[0];

                Console.WriteLine("count: {0}", count);

                for (int a = 0; a < (int)count; a++)
                {
                    ms.Read(item_buf, 0, item_buf.Length);
                    XorFilter(ref item_buf, item_buf.Length);

                    object item = Marshal.PtrToStructure(
                       Marshal.UnsafeAddrOfPinnedArrayElement(item_buf, 0),
                       typeof(MuDef.MUFile_Minimap));

                    CurrentItem = (MuDef.MUFile_Minimap)item;
                    MuDef.MUFile_Minimap_NoMarshal no_marshal = new MuDef.MUFile_Minimap_NoMarshal()
                    {
                        MapIndex = mapindex,
                        MinimapStruct = CurrentItem
                    };

                    if (m_CurrentLine++ > 500) break;
                    Res[m_CurrentLine] = (object)no_marshal;
                }

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
                
                List<MuDef.MUFile_Minimap_NoMarshal> TmpList = new List<MuDef.MUFile_Minimap_NoMarshal>(dgv.Rows.Count);
                List<int> all_map_indexes = GetAllMapIndexes(dgv);

                //map count
                byte[] int_buf = BitConverter.GetBytes(all_map_indexes.Count);
                XorFilter(ref int_buf, sizeof(int));

                OutputStream.Write(int_buf, 0, sizeof(int));

                for (int i = 0; i < all_map_indexes.Count; i++)
                {
                    int mapindex = all_map_indexes[i];
                    //write map index
                    int_buf = BitConverter.GetBytes(mapindex);
                    XorFilter(ref int_buf, sizeof(int));
                    OutputStream.Write(int_buf, 0, sizeof(int));


                    List<MuDef.MUFile_Minimap> map_structs = GetStructuresByMapIndex(dgv, mapindex);

                    byte count = (byte)map_structs.Count;

                    byte[] count_bytes = new byte [1] { 
                        (byte)count
                    };

                    XorFilter(ref count_bytes, 1);

                    OutputStream.Write(count_bytes, 0, 1);
                    for (int a = 0; a < map_structs.Count; a++)
                    {
                        byte[] item_bytes = StructureToByteArray(map_structs[a]);
                        XorFilter(ref item_bytes, item_bytes.Length);
                        OutputStream.Write(item_bytes, 0, item_bytes.Length);
                    }
                   // byte count = 
                    //
                }


                 
                OutputStream.Flush();
                OutputStream.Close();

                byte[] crc_bytes = new byte[4];
                byte[] buf = File.ReadAllBytes(OutputPath);

                uint crc = GetCRC(buf, buf.Length);
                crc_bytes = BitConverter.GetBytes(crc);

                OutputStream = new FileStream(OutputPath, FileMode.Append, FileAccess.Write);
                OutputStream.Write(crc_bytes, 0, sizeof(uint));
                OutputStream.Flush();
                OutputStream.Close();
               
            }
            catch { MessageBox.Show("Failed to save file "); }
        }
    }
}
