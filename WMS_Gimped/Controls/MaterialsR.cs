/*模块名称:材料入库
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
    public partial class MaterialsR : UserControl
    {
        #region 定量

        #endregion

        #region 变量

        #endregion

        #region 初始化
        public MaterialsR()
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
            dateEdit1.EditValue = DateTime.Now.Date.AddDays(-2).ToString();
            dateEdit2.EditValue = DateTime.Now.Date.ToString();
            //数据库连接字符串
            SqlHelper.connectionStr = GetSQLConnect();
            comboBoxEdit1.SelectedIndex = 0;
            //DataGridView
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //combobox数据绑定
            DataTable dtcom1 = SqlHelper.ExecuteDataTable("SELECT KHJB FROM JCZL_GYSLB");
            foreach (DataRow dr in dtcom1.Rows)
            {
              comboBoxEdit2.Properties.Items.Add(dr[0]);
            }
        }
        #endregion

        #region 窗体事件
        //查询
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if(dateEdit1.Text.Equals(""))
            {
                MessageBox.Show("制单开始时间不允许为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            if (dateEdit2.Text.Equals(""))
            {
                MessageBox.Show("制单结束时间不允许为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cursor = Cursors.Default;
                return;
            }
            if (comboBoxEdit1.EditValue.ToString() == "未审核")
            {
                try
                {
                    DataTable dt1 = new DataTable();
                    SqlParameter[] para ={
                    new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                   ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)
                   ,new SqlParameter("@FLAG", SqlDbType.VarChar)};
                    para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                    para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                    para[2].Value = Getmodevalue(comboBoxEdit1.EditValue.ToString());

                    dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDCLQuery", para);
                    dataGridView1.DataSource = dt1;
                    dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                    dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                    dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                    dataGridView1.Refresh();

                    #region 统计数据
                    double index = 0;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        index = index + Convert.ToDouble(dt1.Rows[i]["入库数量"].ToString());
                    }
                    toolStripStatusLabel2.Text = Convert.ToString(index);
                    #endregion
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            else
            {
                try
                {
                    DataTable dt1 = new DataTable();
                    SqlParameter[] para ={
                    new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                   ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)
                   ,new SqlParameter("@FLAG", SqlDbType.VarChar)};
                    para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                    para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                    para[2].Value = Getmodevalue(comboBoxEdit1.EditValue.ToString());

                    dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDCLQuery", para);
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
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            this.Cursor = Cursors.Default;
        }
        //材料入库数量
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            WMS_Gimped.Frm.MaterialInput ob = WMS_Gimped.Frm.MaterialInput.CreateInstrance();
            for (int i = 0; i < this.dataGridView1.SelectedRows.Count; i++)
            {
                ob.BDBH = this.dataGridView1.SelectedRows[i].Cells["表单单号"].EditedFormattedValue.ToString();
                ob.CLBM = this.dataGridView1.SelectedRows[i].Cells["物料编号"].EditedFormattedValue.ToString();
                ob.CLMC = this.dataGridView1.SelectedRows[i].Cells["物料名称"].EditedFormattedValue.ToString();
                ob.RKSL = Convert.ToDecimal(this.dataGridView1.SelectedRows[i].Cells["入库数量"].EditedFormattedValue.ToString());
                ob.GYSLB = this.dataGridView1.SelectedRows[i].Cells["供应商类别"].EditedFormattedValue.ToString();
                ob.GYSMC= this.dataGridView1.SelectedRows[i].Cells["供应商名称"].EditedFormattedValue.ToString();
                ob.DWMC= this.dataGridView1.SelectedRows[i].Cells["单位"].EditedFormattedValue.ToString();
            }
            if (ob.ShowDialog().Equals(DialogResult.OK))
            {

            }
        }
        private void comboBoxEdit2_TextChanged(object sender, EventArgs e)
        {
            comboBoxEdit3.Text = "";
            comboBoxEdit3.Properties.Items.Clear();
            if (comboBoxEdit2.Text.Trim().Equals("纸张供应商"))
            {
                DataTable dtcom1 = SqlHelper.ExecuteDataTable("SELECT ZWMC FROM JCZL_GYSZL_M WHERE ZBXH_GYSLB='01'");
                foreach (DataRow dr in dtcom1.Rows)
                {
                    comboBoxEdit3.Properties.Items.Add(dr[0]);
                }
            }
            if (comboBoxEdit2.Text.Trim().Equals("直接材料供应商"))
            {
                DataTable dtcom1 = SqlHelper.ExecuteDataTable("SELECT ZWMC FROM JCZL_GYSZL_M WHERE ZBXH_GYSLB='02'");
                foreach (DataRow dr in dtcom1.Rows)
                {
                    comboBoxEdit3.Properties.Items.Add(dr[0]);
                }
            }
            if (comboBoxEdit2.Text.Trim().Equals("间接材料供应商"))
            {
                DataTable dtcom1 = SqlHelper.ExecuteDataTable("SELECT ZWMC FROM JCZL_GYSZL_M WHERE ZBXH_GYSLB='03'");
                foreach (DataRow dr in dtcom1.Rows)
                {
                    comboBoxEdit3.Properties.Items.Add(dr[0]);
                }
            }
            if (comboBoxEdit2.Text.Trim().Equals("其它供应商"))
            {
                DataTable dtcom1 = SqlHelper.ExecuteDataTable("SELECT ZWMC FROM JCZL_GYSZL_M WHERE ZBXH_GYSLB='04'");
                foreach (DataRow dr in dtcom1.Rows)
                {
                    comboBoxEdit3.Properties.Items.Add(dr[0]);
                }
            }
        }
        //供应商查询
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = new DataTable();
                SqlParameter[] para ={
                    new SqlParameter("@DAH1",SqlDbType.Text)
                   ,new SqlParameter("@DAH2", SqlDbType.Text)};
                para[0].Value = comboBoxEdit2.Text;
                para[1].Value = comboBoxEdit3.Text;

                dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_RKDCLQuery1", para);
                dataGridView1.DataSource = dt1;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView1.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
        }
        #endregion

        #region 方法
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
            return "";
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
