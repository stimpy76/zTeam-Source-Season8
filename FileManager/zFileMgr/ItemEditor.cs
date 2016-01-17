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

namespace zFileMgr
{
    public partial class ItemEditor : Form
    {
        #region Private members
        bool m_IsSaved;
        bool m_NeedSaving;
        bool m_NeedRefresh;
        string m_SaveFolderPath;
        BmdFile m_File;
        object[] m_Items;
        int m_ItemCode;
        int m_CopyboardIndex;
        #endregion 

        public ItemEditor(BmdFile File)
        {
            InitializeComponent();
            init(File);
        }

        #region Common
        void init(BmdFile File)
        {
            m_File = File;
            m_Items = m_File.GetStructures();
            m_SaveFolderPath = Properties.Settings.Default.SaveFolder;
            m_IsSaved = false;
            m_NeedSaving = false;
            m_CopyboardIndex = -1;
            button4.Enabled = false;
            m_ItemCode = -1;
            Properties.Settings.Default.Save();
            fillCombo();
        }

        void fillCombo()
        {
            comboBoxSlot.DataSource = null;
            object[] ItemSlots = new object[] { 
                new ItemField(255, "Inventory"),
                new ItemField(0, "Left hand"),
                new ItemField(1, "Right hand"),
                new ItemField(2, "Helm"),
                new ItemField(3, "Armor"),
                new ItemField(4, "Pants"),
                new ItemField(5, "Gloves"),
                new ItemField(6, "Boots"),
                new ItemField(7, "Wings"),
                new ItemField(8, "Pet"),
                new ItemField(9, "Pendant"),
                new ItemField(10, "Ring")
            };
            comboBoxSlot.DataSource = ItemSlots;

            comboBoxReqClass0.DataSource = null;
            object[] Class0Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Dark Wizard"),
                new ItemField(2, "Soul Master"),
                new ItemField(3, "Grand Master"),
            };
            comboBoxReqClass0.DataSource = Class0Profs;

            comboBoxReqClass1.DataSource = null;
            object[] Class1Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Dark Knight"),
                new ItemField(2, "Blade Knight"),
                new ItemField(3, "Blade Master"),
            };
            comboBoxReqClass1.DataSource = Class1Profs;

            comboBoxReqClass2.DataSource = null;
            object[] Class2Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Elf"),
                new ItemField(2, "Muse Elf"),
                new ItemField(3, "High Elf"),
            };
            comboBoxReqClass2.DataSource = Class2Profs;

            comboBoxReqClass3.DataSource = null;
            object[] Class3Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Magic Gladiator"),
                new ItemField(3, "Duel Master"),
            };
            comboBoxReqClass3.DataSource = Class3Profs;

            comboBoxReqClass4.DataSource = null;
            object[] Class4Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Dark Lord"),
                new ItemField(3, "Lord Emperor"),
            };
            comboBoxReqClass4.DataSource = Class4Profs;

            comboBoxReqClass5.DataSource = null;
            object[] Class5Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Summoner"),
                new ItemField(2, "Bloody Summoner"),
                new ItemField(3, "Dimension Master"),
            };
            comboBoxReqClass5.DataSource = Class5Profs;

            comboBoxReqClass6.DataSource = null;
            object[] Class6Profs = new object[] {
                new ItemField(0, "None"),
                new ItemField(1, "Rage Fighter"),
                new ItemField(3, "First Master"),
            };
            comboBoxReqClass6.DataSource = Class6Profs;
        }

        bool swapItemInfo()
        {
            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_Item ItemPtr;

                if (m_Items[i] != null)
                {
                    ItemPtr = (MuDef.MUFile_Item)m_Items[i];

                    if (ItemPtr.ItemID == m_ItemCode)
                    {
                        /*ushort ItemCode = (ushort)(numericUpDownCategory.Value * 512 + numericUpDownIndex.Value);
   
                        //item id has been changed
                        if (ItemPtr.ItemID != ItemCode)
                        {
                            foreach (object ItemObjIt in m_Items)
                            {
                                if (ItemObjIt == null)
                                {
                                    continue;
                                }

                                MuDef.MUFile_Item ItemIt = (MuDef.MUFile_Item)ItemObjIt;

                                if (ItemIt.ItemID == ItemCode)
                                {
                                    MessageBox.Show("Error to save item, reason:\n"
                                        + "Item index already in use", "zFileManager");
                                    return false;
                                }
                            }
                        }

                        ItemPtr.ItemID = ItemCode;*/
                        ItemPtr.ItemInfo.ItemLvl = (ushort)numericUpDownLevel.Value;
                        ItemPtr.ItemInfo.Name = textBoxName.Text;
                        ItemPtr.TexturePath = textBoxTexturePath.Text;
                        ItemPtr.ModelName = textBoxModelFile.Text;
                        ItemPtr.ItemInfo.ItemLvl = (ushort)numericUpDownLevel.Value;
                        ItemField FieldSlot = (ItemField)comboBoxSlot.Items[comboBoxSlot.SelectedIndex];
                        ItemPtr.ItemInfo.ItemSlot = FieldSlot.ItemCode;
                        ItemPtr.ItemInfo.ItemSkill = (ushort)numericUpDownSkill.Value;
                        ItemPtr.ItemInfo.X = (byte)numericUpDownWidth.Value;
                        ItemPtr.ItemInfo.Y = (byte)numericUpDownHeight.Value;
                        ItemPtr.ItemInfo.ReqLvl = (ushort)numericUpDownReqLevel.Value;
                        ItemPtr.ItemInfo.ReqStr = (ushort)numericUpDownReqStr.Value;
                        ItemPtr.ItemInfo.ReqDex = (ushort)numericUpDownReqDex.Value;
                        ItemPtr.ItemInfo.ReqVit = (ushort)numericUpDownReqVit.Value;
                        ItemPtr.ItemInfo.ReqEne = (ushort)numericUpDownReqEne.Value;
                        ItemPtr.ItemInfo.ReqLea = (ushort)numericUpDownReqCom.Value;
                        ItemField FieldClass0 = (ItemField)comboBoxReqClass0.Items[comboBoxReqClass0.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[0] = (byte)FieldClass0.ItemCode;
                        ItemField FieldClass1 = (ItemField)comboBoxReqClass1.Items[comboBoxReqClass1.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[1] = (byte)FieldClass1.ItemCode;
                        ItemField FieldClass2 = (ItemField)comboBoxReqClass2.Items[comboBoxReqClass2.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[2] = (byte)FieldClass2.ItemCode;
                        ItemField FieldClass3 = (ItemField)comboBoxReqClass3.Items[comboBoxReqClass3.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[3] = (byte)FieldClass3.ItemCode;
                        ItemField FieldClass4 = (ItemField)comboBoxReqClass4.Items[comboBoxReqClass4.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[4] = (byte)FieldClass4.ItemCode;
                        ItemField FieldClass5 = (ItemField)comboBoxReqClass5.Items[comboBoxReqClass5.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[5] = (byte)FieldClass5.ItemCode;
                        ItemField FieldClass6 = (ItemField)comboBoxReqClass6.Items[comboBoxReqClass6.SelectedIndex];
                        ItemPtr.ItemInfo.Classes[6] = (byte)FieldClass6.ItemCode;
                        ItemPtr.ItemInfo.ReqStr = (ushort)numericUpDownReqStr.Value;
                        ItemPtr.ItemInfo.ReqDex = (ushort)numericUpDownReqDex.Value;
                        ItemPtr.ItemInfo.ReqVit = (ushort)numericUpDownReqVit.Value;
                        ItemPtr.ItemInfo.ReqEne = (ushort)numericUpDownReqEne.Value;
                        ItemPtr.ItemInfo.ReqLea = (ushort)numericUpDownReqCom.Value;
                        ItemPtr.ItemInfo.Durability = (byte)numericUpDownDurability.Value;
                        ItemPtr.ItemInfo.DmgMin = (ushort)numericUpDownDamageMin.Value;
                        ItemPtr.ItemInfo.DmgMax = (ushort)numericUpDownDamageMax.Value;
                        ItemPtr.ItemInfo.MagicDur = (byte)numericUpDownMagicDur.Value;
                        ItemPtr.ItemInfo.MagicPwr = (byte)numericUpDownMagicPower.Value;
                        ItemPtr.ItemInfo.Defence = (ushort)numericUpDownDefense.Value;
                        ItemPtr.ItemInfo.DefRate = (ushort)numericUpDownDefenseRate.Value;
                        ItemPtr.ItemInfo.AtkSpeed = (byte)numericUpDownAttackSpeed.Value;
                        ItemPtr.ItemInfo.WalkSpeed = (byte)numericUpDownWalkSpeed.Value;
                        ItemPtr.ItemInfo.Zen = (int)numericUpDownMoneyFact.Value;
                        ItemPtr.ItemInfo.Resistances[0] = (byte)numericUpDownResIce.Value;
                        ItemPtr.ItemInfo.Resistances[1] = (byte)numericUpDownResPoision.Value;
                        ItemPtr.ItemInfo.Resistances[2] = (byte)numericUpDownResLighting.Value;
                        ItemPtr.ItemInfo.Resistances[3] = (byte)numericUpDownResFire.Value;
                        ItemPtr.ItemInfo.Resistances[4] = (byte)numericUpDownResEarth.Value;
                        ItemPtr.ItemInfo.Resistances[5] = (byte)numericUpDownResWind.Value;
                        ItemPtr.ItemInfo.Resistances[6] = (byte)numericUpDownResWater.Value;
                        ItemPtr.ItemInfo.SetOption = (byte)numericUpDown22.Value;
                        ItemPtr.ItemInfo.IsApplyToDrop = (byte)comboBoxAllowDrop.SelectedIndex;
                        ItemPtr.ItemInfo.IsExpensive = (byte)comboBoxIsExpensive.SelectedIndex;
                        ItemPtr.ItemInfo.StackMax = (byte)numericUpDownMaxStack.Value;
                        ItemPtr.ItemInfo.ItemValue = (ushort)numericUpDown1.Value;
                        m_Items[i] = ItemPtr;
                        break;
                    }
                }
            }

            return true;
        }

        void updateItemList()
        {
            listBoxItems.DataSource = null;
            List<ItemField> FieldList = new List<ItemField>();

            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_Item Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_Item)m_Items[i];
                    
                    //filter items by category
                    if ((Item.ItemID / 512) == listBoxCategory.SelectedIndex)
                    {
                        ItemField Field = new ItemField();
                        Field.ItemCode = (ushort)Item.ItemID;
                        Field.Name = Item.ItemInfo.Name;
                        FieldList.Add(Field);
                    }
                }
            }

            listBoxItems.DataSource = FieldList;
        }

        void fillFormInfo(MuDef.MUFile_Item Item)
        {
            textBoxName.Text = Item.ItemInfo.Name;
            numericUpDownCategory.Value = Item.ItemID / 512;
            numericUpDownIndex.Value = Item.ItemID % 512;
            textBoxTexturePath.Text = Item.TexturePath;
            textBoxModelFile.Text = Item.ModelName;
            checkBoxTwoHands.Checked = Convert.ToBoolean(Item.ItemInfo.TwoHands);

            for (int i = 0; i < comboBoxSlot.Items.Count; i++)
            {
                ItemField Field = (ItemField)comboBoxSlot.Items[i];
                if (Field.ItemCode == Item.ItemInfo.ItemSlot)
                {
                    comboBoxSlot.SelectedIndex = i;
                    break;
                } 
            }

            numericUpDownSkill.Value = Item.ItemInfo.ItemSkill;
            numericUpDownWidth.Value = Item.ItemInfo.X;
            numericUpDownHeight.Value = Item.ItemInfo.Y;
            numericUpDownLevel.Value = Item.ItemInfo.ItemLvl;
            numericUpDownReqLevel.Value = Item.ItemInfo.ReqLvl;
            numericUpDownReqStr.Value = Item.ItemInfo.ReqStr;
            numericUpDownReqDex.Value = Item.ItemInfo.ReqDex;
            numericUpDownReqVit.Value = Item.ItemInfo.ReqVit;
            numericUpDownReqEne.Value = Item.ItemInfo.ReqEne;
            numericUpDownReqCom.Value = Item.ItemInfo.ReqLea;

            for (int i = 0; i < comboBoxReqClass0.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass0.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[0])
                {
                    comboBoxReqClass0.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxReqClass1.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass1.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[1])
                {
                    comboBoxReqClass1.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxReqClass2.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass2.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[2])
                {
                    comboBoxReqClass2.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxReqClass3.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass3.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[3])
                {
                    comboBoxReqClass3.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxReqClass4.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass4.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[4])
                {
                    comboBoxReqClass4.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxReqClass5.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass5.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[5])
                {
                    comboBoxReqClass5.SelectedIndex = i;
                    break;
                }
            }

            for (int i = 0; i < comboBoxReqClass6.Items.Count; i++)
            {
                ItemField It = (ItemField)comboBoxReqClass6.Items[i];
                if (It.ItemCode == Item.ItemInfo.Classes[6])
                {
                    comboBoxReqClass6.SelectedIndex = i;
                    break;
                }
            }

            numericUpDownDurability.Value = Item.ItemInfo.Durability;
            numericUpDownDamageMin.Value = Item.ItemInfo.DmgMin;
            numericUpDownDamageMax.Value = Item.ItemInfo.DmgMax;
            numericUpDownMagicDur.Value = Item.ItemInfo.MagicDur;
            numericUpDownMagicPower.Value = Item.ItemInfo.MagicPwr;
            numericUpDownDefense.Value = Item.ItemInfo.Defence;
            numericUpDownDefenseRate.Value = Item.ItemInfo.DefRate;
            numericUpDownAttackSpeed.Value = Item.ItemInfo.AtkSpeed;
            numericUpDownWalkSpeed.Value = Item.ItemInfo.WalkSpeed;
            numericUpDownMoneyFact.Value = Item.ItemInfo.Zen;
            
            numericUpDownResIce.Value = Item.ItemInfo.Resistances[0];
            numericUpDownResPoision.Value = Item.ItemInfo.Resistances[1];
            numericUpDownResLighting.Value = Item.ItemInfo.Resistances[2];
            numericUpDownResFire.Value = Item.ItemInfo.Resistances[3];
            numericUpDownResEarth.Value = Item.ItemInfo.Resistances[4];
            numericUpDownResWind.Value = Item.ItemInfo.Resistances[5];
            numericUpDownResWater.Value = Item.ItemInfo.Resistances[6];

            numericUpDown22.Value = Item.ItemInfo.SetOption;
            numericUpDownMaxStack.Value = Item.ItemInfo.StackMax;
            numericUpDown1.Value = Item.ItemInfo.ItemValue;
            comboBoxAllowDrop.SelectedIndex = Item.ItemInfo.IsApplyToDrop;
            comboBoxIsExpensive.SelectedIndex = Item.ItemInfo.IsExpensive;
        }
        #endregion

        #region Events
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateItemList();
        }

        private void listBox1_DataSourceChanged(object sender, EventArgs e)
        {
            ListBox ctlLIST = (ListBox)sender;
            if (ctlLIST.DataSource == null)
            {
                ctlLIST.Items.Clear();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FilePath;
            SaveFileDialog Dialog = new SaveFileDialog();
            Dialog.InitialDirectory = m_SaveFolderPath;
            Dialog.Filter = "MU Config File (*.bmd)|*.bmd";

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
            swapItemInfo();
            m_File.SaveAsBmd(FilePath, m_Items);
            m_IsSaved = true;
            MessageBox.Show(FilePath + " has been saved", "zFileManager");
            Properties.Settings.Default.SaveFolder = Path.GetDirectoryName(FilePath);
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //get next select position
            int SelectIndex = listBoxItems.SelectedIndex - 1;

            if (SelectIndex < 1)
            {
                SelectIndex = 0;
            }

            ItemField Field = (ItemField)listBoxItems.Items[listBoxItems.SelectedIndex];

            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_Item Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_Item)m_Items[i];

                    if (Item.ItemID == Field.ItemCode)
                    {
                        m_Items[i] = null;
                        updateItemList();
                        listBoxItems.SelectedIndex = SelectIndex; //jump to previus
                        break;
                    }
                }
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemField Field = (ItemField)listBoxItems.Items[listBoxItems.Items.Count - 1];
            int ItemIndex = Field.ItemCode + 1;

            MuDef.MUFile_Item Item = new MuDef.MUFile_Item();
            Item.ItemInfo = new MuDef.MUFile_ItemInfo()
            {
                Classes = new byte[7],
                Resistances = new byte[7],
            };

            Item.ItemID = ItemIndex;
            Item.ItemInfo.Name = "New item " + (Item.ItemID % 512);
            Item.TexturePath = "Data\\Item\\";
            Item.ModelName = "Item" + Item.ItemID + ".bmd";

            //swap items info
            object[] NewItems = new object[m_Items.Length + 1];
            m_Items.CopyTo(NewItems, 0);
            NewItems[m_Items.Length] = Item;
            m_Items = NewItems;
                 
            updateItemList();
            listBoxItems.SelectedIndex = listBoxItems.Items.Count - 1; //jump to new item
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ItemField Field = (ItemField)listBoxItems.Items[listBoxItems.SelectedIndex];
            int DestineSelect = listBoxItems.SelectedIndex;
            int DestinyIndex = -1;

            //get destiny array index
            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_Item Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_Item)m_Items[i];

                    if (Item.ItemID == Field.ItemCode)
                    {
                        DestinyIndex = i;
                        break;
                    }
                }
            }
            
            if (DestinyIndex == -1)
            {
                return;
            }

            //swap item info
            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_Item ItemInfo;

                if (m_Items[i] != null)
                {
                    ItemInfo = (MuDef.MUFile_Item)m_Items[i];

                    if (ItemInfo.ItemID == m_CopyboardIndex)
                    {
                        ItemInfo.ItemID = Field.ItemCode;
                        m_Items[DestinyIndex] = ItemInfo;
                        fillFormInfo(ItemInfo);
                        break;
                    }
                }
            }

            updateItemList();
            listBoxItems.SelectedIndex = DestineSelect;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemField Field = (ItemField)listBoxItems.Items[listBoxItems.SelectedIndex];
            m_CopyboardIndex = Field.ItemCode;
            button4.Enabled = true;
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedIndex == -1)
            {
                return;
            }

            //auto save changes
            if (!swapItemInfo())
            {
                return;
            }

            ItemField Field = (ItemField)listBoxItems.Items[listBoxItems.SelectedIndex];

            //form refresh
            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_Item Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_Item)m_Items[i];

                    if (Item.ItemID == Field.ItemCode)
                    {
                        fillFormInfo(Item);
                        m_ItemCode = Item.ItemID;
                        //pictureBox1.BackgroundImage = Image.FromFile("Data\\Images\\Items\\" + 1 + ".png");
                        break;
                    }
                }
            }
        }
        #endregion 

    }

    #region Helpers
    public class ItemField
    {
        private ushort _ItemCode;
        private string _Name;

        public ushort ItemCode
        {
            get { return _ItemCode; }
            set { this._ItemCode = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { this._Name = value; }
        }

        public ItemField()
        {

        }

        public ItemField(ushort ItemCode, string Name)
        {
            _ItemCode = ItemCode;
            _Name = Name;
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    };
    #endregion
}
