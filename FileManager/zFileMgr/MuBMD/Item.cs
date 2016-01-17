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
    class Item : BmdFile
    {
        const int cTotalStructCount = 512 * 16;
        
        //nothing to do here, just pass data to generic constructor
        public Item(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }      

        public static object[] GetItemsByGroupID(object[] src, int group)
        {
            object[] items = new object[512];

            int nItem = 0;


            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] != null)
                {
                    if (((MuGeneric.MuDef.MUFile_Item)(src[i])).Index == group)
                    {
                        items[nItem++] = src[i];
                    }
                }
            }

            return items;
        }

        public static int GetFirstFreeItemPlace(object[] src, int group)
        {
            
            int nItem = 0;


            for (int i = 1; i < src.Length; i++)
            {
                if (src[i] == null)
                {
                    nItem = i;
                    break;
                }
            }
            return nItem;

        }

        public override object GetStructure()
        {
            return 0;
        }

        public static void SaveTemporalGridData(ref object[] CurrentItems, DataGridView dgv)
        {
            int nFilledRows = dgv.Rows.Count - 1;

            for (int i = 0; i < nFilledRows; i++)
            {
                for (int j = 0; j < CurrentItems.Length; j++)
                {
                    if (CurrentItems[j] != null)
                    {
                        if (((MuDef.MUFile_Item)(CurrentItems[i])).Index == ushort.Parse(dgv.Rows[i].Cells[0].Value.ToString()))
                        {
                            /*
                            (MuDef.MUFile_Item)(CurrentItems[i]) = new MuDef.MUFile_Item()
                            {
                               

                            };
                             * */
                        }
                    }
                }
            }

        }

        public override void BuildTable(DataGridView dgv, object[] items)
        {
          //  dgv = new DataGridView();
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = true;

            dgv.Rows.Clear();
            dgv.Columns.Clear();
            DataGridViewColumn col;


            string[] col_names = { "ItemID", "Index", "Number", "Texture", "Model", "Name", "Type1", "Type2", "Type3",
                                    "TwoHands", "ItemLvl", "ItemSlot", "ItemSkill", "X", "Y", "DmgMin", "DmgMax", "DefRate", "Defence",
                                    "Unknown1", "AtkSpeed", "WalkSpeed", "Durability", "MagicDur", "MagicPwr", "Unknown2", "ReqStr",
                                    "ReqDex", "ReqEne", "ReqVit", "ReqLea", "ReqLvl", "ItemValue", "Zen", "SetOption",
                                    "Class1", "Class2", "Class3", "Class4", "Class5", "Class6", "Class7",
                                    "Resist1", "Resist2", "Resist3", "Resist4", "Resist5", "Resist6", "Resist7",
                                    "Unknown3_1", "Unknown3_2", "Unknown3_3", "Unknown3_4", "Unknown3_5", "Unknown3_6", "Unknown3_7", "Unknown3_8",
                                    "Pad" };

            for (int i = 0; i < col_names.Length; i++)
            {

                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                //itemid, index, number = invisible columns
                if (i < 3)
                {
                   // col.Visible = false;
                }
                dgv.Columns.Add(col);

            }

    
            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_Item Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_Item)items[i];
                    dgv.Rows.Add(
                                  Item.ItemID,
                                  Item.Index,
                                  Item.Number,
                                  Item.TexturePath,
                                  Item.ModelName,
                                  Item.ItemInfo.Name,
                                  Item.ItemInfo.Type1,
                                  Item.ItemInfo.Type2,
                                  Item.ItemInfo.Type3,
                                  Item.ItemInfo.TwoHands,
                                  Item.ItemInfo.ItemLvl,
                                  Item.ItemInfo.ItemSlot,
                                  Item.ItemInfo.ItemSkill,
                                  Item.ItemInfo.X,
                                  Item.ItemInfo.Y,
                                  Item.ItemInfo.DmgMin,
                                  Item.ItemInfo.DmgMax,
                                  Item.ItemInfo.DefRate,
                                  Item.ItemInfo.Defence,
                                  Item.ItemInfo.Unknown1,
                                  Item.ItemInfo.AtkSpeed,
                                  Item.ItemInfo.WalkSpeed,
                                  Item.ItemInfo.Durability,
                                  Item.ItemInfo.MagicDur,
                                  Item.ItemInfo.MagicPwr,
                                  Item.ItemInfo.Unknown2,
                                  Item.ItemInfo.ReqStr,
                                  Item.ItemInfo.ReqDex,
                                  Item.ItemInfo.ReqEne,
                                  Item.ItemInfo.ReqVit,
                                  Item.ItemInfo.ReqLea,
                                  Item.ItemInfo.ReqLvl,
                                  Item.ItemInfo.ItemValue,
                                  Item.ItemInfo.Zen,
                                  Item.ItemInfo.SetOption,

                                  Item.ItemInfo.Classes[0],
                                  Item.ItemInfo.Classes[1],
                                  Item.ItemInfo.Classes[2],
                                  Item.ItemInfo.Classes[3],
                                  Item.ItemInfo.Classes[4],
                                  Item.ItemInfo.Classes[5],
                                  Item.ItemInfo.Classes[6],
                                  
                                  Item.ItemInfo.Resistances[0],
                                  Item.ItemInfo.Resistances[1],
                                  Item.ItemInfo.Resistances[2],
                                  Item.ItemInfo.Resistances[3],
                                  Item.ItemInfo.Resistances[4],
                                  Item.ItemInfo.Resistances[5],
                                  Item.ItemInfo.Resistances[6],

                                  Item.ItemInfo.IsApplyToDrop,
                                  Item.ItemInfo.Unknown3,
                                  Item.ItemInfo.Unknown4,
                                  Item.ItemInfo.Unknown5,
                                  Item.ItemInfo.Unknown6,
                                  Item.ItemInfo.IsExpensive,
                                  Item.ItemInfo.Unknown7,
                                  Item.ItemInfo.StackMax,
                                  Item.ItemInfo.Pad




                        );
                }
            }

            //enumerate rows
            EnumerateLines(dgv);

        }

        public override object[] GetStructures()
        {
            object[] Res = new object[cTotalStructCount];

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Item));
            MuDef.MUFile_Item CurrentItem;

            int LineCount = ReadInt(m_FileStream);
            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_Item));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_Item)Item;
                    Res[m_CurrentLine] = (object)CurrentItem;
                }
            }
            //todo: prevent
            catch (ArgumentException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }
            catch// (Exception e)
            {
                MessageBox.Show("Failed to read file structures.");
            }
            CloseSourceFile();

            return Res;
        }

        public override void SaveAsBmd(string OutputPath, object[] items)
        {
            try
            {
               
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);
                int nSizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Item));
                List<MuDef.MUFile_Item> TmpList = new List<MuDef.MUFile_Item>(512);
                MuDef.MUFile_Item CurrentItem = new MuDef.MUFile_Item();

                object[] cur_group;
                byte[] FileBuffer = new byte[nSizeOfItem * items.Length];

                int nTotalItems = 0;

                for (int i = 0; i < 16; i++)
                {
                    cur_group = GetItemsByGroupID(items, i);
                    //each item in group
                    int nItemCounter = 0;

                    while(nItemCounter < 512)
                    {
                        if (cur_group[i] != null)
                        {
                            try
                            {
                                CurrentItem = ((MuDef.MUFile_Item)(cur_group[nItemCounter]));


                                byte[] buf = StructureToByteArray(cur_group[nItemCounter]);
                                XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_Item)));

                                buf.CopyTo(FileBuffer, nTotalItems * nSizeOfItem);

                                nTotalItems++;
                                nItemCounter++;
                            }
                            catch { break;  }
                        }
                        else break;
                        
                    }
                }

                OutputStream.Write(BitConverter.GetBytes(nTotalItems), 0, 4);
                OutputStream.Write(FileBuffer, 0, nTotalItems * nSizeOfItem);
                byte[] crc_bytes = BitConverter.GetBytes(GetCRC(FileBuffer, nTotalItems * nSizeOfItem));
                OutputStream.Write(crc_bytes, 0, sizeof(int));

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }

        public override void SaveAsBmd(string OutputPath, DataGridView dgv)
        {
           
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
                        string.Format("{0:d}	\"{1}\"\r\n",
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

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<infotooltiptext>\r\n");

                OutputStream.Write(buf, 0, buf.Length);


                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {



                    line =
                        string.Format("\t<line id=\"{0:d}\">{1}</line>\r\n",
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString());

                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</infotooltiptext>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
