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
    public partial class CPKW : Form
    {
        #region 定量
        private static CPKW frm = null;
        #endregion

        #region 变量

        #endregion

        #region 初始化
        public CPKW()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }
        public static CPKW CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new CPKW();
            }
            return frm;
        }
        #endregion

        #region 窗体事件
        //成品库位保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CheckDataIntegrity() == true)
            {
                Model.CPKW ck = new Model.CPKW();
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
                if (SQL.SqlExecute.CPKWInsert(ck) == 0)
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

        //成品货位保存
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (CheckDataIntegrity1() == true)
            {
                Model.CPHW ch = new Model.CPHW();
                ch.Id = Guid.NewGuid().ToString();
                ch.GoodName = textBox5.Text;
                ch.StoreName = textBox3.Text;
                if (SQL.SqlExecute.CPHWInsert(ch) == 0)
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
            dt1 = SqlHelper.ExecStoredProcedureDataTable("WMS_CPHWJCZLQuery", para);
            dataGridView1.DataSource = dt1;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Gainsboro;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView1.Refresh();
        }
        //成品货位输入值变化
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = SqlHelper.ExecuteDataTable("SELECT CPKWName FROM WMS_M_CPKW WHERE CPKW='" + textBox3.Text + "'");
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
            DataTable dttemp = SqlHelper.ExecuteDataTable("SELECT * FROM WMS_M_CPKW WHERE CPKW='"+s+"'");
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
            DataTable dttemp = SqlHelper.ExecuteDataTable("SELECT * FROM WMS_M_CPKW WHERE CPKWName='" + s + "'");
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
