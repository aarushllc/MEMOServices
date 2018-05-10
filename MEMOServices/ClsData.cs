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

        #region "ClsAdmin"

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

        #endregion

        #region "ClsLoadUnloadMO"

        public void CW_MEMO_LOADMEMO(int PSTOREID, int PSTARTSEQ, int PENDSEQ, int PCURRENTSEQ, int PSTARTCHKNO, int PENDCHKNO, int PNOOFMEMOLEFT, string PISPROCESSIOG, int PCREATEDBY)
        {
            OracleCommand oraCmd = new OracleCommand("CW_MEMO_LOADMEMO");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("PSTOREID", OracleType.Number)).Value = PSTOREID;
            oraCmd.Parameters.Add(new OracleParameter("PSTARTSEQ", OracleType.Number)).Value = PSTARTSEQ;
            oraCmd.Parameters.Add(new OracleParameter("PENDSEQ", OracleType.Number)).Value = PENDSEQ;
            oraCmd.Parameters.Add(new OracleParameter("PCURRENTSEQ", OracleType.Number)).Value = PCURRENTSEQ;
            oraCmd.Parameters.Add(new OracleParameter("PSTARTCHKNO", OracleType.Number)).Value = PSTARTCHKNO;
            oraCmd.Parameters.Add(new OracleParameter("PENDCHKNO", OracleType.Number)).Value = PENDCHKNO;
            oraCmd.Parameters.Add(new OracleParameter("PNOOFMEMOLEFT", OracleType.Number)).Value = PNOOFMEMOLEFT;
            oraCmd.Parameters.Add(new OracleParameter("PISPROCESSIOG", OracleType.VarChar, 2000)).Value = PISPROCESSIOG;
            oraCmd.Parameters.Add(new OracleParameter("PCREATEDBY", OracleType.Number)).Value = PCREATEDBY;
            try
            {
                clsDatabase objOradb = new clsDatabase();
                if (objOradb.ProcedureExecute(oraCmd) == true)
                {
                    //return true;
                }
                else
                {
                    //return false;
                }
            }
            catch (Exception ex)
            {
                //return false;
                //ObjCommon.WriteErrMsg(ex.Message);
            }
        }

        #endregion

        #region "ClsLogin"

        public DataSet Login(string UserName, string Password, string SysDt)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_REGISTERSTATUS");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pEMPID", OracleType.Number)).Value = System.Convert.ToInt32(UserName.ToString().Trim());
                oraCmd.Parameters.Add(new OracleParameter("pEMPPASS", OracleType.VarChar, 2000)).Value = Password.ToString().Trim();
                //oraCmd.Parameters.Add(new OracleParameter("pSTATIONID", OracleType.Number)).Value = ModGlobal.ApplicationSettings.Station_ID;
                oraCmd.Parameters.Add(new OracleParameter("pSTDT", OracleType.VarChar)).Value = SysDt;
                //oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = ModGlobal.Store.Store_ID;
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

        public string Login1(string UserName, string Password, int Station, string SysDt)
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

        public DataSet GetEmpDetails(string EmpID)
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
                    return null /* TODO Change to default(_) if this is not a reference type */;

                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //ObjCommon = new clsCommon();
                //ObjCommon.WriteErrMsg("clsLogin: GetEmpDetails() : " + ex.Message);
            }

        }

        public DataSet GetStoreDetail(long Sid)
        {
            try
            {
                OracleCommand OraCmd = new OracleCommand("CHK_GETSTOREDTL");
                OraCmd.CommandType = CommandType.StoredProcedure;
                OraCmd.Parameters.Add(new OracleParameter("SID", OracleType.Number)).Value = Sid;
                OraCmd.Parameters.Add(new OracleParameter("pSTORE", OracleType.Cursor)).Direction = ParameterDirection.Output;
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
                //ObjCommon.WriteErrMsg("clsLogin: GetStoreDetail() : " + ex.Message);
            }
        }

        public DataSet MO_Login(string PassCode)
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
                    return null /* TODO Change to default(_) if this is not a reference type */;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsLogin: Login : " + ex.Message);
            }
        }

        public DataSet GetServiceDetailForMemo()
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
                    return null /* TODO Change to default(_) if this is not a reference type */;
                OraCmd.Dispose();
            }
            catch (Exception ex)
            {
                return null /* TODO Change to default(_) if this is not a reference type */;
                //ObjCommon = new clscommon();
                //ObjCommon.WriteErrMsg("clsLogin: GetStoreDetail() : " + ex.Message);
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

        #region "ClsMOProcess"

        public DataSet CW_MEMO_ISPROCESS(int pSTOREID)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_MEMO_CHKPROCESS");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = pSTOREID;
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

        public bool UpdateIsProcessMemo(int pSTOREID, string pFLAG)
        {
            OracleCommand oraCmd = new OracleCommand("CW_UPDATEMEMOISPROCESS");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = pSTOREID;
            oraCmd.Parameters.Add(new OracleParameter("pFLG", OracleType.VarChar, 1)).Value = pFLAG;
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

        public long CW_MEMO_TRAN(int PID, int PBATCHID, int PCUSTID, System.DateTime PTRANSDT, double PTRANSAMT, long PSERIALNO, string PCREATEDBY, string PCOMPNAME, int PCHKNO, int PSTOREID, int PSTATIONID, double PFEES, int PSERVICEID, int RID, int PMODE, int TQTY, int TDISC, int ExeId)
        {
            long OutVal = 0;
            DataSet DsMoProcess = new DataSet();
            OracleCommand oraCmd = new OracleCommand("CW_MEMO_TRAN");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oraCmd.Parameters.Add(new OracleParameter("PID", OracleType.Number)).Value = PID;
            oraCmd.Parameters.Add(new OracleParameter("PBATCHID", OracleType.Number)).Value = PBATCHID;
            oraCmd.Parameters.Add(new OracleParameter("PCUSTID", OracleType.Number)).Value = PCUSTID;
            oraCmd.Parameters.Add(new OracleParameter("PTRANSDT", OracleType.DateTime)).Value = PTRANSDT;
            oraCmd.Parameters.Add(new OracleParameter("PTRANSAMT", OracleType.Number)).Value = PTRANSAMT;
            oraCmd.Parameters.Add(new OracleParameter("PSERIALNO", OracleType.Number)).Value = PSERIALNO;
            oraCmd.Parameters.Add(new OracleParameter("PCREATEDBY", OracleType.VarChar, 2000)).Value = PCREATEDBY;
            oraCmd.Parameters.Add(new OracleParameter("PCOMPNAME", OracleType.VarChar, 250)).Value = PCOMPNAME;
            oraCmd.Parameters.Add(new OracleParameter("PCHKNO", OracleType.Number)).Value = PCHKNO;
            oraCmd.Parameters.Add(new OracleParameter("PSTOREID", OracleType.Number)).Value = PSTOREID;
            oraCmd.Parameters.Add(new OracleParameter("PSTATIONID", OracleType.Number)).Value = PSTATIONID;
            oraCmd.Parameters.Add(new OracleParameter("PFEES", OracleType.Double)).Value = PFEES;
            oraCmd.Parameters.Add(new OracleParameter("PSERVICEID", OracleType.Number)).Value = PSERVICEID;
            oraCmd.Parameters.Add(new OracleParameter("RID", OracleType.Number)).Value = RID;
            oraCmd.Parameters.Add(new OracleParameter("PMODE", OracleType.Number)).Value = PMODE;
            oraCmd.Parameters.Add(new OracleParameter("TQTY", OracleType.Number)).Value = TQTY;
            oraCmd.Parameters.Add(new OracleParameter("TDISC", OracleType.Number)).Value = TDISC;
            oraCmd.Parameters.Add(new OracleParameter("GTRANSID", OracleType.Number)).Direction = ParameterDirection.Output;
            //oraCmd.Parameters.Add(new OracleParameter("PSNAME", OracleType.VarChar, 250)).Value = ModGlobal.Store.ServiceName.ToString.Trim;
            oraCmd.Parameters.Add(new OracleParameter("pEXEID", OracleType.Number)).Value = ExeId;
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

        public bool InsertMEMOEVENTLOG(string EventDisc, string OperatorId)
        {
            OracleCommand oraCmd = new OracleCommand("CW_MEMOEVENT_LOG");
            oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
            //oraCmd.Parameters.Add(new OracleParameter("pSTOREID", OracleType.Number)).Value = ModGlobal.Store.Store_ID;
            //oraCmd.Parameters.Add(new OracleParameter("pTERMINALID", OracleType.Number)).Value = ModGlobal.Store.AgentId;
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

        public DataSet GetMemoEventLog(string EventDate)
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

        public DataSet CW_MEMO_DAILYTRAN(string pSTDT, int EmpId)
        {
            try
            {
                OracleCommand oraCmd = new OracleCommand("CW_MEMO_DAILYRPT");
                oraCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oraCmd.Parameters.Add(new OracleParameter("pSYSDATE", OracleType.VarChar)).Value = Convert.ToDateTime(pSTDT).ToString("dd-MMM-yy");
                oraCmd.Parameters.Add(new OracleParameter("pEmpId", OracleType.Number)).Value = EmpId;
                //oraCmd.Parameters.Add(new OracleParameter("pStoreId", OracleType.Number)).Value = System.Convert.ToInt32(ModGlobal.Store.Store_ID);
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

    }
}