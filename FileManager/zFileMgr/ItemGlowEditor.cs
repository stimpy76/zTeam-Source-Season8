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
    public partial class ItemGlowEditor : Form
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

        public ItemGlowEditor(BmdFile File)
        {
            InitializeComponent();
            init(File);
        }

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
        }

        bool swapItemInfo()
        {
            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_ItemGlow ItemPtr;

                if (m_Items[i] != null)
                {
                    ItemPtr = (MuDef.MUFile_ItemGlow)m_Items[i];

                    if (ItemPtr.ItemCode == m_ItemCode)
                    {
                        ushort ItemCode = (ushort)(numericUpDownCategory.Value * 512 + numericUpDownIndex.Value);
                        ItemPtr.ItemCode = ItemCode;
                        ItemPtr.R = panel1.BackColor.R;
                        ItemPtr.G = panel1.BackColor.G;
                        ItemPtr.B = panel1.BackColor.B;
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
                MuDef.MUFile_ItemGlow Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemGlow)m_Items[i];

                    //filter items by category
                    if ((Item.ItemCode / 512) == listBoxCategory.SelectedIndex)
                    {
                        ItemField Field = new ItemField();
                        Field.ItemCode = (ushort)Item.ItemCode;
                        Field.Name = string.Format("Item {0}:{1}", 
                            Item.ItemCode / 512, Item.ItemCode % 512);
                        FieldList.Add(Field);
                    }
                }
            }

            listBoxItems.DataSource = FieldList;
        }

        void fillFormInfo(MuDef.MUFile_ItemGlow Item)
        {
            numericUpDownCategory.Value = Item.ItemCode / 512;
            numericUpDownIndex.Value = Item.ItemCode % 512;
            panel1.BackColor = Color.FromArgb(Item.R, Item.G, Item.B);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ColorDialog Picker = new ColorDialog();

            if (Picker.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = Picker.Color;
            }
        }

        private void listBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateItemList();
        }

        private void listBoxCategory_DataSourceChanged(object sender, EventArgs e)
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
            swapItemInfo();
            m_File.SaveAsBmd(FilePath, m_Items);
            m_IsSaved = true;
            MessageBox.Show(FilePath + " has been saved", "zFileManager");
            Properties.Settings.Default.SaveFolder = Path.GetDirectoryName(FilePath);
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBoxItems.SelectedIndex == -1)
            {
                return;
            }
            //get next select position
            int SelectIndex = listBoxItems.SelectedIndex - 1;

            if (SelectIndex < 1)
            {
                SelectIndex = 0;
            }

            ItemField Field = (ItemField)listBoxItems.Items[listBoxItems.SelectedIndex];

            for (int i = 0; i < m_Items.Length; i++)
            {
                MuDef.MUFile_ItemGlow Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemGlow)m_Items[i];

                    if (Item.ItemCode == Field.ItemCode)
                    {
                        m_Items[i] = null;
                        updateItemList();
                        if (listBoxItems.Items.Count != 0)
                        {
                            listBoxItems.SelectedIndex = SelectIndex; //jump to previus
                        }
                        break;
                    }
                }
            } 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemField Field;
            int ItemIndex;

            if (listBoxItems.Items.Count > 0)
            {
                Field = (ItemField)listBoxItems.Items[listBoxItems.Items.Count - 1];
                ItemIndex = Field.ItemCode + 1;
            }
            else
            {
                ItemIndex = listBoxCategory.SelectedIndex * 512;
            }

            MuDef.MUFile_ItemGlow Item = new MuDef.MUFile_ItemGlow();

            Item.ItemCode = (ushort)ItemIndex;
            Item.R = 0;
            Item.G = 0;
            Item.B = 0;

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
                MuDef.MUFile_ItemGlow Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemGlow)m_Items[i];

                    if (Item.ItemCode == Field.ItemCode)
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
                MuDef.MUFile_ItemGlow ItemInfo;

                if (m_Items[i] != null)
                {
                    ItemInfo = (MuDef.MUFile_ItemGlow)m_Items[i];

                    if (ItemInfo.ItemCode == m_CopyboardIndex)
                    {
                        ItemInfo.ItemCode = Field.ItemCode;
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
                MuDef.MUFile_ItemGlow Item;

                if (m_Items[i] != null)
                {
                    Item = (MuDef.MUFile_ItemGlow)m_Items[i];

                    if (Item.ItemCode == Field.ItemCode)
                    {
                        fillFormInfo(Item);
                        m_ItemCode = Item.ItemCode;
                        break;
                    }
                }
            }
        }
    }
}
