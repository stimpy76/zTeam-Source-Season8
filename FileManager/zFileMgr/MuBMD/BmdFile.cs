using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using System.Runtime.InteropServices;

namespace zFileMgr.MuBMD
{
    public abstract class BmdFile
    {
        public enum FileType
        {
            BuffEffect = 0,
            Credit = 1,
            Gate = 2,
            Dialog = 3,
            Filter = 4,
            FilterName = 5,
            MoveReq = 6,
            Minimap = 7,
            NPCDialogue = 8,
            Pet = 9,
            PetData = 10,
            ServerList = 11,
            Slide = 12,
            Text = 13,
            JewelOfHarmonyOption = 14,
            SocketItem = 15,
            NPCName = 16,
            InfoTooltipText = 17,
            MasterSkillTooltip = 18,
            ItemTooltipText = 19,
            Skill = 20,
            Item = 21,
            MasterSkillTreeData = 22,
            MonsterSkill = 23,
            ItemLevelTooltip = 24,
            QuestProgress = 26,
            QuestWords = 27,
            HelpData = 28,
            ItemAddOption = 29,
            ItemSetType = 30,
            ItemTRSData = 31,
            ItemSetOptionText = 32,
            ItemSetOption = 33,
            ItemGlow = 34,
            ClientCommon = 35,
        }

        public enum BmdConversationType
        {
            BmdToText,
            TextToBmd,
            BmdToXml,
            XmlToBmd,
            XmlToText,
            TextToXml
        }

        protected string m_FilePath;
        protected FileType m_FileType;
        protected FileStream m_FileStream;
        protected byte[] m_FileBuffer;
        protected int m_CurrentLine;
        public List<BmdConversationType> AllowedConversationModes;

        private byte[] m_XorKey = { 0xFC, 0xCF, 0xAB };

        public FileType MyType
        {
            get { return m_FileType; }
        }

        public BmdFile(string _path, FileType _type, params BmdConversationType[] ConvTypes)
        {
            m_FilePath = _path;
            m_FileType = _type;

            AllowedConversationModes = new List<BmdConversationType>(ConvTypes.Length);
            AllowedConversationModes.AddRange(ConvTypes);

            m_CurrentLine = 0;


            m_FileStream = File.Open(_path, FileMode.Open, FileAccess.Read);
        }


        //abstract methods that has to be implemented in derived class
        public abstract object[] GetStructures();
        public abstract object GetStructure();
        public abstract void BuildTable(DataGridView dgv, object[] items);

        //all bmd files can be saved as bmd lol
        public abstract void SaveAsBmd(string OutputPath, DataGridView dgv);


        //for Item.bmd only at the moment
        public virtual void SaveAsBmd(string OutputPath, object[] items)
        {

        }

        public virtual void SaveAsBmd(string OutputPath, object item)
        {

        }

        public virtual void SaveAsText(string OutputPath, DataGridView dgv)
        {
            MessageBox.Show("This file processor doesent have SaveAsText() method implemented");
        }

        public virtual void SaveAsXml(string OutputPath, DataGridView dgv)
        {
            MessageBox.Show("This file processor doesent have SaveAsXml() method implemented");
        }

        public virtual void SaveAsXml(string OutputPath, object[] items)
        {
            MessageBox.Show("This file processor doesent have SaveAsXml() method implemented");
        }

        //accessable from ClientEditor
        protected void CloseSourceFile()
        {
            try
            {
                m_FileStream.Close();
            }
            catch { }
        }

        public virtual void EnumerateLines(DataGridView dgv)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                dgv.Rows[i].HeaderCell.Value = i.ToString();
            }
            // ----
            dgv.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }
        
        protected uint GetCRC(byte[] Buffer, int Size)
        {
            uint KeyVal = 0;
            // ----
            switch (m_FileType)
            {
                case FileType.BuffEffect:
                    KeyVal = 0xE2F1;
                    break;
                case FileType.Filter:
                    KeyVal = 0x3E7D;
                    break;
                case FileType.FilterName:
                    KeyVal = 0x2BC1;
                    break;
                case FileType.InfoTooltipText:
                    KeyVal = 0xA4C6;
                    break;
                case FileType.MasterSkillTooltip:
                    KeyVal = 0x2BC1;
                    break;
                case FileType.ItemTooltipText:
                    KeyVal = 0xE2F1;
                    break;
                case FileType.Skill:
                    KeyVal = 0x5A18;
                    break;
                case FileType.Item:
                    KeyVal = 0xE2F1;
                    break;
                case FileType.ItemLevelTooltip:
                    KeyVal = 0xE2F1;
                    break;
                case FileType.HelpData:
                    KeyVal = 0xC4F7;
                    break;
                case FileType.MasterSkillTreeData:
                    KeyVal = 0x2BC1;
                    break;
                case FileType.Minimap:
                    KeyVal = 0xCE54;
                    break;
                case FileType.ItemTRSData:
                    KeyVal = 0xE2F1;
                    break;
                case FileType.ItemSetOptionText:
                    KeyVal = 0xE2F1;
                    break;
                case FileType.ItemSetOption:
                    KeyVal = 0xA2F1;
                    break;
                case FileType.ItemSetType:
                    KeyVal = 0xE5F1;
                    break;
            }
            // ----
            uint CrcKey = KeyVal * 512;
            // ----
            for (uint i = 0; i <= (Size - 4); i += 4)
            {
                uint BufferKey = BitConverter.ToUInt32(Buffer, (int)i);

                switch ((((i / 4) + KeyVal) % 2))
                {
                    case 0:
                        CrcKey ^= BufferKey;
                        break;
                    case 1:
                        CrcKey += BufferKey;
                        break;
                }
                // ----
                if ((i % 16) == 0)
                {
                    CrcKey ^= (KeyVal + CrcKey) >> (int)(((i / 4) % 8) + 1);
                }
            }
            // ----
            Main.Log(string.Format("Crc key: {0}", CrcKey));
            return CrcKey;
        }
        // ----
        public static void XorFilter(ref object Data)
        {
            byte[] m_XorKey = { 0xFC, 0xCF, 0xAB };
            byte[] Buffer = StructureToByteArray(Data);
            // ----
            for (int i = 0; i < Marshal.SizeOf(Data); i++)
            {
                Buffer[i] ^= m_XorKey[i % m_XorKey.Length];
            }
            // ----
            ByteArrayToStructure(Buffer, ref Data);
        }

        public static void XorFilter(ref object[] Data, int Size)
        {
            byte[] m_XorKey = { 0xFC, 0xCF, 0xAB };
            for (int n = 0; n < Data.Length; n++)
            {
                byte[] Buffer = StructureToByteArray(Data[n]);
                // ----
                for (int i = 0; i < Size; i++)
                {
                    Buffer[i] ^= m_XorKey[i % m_XorKey.Length];
                }
                // ----
                object tData = Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(Data, Size * n),
                        typeof(MuGeneric.MuDef.MUFile_ItemGlow));
                ByteArrayToStructure(Buffer, ref tData);
                Data[n] = tData;
            }
        }
        // ----
        public static void XorFilter(ref byte[] Data, int Size)
        {
            byte[] m_XorKey = { 0xFC, 0xCF, 0xAB };
            for (int i = 0; i < Size; i++)
            {
                Data[i] ^= m_XorKey[i % m_XorKey.Length];
            }
        }
        // ----
        public static void XorFilter(byte[] Data, int StartIndex, int Size)
        {
            byte[] m_XorKey = { 0xFC, 0xCF, 0xAB };
            for (int i = 0; i < Size; i++)
            {
                Data[StartIndex + i] ^= m_XorKey[i % m_XorKey.Length];
            }
        }
        // ----
        public static byte[] StructureToByteArray(object Data)
        {
            byte[] Buffer = new byte[Marshal.SizeOf(Data)];
            IntPtr Ptr = Marshal.AllocHGlobal(Marshal.SizeOf(Data));
            Marshal.StructureToPtr(Data, Ptr, true);
            Marshal.Copy(Ptr, Buffer, 0, Marshal.SizeOf(Data));
            Marshal.FreeHGlobal(Ptr);
            return Buffer;
        }
        // ----
        public static void ByteArrayToStructure(byte[] Array, ref object Data)
        {
            IntPtr Ptr = Marshal.AllocHGlobal(Marshal.SizeOf(Data));
            Marshal.Copy(Array, 0, Ptr, Marshal.SizeOf(Data));
            Data = Marshal.PtrToStructure(Ptr, Data.GetType());
            Marshal.FreeHGlobal(Ptr);
        }

        // ----
        protected int ReadInt(FileStream File)
        {
            Byte[] Buffer = new Byte[sizeof(int)];
            File.Read(Buffer, 0, sizeof(int));
            return BitConverter.ToInt32(Buffer, 0);
        }
        // ----

        public short ReadShort(FileStream File)
        {
            Byte[] Buffer = new Byte[sizeof(short)];
            File.Read(Buffer, 0, sizeof(short));
            return BitConverter.ToInt16(Buffer, 0);
        }
        // ----
        public ushort ReadUShort(FileStream File)
        {
            Byte[] Buffer = new Byte[sizeof(ushort)];
            File.Read(Buffer, 0, sizeof(ushort));
            return BitConverter.ToUInt16(Buffer, 0);
        }

        public void WriteInt32(FileStream File, Int32 val)
        {
            byte[] Buffer = BitConverter.GetBytes(val);
            File.Write(Buffer, 0, sizeof(int));
        }


    }
}
