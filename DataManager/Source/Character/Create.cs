using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace zDBManager.Character
{
    partial class Create : Form
    {
        private string AccountName;

        public Create(string Account)
        {
            InitializeComponent();
            AccountName = Account;
            ComboboxItem ComboItem1 = new ComboboxItem();
            ComboItem1.Text = "Dark Wizard";
            ComboItem1.Value = 0;
            comboBox1.Items.Add(ComboItem1);
            ComboboxItem ComboItem2 = new ComboboxItem();
            ComboItem2.Text = "Dark Knight";
            ComboItem2.Value = 16;
            comboBox1.Items.Add(ComboItem2);
            ComboboxItem ComboItem3 = new ComboboxItem();
            ComboItem3.Text = "Fairy Elf";
            ComboItem3.Value = 32;
            comboBox1.Items.Add(ComboItem3);
            ComboboxItem ComboItem4 = new ComboboxItem();
            ComboItem4.Text = "Magic Gladiator";
            ComboItem4.Value = 48;
            comboBox1.Items.Add(ComboItem4);
            ComboboxItem ComboItem5 = new ComboboxItem();
            ComboItem5.Text = "Dark Lord";
            ComboItem5.Value = 64;
            comboBox1.Items.Add(ComboItem5);
            ComboboxItem ComboItem6 = new ComboboxItem();
            ComboItem6.Text = "Summoner";
            ComboItem6.Value = 80;
            comboBox1.Items.Add(ComboItem6);
            ComboboxItem ComboItem7 = new ComboboxItem();
            ComboItem7.Text = "Rage Fighter";
            ComboItem7.Value = 96;
            comboBox1.Items.Add(ComboItem7);
            comboBox1.SelectedIndex = 0;
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 4)
            {
                MessageBox.Show("Wrongs name, length < 4", "zDBManager", MessageBoxButtons.OK);
                return;
            }

            DBLite.dbMu.Exec("WZ_CreateCharacter '" + AccountName + "','" + textBox1.Text + "'," + (comboBox1.SelectedItem as ComboboxItem).Value);
            DBLite.dbMu.Close();

            MessageBox.Show("Character " + textBox1.Text + " has been added", "zDBManager", MessageBoxButtons.OK);
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
