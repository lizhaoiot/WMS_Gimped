/*
 * 模型说明：将用户表封装成对象
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WMS_Gimped.Model
{
    public class Users
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        private string _jobs;

        public string Jobs
        {
            get { return _jobs; }
            set { _jobs = value; }
        }
    }
}
