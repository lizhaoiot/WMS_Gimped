using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class CLKW
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _cpkw;

        public string Cpkw
        {
            get { return _cpkw; }
            set { _cpkw = value; }
        }
        private string _cPKWName;

        public string CPKWName
        {
            get { return _cPKWName; }
            set { _cPKWName = value; }
        }

    }
}
