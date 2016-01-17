using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using zFileMgr.MuBMD;
using zFileMgr;



namespace zFileMgr
{
    public partial class Main : Form
    {
        public static Main Instance;
        static object m_LogLock = new object();
        string m_OpenFileFolder;

        public static void Log(string Text, params object[] args)
        {
            lock (m_LogLock)
            {
                /*
                Main.Instance.textBox1.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                Main.Instance.textBox1.AppendText(string.Format(Text, args) + "\r\n");
                Main.Instance.textBox1.SelectionStart = Main.Instance.textBox1.Text.Length;
                Main.Instance.textBox1.ScrollToCaret();
                 * */
            }
        }

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Main.Instance = this;


            string tmp = Properties.Settings.Default.OpenFolder;
           
            m_OpenFileFolder = tmp;
   
        }


        private string OpenDialog()
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.InitialDirectory = m_OpenFileFolder;
            
            Dialog.Filter = "MU Config File (*.bmd)|*.bmd";
            // ----
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                m_OpenFileFolder = Path.GetDirectoryName(Dialog.FileName);
                Properties.Settings.Default.OpenFolder = m_OpenFileFolder;
                Properties.Settings.Default.Save();
                return Dialog.FileName;
            }
            // ----
            return null;
        }

        private string OpenDialogZ()
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.InitialDirectory = m_OpenFileFolder;

            Dialog.Filter = "ZT Config File (*.z)|*.z";
            // ----
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                m_OpenFileFolder = Path.GetDirectoryName(Dialog.FileName);
                Properties.Settings.Default.OpenFolder = m_OpenFileFolder;
                Properties.Settings.Default.Save();
                return Dialog.FileName;
            }
            // ----
            return null;
        }
        // ----


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void filterbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            GenericFilter Decoder = new GenericFilter(fpath, BmdFile.FileType.Filter);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void buffEffectbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            BuffEffect Decoder = new BuffEffect(fpath, BmdFile.FileType.BuffEffect);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void filterNamebmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            GenericFilter Decoder = new GenericFilter(fpath, BmdFile.FileType.Filter);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        //todo: implement
        private void gatebmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Gate Decoder = new Gate(fpath, BmdFile.FileType.Gate);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
          

        }

        private void textbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Text Decoder = new Text(fpath, BmdFile.FileType.Text);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();

            /*
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;
            new ClientEditor(fpath,(int)MuFile.FileType.Text);
             * */
        }

        private void creditbmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Credits Decoder = new Credits(fpath, BmdFile.FileType.Credit);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
           
        }

        private void MinimaptoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Minimap Decoder = new Minimap(fpath, BmdFile.FileType.Minimap);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
           
        }

        private void movereqbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            MoveReq Decoder = new MoveReq(fpath, BmdFile.FileType.MoveReq);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void jewelOfHarmonyOptionbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            JewelOfHarmonyOption Decoder = new JewelOfHarmonyOption(fpath, BmdFile.FileType.JewelOfHarmonyOption);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void serverListbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ServerList Decoder = new ServerList(fpath, BmdFile.FileType.ServerList);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void socketItembmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;


            SocketItem Decoder = new SocketItem(fpath, BmdFile.FileType.SocketItem);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void nPCNamebmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;


            NPCName Decoder = new NPCName(fpath, BmdFile.FileType.NPCName);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void infoToolstripTextbmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            InfoTooltipText Decoder = new InfoTooltipText(fpath, BmdFile.FileType.InfoTooltipText);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void masterSkillTooltipbmdToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            MasterSkillTooltip Decoder = new MasterSkillTooltip(fpath, BmdFile.FileType.MasterSkillTooltip);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void itemTextTooltipbmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemTooltipText Decoder = new ItemTooltipText(fpath, BmdFile.FileType.ItemTooltipText);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void skillbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Skill Decoder = new Skill(fpath, BmdFile.FileType.Skill);
            ClientEditor editor = new ClientEditor(Decoder, false);
            editor.Show();
        }

        private void itembmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Item Decoder = new Item(fpath, BmdFile.FileType.Item);
            //ClientEditor editor = new ClientEditor(Decoder, true);
            ItemEditor editor = new ItemEditor(Decoder);
            editor.Show();
        }

        private void editorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainform frm = new mainform();
            frm.Show();
        }

        private void nPCDialoguebmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            NPCDialog Decoder = new NPCDialog(fpath, BmdFile.FileType.NPCDialogue);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void slidebmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            Slide Decoder = new Slide(fpath, BmdFile.FileType.Slide);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void masterSkillTreeDatabmdToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            MasterSkillTreeData Decoder = new MasterSkillTreeData(fpath, BmdFile.FileType.MasterSkillTreeData);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void monsterSkillbmdToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            MonsterSkill Decoder = new MonsterSkill(fpath, BmdFile.FileType.MonsterSkill);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void itemLevelTooltipbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemLevelTooltip Decoder = new ItemLevelTooltip(fpath, BmdFile.FileType.ItemLevelTooltip);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void itemAddOptionbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemAddOption Decoder = new ItemAddOption(fpath, BmdFile.FileType.ItemAddOption);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
            
        }

        private void questProgressbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            QuestProgress Decoder = new QuestProgress(fpath, BmdFile.FileType.QuestProgress);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void questWordsbmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            QuestWords Decoder = new QuestWords(fpath, BmdFile.FileType.QuestWords);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void petDatabmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            PetData Decoder = new PetData(fpath, BmdFile.FileType.PetData);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void helpDatabmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            HelpData Decoder = new HelpData(fpath, BmdFile.FileType.HelpData);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void shopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NormalShopEditor frm = new NormalShopEditor();
            frm.Show();
        }

        private void itemSetTypebmdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemSetType Decoder = new ItemSetType(fpath, BmdFile.FileType.ItemSetType);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void itemTRSDatabmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemTRSData Decoder = new ItemTRSData(fpath, BmdFile.FileType.ItemTRSData);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void itemSetOptionTextbmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemSetOptionText Decoder = new ItemSetOptionText(fpath, BmdFile.FileType.ItemSetOptionText);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void itemSetOptionnmdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialog();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemSetOption Decoder = new ItemSetOption(fpath, BmdFile.FileType.ItemSetOption);
            ClientEditor editor = new ClientEditor(Decoder, false);

            editor.Show();
        }

        private void itemModelzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialogZ();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemGlow Decoder = new ItemGlow(fpath, BmdFile.FileType.ItemGlow);
            //ClientEditor editor = new ClientEditor(Decoder, true);
            ItemGlowEditor editor = new ItemGlowEditor(Decoder);
            editor.Show();
        }

        private void commonzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialogZ();
            if (string.IsNullOrEmpty(fpath))
                return;

            CommonEditor editor = new CommonEditor(fpath);

            editor.Show();
        }

        private void itemExcellentOptionzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fpath = OpenDialogZ();
            if (string.IsNullOrEmpty(fpath))
                return;

            ItemExcellentOption Decoder = new ItemExcellentOption(fpath, BmdFile.FileType.ItemGlow);
            ItemExcellentOptionEditor editor = new ItemExcellentOptionEditor(Decoder);
            editor.Show();
        }
    }
}
