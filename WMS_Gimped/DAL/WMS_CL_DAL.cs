using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Maticsoft.DBUtility;
namespace Maticsoft.DAL
{
    //WMS_CL
    public partial class WMS_CL
    {

        public bool Exists(string ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WMS_CL");
            strSql.Append(" where ");
            strSql.Append(" ID = @ID  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.VarChar,64)           };
            parameters[0].Value = ID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Maticsoft.Model.WMS_CL model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WMS_CL(");
            strSql.Append("ID,RQ,DW,REMARK,SL3,CLTYPE,CLBH,CLMC,SGBH,CW,SL1,SL2,ZDRQ");
            strSql.Append(") values (");
            strSql.Append("@ID,@RQ,@DW,@REMARK,@SL3,@CLTYPE,@CLBH,@CLMC,@SGBH,@CW,@SL1,@SL2,@ZDRQ");
            strSql.Append(") ");

            SqlParameter[] parameters = {
                        new SqlParameter("@ID", SqlDbType.VarChar,64) ,
                        new SqlParameter("@RQ", SqlDbType.DateTime) ,
                        new SqlParameter("@DW", SqlDbType.VarChar,100) ,
                        new SqlParameter("@REMARK", SqlDbType.VarChar,999) ,
                        new SqlParameter("@SL3", SqlDbType.Decimal,9) ,
                        new SqlParameter("@CLTYPE", SqlDbType.VarChar,10) ,
                        new SqlParameter("@CLBH", SqlDbType.VarChar,100) ,
                        new SqlParameter("@CLMC", SqlDbType.VarChar,100) ,
                        new SqlParameter("@SGBH", SqlDbType.VarChar,100) ,
                        new SqlParameter("@CW", SqlDbType.VarChar,100) ,
                        new SqlParameter("@SL1", SqlDbType.Decimal,9) ,
                        new SqlParameter("@SL2", SqlDbType.Decimal,9) ,
                        new SqlParameter("@ZDRQ", SqlDbType.DateTime)

            };

            parameters[0].Value = model.ID;
            parameters[1].Value = model.RQ;
            parameters[2].Value = model.DW;
            parameters[3].Value = model.REMARK;
            parameters[4].Value = model.SL3;
            parameters[5].Value = model.CLTYPE;
            parameters[6].Value = model.CLBH;
            parameters[7].Value = model.CLMC;
            parameters[8].Value = model.SGBH;
            parameters[9].Value = model.CW;
            parameters[10].Value = model.SL1;
            parameters[11].Value = model.SL2;
            parameters[12].Value = model.ZDRQ;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Maticsoft.Model.WMS_CL model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WMS_CL set ");

            strSql.Append(" ID = @ID , ");
            strSql.Append(" RQ = @RQ , ");
            strSql.Append(" DW = @DW , ");
            strSql.Append(" REMARK = @REMARK , ");
            strSql.Append(" SL3 = @SL3 , ");
            strSql.Append(" CLTYPE = @CLTYPE , ");
            strSql.Append(" CLBH = @CLBH , ");
            strSql.Append(" CLMC = @CLMC , ");
            strSql.Append(" SGBH = @SGBH , ");
            strSql.Append(" CW = @CW , ");
            strSql.Append(" SL1 = @SL1 , ");
            strSql.Append(" SL2 = @SL2 , ");
            strSql.Append(" ZDRQ = @ZDRQ  ");
            strSql.Append(" where ID=@ID  ");

            SqlParameter[] parameters = {
                        new SqlParameter("@ID", SqlDbType.VarChar,64) ,
                        new SqlParameter("@RQ", SqlDbType.DateTime) ,
                        new SqlParameter("@DW", SqlDbType.VarChar,100) ,
                        new SqlParameter("@REMARK", SqlDbType.VarChar,999) ,
                        new SqlParameter("@SL3", SqlDbType.Decimal,9) ,
                        new SqlParameter("@CLTYPE", SqlDbType.VarChar,10) ,
                        new SqlParameter("@CLBH", SqlDbType.VarChar,100) ,
                        new SqlParameter("@CLMC", SqlDbType.VarChar,100) ,
                        new SqlParameter("@SGBH", SqlDbType.VarChar,100) ,
                        new SqlParameter("@CW", SqlDbType.VarChar,100) ,
                        new SqlParameter("@SL1", SqlDbType.Decimal,9) ,
                        new SqlParameter("@SL2", SqlDbType.Decimal,9) ,
                        new SqlParameter("@ZDRQ", SqlDbType.DateTime)

            };

            parameters[0].Value = model.ID;
            parameters[1].Value = model.RQ;
            parameters[2].Value = model.DW;
            parameters[3].Value = model.REMARK;
            parameters[4].Value = model.SL3;
            parameters[5].Value = model.CLTYPE;
            parameters[6].Value = model.CLBH;
            parameters[7].Value = model.CLMC;
            parameters[8].Value = model.SGBH;
            parameters[9].Value = model.CW;
            parameters[10].Value = model.SL1;
            parameters[11].Value = model.SL2;
            parameters[12].Value = model.ZDRQ;
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WMS_CL ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.VarChar,64)           };
            parameters[0].Value = ID;


            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Maticsoft.Model.WMS_CL GetModel(string ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID, RQ, DW, REMARK, SL3, CLTYPE, CLBH, CLMC, SGBH, CW, SL1, SL2, ZDRQ  ");
            strSql.Append("  from WMS_CL ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.VarChar,64)           };
            parameters[0].Value = ID;


            Maticsoft.Model.WMS_CL model = new Maticsoft.Model.WMS_CL();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            if (ds.Tables[0].Rows.Count > 0)
            {
                model.ID = ds.Tables[0].Rows[0]["ID"].ToString();
                if (ds.Tables[0].Rows[0]["RQ"].ToString() != "")
                {
                    model.RQ = DateTime.Parse(ds.Tables[0].Rows[0]["RQ"].ToString());
                }
                model.DW = ds.Tables[0].Rows[0]["DW"].ToString();
                model.REMARK = ds.Tables[0].Rows[0]["REMARK"].ToString();
                if (ds.Tables[0].Rows[0]["SL3"].ToString() != "")
                {
                    model.SL3 = decimal.Parse(ds.Tables[0].Rows[0]["SL3"].ToString());
                }
                model.CLTYPE = ds.Tables[0].Rows[0]["CLTYPE"].ToString();
                model.CLBH = ds.Tables[0].Rows[0]["CLBH"].ToString();
                model.CLMC = ds.Tables[0].Rows[0]["CLMC"].ToString();
                model.SGBH = ds.Tables[0].Rows[0]["SGBH"].ToString();
                model.CW = ds.Tables[0].Rows[0]["CW"].ToString();
                if (ds.Tables[0].Rows[0]["SL1"].ToString() != "")
                {
                    model.SL1 = decimal.Parse(ds.Tables[0].Rows[0]["SL1"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SL2"].ToString() != "")
                {
                    model.SL2 = decimal.Parse(ds.Tables[0].Rows[0]["SL2"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ZDRQ"].ToString() != "")
                {
                    model.ZDRQ = DateTime.Parse(ds.Tables[0].Rows[0]["ZDRQ"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM WMS_CL ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM WMS_CL ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }


    }
}

