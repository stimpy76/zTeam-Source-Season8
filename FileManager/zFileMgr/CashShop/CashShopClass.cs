using System;
using System.Collections.Generic;
using System.IO;


namespace zFileMgr.CashShop
{
    

    class CashShopClass
    {
        public List<CashShopCategory> Categories;


        public CashShopClass()
        {
            Categories = new List<CashShopCategory>();
        }

        public bool LoadCategories()
        {
            bool res = false;

            string FileName = ".\\IBSCategory.txt";

            if (!File.Exists(FileName))
            {
                return res;
            }

            string[] Lines = File.ReadAllLines(FileName);
            string[] LineBuf;

            bool is_end = false;

            CashShopCategory item = new CashShopCategory();
            CashShopCategory subcat = new CashShopCategory();

            int nCounter = 0;


            while (!is_end)
            {
                try
                {
                    LineBuf = Lines[nCounter].Split(
                        new char[] { '@' });
                    item.ID = int.Parse(LineBuf[0]);
                    item.Name = LineBuf[1];
                    item.Unknown1 = int.Parse(LineBuf[2]);
                    item.Unknown2 = int.Parse(LineBuf[3]);
                    item.Group = int.Parse(LineBuf[4]);
                    item.Position = int.Parse(LineBuf[5]);
                    item.IsHeader = int.Parse(LineBuf[6]);
                    item.SubCategories = new List<CashShopCategory>();
                    nCounter++;

                    if (item.IsHeader == 1)
                    {
                       Categories.Add(item);
                    }

                    while (true)
                    {
                        //read sub-cats
                        item = new CashShopCategory();

                        LineBuf = Lines[nCounter].Split(
                       new char[] { '@' });
                        item.ID = int.Parse(LineBuf[0]);
                        item.Name = LineBuf[1];
                        item.Unknown1 = int.Parse(LineBuf[2]);
                        item.Unknown2 = int.Parse(LineBuf[3]);
                        item.Group = int.Parse(LineBuf[4]);
                        item.Position = int.Parse(LineBuf[5]);
                        item.IsHeader = int.Parse(LineBuf[6]);
                        item.SubCategories = new List<CashShopCategory>();

                        nCounter++;

                        

                        if (item.IsHeader == 1)
                        {
                            nCounter--;
                            break;
                        }
                        else Categories[Categories.Count - 1].SubCategories.Add(item);
                       
                    }
                   
                   
                }
                catch { break; }

            }

            return res;
        }
    }
}
