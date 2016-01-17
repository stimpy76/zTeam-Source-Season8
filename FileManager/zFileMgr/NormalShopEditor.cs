using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using zFileMgr.NormalShop;

namespace zFileMgr
{
    public partial class NormalShopEditor : Form
    {
        private NormalShop.NormalShop m_Obj;

        public NormalShopEditor()
        {
            InitializeComponent();
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
           

            /*
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();

            string path = dialog.FileName;

            m_Obj = new NormalShop.NormalShop();

            m_Obj.LoadShopItems();

            //populate picbox grid
            //8 x 15
            */
           

        }

        private void NormalShopEditor_Load(object sender, EventArgs e)
        {
            m_Obj = new NormalShop.NormalShop(picturebox_inventory);
            picturebox_inventory = m_Obj.DrawImagesOnGrid();
        }
    }
}
