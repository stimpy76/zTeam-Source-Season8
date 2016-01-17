using System;
using System.Collections.Generic;
using System.Text;

namespace zDBManager
{
    class Extra
    {
        public struct Find
        {
            public string AccountID;
            public string Character;
        };
        public static Find FindResult = new Find();
    }
}
