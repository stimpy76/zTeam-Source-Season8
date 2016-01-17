using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using zFileMgr.MuGeneric;
using zFileMgr.CashShop;

namespace zFileMgr
{
    public partial class mainform : Form
    {
        private CashShopClass m_Worker;
        private List<Button> m_Subcategories = new List<Button>();

        ContextMenu ItemOptionSel;

        public mainform()
        {
            InitializeComponent();
            groupbox_container.BackgroundImage = Properties.Resources.Background;
            groupbox_container.BackgroundImageLayout = ImageLayout.None;

            //assign click handlers etc
            ItemOptionSel = new ContextMenu();

            ItemOptionSel.MenuItems.Add("Edit");
            ItemOptionSel.MenuItems.Add("Remove");

            itemimage_1.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_2.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_3.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_4.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_5.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_6.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_7.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_8.MouseClick += new MouseEventHandler(OnItemClick);
            itemimage_9.MouseClick += new MouseEventHandler(OnItemClick);
           
        }

        void OnItemClick(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ItemOptionSel.Show((Control)sender, e.Location, LeftRightAlignment.Right);
            }
        }


        private void CashShopEditor_Load(object sender, EventArgs e)
        {
            m_Worker = new CashShopClass();
            m_Worker.LoadCategories();

            DrawCategories();
        }


       

        void DrawCategories()
        {
            for (int i = 0; i < groupbox_container.Controls.Count; i++)
            {
                if (groupbox_container.Controls[i].GetType() != typeof(PictureBox))
                {
                    groupbox_container.Controls.Remove(groupbox_container.Controls[i]);
                }
    
            }

           // groupbox_container.Controls.Clear();

            for (int i = 0; i < m_Worker.Categories.Count; i++)
            {
                Button btn = new Button();
                btn.Text = m_Worker.Categories[i].Name;
                btn.Location = new Point(100 * (i + 1), 15);
                btn.BackColor = Color.Black;
                btn.ForeColor = Color.White;
                btn.BackgroundImage = Properties.Resources.Category_Head_State1;
                
                btn.Size = new Size(75, 22);
                //hazx
                btn.Name = i.ToString();

                btn.Click += new EventHandler(OnSupercatClicked);

                groupbox_container.Controls.Add(btn);

              
            }
        }

        void OnSupercatClicked(object sender, EventArgs e)
        {
            DrawCategories();
           
 

 
            //MessageBox.Show(groupbox_container.Controls.Count.ToString());

            Button _sender = sender as Button;

            //draw subcats

            List<CashShopCategory> sub = m_Worker.Categories[int.Parse(_sender.Name)].SubCategories;
            for (int i = 0; i < sub.Count; i++)
            {
                Button btn = new Button();
                btn.Text = sub[i].Name;
                btn.Location = new Point(5, 25 * (i + 1));
                btn.BackColor = Color.Black;
                btn.ForeColor = Color.White;
                btn.Size = new Size(68, 22);
                btn.BackgroundImage = Properties.Resources.Category_Sub_State1;
                //hazx
                btn.Name = i.ToString();
                btn.Tag = "subcat_btn";
                btn.Click += new EventHandler(OnSubcatClick);
                groupbox_container.Controls.Add(btn);
                m_Subcategories.Add(btn);
            }
           
        }

        void OnSubcatClick(object sender, EventArgs e)
        {
            
        }


       

   

      
    }
}
