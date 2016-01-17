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
    class ClientCommon : BmdFile
    {

        public ClientCommon(string _path, FileType _type) :
            base(_path, _type, BmdConversationType.BmdToText, BmdConversationType.BmdToXml) { }

        public override object[] GetStructures()
        {
            return new object[1];
        }

        public void XorFilterZ(ref object Data, int Size)
        {
            byte[] XorKey = { 0xFC, 0xCF, 0xAB };
            byte[] Buffer = StructureToByteArray(Data);
            // ----
            for (int i = 0; i < Size; i++)
            {
                Buffer[i] ^= XorKey[i % XorKey.Length];
            }
            // ----
            ByteArrayToStructure(Buffer, ref Data);
        }

        public override object GetStructure()
        {
            int ElementSize = Marshal.SizeOf(typeof(MuDef.MUFile_ClientCommon));
            m_FileBuffer = new byte[ElementSize];

            try
            {
                int ReadedSize = m_FileStream.Read(m_FileBuffer, 0, ElementSize);
                if (ReadedSize == ElementSize)
                {
                    object Item = Marshal.PtrToStructure(
                        Marshal.UnsafeAddrOfPinnedArrayElement(m_FileBuffer, ElementSize),
                        typeof(MuDef.MUFile_ClientCommon));
                    XorFilterZ(ref Item, ElementSize);
                    CloseSourceFile();
                    return Item;
                }
            }
            catch (ArgumentException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }
            catch (Exception)
            {
                MessageBox.Show("Failed to read file structures.");
            }

            CloseSourceFile();
            return 0;
        }

        public override void SaveAsBmd(string OutputPath, object item)
        {
            try
            {
                FileStream OutputStream = File.Open(OutputPath, FileMode.Create, FileAccess.Write);
                int ElementSize = Marshal.SizeOf(typeof(MuDef.MUFile_ClientCommon));

                byte[] Buffer = new byte[ElementSize];
                IntPtr Ptr = Marshal.AllocHGlobal(ElementSize);
                Marshal.StructureToPtr(item, Ptr, true);
                Marshal.Copy(Ptr, Buffer, 0, ElementSize);
                Marshal.FreeHGlobal(Ptr);

                byte[] TempBuffer = StructureToByteArray(item);
                
                XorFilter(ref Buffer, ElementSize);
                OutputStream.Write(Buffer, 0, ElementSize);
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
