using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using System.IO;
using System.Data;
using System.Management;
using System.Net.Mail;
using System.Drawing;

namespace MEMOServices
{
    public class ClsData
    {
        clsDatabase ObjOrdDb = new clsDatabase();
        DataSet dsInformation = new DataSet();

        #region "MEMO"

        #region ClsAdmin

        public DataSet GetEmployeeList(long MoGrpID)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("MO_GETEMPLOYEELIST");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("MoGrp", OracleType.Number)).Value = MoGrpID;
                oraCmd.Parameters.Add(new OracleParameter("CwOut", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public DataSet GetAllGroups()
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_GETALLGROUPS");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pSERVICE", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public bool UpdateEmpPasscode(long MoID, long EmpId, string Passcode, string Flg)
        {
            try
            {
                clsDatabase ObjOrdDb = new clsDatabase();
                OracleCommand oraCmd = new OracleCommand("MO_UPDATEPASSCODE");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("MoPasscode", OracleType.VarChar, 2000)).Value = Passcode.ToString().Trim();
                oraCmd.Parameters.Add(new OracleParameter("ActiveFlg", OracleType.VarChar, 2000)).Value = Flg.ToString().Trim();
                oraCmd.Parameters.Add(new OracleParameter("Eid", OracleType.Number)).Value = EmpId;
                oraCmd.Parameters.Add(new OracleParameter("MoID", OracleType.Number)).Value = MoID;
                clsDatabase objOradb = new clsDatabase();
                try
                {
                    if ((ObjOrdDb.ProcedureExecutescaler(oraCmd) == true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    //clsCommon.WriteError(ex.Message);
                    return false;
                }
                finally
                {
                    oraCmd.Dispose();
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion        

        #region "ClsLoadUnloadMO"

        public bool LoadMEMO(int StoreID, int StartSeq, int EndSeq, int CurrentSeq, int StartChkNo, int EndChkNo, int NumberOfMemoLeft, string IsProcessIOG, int CreatedBy)
        {
            OracleCommand oraCmd = new OracleCommand("CW_MEMO_LOADMEMO");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("PSTOREID", OracleType.Number)).Value = StoreID;
            oraCmd.Parameters.Add(new OracleParameter("PSTARTSEQ", OracleType.Number)).Value = StartSeq;
            oraCmd.Parameters.Add(new OracleParameter("PENDSEQ", OracleType.Number)).Value = EndSeq;
            oraCmd.Parameters.Add(new OracleParameter("PCURRENTSEQ", OracleType.Number)).Value = CurrentSeq;
            oraCmd.Parameters.Add(new OracleParameter("PSTARTCHKNO", OracleType.Number)).Value = StartChkNo;
            oraCmd.Parameters.Add(new OracleParameter("PENDCHKNO", OracleType.Number)).Value = EndChkNo;
            oraCmd.Parameters.Add(new OracleParameter("PNOOFMEMOLEFT", OracleType.Number)).Value = NumberOfMemoLeft;
            oraCmd.Parameters.Add(new OracleParameter("PISPROCESSIOG", OracleType.VarChar, 2000)).Value = IsProcessIOG.ToString().Trim();
            oraCmd.Parameters.Add(new OracleParameter("PCREATEDBY", OracleType.Number)).Value = CreatedBy;
            try
            {
                clsDatabase objOradb = new clsDatabase();
                if (objOradb.ProcedureExecute(oraCmd) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon.WriteErrMsg(ex.Message);
            }
        }

        #endregion

        #region "ClsLogin"

        public DataSet Login_RegisterStatus(string UserName, string Password, string SysDt, int StoreID, int StationID)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_REGISTERSTATUS");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.Number)).Value = System.Convert.ToInt32(UserName.ToString().Trim());
                oraCmd.Parameters.Add(new OracleParameter("pEMPPASS", OracleType.VarChar, 2000)).Value = Password.ToString().Trim();
                oraCmd.Parameters.Add(new OracleParameter("pSTATIONID", OracleType.Number)).Value = StationID;
                oraCmd.Parameters.Add(new OracleParameter("pSTDT", OracleType.VarChar)).Value = SysDt;
                oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
                oraCmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = 1;
                oraCmd.Parameters.Add(new OracleParameter("pCW", OracleType.Cursor)).Direction = ParameterDirection.Output;
                oraCmd.Parameters.Add(new OracleParameter("pCWSTATION", OracleType.Cursor)).Direction = ParameterDirection.Output;
                oraCmd.Parameters.Add(new OracleParameter("pCWEMP", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public string Login_Employee(string UserName, string Password, int Station, string SysDt)
        {
            try
            {
                string StrMsg = "";
                string Code = "";

                OracleCommand oraCmd = new OracleCommand("CHK_EMPLOGIN");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.VarChar, 2000)).Value = UserName.ToString().Trim();
                oraCmd.Parameters.Add(new OracleParameter("EMPPASS", OracleType.VarChar, 2000)).Value = Password.ToString().Trim();
                oraCmd.Parameters.Add(new OracleParameter("EMPSTATION", OracleType.Number)).Value = Station;
                oraCmd.Parameters.Add(new OracleParameter("LOGINDT", OracleType.DateTime)).Value = SysDt;
                oraCmd.Parameters.Add(new OracleParameter("STATUSTEXT", OracleType.VarChar, 2000)).Direction = ParameterDirection.Output;
                oraCmd.Parameters.Add(new OracleParameter("STATUSCODE", OracleType.Number)).Direction = ParameterDirection.Output;
                try
                {
                    if ((ObjOrdDb.ProcedureExecutescaler(oraCmd) == true))
                    {
                        StrMsg = oraCmd.Parameters["STATUSTEXT"].Value.ToString();
                        Code = oraCmd.Parameters["STATUSCODE"].Value.ToString();

                        if (StrMsg != null)
                            return StrMsg;
                        else
                            return Code;
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public DataSet Login_MO(string PassCode)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("MO_LOGIN");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("pPASSCODE", OracleType.VarChar, 2000)).Value = PassCode.ToString().Trim();
                OraCmd.Parameters.Add(new OracleParameter("pCW", OracleType.Cursor)).Direction = ParameterDirection.Output;

                ObjOrdDb = new clsDatabase();
                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsLogin: Login : " + ex.Message);
            }
        }

        public DataSet GetEmpDetailsFromEmpID(string EmpID)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand();
                OraCmd.CommandType = CommandType.Text;
                OraCmd.CommandText = "Select * from employee where emp_id = '" + EmpID + "'";
                ObjOrdDb = new clsDatabase();

                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;

                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsLogin: GetEmpDetails() : " + ex.Message);
            }

        }

        public DataSet GetStoreDetailFrmStoreID(long StoreID)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("CHK_GETSTOREDTL");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("SID", OracleType.Number)).Value = StoreID;
                OraCmd.Parameters.Add(new OracleParameter("pSTORE", OracleType.Cursor)).Direction = ParameterDirection.Output;

                ObjOrdDb = new clsDatabase();
                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsLogin: GetStoreDetail() : " + ex.Message);
            }
        }

        public DataSet GetServiceDetailForMEMO()
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("CW_GETSERVICESFORMEMO");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("pSERVICE", OracleType.Cursor)).Direction = ParameterDirection.Output;

                ObjOrdDb = new clsDatabase();
                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsLogin: GetStoreDetail() : " + ex.Message);
            }
        }

        #endregion        

        #region "ClsMOProcess"

        public DataSet MEMO_IsProcess(int StoreID)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_MEMO_CHKPROCESS");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
                oraCmd.Parameters.Add(new OracleParameter("GEOUT", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public bool UpdateIsProcessMEMO(int StoreID, string Flg)
        {
            OracleCommand oraCmd = new OracleCommand("CW_UPDATEMEMOISPROCESS");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
            oraCmd.Parameters.Add(new OracleParameter("pFLG", OracleType.VarChar, 1)).Value = Flg;
            try
            {
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(oraCmd) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon.WriteErrMsg(ex.Message);
            }
        }

        public long MEMO_Transaction(int PID, int BatchID, int CustID, System.DateTime TransDate, double TransAmt, long SerialNo, string CreatedBy, string CmpName, int ChkNo, int StoreID, int StationID, double Fees, int ServiceID, int RID, int Mode, int Qty, int Disc, int ExeId, string ServiceName)
        {
            long OutVal = 0;
            DataSet DsMoProcess = new DataSet();
            OracleCommand oraCmd = new OracleCommand("CW_MEMO_TRAN");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("PID", OracleType.Number)).Value = PID;
            oraCmd.Parameters.Add(new OracleParameter("PBATCHID", OracleType.Number)).Value = BatchID;
            oraCmd.Parameters.Add(new OracleParameter("PCUSTID", OracleType.Number)).Value = CustID;
            oraCmd.Parameters.Add(new OracleParameter("PTRANSDT", OracleType.DateTime)).Value = TransDate;
            oraCmd.Parameters.Add(new OracleParameter("PTRANSAMT", OracleType.Number)).Value = TransAmt;
            oraCmd.Parameters.Add(new OracleParameter("PSERIALNO", OracleType.Number)).Value = SerialNo;
            oraCmd.Parameters.Add(new OracleParameter("PCREATEDBY", OracleType.VarChar, 2000)).Value = CreatedBy.ToString().Trim();
            oraCmd.Parameters.Add(new OracleParameter("PCOMPNAME", OracleType.VarChar, 250)).Value = CmpName.ToString().Trim();
            oraCmd.Parameters.Add(new OracleParameter("PCHKNO", OracleType.Number)).Value = ChkNo;
            oraCmd.Parameters.Add(new OracleParameter("PSTOREID", OracleType.Number)).Value = StoreID;
            oraCmd.Parameters.Add(new OracleParameter("PSTATIONID", OracleType.Number)).Value = StationID;
            oraCmd.Parameters.Add(new OracleParameter("PFEES", OracleType.Double)).Value = Fees;
            oraCmd.Parameters.Add(new OracleParameter("PSERVICEID", OracleType.Number)).Value = ServiceID;
            oraCmd.Parameters.Add(new OracleParameter("RID", OracleType.Number)).Value = RID;
            oraCmd.Parameters.Add(new OracleParameter("PMODE", OracleType.Number)).Value = Mode;
            oraCmd.Parameters.Add(new OracleParameter("TQTY", OracleType.Number)).Value = Qty;
            oraCmd.Parameters.Add(new OracleParameter("TDISC", OracleType.Number)).Value = Disc;
            oraCmd.Parameters.Add(new OracleParameter("PSNAME", OracleType.VarChar, 250)).Value = ServiceName.ToString().Trim();
            oraCmd.Parameters.Add(new OracleParameter("pEXEID", OracleType.Number)).Value = ExeId;
            oraCmd.Parameters.Add(new OracleParameter("GTRANSID", OracleType.Number)).Direction = ParameterDirection.Output;
            try
            {
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(oraCmd))
                    OutVal = Convert.ToInt64(oraCmd.Parameters["GTRANSID"].Value.ToString());
                return OutVal;
            }
            catch (Exception Ex)
            {
                return 0;
            }
            finally
            {
                oraCmd.Dispose();
            }
        }

        public bool InsertMEMO_EventLog(string EventDisc, string OperatorId, int StoreID, int AgentID)
        {
            OracleCommand oraCmd = new OracleCommand("CW_MEMOEVENT_LOG");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
            oraCmd.Parameters.Add(new OracleParameter("pTERMINALID", OracleType.Number)).Value = AgentID;
            oraCmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.VarChar, 2000)).Value = OperatorId;
            oraCmd.Parameters.Add(new OracleParameter("pEVENT", OracleType.VarChar, 2000)).Value = EventDisc;
            try
            {
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(oraCmd) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon.WriteErrMsg(ex.Message);
            }
        }

        public DataSet GetMEMOEventLog(string EventDate)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_MEMO_Prc_GetMemoEventLog");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pEventDate", OracleType.VarChar, 2000)).Value = EventDate;
                oraCmd.Parameters.Add(new OracleParameter("GEOUT", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        #endregion

        #region "ClsMoReports"

        public DataSet MEMO_DailyTransaction(string SysDate, int EmpId, int StoreID)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_MEMO_DAILYRPT");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pSYSDATE", OracleType.VarChar)).Value = Convert.ToDateTime(SysDate).ToString("dd-MMM-yy");
                oraCmd.Parameters.Add(new OracleParameter("pEmpId", OracleType.Number)).Value = EmpId;
                oraCmd.Parameters.Add(new OracleParameter("pStoreId", OracleType.Number)).Value = StoreID;
                oraCmd.Parameters.Add(new OracleParameter("CwOut", OracleType.Cursor)).Direction = ParameterDirection.Output;
                oraCmd.Parameters.Add(new OracleParameter("CwOut1", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        #endregion

        #region "clsNewCustomer"

        public long SaveCustomer(string SysDt)
        {
            try
            {
                Int32 CUSTID = 0;
                OracleCommand OraCmd = new OracleCommand("SAVE_CUSTOMER");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("cMODE", OracleType.VarChar, 2000)).Value = "SAVE";
                //OraCmd.Parameters.Add(new OracleParameter("cNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Name;
                //OraCmd.Parameters.Add(new OracleParameter("cFNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_FName;
                //OraCmd.Parameters.Add(new OracleParameter("cMNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_MName;
                //OraCmd.Parameters.Add(new OracleParameter("cLNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Lname;
                OraCmd.Parameters.Add(new OracleParameter("cID", OracleType.Number)).Value = 0;
                //OraCmd.Parameters.Add(new OracleParameter("cADDRESS1", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Address1;
                //OraCmd.Parameters.Add(new OracleParameter("cADDRESS2", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Address2;
                //OraCmd.Parameters.Add(new OracleParameter("cCITY", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_City;
                //OraCmd.Parameters.Add(new OracleParameter("cSTATE", OracleType.Char, 2)).Value = ModGlobal.Customer.Cust_State;
                //OraCmd.Parameters.Add(new OracleParameter("cZIP", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Zip;
                //OraCmd.Parameters.Add(new OracleParameter("cPHONE1", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Phone1;
                //OraCmd.Parameters.Add(new OracleParameter("cPHONE2", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Phone2;
                //OraCmd.Parameters.Add(new OracleParameter("cDOB", OracleType.DateTime)).Value = ModGlobal.Customer.Cust_DOB;
                //OraCmd.Parameters.Add(new OracleParameter("cLICENSEID", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_LicenceID;
                //OraCmd.Parameters.Add(new OracleParameter("cSSN", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_SSN;
                //OraCmd.Parameters.Add(new OracleParameter("cLIC_ISSUEDON", OracleType.DateTime)).Value = ModGlobal.Customer.Cust_DLIssuedOn;
                //OraCmd.Parameters.Add(new OracleParameter("cLIC_EXPIRESON", OracleType.DateTime)).Value = ModGlobal.Customer.Cust_DLExpiresOn;
                OraCmd.Parameters.Add(new OracleParameter("cCREATED_ON", OracleType.DateTime)).Value = SysDt;
                //OraCmd.Parameters.Add(new OracleParameter("cCREATED_BY", OracleType.VarChar, 2000)).Value = ModGlobal.Employee.Emp_ID;
                //OraCmd.Parameters.Add(new OracleParameter("cISOFAC", OracleType.Char, 1)).Value = ModGlobal.Customer.ISOFACVERIFIED;
                //OraCmd.Parameters.Add(new OracleParameter("cMESSAGE", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Message;
                //OraCmd.Parameters.Add(new OracleParameter("cDBA", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_DBA;
                //OraCmd.Parameters.Add(new OracleParameter("cEIN", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_EIN;
                //OraCmd.Parameters.Add(new OracleParameter("cHEIGHT", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Height;
                //OraCmd.Parameters.Add(new OracleParameter("cWEIGHT", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Weight;
                //OraCmd.Parameters.Add(new OracleParameter("cSEX", OracleType.Char, 1)).Value = ModGlobal.Customer.Cust_Sex;
                OraCmd.Parameters.Add(new OracleParameter("cFEES", OracleType.Number)).Value = System.Convert.ToDouble(20);
                OraCmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Direction = ParameterDirection.Output;
                //OraCmd.Parameters.Add(new OracleParameter("cIDTYPE", OracleType.VarChar, 10)).Value = ModGlobal.Customer.Cust_IDType;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecutescaler(OraCmd) == true)
                {
                    CUSTID = Convert.ToInt32(OraCmd.Parameters["GTRANSID"].Value.ToString());
                    return CUSTID;
                }
                else
                    return 0;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return 0;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsNewCustomer: SaveCustomer : " + ex.Message);
            }
        }

        public long UpdateCustomer(string CName, string FName, string MName, string LName, string Add1, string Add2, string City, string State, string Zip, string PH1, string PH2, System.DateTime DOB, string LicID, string SSN, System.DateTime IssuedOn, System.DateTime ExpireOn, string OFAC, string Message, string DBA, string EIN, string Height, string Weight, string Gender, double Fees, string SysDt)
        {
            try
            {
                Int32 CUSTID = 0;
                OracleCommand OraCmd = new OracleCommand("SAVE_CUSTOMER");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("cMODE", OracleType.VarChar, 2000)).Value = "UPDATE";
                OraCmd.Parameters.Add(new OracleParameter("cNAME", OracleType.VarChar, 2000)).Value = CName.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cFNAME", OracleType.VarChar, 2000)).Value = FName.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cMNAME", OracleType.VarChar, 2000)).Value = MName.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cLNAME", OracleType.VarChar, 2000)).Value = LName.Trim().ToString();
                //OraCmd.Parameters.Add(new OracleParameter("cID", OracleType.Number)).Value = ModGlobal.Customer.Cust_Id;
                OraCmd.Parameters.Add(new OracleParameter("cADDRESS1", OracleType.VarChar, 2000)).Value = Add1.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cADDRESS2", OracleType.VarChar, 2000)).Value = Add2.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cCITY", OracleType.VarChar, 2000)).Value = City.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cSTATE", OracleType.Char, 2)).Value = State.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cZIP", OracleType.VarChar, 2000)).Value = Zip.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cPHONE1", OracleType.VarChar, 2000)).Value = PH1.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cPHONE2", OracleType.VarChar, 2000)).Value = PH2.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cDOB", OracleType.DateTime)).Value = DOB;
                OraCmd.Parameters.Add(new OracleParameter("cLICENSEID", OracleType.VarChar, 2000)).Value = LicID.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cSSN", OracleType.VarChar, 2000)).Value = SSN.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cLIC_ISSUEDON", OracleType.DateTime)).Value = IssuedOn;
                OraCmd.Parameters.Add(new OracleParameter("cLIC_EXPIRESON", OracleType.DateTime)).Value = ExpireOn;
                OraCmd.Parameters.Add(new OracleParameter("cCREATED_ON", OracleType.DateTime)).Value = SysDt;
                //OraCmd.Parameters.Add(new OracleParameter("cCREATED_BY", OracleType.VarChar, 2000)).Value = ModGlobal.Employee.Emp_ID;
                OraCmd.Parameters.Add(new OracleParameter("cISOFAC", OracleType.Char, 1)).Value = OFAC.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cMESSAGE", OracleType.VarChar, 2000)).Value = Message.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cDBA", OracleType.VarChar, 2000)).Value = DBA.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cEIN", OracleType.VarChar, 2000)).Value = EIN.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cHEIGHT", OracleType.VarChar, 2000)).Value = Height.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cWEIGHT", OracleType.VarChar, 2000)).Value = Weight.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cSEX", OracleType.Char, 1)).Value = Gender.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cFEES", OracleType.Number)).Value = Fees;
                OraCmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Direction = ParameterDirection.Output;
                //OraCmd.Parameters.Add(new OracleParameter("cIDTYPE", OracleType.VarChar, 10)).Value = ModGlobal.Customer.Cust_IDType;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecutescaler(OraCmd) == true)
                {
                    CUSTID = Convert.ToInt32(OraCmd.Parameters["CUSTOMERID"].Value.ToString());
                    return CUSTID;
                }
                else
                    return 0;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return 0;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsViewCust: UpdateCustomer : " + ex.Message);
            }
        }

        #endregion

        #region "clsPdf"
        #endregion

        #region "clsReduceImage"

        public string ReduceImageSize(string ImageInPath, double factor, string ImageOutPath)
        {
            Bitmap img;
            System.Drawing.Imaging.ImageFormat imgFormat;

            //FileStream fs = new FileStream(ImageInPath, FileMode.Open, FileAccess.Read);
            //byte imgData;
            //fs.Read(imgData, 0, fs.Length);
            //fs.Close();
            //try
            //{
            //    img = Image.FromStream(new MemoryStream(imgData));
            //    imgFormat = img.RawFormat;
            //    img = new Bitmap(img, new Size(img.Size.Width * factor, img.Size.Height * factor));
            //    MemoryStream ms = new MemoryStream();
            //    img.Save(ms, Imaging.ImageFormat.Jpeg);
            //    MemoryStream ms1 = new MemoryStream();
            //    img.Save(ms1, Imaging.ImageFormat.Jpeg);
            //    byte imgData1;
            //    ms1.Position = 0;
            //    ms1.Read(imgData1, 0, ms1.Length);
            //    FileStream fs1 = new FileStream(ImageOutPath, FileMode.Create, FileAccess.Write);
            //    fs1.Write(imgData1, 0, UBound(imgData1));
            //    fs1.Close();
            //    File.Delete(ImageInPath);
            return null;
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //    ObjCmn.WriteErrMsg("clsReduceImage : ReduceImageSize :" + ex.Message + vbNewLine + DateTime.Now.ToString + vbNewLine);
            //}
        }

        #endregion

        #region "clsViewCust"

        public DataSet fillcustomerView()
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CHK_CustomerView");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                //oraCmd.Parameters.Add(new OracleParameter("pCustId", OracleType.VarChar, 2000)).Value = System.Convert.ToInt32(ModGlobal.Store.Store_ID);
                oraCmd.Parameters.Add(new OracleParameter("pMAKER", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public DataSet GetCustDetail()
        {
            try
            {
                //string Qry = "SELECT * FROM CUSTOMER WHERE CUSTOMER.CUST_ID='" + ModGlobal.Customer.Cust_Id.ToString + "'";
                OracleCommand OraCmd = new OracleCommand();
                OraCmd.CommandType = CommandType.Text;
                //OraCmd.CommandText = Qry;
                ObjOrdDb = new clsDatabase();
                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null /* TODO Change to default(_) if this is not a reference type */;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //ObjCommon.WriteErrMsg("clsViewCust: GetCustDetailByID : " + ex.Message);
            }
        }

        public long UpdateCustomerNew(string CName, string FName, string MName, string LName, string Add1, string Add2, string City, string State, string Zip, string PH1, string PH2, System.DateTime DOB, string LicID, string SSN, System.DateTime IssuedOn, System.DateTime ExpireOn, string OFAC, string Message, string DBA, string EIN, string Height, string Weight, string Gender, double Fees, string SysDt)
        {
            try
            {
                Int32 CUSTID = 0;
                OracleCommand OraCmd = new OracleCommand("SAVE_CUSTOMER");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("cMODE", OracleType.VarChar, 2000)).Value = "UPDATE";
                OraCmd.Parameters.Add(new OracleParameter("cNAME", OracleType.VarChar, 2000)).Value = CName.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cFNAME", OracleType.VarChar, 2000)).Value = FName.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cMNAME", OracleType.VarChar, 2000)).Value = MName.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cLNAME", OracleType.VarChar, 2000)).Value = LName.Trim().ToString();
                //OraCmd.Parameters.Add(new OracleParameter("cID", OracleType.Number)).Value = ModGlobal.Customer.Cust_Id;
                OraCmd.Parameters.Add(new OracleParameter("cADDRESS1", OracleType.VarChar, 2000)).Value = Add1.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cADDRESS2", OracleType.VarChar, 2000)).Value = Add2.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cCITY", OracleType.VarChar, 2000)).Value = City.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cSTATE", OracleType.Char, 2)).Value = State.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cZIP", OracleType.VarChar, 2000)).Value = Zip.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cPHONE1", OracleType.VarChar, 2000)).Value = PH1.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cPHONE2", OracleType.VarChar, 2000)).Value = PH2.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cDOB", OracleType.DateTime)).Value = DOB;
                OraCmd.Parameters.Add(new OracleParameter("cLICENSEID", OracleType.VarChar, 2000)).Value = LicID.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cSSN", OracleType.VarChar, 2000)).Value = SSN.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cLIC_ISSUEDON", OracleType.DateTime)).Value = IssuedOn;
                OraCmd.Parameters.Add(new OracleParameter("cLIC_EXPIRESON", OracleType.DateTime)).Value = ExpireOn;
                OraCmd.Parameters.Add(new OracleParameter("cCREATED_ON", OracleType.DateTime)).Value = SysDt;
                //OraCmd.Parameters.Add(new OracleParameter("cCREATED_BY", OracleType.VarChar, 2000)).Value = ModGlobal.Employee.Emp_ID;
                OraCmd.Parameters.Add(new OracleParameter("cISOFAC", OracleType.Char, 1)).Value = OFAC.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cMESSAGE", OracleType.VarChar, 2000)).Value = Message.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cDBA", OracleType.VarChar, 2000)).Value = DBA.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cEIN", OracleType.VarChar, 2000)).Value = EIN.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cHEIGHT", OracleType.VarChar, 2000)).Value = Height.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cWEIGHT", OracleType.VarChar, 2000)).Value = Weight.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cSEX", OracleType.Char, 1)).Value = Gender.Trim().ToString();
                OraCmd.Parameters.Add(new OracleParameter("cFEES", OracleType.Number)).Value = Fees;
                OraCmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Direction = ParameterDirection.Output;
                //OraCmd.Parameters.Add(new OracleParameter("cIDTYPE", OracleType.VarChar, 10)).Value = ModGlobal.Customer.Cust_IDType;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecutescaler(OraCmd) == true)
                {
                    CUSTID = Convert.ToInt32(OraCmd.Parameters["CUSTOMERID"].Value.ToString());
                    return CUSTID;
                }
                else
                    return 0;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return 0;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsViewCust: UpdateCustomer : " + ex.Message);
            }
        }

        public bool IsBadprocedure(int custid, int transid, double chkamt, string remark)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("CHECKCASHING_UPDATE_NEW1");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("MODESTR", OracleType.VarChar)).Value = "3";
                cmd.Parameters.Add(new OracleParameter("TRANSID", OracleType.Number)).Value = transid;
                cmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Value = custid;
                cmd.Parameters.Add(new OracleParameter("AMT", OracleType.Number)).Value = chkamt;
                cmd.Parameters.Add(new OracleParameter("BADREMARK", OracleType.VarChar)).Value = remark;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(cmd) == true)
                    return true;
                else
                    return false;
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Recovery(int custid, int tranid, double chkamt)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("CHECKCASHING_UPDATE_NEW");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("MODESTR", OracleType.VarChar)).Value = "2";
                cmd.Parameters.Add(new OracleParameter("TRANSID", OracleType.Number)).Value = tranid;
                cmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Value = custid;
                cmd.Parameters.Add(new OracleParameter("AMT", OracleType.Number)).Value = chkamt;
                cmd.Parameters.Add(new OracleParameter("BADREMARK", OracleType.VarChar)).Value = "";
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(cmd) == true)
                    return true;
                else
                    return false;
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Collected(int custid, int tranid, double chkamt)
        {
            try
            {
                OracleCommand cmd = new OracleCommand("CHECKCASHING_UPDATE_NEW");
                bool @bool;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new OracleParameter("MODESTR", OracleType.VarChar)).Value = "3";
                cmd.Parameters.Add(new OracleParameter("TRANSID", OracleType.Number)).Value = tranid;
                cmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Value = custid;
                cmd.Parameters.Add(new OracleParameter("AMT", OracleType.Number)).Value = chkamt;
                cmd.Parameters.Add(new OracleParameter("BADREMARK", OracleType.VarChar)).Value = "";
                ObjOrdDb = new clsDatabase();
                @bool = ObjOrdDb.ProcedureExecute(cmd);
                return @bool;
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public long SaveCustomerNew(string SysDt)
        {
            try
            {
                Int32 CUSTID;
                OracleCommand OraCmd = new OracleCommand("SAVE_CUSTOMER");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("cMODE", OracleType.VarChar, 2000)).Value = "SAVE";
                //OraCmd.Parameters.Add(new OracleParameter("cNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Name;
                //OraCmd.Parameters.Add(new OracleParameter("cFNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_FName;
                //OraCmd.Parameters.Add(new OracleParameter("cMNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_MName;
                //OraCmd.Parameters.Add(new OracleParameter("cLNAME", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_LName;
                OraCmd.Parameters.Add(new OracleParameter("cID", OracleType.Number)).Value = 0;
                //OraCmd.Parameters.Add(new OracleParameter("cADDRESS1", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Address1;
                //OraCmd.Parameters.Add(new OracleParameter("cADDRESS2", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Address2;
                //OraCmd.Parameters.Add(new OracleParameter("cCITY", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_City;
                //OraCmd.Parameters.Add(new OracleParameter("cSTATE", OracleType.Char, 2)).Value = ModGlobal.Customer.Cust_State;
                //OraCmd.Parameters.Add(new OracleParameter("cZIP", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Zip;
                //OraCmd.Parameters.Add(new OracleParameter("cPHONE1", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Phone1;
                //OraCmd.Parameters.Add(new OracleParameter("cPHONE2", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Phone2;
                //OraCmd.Parameters.Add(new OracleParameter("cDOB", OracleType.DateTime)).Value = ModGlobal.Customer.Cust_DOB;
                //OraCmd.Parameters.Add(new OracleParameter("cLICENSEID", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_LicenceID;
                OraCmd.Parameters.Add(new OracleParameter("cSSN", OracleType.VarChar, 2000)).Value = "";
                //OraCmd.Parameters.Add(new OracleParameter("cLIC_ISSUEDON", OracleType.DateTime)).Value = ModGlobal.Customer.Cust_DLIssuedOn;
                //OraCmd.Parameters.Add(new OracleParameter("cLIC_EXPIRESON", OracleType.DateTime)).Value = ModGlobal.Customer.Cust_DLExpiresOn;
                OraCmd.Parameters.Add(new OracleParameter("cCREATED_ON", OracleType.DateTime)).Value = SysDt;
                //OraCmd.Parameters.Add(new OracleParameter("cCREATED_BY", OracleType.VarChar, 2000)).Value = ModGlobal.Employee.Emp_ID;
                //OraCmd.Parameters.Add(new OracleParameter("cISOFAC", OracleType.Char, 1)).Value = ModGlobal.Customer.ISOFACVERIFIED;
                //OraCmd.Parameters.Add(new OracleParameter("cMESSAGE", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Message;
                //OraCmd.Parameters.Add(new OracleParameter("cDBA", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_DBA;
                //OraCmd.Parameters.Add(new OracleParameter("cEIN", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_EIN;
                //OraCmd.Parameters.Add(new OracleParameter("cHEIGHT", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Height;
                //OraCmd.Parameters.Add(new OracleParameter("cWEIGHT", OracleType.VarChar, 2000)).Value = ModGlobal.Customer.Cust_Weight;
                //OraCmd.Parameters.Add(new OracleParameter("cSEX", OracleType.Char, 1)).Value = ModGlobal.Customer.Cust_Sex;
                OraCmd.Parameters.Add(new OracleParameter("cFEES", OracleType.Number)).Value = System.Convert.ToDouble(0);
                //OraCmd.Parameters.Add(new OracleParameter("cIDTYPE", OracleType.VarChar, 10)).Value = ModGlobal.Customer.Cust_IDType;
                OraCmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Direction = ParameterDirection.Output;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecutescaler(OraCmd) == true)
                {
                    CUSTID = Convert.ToInt32(OraCmd.Parameters["CUSTOMERID"].Value.ToString());
                    return CUSTID;
                }
                else
                    return 0;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return 0;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsTransaction: SaveCustomer : " + ex.Message);
            }
        }

        public DataSet GetCustUtilPayment(string pid)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("GE_Prc_GetCustUtilPayment");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("PCustID", OracleType.VarChar, 2000)).Value = pid;
                oraCmd.Parameters.Add(new OracleParameter("GEOUT", OracleType.Cursor)).Direction = ParameterDirection.Output;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public bool UpdateUtility(string pCust_ID, string pAccountNo, string pUtilityCode)
        {
            OracleCommand oraCmd = new OracleCommand("GE_PRC_UPDATEUTILITY");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pCust_ID", OracleType.VarChar)).Value = pCust_ID;
            oraCmd.Parameters.Add(new OracleParameter("pAccountNo", OracleType.VarChar)).Value = pAccountNo;
            oraCmd.Parameters.Add(new OracleParameter("pUtilCode", OracleType.VarChar)).Value = pUtilityCode;
            try
            {
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(oraCmd) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon.WriteErrMsg(ex.Message);
            }
        }

        public bool UpdateCustomerUtility(string pCust_ID, string pAccountNo, string pUtilityCode, string pIsdelete)
        {
            OracleCommand oraCmd = new OracleCommand("GE_PRC_UPDATECUSTOMERUTILITY");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pCust_ID", OracleType.VarChar)).Value = pCust_ID;
            oraCmd.Parameters.Add(new OracleParameter("pAccountNo", OracleType.VarChar)).Value = pAccountNo;
            oraCmd.Parameters.Add(new OracleParameter("pUtilCode", OracleType.VarChar)).Value = pUtilityCode;
            oraCmd.Parameters.Add(new OracleParameter("pIsDelete", OracleType.VarChar)).Value = pIsdelete;
            try
            {
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(oraCmd) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon.WriteErrMsg(ex.Message);
            }
        }

        #endregion

        // Other Functions:

        #region "clscommon"

        public string getsysdate()
        {
            try
            {
                string SysDate = "";

                clsDatabase ObjOrdDb = new clsDatabase();
                OracleCommand oraCmd = new OracleCommand("CHK_GETSYSDATE");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pvalue", OracleType.VarChar, 2000)).Direction = ParameterDirection.Output;
                clsDatabase objOradb = new clsDatabase();
                try
                {
                    if ((ObjOrdDb.ProcedureExecutescaler(oraCmd) == true))
                    {
                        SysDate = oraCmd.Parameters["pvalue"].Value.ToString();
                        return SysDate;
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    //clsCommon.WriteError(ex.Message);
                    return "";
                }
                finally
                {
                    oraCmd.Dispose();
                }
            }
            catch
            {
                return "";
            }
        }

        public void WriteErrMsg(string exMsg)
        {
            //try
            //{
            //    string directory = ModGlobal.ApplicationSettings.AppPath;
            //    string[] Str;
            //    Str = directory.Split("bin");
            //    directory = Str[0] + "Error" + "\\ErrorLog.txt";
            //    StreamWriter writer = new StreamWriter(directory, true, System.Text.Encoding.ASCII);
            //    writer.WriteLine(exMsg + vbNewLine + DateTime.Now.ToString + vbNewLine);
            //    writer.Close();
            //}
            //catch (Exception ex)
            //{
            //    MsgBox("Error while writing error massage. " + ex.Message, MsgBoxStyle.OkOnly, "Error");
            //}
        }

        public DataSet GetCustData(string StrQuery)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand();
                oraCmd.CommandType = System.Data.CommandType.Text;
                oraCmd.CommandText = StrQuery;
                try
                {
                    dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                    if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    {
                        return dsInformation;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    //clsCommon.WriteError(ex.Message);
                    //return ex.ToString();
                    return null;
                }
                finally
                {
                    oraCmd.Dispose();
                    dsInformation.Dispose();
                }
            }
            catch (Exception ex)
            {
                //return ex.ToString();
                return null;
            }
        }

        public void WriteMemoEventLog(string exMsg, string Filename)
        {
            StreamWriter writer;
            //try
            //{
            //    string LogFilename = string.Empty;
            //    string Directory = string.Empty;
            //    LogFilename = Filename.Trim().Replace("/", "");
            //    if (File.Exists(ModGlobal.ApplicationSettings.MemoEventLogPath + "\\" + LogFilename + ".txt"))
            //    {
            //    }
            //    else
            //        File.Create(ModGlobal.ApplicationSettings.MemoEventLogPath + "\\" + LogFilename + ".txt");
            //    Directory = ModGlobal.ApplicationSettings.MemoEventLogPath;
            //    string[] Str;
            //    Str = Directory.Split("bin");
            //    Directory = Str[0] + "\\" + LogFilename + ".txt";
            //    writer = new StreamWriter(Directory, true, System.Text.Encoding.ASCII);
            //    writer.WriteLine(exMsg + vbNewLine + DateTime.Now.ToString + vbNewLine);
            //    writer.Close();
            //}
            //catch (Exception ex)
            //{
            //    writer.Close();
            //    MsgBox("Error while writing error massage. " + ex.Message, MsgBoxStyle.OkOnly, "Error");
            //}
        }

        public void GetHardDiskNo()
        {
            //try
            //{
            //    string HDD = string.Empty;
            //    string MySystemDirectory = Environment.SystemDirectory;
            //    string MyInstallDrive = MySystemDirectory.Substring(0, 2);
            //    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive Where Index = 0");
            //    foreach (ManagementObject wmi_HD in searcher.Get())
            //        HDD = HDD + wmi_HD("Signature").ToString;
            //    try
            //    {
            //        ManagementObjectSearcher Searcher_L = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk WHERE DeviceID = '" + MyInstallDrive + "'");
            //        foreach (ManagementObject queryObj in Searcher_L.Get())
            //            HDD = HDD + queryObj("VolumeSerialNumber").ToString.Trim();
            //    }
            //    catch (Exception ex)
            //    {
            //        WriteErrMsg("An error occurred while querying for WMI data: VolumeSerialNumber " + ex.Message);
            //    }

            //    return HDD;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //    WriteErrMsg("An error occurred while querying for WMI data: VolumeSerialNumber " + ex.Message);
            //}
        }

        public DataSet GetRegStatusByID(string EmpID, string StoreID, string StationID)
        {
            dsInformation = new DataSet();
            ObjOrdDb = new clsDatabase();
            OracleCommand OraCmd = new OracleCommand("CW_GETREGSTATUSBYID");
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.VarChar, 25)).Value = EmpID;
            OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.Number)).Value = StationID;
            OraCmd.Parameters.Add(new OracleParameter("STOREID", OracleType.Number)).Value = StoreID;
            OraCmd.Parameters.Add(new OracleParameter("CW1", OracleType.Cursor)).Direction = ParameterDirection.Output;
            OraCmd.Parameters.Add(new OracleParameter("CW2", OracleType.Cursor)).Direction = ParameterDirection.Output;
            try
            {
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                //clsCommon ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsCommon: GetRegStatusByID()" + ex.Message);
                return null;
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        #endregion

        #region "clsMail"

        public bool SendMail(string MailFrom, string MailTo, string MailSubject, string MailBody, string MailAttachment)
        {
            MailMessage mailMSg = new MailMessage();
            SmtpClient SMTPserver = new SmtpClient();
            try
            {
                SMTPserver.Credentials = new System.Net.NetworkCredential("prami@cwf-llc.com", "Password1");
                SMTPserver.Port = 25;
                SMTPserver.Host = "mail.cwf-llc.com";
                mailMSg.From = new MailAddress(MailFrom);
                mailMSg.To.Add(MailTo);
                mailMSg.Subject = MailSubject;
                mailMSg.Body = MailBody;
                SMTPserver.Send(mailMSg);
                return true;
            }
            catch (Exception ex)
            {
                //objComm.WriteErrMsg("clsMail : SendMail :" + ex.Message + vbNewLine + DateTime.Now.ToString + vbNewLine);
                return false;
            }
        }

        #endregion

        #endregion

        #region CheckCashing

        #region "clsAddstoretoGrp"

        public bool UpdateGrpIdToStore(long grpID, long storeid)
        {
            try
            {
                string Str = string.Empty;
                Str = "UPDATE STORE SET GRPID = " + grpID.ToString() + " WHERE ID = " + storeid.ToString();
                DataSet Ds = new DataSet();
                Ds = ExecuteTextQuery(Str);
                return true;
            }
            catch (Exception ex)
            {
                return default(Boolean);
            }
        }

        public DataSet ExecuteTextQuery(string StrQuery)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand();
                OraCmd.CommandType = CommandType.Text;
                OraCmd.CommandText = StrQuery;
                ObjOrdDb = new clsDatabase();
                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null /* TODO Change to default(_) if this is not a reference type */;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsReports: ExecuteTextQuery() : " + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        #endregion

        #region "clscashTrans"

        public DataSet GetAllStations(string sysdate, int StoreID)
        {
            OracleCommand OraCmd = new OracleCommand("CW_GETALLSTATION");
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add(new OracleParameter("CwOut", OracleType.Cursor)).Direction = ParameterDirection.Output;
            OraCmd.Parameters.Add(new OracleParameter("CwOut1", OracleType.Cursor)).Direction = ParameterDirection.Output;
            OraCmd.Parameters.Add(new OracleParameter("pSTORE", OracleType.Number)).Value = StoreID;
            OraCmd.Parameters.Add(new OracleParameter("pSYSDATE", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = new DataSet();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //objcommon = new clsCommon();
                //objcommon.WriteErrMsg("clscashTrans: GetAllStations()" + ex.Message);
                //return null /* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public long InsertCashTransactions(int pSTATIONFROM, int pSTATIONTO, double pAMOUNT, long pEMPID, long pTRANSFER_EMP_ID, long pSHIFTID, long pSOTREID, string sysdate, string Remark)
        {
            int returnVal = 0;
            OracleCommand oracmd = new OracleCommand("CW_INSERTCASHTRANS");
            oracmd.CommandType = System.Data.CommandType.StoredProcedure;
            oracmd.Parameters.Add(new OracleParameter("pSTATIONFROM", OracleType.Number)).Value = pSTATIONFROM;
            oracmd.Parameters.Add(new OracleParameter("pSTATIONTO", OracleType.Number)).Value = pSTATIONTO;
            oracmd.Parameters.Add(new OracleParameter("pAMOUNT", OracleType.Number)).Value = pAMOUNT;
            oracmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.Number)).Value = pEMPID;
            oracmd.Parameters.Add(new OracleParameter("pTRANSFER_EMP_ID", OracleType.Number)).Value = pTRANSFER_EMP_ID;
            oracmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = pSHIFTID;
            oracmd.Parameters.Add(new OracleParameter("pSOTREID", OracleType.Number)).Value = pSOTREID;
            oracmd.Parameters.Add(new OracleParameter("pSYSDATE", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            oracmd.Parameters.Add(new OracleParameter("pREMARK", OracleType.VarChar, 2000)).Value = Remark;
            oracmd.Parameters.Add(new OracleParameter("POUTID", OracleType.Number)).Direction = ParameterDirection.Output;
            ObjOrdDb = new clsDatabase();
            try
            {
                if (ObjOrdDb.ProcedureExecutescaler(oracmd))
                {
                    returnVal = Convert.ToInt32(oracmd.Parameters["POUTID"].Value.ToString());
                    return returnVal;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                //objcommon.WriteErrMsg("clscashTrans : INSERT_CASHTRANS :" + ex.Message + vbNewLine + DateTime.Now.ToString + vbNewLine);
                return 0;
            }
            finally
            {
                oracmd.Dispose();
            }
        }

        public DataSet GetReceivedCash(string sysdate, int StoreID, int StationID)
        {
            OracleCommand oraCmd = new OracleCommand("CW_GETCASHRECEIVEDST");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pCW", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("pSTDT", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            oraCmd.Parameters.Add(new OracleParameter("pSTID", OracleType.Number)).Value = StationID;
            oraCmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = 1;
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                oraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //objcommon.WriteErrMsg("clscashTrans: GetReceivedCash()" + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public DataSet GetTransCash(string sysdate, int StoreID, int StationID)
        {
            OracleCommand oraCmd = new OracleCommand("CW_GETCASHTRNST");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pCW", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("pSTDT", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            oraCmd.Parameters.Add(new OracleParameter("pSTID", OracleType.Number)).Value = StationID;
            oraCmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = 1;
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                oraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //objcommon.WriteErrMsg("clscashTrans: GetTransCash()" + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        //From Here Code Comes for VAULT TRANSACTION

        public DataSet GetVaultTransDetail(string sysdate, string EmpID, int StoreID, int StationID)
        {
            OracleCommand oraCmd = new OracleCommand("CW_GETVAULTTRANSDETAIL1");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("CwOut", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("CwOut1", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("CwOut2", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.VarChar)).Value = EmpID.ToString().Trim();
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
            oraCmd.Parameters.Add(new OracleParameter("pSTATIONID", OracleType.Number)).Value = StationID;
            oraCmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = 1;
            oraCmd.Parameters.Add(new OracleParameter("pSTDT", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                oraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //objcommon.WriteErrMsg("clscashTrans : GetVaultTransDetail :" + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public DataSet GetVaultTransDetailForVault(string sysdate, string EmpID, int StoreID, int StationID)
        {
            OracleCommand oraCmd = new OracleCommand("CW_GETVAULTTRANSDETAIL2");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("CwOut", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("CwOut1", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("CwOut2", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.VarChar)).Value = EmpID.ToString().Trim();
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
            oraCmd.Parameters.Add(new OracleParameter("pSTATIONID", OracleType.Number)).Value = StationID;
            oraCmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = 1;
            oraCmd.Parameters.Add(new OracleParameter("pSTDT", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                oraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //objcommon.WriteErrMsg("clscashTrans : GetVaultTransDetail :" + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public bool InsertVault(int pSTATIONFROM, int pSTATIONTO, double pAMOUNT, long pEMPID, long pTRANSFER_EMP_ID, long pSHIFTID, long pSOTREID, string sysdate, string Remark, int OneD, int FiveD, int TenD, int TwentyD, int FiftyD, int HundredD, int OneC, int FiveC, int TenC, int TwntyFiveC, int FiftyC, string Mode, char Flag)
        {
            OracleCommand oracmd = new OracleCommand("CW_INSERTCASHTRANSVAULT1");
            oracmd.CommandType = System.Data.CommandType.StoredProcedure;
            oracmd.Parameters.Add(new OracleParameter("pSTATIONFROM", OracleType.Number)).Value = pSTATIONFROM;
            oracmd.Parameters.Add(new OracleParameter("pSTATIONTO", OracleType.Number)).Value = pSTATIONTO;
            oracmd.Parameters.Add(new OracleParameter("pAMOUNT", OracleType.Number)).Value = pAMOUNT;
            oracmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.Number)).Value = pEMPID;
            oracmd.Parameters.Add(new OracleParameter("pTRANSFER_EMP_ID", OracleType.Number)).Value = pTRANSFER_EMP_ID;
            oracmd.Parameters.Add(new OracleParameter("pSHIFTID", OracleType.Number)).Value = pSHIFTID;
            oracmd.Parameters.Add(new OracleParameter("pSOTREID", OracleType.Number)).Value = pSOTREID;
            oracmd.Parameters.Add(new OracleParameter("pSYSDATE", OracleType.VarChar)).Value = System.Convert.ToDateTime(sysdate).ToString("dd-MMM-yy");
            oracmd.Parameters.Add(new OracleParameter("pREMARK", OracleType.VarChar, 2000)).Value = Remark;
            oracmd.Parameters.Add(new OracleParameter("pONED", OracleType.Number)).Value = OneD;
            oracmd.Parameters.Add(new OracleParameter("pFIVED", OracleType.Number)).Value = FiveD;
            oracmd.Parameters.Add(new OracleParameter("pTEND", OracleType.Number)).Value = TenD;
            oracmd.Parameters.Add(new OracleParameter("pTWENTYD", OracleType.Number)).Value = TwentyD;
            oracmd.Parameters.Add(new OracleParameter("pFIFTYD", OracleType.Number)).Value = FiftyD;
            oracmd.Parameters.Add(new OracleParameter("pHUNDREDD", OracleType.Number)).Value = HundredD;
            oracmd.Parameters.Add(new OracleParameter("pONEC", OracleType.Number)).Value = OneC;
            oracmd.Parameters.Add(new OracleParameter("pFIVEC", OracleType.Number)).Value = FiveC;
            oracmd.Parameters.Add(new OracleParameter("pTENC", OracleType.Number)).Value = TenC;
            oracmd.Parameters.Add(new OracleParameter("pTWENTYFIVEC", OracleType.Number)).Value = TwntyFiveC;
            oracmd.Parameters.Add(new OracleParameter("pFIFTYC", OracleType.Number)).Value = FiftyC;
            oracmd.Parameters.Add(new OracleParameter("pMODE", OracleType.VarChar)).Value = Mode;
            oracmd.Parameters.Add(new OracleParameter("pFLAG", OracleType.Char, 5)).Value = Flag;
            ObjOrdDb = new clsDatabase();
            try
            {
                if (ObjOrdDb.ProcedureExecute(oracmd) == true)
                    return true;
                else
                    return false;
                oracmd.Dispose();
            }
            catch (Exception ex)
            {
                //objcommon.WriteErrMsg("clscashTrans : InsertVault :" + ex.Message);
                return false;
            }
            finally
            {
                oracmd.Dispose();
            }
        }

        #endregion

        #region "clsChecks"

        public DataSet GetAllServices()
        {
            ObjOrdDb = new clsDatabase();
            dsInformation = new DataSet();
            OracleCommand OraCmd = new OracleCommand("CW_GETALLSERVICES");
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add(new OracleParameter("pSERVICE", OracleType.Cursor)).Direction = ParameterDirection.Output;
            try
            {
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;

                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: GetAllService()" + ex.Message);
                return null;
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public bool InsertTempData(long SID, long EmpID, long ShiftID, string SType, long StID, long CustId, int PayMode, long Qty, double Disc, double Price, double Fees, double Amount, long RwID, long StoreID, double ServiceComm, double StoreComm)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("SAVE_TEMPDATA");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("SERVICEID", OracleType.VarChar)).Value = SID;
                OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.VarChar)).Value = EmpID;
                OraCmd.Parameters.Add(new OracleParameter("SHIFTID", OracleType.VarChar)).Value = ShiftID;
                OraCmd.Parameters.Add(new OracleParameter("STYPE", OracleType.VarChar)).Value = SType.Trim();
                OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.VarChar)).Value = StID;
                OraCmd.Parameters.Add(new OracleParameter("CUSTID", OracleType.Number)).Value = CustId;
                OraCmd.Parameters.Add(new OracleParameter("PMODE", OracleType.Char)).Value = PayMode;
                OraCmd.Parameters.Add(new OracleParameter("TQTY", OracleType.Number)).Value = Qty;
                OraCmd.Parameters.Add(new OracleParameter("TDISC", OracleType.Number)).Value = Disc;
                OraCmd.Parameters.Add(new OracleParameter("TPRICE", OracleType.Number)).Value = Price;
                OraCmd.Parameters.Add(new OracleParameter("TFEES", OracleType.Number)).Value = Fees;
                OraCmd.Parameters.Add(new OracleParameter("SERFEE", OracleType.Number)).Value = ServiceComm;
                OraCmd.Parameters.Add(new OracleParameter("STRFEE", OracleType.Number)).Value = StoreComm;
                OraCmd.Parameters.Add(new OracleParameter("TAMOUNT", OracleType.Number)).Value = Amount;
                OraCmd.Parameters.Add(new OracleParameter("RID", OracleType.Number)).Value = RwID;
                OraCmd.Parameters.Add(new OracleParameter("STOREID", OracleType.Number)).Value = StoreID;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(OraCmd) == true)
                    return true;
                else
                    return false;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: InsertTempData" + ex.Message);
            }
        }

        public bool DeleteTempData(long EmpID, long ShiftID, long StID, long RwID, long StoreID, string Flag)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("CW_DELETETEMPDATA2");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.VarChar, 2000)).Value = EmpID;
                OraCmd.Parameters.Add(new OracleParameter("SHIFTID", OracleType.VarChar, 200)).Value = ShiftID;
                OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.VarChar, 200)).Value = StID;
                OraCmd.Parameters.Add(new OracleParameter("FLAG", OracleType.VarChar, 2000)).Value = Flag;
                OraCmd.Parameters.Add(new OracleParameter("RID", OracleType.Number)).Value = RwID;
                OraCmd.Parameters.Add(new OracleParameter("STOREID", OracleType.Number)).Value = StoreID;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(OraCmd) == true)
                    return true;
                else
                    return false;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: DeleteTempData" + ex.Message);
            }
        }

        public bool InsertTrans(long SID, string SType, long TransQty, double TransPrice, double TransFee, double TransDisc, double TransAmt, int PayMode, long EmpID, long ShiftID, long StID, long CreatedBy, string AcNo, long CustId, long PaymentID, System.DateTime SDate, Int16 RwId, long SarID, double ServiceFee, double StoreFee, int StoreID)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("CW_SAVETRANS1");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("SERVICE_ID", OracleType.Number)).Value = SID;
                OraCmd.Parameters.Add(new OracleParameter("SERVICETYPE", OracleType.VarChar)).Value = SType.Trim();
                OraCmd.Parameters.Add(new OracleParameter("TRANS_DT", OracleType.DateTime)).Value = SDate;
                OraCmd.Parameters.Add(new OracleParameter("TRANSQTY", OracleType.Number)).Value = TransQty;
                OraCmd.Parameters.Add(new OracleParameter("TRANSPRICE", OracleType.Number)).Value = TransPrice;
                OraCmd.Parameters.Add(new OracleParameter("TRANSFEES", OracleType.Number)).Value = TransFee;
                OraCmd.Parameters.Add(new OracleParameter("TRANSDISC", OracleType.Number)).Value = TransDisc;
                OraCmd.Parameters.Add(new OracleParameter("TRANSAMT", OracleType.Number)).Value = TransAmt;
                OraCmd.Parameters.Add(new OracleParameter("PAYMENTMODE", OracleType.Char)).Value = PayMode;
                OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.Number)).Value = EmpID;
                OraCmd.Parameters.Add(new OracleParameter("SHIFTID", OracleType.Number)).Value = ShiftID;
                OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.Number)).Value = StID;
                OraCmd.Parameters.Add(new OracleParameter("CREATEDBY", OracleType.Number)).Value = CreatedBy;
                OraCmd.Parameters.Add(new OracleParameter("CREATEDON", OracleType.DateTime)).Value = SDate;
                OraCmd.Parameters.Add(new OracleParameter("ACCOUNTNUMBER", OracleType.VarChar)).Value = AcNo.Trim();
                OraCmd.Parameters.Add(new OracleParameter("CUSTOMERID", OracleType.Number)).Value = CustId;
                OraCmd.Parameters.Add(new OracleParameter("PAYMENT_ID", OracleType.Number)).Value = PaymentID;
                OraCmd.Parameters.Add(new OracleParameter("SARID", OracleType.Number)).Value = SarID;
                OraCmd.Parameters.Add(new OracleParameter("STOREID", OracleType.Number)).Value = StoreID;
                OraCmd.Parameters.Add(new OracleParameter("RID", OracleType.Number)).Value = RwId;
                OraCmd.Parameters.Add(new OracleParameter("SERVICEFEE", OracleType.Number)).Value = ServiceFee;
                OraCmd.Parameters.Add(new OracleParameter("STOREFEE", OracleType.Number)).Value = StoreFee;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecute(OraCmd) == true)
                    return true;
                else
                    return false;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: InsertTrans" + ex.Message);
            }
        }

        public long InsertPayment(double TAmt, double STax, double Disc, double Cash, double Check, double CC, double TotalAmt, double Change, long EmpID, long ShiftID, long StID, System.DateTime Dt, int StoreID)
        {
            try
            {
                long TransID;
                OracleCommand OraCmd = new OracleCommand("CW_SAVETRANSPAYMENT");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_DT", OracleType.DateTime)).Value = Dt;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_AMT", OracleType.Double)).Value = TAmt;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_SALESTAX", OracleType.Double)).Value = STax;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_DISC", OracleType.Double)).Value = Disc;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_CASH", OracleType.Double)).Value = Cash;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_CHECK", OracleType.Double)).Value = Check;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_CC", OracleType.Double)).Value = CC;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_TOTAL", OracleType.Double)).Value = TotalAmt;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_CHANGE", OracleType.Double)).Value = Change;
                OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.Number)).Value = EmpID;
                OraCmd.Parameters.Add(new OracleParameter("SHIFTID", OracleType.Number)).Value = ShiftID;
                OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.Number)).Value = StID;
                OraCmd.Parameters.Add(new OracleParameter("STOREID", OracleType.Number)).Value = StoreID;
                OraCmd.Parameters.Add(new OracleParameter("TRANS_id", OracleType.Number)).Direction = ParameterDirection.Output;
                ObjOrdDb = new clsDatabase();
                if (ObjOrdDb.ProcedureExecutescaler(OraCmd) == true)
                {
                    TransID = Convert.ToInt64(OraCmd.Parameters["TRANS_id"].Value.ToString());
                    return TransID;
                }
                else
                    return 0;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return 0;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: InsertPayment" + ex.Message);
            }
        }

        public DataSet GetAllTempData(int StoreID, int StationID)
        {
            dsInformation = new DataSet();
            ObjOrdDb = new clsDatabase();
            OracleCommand OraCmd = new OracleCommand("CW_GETALLTEMPDATA");
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add(new OracleParameter("pSTORE", OracleType.Number)).Value = StoreID;
            OraCmd.Parameters.Add(new OracleParameter("pSTATION", OracleType.Number)).Value = StationID;
            OraCmd.Parameters.Add(new OracleParameter("pTEMP", OracleType.Cursor)).Direction = ParameterDirection.Output;
            try
            {
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: GetAllTempData()" + ex.Message);
                return null;
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public DataSet GetServicesByStoreID(int StoreID)
        {
            dsInformation = new DataSet();
            ObjOrdDb = new clsDatabase();
            OracleCommand OraCmd = new OracleCommand("CW_GETSERVICESBYSTOREID");
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add(new OracleParameter("pSTORE", OracleType.Number)).Value = StoreID;
            OraCmd.Parameters.Add(new OracleParameter("pSERVICE", OracleType.Cursor)).Direction = ParameterDirection.Output;
            try
            {
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: GetServicesByStoreID()" + ex.Message);
                return null;
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public DataSet GetPOSButtonsByStoreID(int StoreID)
        {
            dsInformation = new DataSet();
            ObjOrdDb = new clsDatabase();
            OracleCommand OraCmd = new OracleCommand("CW_GETPOSBYSTOREID");
            OraCmd.CommandType = CommandType.StoredProcedure;
            OraCmd.Parameters.Add(new OracleParameter("cwSTORE", OracleType.Number)).Value = StoreID;
            OraCmd.Parameters.Add(new OracleParameter("cwPOS", OracleType.Cursor)).Direction = ParameterDirection.Output;
            try
            {
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsChecks: GetPOSButtonsByStoreID()" + ex.Message);
                return null;
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        #endregion

        #region "clsDeposit"

        public DataSet GetChecksDeposit(int STID, int EmpID, int StoreID)
        {
            try
            {
                dsInformation = new DataSet();
                ObjOrdDb = new clsDatabase();
                OracleCommand OraCmd = new OracleCommand("CHK_GETCHECKSDEPOSIT");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("POUT", OracleType.Cursor)).Direction = ParameterDirection.Output;
                OraCmd.Parameters.Add(new OracleParameter("EID", OracleType.Number)).Value = EmpID;
                OraCmd.Parameters.Add(new OracleParameter("SFTID", OracleType.Number)).Value = 1;
                OraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
                OraCmd.Parameters.Add(new OracleParameter("ST_ID", OracleType.Number)).Value = STID;
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsDeposit: GetChecksDeposit() : " + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public DataSet GetChecksDepositForMaker(int STID, int StoreID)
        {
            try
            {
                dsInformation = new DataSet();
                ObjOrdDb = new clsDatabase();
                OracleCommand OraCmd = new OracleCommand("CHK_GETCHECKSDEPOSIT");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("POUT", OracleType.Cursor)).Direction = ParameterDirection.Output;
                OraCmd.Parameters.Add(new OracleParameter("EID", OracleType.Number)).Value = 0;
                OraCmd.Parameters.Add(new OracleParameter("SFTID", OracleType.Number)).Value = 1;
                OraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = StoreID;
                OraCmd.Parameters.Add(new OracleParameter("ST_ID", OracleType.Number)).Value = STID;
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(OraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsDeposit: GetChecksDeposit() : " + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public bool DeleteChecks(long Tid, double Amt, int StationID, int EmpID)
        {
            try
            {
                dsInformation = new DataSet();
                ObjOrdDb = new clsDatabase();
                OracleCommand OraCmd = new OracleCommand("CHK_DELETECHECKS");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("TID", OracleType.Number)).Value = Tid;
                OraCmd.Parameters.Add(new OracleParameter("AMT", OracleType.Number)).Value = Amt;
                OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.Number)).Value = EmpID;
                OraCmd.Parameters.Add(new OracleParameter("SHIFTID", OracleType.Number)).Value = 1;
                OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.Number)).Value = StationID;
                if (ObjOrdDb.ProcedureExecute(OraCmd) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsDeposit: DeleteChecks() : " + ex.Message);
            }
        }

        public bool DeleteTransaction(long Tid, string Dt, string EmpID, int StoreID, int StationID)
        {
            try
            {
                dsInformation = new DataSet();
                ObjOrdDb = new clsDatabase();
                OracleCommand OraCmd = new OracleCommand("CHK_DELETETRANS");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("TRANSID", OracleType.Number)).Value = Tid;
                OraCmd.Parameters.Add(new OracleParameter("EMPID", OracleType.VarChar, 200)).Value = EmpID.ToString().Trim();
                OraCmd.Parameters.Add(new OracleParameter("SHIFTID", OracleType.Number)).Value = 1;
                OraCmd.Parameters.Add(new OracleParameter("STATIONID", OracleType.Number)).Value = StationID;
                OraCmd.Parameters.Add(new OracleParameter("STOREID", OracleType.Number)).Value = StoreID;
                OraCmd.Parameters.Add(new OracleParameter("STDT", OracleType.VarChar, 200)).Value = Dt;
                if (ObjOrdDb.ProcedureExecute(OraCmd) == true)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return default(Boolean);
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsSearchCheck: DeleteTransaction() : " + ex.Message);
            }
        }

        #endregion

        #region "clsHome"

        public DataSet GetVarifyStatusByEmpID(long PEMPID)
        {
            dsInformation = new DataSet();
            OracleCommand oraCmd = new OracleCommand("CW_GETVARIFYSTATUSBYEMPID");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("PCW", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("PEMPID", OracleType.Number)).Value = PEMPID;
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                oraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsHome: CW_GETVARIFYSTATUSBYEMPID() : " + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        public DataSet GetVarifyChkDetail(int PVARID)
        {
            dsInformation = new DataSet();
            OracleCommand oraCmd = new OracleCommand("CW_GETVARIFYCHKDTL");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("PCW", OracleType.Cursor)).Direction = ParameterDirection.Output;
            oraCmd.Parameters.Add(new OracleParameter("PVARID", OracleType.Number)).Value = PVARID;
            try
            {
                ObjOrdDb = new clsDatabase();
                dsInformation = ObjOrdDb.ProcedureExecuteDataset(oraCmd);
                if ((dsInformation != null) && ((dsInformation.Tables.Count > 0) && (dsInformation.Tables[0].Rows.Count > 0)))
                    return dsInformation;
                else
                    return null;
                oraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsHome: CW_GETVARIFYCHKDTL() : " + ex.Message);
            }
            finally
            {
                dsInformation.Dispose();
            }
        }

        #endregion

        #endregion

    }
}