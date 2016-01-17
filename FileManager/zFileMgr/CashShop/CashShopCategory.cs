using System;
using System.Collections.Generic;
using System.Text;

namespace zFileMgr.CashShop
{
    class CashShopCategory
    {
        public int ID;
        public string Name;
        public int Unknown1;
        public int Unknown2;
        public int Group;
        public int Position;
        public int IsHeader;

        public List<CashShopCategory> SubCategories;

        public CashShopCategory()
        {
            SubCategories = new List<CashShopCategory>();
        }
    }
}
