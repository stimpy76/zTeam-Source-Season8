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
    class ItemExcellentOption : BmdFile
    {
        public ItemExcellentOption(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }

        public static object[] GetItemsByGroupID(object[] src, int group)
        {
            object[] items = new object[512];
            int nItem = 0;

            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] != null)
                {
                    if ((((MuDef.MUFile_ItemExcellentOption)(src[i])).ItemCode / 512) == group)
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
            int SizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemExcellentOption));
            
            MuDef.MUFile_ItemExcellentOption CurrentItem;
            int LineCount = ReadInt(m_FileStream);

            object[] Res = new object[LineCount];
            m_FileBuffer = new byte[SizeOfItem * LineCount];

            try
            {
                while ((m_FileStream.Read(m_FileBuffer, SizeOfItem * m_CurrentLine, SizeOfItem)) == SizeOfItem)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, SizeOfItem * m_CurrentLine),
                        typeof(MuDef.MUFile_ItemExcellentOption));
                    XorFilter(ref Item);
                    CurrentItem = (MuDef.MUFile_ItemExcellentOption)Item;
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
                int nSizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemExcellentOption));
                MuDef.MUFile_ItemExcellentOption CurrentItem = new MuDef.MUFile_ItemExcellentOption();
                object[] cur_group;
                byte[] FileBuffer = new byte[nSizeOfItem * items.Length];

                for (int n = 0; n < items.Length; n++)
                {
                    if (items[n] == null)
                    {
                        continue;
                    }
                    CurrentItem = ((MuDef.MUFile_ItemExcellentOption)(items[n]));
                    byte[] buf = StructureToByteArray(items[n]);
                    XorFilter(ref buf, Marshal.SizeOf(typeof(MuDef.MUFile_ItemExcellentOption)));
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

        public override void SaveAsXml(string OutputPath, object[] items)
        {
            try
            {
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);
                int nSizeOfItem = Marshal.SizeOf(typeof(MuDef.MUFile_ItemExcellentOption));
                MuDef.MUFile_ItemExcellentOption CurrentItem = new MuDef.MUFile_ItemExcellentOption();
                object[] cur_group;
                byte[] FileBuffer = new byte[nSizeOfItem * items.Length];

                byte[] buf = new byte[1000];
                buf = ASCIIEncoding.ASCII.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<itemexcellentoption>\r\n");
                OutputStream.Write(buf, 0, buf.Length);

                for (int n = 0; n < items.Length; n++)
                {
                    if (items[n] == null)
                    {
                        continue;
                    }
                    CurrentItem = ((MuDef.MUFile_ItemExcellentOption)(items[n]));

                    string line = string.Format("\t<item type=\"{0}\" index=\"{1}\">\n\t\t<option index=\"0\" value=\"{2}\"/>\n\t\t<option index=\"1\" value=\"{3}\"/>\n\t\t<option index=\"2\" value=\"{4}\"/>\n\t\t<option index=\"3\" value=\"{5}\"/>\n\t\t<option index=\"4\" value=\"{6}\"/>\n\t\t<option index=\"5\" value=\"{7}\"/>\n\t</item>\r\n",
                        CurrentItem.ItemCode / 512, CurrentItem.ItemCode % 512,
                        CurrentItem.Option0, CurrentItem.Option1,
                        CurrentItem.Option2, CurrentItem.Option3,
                        CurrentItem.Option4, CurrentItem.Option5);
                    buf = ASCIIEncoding.ASCII.GetBytes(line);
                    OutputStream.Write(buf, 0, buf.Length);
                }

                buf = ASCIIEncoding.ASCII.GetBytes("</itemexcellentoption>");
                OutputStream.Write(buf, 0, buf.Length);

                OutputStream.Flush();
                OutputStream.Close();

                Main.Log("File {0} saved", OutputPath);
            }
            catch 
            { 
                MessageBox.Show("Failed to save file"); 
            }
        }
    }
}
