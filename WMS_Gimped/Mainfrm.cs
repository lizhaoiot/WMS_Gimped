using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading.Tasks;

namespace WMS_Gimped
{
    public partial class Mainfrm : Form
    {

        #region 定量
          public static string Dialog = null;
          public static string Status = null;
          Dictionary<int, string> Title = new Dictionary<int, string>();
          int index = 0;
        #endregion

        #region 变量
          public static string LoginName = string.Empty;
        #endregion

        #region 初始化
          public Mainfrm()
        {
            Login lg = new Login();
            lg.ShowDialog();
            if (Dialog == "OK")
            {
                InitializeComponent();
                this.WindowState = FormWindowState.Maximized;
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            }
            switch (Status)
            {
                //成品库
                case "0":
                    材料管理ToolStripMenuItem.Visible = false;
                    材料库ToolStripMenuItem.Visible = false;
                    材料ToolStripMenuItem.Visible = false;
                    人员权限ToolStripMenuItem.Visible = false;
                    日志查询ToolStripMenuItem.Visible = false;
                    break;
                //材料库
                case "1":
                    产品管理ToolStripMenuItem.Visible = false;
                    成品库ToolStripMenuItem.Visible = false;
                    产品ToolStripMenuItem.Visible = false;
                    人员权限ToolStripMenuItem.Visible = false;
                    日志查询ToolStripMenuItem.Visible = false;
                    break;
            }
            toolStripStatusLabel2.Text = LoginName;
        }
          private void Mainfrm_Load(object sender, EventArgs e)
        {
            //this.skinEngine1.SkinFile = sSkinPath + "MP10.ssk";
            toolStripStatusLabel2.Text = LoginName;

            //默认加载CNC窗体
            string formControl = "WMS_Gimped.Controls.Hello";
            GenerateForm(formControl, tabControl1, "欢迎界面");

        }

        #endregion

        #region 窗体事件
          private void 材料入库ToolStripMenuItem_Click(object sender, EventArgs e)
         {
            string formClass = "WMS_Gimped.Controls.MaterialsR";
            GenerateForm(formClass, sender, "材料入库");
         }
          private void 材料出库ToolStripMenuItem_Click(object sender, EventArgs e)
         {
            string formClass = "WMS_Gimped.Controls.MaterialsC";
            GenerateForm(formClass, sender, "材料出库");
        }
          private void 材料盘点ToolStripMenuItem_Click(object sender, EventArgs e)
         {
            string formClass = "WMS_Gimped.Controls.MaterialsP";
            GenerateForm(formClass, sender, "材料盘点");
        }
          private void 产品出库ToolStripMenuItem_Click(object sender, EventArgs e)
         {
            string formClass = "WMS_Gimped.Controls.ProductsC";
            GenerateForm(formClass, sender, "产品出库");
        }
          private void 产品入库ToolStripMenuItem_Click(object sender, EventArgs e)
         {
            string formClass = "WMS_Gimped.Controls.ProductsR";
            GenerateForm(formClass, sender, "产品入库");
        }
          private void 产品盘点ToolStripMenuItem_Click(object sender, EventArgs e)
         {
            string formClass = "WMS_Gimped.Controls.ProductsP";
             GenerateForm(formClass, sender, "产品盘点");
         }
          private void 成品库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMS_Gimped.Frm.CPKW cp = WMS_Gimped.Frm.CPKW.CreateInstrance();
            toolStripStatusLabel4.Text = "成品库基础资料";
            cp.Show();
        }
          private void 材料库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMS_Gimped.Frm.CLKW cl = WMS_Gimped.Frm.CLKW.CreateInstrance();
            toolStripStatusLabel4.Text = "材料库基础资料";
            cl.Show();
        }
          private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WMS_Gimped.Frm.ChangePwd cp = WMS_Gimped.Frm.ChangePwd.CreateInstrance();
            ERPInquire3.HelpClass.iniFileHelper ini = new ERPInquire3.HelpClass.iniFileHelper(Application.StartupPath + "/data/Config.ini");
            string name= ini.ReadString("Login", "Name", "");
            string[] s = name.Split('-');
            cp.code = s[0];
            cp.name = s[1];
            toolStripStatusLabel4.Text = "修改密码";
            cp.Show();
        }
          private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
          private void 数据校验ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
          private void 产品出入库明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string formClass = "WMS_Gimped.ERP.Product.CPKC";
            GenerateForm(formClass, sender, "ERP产品实时库存");
        }
          private void 产品出入库明细ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string formClass = "WMS_Gimped.ERP.Product.CPCRK";
            GenerateForm(formClass, sender, "ERP产品出入库明细");
        }
          private void 产品收发存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string formClass = "WMS_Gimped.ERP.Product.CPSFC";
            GenerateForm(formClass, sender, "ERP产品收发存");
        }
          private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
          {
            try
            {
                toolStripStatusLabel4.Text = tabControl1.SelectedTab.Text;
            }
            catch
            {
                toolStripStatusLabel4.Text = "";
            }
          }
        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (tabControl1.SelectedTab.Text == "欢迎界面") return;
                var keys = Title.FirstOrDefault(q => q.Value == tabControl1.SelectedTab.Text).Key;
                Title.Remove(Convert.ToUInt16(keys));
                TabPage page = tabControl1.SelectedTab;
                tabControl1.TabPages.Remove(page);
            }
        }
        private void 材料实时库存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string formClass = "WMS_Gimped.ERP.Material.WLKC";
            GenerateForm(formClass, sender, "ERP材料实时库存");
        }

        private void 材料收发存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string formClass = "WMS_Gimped.ERP.Material.WLCRK";
            GenerateForm(formClass, sender, "ERP材料出入库明细");
        }

        private void 材料收发存ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string formClass = "WMS_Gimped.ERP.Material.WLSFC";
            GenerateForm(formClass, sender, "ERP材料收发存");
        }
        private void 人员权限ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 方法
        public void GenerateForm(string form, object sender,string name)
        {
            //防止tab页重复
            foreach (KeyValuePair<int,string> st in Title)
            {
                if (st.Value.Equals(name))
                {
                    return;
                }                     
            }
            //反射生成窗体
            Control fm = (Control)Assembly.GetExecutingAssembly().CreateInstance(form);
            //设置窗体没有边框，加入到选项卡中
            TabPage p = new TabPage();
            p.Text = name;
            tabControl1.TabPages.Insert(tabControl1.SelectedIndex + 1, p);
            tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
            tabControl1.TabPages[tabControl1.SelectedIndex].ImageIndex = 0;
            p.Controls.Add(fm);
            fm.Dock = DockStyle.Fill;
            fm.Show();
            toolStripStatusLabel4.Text = name;
            Title.Add(index,name);
            index = index + 1;
        }

        #endregion

    }
}
