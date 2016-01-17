using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace zFileMgr.NormalShop
{
    class ShopItem
    {


       

        private MuGeneric.MuDef.ItemType m_Category;
        private int m_ItemID;
        private byte m_X;
        private byte m_Y;

        const string cItemsFolderPath = ".\\Data\\items\\";

        public byte MyX
        {
            get { return m_X; }
        }

        public byte MyY
        {
            get { return m_Y; }
        }


        public ShopItem(MuGeneric.MuDef.ItemType cat_id, int item_id, int x, int y)
        {
            m_Category = cat_id;
            m_ItemID = item_id;
            m_X = (byte)x;
            m_Y = (byte)y;

        }

        public Image GetImage()
        {
            Image img = null;

            if (Directory.Exists(cItemsFolderPath))
            {
                img = Image.FromFile(GetImagePath());
            }
            return img;
        }

        private string GetImagePath()
        {
            string default_res = string.Format("{0}{1}.gif", (byte)m_Category, "item_noimg");

            string file_name = string.Format("{0}{1}.gif", (byte)m_Category, m_ItemID);

            bool dir_exists = Directory.Exists(cItemsFolderPath);


            if(!dir_exists)
            {
                Console.WriteLine("items dir not found");
            }

            if(dir_exists && File.Exists(file_name))
            {
                return file_name;
            }


            return default_res;
        }
    }
}
