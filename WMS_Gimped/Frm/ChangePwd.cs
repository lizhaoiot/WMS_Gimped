using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Hui.iMRP.Utils;

namespace WMS_Gimped.Frm
{
    public partial class ChangePwd : Form
    {
        #region 定量
          private static ChangePwd frm = null;
        #endregion

        #region 变量
         public string name = string.Empty;
         public string code = string.Empty;
        #endregion

        #region 初始化
        public ChangePwd()
        {
             InitializeComponent();
             this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }
         public static ChangePwd CreateInstrance()
         {
             if (frm == null || frm.IsDisposed)
             {
                 frm = new ChangePwd();
             }
             return frm;
         }
        #endregion

        #region 窗体事件
        private void ChangePwd_Load(object sender, EventArgs e)
        {
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            SqlHelper.connectionStr = GetSQLConnect();
            textBox1.Text = name;
        }
        //确定
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (!textBox2.Text.Equals(textBox3.Text))
            {
                MessageBox.Show("两次输入密码不一致", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                SqlHelper.ExecCommand("UPDATE WMS_Users SET Password='" + textBox2.Text + "' WHERE Code='" + code + "'");
                MessageBox.Show("密码修改成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch
            {
                MessageBox.Show("密码修改失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 方法
        public string GetSQLConnect()
        {
            ERPInquire3.HelpClass.iniFileHelper myIni = new ERPInquire3.HelpClass.iniFileHelper(Application.StartupPath + "/data/Config.ini");
            string IP = myIni.IniReadValue("SqlConnect", "IP");
            string DB = myIni.IniReadValue("SqlConnect", "DB");
            string Uid = myIni.IniReadValue("SqlConnect", "Uid");
            string Pwd = myIni.IniReadValue("SqlConnect", "Pwd");
            return "server=" + IP + ";uid=" + Uid + ";pwd=" + Pwd + ";Trusted_Connection=no;database=" + DB + "";
        }
        #endregion
    }
}
