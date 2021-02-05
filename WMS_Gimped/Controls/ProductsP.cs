/*模块名称：产品盘点
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
using Com.Hui.iMRP.Utils;
using System.Data.SqlClient;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;

namespace WMS_Gimped.Controls
{
    public partial class ProductsP : UserControl
    {
        #region 定量

        #endregion

        #region 变量
        private string CPBH = string.Empty;
        #endregion

        #region 初始化
         public ProductsP()
         {
             InitializeComponent();
             dataGridView1.MultiSelect = false;
             dataGridView2.MultiSelect = false;
             dataGridView3.MultiSelect = false;
             dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
             dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
             dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
         private void ProductsP_Load(object sender, EventArgs e)
         {
            SqlHelper.connectionStr = GetSQLConnect();
            radioButton1.Checked = true;
         }
        #endregion

        #region 窗体事件
         //查询
         private void simpleButton1_Click(object sender, EventArgs e)
         {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            tabControl2.SelectedTab = tabControl2.TabPages[0];
            if (radioButton1.Checked == true)
            {
                #region 产品编码查询
                  //产品档案
                  SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
                  para[0].Value = textBox1.Text;
                  DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_ProductQuery", para);
                  dataGridView1.DataSource = dt1;
                  dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                  dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                  dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                  dataGridView1.Refresh();
                  //实际库存
                  SqlParameter[] para1 = { new SqlParameter("@DAH", SqlDbType.Text) };
                  para1[0].Value = textBox1.Text;
                  DataTable dt2 = SqlHelper.ExecStoredProcedureDataTable("WMS_ProductInventory", para1);
                  dataGridView2.DataSource = dt2;
                  dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                  dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                  dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                  dataGridView2.Refresh();

                  #region 统计数据
                  double index = 0;
                  for (int i = 0; i < dt2.Rows.Count; i++)
                  {
                      index = index + Convert.ToDouble(dt2.Rows[i]["实际库存"].ToString());
                  }
                  toolStripStatusLabel4.Text = Convert.ToString(index);
                  #endregion
                #endregion
            }
            else
            {
                #region  产品名称查询
                  //产品档案
                  SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
                  para[0].Value = textBox1.Text;
                  DataTable dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_ProductQuery1", para);
                  dataGridView1.DataSource = dt1;
                  dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                  dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                  dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                  dataGridView1.Refresh();
                  //实际库存
                  SqlParameter[] para1 = { new SqlParameter("@DAH", SqlDbType.Text) };
                  para1[0].Value = textBox1.Text;
                  DataTable dt2 = SqlHelper.ExecStoredProcedureDataTable("WMS_ProductInventory1", para1);
                  dataGridView2.DataSource = dt2;
                  dataGridView2.RowsDefaultCellStyle.BackColor = Color.White;
                  dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
                  dataGridView2.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                  dataGridView2.Refresh();

                  #region 统计数据
                  double index = 0;
                  for (int i = 0; i < dt2.Rows.Count; i++)
                  {
                      index = index + Convert.ToDouble(dt2.Rows[i]["实际库存"].ToString());
                  }
                  toolStripStatusLabel4.Text = Convert.ToString(index);
                  #endregion
                #endregion
            }
        }
        //库存明细查询
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView3(dataGridView2.SelectedRows[0].Cells["产品编号"].Value.ToString());
        }
        private void dataGridView3_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    Point contextMenuPoint = dataGridView3.PointToClient(Control.MousePosition);
            //    contextMenuStrip1.Show(dataGridView3, contextMenuPoint);
            //
            //    dataGridView3.ClearSelection();
            //    dataGridView3.Rows[e.RowIndex].Selected = true;
            //    dataGridView3.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            //
            //}
        }
        private void dataGridView3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    if (e.RowIndex >= 0)
            //    {
            //        dataGridView3.ClearSelection();
            //        dataGridView3.Rows[e.RowIndex].Selected = true;
            //        dataGridView3.CurrentCell = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //        contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            //    }
            //}
        }
        private void 修改仓位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMS_Gimped.Frm.ChangeCPLocation cc = new WMS_Gimped.Frm.ChangeCPLocation();
            cc.CW = dataGridView3.SelectedRows[0].Cells["仓位"].Value.ToString();
            cc.ID = dataGridView3.SelectedRows[0].Cells["序号"].Value.ToString();
            cc.SL = dataGridView3.SelectedRows[0].Cells["数量"].Value.ToString();
            CPBH  = dataGridView3.SelectedRows[0].Cells["产品编号"].Value.ToString();
            if (cc.ShowDialog().Equals(DialogResult.OK))
           {
                DataGridView3(CPBH);
           }
        }
        private void dataGridView3_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView3.ClearSelection();
                    dataGridView3.Rows[e.RowIndex].Selected = true;
                    dataGridView3.CurrentCell = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        //导出实际库存
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Export2Excel("产品实际库存",GetDgvToTable(dataGridView2));
        }
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
        //合并显示库存数量
        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }
        private void 合并显示库存情况ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMS_Gimped.Frm.DisplayCPKC dc = WMS_Gimped.Frm.DisplayCPKC.CreateInstrance();
            dc.CPBH = dataGridView2.SelectedRows[0].Cells["产品编号"].Value.ToString();
            dc.Show();
        }
        private void dataGridView2_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView2.ClearSelection();
                    dataGridView2.Rows[e.RowIndex].Selected = true;
                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    contextMenuStrip2.Show(MousePosition.X, MousePosition.Y);
                }
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
        private void DataGridView3(string s)
        {
            tabControl2.SelectedTab = tabControl2.TabPages[1];
            SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
            para[0].Value = s;
            DataTable dttemp = SqlHelper.ExecStoredProcedureDataTable("WMS_ProductQuery2", para);
            dataGridView3.DataSource = dttemp;
            dataGridView3.Columns[0].Visible = false;
            dataGridView3.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView3.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView3.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView3.Refresh();

            #region 统计数据
            double index = 0;
            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                index = index + Convert.ToDouble(dttemp.Rows[i]["数量"].ToString());
            }
            toolStripStatusLabel3.Text = Convert.ToString(index);
            #endregion
        }
        #endregion

    }
}
