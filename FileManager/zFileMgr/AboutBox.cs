using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace zFileMgr
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            lbl_product.Text = fvi.ProductName;
            lbl_ver.Text = fvi.FileVersion;
            lbl_build.Text = "February 13 2015";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://z-team.pro/");
        }

    }
}
