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
    class ItemGlow : BmdFile
    {
        public ItemGlow(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }

        public static object[] GetItemsByGroupID(object[] src, int group)
        {
            object[] items = new object[512];
            int nItem = 0;

            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] != null)
                {
                    if ((((MuDef.MUFile_ItemGlow)(src[i])).ItemCode / 512) == group)
                    {
                        items[nItem++] = src[i];
                    }
                }
            }

            return items;
        }

        public override object GetStructure()
        {
            return 0;
        }

        public override object[] GetStructures()
        {
            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemGlow));
            MuDef.MUFile_ItemGlow CurrentItem;
            int LineCount = ReadInt(m_FileStream);
            object[] Res = new object[LineCount];
            m_FileBuffer = new byte[SizeOfItem * LineCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine),
                        typeof(MuDef.MUFile_ItemGlow));
                    XorFilter(ref Item);
                    CurrentItem = (MuDef.MUFile_ItemGlow)Item;
                    Res[m_CurrentLine++] = (object)CurrentItem;
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
                int nSizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemGlow));
                MuDef.MUFile_ItemGlow CurrentItem = new MuDef.MUFile_ItemGlow();
                object[] cur_group;
                byte[] FileBuffer = new byte[nSizeOfItem * items.Length];

                for (int n = 0; n < items.Length; n++)
                {
                    if (items[n] == null)
                    {
                        continue;
                    }
                    CurrentItem = ((MuDef.MUFile_ItemGlow)(items[n]));
                    byte[] buf = StructureToByteArray(items[n]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemGlow)));
                    buf.CopyTo(FileBuffer, n * nSizeOfItem);
                }

                OutputStream.Write(BitConverter.GetBytes(items.Length), 0, 4);
                OutputStream.Write(FileBuffer, 0, items.Length * nSizeOfItem);
               
                OutputStream.Flush();
                OutputStream.Close();
            }
            catch { MessageBox.Show("Failed to save file"); }
        }
        
        public override void BuildTable(DataGridView dgv, object[] items)
        {
        
        }

        public override void SaveAsBmd(string OutputPath, DataGridView dgv)
        {

        }
    }
}
