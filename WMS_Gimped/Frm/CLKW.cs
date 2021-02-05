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
    public partial class CLKW : Form
    {
        #region 定量
        private static CLKW frm = null;
        #endregion

        #region 、变量

        #endregion

        #region 初始化
        public CLKW()
         {
             InitializeComponent();
             this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }
        public static CLKW CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new CLKW();
            }
            return frm;
        }
        #endregion

        #region 窗体事件
        //材料库位保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CheckDataIntegrity() == true)
            {
                Model.CLKW ck = new Model.CLKW();
                ck.Id = Guid.NewGuid().ToString();
                ck.Cpkw = textBox1.Text;
                ck.CPKWName = textBox2.Text;
                //检测库位代码和库位名称时候重复
                if (CheckLocationCodeRepeat(textBox1.Text))
                {
                    MessageBox.Show("库位代码重复", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (CheckLocationNameRepeat(textBox2.Text))
                {
                    MessageBox.Show("库位名称重复", "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (SQL.SqlExecute.CLKWInsert(ck) == 0)
                {
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("保存失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("所填数据必须为非空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //材料货位保存
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (CheckDataIntegrity1() == true)
            {
                Model.CLHW ch = new Model.CLHW();
                ch.Id = Guid.NewGuid().ToString();
                ch.MterialName = textBox5.Text;
                ch.StoreName = textBox3.Text;
                if (SQL.SqlExecute.CLHWInsert(ch) == 0)
                {
                    MessageBox.Show("保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("保存失败", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("所填数据必须为非空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //查询   
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            SqlParameter[] para ={
                new SqlParameter("@DAH",SqlDbType.Text)};
            para[0].Value = toolStripTextBox1.Text;
            dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CLHWJCZLQuery", para);
            dataGridView1.DataSource = dt1;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView1.Refresh();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = SqlHelper.ExecuteDataTable("SELECT CLKWName FROM WMS_M_CLKW WHERE CLKW='" + textBox3.Text + "'");
                textBox4.Text = dt1.Rows[0][0].ToString();
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region 方法
        private bool CheckDataIntegrity()
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool CheckDataIntegrity1()
        {
            if (textBox3.Text.Equals("") || textBox5.Text.Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //检测库位代码是否重复
        private bool CheckLocationCodeRepeat(string s)
        {
            DataTable dttemp = SqlHelper.ExecuteDataTable("SELECT * FROM WMS_M_CLKW WHERE CLKW='" + s + "'");
            if (dttemp.Rows.Count.Equals(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //检测库位名称是否重复
        private bool CheckLocationNameRepeat(string s)
        {
            DataTable dttemp = SqlHelper.ExecuteDataTable("SELECT * FROM WMS_M_CLKW WHERE CLKWName='" + s + "'");
            if (dttemp.Rows.Count.Equals(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
