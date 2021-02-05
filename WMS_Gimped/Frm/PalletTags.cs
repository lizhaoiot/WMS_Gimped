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
    public partial class PalletTags : Form
    {
        #region 定量
         private static PalletTags frm = null;
         private DataTable dt = new DataTable();
         public  DataTable dtpublic = new DataTable();

        #endregion

        #region 变量
         //入库单号
         public string BDBH = string.Empty;
         //产品编码
         public string CPBH = string.Empty;
         //产品名称
         public string CPMC = string.Empty;
         //入库数量
         public string RKSL = string.Empty;
         //生产单号
         public string SCBH = string.Empty;
         //订单数量
         public string DDSL = string.Empty;
         //单位
         public string DWMC = string.Empty;
         //制单日期
         public string ZDRQ = string.Empty;
         //生产数量
         //public string SCSL = string.Empty;
        #endregion

        #region 初始化
        public PalletTags()
        {
           InitializeComponent();
           SqlHelper.connectionStr = GetSQLConnect();
           this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
           InitDataTablePublic();
        }
        public static PalletTags CreateInstrance()
         {
             if (frm == null || frm.IsDisposed)
             {
                 frm = new PalletTags();
             }
             return frm;
         }
        private void InitDataTablePublic()
        {
            dtpublic.Columns.Add("入库单号", Type.GetType("System.String"));
            dtpublic.Columns.Add("产品编号", Type.GetType("System.String"));
            dtpublic.Columns.Add("产品名称", Type.GetType("System.String"));
            dtpublic.Columns.Add("仓位名称", Type.GetType("System.String"));
            dtpublic.Columns.Add("每盘入库数量", Type.GetType("System.String"));
            dtpublic.Columns.Add("单位", Type.GetType("System.String"));
            dtpublic.Columns.Add("入库时间", Type.GetType("System.String"));

            #region 构造datatable
            //初始化表格信息
            DataColumn dc = null;
            dc = dt.Columns.Add("仓位", Type.GetType("System.String"));
            dc = dt.Columns.Add("数量", Type.GetType("System.String"));
            #endregion
        }
        private void PalletTags_Load(object sender, EventArgs e)
        {
            #region 产品基本信息赋值
            textBox3.Text = BDBH;
            textBox4.Text = CPBH;
            textBox5.Text = CPMC;
            textBox6.Text = RKSL;
            textBox7.Text = RKSL;
            //textBox8.Text = SCBH;
            textBox9.Text = DDSL;
            textBox10.Text = DWMC;
            textBox2.Text= RKSL;
            #endregion

            #region CheckListBox
            DataTable dt1 = new DataTable();
            dt1 = SqlHelper.ExecuteDataTable("SELECT GoodName FROM WMS_CPHW");
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dt1.Rows[i]["GoodName"].ToString());
            }
            #endregion
        }

        #endregion

        #region 窗体事件
        //仓库筛选
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            DataTable dt1 = new DataTable();
            string s = "SELECT GoodName FROM WMS_CPHW where GoodName like '" + textBox1.Text + "%'";
            dt1 = SqlHelper.ExecuteDataTable(s);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(dt1.Rows[i]["GoodName"].ToString());
            }
        }
        //分盘操作
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string ww = GetCheckList();
            if (ww.Equals(""))
            {
                MessageBox.Show("请选择仓位", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string[] s = ww.Split('/');
            UInt64 ut = Convert.ToUInt64(s.Length);
            if (CheckIFCANRK(ut))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    DataRow newRow;
                    newRow = dt.NewRow();
                    newRow["仓位"] = s[i].ToString();
                    newRow["数量"] = ISNULL(textBox2.Text);
                    dt.Rows.Add(newRow);
                    textBox7.Text = Convert.ToString(Convert.ToUInt64(textBox7.Text) - Convert.ToUInt64(ISNULL(textBox2.Text)));
                }
                dataGridView1.DataSource = dt;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView1.Refresh();
              
            }
            else
            {
                MessageBox.Show("本盘入库数大于剩余入库数", "警告", MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
            }
        }
        //确定分盘操作结果
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!Convert.ToUInt64(textBox7.Text).Equals(0))
            {
                MessageBox.Show("存在未入库的产品", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dataGridView1.RowCount.Equals(0))
            {
                MessageBox.Show("请添加每盘入库明细", "警告", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            #region 审核入库单
              SqlParameter[] para ={
                  new SqlParameter("@BDBH",SqlDbType.Text),
                  new SqlParameter("@SHR",SqlDbType.Text),
            };         
            ERPInquire3.HelpClass.iniFileHelper ini = new ERPInquire3.HelpClass.iniFileHelper(Application.StartupPath + "/data/Config.ini");
            para[0].Value = BDBH;
            para[1].Value = ini.ReadString("Login", "Code", "");
            SqlHelper.ExecStoredProcedureDataTable("WMS_RKBJ", para);
            #endregion

            #region 将数据录入系统中
            Model.CP C1 = new Model.CP();
              for (int i = 0; i < dataGridView1.RowCount; i++)
              {
                  C1.Id     = Guid.NewGuid().ToString();//ID
                  C1.Cptype = "0";//类型
                  C1.Cpbh   = CPBH;//产品编号
                  C1.Cpmc   = CPMC;//产品名称
                  C1.Bdbh   = BDBH;//入库单号  
                  C1.Cw     = dataGridView1.Rows[i].Cells[0].Value.ToString();//仓位名称
                  C1.Sl1    = Convert.ToUInt32(RKSL);//入库数量
                  C1.Sl2    = Convert.ToUInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());//每盘入库数量
                  C1.Sl3    = Convert.ToUInt32(dataGridView1.Rows[i].Cells[1].Value.ToString());//每盘出库余量
                  C1.Ddsl   = Convert.ToInt64(DDSL);//订单数量
                  C1.Zdrq   = Convert.ToDateTime(ZDRQ);//制单日期
                  C1.Rq     = DateTime.Now;//入库时间
                  C1.Dw     = DWMC;//单位
                  C1.Remark = richTextBox1.Text;//备注
                  C1.TYDH = GetGDH(BDBH);
                  SQL.SqlExecute.CPInsert(C1);

                  #region dtpublic创建时间
                     DataRow drrow = dtpublic.NewRow();//创建新行
                     drrow["入库单号"] = BDBH;
                     drrow["产品编号"] = CPBH;
                     drrow["产品名称"] = CPMC;
                     drrow["仓位名称"] = dataGridView1.Rows[i].Cells[0].Value.ToString();
                     drrow["每盘入库数量"] = dataGridView1.Rows[i].Cells[1].Value.ToString();
                     drrow["单位"] = DWMC;
                     drrow["入库时间"] = DateTime.Now;
                     dtpublic.Rows.Add(drrow);//将新行加入到表中
                  #endregion
            }
            #endregion

            #region 返回页面刷新页面
               DialogResult = DialogResult.OK;
            #endregion
        }
        //取消
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
        //删除datagridview最后一行
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                textBox7.Text = Convert.ToString(Convert.ToUInt64(textBox7.Text)+Convert.ToUInt64(dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].EditedFormattedValue));             
                DataGridViewRow row = dataGridView1.Rows[dataGridView1.RowCount - 1];
                dataGridView1.Rows.Remove(row);
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region 方法
        //将生产施工单变成一个字段
        private string GetGDH(string BDBH)
        {
            string GDH = string.Empty;
            string SQL = "SELECT D.ZBXH_BDBH FROM KC_CPRK_M M LEFT JOIN KC_CPRK_D D  ON M.XH=D.ZBXH WHERE M.XH=D.ZBXH AND M.BDBH='" + BDBH + "'";
            DataTable dtGDH = SqlHelper.ExecuteDataTable(SQL);
            foreach (DataRow DW in dtGDH.Rows)
            {
                GDH = GDH + "," + DW["ZBXH_BDBH"].ToString();
            }
            return GDH;
        }
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
                        strCollected = strCollected + "/" +checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    }
                }
            }
            return strCollected;
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
            if (Convert.ToUInt64(textBox7.Text) >= (ink * Convert.ToUInt64(ISNULL(textBox2.Text))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //窗体使能方法
        private void EnableForm()
        {
            richTextBox1.ReadOnly = true;
            textBox1.ReadOnly = true;
            checkedListBox1.Enabled = false;
            textBox2.Enabled = false;
            simpleButton1.Enabled = false;
            simpleButton4.Enabled = false;
            dataGridView1.ReadOnly = true;
            simpleButton2.Enabled = false;
        }
        #endregion
    }
}
