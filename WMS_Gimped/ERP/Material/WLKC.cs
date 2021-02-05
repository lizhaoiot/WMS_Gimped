using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Com.Hui.iMRP.Utils;
using System.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace WMS_Gimped.ERP.Material
{
    public partial class WLKC : UserControl
    {
        #region 定量
        private static WLKC frm = null;
        #endregion

        #region 变量

        #endregion

        #region 初始化
        public WLKC()
        {
            InitializeComponent();
        }
        public static WLKC CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new WLKC();
            }
            return frm;
        }
        #endregion

        #region 窗体事件
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SqlHelper.connectionStr = "server=192.168.0.97;uid=sa;pwd=;Trusted_Connection=no;database=hy";
            SqlParameter[] para ={
                    new SqlParameter("@WLMC",SqlDbType.VarChar),
                    new SqlParameter("@WLLB",SqlDbType.VarChar),
                    new SqlParameter("@ZBXH_CK",SqlDbType.VarChar),
                    new SqlParameter("@WLZL",SqlDbType.VarChar),
                    new SqlParameter("@WLFL",SqlDbType.VarChar),
                    new SqlParameter("@WLBH",SqlDbType.VarChar)
                 };
            para[0].Value = "";
            para[1].Value = "";
            para[2].Value = "";
            para[3].Value = "";
            para[4].Value = "";
            para[5].Value = toolStripTextBox1.Text;
            DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("HUI_KC_CLSJKC1", para);
            dataGridView1.DataSource = dt1;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView1.Refresh();
            this.Cursor = Cursors.Default;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Export2Excel("ERP材料实际库存", GetDgvToTable(dataGridView1));
        }
        #endregion

        #region 方法
        public static void Export2Excel(string nodename, DataTable dt)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = nodename + DateTime.Now.ToString("yyyyMMddhhmmss");
            dlg.Filter = "xlsx files(*.xlsx)|*.xlsx|xls files(*.xls)|*.xls|All files(*.*)|*.*";
            dlg.ShowDialog();
            if (dlg.FileName.IndexOf(":") < 0) return; //被点了"取消"
            ExcelHelper helper = new ExcelHelper(dlg.FileName);
            //int i=helper.DataTableToExcel(dt, "sheet1", true);
            DataTableToExcel1(dt, dlg.FileName, "sheet1", true); ;
            MessageBox.Show("导出成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DataTable GetDgvToTable(DataGridView dgv)
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
        public static int DataTableToExcel1(DataTable dt, string fileName, string sheetName, bool isColumnWritten)
        {
            IWorkbook workbook = null;
            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0)
            {
                workbook = new XSSFWorkbook();
            }
            else if (fileName.IndexOf(".xls") > 0)
            {
                workbook = new HSSFWorkbook();
            }
            int result;
            if (workbook != null)
            {
                ISheet sheet = workbook.CreateSheet(sheetName);
                int num;
                if (isColumnWritten)
                {
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        row.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    }
                    num = 1;
                }
                else
                {
                    num = 0;
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    IRow row = sheet.CreateRow(num);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if ((dt.Columns[i].ColumnName.Contains("数") || dt.Columns[i].ColumnName.Contains("价") || dt.Columns[i].ColumnName.Contains("量") || dt.Columns[i].ColumnName.Contains("面积") || dt.Columns[i].ColumnName.Contains("长") || dt.Columns[i].ColumnName.Contains("宽") || dt.Columns[i].ColumnName.Contains("宽") || dt.Columns[i].ColumnName.Contains("重") || dt.Columns[i].ColumnName.Contains("度")) && dt.Rows[j][i] != DBNull.Value)
                        {
                            try
                            {
                                row.CreateCell(i).SetCellValue(Convert.ToDouble(dt.Rows[j][i]));
                            }
                            catch
                            {
                                row.CreateCell(i).SetCellValue(dt.Rows[j][i].ToString());
                            }
                        }
                        else
                        {
                            row.CreateCell(i).SetCellValue(dt.Rows[j][i].ToString());
                        }
                    }
                    num++;
                }
                workbook.Write(fileStream);
                fileStream.Close();
                workbook.Close();
                result = num;
            }
            else
            {
                result = -1;
            }
            return result;
        }

        #endregion


    }
}
