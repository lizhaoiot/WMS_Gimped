/*
 * 模型说明：将产品表封装成对象
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class CP
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _cptype;

        public string Cptype
        {
            get { return _cptype; }
            set { _cptype = value; }
        }
        private string _cpbh;

        public string Cpbh
        {
            get { return _cpbh; }
            set { _cpbh = value; }
        }
        private string _cpmc;

        public string Cpmc
        {
            get { return _cpmc; }
            set { _cpmc = value; }
        }
        private string _bdbh;

        public string Bdbh
        {
            get { return _bdbh; }
            set { _bdbh = value; }
        }
        private string _scbh;

        public string Scbh
        {
            get { return _scbh; }
            set { _scbh = value; }
        }
        private string _cw;

        public string Cw
        {
            get { return _cw; }
            set { _cw = value; }
        }
        private long _sl1;

        public long Sl1
        {
            get { return _sl1; }
            set { _sl1 = value; }
        }
        private long _sl2;

        public long Sl2
        {
            get { return _sl2; }
            set { _sl2 = value; }
        }
        private long _sl3;

        public long Sl3
        {
            get { return _sl3; }
            set { _sl3 = value; }
        }
        private long _ddsl;

        public long Ddsl
        {
            get { return _ddsl; }
            set { _ddsl = value; }
        }
        private System.DateTime _zdrq;

        public System.DateTime Zdrq
        {
            get { return _zdrq; }
            set { _zdrq = value; }
        }
        private System.DateTime _rq;

        public System.DateTime Rq
        {
            get { return _rq; }
            set { _rq = value; }
        }
        private string _dw;

        public string Dw
        {
            get { return _dw; }
            set { _dw = value; }
        }
        private string _remark;

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        private string _tydh;

        public string TYDH
        {
            get { return _tydh; }
            set { _tydh = value; }
        }
    }
}
