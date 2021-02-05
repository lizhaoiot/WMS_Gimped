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
    public partial class MaterialOutbound : Form
    {
        #region 定量
        private static MaterialOutbound frm = null;
        DataTable dt = new DataTable();
        #endregion

        #region 变量
        //表单编号
        public string BDBH = string.Empty;
        //采购编号
        public string CGDH = string.Empty;
        //材料编号
        public string CLBH = string.Empty;
        //材料名称
        public string CLMC = string.Empty;
        //申请数量
        public string SQSL = string.Empty;
        //采购数量
        public string CGSL = string.Empty;
        //单位名称
        public string DWMC = string.Empty;
        #endregion

        #region 初始化
        public MaterialOutbound()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            SqlHelper.connectionStr = GetSQLConnect();
        }
        public static MaterialOutbound CreateInstrance()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new MaterialOutbound();
            }
            return frm;
        }

        private void MaterialOutbound_Load(object sender, EventArgs e)
        {
            #region 控件初始化
              textBox1.Text = BDBH;
              textBox2.Text = CGDH;
              textBox3.Text = CLBH;
              textBox4.Text = CLMC;
              textBox5.Text = SQSL;
              textBox6.Text = DWMC;
              textBox7.Text = CGSL;
            #endregion

            #region 查找数据
              SqlParameter[] para = { new SqlParameter("@DAH", SqlDbType.Text) };
              para[0].Value = CLBH;
              dt = SqlHelper.ExecStoredProcedureDataTable("WMS_CPCK3", para);
              dataGridView1.DataSource = dt;
              dataGridView1.Columns[5].Visible = false;
              dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightCyan;
              dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
              dataGridView1.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
              dataGridView1.Refresh();
            #endregion
        }

        #endregion

        #region 窗体事件
        //确定
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
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
        #endregion

    }
}
