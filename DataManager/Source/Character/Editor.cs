using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace zDBManager.Character
{
    partial class Editor : Form
    {
        private byte[] m_QuestData = new byte[50];
        private bool m_HaveParent;
        private bool m_SearchProc;

        public Editor(string Account, string User)
        {
            InitializeComponent();

            Account_Reload();
            Character_Reload();

            comboBox1.Text = Account;
            comboBox2.Text = User;

            if (Account != "")
            {
                button6.Enabled = false;
                button7.Enabled = false;
                m_HaveParent = true;
            }

            ComboboxItem ComboItem;

            ComboItem = new ComboboxItem();
            ComboItem.Text = "Dark Wizard";
            ComboItem.Value = 0;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Soul Master";
            ComboItem.Value = 1;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Grand Master";
            ComboItem.Value = 3;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Dark Knight";
            ComboItem.Value = 16;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Blade Knight";
            ComboItem.Value = 17;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Blade Master";
            ComboItem.Value = 19;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Fairy Elf";
            ComboItem.Value = 32;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Muse Elf";
            ComboItem.Value = 33;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Hight Elf";
            ComboItem.Value = 35;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Magic Gladiator";
            ComboItem.Value = 48;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Duel Master";
            ComboItem.Value = 50;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Dark Lord";
            ComboItem.Value = 64;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Lord Emperor";
            ComboItem.Value = 66;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Summoner";
            ComboItem.Value = 80;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Bloody Summoner";
            ComboItem.Value = 81;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Dimension Master";
            ComboItem.Value = 83;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Rage Fighter";
            ComboItem.Value = 96;
            comboBox3.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "First Master";
            ComboItem.Value = 98;
            comboBox3.Items.Add(ComboItem);

            ComboItem = new ComboboxItem();
            ComboItem.Text = "None";
            ComboItem.Value = 255;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Find the 'Scrool of Emperor'";
            ComboItem.Value = 254;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Three Treasures of Mu";
            ComboItem.Value = 250;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Gain 'Hero Status'";
            ComboItem.Value = 234;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Secret of 'Dark Stone' (BK)";
            ComboItem.Value = 170;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Certificate of Strength! (I)";
            ComboItem.Value = 176;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Certificate of Strength! (II)";
            ComboItem.Value = 177;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Certificate of Strength! (III)";
            ComboItem.Value = 178;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Infiltration of Barracks of Ballgass! (I)";
            ComboItem.Value = 179;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Infiltration of Barracks of Ballgass! (II)";
            ComboItem.Value = 180;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Infiltration of Refuge! (I)";
            ComboItem.Value = 181;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Infiltration of Refuge! (II)";
            ComboItem.Value = 182;
            comboBox4.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Completed 3rd class";
            ComboItem.Value = 186;
            comboBox4.Items.Add(ComboItem);

            ComboItem = new ComboboxItem();
            ComboItem.Text = "Normal";
            ComboItem.Value = 0;
            comboBox5.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Blocked";
            ComboItem.Value = 1;
            comboBox5.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Items Blocked";
            ComboItem.Value = 1;
            comboBox5.Items.Add(ComboItem);
            ComboItem = new ComboboxItem();
            ComboItem.Text = "Game Master";
            ComboItem.Value = 32;
            comboBox5.Items.Add(ComboItem);

            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

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

        public void InformationLoad()
        {
            if (comboBox2.Text == "")
            {
                return;
            }

            DBLite.dbMu.Read("select * from Character where Name = '" + comboBox2.Text + "'");
            DBLite.dbMu.Fetch();

            int Class = DBLite.dbMu.GetAsInteger("Class");
            int Access = DBLite.dbMu.GetAsInteger("CtlCode");

            for (Int32 i = 0; i < comboBox3.Items.Count; i++)
            {
                 if( (comboBox3.Items[i] as ComboboxItem).Value == Class )
                 {
                     comboBox3.SelectedIndex = i;
                     break;
                 }
            }

            for (Int32 i = 0; i < comboBox5.Items.Count; i++)
            {
                if ((comboBox5.Items[i] as ComboboxItem).Value == Access)
                {
                    comboBox5.SelectedIndex = i;
                    break;
                }
            }

            numericUpDown1.Value = DBLite.dbMu.GetAsInteger("cLevel");
            numericUpDown2.Value = DBLite.dbMu.GetAsInteger("Resets");
            numericUpDown3.Value = DBLite.dbMu.GetAsInteger("LevelUpPoint");
            numericUpDown4.Value = DBLite.dbMu.GetAsInteger64("Experience");
            numericUpDown9.Value = DBLite.dbMu.GetAsInteger("Strength");
            numericUpDown8.Value = DBLite.dbMu.GetAsInteger("Dexterity");
            numericUpDown7.Value = DBLite.dbMu.GetAsInteger("Vitality");
            numericUpDown6.Value = DBLite.dbMu.GetAsInteger("Energy");
            numericUpDown5.Value = DBLite.dbMu.GetAsInteger("Leadership");
            numericUpDown10.Value = DBLite.dbMu.GetAsInteger("ExInventory");
            numericUpDown12.Value = DBLite.dbMu.GetAsInteger("ChatLimitTime");
            numericUpDown11.Value = DBLite.dbMu.GetAsInteger("Money");

            comboBox7.SelectedIndex = DBLite.dbMu.GetAsInteger("PkLevel") - 1;

            if (comboBox7.SelectedIndex < 0)
            {
                comboBox7.SelectedIndex = 0;
            }

            if (comboBox7.SelectedIndex > 5)
            {
                comboBox7.SelectedIndex = 5;
            }

            numericUpDown19.Value = DBLite.dbMu.GetAsInteger("PkCount");
            numericUpDown18.Value = DBLite.dbMu.GetAsInteger("PkTime");

            m_QuestData = DBLite.dbMu.GetAsBinary("Quest");
            DBLite.dbMu.Close();

            for (Int32 i = 0; i < comboBox4.Items.Count; i++)
            {
                if ((comboBox4.Items[i] as ComboboxItem).Value == m_QuestData[0])
                {
                    comboBox4.SelectedIndex = i;
                    break;
                }
            }

            DBLite.dbMu.Read("select * from GensUserInfo where memb_char = '" + comboBox2.Text + "'");
            DBLite.dbMu.Fetch();
            comboBox6.SelectedIndex = DBLite.dbMu.GetAsInteger("memb_clan");
            numericUpDown17.Value = DBLite.dbMu.GetAsInteger("memb_rank");
            numericUpDown16.Value = DBLite.dbMu.GetAsInteger("memb_contribution");
            DBLite.dbMu.Close();

            DBLite.dbMu.Read("select * from T_MasterLevelSystem where CHAR_NAME = '" + comboBox2.Text + "'");
            DBLite.dbMu.Fetch();
            numericUpDown13.Value = DBLite.dbMu.GetAsInteger("MASTER_LEVEL");
            numericUpDown14.Value = DBLite.dbMu.GetAsInteger64("ML_EXP");
            numericUpDown15.Value = DBLite.dbMu.GetAsInteger("ML_POINT");
            DBLite.dbMu.Close();
        }

        public void InformationSave()
        {
            if (comboBox2.Text == "")
            {
                return;
            }

            m_QuestData[0] = (byte)(comboBox4.Items[comboBox4.SelectedIndex] as ComboboxItem).Value;
            string QuestFill = "0x" + System.BitConverter.ToString(m_QuestData).Replace("-", "");

            DBLite.dbMu.Exec("update Character set cLevel = " + numericUpDown1.Value + ", Class = " + (comboBox3.Items[comboBox3.SelectedIndex] as ComboboxItem).Value + ", CtlCode = " + (comboBox5.Items[comboBox5.SelectedIndex] as ComboboxItem).Value
                + ", Resets = " + numericUpDown2.Value + ", LevelUpPoint = " + numericUpDown3.Value + ", Experience = " + numericUpDown4.Value + ", Strength = " + numericUpDown9.Value + ", Dexterity = " + numericUpDown8.Value + ", Vitality = " + numericUpDown7.Value
                + ", Energy = " + numericUpDown6.Value + ", Leadership = " + numericUpDown5.Value + ", ExInventory = " + numericUpDown10.Value + ", ChatLimitTime = " + numericUpDown12.Value + ", Money = " + numericUpDown11.Value + ", PkLevel = " + (comboBox7.SelectedIndex + 1)
                + ", PkCount = " + numericUpDown19.Value + ", PkTime = " + numericUpDown18.Value + ", Quest = "+ QuestFill +" where Name = '" + comboBox2.Text + "'");
            DBLite.dbMu.Close();

            Int32 Result = DBLite.dbMu.ExecWithResult("select count(*) from GensUserInfo where memb_char = '" + comboBox2.Text + "'");
            DBLite.dbMu.Close();

            if (Result > 0)
            {
                DBLite.dbMu.Exec("update GensUserInfo set memb_clan = " + comboBox6.SelectedIndex + ", memb_rank = " + numericUpDown17.Value + ", memb_contribution = " + numericUpDown16.Value
                    + " where memb_char = '" + comboBox2.Text + "'");
            }
            else
            {
                DBLite.dbMu.Exec("insert into GensUserInfo (memb_char, memb_clan, memb_rank, memb_contribution, memb_reward) values('"+ comboBox2.Text +"', "+ comboBox6.SelectedIndex
                    + ", "+ numericUpDown17.Value +", "+ numericUpDown16.Value +", 0)");
            }
            DBLite.dbMu.Close();

            Result = DBLite.dbMu.ExecWithResult("select count(*) from T_MasterLevelSystem where CHAR_NAME = '" + comboBox2.Text + "'");
            DBLite.dbMu.Close();

            if (Result > 0)
            {
                DBLite.dbMu.Exec("update T_MasterLevelSystem set MASTER_LEVEL = " + numericUpDown13.Value + ", ML_EXP = " + numericUpDown14.Value + ", ML_POINT = " + numericUpDown15.Value
                    + " where CHAR_NAME = '" + comboBox2.Text + "'");
            }
            else
            {
                DBLite.dbMu.Exec("insert into T_MasterLevelSystem (CHAR_NAME, MASTER_LEVEL, ML_EXP, ML_NEXTEXP, ML_POINT) values('" + comboBox2.Text + "', " + numericUpDown13.Value
                    + ", " + numericUpDown14.Value + ", 0, " + numericUpDown15.Value + ")");
            }
            DBLite.dbMu.Close();

            MessageBox.Show("Character has been saved", "zDBManager", MessageBoxButtons.OK);

            if (m_HaveParent)
            {
                Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InformationSave();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                return;
            }
            Character.SkillsEditor SkillsForm = new Character.SkillsEditor(comboBox1.Text, comboBox2.Text);
            SkillsForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Character.Item formInv = new Character.Item();
            formInv.CharName = comboBox2.Text;
            formInv.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {

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

            InformationLoad();
            m_SearchProc = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                return;
            }
            Account.AccountEditor AccountEditorForm = new Account.AccountEditor(comboBox1.Text, comboBox2.Text);
            AccountEditorForm.Show();
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            InformationLoad();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_SearchProc)
            {
                Character_Reload();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            InformationLoad();
        }
    }
}
