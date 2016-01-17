using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace zDBManager.Character
{
    public partial class SkillsEditor : Form
    {
        public struct SkillInfo
        {
            public short Number;
            public byte Level;
            public Image Icon;
            public Rectangle DrawPos;
        };

        public struct SkillData
        {
            public string Name;
            public short Number;
            public byte CanBeUsedByWizard;
            public byte CanBeUsedByKnight;
            public byte CanBeUsedByElf;
            public byte CanBeUsedByGladiator;
            public byte CanBeUsedByLord;
            public byte CanBeUsedBySummoner;
            public byte CanBeUsedByFighter;
        };

        private Int32 m_SelectedSkill;
        private List<SkillInfo> m_SkillList;
        private List<SkillData> m_SkillData;
        private bool m_SearchProc;

        public SkillsEditor(string Account, string Character)
        {
            m_SelectedSkill = Int32.MaxValue;
            m_SkillList = new List<SkillInfo>();
            m_SkillData = new List<SkillData>();

            InitializeComponent();
            pictureBox1.Image = Image.FromFile("Data\\Images\\Skills\\Unknown.jpg");
            Account_Reload();
            Character_Reload();
            comboBox1.Text = Account;
            comboBox2.Text = Character;
        }

        public void UpdateClassReqInfo()
        {
            listBox1.Items.Clear();
            short SkillNumber = (short)(comboBox3.Items[comboBox3.SelectedIndex] as ComboboxItem).Value;
            SkillData SelectedSkill = new SkillData();
            bool Setted = false;

            for (Int32 i = 0; i < m_SkillData.Count; i++)
            {
                if (m_SkillData[i].Number == SkillNumber)
                {
                    SelectedSkill = m_SkillData[i];
                    Setted = true;
                    break;
                }
            }

            if (!Setted)
            {
                return;
            }

            if (SelectedSkill.CanBeUsedByWizard > 0)
            {
                switch (SelectedSkill.CanBeUsedByWizard)
                {
                    case 1:
                        listBox1.Items.Add("Dark Wizard");
                        break;
                    case 2:
                        listBox1.Items.Add("Soul Master");
                        break;
                    case 3:
                        listBox1.Items.Add("Grand Master");
                        break;
                }
            }

            if (SelectedSkill.CanBeUsedByKnight > 0)
            {
                switch (SelectedSkill.CanBeUsedByKnight)
                {
                    case 1:
                        listBox1.Items.Add("Dark Knight");
                        break;
                    case 2:
                        listBox1.Items.Add("Blade Knight");
                        break;
                    case 3:
                        listBox1.Items.Add("Blade Master");
                        break;
                }
            }

            if (SelectedSkill.CanBeUsedByElf > 0)
            {
                switch (SelectedSkill.CanBeUsedByElf)
                {
                    case 1:
                        listBox1.Items.Add("Elf");
                        break;
                    case 2:
                        listBox1.Items.Add("Muse Elf");
                        break;
                    case 3:
                        listBox1.Items.Add("Hight Elf");
                        break;
                }
            }

            if (SelectedSkill.CanBeUsedByGladiator > 0)
            {
                switch (SelectedSkill.CanBeUsedByGladiator)
                {
                    case 1:
                        listBox1.Items.Add("Magic Gladiator");
                        break;
                    case 3:
                        listBox1.Items.Add("Duel Master");
                        break;
                }
            }

            if (SelectedSkill.CanBeUsedByLord > 0)
            {
                switch (SelectedSkill.CanBeUsedByLord)
                {
                    case 1:
                        listBox1.Items.Add("Dark Lord");
                        break;
                    case 3:
                        listBox1.Items.Add("Lord Emperor");
                        break;
                }
            }

            if (SelectedSkill.CanBeUsedBySummoner > 0)
            {
                switch (SelectedSkill.CanBeUsedBySummoner)
                {
                    case 1:
                        listBox1.Items.Add("Summoner");
                        break;
                    case 2:
                        listBox1.Items.Add("Bloody Summoner");
                        break;
                    case 3:
                        listBox1.Items.Add("Dimension Master");
                        break;
                }
            }

            if (SelectedSkill.CanBeUsedByFighter > 0)
            {
                switch (SelectedSkill.CanBeUsedByFighter)
                {
                    case 1:
                        listBox1.Items.Add("Rage Fighter");
                        break;
                    case 3:
                        listBox1.Items.Add("First Master");
                        break;
                }
            }
        }

        public void DataLoad()
        {
            string[] Lines = System.IO.File.ReadAllLines("Data\\Skill.txt");
            string[] NumberBuffer;

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    ComboboxItem ComboItem = new ComboboxItem();
                    SkillData NewSkill = new SkillData();

                    NumberBuffer = Lines[i].Split(null);

                    var Regular = new System.Text.RegularExpressions.Regex("\".*?\"");
                    var Matches = Regular.Matches(Lines[i]);
                    NewSkill.Name = Matches[0].ToString().Replace("\"", "");

                    NewSkill.Number = short.Parse(NumberBuffer[0]);

                    NewSkill.CanBeUsedByWizard = byte.Parse(NumberBuffer[NumberBuffer.Length - 19]);
                    NewSkill.CanBeUsedByKnight = byte.Parse(NumberBuffer[NumberBuffer.Length - 18]);
                    NewSkill.CanBeUsedByElf = byte.Parse(NumberBuffer[NumberBuffer.Length - 17]);
                    NewSkill.CanBeUsedByGladiator = byte.Parse(NumberBuffer[NumberBuffer.Length - 16]);
                    NewSkill.CanBeUsedByLord = byte.Parse(NumberBuffer[NumberBuffer.Length - 15]);
                    NewSkill.CanBeUsedBySummoner = byte.Parse(NumberBuffer[NumberBuffer.Length - 14]);
                    NewSkill.CanBeUsedByFighter = byte.Parse(NumberBuffer[NumberBuffer.Length - 13]);

                    /*LogWindow.SqlLog.LogAdd(string.Format("Skill -> [" + NewSkill.Number + "] " + NewSkill.Name + "[{0},{1},{2},{3},{4},{5},{6}]",
                        NewSkill.CanBeUsedByWizard, NewSkill.CanBeUsedByKnight, NewSkill.CanBeUsedByElf,
                        NewSkill.CanBeUsedByGladiator, NewSkill.CanBeUsedByLord, NewSkill.CanBeUsedBySummoner,
                        NewSkill.CanBeUsedByFighter));*/

                    m_SkillData.Add(NewSkill);

                    ComboItem.Text = NewSkill.Name;
                    ComboItem.Value = NewSkill.Number;
                    comboBox3.Items.Add(ComboItem);
                }
                catch// (Exception ParseEx)
                {

                    //bool get_end_ok = (lines[i] == "end") ? true : false;

                    //if (get_end_ok) cur_cat++;
                }
            }
        }

        public void Account_Reload()
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            comboBox1.Text = "";
            comboBox1.Items.Clear();
            DBLite.dbMe.Read("select memb___id from MEMB_INFO order by memb___id");
            while (DBLite.dbMe.Fetch())
            {
                comboBox1.Items.Add(DBLite.dbMe.GetAsString("memb___id"));
            }
            DBLite.dbMe.Close();
        }

        public void Character_Reload()
        {
            comboBox2.Text = "";
            comboBox2.Items.Clear();
            if (comboBox1.Text != "")
            {
                DBLite.dbMu.Read("select Name from Character where AccountID = '" + comboBox1.Text + "' order by Name");
                while (DBLite.dbMu.Fetch())
                {
                    comboBox2.Items.Add(DBLite.dbMu.GetAsString("Name"));
                }
                DBLite.dbMu.Close();
            }
        }

        private void InformationSave()
        {
            if (comboBox2.Text == "")
            {
                return;
            }

            byte[] MagicList = new byte[450];

            for (int i = 0; i < 150; i++)
            {
                if (i >= m_SkillList.Count)
                {
                    MagicList[i * 3] = 0xFF;
                    MagicList[i * 3 + 1] = 0;
                    MagicList[i * 3 + 2] = 0;
                }
                else
                {
                    byte Skill = (byte)m_SkillList[i].Number;
                    byte Level = 0;
                    byte Information = 0;

                    if (m_SkillList[i].Number > 765)
                    {
                        Skill = 0xFF;
                        Level = 3;
                        Information = (byte)(m_SkillList[i].Number - Skill * 3);
                    }
                    else if (m_SkillList[i].Number > 510)
                    {
                        Skill = 0xFF;
                        Level = 2;
                        Information = (byte)(m_SkillList[i].Number - Skill * 2);
                    }
                    else if (m_SkillList[i].Number > 255)
                    {
                        Skill = 0xFF;
                        Level = 1;
                        Information = (byte)(m_SkillList[i].Number - Skill);
                    }

                    MagicList[i * 3] = Skill;
                    MagicList[i * 3 + 1] = (byte)(m_SkillList[i].Level << 3);
                    MagicList[i * 3 + 1] |= (byte)(Level & 0x07);
                    MagicList[i * 3 + 2] = Information;
                }
            }
            string magiclistStr = "0x" + System.BitConverter.ToString(MagicList).Replace("-", "");
            DBLite.dbMu.Exec("update Character set magiclist=" + magiclistStr + " where Name='" + comboBox2.Text + "'");
            DBLite.dbMu.Close();
        }

        private void InformationLoad()
        {
            if (comboBox2.Text == "")
            {
                return;
            }
            
            m_SkillList.Clear();

            DBLite.dbMu.Read("select magiclist from Character where Name='" + comboBox2.Text + "'");
            DBLite.dbMu.Fetch();
   
            byte[] MagicList = new byte[450];
            MagicList = DBLite.dbMu.GetAsBinary("magiclist");
            DBLite.dbMu.Close();

            if (MagicList.Length < 450)
            {
                byte[] NewMagicList = new byte[450];
                MagicList.CopyTo(NewMagicList, 1);

                for (Int32 i = MagicList.Length / 3; i < 150; i++)
                {
                    NewMagicList[i * 3] = 0xFF;
                    NewMagicList[i * 3 + 1] = 0;
                    NewMagicList[i * 3 + 2] = 0;
                }

                MagicList = NewMagicList;
            }

            for (Int32 i = 0; i < 150; i++)
            {
                SkillInfo NewSkill;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0:X2}", MagicList[i * 3]);

                short SkillNumber = short.Parse(sb.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);

                sb = new StringBuilder();
                sb.AppendFormat("{0:X2}", MagicList[i * 3 + 1]);

                byte SkillLevel = byte.Parse(sb.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                byte SkillInfo = 255;

                if ((SkillLevel & 7) > 0)
                {
                    sb = new StringBuilder();
                    sb.AppendFormat("{0:X2}", MagicList[i * 3 + 2]);
                    SkillInfo = byte.Parse(sb.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                    SkillNumber = (short)(SkillNumber * (SkillLevel & 7) + SkillInfo);
                }

                const byte StartPosX = 3;
                const byte StartPosY = 4;
                const byte PosXSpace = 6;
                const byte PosYSpace = 7;
                const byte Width = 20;
                const byte Height = 28;

                if (SkillNumber != 255)
                {
                    NewSkill = new SkillInfo();

                    NewSkill.Number = (short)SkillNumber;
                    
                    if (SkillNumber >= 300)
                    {
                        NewSkill.Level = (byte)(SkillLevel >> 3);
                    }
                    else
                    {
                        NewSkill.Level = (byte)(SkillLevel & 0xF);
                    }

                    string Path = "Data\\Images\\Skills\\" + SkillNumber + ".jpg";
                    if (File.Exists(Path))
                    {
                        NewSkill.Icon = Image.FromFile("Data\\Images\\Skills\\" + SkillNumber + ".jpg");
                    }
                    else
                    {
                        NewSkill.Icon = Image.FromFile("Data\\Images\\Skills\\Unknown.jpg");
                    }
                    
                    Int32 PosX = StartPosX + (Width + PosXSpace) * i - (i / 15) * panel1.Width;
                    Int32 PosY = StartPosY + (i / 15) * (Height + PosYSpace);

                    NewSkill.DrawPos = new Rectangle(PosX, PosY, Width, Height);

                    m_SkillList.Add(NewSkill);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics Painter = e.Graphics;

            for (Int32 i = 0; i < m_SkillList.Count; i++)
            {
                if (i == m_SelectedSkill)
                {
                    Painter.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    Brush DrawBrush = new SolidBrush(Color.FromArgb(200, 247, 131, 32));
                    Pen DrawPen = new Pen(DrawBrush, 4);
                    Painter.DrawRectangle(DrawPen, m_SkillList[i].DrawPos);

                    Painter.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                }
                
                Painter.DrawImage(m_SkillList[i].Icon, m_SkillList[i].DrawPos);
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point MousePoint = new Point(e.X, e.Y);

            for (Int32 i = 0; i < m_SkillList.Count; i++)
            {
                if (m_SkillList[i].DrawPos.Contains(MousePoint))
                {
                    m_SelectedSkill = i;
                    textBox2.Text = m_SkillList[i].Number.ToString();

                    for (Int32 j = 0; j < m_SkillData.Count; j++)
                    {
                        if (m_SkillData[j].Number == m_SkillList[i].Number)
                        {
                            textBox1.Text = m_SkillData[j].Name;
                            break;
                        }
                    }

                    numericUpDown1.Value = m_SkillList[i].Level;
                    panel1.Refresh();
                    break;
                }
            }
        }

        private void SkillsEditor_Load(object sender, EventArgs e)
        {
            DataLoad();
            //InformationLoad();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            InformationSave();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (Int32 i = 0; i < m_SkillList.Count; i++)
            {
                if (i == m_SelectedSkill)
                {
                    m_SelectedSkill = Int32.MaxValue;
                    m_SkillList.RemoveAt(i);
                    break;
                }
            }

            const byte StartPosX = 3;
            const byte StartPosY = 4;
            const byte PosXSpace = 6;
            const byte PosYSpace = 7;
            const byte Width = 20;
            const byte Height = 28;

            for (Int32 i = 0; i < m_SkillList.Count; i++)
            {
                SkillInfo ModifySkill = m_SkillList[i];

                string Path = "Data\\Images\\Skills\\" + ModifySkill.Number + ".jpg";
                if (File.Exists(Path))
                {
                    ModifySkill.Icon = Image.FromFile("Data\\Images\\Skills\\" + ModifySkill.Number + ".jpg");
                }
                else
                {
                    ModifySkill.Icon = Image.FromFile("Data\\Images\\Skills\\Unknown.jpg");
                }

                Int32 PosX = StartPosX + (Width + PosXSpace) * i - (i / 15) * panel1.Width;
                Int32 PosY = StartPosY + (i / 15) * (Height + PosYSpace);

                ModifySkill.DrawPos = new Rectangle(PosX, PosY, Width, Height);

                m_SkillList[i] = ModifySkill;
            }

            panel1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (Int32 i = 0; i < m_SkillList.Count; i++)
            {
                if (i == m_SelectedSkill)
                {
                    SkillInfo ModifySkill = m_SkillList[i];
                    ModifySkill.Level = (byte)numericUpDown1.Value;
                    m_SkillList[i] = ModifySkill;
                    break;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_SearchProc)
            {
                Character_Reload();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                return;
            }

            m_SearchProc = true;

            DBLite.dbMu.Read("select AccountID from Character where Name = '" + comboBox2.Text + "'");
            DBLite.dbMu.Fetch();
            comboBox1.Text = DBLite.dbMu.GetAsString("AccountID");
            DBLite.dbMu.Close();

            if (comboBox1.Text == "")
            {
                m_SearchProc = false;
                return;
            }

            string CharName = comboBox2.Text;
            Character_Reload();

            for (Int32 i = 0; i < comboBox2.Items.Count; i++)
            {
                if (comboBox2.Items[i].ToString() == CharName)
                {
                    comboBox2.SelectedIndex = i;
                    break;
                }
            }

            //InformationLoad();
            m_SearchProc = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            InformationLoad();
            m_SelectedSkill = Int32.MaxValue;
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            panel1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            m_SelectedSkill = Int32.MaxValue;
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            m_SkillList = new List<SkillInfo>();
            panel1.Refresh();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            short SkillNumber = (short)(comboBox3.Items[comboBox3.SelectedIndex] as ComboboxItem).Value;
            string Path = "Data\\Images\\Skills\\" + SkillNumber + ".jpg";

            if (File.Exists(Path))
            {
                pictureBox1.Image = Image.FromFile("Data\\Images\\Skills\\" + SkillNumber + ".jpg");
            }
            else
            {
                pictureBox1.Image = Image.FromFile("Data\\Images\\Skills\\Unknown.jpg");
            }

            textBox3.Text = SkillNumber.ToString();
            UpdateClassReqInfo();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                return;
            }

            if (comboBox3.SelectedIndex < 0)
            {
                return;
            }

            short SkillNumber = (short)(comboBox3.Items[comboBox3.SelectedIndex] as ComboboxItem).Value;

            for (Int32 i = 0; i < m_SkillList.Count; i++)
            {
                if (m_SkillList[i].Number == SkillNumber)
                {
                    MessageBox.Show("This skill already exist", "zDBManager", MessageBoxButtons.OK);

                    m_SelectedSkill = i;
                    textBox2.Text = m_SkillList[i].Number.ToString();

                    for (Int32 j = 0; j < m_SkillData.Count; j++)
                    {
                        if (m_SkillData[j].Number == m_SkillList[i].Number)
                        {
                            textBox1.Text = m_SkillData[j].Name;
                            break;
                        }
                    }

                    numericUpDown1.Value = m_SkillList[i].Level;
                    panel1.Refresh();
                    return;
                }
            }
            
            const byte StartPosX = 3;
            const byte StartPosY = 4;
            const byte PosXSpace = 6;
            const byte PosYSpace = 7;
            const byte Width = 20;
            const byte Height = 28;

            SkillInfo NewSkill = new SkillInfo();
            NewSkill.Number = SkillNumber;
            NewSkill.Level = 0;

            string Path = "Data\\Images\\Skills\\" + NewSkill.Number + ".jpg";
            if (File.Exists(Path))
            {
                NewSkill.Icon = Image.FromFile("Data\\Images\\Skills\\" + NewSkill.Number + ".jpg");
            }
            else
            {
                NewSkill.Icon = Image.FromFile("Data\\Images\\Skills\\Unknown.jpg");
            }

            Int32 PosX = StartPosX + (Width + PosXSpace) * m_SkillList.Count - (m_SkillList.Count / 15) * panel1.Width;
            Int32 PosY = StartPosY + (m_SkillList.Count / 15) * (Height + PosYSpace);

            NewSkill.DrawPos = new Rectangle(PosX, PosY, Width, Height);
            m_SkillList.Add(NewSkill);
            panel1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                return;
            }

            for (Int32 i = 0; i < comboBox3.Items.Count; i++)
            {
                if ((comboBox3.Items[i] as ComboboxItem).Value == short.Parse(textBox3.Text))
                {
                    comboBox3.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    public partial class PanelEx : Panel
    {
        public PanelEx()
        {
            this.DoubleBuffered = true;
        }
    }
}
