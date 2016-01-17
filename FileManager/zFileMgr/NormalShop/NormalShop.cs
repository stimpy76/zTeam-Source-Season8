using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace zFileMgr.NormalShop
{
    class NormalShop
    {
        public List<ShopItem> GetItems
        {
            get
            {
                return m_ShopItems;
            }
        }

        const string cDataDir = ".\\Data\\";


        private List<ShopItem> m_ShopItems;
        private Graphics m_Graphics;
        private PictureBox m_Grid;

        public NormalShop(PictureBox pb)
        {
            m_ShopItems = new List<ShopItem>();
            
            m_Graphics = Graphics.FromImage(pb.Image);
            m_Grid = pb;

            LoadShopItems();
        }

 

        public PictureBox DrawImagesOnGrid()
        {
            Point cur_pos = new Point(0, 0);
            Brush MyBrush = SystemBrushes.ControlDark;
          
            
            Pen pen = new Pen(MyBrush, 5);

            Image empty_cell = Image.FromFile("Data\\img\\empty_cell.jpg");

            Console.WriteLine("{0} {1}", empty_cell.Size.Height, empty_cell.Size.Width);

            int nItem = 0;
            Image item_img;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 15; y++)
                {
                    float a = 32.0f;
                    float w = (float)cur_pos.X;
                    float h = (float)cur_pos.Y;

                    m_Graphics.DrawImage(empty_cell, w, h, a, a);
                    cur_pos.Y += 32;

                    item_img = m_ShopItems[nItem].GetImage();

                    m_Graphics.DrawImage(item_img, (float)cur_pos.X, (float)cur_pos.Y, (float)item_img.Height, (float)item_img.Width);

                    nItem++;
                }

                cur_pos.X += 32;
                cur_pos.Y = 0;
            }

            m_Graphics.Flush();

            Image img = m_Grid.Image;

             m_Graphics.DrawImage(img, 0, 0);
             m_Grid.Image = img;
             return m_Grid;
        }

        public bool LoadShopItems()
        {
            bool res = false;

           // if (Directory.Exists(cDataDir))
            {
                string[] lines = File.ReadAllLines("Data\\Item.txt");
                int cur_cat = 0;

                string[] tmp;
                ShopItem cur_item;

                for (int i = 0; i < lines.Length; i++)
                {
                    try
                    {
                        tmp = lines[i].Split(new char[] { '\t' });

                        int item_id = int.Parse(tmp[0]);
                        int skill = int.Parse(tmp[1]);
                        int x = int.Parse(tmp[2]);
                        int y = int.Parse(tmp[3]);
                        cur_item = new ShopItem((MuGeneric.MuDef.ItemType)cur_cat, item_id, x, y);
                        m_ShopItems.Add(cur_item);
                    }
                    catch// (Exception ParseEx)
                    {
                       
                        bool get_end_ok = (lines[i] == "end") ? true : false;

                        if (get_end_ok) cur_cat++;
                    }
                }

            }

            return res;

        }
    }
}
