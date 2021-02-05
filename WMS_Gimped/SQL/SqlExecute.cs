/*
 * 类说明：根据对象执行SQL语句
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace WMS_Gimped.SQL
{
    /// <summary>
    /// SQL帮助类
    /// </summary>
    public class SqlHelper
    {
        public static string connectionStr = null;
        public static DataTable GetUserList(string connectionStr, string sql, SqlParameter[] prams)
        {
            DataTable result = null;
            SqlConnection sqlConnection = new SqlConnection(connectionStr);
            sqlConnection.Open();
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlCommand.Parameters.AddRange(prams);
                DataSet dataSet = new DataSet();
                DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                dbDataAdapter.SelectCommand = sqlCommand;
                dbDataAdapter.Fill(dataSet);
                result = dataSet.Tables[0];
            }
            sqlConnection.Close();
            return result;
        }
        public static SqlConnection GetConnection()
        {
            if (connectionStr == null)
            {
                return GetConnection("packet size=4096;user id=sa;pwd=;data source=192.168.0.97;persist security info=False;initial catalog=hy");
            }
            return GetConnection(connectionStr);
        }
        public static SqlConnection GetConnection(string connstr)
        {
            SqlConnection sqlConnection = null;
            sqlConnection = new SqlConnection(connstr);
            sqlConnection.Open();
            return sqlConnection;
        }
        public static void BulkCopy(DataTable src, string dest)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection);
                sqlBulkCopy.DestinationTableName = dest;
                sqlBulkCopy.WriteToServer(src);
            }
        }
        public static void BulkCopy(DataTable src, string dest, SqlConnection conn, SqlTransaction t)
        {
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, t);
            sqlBulkCopy.DestinationTableName = dest;
            sqlBulkCopy.WriteToServer(src);
        }
        public static void ExecCommand(string commtext)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(commtext, connection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
            }
        }
        public static void ExecCommand(string[] commtext)
        {
            using (SqlConnection connection = GetConnection())
            {
                for (int i = 0; i < commtext.Length; i++)
                {
                    SqlCommand sqlCommand = new SqlCommand(commtext[i], connection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                }
            }
        }
        public static int ExecCommand(string commtext, SqlParameter[] prams)
        {
            using (SqlConnection connection = GetConnection())
            {
                int num = -1;
                SqlCommand sqlCommand = new SqlCommand(commtext, connection);
                sqlCommand.CommandType = CommandType.Text;
                if (prams != null)
                {
                    sqlCommand.Parameters.AddRange(prams);
                }
                num = sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                return num;
            }
        }
        public static void ExecCommand(string commtext, SqlParameter[] prams, SqlConnection conn, SqlTransaction t)
        {
            SqlCommand sqlCommand = new SqlCommand(commtext, conn);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Transaction = t;
            if (prams != null)
            {
                sqlCommand.Parameters.AddRange(prams);
            }
            sqlCommand.ExecuteNonQuery();
        }
        public static void ExecCommand(string commtext, SqlConnection conn, SqlTransaction t)
        {
            SqlCommand sqlCommand = new SqlCommand(commtext, conn);
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Transaction = t;
            sqlCommand.ExecuteNonQuery();
        }
        public static int ExecStoredProcedure(string procedureName, SqlParameter[] prams)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 600;
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = procedureName;
                if (prams != null)
                {
                    sqlCommand.Parameters.AddRange(prams);
                }
                int result = sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                return result;
            }
        }
        public static DataTable ExecStoredProcedureDataTable(string procedureName, SqlParameter[] prams)
        {
            using (SqlConnection connection = GetConnection())
            {
                DataTable dataTable = null;
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 600;
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = procedureName;
                if (prams != null)
                {
                    sqlCommand.Parameters.AddRange(prams);
                }
                DataSet dataSet = new DataSet();
                DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                dbDataAdapter.SelectCommand = sqlCommand;
                dbDataAdapter.Fill(dataSet);
                if (dataSet.Tables.Count == 0)
                {
                    return null;
                }
                dataTable = dataSet.Tables[0];
                sqlCommand.Dispose();
                return dataTable;
            }
        }
        public static DataSet ExecStoredProcedureDataSet(string procedureName, SqlParameter[] prams)
        {
            using (SqlConnection connection = GetConnection())
            {
                DataSet dataSet = new DataSet();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandTimeout = 600;
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = procedureName;
                if (prams != null)
                {
                    sqlCommand.Parameters.AddRange(prams);
                }
                DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                dbDataAdapter.SelectCommand = sqlCommand;
                dbDataAdapter.Fill(dataSet);
                sqlCommand.Dispose();
                return dataSet;
            }
        }
        public static DataSet ExecStoredProcedureDataSet(string procedureName, SqlParameter[] prams, SqlConnection conn, SqlTransaction t)
        {
            DataSet dataSet = new DataSet();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 600;
            sqlCommand.Connection = conn;
            sqlCommand.Transaction = t;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = procedureName;
            if (prams != null)
            {
                sqlCommand.Parameters.AddRange(prams);
            }
            DbDataAdapter dbDataAdapter = new SqlDataAdapter();
            dbDataAdapter.SelectCommand = sqlCommand;
            dbDataAdapter.Fill(dataSet);
            return dataSet;
        }
        public static DataTable ExecStoredProcedureDataTable(string procedureName, SqlParameter[] prams, SqlConnection conn, SqlTransaction t)
        {
            DataTable dataTable = null;
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandTimeout = 600;
            sqlCommand.Connection = conn;
            sqlCommand.Transaction = t;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = procedureName;
            if (prams != null)
            {
                sqlCommand.Parameters.AddRange(prams);
            }
            DataSet dataSet = new DataSet();
            DbDataAdapter dbDataAdapter = new SqlDataAdapter();
            dbDataAdapter.SelectCommand = sqlCommand;
            dbDataAdapter.Fill(dataSet);
            return dataSet.Tables[0];
        }
        public static int ExecStoredProcedure(string procedureName, SqlParameter[] prams, SqlConnection conn, SqlTransaction t)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = conn;
            sqlCommand.Transaction = t;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = procedureName;
            if (prams != null)
            {
                sqlCommand.Parameters.AddRange(prams);
            }
            return sqlCommand.ExecuteNonQuery();
        }
        public static int ExecScalar(string CommText)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(CommText, connection);
                sqlCommand.CommandType = CommandType.Text;
                return (sqlCommand.ExecuteScalar() != DBNull.Value) ? ((int)sqlCommand.ExecuteScalar()) : 0;
            }
        }
        public static int ExecScalar(string CommText, SqlConnection TConn)
        {
            SqlCommand sqlCommand = new SqlCommand(CommText, TConn);
            sqlCommand.CommandType = CommandType.Text;
            return (sqlCommand.ExecuteScalar() != DBNull.Value) ? ((int)sqlCommand.ExecuteScalar()) : 0;
        }
        public static int ExecScalar(string CommText, SqlTransaction t)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(CommText, connection);
                sqlCommand.Transaction = t;
                sqlCommand.CommandType = CommandType.Text;
                return (sqlCommand.ExecuteScalar() != DBNull.Value) ? Convert.ToInt32(sqlCommand.ExecuteScalar()) : 0;
            }
        }
        public static int ExecScalar(string CommText, SqlConnection TConn, SqlTransaction t)
        {
            SqlCommand sqlCommand = new SqlCommand(CommText, TConn);
            sqlCommand.Transaction = t;
            sqlCommand.CommandType = CommandType.Text;
            return (sqlCommand.ExecuteScalar() != DBNull.Value) ? Convert.ToInt32(sqlCommand.ExecuteScalar()) : 0;
        }
        public static int ExecScalar(string CommText, SqlParameter[] prams)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand sqlCommand = new SqlCommand(CommText, connection);
                sqlCommand.CommandType = CommandType.Text;
                if (prams != null)
                {
                    sqlCommand.Parameters.AddRange(prams);
                }
                return (sqlCommand.ExecuteScalar() != DBNull.Value) ? Convert.ToInt32(sqlCommand.ExecuteScalar()) : 0;
            }
        }
        public static int ExecScalar(string CommText, SqlParameter[] prams, SqlConnection TConn)
        {
            SqlCommand sqlCommand = new SqlCommand(CommText, TConn);
            sqlCommand.CommandType = CommandType.Text;
            if (prams != null)
            {
                foreach (SqlParameter value in prams)
                {
                    sqlCommand.Parameters.Add(value);
                }
            }
            return (sqlCommand.ExecuteScalar() != DBNull.Value) ? Convert.ToInt32(sqlCommand.ExecuteScalar()) : 0;
        }
        public static DataTable ExecuteDataTable(string sql)
        {
            using (SqlConnection connection = GetConnection())
            {
                DataTable result = null;
                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                {
                    DataSet dataSet = new DataSet();
                    DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                    dbDataAdapter.SelectCommand = sqlCommand;
                    dbDataAdapter.Fill(dataSet);
                    result = dataSet.Tables[0];
                    sqlCommand.Parameters.Clear();
                }
                return result;
            }
        }
        public static DataTable ExecuteDataTable(string sql, SqlConnection conn, SqlTransaction t)
        {
            DataTable result = null;
            using (SqlCommand sqlCommand = new SqlCommand(sql, conn))
            {
                sqlCommand.Transaction = t;
                DataSet dataSet = new DataSet();
                DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                dbDataAdapter.SelectCommand = sqlCommand;
                dbDataAdapter.Fill(dataSet);
                result = dataSet.Tables[0];
                sqlCommand.Parameters.Clear();
            }
            return result;
        }
        public static DataTable ExecuteDataTable(string sql, DbParameter[] parameters)
        {
            DataTable result = null;
            using (SqlConnection connection = GetConnection())
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        sqlCommand.Parameters.AddRange(parameters);
                    }
                    sqlCommand.CommandTimeout = 600;
                    DataSet dataSet = new DataSet();
                    DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                    dbDataAdapter.SelectCommand = sqlCommand;
                    dbDataAdapter.Fill(dataSet);
                    result = dataSet.Tables[0];
                    sqlCommand.Parameters.Clear();
                }
                return result;
            }
        }
        public static DataTable ExecuteDataTable(string sql, DbParameter[] parameters, string connectionString)
        {
            DataTable result = null;
            using (SqlConnection connection = GetConnection(connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
                {
                    sqlCommand.Parameters.AddRange(parameters);
                    sqlCommand.CommandTimeout = 600;
                    DataSet dataSet = new DataSet();
                    DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                    dbDataAdapter.SelectCommand = sqlCommand;
                    dbDataAdapter.Fill(dataSet);
                    result = dataSet.Tables[0];
                    sqlCommand.Parameters.Clear();
                }
                return result;
            }
        }
        public static DataTable ExecuteDataTable(string sql, DbParameter[] parameters, SqlConnection conn, SqlTransaction t)
        {
            DataTable result = null;
            using (SqlCommand sqlCommand = new SqlCommand(sql, conn))
            {
                sqlCommand.Transaction = t;
                sqlCommand.Parameters.AddRange(parameters);
                DataSet dataSet = new DataSet();
                DbDataAdapter dbDataAdapter = new SqlDataAdapter();
                dbDataAdapter.SelectCommand = sqlCommand;
                dbDataAdapter.Fill(dataSet);
                result = dataSet.Tables[0];
                sqlCommand.Parameters.Clear();
            }
            return result;
        }
        public static void Exec4DS(string CmdText, string TableName, out DataSet DS)
        {
            using (SqlConnection selectConnection = GetConnection())
            {
                DS = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(CmdText, selectConnection);
                sqlDataAdapter.Fill(DS, TableName);
            }
        }
        public static DataSet ExecuteDataSet(string sql, SqlParameter[] prams)
        {
            using (SqlConnection connection = GetConnection())
            {
                DataSet dataSet = new DataSet();
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.CommandType = CommandType.Text;
                if (prams != null)
                {
                    sqlCommand.Parameters.AddRange(prams);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataSet);
                sqlCommand.Parameters.Clear();
                return dataSet;
            }
        }
        public static void ExecXDS(string[] sqlCmds, string[] TableNames, out DataSet DS)
        {
            using (SqlConnection selectConnection = GetConnection())
            {
                DS = new DataSet();
                for (int i = 0; i < sqlCmds.Length; i++)
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCmds[i], selectConnection);
                    sqlDataAdapter.Fill(DS, TableNames[i]);
                }
            }
        }
        public static void ExecXDS(string[] sqlCmds, string[] TableNames, out DataSet DS, string connectionString)
        {
            using (SqlConnection selectConnection = GetConnection(connectionString))
            {
                DS = new DataSet();
                for (int i = 0; i < sqlCmds.Length; i++)
                {
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCmds[i], selectConnection);
                    sqlDataAdapter.Fill(DS, TableNames[i]);
                }
            }
        }
        public static void ExecXDS(string[] sqlCmds, string[] TableNames, SqlParameter[] prams, out DataSet DS)
        {
            using (SqlConnection connection = GetConnection())
            {
                DS = new DataSet();
                for (int i = 0; i < sqlCmds.Length; i++)
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlCmds[i]);
                    sqlCommand.Parameters.Add(prams[i]);
                    sqlCommand.Connection = connection;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(DS, TableNames[i]);
                }
            }
        }
        public static void ExecXDS(string[] sqlCmds, string[] TableNames, List<SqlParameter[]> prams, out DataSet DS)
        {
            using (SqlConnection connection = GetConnection())
            {
                DS = new DataSet();
                for (int i = 0; i < sqlCmds.Length; i++)
                {
                    SqlCommand sqlCommand = new SqlCommand(sqlCmds[i]);
                    SqlParameter[] values = prams[i];
                    sqlCommand.Parameters.AddRange(values);
                    sqlCommand.Connection = connection;
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(DS, TableNames[i]);
                }
            }
        }
        public static DateTime GetDateTimeFromSQL(SqlConnection conn, SqlTransaction t)
        {
            string cmdText = "select getdate()";
            SqlCommand sqlCommand = new SqlCommand(cmdText, conn);
            sqlCommand.Transaction = t;
            return (DateTime)sqlCommand.ExecuteScalar();
        }
        public static DateTime GetDateTimeFromSQL()
        {
            using (SqlConnection connection = GetConnection())
            {
                string cmdText = "select getdate()";
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);
                return (DateTime)sqlCommand.ExecuteScalar();
            }
        }
        public static DataTable Join(DataTable left, DataTable right, DataColumn[] leftCols, DataColumn[] rightCols, bool includeLeftJoin, bool includeRightJoin)
        {
            DataTable dataTable = new DataTable("JoinResult");
            using (DataSet dataSet = new DataSet())
            {
                dataSet.Tables.AddRange(new DataTable[2]
                {
                left.Copy(),
                right.Copy()
                });
                DataColumn[] array = new DataColumn[leftCols.Length];
                for (int i = 0; i < leftCols.Length; i++)
                {
                    array[i] = dataSet.Tables[0].Columns[leftCols[i].ColumnName];
                }
                DataColumn[] array2 = new DataColumn[rightCols.Length];
                for (int i = 0; i < rightCols.Length; i++)
                {
                    array2[i] = dataSet.Tables[1].Columns[rightCols[i].ColumnName];
                }
                for (int i = 0; i < left.Columns.Count; i++)
                {
                    dataTable.Columns.Add(left.Columns[i].ColumnName, left.Columns[i].DataType);
                }
                for (int i = 0; i < right.Columns.Count; i++)
                {
                    string text = right.Columns[i].ColumnName;
                    while (dataTable.Columns.Contains(text))
                    {
                        text += "_2";
                    }
                    dataTable.Columns.Add(text, right.Columns[i].DataType);
                }
                DataRelation relation = new DataRelation("rLeft", array, array2, false);
                dataSet.Relations.Add(relation);
                dataTable.BeginLoadData();
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    DataRow[] childRows = row.GetChildRows(relation);
                    if (childRows != null && childRows.Length > 0)
                    {
                        object[] itemArray = row.ItemArray;
                        DataRow[] array3 = childRows;
                        foreach (DataRow dataRow2 in array3)
                        {
                            object[] itemArray2 = dataRow2.ItemArray;
                            object[] array4 = new object[itemArray.Length + itemArray2.Length];
                            Array.Copy(itemArray, 0, array4, 0, itemArray.Length);
                            Array.Copy(itemArray2, 0, array4, itemArray.Length, itemArray2.Length);
                            dataTable.LoadDataRow(array4, true);
                        }
                    }
                    else if (includeLeftJoin)
                    {
                        object[] itemArray = row.ItemArray;
                        object[] array4 = new object[itemArray.Length];
                        Array.Copy(itemArray, 0, array4, 0, itemArray.Length);
                        dataTable.LoadDataRow(array4, true);
                    }
                }
                if (includeRightJoin)
                {
                    DataRelation relation2 = new DataRelation("rRight", array2, array, false);
                    dataSet.Relations.Add(relation2);
                    foreach (DataRow row2 in dataSet.Tables[1].Rows)
                    {
                        DataRow[] childRows = row2.GetChildRows(relation2);
                        if (childRows == null || childRows.Length == 0)
                        {
                            object[] itemArray = row2.ItemArray;
                            object[] array4 = new object[dataTable.Columns.Count];
                            Array.Copy(itemArray, 0, array4, array4.Length - itemArray.Length, itemArray.Length);
                            dataTable.LoadDataRow(array4, true);
                        }
                    }
                }
                dataTable.EndLoadData();
            }
            return dataTable;
        }
    }

    /// <summary>
    /// SQL具体业务执行
    /// </summary>
    public class SqlExecute
    {

        #region  数据连接字符串
        private void Init()
        {
            SqlHelper.connectionStr = GetSQLConnect();
        }
        public static string GetSQLConnect()
        {
            ERPInquire3.HelpClass.iniFileHelper myIni = new ERPInquire3.HelpClass.iniFileHelper(Environment.CurrentDirectory + "/data/Config.ini");
            string IP = myIni.IniReadValue("SqlConnect", "IP");
            string DB = myIni.IniReadValue("SqlConnect", "DB");
            string Uid = myIni.IniReadValue("SqlConnect", "Uid");
            string Pwd = myIni.IniReadValue("SqlConnect", "Pwd");
            return "server=" + IP + ";uid=" + Uid + ";pwd=" + Pwd + ";Trusted_Connection=no;database=" + DB + "";
        }
        #endregion

        #region 产品
        //产品分盘入库
        public static int CPInsert(Model.CP cp)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string ID = cp.Id;
            string CPTYPE = cp.Cptype;
            string CPBH = cp.Cpbh;
            string CPMC = cp.Cpmc;
            string BDBH = cp.Bdbh;
            string SCBH = cp.Scbh;
            string CW = cp.Cw;
            long SL1 = cp.Sl1;
            long SL2 = cp.Sl2;
            long SL3 = cp.Sl3;
            long DDSL = cp.Ddsl;
            DateTime ZDRQ = cp.Zdrq;
            DateTime RQ = cp.Rq;
            string DW = cp.Dw;
            string REMARK = cp.Remark;
            string TYDH = cp.TYDH;
            string sqlstr = "INSERT INTO  WMS_CP (ID,BDBH,CPTYPE,CPBH,CPMC,SCBH,CW,SL1,SL2,SL3,DDSL,ZDRQ,RQ,DW,TYDH,REMARK)";
            sqlstr += "  VALUES('" + ID + "','" +BDBH + "','" + CPTYPE + "','" + CPBH + "','" + CPMC + "','" + SCBH + "','" + CW + "'";
            sqlstr += ",'" + SL1 + "','" + SL2 + "','" + SL3 + "','" + DDSL + "','" + ZDRQ + "','" + RQ + "','" + DW + "','" + TYDH + "','" + REMARK + "')";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        //产品出库
        public static int CPOutsert(Model.CP cp)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string ID = cp.Id;
            string CPTYPE = cp.Cptype;
            string CPBH = cp.Cpbh;
            string CPMC = cp.Cpmc;
            string BDBH = cp.Bdbh;
            string SCBH = cp.Scbh;
            string CW = cp.Cw;
            long SL1 = cp.Sl1;
            long SL2 = cp.Sl2;
            long DDSL = cp.Ddsl;
            DateTime ZDRQ = cp.Zdrq;
            DateTime RQ = cp.Rq;
            string DW = cp.Dw;
            string REMARK = cp.Remark;
            string TYDH = cp.TYDH;
            string sqlstr = "INSERT INTO  WMS_CP (ID,CPTYPE,CPBH,CPMC,BDBH,SCBH,CW,SL1,SL2,DDSL,ZDRQ,RQ,DW,TYDH,REMARK)";
            sqlstr += "  VALUES('" + ID + "','" + CPTYPE + "','" + CPBH + "','" + CPMC + "','" + BDBH + "','" + SCBH + "','" + CW + "'";
            sqlstr += ",'" + SL1 + "','" + SL2 + "','" + DDSL + "','" + ZDRQ + "','" + RQ + "','" + DW + "','" + TYDH + "','" + REMARK + "')";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }

        }

        #endregion

        #region 材料
        //材料分盘入库
        public static int CLInsert(Model.CL cl)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string ID = cl.Id;
            string CLTYPE = cl.Cltype;
            string CLBH = cl.Clbh;
            string CLMC = cl.Clmc;
            string SGBH = cl.Sgbh;
            string CW = cl.Cw;
            long SL1 = cl.Sl1;
            long SL2 = cl.Sl2;
            DateTime ZDRQ = cl.Zdrq;
            DateTime RQ = cl.Rq;
            string DW = cl.Dw;
            string REMARK = cl.Remark;
            string sqlstr = "INSERT INTO  WMS_CL(ID,CLTYPE,CLBH,CLMC,SGBH,CW,SL1,SL2,ZDRQ,RQ,DW,REMARK)";
            sqlstr += "  VALUES('" + ID + "','" + CLTYPE + "','" + CLBH + "','" + CLMC + "','" + SGBH + "','" + CW + "'";
            sqlstr += ",'" + SL1 + "','" + SL2 + "','" + ZDRQ + "'.'" + RQ + "','" + DW + "','" + REMARK + "')";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        //材料出库


        #endregion

        #region 用户注册
        public static int UsersInsert(Model.Users us)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string ID = us.Id;
            string UsersName = us.UserName;
            string Password = us.Password;
            string Jobs = us.Jobs;
            string sqlstr = "INSERT INTO  WMS_Users (ID,UserName,Password,Jobs)";
            sqlstr += "  VALUES('" + ID + "','" + UsersName + "','" + Password + "','" + Jobs + "')";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 成品库位
        public static int CPKWInsert(Model.CPKW ck)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string sqlstr = "INSERT INTO WMS_M_CPKW (ID,CPKW,CPKWName) VALUES ('" + ck.Id + "','" + ck.Cpkw + "','" + ck.CPKWName + "')";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 成品货位
        public static int CPHWInsert(Model.CPHW ch)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string sqlstr = "INSERT INTO WMS_CPHW (ID,GoodName,StoreName,Finalize,FLAG) VALUES ('" + ch.Id + "','" + ch.GoodName + "','" + ch.StoreName + "','',0)";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 材料库位
        public static int CLKWInsert(Model.CLKW ck)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string sqlstr = "INSERT INTO WMS_M_CLKW (ID,CLKW,CLKWName) VALUES ('" + ck.Id + "','" + ck.Cpkw + "','" + ck.CPKWName + "')";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 材料货位
        public static int CLHWInsert(Model.CLHW ch)
        {
            SqlHelper.connectionStr = GetSQLConnect();
            string sqlstr = "INSERT INTO WMS_CLHW (ID,MaterialName,StoreName,Finalize,FLAG) VALUES ('" + ch.Id + "','" + ch.MterialName + "','" + ch.StoreName + "','',0)";
            try
            {
                SqlHelper.ExecCommand(sqlstr);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region 反射获取对象属性值
        private static string GetObjectPropertyValue<T>(T t, string propertyname)
        {
            Type type = typeof(T);
            PropertyInfo property = type.GetProperty(propertyname);
            if (property == null) return string.Empty;
            object o = property.GetValue(t, null);
            if (o == null) return string.Empty;
            return o.ToString();
        }
        #endregion

    }


}
