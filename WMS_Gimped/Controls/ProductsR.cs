/*模块名称：产品入库
 * 
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils;
using System.Data.SqlClient;
using DataGridViewAutoFilter;
using Com.Hui.ERP.CommonUtils;

namespace WMS_Gimped.Controls
{
    public partial class ProductsR : UserControl
    {
        #region 定量
        public static List<Model.CPRTemp> cpr = new List<Model.CPRTemp>();
        #endregion

        #region 变量

        #endregion

        #region 初始化
        public ProductsR()
        {
            InitializeComponent();
            //设置时间控件显示时间
            dateEdit1.Properties.VistaDisplayMode = DefaultBoolean.True;
            dateEdit1.Properties.VistaEditTime = DefaultBoolean.True;
            this.dateEdit1.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit1.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit1.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            dateEdit2.Properties.VistaDisplayMode = DefaultBoolean.True;
            dateEdit2.Properties.VistaEditTime = DefaultBoolean.True;
            this.dateEdit2.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit2.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit2.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEdit2.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";
            //设置下拉框选项
            comboBoxEdit1.Properties.Items.Add("未审核");
            comboBoxEdit1.Properties.Items.Add("已审核");
            //comboBoxEdit1.Properties.Items.Add("入库明细");
            //comboBoxEdit1.Properties.Items.Add("实际库存");
            //数据库连接字符串
            SqlHelper.connectionStr = GetSQLConnect();
            comboBoxEdit1.SelectedIndex = 0;
            //DataGridView
            dateEdit1.EditValue= DateTime.Now.Date.AddDays(-2).ToString();
            dateEdit2.EditValue = DateTime.Now.Date.ToString();
            dataGridView1.MultiSelect = false;
            dataGridView2.MultiSelect = false;
            dataGridView3.MultiSelect = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void ProductsR_Load(object sender, EventArgs e)
        {
            //DataFresch();
        }
        #endregion

        #region 窗体事件
        //查询未审核的入库单
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //跳转TabControl页面
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            tabControl2.SelectedTab = tabControl2.TabPages[0];
            this.Cursor = Cursors.WaitCursor;
            //判断数据完整性
            if (dateEdit1.Text.Equals(""))
            {
                MessageBox.Show("开始时间不允许为空","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            if (dateEdit2.Text.Equals(""))
            {
                MessageBox.Show("结束时间不允许为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            try
            {
                if (comboBoxEdit1.Text.Equals("未审核"))
                {
                    DataTable dt1 = new DataTable();
                    SqlParameter[] para ={
                    new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                   ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)
                   ,new SqlParameter("@FLAG", SqlDbType.VarChar)};
                    para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                    para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                    para[2].Value = Getmodevalue(comboBoxEdit1.EditValue.ToString());

                    dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDQuery", para);
                    DataGridViewAutoFilter.DataGridViewFunction Get = new DataGridViewFunction();
                    //Get.GridViewDataLoad(dt1, dataGridView1);//填充DataGridView
                    //Get.GridViewHeaderFilter(dataGridView1);
                    dataGridView1.DataSource = dt1;
                    dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    //dataGridView1.Refresh();
                    #region 统计数据
                    double index = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        index = index + Convert.ToDouble(dt1.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel2.Text = Convert.ToString(index);
                    #endregion

                    this.Cursor = Cursors.Default;
                }
                else
                {
                    DataTable dt1 = new DataTable();
                    SqlParameter[] para ={
                    new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                   ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)
                   ,new SqlParameter("@FLAG", SqlDbType.VarChar)};
                    para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                    para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                    para[2].Value = Getmodevalue(comboBoxEdit1.EditValue.ToString());

                    dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDQuery", para);
                    //dataGridView2.DataSource = dt1;
                    //DataGridViewAutoFilter.DataGridViewFunction Get = new DataGridViewFunction();
                    //Get.GridViewDataLoad(dt1, dataGridView2);//填充DataGridView
                    dataGridView2.DataSource = dt1;
                    dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    dataGridView2.Refresh();

                    #region 统计数据
                    double index = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        index = index + Convert.ToDouble(dt1.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel4.Text = Convert.ToString(index);
                    #endregion

                    this.Cursor = Cursors.Default;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
        }
        //查询入库情况
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    SqlParameter[] para = {
            //    new SqlParameter("@BDBH", SqlDbType.Text),
            //    new SqlParameter("@SCDH", SqlDbType.Text),
            //    new SqlParameter("@CPBH", SqlDbType.Text),
            //    new SqlParameter("@CPMC", SqlDbType.Text)
            //};
            //    para[0].Value = dataGridView2.Rows[dataGridView2.SelectedRows.Count].Cells["入库单号"].Value.ToString();
            //    para[1].Value = dataGridView2.Rows[dataGridView2.SelectedRows.Count].Cells["生产单号"].Value.ToString();
            //    para[2].Value = dataGridView2.Rows[dataGridView2.SelectedRows.Count].Cells["产品编号"].Value.ToString();
            //    para[3].Value = dataGridView2.Rows[dataGridView2.SelectedRows.Count].Cells["产品名称"].Value.ToString();
            //    dataGridView2.DataSource = null;
            //    dataGridView2.DataSource = SqlHelper.ExecStoredProcedureDataTable("WMS1", para);
            //    SqlHelper.GetConnection().Close();
            //    dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
            //    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            //    dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            //    dataGridView2.Refresh();
            //}
            //catch
            //{
            //    dataGridView2.DataSource = null;
            //    return;
            //}
        }
        //进行货位审核并标记货位
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var pt = new WMS_Gimped.Frm.PalletTags();
            pt.StartPosition = FormStartPosition.CenterParent;
            for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
            {
                pt.BDBH = this.dataGridView1.SelectedRows[i].Cells["入库单号"].EditedFormattedValue.ToString();
                pt.CPBH = this.dataGridView1.SelectedRows[i].Cells["产品编号"].EditedFormattedValue.ToString();
                pt.CPMC = this.dataGridView1.SelectedRows[i].Cells["产品名称"].EditedFormattedValue.ToString();
                pt.RKSL = this.dataGridView1.SelectedRows[i].Cells["入库数量"].EditedFormattedValue.ToString();
                //pt.SCBH = this.dataGridView1.SelectedRows[i].Cells["生产单号"].EditedFormattedValue.ToString();
                pt.DDSL = this.dataGridView1.SelectedRows[i].Cells["订单数量"].EditedFormattedValue.ToString();
                pt.DWMC = this.dataGridView1.SelectedRows[i].Cells["单位名称"].EditedFormattedValue.ToString();
                pt.ZDRQ = this.dataGridView1.SelectedRows[i].Cells["制单日期"].EditedFormattedValue.ToString();
            }
            if (pt.ShowDialog(this) == DialogResult.OK)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);   
                }
                tabControl1.SelectedTab = tabControl1.TabPages[1];
                dataGridView3.DataSource = pt.dtpublic;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView3.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView3.Refresh();
            }
            else
            {
                dataGridView3.DataSource = null;
                return;
            }
        }
        //入库单扫描
        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;
                DataTable dt1 = null;
                if (!textEdit1.Text.Trim().Equals(""))
                {
                    //入库单查询 
                    SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
                    para[0].Value = textEdit1.Text;
                    dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDQuery1", para);
                    DataTable dtSHF = dt1.Clone();
                    DataTable dtNONSHF = dt1.Clone();
                    //判断入库单是已审核单据还是未审核单据
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (JudgeDocuments(dr["入库单号"].ToString()))
                        {
                            dtSHF.Rows.Add(dr.ItemArray);
                        }
                        else
                        {
                            dtNONSHF.Rows.Add(dr.ItemArray);
                        }
                    }
                    dataGridView2.DataSource = dtSHF;
                    dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    dataGridView2.Refresh();

                    dataGridView1.DataSource = dtNONSHF;
                    dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    dataGridView1.Refresh();

                    #region 统计数据
                    double index1 = 0;
                    for (int i = 0; i < dtNONSHF.Rows.Count; i++)
                    {
                        index1 = index1 + Convert.ToDouble(dtNONSHF.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel2.Text = Convert.ToString(index1);

                    double index2 = 0;
                    for (int i = 0; i < dtSHF.Rows.Count; i++)
                    {
                        index2 = index2 + Convert.ToDouble(dtSHF.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel4.Text = Convert.ToString(index2);
                    #endregion

                }
                else
                {
                    dataGridView1.DataSource = dt1;
                    dataGridView2.DataSource = dt1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //单号查询
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;
                DataTable dt1 = null;
                if (!textEdit2.Text.Trim().Equals(""))
                {
                    //入库单查询 
                    SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
                    para[0].Value = textEdit2.Text;
                    dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDQuery3", para);
                    DataTable dtSHF = dt1.Clone();
                    DataTable dtNONSHF = dt1.Clone();
                    //判断入库单是已审核单据还是未审核单据
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (JudgeDocuments(dr["入库单号"].ToString()))
                        {
                            dtSHF.Rows.Add(dr.ItemArray);
                        }
                        else
                        {
                            dtNONSHF.Rows.Add(dr.ItemArray);
                        }
                    }
                    dataGridView2.DataSource = dtSHF;
                    dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    dataGridView2.Refresh();

                    dataGridView1.DataSource = dtNONSHF;
                    dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    dataGridView1.Refresh();

                    #region 统计数据
                    double index1 = 0;
                    for (int i = 0; i < dtNONSHF.Rows.Count; i++)
                    {
                        index1 = index1 + Convert.ToDouble(dtNONSHF.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel2.Text = Convert.ToString(index1);

                    double index2 = 0;
                    for (int i = 0; i < dtSHF.Rows.Count; i++)
                    {
                        index2 = index2 + Convert.ToDouble(dtSHF.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel4.Text = Convert.ToString(index2);
                    #endregion
                }
                else
                {
                    dataGridView1.DataSource = dt1;
                    dataGridView2.DataSource = dt1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //查看已入库明细
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string BDBH = dataGridView2.SelectedRows[0].Cells["入库单号"].Value.ToString();
            DataTable dtt = new DataTable();
            DataColumn dc = null;
            dc = dtt.Columns.Add("仓位", Type.GetType("System.String"));
            dc = dtt.Columns.Add("数量", Type.GetType("System.String"));
            dtt = SqlHelper.ExecuteDataTable("SELECT BDBH AS 入库单号,CPBH AS 产品编号,CPMC AS 产品名称,CW AS 仓位名称,SL2 AS 每盘入库数量,DW AS 单位,RQ AS 入库时间 FROM WMS_CP where bdbh='" + BDBH + "'");

            tabControl1.SelectedTab = tabControl1.TabPages[1];
            dataGridView3.DataSource = dtt;
            dataGridView3.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView3.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView3.Refresh();
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
            return "server="+IP+";uid="+ Uid + ";pwd="+ Pwd + ";Trusted_Connection=no;database="+ DB + "";
        }
        private bool JudgeDocuments(string s)
        {
            DataTable dttemp = SqlHelper.ExecuteDataTable("SELECT * FROM KC_CPRK_M WHERE BDBH='"+s+"' AND ISNULL(SHR,'')<>''");
            if (dttemp.Rows.Count.Equals(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string Getmodevalue(string s)
        {
            if (s == "未审核")
            {
                return "0";
            }
            if (s == "已审核")
            {
                return "1";
            }
            if (s == "入库明细")
            {
                return "2";
            }
            if (s == "实际库存")
            {
                return "3";
            }
            return "";
        }
        //刷新dataGridView2的数据
        public  void  DataFresch()
        {
            SqlParameter[] para ={
                new SqlParameter("@STARTTIME",SqlDbType.DateTime)
               ,new SqlParameter("@ENDTIME", SqlDbType.DateTime) };
            para[0].Value = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 00:00:00");
            para[1].Value = DateTime.Now;
            DataTable dt1 =SqlHelper.ExecStoredProcedureDataTable("WMS_RKDQuery2", para);
            dataGridView2.DataSource = dt1;
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView2.Refresh();
        }
        //筛选条件
        private void textEdit3_TextChanged(object sender, EventArgs e)
        {
            DataView dv1 = GetDgvToTable(dataGridView1).DefaultView;
            dv1.RowFilter = "入库单号=" + textEdit3.Text + "";

            DataGridViewAutoFilter.DataGridViewFunction Get1 = new DataGridViewFunction();
            Get1.GridViewDataLoad(dv1.ToTable(), dataGridView1);//填充DataGridView
            Get1.GridViewHeaderFilter(dataGridView1);

            DataView dv2 = GetDgvToTable(dataGridView2).DefaultView;
            dv2.RowFilter = "入库单号=" + textEdit3.Text + " or 产品编号=" + textEdit3.Text;

            DataGridViewAutoFilter.DataGridViewFunction Get2 = new DataGridViewFunction();
            Get2.GridViewDataLoad(dv2.ToTable(), dataGridView2);//填充DataGridView
            Get2.GridViewHeaderFilter(dataGridView2);

        }
        public DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            // 列强制转换
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            // 循环行
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion
    }
}
