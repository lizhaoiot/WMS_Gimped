using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Com.Hui.iMRP.Utils;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WMS_Gimped.Frm
{
    public partial class MaterialInput : Form
    {
        #region 定量
         private static MaterialInput frm = null;
         private DataTable dt = new DataTable();
        #endregion

        #region 变量
          //表单单号
          public string BDBH = string.Empty;
          //材料编码
          public string CLBM = string.Empty;
          //材料名称
          public string CLMC = string.Empty;
          //入库数量
          public decimal RKSL = 0;
          //供应商类别
          public string GYSLB = string.Empty;
          //供应商名称
          public string GYSMC = string.Empty;
          //单位
          public string DWMC = string.Empty;
        #endregion

        #region 初始化
          public MaterialInput()
          {
              InitializeComponent();
              SqlHelper.connectionStr = GetSQLConnect();
              this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          }
          public static MaterialInput CreateInstrance()
          {
              if (frm == null || frm.IsDisposed)
              {
                  frm = new MaterialInput();
              }
              return frm;
          }
          private void MaterialInput_Load(object sender, EventArgs e)
          {
              #region 材料基本信息初始化
                textBox3.Text  = BDBH;
                textBox4.Text  = CLBM;
                textBox5.Text  = CLMC;
                textBox6.Text  = Convert.ToString(RKSL);
                textBox8.Text  = GYSLB;
                textBox9.Text  = GYSMC;
                textBox10.Text = DWMC;
                textBox7.Text= Convert.ToString(RKSL);//剩余入库数量初始化
                textBox2.Text= Convert.ToString(RKSL);//本盘入库数量数值初始化
            #endregion

              #region CheckListBox数据初始化
                checkedListBox1.Items.Clear();
                DataTable dt1 = new DataTable();
                dt1 = SqlHelper.ExecuteDataTable("SELECT MaterialName FROM WMS_CLHW");
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    checkedListBox1.Items.Add(dt1.Rows[i]["MaterialName"].ToString());
                }
            #endregion

              #region 构造datatable
              dt = new DataTable();
              //初始化表格信息
              DataColumn dc = null;
              dc = dt.Columns.Add("仓位", Type.GetType("System.String"));
              dc = dt.Columns.Add("数量", Type.GetType("System.String"));
              #endregion
          }
        #endregion

        #region 分盘操作
          //添加
          private void simpleButton1_Click(object sender, EventArgs e)
        {
            //获取仓位名称
            string ww = GetCheckList();
            if (ww.Equals(""))
            {
                MessageBox.Show("请选择仓位", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> s = ww.Split('/').ToList<string>();
            UInt64 ut = Convert.ToUInt64(s.Count);
            if (CheckIFCANRK(ut))
            {
                for (int i = 0; i < s.Count; i++)
                {
                    DataRow newRow;
                    newRow = dt.NewRow();
                    newRow["仓位"] = s[i].ToString();
                    newRow["数量"] = ISNULL(textBox2.Text);
                    dt.Rows.Add(newRow);
                    textBox7.Text = Convert.ToString(Convert.ToDecimal(textBox7.Text) - Convert.ToDecimal(ISNULL(textBox2.Text)));
                }
                dataGridView1.DataSource = dt;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("本盘入库数大于剩余入库数", "警告", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
          //删除
          private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                textBox7.Text = Convert.ToString(Convert.ToDecimal(textBox7.Text) + Convert.ToDecimal(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].EditedFormattedValue));
                DataGridViewRow row = dataGridView1.Rows[dataGridView1.RowCount - 1];
                dataGridView1.Rows.Remove(row);
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region 材料入库操作
          //确定
          private void simpleButton2_Click(object sender, EventArgs e)
        {               
            //每盘入库数量必须要全部入库，不能剩余物料
            if (!Convert.ToUInt64(textBox7.Text).Equals(0))
            {
                MessageBox.Show("存在未入库的产品", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dataGridView1.RowCount.Equals(0))
            {
                MessageBox.Show("请添加每盘入库明细", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                #region 将数据录入系统中
                Maticsoft.Model.WMS_CL cl = new Maticsoft.Model.WMS_CL();
                Maticsoft.DAL.WMS_CL cldal = new Maticsoft.DAL.WMS_CL();
                cl.ID = Guid.NewGuid().ToString();
                cl.CLTYPE = "0";
                cl.CLBH = CLBM;
                cl.CLMC = CLMC;
                //cl.SGBH=
                cldal.Add(cl);
                #endregion

                #region 审核入库单
                  //将未审核的入库单变成已审核的状态，审核级别到达物料层次
                  SqlParameter[] para ={
                    new SqlParameter("@BDBH",SqlDbType.Text),
                    new SqlParameter("@WLBM",SqlDbType.Text),
                    new SqlParameter("@SHR",SqlDbType.Text)};
                  ERPInquire3.HelpClass.iniFileHelper ini = new ERPInquire3.HelpClass.iniFileHelper(Application.StartupPath + "/data/Config.ini");
                  para[0].Value = BDBH;
                  para[1].Value = CLBM;
                  para[2].Value = ini.ReadString("Login", "Code", "");
                  SqlHelper.ExecStoredProcedureDataTable("WMS_RKBJ1", para);
                #endregion

                #region 委托调用窗体更新
                  DialogResult = DialogResult.OK;
                #endregion
            }
            catch
            {

            }
            finally
            {

            }
        }
          //取消
          private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
          //仓库筛选
          private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            DataTable dt1 = new DataTable();
            string s = "SELECT MaterialName FROM WMS_CLHW where MaterialName like '" + textBox1.Text + "%'";
            dt1 = SqlHelper.ExecuteDataTable(s);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dt1.Rows[i]["MaterialName"].ToString());
            }
        }
        #endregion

        #region 方法
          private string GetCheckList()
        {
            string strCollected = string.Empty;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    if (strCollected == string.Empty)
                    {
                        strCollected = checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    }
                    else
                    {
                        strCollected = strCollected + "/" + checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    }
                }
            }
            return strCollected;
        }
          private string ISNULL(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "0";
            }
            else
            {
                return s;
            }
        }
          //判断剩余入库数量是否大于将要入库数量
          private bool CheckIFCANRK(UInt64 ink)
        {
            if (Convert.ToDecimal(textBox7.Text) >= (ink * Convert.ToDecimal(ISNULL(textBox2.Text))))
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
    }
}
