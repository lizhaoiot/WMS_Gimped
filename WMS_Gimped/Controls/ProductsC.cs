/*模块名称：产品出库
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
using Com.Hui.iMRP.Utils;
using System.Data.SqlClient;

namespace WMS_Gimped.Controls
{
    public partial class ProductsC : UserControl
    {

        #region 定量

        #endregion

        #region 变量
        public static DataTable dtpublic = new DataTable();
        #endregion

        #region 初始化
        public ProductsC()
        {
            InitializeComponent();
            SqlHelper.connectionStr = GetSQLConnect();
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
            this.dateEdit2.Properties.Mask.EditMask = "yyyy-MM-dd HH:mm:ss";            //DataGridView
            dateEdit1.EditValue = DateTime.Now.Date.AddDays(-2).ToString();
            dateEdit2.EditValue = DateTime.Now.Date.ToString();
            dataGridView1.MultiSelect = false;
            dataGridView2.MultiSelect = false;
            dataGridView3.MultiSelect = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        #endregion

        #region 窗体事件

        //送货通知单查询
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SHTZD();
        }
        //出库单查询
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            tabControl2.SelectedTab = tabControl2.TabPages[0];
            DataTable dt1 = new DataTable();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                SqlParameter[] para ={
                 new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)};
                para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CPCK2", para);
                dataGridView2.DataSource = dt1;
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView2.Refresh();
            }
            catch 
            {
                return;
            }

            #region 统计数据
            double index = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                index = index + Convert.ToDouble(dt1.Rows[i]["出库数量"].ToString());
            }
            toolStripStatusLabel4.Text = Convert.ToString(index);
            #endregion
            this.Cursor = Cursors.Default;
        }
        //标签扫描
        private void textEdit2_TextChanged(object sender, EventArgs e)
        {
            SqlParameter[] para ={
                 new SqlParameter("@DAH",SqlDbType.Text)};
            para[0].Value = textEdit2.Text;
            DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CPCK1", para);
            dataGridView1.DataSource = dt1;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView1.Refresh();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var ob = new WMS_Gimped.Frm.Outbound();
            ob.StartPosition = FormStartPosition.CenterParent;
            for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
            {
                ob.BDBH = this.dataGridView1.SelectedRows[i].Cells["表单编号"].EditedFormattedValue.ToString();
                ob.DDBH = this.dataGridView1.SelectedRows[i].Cells["订单编号"].EditedFormattedValue.ToString();
                ob.CPBH = this.dataGridView1.SelectedRows[i].Cells["产品编号"].EditedFormattedValue.ToString();
                ob.CPMC = this.dataGridView1.SelectedRows[i].Cells["产品名称"].EditedFormattedValue.ToString();
                ob.SQSL = Convert.ToInt64(this.dataGridView1.SelectedRows[i].Cells["出库数量"].EditedFormattedValue.ToString());
                ob.DDSL = Convert.ToInt64(this.dataGridView1.SelectedRows[i].Cells["订单数量"].EditedFormattedValue.ToString());
                ob.DWMC = this.dataGridView1.SelectedRows[i].Cells["单位名称"].EditedFormattedValue.ToString();
                ob.ZDRQ = this.dataGridView1.SelectedRows[i].Cells["制单日期"].EditedFormattedValue.ToString();
            }
            if (ob.ShowDialog(this) == DialogResult.OK)
            {
                tabControl2.SelectedTab = tabControl2.TabPages[1];
                dataGridView3.DataSource = dtpublic;
                dataGridView3.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView3.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView3.Refresh();

                SHTZD();
            }
            else
            {
                return;
            }
        }
        //查看出库明细
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string BDBH = dataGridView2.SelectedRows[0].Cells["出库单号"].Value.ToString();
            DataTable dtt = new DataTable();
            dtt = SqlHelper.ExecuteDataTable("SELECT BDBH AS 出库单号,CPBH AS 产品编号,CPMC AS 产品名称,CW AS 仓位名称,SL2 AS 每盘出库数量,DW AS 单位,RQ AS 出库时间 FROM WMS_CP where bdbh='" + BDBH + "'");

            tabControl2.SelectedTab = tabControl2.TabPages[1];
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
            return "server=" + IP + ";uid=" + Uid + ";pwd=" + Pwd + ";Trusted_Connection=no;database=" + DB + "";
        }
        private void SHTZD()
        {
            DataTable dt1 = new DataTable();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                SqlParameter[] para ={
                 new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)};
                para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CPCK", para);
                dataGridView1.DataSource = dt1;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #region 统计数据
            double index = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                index = index + Convert.ToDouble(dt1.Rows[i]["出库数量"].ToString());
            }
            toolStripStatusLabel3.Text = Convert.ToString(index);
            #endregion

            this.Cursor = Cursors.Default;
        }
        #endregion
    }
}
