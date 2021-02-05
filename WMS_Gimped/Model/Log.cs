using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class Log
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _bdbh;

        public string Bdbh
        {
            get { return _bdbh; }
            set { _bdbh = value; }
        }
        private string _zdr;

        public string Zdr
        {
            get { return _zdr; }
            set { _zdr = value; }
        }
        private System.DateTime _zdrq;

        public System.DateTime Zdrq
        {
            get { return _zdrq; }
            set { _zdrq = value; }
        }
        private string _ip;

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _mac;

        public string Mac
        {
            get { return _mac; }
            set { _mac = value; }
        }
        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _cw;

        public string Cw
        {
            get { return _cw; }
            set { _cw = value; }
        }
        private long _sl;

        public long Sl
        {
            get { return _sl; }
            set { _sl = value; }
        }
    }
}
