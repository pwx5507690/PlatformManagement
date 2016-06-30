using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public class DBHerper
    {
        public static readonly Database dbTp;

        private static IList<DbParameter> listParameter = new List<DbParameter>();
         static DBHerper()
        {
            var factory = new DatabaseProviderFactory();
            dbTp = factory.Create("connectionStr");
        }
        /// <summary>
        /// Execute a single SQL, return the value.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object ExecuteScalarTp(string sql)
        {
            var cm = dbTp.GetSqlStringCommand(sql);
            object ret = dbTp.ExecuteScalar(cm);
            return ret;
        }

        public static object ExecuteScalarTp(string sql, params DbParameter[] parameters)
        {
            var cm = dbTp.GetSqlStringCommand(sql);
            PrepareCommand(cm, parameters, dbTp);
            object ret = dbTp.ExecuteScalar(cm);
            return ret;
        }

        /// <summary>
        /// Execute SQL, return the DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReaderTp(string sql)
        {
            var cm = dbTp.GetSqlStringCommand(sql);
            return dbTp.ExecuteReader(cm);

        }
        /// <summary>
        /// Execute SQL, return the DataReader with the Parameters
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReaderTp(string sql, params DbParameter[] parameters)
        {
            var cm = dbTp.GetSqlStringCommand(sql);
            PrepareCommand(cm, parameters, dbTp);
            return dbTp.ExecuteReader(cm);
        }
        /// <summary>
        /// Execute the SQL, return the dataset
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetTp(string sql)
        {
            DbCommand cm = dbTp.GetSqlStringCommand(sql);
            return dbTp.ExecuteDataSet(cm);
        }

        /// <summary>
        /// Execute the SQL with parameters, return the dataset
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetTp(string sql, params DbParameter[] parameters)
        {
            var cm = dbTp.GetSqlStringCommand(sql);
            PrepareCommand(cm, parameters, dbTp);
            return dbTp.ExecuteDataSet(cm);
        }
        /// <summary>
        /// Execute the Store Procedure, return the dataset
        /// </summary>
        /// <param name="spName"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSetStoreProcedureTp(string spName, params DbParameter[] parameters)
        {
            DbCommand cm = dbTp.GetStoredProcCommand(spName);
            if (parameters != null && parameters.Length > 0)
            {
                PrepareCommand(cm, parameters, dbTp);
            }
            return dbTp.ExecuteDataSet(cm);
        }

        /// <summary>
        /// Execute the SQL, return the rows count.
        /// </summary>
        /// <returns></returns>
        public static int ExecuteNonQueryTp(string sql)
        {
            DbCommand cm = dbTp.GetSqlStringCommand(sql);
            return dbTp.ExecuteNonQuery(cm);
        }
        /// <summary>
        /// Execute the SQL with parameters, return the rows count.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQueryTp(string sql, params DbParameter[] parameters)
        {
            DbCommand cm = dbTp.GetSqlStringCommand(sql);
            PrepareCommand(cm, parameters, dbTp);
            return dbTp.ExecuteNonQuery(cm);
        }

        /// <summary>
        /// Execute the SQL store procedurem, return the return value.
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parameters"></param>
        /// <param name="outPutColumnName"></param>
        /// <returns></returns>
        public static object ExecuteStoreProcedureTp(string procName, string outPutColumnName, params DbParameter[] parameters)
        {
            DbCommand cm = dbTp.GetStoredProcCommand(procName);
            PrepareCommand(cm, parameters, dbTp);
            int result = dbTp.ExecuteNonQuery(cm);
            if (!string.IsNullOrEmpty(outPutColumnName))
            {
                object ret = cm.Parameters[outPutColumnName].Value;
                return ret;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Execute the SQL statement with the Tansaction
        /// </summary>
        /// <param name="SQLStringList"></param>
        public static void ExecuteSqlTranTp(ArrayList SQLStringList)
        {
            DbConnection cn = dbTp.CreateConnection();
            DbTransaction tx = cn.BeginTransaction();
            DbCommand cm = new SqlCommand();
            cm.Connection = cn;
            cm.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cm.CommandText = strsql;
                        cm.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (System.Data.OleDb.OleDbException E)
            {
                tx.Rollback();
                throw new Exception(E.Message);
            }

        }

        public static void AddParameter(string paramName, int paramSize, DbType paramType, ParameterDirection direction, object value, Database database)
        {

            DbProviderFactory fac = database.DbProviderFactory;
            DbParameter param = fac.CreateParameter();
            param.DbType = paramType;
            param.ParameterName = paramName;
            param.Size = paramSize;
            param.Value = value;
            param.Direction = direction;
            listParameter.Add(param);
        }


        /// <summary>
        /// Prepare the parameters for the DB command
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="parameters"></param>
        /// <param name="database"></param>
        private static void PrepareCommand(DbCommand cm, DbParameter[] parameters, Database database)
        {
            if (listParameter != null)
            {
                listParameter.Clear();
            }
            foreach (DbParameter p in parameters)
            {
                AddParameter(p.ParameterName, p.Size, p.DbType, p.Direction, p.Value, database);
            }
            cm.Parameters.AddRange(listParameter.ToArray());
        }

    }
}
