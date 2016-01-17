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
    class MasterSkillTreeData : BmdFile
    {
        const int cTotalStructCount = 400;

        //nothing to do here, just pass data to generic constructor
        public MasterSkillTreeData(string _path, FileType _type) :
            base(_path, _type) { }



        public override void BuildTable(DataGridView dgv, object[] items)
        {
            DataGridViewColumn col;
            string[] col_names = { "MasterSkillID", "Tmp", "Type", "Group", "RequiredPoints", "MaxLevel", "Unknown1", "Skill1", "Skill2", "Skill3", "DefValue" };


            for (int i = 0; i < col_names.Length; i++)
            {
                col = new DataGridViewTextBoxColumn();
                col.Name = col_names[i];
                dgv.Columns.Add(col);

            }

            for (int i = 0; i < items.Length; i++)
            {
                MuDef.MUFile_MasterSkillTreeData Item;

                if (items[i] != null)
                {
                    Item = (MuDef.MUFile_MasterSkillTreeData)items[i];
                    dgv.Rows.Add(
                        Item.MasterSkillID,
                        Item.Tmp,
                        Item.Type,
                        Item.Group,
                        Item.RequiredPoints,
                        Item.MaxLevel,
                        Item.Unknown1,
                        Item.Skill1,
                        Item.Skill2,
                        Item.Skill3,
                        Item.DefValue


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

            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_MasterSkillTreeData));
            MuDef.MUFile_MasterSkillTreeData CurrentItem;

            m_FileBuffer = new byte[SizeOfItem * cTotalStructCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine++),
                        typeof(MuDef.MUFile_MasterSkillTreeData));
                    XorFilter(ref Item);

                    CurrentItem = (MuDef.MUFile_MasterSkillTreeData)Item;

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

                List<MuDef.MUFile_MasterSkillTreeData> TmpList = new List<MuDef.MUFile_MasterSkillTreeData>(dgv.Rows.Count);
                MuDef.MUFile_MasterSkillTreeData CurrentItem = new MuDef.MUFile_MasterSkillTreeData();


                //last row is null
                for (int i = 0; i < FilledRowCount; i++)
                {
                    //if (dgv.Rows[i].Cells[0].Value != null)
                    {
                        CurrentItem.MasterSkillID = byte.Parse(dgv.Rows[i].Cells[0].Value.ToString());
                        CurrentItem.Tmp = byte.Parse(dgv.Rows[i].Cells[1].Value.ToString());
                        CurrentItem.Type = ushort.Parse(dgv.Rows[i].Cells[2].Value.ToString());
                        CurrentItem.Group = byte.Parse(dgv.Rows[i].Cells[3].Value.ToString());
                        CurrentItem.RequiredPoints = byte.Parse(dgv.Rows[i].Cells[4].Value.ToString());
                        CurrentItem.MaxLevel = byte.Parse(dgv.Rows[i].Cells[5].Value.ToString());
                        CurrentItem.Unknown1 = byte.Parse(dgv.Rows[i].Cells[6].Value.ToString());
                        CurrentItem.Skill1 = int.Parse(dgv.Rows[i].Cells[7].Value.ToString());
                        CurrentItem.Skill2 = int.Parse(dgv.Rows[i].Cells[8].Value.ToString());
                        CurrentItem.Skill3 = int.Parse(dgv.Rows[i].Cells[9].Value.ToString());
                        CurrentItem.DefValue = float.Parse(dgv.Rows[i].Cells[10].Value.ToString());

                        TmpList.Add(CurrentItem);
                    }

                }

                int ItemSize = Marshal.SizeOf(typeof(MuDef.MUFile_MasterSkillTreeData));
                int TotalSize = ItemSize * FilledRowCount;

                byte[] FileBuffer = new byte[TotalSize];

                //write
                for (int i = 0; i < FilledRowCount; i++)
                {
                    byte[] buf = StructureToByteArray(TmpList[i]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_MasterSkillTreeData)));
                    buf.CopyTo(FileBuffer, i * ItemSize);
                }

                OutputStream.Write(FileBuffer, 0, TotalSize);

                //crc
                byte[] crc = new byte[4];
                crc = BitConverter.GetBytes(GetCRC(FileBuffer, TotalSize));

                OutputStream.Write(crc, 0, crc.Length);
                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
    }

}
