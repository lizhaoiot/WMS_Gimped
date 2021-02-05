using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Maticsoft.Common;
using Maticsoft.Model;
namespace Maticsoft.BLL
{
    //WMS_CL
    public partial class WMS_CL
    {

        private readonly Maticsoft.DAL.WMS_CL dal = new Maticsoft.DAL.WMS_CL();
        public WMS_CL()
        { }

        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string ID)
        {
            return dal.Exists(ID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Maticsoft.Model.WMS_CL model)
        {
            dal.Add(model);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Maticsoft.Model.WMS_CL model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ID)
        {

            return dal.Delete(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Maticsoft.Model.WMS_CL GetModel(string ID)
        {

            return dal.GetModel(ID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Maticsoft.Model.WMS_CL GetModelByCache(string ID)
        {

            string CacheKey = "WMS_CLModel-" + ID;
            object objModel = Maticsoft.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(ID);
                    if (objModel != null)
                    {
                        int ModelCache = Maticsoft.Common.ConfigHelper.GetConfigInt("ModelCache");
                        Maticsoft.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (Maticsoft.Model.WMS_CL)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Maticsoft.Model.WMS_CL> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Maticsoft.Model.WMS_CL> DataTableToList(DataTable dt)
        {
            List<Maticsoft.Model.WMS_CL> modelList = new List<Maticsoft.Model.WMS_CL>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                Maticsoft.Model.WMS_CL model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new Maticsoft.Model.WMS_CL();
                    model.ID = dt.Rows[n]["ID"].ToString();
                    if (dt.Rows[n]["RQ"].ToString() != "")
                    {
                        model.RQ = DateTime.Parse(dt.Rows[n]["RQ"].ToString());
                    }
                    model.DW = dt.Rows[n]["DW"].ToString();
                    model.REMARK = dt.Rows[n]["REMARK"].ToString();
                    if (dt.Rows[n]["SL3"].ToString() != "")
                    {
                        model.SL3 = decimal.Parse(dt.Rows[n]["SL3"].ToString());
                    }
                    model.CLTYPE = dt.Rows[n]["CLTYPE"].ToString();
                    model.CLBH = dt.Rows[n]["CLBH"].ToString();
                    model.CLMC = dt.Rows[n]["CLMC"].ToString();
                    model.SGBH = dt.Rows[n]["SGBH"].ToString();
                    model.CW = dt.Rows[n]["CW"].ToString();
                    if (dt.Rows[n]["SL1"].ToString() != "")
                    {
                        model.SL1 = decimal.Parse(dt.Rows[n]["SL1"].ToString());
                    }
                    if (dt.Rows[n]["SL2"].ToString() != "")
                    {
                        model.SL2 = decimal.Parse(dt.Rows[n]["SL2"].ToString());
                    }
                    if (dt.Rows[n]["ZDRQ"].ToString() != "")
                    {
                        model.ZDRQ = DateTime.Parse(dt.Rows[n]["ZDRQ"].ToString());
                    }


                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }
        #endregion

    }
}