using Com.Hui.ERP.CommonUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WMS_Gimped.Frm
{
    public partial class ChangeCPLocation : Form
    {

        #region 定量
          public string CW = string.Empty;
          public string ID = string.Empty;
          public string SL = string.Empty;
        #endregion

        #region 变量

        #endregion

        #region 初始化
          public ChangeCPLocation()
        {
            InitializeComponent();
            SqlHelper.connectionStr = GetSQLConnect();
            #region CheckListBox
            DataTable dt1 = new DataTable();
            dt1 = SqlHelper.ExecuteDataTable("SELECT GoodName FROM WMS_CPHW");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dt1.Rows[i]["GoodName"].ToString());
            }
            #endregion
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }
          private void ChangeCPLocation_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.Items[i].ToString().Equals(CW))
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            labelControl4.Text = CW;
            textEdit2.Text = SL;
        }
        #endregion

        #region 窗体事件
          //确定
          private void simpleButton1_Click(object sender, EventArgs e)
        {
            int index = 0;
            string cw = null;
            //判断CheckList选择项不能为多个
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    index = index + 1;
                    cw = checkedListBox1.Items[i].ToString();
                }
            }
            if (index >= 2)
            {
                MessageBox.Show("仓位不能为多个", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cw.Equals(null))
            {
                MessageBox.Show("仓位不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cw.Equals(CW))
            {
                Int32 SLL = (Convert.ToInt32(SL) - Convert.ToInt32(textEdit2.Text));
                if (!SLL.Equals(0))
                {
                    MessageBox.Show("整盘移仓操作,数量不能修改", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //整盘移库
                SqlHelper.ExecCommand("UPDATE WMS_CP SET CW='" + cw + "' WHERE ID='" + ID + "'");
            }
            else
            {
                //部分移库
                //原仓位
                Int32 SLL = (Convert.ToInt32(SL) - Convert.ToInt32(textEdit2.Text));
                if (SLL < 0)
                {
                    MessageBox.Show("移库数量不能大于原仓位数量", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string sql = "UPDATE WMS_CP SET SL3=" + SLL + ",SCBH='"+ID+"' WHERE ID='" + ID + "'";
                SqlHelper.ExecCommand(sql);
                //新仓位
                //查找原数据
                DataTable dttemp = SqlHelper.ExecuteDataTable("SELECT * FROM WMS_CP WHERE ID='"+ID+"'");
                #region 插入新数据
                Model.CP C1 = new Model.CP();
                C1.Id = Guid.NewGuid().ToString();//ID
                C1.Cptype = "0";//类型
                C1.Cpbh = dttemp.Rows[0]["CPBH"].ToString();
                C1.Cpmc = dttemp.Rows[0]["CPMC"].ToString();
                C1.Bdbh = dttemp.Rows[0]["BDBH"].ToString();
                C1.Cw = cw;
                C1.Scbh = ID;
                C1.Sl1 = Convert.ToUInt32(dttemp.Rows[0]["SL1"].ToString());
                C1.Sl3 = Convert.ToUInt32(textEdit2.Text);
                C1.Ddsl = Convert.ToUInt32(dttemp.Rows[0]["DDSL"].ToString());
                C1.Zdrq = Convert.ToDateTime(dttemp.Rows[0]["ZDRQ"].ToString());
                C1.Rq = Convert.ToDateTime(dttemp.Rows[0]["RQ"].ToString());
                C1.Dw = (dttemp.Rows[0]["DW"].ToString());
                C1.Remark = (dttemp.Rows[0]["REMARK"].ToString());
                SQL.SqlExecute.CPInsert(C1);
                #endregion
            }
            DialogResult =  DialogResult.OK;
        }
          private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            DataTable dt1 = new DataTable();
            string s = "SELECT GoodName FROM WMS_CPHW where GoodName like '" + textEdit1.Text + "%'";
            dt1 = SqlHelper.ExecuteDataTable(s);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dt1.Rows[i]["GoodName"].ToString());
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
