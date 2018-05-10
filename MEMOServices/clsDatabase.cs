using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OracleClient;

namespace MEMOServices
{
    public class clsDatabase
    {
        private OracleConnection oraConn;
        private OracleCommand oraCmd;
        private OracleDataAdapter oraAdpt;
        private OracleTransaction oraTran;

        private DataSet dsGlobal;

        public string strErrorMsg = String.Empty;


        //  <summary>
        //  Open a new oracle connectoin.
        //  </summary>
        //  <remarks></remarks>
        public void OpenConnection()
        {
            try
            {
                // Dim sConn As String = ConfigurationManager.ConnectionStrings("ConnString").ToString
                // Dim sConn As String = "user id = 'SYSTEM';password = 'SYS1234';data source = 'CHECKSCASHED'"
                string connStr = System.Configuration.ConfigurationManager.AppSettings["ConnStr"].ToString();
                strErrorMsg = "";
                oraConn = new OracleConnection(connStr);
                oraConn.Open();
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        //  <summary>
        //   This Function is used to close the existing open connectoin
        //  </summary>
        //  <remarks></remarks>
        public void CloseConnection()
        {
            try
            {
                strErrorMsg = "";
                if (!(oraConn == null))
                {
                    oraConn.Close();
                    oraConn.Dispose();
                    oraConn = null;
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        //  <summary>
        //  This function will execute the oracle command with type as Store Procedure
        //  </summary>
        //  <param name="cmd">Oracle command</param>
        //  <returns>True/False</returns>
        //  <remarks></remarks>
        public bool ProcedureExecute(OracleCommand cmd)
        {
            try
            {
                strErrorMsg = "";
                OpenConnection();
                cmd.Connection = oraConn;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
                return false;
            }
            finally
            {
                cmd.Dispose();
                oraConn.Close();
                oraConn.Dispose();
            }
        }

        //public string readoraclereader(OracleCommand cmd)
        //{
        //    try
        //    {
        //        strErrorMsg = "";
        //        OracleCommand cmd1 = new OracleCommand();
        //        System.Data.OracleClient.OracleDataReader oraReader;
        //        System.Data.OracleClient.OracleLob myClob;
        //        cmd1 = cmd;
        //        OpenConnection();
        //        cmd.Connection = oraConn;
        //        oraReader = cmd1.ExecuteReader();
        //        myClob = cmd1.Parameters["status"].Value;
        //        return myClob.Value.ToString;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //}

        ///  <summary>
        ///  Execute the oracle command and return a single value.
        ///  
        ///  </summary>
        ///  <param name="cmd">Oracle command to be executed.</param>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public bool ProcedureExecutescaler(OracleCommand cmd)
        {
            try
            {
                strErrorMsg = "";
                OpenConnection();
                cmd.Connection = oraConn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteOracleScalar();
                return true;
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
                return false;
            }
            finally
            {
                cmd.Dispose();
                oraConn.Close();
                oraConn.Dispose();
            }
        }

        ///  <summary>
        ///  This function will execute the oracle command with type as Store Procedure
        ///  </summary>
        ///  <param name="cmd">Oracle command</param>
        ///  <returns>True/False</returns>
        ///  <remarks></remarks>
        public DataSet ProcedureExecuteDataset(OracleCommand cmd)
        {
            try
            {
                strErrorMsg = "";
                dsGlobal = new DataSet();
                // If oraConn.State = ConnectionState.Closed Then
                // oraConn.Open()
                OpenConnection();
                // End If
                cmd.Connection = oraConn;
                oraAdpt = new OracleDataAdapter();
                oraAdpt.SelectCommand = cmd;
                oraAdpt.Fill(dsGlobal);
                return dsGlobal;
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
                return null;
            }
            finally
            {
                dsGlobal = null;
                cmd.Dispose();
                oraConn.Close();
                oraConn.Dispose();
            }
        }

        ///  <summary>
        ///  Executer procedure without returning any object. Return boolean variable.
        ///  </summary>
        ///  <param name="cmd">Oracle Command to be executed.</param>
        ///  <returns></returns>
        ///  <remarks></remarks>
        public bool ProcedureExecute_withoutReturn(OracleCommand cmd)
        {
            try
            {
                strErrorMsg = "";
                oraAdpt = new OracleDataAdapter(cmd.CommandText, oraConn);
                oraAdpt.SelectCommand.CommandTimeout = 0;
                oraAdpt.SelectCommand.CommandType = CommandType.StoredProcedure;
                // If oraConn.State = ConnectionState.Closed Then
                // oraConn.Open()
                OpenConnection();
                // End If
                if (!(this.oraTran == null))
                {
                    cmd.Transaction = oraTran;
                }
                try
                {
                    oraAdpt.SelectCommand.ExecuteNonQuery();
                    oraAdpt = null;
                    oraAdpt.Dispose();
                    oraConn.Close();
                    oraConn.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    strErrorMsg = ex.Message.ToString();
                    return false;
                }


            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        ///  <summary>
        ///  This function begins the Oracle transaction
        ///  </summary>
        ///  <remarks></remarks>
        public void Begin_Transaction()
        {
            try
            {
                strErrorMsg = "";
                if ((oraTran == null))
                {
                    oraTran = oraConn.BeginTransaction();
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        ///  <summary>
        ///  This function Commits the Oracle transaction
        ///  </summary>
        ///  <remarks></remarks>
        public void Commit_Transaction()
        {
            try
            {
                strErrorMsg = "";
                if (!(oraTran == null))
                {
                    oraTran.Commit();
                    oraTran = null;
                    oraConn.Close();
                    oraConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        ///  <summary>
        ///  This function RollBacks the Oracle transaction
        ///  </summary>
        ///  <remarks></remarks>
        public void RollBack_Transaction()
        {
            try
            {
                strErrorMsg = "";
                if (!(oraTran == null))
                {
                    oraTran.Rollback();
                    oraTran = null;
                    oraConn.Close();
                    oraConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        ////  <summary>
        ////  This function Disposes all the Oracle objects(Oracle command,Oracle Transaction,Oracle DataAdapter,Oracle Connection)
        ////  </summary>
        ////  <remarks></remarks>
        public void Dispose()
        {
            try
            {
                strErrorMsg = "";
                if (!(oraCmd == null))
                {
                    oraCmd.Dispose();
                    oraCmd = null;
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
            try
            {
                strErrorMsg = "";
                if (!(oraTran == null))
                {
                    oraTran.Dispose();
                    oraTran = null;
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
            try
            {
                strErrorMsg = "";
                if (!(oraAdpt == null))
                {
                    oraAdpt.Dispose();
                    oraAdpt = null;
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
            try
            {
                strErrorMsg = "";
                if (!(oraConn == null))
                {
                    oraConn.Close();
                    oraConn = null;
                }
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        ////  <summary>
        ////  Dispose the class
        ////  </summary>
        ////  <remarks></remarks>
        protected void Finalize()
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
            }
        }

        ////  <summary>
        ////  This function will execute the oracle command with type as Store Procedure
        ////  </summary>
        ////  <param name="cmd">Oracle command</param>
        ////  <returns>True/False</returns>
        ////  <remarks></remarks>
        public Int32 ProcedureExecuteCommandText(OracleCommand cmd)
        {
            try
            {
                cmd.CommandType = CommandType.Text;
                OpenConnection();
                cmd.Connection = oraConn;
                //cmd.ExecuteNonQuery();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message.ToString();
                return -1;
            }
            finally
            {
                cmd.Dispose();
                oraConn.Close();
                oraConn.Dispose();
            }
        }

        public string getSysDate()
        {
            string sysdate = "";
            //try
            //{
            //    OracleCommand cmd = new OracleCommand("CHK_GETSYSDATE");
            //    objOradb = new ClsConnectToDB();
            //    cmd.Parameters.Add(new OracleParameter("pvalue", OracleType.VarChar, 2000)).Direction = ParameterDirection.Output;
            //    if (objOradb.ProcedureExecutescaler(cmd))
            //    {
            //        sysdate = cmd.Parameters["pvalue"].Value;
            //        return sysdate;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    WriteErrMsg(("clsCommon :getsysdate()  :"
            //                    + (ex.Message + ("\r\n"
            //                    + (DateTime.Now.ToString + "\r\n")))));
            //}
            return sysdate;
        }

    }
}