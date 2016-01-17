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
    class Skill : BmdFile
    {
        const int cTotalStructCount = 700;

        //nothing to do here, just pass data to generic constructor
        public Skill(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names =
            {
                "Name",
                "Level",
                "Damage",
                "Mana",
                "BP",
                "Dis",
                "Delay",
                "Energy",
                "Leadership",
                "Attr",
                "UseType",
                "Brand",
                "KillCount",
                "KillStatus1",
                "KillStatus2",
                "KillStatus3",
                "Class1",
                "Class2",
                "Class3",
                "Class4",
                "Class5",
                "Class6",
                "Class7",
                "SkillRank",
                "SkillGroup",
                "Unknown3",
                "Unknown4",
                "ReqStr",
                "ReqDex",
                "ItemSkill",
                "IsDamage",
                "SkillIcon",
                "Unknown5",
                "Unknown6",
                "Unknown7",
                "Unknown8",
                "Unknown9",
                "Unknown10",
                "Unknown11",
                "Unknown12",

            };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_Skill Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_Skill)items[i];
                    dgv.Rows.Add(
                                    Item.Name,
                                    Item.Level,
                                    Item.Damage,
                                    Item.Mana,
                                    Item.BP,
                                    //
                                    Item.Dis,
                                    Item.Delay,
                                    Item.Energy,
                                    Item.Leadership,
                                    Item.Attr,
                                    Item.UseType,
                                    Item.Brand,
                                    Item.KillCount,
                                    Item.KillStatus1,
                                    Item.KillStatus2,
                                    Item.KillStatus3,
                                    Item.Class1,
                                    Item.Class2,
                                    Item.Class3,
                                    Item.Class4,
                                    Item.Class5,
                                    Item.Class6,
                                    Item.Class7,
                                    Item.SkillRank,
                                    Item.SkillGroup,
                                    Item.Unknown3,
                                    Item.Unknown4,
                                    Item.ReqStr,
                                    Item.ReqDex,
                                    Item.ItemSkill,
                                    Item.IsDamage,
                                    Item.SkillIcon,
                                    Item.Unknown5,
                                    Item.Unknown6,
                                    Item.Unknown7,
                                    Item.Unknown8,
                                    Item.Unknown9,
                                    Item.Unknown10,
                                    Item.Unknown11,
                                    Item.Unknown12


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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_Skill));
            MuDef.MUFile_Skill CurrentItem;


            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_Skill));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_Skill)Item;

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

                List<MuDef.MUFile_Skill> TmpList = new List<MuDef.MUFile_Skill>(dgv.Rows.Count);
                MuDef.MUFile_Skill CurrentItem = new MuDef.MUFile_Skill();




                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        //---
                        CurrentItem.Name = dgv.Rows[i].Cells[0].Value.ToString();
                        CurrentItem.Level = ushort.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.Damage = ushort.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.Mana = ushort.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.BP = ushort.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.Dis = int.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.Delay = int.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.Energy = int.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        CurrentItem.Leadership = ushort.Parse(dgv.Rows[i].Cells[8].Value.ToString());
                        CurrentItem.Attr = byte.Parse(dgv.Rows[i].Cells[9].Value.ToString());
                        CurrentItem.UseType = byte.Parse(dgv.Rows[i].Cells[10].Value.ToString());
                        CurrentItem.Brand = int.Parse(dgv.Rows[i].Cells[11].Value.ToString());

                        CurrentItem.KillCount = byte.Parse(dgv.Rows[i].Cells[12].Value.ToString());
                        CurrentItem.KillStatus1 = byte.Parse(dgv.Rows[i].Cells[13].Value.ToString());
                        CurrentItem.KillStatus2 = byte.Parse(dgv.Rows[i].Cells[14].Value.ToString());
                        CurrentItem.KillStatus3 = byte.Parse(dgv.Rows[i].Cells[15].Value.ToString());

                        //---
                        CurrentItem.Class1 = byte.Parse(dgv.Rows[i].Cells[16].Value.ToString());
                        CurrentItem.Class2 = byte.Parse(dgv.Rows[i].Cells[17].Value.ToString());
                        CurrentItem.Class3 = byte.Parse(dgv.Rows[i].Cells[18].Value.ToString());
                        CurrentItem.Class4 = byte.Parse(dgv.Rows[i].Cells[19].Value.ToString());
                        CurrentItem.Class5 = byte.Parse(dgv.Rows[i].Cells[20].Value.ToString());
                        CurrentItem.Class6 = byte.Parse(dgv.Rows[i].Cells[21].Value.ToString());
                        CurrentItem.Class7 = byte.Parse(dgv.Rows[i].Cells[22].Value.ToString());
                        //---
                        CurrentItem.SkillRank = byte.Parse(dgv.Rows[i].Cells[23].Value.ToString());
                        CurrentItem.SkillGroup = ushort.Parse(dgv.Rows[i].Cells[24].Value.ToString());
                        CurrentItem.Unknown3 = byte.Parse(dgv.Rows[i].Cells[25].Value.ToString());
                        CurrentItem.Unknown4 = byte.Parse(dgv.Rows[i].Cells[26].Value.ToString());
                        CurrentItem.ReqStr = int.Parse(dgv.Rows[i].Cells[27].Value.ToString());
                        CurrentItem.ReqDex = int.Parse(dgv.Rows[i].Cells[28].Value.ToString());
                        CurrentItem.ItemSkill = byte.Parse(dgv.Rows[i].Cells[29].Value.ToString());
                        CurrentItem.IsDamage = byte.Parse(dgv.Rows[i].Cells[30].Value.ToString());
                        CurrentItem.SkillIcon = ushort.Parse(dgv.Rows[i].Cells[31].Value.ToString());
                        //---
                        CurrentItem.Unknown5 = byte.Parse(dgv.Rows[i].Cells[32].Value.ToString());
                        CurrentItem.Unknown6 = byte.Parse(dgv.Rows[i].Cells[33].Value.ToString());
                        CurrentItem.Unknown7 = byte.Parse(dgv.Rows[i].Cells[34].Value.ToString());
                        CurrentItem.Unknown8 = byte.Parse(dgv.Rows[i].Cells[35].Value.ToString());
                        CurrentItem.Unknown9 = byte.Parse(dgv.Rows[i].Cells[36].Value.ToString());
                        CurrentItem.Unknown10 = byte.Parse(dgv.Rows[i].Cells[37].Value.ToString());
                        CurrentItem.Unknown11 = byte.Parse(dgv.Rows[i].Cells[38].Value.ToString());
                        CurrentItem.Unknown12 = byte.Parse(dgv.Rows[i].Cells[39].Value.ToString());
                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_Skill));
                byte[] FileBuffer = new byte[ItemSize * FilledRowCount];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_Skill)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }


                OutputStream.Write(FileBuffer, 0, FileBuffer.Length);
                byte[] crc_bytes = BitConverter.GetBytes(GetCRC(FileBuffer, FileBuffer.Length));
                OutputStream.Write(crc_bytes, 0, crc_bytes.Length);

                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file "); }
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
                        string.Format("{0:d}	{1:d}	\"{2}\"\r\n",
                    dgv.Rows[i].Cells[0].Value.ToString(),
                    dgv.Rows[i].Cells[1].Value.ToString(),
                    dgv.Rows[i].Cells[2].Value.ToString());

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

                //   "ID", "Type", "Name" };

                buf = ASCIIEncoding.ASCII.GetBytes("<xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<npcname>\r\n");

                OutputStream.Write(buf, 0, buf.Length);


                //bah, could use XmlWriter from System.Xml, but nah... this will be faster xD
                for (int i = 0; i < FilledRowCount; i++)
                {



                    line =
                        string.Format("\t<npc id=\"{0:d}\" type=\"{1}\">{2}</npc>\r\n",
                        dgv.Rows[i].Cells[0].Value.ToString(),
                        dgv.Rows[i].Cells[1].Value.ToString(),
                        dgv.Rows[i].Cells[2].Value.ToString());

                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</npcname>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }
}
