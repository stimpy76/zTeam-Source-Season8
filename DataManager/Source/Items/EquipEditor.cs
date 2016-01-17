using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace zDBManager
{
    public partial class EquipEditor : UserControl
    {
        ArrayList itemTypes = new ArrayList();
        EquipItem editItem = null;

        byte defDurability = 0xFF;

        public EquipItem EditItem
        {
            get
            {
                return editItem;
            }
        }

        public byte DefaultDurability
        {
            get
            {
                return defDurability;
            }
            set
            {
                defDurability = value;
            }
        }

        public EquipEditor()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chkEquipZY1.Checked = true;
            chkEquipZY2.Checked = true;
            chkEquipZY3.Checked = true;
            chkEquipZY4.Checked = true;
            chkEquipZY5.Checked = true;
            chkEquipZY6.Checked = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chkEquipZY1.Checked = false;
            chkEquipZY2.Checked = false;
            chkEquipZY3.Checked = false;
            chkEquipZY4.Checked = false;
            chkEquipZY5.Checked = false;
            chkEquipZY6.Checked = false;
        }    

        protected bool loadItemTypes(IList itemTypes)
        {
            itemTypes.Clear();
            DBLite.mdb.Read("select TypeId, TypeName from MuType order by TypeId");
            while (DBLite.mdb.Fetch())
            {
                itemTypes.Add(new EquipItemType((byte)DBLite.mdb.GetAsInteger("TypeId"), DBLite.mdb.GetAsString("TypeName")));
            }
            DBLite.mdb.Close();
            return true;
        }


        private void cboEquipType_SelectedIndexChanged(object sender, EventArgs e)
        {

            cboEquipName.Items.Clear();
            //foreach (string name in itemType.ItemNames)
            //{
            for (Int32 i = 0; i < EquipItemInfo.g_ItemInfo.m_ItemInfo.Count; i++)
            {
                if (EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Category == (cboEquipType.Items[cboEquipType.SelectedIndex] as EquipItemType).TypeId)
                {
                    Character.ComboboxItem ComboItem = new Character.ComboboxItem();
                    ComboItem.Text = EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Name;
                    ComboItem.Value = EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Number;
                    cboEquipName.Items.Add(ComboItem);
                }
            }
            //}

            comboHarmony.Items.Clear();
            comboHarmony.Items.Add("None");
            comboHarmony.SelectedIndex = 0;

            if(cboEquipType.SelectedIndex >= 0 && cboEquipType.SelectedIndex <= 4)
            {
                comboHarmony.Items.Add("Min Attack Power");
                comboHarmony.Items.Add("Max Attack Power");
                comboHarmony.Items.Add("Strength Requirement");
                comboHarmony.Items.Add("Agility Requirement");
                comboHarmony.Items.Add("Attack (Max,Min)");
                comboHarmony.Items.Add("Critical Damage");
                comboHarmony.Items.Add("Skill Power");
                comboHarmony.Items.Add("Attack % Rate");
                comboHarmony.Items.Add("SD - Rate");
                comboHarmony.Items.Add("SD Ignore Rate");
            }
            else if (cboEquipType.SelectedIndex >= 6 && cboEquipType.SelectedIndex <= 11)
            {
                comboHarmony.Items.Add("Defense Power");
                comboHarmony.Items.Add("Max AG");
                comboHarmony.Items.Add("Max HP");
                comboHarmony.Items.Add("HP Auto Rate");
                comboHarmony.Items.Add("MP Auto Rate");
                comboHarmony.Items.Add("Defense Success Rate");
                comboHarmony.Items.Add("Damage Reduction Rate");
                comboHarmony.Items.Add("SD Rate");
            }
            else if (cboEquipType.SelectedIndex == 5)
            {
                comboHarmony.Items.Add("Magic Power");
                comboHarmony.Items.Add("Strength Requirement");
                comboHarmony.Items.Add("Agility Requirement");
                comboHarmony.Items.Add("Skill Power");
                comboHarmony.Items.Add("Critical Damage");
                comboHarmony.Items.Add("SD - Rate");
                comboHarmony.Items.Add("Attack % Rate");
                comboHarmony.Items.Add("SD Ignore Rate");
            }
        }
        protected bool loadItemNamesByType(IList itemNames, byte typeId)
        {

            itemTypes.Clear();
            DBLite.mdb.Read("select Name from MuItem where Type = " + Convert.ToString(typeId) + " order by UniQue");
            while (DBLite.mdb.Fetch())
            {
                itemNames.Add(DBLite.mdb.GetAsString("Name"));
            }
            DBLite.mdb.Close();
            return true;
        }

        public void GetSocketVal(byte Sock, ref ComboBox combo, ref NumericUpDown numeric)
        {
            byte Lvl = Convert.ToByte(Sock / 50);
            byte Val = Convert.ToByte(Sock - (Lvl * 50));

            if (Lvl == 0)
                Lvl++;
            else if (Lvl != 5)
                Lvl++;

            numeric.Value = Lvl;

            if (Sock == 0xFF)
            {
                combo.SelectedIndex = 28;
                numeric.Value = 1;
                return;
            }
            else if (Sock == 0)
            {
                combo.SelectedIndex = 0;
                numeric.Value = 1;
                return;
            }

            if (Val >= 1 && Val <= 6)
            {
                combo.SelectedIndex = Val;
            }
            else if (Val >= 11 && Val <= 15)
            {
                combo.SelectedIndex = Val - 4;
            }
            else if (Val >= 17 && Val <= 21)
            {
                combo.SelectedIndex = Val - 5;
            }
            else if (Val >= 22 && Val <= 27)
            {
                combo.SelectedIndex = Val - 5;
            }
            else if (Val >= 30 && Val <= 33)
            {
                combo.SelectedIndex = Val - 7;
            }
            else if (Val == 37)
            {
                combo.SelectedIndex = Val - 10;
            }
        }

        public byte GetSocketNum(ComboBox combo, decimal Level)
        {
            byte sock1 = 0;
            if (combo.SelectedItem == null)
            {
                sock1 = 0;
            }
            else
            {
                byte sockVal = Convert.ToByte(combo.SelectedIndex);
                byte mult = Convert.ToByte(Level - 1);
                if (sockVal >= 1 && sockVal <= 6)
                {
                    sock1 = Convert.ToByte(sockVal + (50 * mult));
                }
                else if (sockVal >= 7 && sockVal <= 11)
                {
                    sock1 = Convert.ToByte((sockVal + 4) + (50 * mult));
                }
                else if (sockVal >= 12 && sockVal <= 16)
                {
                    sock1 = Convert.ToByte((sockVal + 5) + (50 * mult));
                }
                else if (sockVal >= 17 && sockVal <= 22)
                {
                    sock1 = Convert.ToByte((sockVal + 5) + (50 * mult));
                }
                else if (sockVal >= 23 && sockVal <= 26)
                {
                    sock1 = Convert.ToByte((sockVal + 7) + (50 * mult));
                }
                else if (sockVal == 27)
                {
                    sock1 = Convert.ToByte((sockVal + 10) + (50 * mult));
                }
                else if (sockVal == 28)
                {
                    sock1 = 255;
                }
            }
            return sock1;
        }

        public void updateUI(EquipItem item)
        {
            numericUpDown1.Value = item.Level;
            cboEquipExt.SelectedIndex = item.Ext;
            chkEquipJN.Checked = item.JN;
            chkEquipXY.Checked = item.XY;
            chkEquipZY1.Checked = item.ZY1;
            chkEquipZY2.Checked = item.ZY2;
            chkEquipZY3.Checked = item.ZY3;
            chkEquipZY4.Checked = item.ZY4;
            chkEquipZY5.Checked = item.ZY5;
            chkEquipZY6.Checked = item.ZY6;
            //chkEquipSet.Checked = item.Set;
            
            int set = item.Set;

            if (set == 0)
                chkEquipSet.Checked = false;
            else
            {
                chkEquipSet.Checked = true;
                txtSet.Text = set.ToString();
            }

            GetSocketVal(item.Socket1, ref comboSock1, ref numericSock1);
            GetSocketVal(item.Socket2, ref comboSock2, ref numericSock2);
            GetSocketVal(item.Socket3, ref comboSock3, ref numericSock3);
            GetSocketVal(item.Socket4, ref comboSock4, ref numericSock4);
            GetSocketVal(item.Socket5, ref comboSock5, ref numericSock5);
            //comboHarmony.SelectedIndex = (item.Harmony & 0xF0) >> 4;
            
            numericUpDown2.Value = item.Durability;
        }

        public void updateData(EquipItem item)
        {
            item.Level = (byte)(numericUpDown1.Value);
            item.Ext = cboEquipExt.SelectedIndex;
            item.JN = chkEquipJN.Checked;
            item.XY = chkEquipXY.Checked;
            item.ZY1 = chkEquipZY1.Checked;
            item.ZY2 = chkEquipZY2.Checked;
            item.ZY3 = chkEquipZY3.Checked;
            item.ZY4 = chkEquipZY4.Checked;
            item.ZY5 = chkEquipZY5.Checked;
            item.ZY6 = chkEquipZY6.Checked;

            if (chkEquipSet.Checked == false)
                item.Set = 0;
            else
            {
                item.Set = Convert.ToByte(txtSet.Text);
            }

            item.Durability = (byte)numericUpDown2.Value;
            item.Socket1 = GetSocketNum(comboSock1, numericSock1.Value);
            item.Socket2 = GetSocketNum(comboSock2, numericSock2.Value);
            item.Socket3 = GetSocketNum(comboSock3, numericSock3.Value);
            item.Socket4 = GetSocketNum(comboSock4, numericSock4.Value);
            item.Socket5 = GetSocketNum(comboSock5, numericSock5.Value);

            item.Harmony = 0;
            if (comboHarmony.SelectedItem != null)
            {
                if (comboHarmony.SelectedIndex > 0)
                {
                    item.Harmony |= Convert.ToByte((comboHarmony.SelectedIndex) << 4);
                    item.Harmony |= Convert.ToByte(item.Level & 0x0F);
                }
            }
        }



        public void LoadData()
        {
            itemTypes.Add(new EquipItemType(0, "Weapons"));
            itemTypes.Add(new EquipItemType(1, "Axes"));
            itemTypes.Add(new EquipItemType(2, "Maces & Scepeters"));
            itemTypes.Add(new EquipItemType(3, "Spears"));
            itemTypes.Add(new EquipItemType(4, "Bows & Crossbows"));
            itemTypes.Add(new EquipItemType(5, "Staffs"));
            itemTypes.Add(new EquipItemType(6, "Shields"));
            itemTypes.Add(new EquipItemType(7, "Helms"));
            itemTypes.Add(new EquipItemType(8, "Armors"));
            itemTypes.Add(new EquipItemType(9, "Pants"));
            itemTypes.Add(new EquipItemType(10, "Gloves"));
            itemTypes.Add(new EquipItemType(11, "Boots"));
            itemTypes.Add(new EquipItemType(12, "Wings & Other"));
            itemTypes.Add(new EquipItemType(13, "Pets & Other"));
            itemTypes.Add(new EquipItemType(14, "Jewels & Other"));
            itemTypes.Add(new EquipItemType(15, "Skills"));

            //loadItemTypes(itemTypes);


            cboEquipType.Items.Clear();
            foreach (EquipItemType itemType in itemTypes)
            {
                cboEquipType.Items.Add(itemType);
            }


            numericUpDown1.Value = 0;
            cboEquipExt.SelectedIndex = 0;
        }

        private void cboEquipName_TextChanged(object sender, EventArgs e)
        {
            string itemName = cboEquipName.Text;
            if (null == itemName || "" == itemName.Trim())
            {
                return;
            }

            EquipItem item = null;

            for (Int32 i = 0; i < EquipItemInfo.g_ItemInfo.m_ItemInfo.Count; i++)
            {
                if (EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Category == (cboEquipType.Items[cboEquipType.SelectedIndex] as EquipItemType).TypeId
                    && EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Number == (cboEquipName.Items[cboEquipName.SelectedIndex] as Character.ComboboxItem).Value)
                {
                    item = new EquipItem(Int32.Parse(EquipItem.getItemCodeType(EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Category, EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Number)), itemName,
                        EquipItemInfo.g_ItemInfo.m_ItemInfo[i].HandFlag, EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Category,
                        EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Width, EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Height, 0);
                    item.Img = EquipItemInfo.g_ItemInfo.m_ItemInfo[i].Photo;
                    break;
                }
            }

            
                
           //     EquipImageCache.Instance.getItem(cboEquipName.Text);
            
            if (null != item)
            {

                if (editItem == null)
                    editItem = new EquipItem(); 
                
                editItem.assign(item);
           }
        }


        private void EquipEditor_Load(object sender, EventArgs e)
        {
            numericUpDown2.Value = defDurability;

            comboSock1.SelectedIndex = 0;
            comboSock2.SelectedIndex = 0;
            comboSock3.SelectedIndex = 0;
            comboSock4.SelectedIndex = 0;
            comboSock5.SelectedIndex = 0;

            comboHarmony.Items.Add("None");
            comboHarmony.SelectedIndex = 0;
        }
    }
}
