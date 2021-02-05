using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class CLHW
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _mterialName;

        public string MterialName
        {
            get { return _mterialName; }
            set { _mterialName = value; }
        }
        private string _storeName;

        public string StoreName
        {
            get { return _storeName; }
            set { _storeName = value; }
        }
        private string _finalize;

        public string Finalize
        {
            get { return _finalize; }
            set { _finalize = value; }
        }
        private int _flag;

        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

    }
}
