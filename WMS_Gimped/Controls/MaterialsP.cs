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
    public partial class MaterialsP : UserControl
    {
        #region 变量

        #endregion

        #region 定量

        #endregion

        #region 初始化
        public MaterialsP()
        {
            InitializeComponent();
        }
        private void MaterialsP_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }
        #endregion

        #region 窗体事件
        //查询
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            #region 物料基础资料查询
              SqlParameter[] para ={
                   new SqlParameter("@DAH",SqlDbType.Text)};
              para[0].Value = textBox1.Text;
              DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("MaterialQuery12", para);
              dataGridView1.DataSource = dt1;
              dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightCyan;
              dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
              dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
              dataGridView1.Refresh();
            #endregion

            #region 实际库存查询
              SqlParameter[] para1 ={
                     new SqlParameter("@DAH",SqlDbType.Text)};
              para[0].Value = "";
              DataTable dt2 = SqlHelper.ExecStoredProcedureDataTable("WMS_CLCK", para);
              dataGridView2.DataSource = dt1;
              dataGridView2.RowsDefaultCellStyle.BackColor = Color.LightCyan;
              dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
              dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
              dataGridView2.Refresh();
            #endregion
        }

        #endregion

        #region 方法

        #endregion
    }
}
