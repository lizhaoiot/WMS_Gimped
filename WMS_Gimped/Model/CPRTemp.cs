using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class CPRTemp
    {
        //入库单单号
        private string _bdbh;

        public string BDBH
        {
            get { return _bdbh; }
            set { _bdbh = value; }
        }
        //产品编码
        private string _cpbh;

        public string CPBH
        {
            get { return _cpbh; }
            set { _cpbh = value; }
        }
        //产品名称
        private string _cpmc;

        public string CPMC
        {
            get { return _cpmc; }
            set { _cpmc = value; }
        }
        //产品入库数量
        private Int64 _rksl;

        public Int64 RKSL
        {
            get { return _rksl; }
            set { _rksl = value; }
        }
        //产品入库库位
        private string _rkkw;

        public string RKKW
        {
            get { return _rkkw; }
            set { _rkkw = value; }
        }
    }
}
