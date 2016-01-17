using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using zFileMgr.Utils;
using zFileMgr.MuBMD;
using zFileMgr.MuGeneric;
using System.Runtime.InteropServices;

namespace zFileMgr
{
    public partial class CommonEditor : Form
    {
        private byte[] m_XorKey = { 0xFC, 0xCF, 0xAB };
        private MuDef.MUFile_ClientCommon m_Body;
        private string m_SaveFolderPath;
        private int m_FileSize;

        public CommonEditor(string File)
        {
            InitializeComponent();
            init(File);
            fillForm();
        }

        void init(string File)
        {
            m_Body = (MuDef.MUFile_ClientCommon)readInfo(File);
            m_SaveFolderPath = Properties.Settings.Default.SaveFolder;
            Properties.Settings.Default.Save();
        }

        object readInfo(string File)
        {
            FileStream Stream = new FileStream(File, FileMode.Open, FileAccess.Read);
            int ElementSize = Marshal.SizeOf(typeof(MuDef.MUFile_ClientCommon));
            byte[] m_File = new byte[ElementSize + 1];
            m_FileSize = Stream.Read(m_File, 0, ElementSize);
            object DecryptedInfo = Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(m_File, 0), typeof(MuDef.MUFile_ClientCommon));
            BmdFile.XorFilter(ref DecryptedInfo);
            Stream.Close();
            return DecryptedInfo;
        }

        void fillForm()
        {
            MuDef.MUFile_ClientCommon TempInfo = (MuDef.MUFile_ClientCommon)m_Body;
            textBox1.Text = TempInfo.IP;
            textBox4.Text = TempInfo.Version;
            textBox5.Text = TempInfo.Serial;
            numericUpDown1.Value = TempInfo.Port;
            switch (TempInfo.ChatEncoding)
            {
                case 1252:
                    comboBox1.SelectedIndex = 0;
                    break;
                case 1251:
                    comboBox1.SelectedIndex = 1;
                    break;
                case 936:
                    comboBox1.SelectedIndex = 2;
                    break;
                case 874:
                    comboBox1.SelectedIndex = 3;
                    break;
                case 1258:
                    comboBox1.SelectedIndex = 4;
                    break;
                case 1255:
                    comboBox1.SelectedIndex = 5;
                    break;
                case 1361:
                    comboBox1.SelectedIndex = 6;
                    break;
                default:
                    comboBox1.SelectedIndex = 0;
                    break;
            }
            if (m_FileSize == Marshal.SizeOf(typeof(MuDef.MUFile_ClientCommon)))
            {
                label3.ForeColor = Color.FromArgb(TempInfo.ChatGlobalR, TempInfo.ChatGlobalG, TempInfo.ChatGlobalB);
            }
            else
            {
                label3.ForeColor = Color.FromArgb(226, 43, 138);
            }
        }

        void saveBody()
        {
            m_Body.IP = textBox1.Text;
            m_Body.Version = textBox4.Text;
            m_Body.Serial = textBox5.Text;
            m_Body.Port = (ushort)numericUpDown1.Value;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    m_Body.ChatEncoding = 1252;
                    break;
                case 1:
                    m_Body.ChatEncoding = 1251;
                    break;
                case 2:
                    m_Body.ChatEncoding = 936;
                    break;
                case 3:
                    m_Body.ChatEncoding = 874;
                    break;
                case 4:
                    m_Body.ChatEncoding = 1258;
                    break;
                case 5:
                    m_Body.ChatEncoding = 1255;
                    break;
                case 6:
                    m_Body.ChatEncoding = 1361;
                    break;
                default:
                    m_Body.ChatEncoding = 0;
                    break;
            }
            m_Body.ChatGlobalR = label3.ForeColor.R;
            m_Body.ChatGlobalG = label3.ForeColor.G;
            m_Body.ChatGlobalB = label3.ForeColor.B;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog Picker = new ColorDialog();

            if (Picker.ShowDialog() == DialogResult.OK)
            {
                label3.ForeColor = Picker.Color;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FilePath;
            SaveFileDialog Dialog = new SaveFileDialog();
            Dialog.InitialDirectory = m_SaveFolderPath;
            Dialog.Filter = "ZT Config File (*.z)|*.z";

            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                m_SaveFolderPath = Path.GetDirectoryName(Dialog.FileName);
                Properties.Settings.Default.SaveFolder = m_SaveFolderPath;
                FilePath = Dialog.FileName;
            }
            else
            {
                return;
            }

            saveBody();

            FileStream Stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
            int ElementSize = Marshal.SizeOf(typeof(MuDef.MUFile_ClientCommon));
            byte[] Temp = BmdFile.StructureToByteArray(m_Body);
            BmdFile.XorFilter(ref Temp, Marshal.SizeOf(typeof(MuDef.MUFile_ClientCommon)));
            Stream.Write(Temp, 0, ElementSize);
            Stream.Close();

            MessageBox.Show(FilePath + " has been saved", "zFileManager");
            Properties.Settings.Default.SaveFolder = Path.GetDirectoryName(FilePath);
            Properties.Settings.Default.Save();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ColorDialog Picker = new ColorDialog();

            if (Picker.ShowDialog() == DialogResult.OK)
            {
                label3.ForeColor = Picker.Color;
            }
        }
    }
}
