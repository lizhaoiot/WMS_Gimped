using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS_Gimped.Controls
{
    public partial class Hello : UserControl
    {
        #region 定量
          private static Hello frm = null;
        #endregion

        #region 变量
          //载入用户登录名
          public string name = string.Empty;
        #endregion

        #region 初始化
          public Hello()
        {
            InitializeComponent();
        }
          public static Hello CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new Hello();
            }
            return frm;
        }
          private void Hello_Load(object sender, EventArgs e)
          {
            ERPInquire3.HelpClass.iniFileHelper ini = new ERPInquire3.HelpClass.iniFileHelper(Application.StartupPath + "/data/Config.ini");
            string s = ini.ReadString("Login", "Name","");
            string[] ss = s.Split('-');
            name = ss[1];
            label1.Text = "欢迎"+name+"使用WMS仓库管理系统";
              //label1.TextAlign = ContentAlignment.MiddleCenter;
        }
        #endregion

        #region 窗体事件

        #endregion

        #region 方法

        #endregion
    }
}
