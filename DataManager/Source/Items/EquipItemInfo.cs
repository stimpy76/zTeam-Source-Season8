using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace zDBManager
{
    class EquipItemInfo
    {
        public static EquipItemInfo g_ItemInfo;
        public List<ItemInfo> m_ItemInfo;

        public EquipItemInfo()
        {
            m_ItemInfo = new List<ItemInfo>();
        }

        public void Load()
        {
            m_ItemInfo = new List<ItemInfo>();

            string[] lines = System.IO.File.ReadAllLines("Data\\Item.txt");
            byte Category = 0;
            string[] NumberBuffer;

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    NumberBuffer = lines[i].Split(null);

                    ItemInfo NewItem = new ItemInfo();

                    NewItem.Category = Category;
                    NewItem.Number = short.Parse(NumberBuffer[0]);
                    NewItem.Slot = byte.Parse(NumberBuffer[1]);
                    NewItem.Width = byte.Parse(NumberBuffer[3]);
                    NewItem.Height = byte.Parse(NumberBuffer[4]);
                    NewItem.HandFlag = 0;

                    if (NewItem.Width >= 2 && Category <= 5)
                    {
                        NewItem.HandFlag = 1;
                    }

                    NewItem.SerialFlag = byte.Parse(NumberBuffer[5]);

                    var reg = new System.Text.RegularExpressions.Regex("\".*?\"");
                    var matches = reg.Matches(lines[i]);

                    NewItem.Name = matches[0].ToString().Replace("\"", "");

                    LogWindow.SqlLog.LogAdd("Item -> (" + Category + ":" + NewItem.Number + ") " + NewItem.Name);

                    //missed image

                    string Path = "Data\\Images\\Items\\" + NewItem.Category + "-" + NewItem.Number + ".gif";

                    if (System.IO.File.Exists(Path))
                    {
                        NewItem.Photo = Image.FromFile("Data\\Images\\Items\\" + NewItem.Category + "-" + NewItem.Number + ".gif");
                    }
                    else
                    {
                        NewItem.Photo = Image.FromFile("Data\\Images\\Skills\\Unknown.jpg");
                    }

                    m_ItemInfo.Add(NewItem);
                }
                catch// (Exception ParseEx)
                {

                    bool get_end_ok = (lines[i] == "end") ? true : false;

                    if (get_end_ok)
                    {
                        Category++;
                    }

                    if (Category > 15)
                    {
                        break;
                    }
                }
            }
        }
    }

    public struct ItemInfo
    {
        public string Name;
        public byte Category;
        public short Number;
        public byte Slot;
        public byte Width;
        public byte Height;
        public byte HandFlag;
        public byte SerialFlag;
        public byte CanBeUsedByWizard;
        public byte CanBeUsedByKnight;
        public byte CanBeUsedByElf;
        public byte CanBeUsedByGladiator;
        public byte CanBeUsedByLord;
        public byte CanBeUsedBySummoner;
        public byte CanBeUsedByFighter;
        public Image Photo;
    }
}
