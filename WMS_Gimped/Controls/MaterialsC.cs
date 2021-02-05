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
    public partial class MaterialsC : UserControl
    {
        #region 定量

        #endregion

        #region 变量

        #endregion

        #region 初始化
        public MaterialsC()
        {
            InitializeComponent();
        }
        #endregion

        #region 窗体方法
        //送货通知单查询
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] para ={
                 new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)};
                para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CLCK", para);
                dataGridView1.DataSource = dt1;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightCyan;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView1.Refresh();
            }
            catch
            {
                return;
            }
        }
        //出库单查询
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParameter[] para ={
                 new SqlParameter("@STARTTIME",SqlDbType.DateTime)
                ,new SqlParameter("@ENDTIME", SqlDbType.DateTime)};
                para[0].Value = Convert.ToDateTime(this.dateEdit1.EditValue.ToString());
                para[1].Value = Convert.ToDateTime(this.dateEdit2.EditValue.ToString());
                DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CLCK1", para);
                dataGridView1.DataSource = dt1;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightCyan;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView1.Refresh();
            }
            catch
            {
                return;
            }
        }
        //双击出现材料出库明细
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        #endregion

        #region 公共方法

        #endregion


    }
}
