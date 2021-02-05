using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Hui.iMRP.Utils;
using System.Data.SqlClient;

namespace WMS_Gimped.Frm
{
    public partial class Outbound : Form
    {
        #region 定量
         private static Outbound frm = null;
         DataTable dt = new DataTable();
         DataTable dtpublic = new DataTable();
        #endregion

        #region 变量
          //表单编号
          public string BDBH = string.Empty;
          //订单编号
          public string DDBH = string.Empty;
          //产品编号
          public string CPBH = string.Empty;
          //产品名称
          public string CPMC = string.Empty;
          //申请数量
          public long SQSL = 0;
          //订单数量
          public long DDSL = 0;
          //单位名称
          public string DWMC = string.Empty;
          //制单日期
           public string ZDRQ = string.Empty;
        #endregion

        #region 初始化
        public Outbound()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            SqlHelper.connectionStr = GetSQLConnect();
        }
        public static Outbound CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new Outbound();
            }
            return frm;
        }
        private void Outbound_Load(object sender, EventArgs e)
        {
            #region 初始化控件
             textBox1.Text = BDBH;
             textBox2.Text = DDBH;
             textBox3.Text = CPBH;
             textBox4.Text = CPMC;
             textBox5.Text = SQSL.ToString();
             textBox6.Text = DWMC;
             textBox7.Text = DDSL.ToString();
            #endregion

            #region 查询数据
              SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
              para[0].Value = CPBH;
              dt = SqlHelper.ExecStoredProcedureDataTable("WMS_CPCK3", para);
              dataGridView1.DataSource = dt;
              dataGridView1.Columns[5].Visible = false;
              dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
              dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
              dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
              dataGridView1.Refresh();
            #endregion

            #region dtpublic
              dtpublic.Columns.Add("出库单号", Type.GetType("System.String"));
              dtpublic.Columns.Add("产品编号", Type.GetType("System.String"));
              dtpublic.Columns.Add("产品名称", Type.GetType("System.String"));
              dtpublic.Columns.Add("仓位名称", Type.GetType("System.String"));
              dtpublic.Columns.Add("每盘出库数量", Type.GetType("System.String"));
              dtpublic.Columns.Add("单位", Type.GetType("System.String"));
              dtpublic.Columns.Add("出库时间", Type.GetType("System.String"));
            #endregion
        }
        #endregion

        #region 窗体事件
        //确定
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //验证出库数量是否大于申请数量
            long sum = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                DataGridViewCheckBoxCell cb = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells[0];
                bool flag = Convert.ToBoolean(cb.Value);
                if (flag == true)
                {
                    sum = sum + Convert.ToInt64(this.dataGridView1.Rows[i].Cells["数量"].Value);
                }
            }
            if (sum > SQSL)
            {
                MessageBox.Show("实际出库数量不能大于申请出库数量","警告",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            List<string> IDNEW = new List<string>();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                //判断是否选中
                DataGridViewCheckBoxCell cb = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells[0];
                bool flag = Convert.ToBoolean(cb.Value);
                if (flag == true)
                {
                    //判断数量是否有修改
                    string id = dt.Rows[i]["序号"].ToString();
                    SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text)};
                    para[0].Value = id;
                    DataTable dttemp= SqlHelper.ExecStoredProcedureDataTable("WMS_CheckCPCK", para);
                    //查询数量-界面获取数量
                    Int64 sl3 = Convert.ToInt64(dttemp.Rows[0][0].ToString()) - Convert.ToInt64(this.dataGridView1.Rows[i].Cells["数量"].Value);
                    if (sl3.Equals(0))
                    {
                        //数值没发生变化
                        Model.CP cp = new Model.CP();
                        cp.Id = Guid.NewGuid().ToString();
                        IDNEW.Add(cp.Id);
                        cp.Cptype = "1";
                        cp.Cpbh = CPBH;
                        cp.Cpmc = CPMC;
                        cp.Bdbh = BDBH;
                        cp.Scbh = id;
                        cp.Cw = this.dataGridView1.Rows[i].Cells["仓位"].Value.ToString();
                        cp.Zdrq =Convert.ToDateTime(ZDRQ);
                        cp.Rq = DateTime.Now;
                        cp.Sl1 = SQSL;
                        cp.Dw = DWMC;
                        cp.Sl2 = Convert.ToInt64(dttemp.Rows[0][0].ToString());
                        cp.Ddsl = DDSL;
                        cp.TYDH = DDBH;
                        cp.Remark = richTextBox1.Text;
                        SQL.SqlExecute.CPInsert(cp);
                        //更新出库余量
                        //查询现在余量
                        int xh = XH(BDBH, DDBH, CPBH);
                        SqlHelper.ExecCommand("UPDATE KC_CPCK_D SET YL=" +(Convert.ToUInt64(SQSL) - (Convert.ToUInt64(this.dataGridView1.Rows[i].Cells["数量"].Value))) + " WHERE xh=" + xh + "");
                        //更新入库记录
                        SqlHelper.ExecCommand("UPDATE WMS_CP SET SL3=" + sl3 + " WHERE ID='" + id + "'");

                        #region dtpublic插入数据
                        DataRow drrow = dtpublic.NewRow();//创建新行
                          drrow["出库单号"] = BDBH;
                          drrow["产品编号"] = CPBH;
                          drrow["产品名称"] = CPMC;
                          drrow["仓位名称"] = this.dataGridView1.Rows[i].Cells["仓位"].Value.ToString();
                          drrow["每盘出库数量"] = Convert.ToInt64(dttemp.Rows[0][0].ToString());
                          drrow["单位"] = DWMC;
                          drrow["出库时间"] = DateTime.Now;
                          dtpublic.Rows.Add(drrow);//将新行加入到表中
                        #endregion
                    }
                    else if (sl3 > 0)
                    {
                        //数值修改变化
                        //写入出库记录
                        Model.CP cp = new Model.CP();
                        cp.Id = Guid.NewGuid().ToString();
                        IDNEW.Add(cp.Id);
                        cp.Cptype = "1";
                        cp.Cpbh = CPBH;
                        cp.Cpmc = CPMC;
                        cp.Bdbh = BDBH;
                        cp.Scbh = id;
                        cp.Cw = this.dataGridView1.Rows[i].Cells["仓位"].Value.ToString();
                        cp.Sl1 = SQSL;
                        cp.Sl2 = Convert.ToUInt32(this.dataGridView1.Rows[i].Cells["数量"].Value);
                        cp.Ddsl = DDSL;
                        cp.Dw = DWMC;
                        cp.Zdrq = Convert.ToDateTime(ZDRQ);
                        cp.Rq = DateTime.Now;
                        cp.TYDH = DDBH;
                        cp.Remark = richTextBox1.Text;
                        SQL.SqlExecute.CPInsert(cp);
                        //更新出库余量
                        //查询现在余量
                        int xh = XH(BDBH, DDBH, CPBH);
                        SqlHelper.ExecCommand("UPDATE KC_CPCK_D SET YL=" + (Convert.ToUInt64(SQSL) - (Convert.ToUInt64(this.dataGridView1.Rows[i].Cells["数量"].Value))) + " WHERE xh=" + xh + "");
                        //写入入库余量
                        SqlHelper.ExecCommand("UPDATE WMS_CP SET SL3="+sl3+" WHERE ID='" + id + "'");

                        #region dtpublic插入数据
                          DataRow drrow = dtpublic.NewRow();//创建新行
                          drrow["出库单号"] = BDBH;
                          drrow["产品编号"] = CPBH;
                          drrow["产品名称"] = CPMC;
                          drrow["仓位名称"] = this.dataGridView1.Rows[i].Cells["仓位"].Value.ToString();
                          drrow["每盘出库数量"] = Convert.ToUInt32(this.dataGridView1.Rows[i].Cells["数量"].Value);
                          drrow["单位"] = DWMC;
                          drrow["出库时间"] = DateTime.Now;
                          dtpublic.Rows.Add(drrow);//将新行加入到表中
                        #endregion
                    }
                    else if (sl3 < 0)
                    {
                        MessageBox.Show("出库数量不能大于每盘现存数量", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }        
                }
            }
            WMS_Gimped.Controls.ProductsC.dtpublic = dtpublic;
            DialogResult = DialogResult.OK;
        }
        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region 方法
        //获得从表序号
        private int XH(string BDBH,string DDH,string CPBH)
        {
            string sql = "SELECT TOP 1 D.XH FROM KC_CPCK_M M LEFT JOIN KC_CPCK_D D ON M.XH=D.ZBXH WHERE M.BDBH='" + BDBH + "' AND D.ZBXH_DDH='" + DDH + "' AND D.ZBXH_CPBH='" + CPBH + "'";
            DataTable dtt = SqlHelper.ExecuteDataTable(sql);
            return  Convert.ToInt32(dtt.Rows[0][0].ToString());
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
