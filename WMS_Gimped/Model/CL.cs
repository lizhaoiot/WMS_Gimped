/*
 * 模型说明：将材料表封装成对象
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class CL
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _cltype;

        public string Cltype
        {
            get { return _cltype; }
            set { _cltype = value; }
        }
        private string _clbh;

        public string Clbh
        {
            get { return _clbh; }
            set { _clbh = value; }
        }
        private string _clmc;

        public string Clmc
        {
            get { return _clmc; }
            set { _clmc = value; }
        }
        private string _sgbh;

        public string Sgbh
        {
            get { return _sgbh; }
            set { _sgbh = value; }
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

    }
}
