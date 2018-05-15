using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.Data;
using System.IO;
using System.Web;

namespace MEMOServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        #region Variable Declaration

        DataSet dsReturn = new DataSet("Return");
        DataTable dtResult = new DataTable("Result");
        DataTable dtData = new DataTable("Data");
        ClsData objData = new ClsData();

        string ErrorMailSendTo = "khengar.kher@aarushinfoweb.com";

        #endregion

        #region Default
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        #endregion

        #region ClsAdmin

        public DataSet GetEmployeeList(long MoGrpID)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetEmployeeList(MoGrpID);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "EmpList";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetEmployeeList", ex.Message.ToString());
                return null;
            }
        }

        public DataSet GetAllGroups()
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetAllGroups();
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "GrpList";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetAllGroups", ex.Message.ToString());
                return null;
            }
        }

        public bool UpdateEmpPasscode(long MoID, long EmpId, string Passcode, string Flg)
        {
            objData = new ClsData();
            bool OutFlg = objData.UpdateEmpPasscode(MoID, EmpId, Passcode, Flg);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region "ClsLoadUnloadMO"

        public bool LoadMEMO(int StoreID, int StartSeq, int EndSeq, int CurrentSeq, int StartChkNo, int EndChkNo, int NumberOfMemoLeft, string IsProcessIOG, int CreatedBy)
        {
            objData = new ClsData();
            bool OutFlg = objData.LoadMEMO(StoreID, StartSeq, EndSeq, CurrentSeq, StartChkNo, EndChkNo, NumberOfMemoLeft, IsProcessIOG, CreatedBy);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ClsLogin

        public DataSet Login_RegisterStatus(string UserName, string Password, string SysDt, int StoreID, int StationID)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");

            if ((dtResult.Columns.Count == 0))
            {
                dtResult.Columns.Add("Result");
                dtResult.Columns.Add("Code");
                dtResult.Columns.Add("Description");
                dtResult.Rows.Add();
            }

            try
            {
                dsAIPLData = objData.Login_RegisterStatus(UserName, Password, SysDt, StoreID, StationID);

                if (dsAIPLData != null)
                {
                    if (dsAIPLData.Tables[0].Rows.Count > 0)
                    {
                        dtResult.Rows[0]["Result"] = "A";
                        dtResult.Rows[0]["Code"] = "000";
                        dtResult.Rows[0]["Description"] = "Success";
                        dtResult.AcceptChanges();
                        dsReturn.Tables.Add(dtResult);

                        for (int intFor = 0; intFor <= dsAIPLData.Tables.Count - 1; intFor++)
                        {
                            dsReturn.Tables.Add(dsAIPLData.Tables[intFor].Copy());
                        }
                    }
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No record found!";

                    dtResult.AcceptChanges();
                    dsReturn.Tables.Add(dtResult);
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "Login_RegisterStatus", ex.Message.ToString());
                dtResult.Rows[0]["Result"] = "R";
                dtResult.Rows[0]["Code"] = "001";
                dtResult.Rows[0]["Description"] = ex.Message.ToString();

                dtResult.AcceptChanges();
                dsReturn.Tables.Add(dtResult);
                return dsReturn;
            }
            finally
            {
                dsAIPLData = null;
                objData = null;
            }
        }

        public string Login_Employee(string UserName, string Password, int Station, string SysDt)
        {
            string StrOutput = "";
            try
            {
                StrOutput = objData.Login_Employee(UserName, Password, Station, SysDt);
                return StrOutput;
            }
            catch (Exception)
            {
                return "FAIL";
            }
        }

        public DataSet Login_MO(string PassCode)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.Login_MO(PassCode);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "tblMO";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "Login_MO", ex.Message.ToString());
                return null;
            }
        }

        public DataSet GetEmpDetailsFromEmpID(string EmpID)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetEmpDetailsFromEmpID(EmpID);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "EmpDetails";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetEmpDetailsFromEmpID", ex.Message.ToString());
                return null;
            }
        }

        public DataSet GetStoreDetailFrmStoreID(long StoreID)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetStoreDetailFrmStoreID(StoreID);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "StoreDtl";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetStoreDetailFrmStoreID", ex.Message.ToString());
                return null;
            }
        }

        public DataSet GetServiceDetailForMEMO()
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetServiceDetailForMEMO();
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "TblService";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetServiceDetailForMEMO", ex.Message.ToString());
                return null;
            }
        }

        #endregion

        #region ClsMOProcess

        public DataSet MEMO_IsProcess(int StoreID)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.MEMO_IsProcess(StoreID);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "tblMEMO";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "MEMO_IsProcess", ex.Message.ToString());
                return null;
            }
        }

        public bool UpdateIsProcessMEMO(int StoreID, string Flg)
        {
            objData = new ClsData();
            bool OutFlg = objData.UpdateIsProcessMEMO(StoreID, Flg);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long MEMO_Transaction(int PID, int BatchID, int CustID, System.DateTime TransDate, double TransAmt, long SerialNo, string CreatedBy, string CmpName, int ChkNo, int StoreID, int StationID, double Fees, int ServiceID, int RID, int Mode, int Qty, int Disc, int ExeId, string ServiceName)
        {
            try
            {
                long ReturnVal = 0;
                objData = new ClsData();
                //ReturnVal = objData.MEMO_Transaction(PID, BatchID, CustID, TransDate, TransAmt, SerialNo, CreatedBy, CmpName, ChkNo, StoreID, StationID, Fees, RID, Mode, Qty, Disc, ExeId, ServiceName);

                if (ReturnVal > 0)
                {
                    return ReturnVal;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool InsertMEMO_EventLog(string EventDisc, string OperatorId, int StoreID, int AgentID)
        {
            objData = new ClsData();
            bool OutFlg = objData.InsertMEMO_EventLog(EventDisc, OperatorId, StoreID, AgentID);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetMEMOEventLog(string EventDate)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetMEMOEventLog(EventDate);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "tblEventLog";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetMEMOEventLog", ex.Message.ToString());
                return null;
            }
        }

        #endregion

        #region ClsMoReports

        public DataSet MEMO_DailyTransaction(string SysDate, int EmpId, int StoreID)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");

            if ((dtResult.Columns.Count == 0))
            {
                dtResult.Columns.Add("Result");
                dtResult.Columns.Add("Code");
                dtResult.Columns.Add("Description");
                dtResult.Rows.Add();
            }

            try
            {
                dsAIPLData = objData.MEMO_DailyTransaction(SysDate, EmpId, StoreID);

                if (dsAIPLData != null)
                {
                    if (dsAIPLData.Tables[0].Rows.Count > 0)
                    {
                        dtResult.Rows[0]["Result"] = "A";
                        dtResult.Rows[0]["Code"] = "000";
                        dtResult.Rows[0]["Description"] = "Success";
                        dtResult.AcceptChanges();
                        dsReturn.Tables.Add(dtResult);

                        for (int intFor = 0; intFor <= dsAIPLData.Tables.Count - 1; intFor++)
                        {
                            dsReturn.Tables.Add(dsAIPLData.Tables[intFor].Copy());
                        }
                    }
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No record found!";

                    dtResult.AcceptChanges();
                    dsReturn.Tables.Add(dtResult);
                }
                return dsReturn;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "MEMO_DailyTransaction", ex.Message.ToString());
                dtResult.Rows[0]["Result"] = "R";
                dtResult.Rows[0]["Code"] = "001";
                dtResult.Rows[0]["Description"] = ex.Message.ToString();

                dtResult.AcceptChanges();
                dsReturn.Tables.Add(dtResult);
                return dsReturn;
            }
            finally
            {
                dsAIPLData = null;
                objData = null;
            }
        }

        #endregion

        #region clsNewCustomer

        public long Insert_Update_Customer(string Cust_Name, string Cust_FName, string Cust_MName, string Cust_LName, string Cust_Add1, string Cust_Add2,
                                string Cust_City, string Cust_State, string Cust_Zip, string Cust_Ph1, string Cust_Ph2, DateTime Cust_DOB,
                                string LicenseID, string SSN, DateTime LicIssuedON, DateTime LicExpiredON, string CreatedBy, char IsOFACVerified,
                                string Cust_Message, string Cust_DBA, string Cust_EIN, string Cust_Height, string Cust_Weight, char Cust_Gender,
                                double Fees, string Cust_IDType, string Mode)
        {
            try
            {
                long ReturnVal = 0;
                objData = new ClsData();

                //ReturnVal = objData.Insert_Update_Customer(Cust_Name, Cust_FName, Cust_MName, Cust_LName, Cust_Add1, Cust_Add2, Cust_City, Cust_State, Cust_Zip, Cust_Ph1, Cust_Ph2, Cust_DOB, LicenseID, SSN, LicIssuedON, LicExpiredON, CreatedBy, IsOFACVerified, Cust_Message, Cust_DBA, Cust_EIN, Cust_Height, Cust_Weight, Cust_Gender, Cust_IDType, Mode);

                if (ReturnVal > 0)
                {
                    return ReturnVal;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        #endregion

        #region clsNewCustomer

        public DataSet fillcustomerView()
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.fillcustomerView();
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "tblCust";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "fillcustomerView", ex.Message.ToString());
                return null;
            }
        }

        public DataSet GetCustDetail()
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetCustDetail();
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "tblCustDtl";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetCustDetail", ex.Message.ToString());
                return null;
            }
        }

        public bool IsBadprocedure(int custid, int transid, double chkamt, string remark)
        {
            objData = new ClsData();
            bool OutFlg = objData.IsBadprocedure(custid, transid, chkamt, remark);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Recovery(int custid, int tranid, double chkamt)
        {
            objData = new ClsData();
            bool OutFlg = objData.Recovery(custid, tranid, chkamt);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Collected(int custid, int tranid, double chkamt)
        {
            objData = new ClsData();
            bool OutFlg = objData.Collected(custid, tranid, chkamt);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetCustUtilPayment(string pid)
        {
            objData = new ClsData();
            DataSet dsAIPLData = new DataSet("AIPLData");
            try
            {
                if ((dtResult.Columns.Count == 0))
                {
                    dtResult.Columns.Add("Result");
                    dtResult.Columns.Add("Code");
                    dtResult.Columns.Add("Description");
                    dtResult.Rows.Add();
                }

                dsAIPLData = objData.GetCustUtilPayment(pid);
                if (dsAIPLData != null)
                {
                    dtResult.Rows[0]["Result"] = "A";
                    dtResult.Rows[0]["Code"] = "000";
                    dtResult.Rows[0]["Description"] = "Success";
                    dtResult.AcceptChanges();

                    dsAIPLData.Tables.Add(dtResult.Copy());
                    dsAIPLData.Tables[0].TableName = "tblCustDtl";
                }
                else
                {
                    dtResult.Rows[0]["Result"] = "R";
                    dtResult.Rows[0]["Code"] = "001";
                    dtResult.Rows[0]["Description"] = "No Data Found";
                    dtResult.AcceptChanges();
                    dsAIPLData = new DataSet();
                    dsAIPLData.Tables.Add(dtResult.Copy());
                }
                return dsAIPLData;
            }
            catch (Exception ex)
            {
                SendEMail(ErrorMailSendTo, "GetCustUtilPayment", ex.Message.ToString());
                return null;
            }
        }

        public bool UpdateUtility(string Cust_ID, string AccountNo, string UtilityCode)
        {
            objData = new ClsData();
            bool OutFlg = objData.UpdateUtility(Cust_ID, AccountNo, UtilityCode);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateIsProcessMEMO(string Cust_ID, string AccountNo, string UtilityCode, string Isdelete)
        {
            objData = new ClsData();
            bool OutFlg = objData.UpdateCustomerUtility(Cust_ID, AccountNo, UtilityCode, Isdelete);

            if (OutFlg == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region GeneralFunctions

        public string SendEMail(string Url, string EmailaAdress, string Subject)
        {
            // return "SUCCESS";

            if (((Url.Trim().Length == 0)
            || ((EmailaAdress.Trim().Length == 0)
            || (Subject.Trim().Length == 0))))
            {
                return "Please provide Propoer Value";
            }
            try
            {
                if (sendMail(Url, EmailaAdress, Subject) == true)
                {
                    return "SUCCESS";
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception)
            {
                return "FAIL";
            }
        }

        public bool sendMail(string Url, string EmailaAdress, string Subject)
        {
            try
            {
                StringWriter sw = new StringWriter();
                HttpContext.Current.Server.Execute(Url, sw);
                string html = sw.ToString();
                EmailCode email = new EmailCode();
                email.SendMail(html, EmailaAdress, Subject);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}
