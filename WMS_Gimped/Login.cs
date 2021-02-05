using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Hui.iMRP.Utils;

namespace WMS_Gimped
{
    public partial class Login : Form
    {
        #region 定量
        #endregion

        #region 变量
        //初始化鼠标位置
        bool beginMove = false;
         int currentXPosition;
         int currentYPosition;
        #endregion 

        #region 初始化
        public Login()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            splitContainer1.Panel2Collapsed = true;
            this.Height = 220;
            //绑定数据源
            SqlHelper.connectionStr = GetSQLConnect();
            //ComboBox绑定数据源   
            this.comboBox1.ValueMember   = "ID";
            this.comboBox1.DisplayMember = "UserName";
            this.comboBox1.DataSource = SqlHelper.ExecuteDataTable("SELECT ID,UserName FROM WMS_Users");
        }
        #endregion

        #region 窗体事件
        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标
            }
        }

        private void Login_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void Login_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }
        }
        private void splitContainer1_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }
        }

        private void splitContainer1_Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void splitContainer1_Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标
            }
        }
        //登录
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (CheckPasswordCorrectness(comboBox1.SelectedValue.ToString(), textBox1.Text) == true)
            {
                //将登录人员信息写入配置文件中
                ERPInquire3.HelpClass.iniFileHelper ini = new ERPInquire3.HelpClass.iniFileHelper(Application.StartupPath+ "/data/Config.ini");
                string code = GetRSCode(comboBox1.Text);
                if (!code.Equals("FLASE"))
                {
                    ini.WriteString("Login", "Code", code);
                    ini.WriteString("Login", "Name", code+"-"+comboBox1.Text);
                    Mainfrm.Dialog = "OK";
                    switch (textBox5.Text)
                    {
                        case "成品库":
                            Mainfrm.Status = "0";
                            break;
                        case "材料库":
                            Mainfrm.Status = "1";
                            break;
                        case "全部库":
                            Mainfrm.Status = "2";
                            break;                       
                    }

                    Mainfrm.LoginName = comboBox1.Text;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("此用户未在ERP上注册", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show("密码输入不正确，请核对后输入", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        //退出
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        //注册用户
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (splitContainer1.Panel2Collapsed == false)
            {
                splitContainer1.Panel2Collapsed = true;
                this.Height = 220;
            }
            else
            {
                splitContainer1.Panel2Collapsed = false;
                this.Height = 450;
            }
        }
        //保存注册成员
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //验证两次密码是否输入正确
            if (CheckPasswordConsistent(textBox3.Text, textBox4.Text) == true)
            {
                Model.Users us = new Model.Users();
                us.Id = Guid.NewGuid().ToString();
                us.UserName = textBox2.Text;
                us.Password = textBox3.Text;
                us.Jobs = comboBox2.Text;
                if(SQL.SqlExecute.UsersInsert(us)==0)
                {
                    MessageBox.Show("用户注册成功", "消息", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    //刷新combobox的绑定数据源
                    this.comboBox1.DataSource = SqlHelper.ExecuteDataTable("SELECT ID,UserName FROM WMS_Users");
                    this.comboBox1.ValueMember = "ID";
                    this.comboBox1.DisplayMember = "UserName";
                }
                else
                {
                    MessageBox.Show("用户注册失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("两次输入的密码不一致","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }  
        }
        //根据选取名字的不用跳转不用的仓库名称
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("SELECT Jobs FROM WMS_Users where ID='" + comboBox1.SelectedValue.ToString() + "'");
                textBox5.Text = dt.Rows[0][0].ToString();
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region 方法
        //获取人员档案Code值
        private string GetRSCode(string s)
        {
            DataTable dt1 = SqlHelper.ExecuteDataTable("SELECT ID FROM ZCX_YHQX_M WHERE NAME='" + s + "'");
            if (dt1.Rows.Count.Equals(1))
            {
                return dt1.Rows[0][0].ToString();
            }
            else
            {
                return "FLASE";
            }
        }
        //验证两次输入密码是否一致
        private bool CheckPasswordConsistent(string s1,string s2)
        {
            if (s1 == s2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //验证密码的正确性
        private bool CheckPasswordCorrectness(string id,string pwd)
        {
            DataTable dt= SqlHelper.ExecuteDataTable("SELECT * FROM WMS_USers where ID='"+id+"' and Password='"+pwd+"'");
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
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

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
