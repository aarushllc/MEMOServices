using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;

namespace MEMOServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        #region Default

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        #endregion

        #region ClsAdmin

        [OperationContract]
        DataSet GetEmployeeList(long MoGrpID);

        [OperationContract]
        DataSet GetAllGroups();

        [OperationContract]
        bool UpdateEmpPasscode(long MoID, long EmpId, string Passcode, string Flg);

        #endregion

        #region ClsLoadUnloadMO

        [OperationContract]
        bool LoadMEMO(int StoreID, int StartSeq, int EndSeq, int CurrentSeq, int StartChkNo, int EndChkNo, int NumberOfMemoLeft, string IsProcessIOG, int CreatedBy);

        #endregion

        #region ClsLogin

        [OperationContract]
        DataSet Login_RegisterStatus(string UserName, string Password, string SysDt, int StoreID, int StationID);

        [OperationContract]
        string Login_Employee(string UserName, string Password, int Station, string SysDt);

        [OperationContract]
        DataSet Login_MO(string PassCode);

        [OperationContract]
        DataSet GetEmpDetailsFromEmpID(string EmpID);

        [OperationContract]
        DataSet GetStoreDetailFrmStoreID(long StoreID);

        [OperationContract]
        DataSet GetServiceDetailForMEMO();

        #endregion

        #region ClsMOProcess

        [OperationContract]
        DataSet MEMO_IsProcess(int StoreID);

        [OperationContract]
        bool UpdateIsProcessMEMO(int StoreID, string Flg);

        [OperationContract]
        long MEMO_Transaction(int PID, int BatchID, int CustID, System.DateTime TransDate, double TransAmt, long SerialNo, string CreatedBy, string CmpName, int ChkNo, int StoreID, int StationID, double Fees, int ServiceID, int RID, int Mode, int Qty, int Disc, int ExeId, string ServiceName);

        [OperationContract]
        bool InsertMEMO_EventLog(string EventDisc, string OperatorId, int StoreID, int AgentID);

        [OperationContract]
        DataSet GetMEMOEventLog(string EventDate);

        #endregion

        #region ClsMoReports

        [OperationContract]
        DataSet MEMO_DailyTransaction(string SysDate, int EmpId, int StoreID);

        #endregion

        #region clsNewCustomer

        long Insert_Update_Customer(string Cust_Name, string Cust_FName, string Cust_MName, string Cust_LName, string Cust_Add1, string Cust_Add2,
                                string Cust_City, string Cust_State, string Cust_Zip, string Cust_Ph1, string Cust_Ph2, DateTime Cust_DOB,
                                string LicenseID, string SSN, DateTime LicIssuedON, DateTime LicExpiredON, string CreatedBy, char IsOFACVerified,
                                string Cust_Message, string Cust_DBA, string Cust_EIN, string Cust_Height, string Cust_Weight, char Cust_Gender,
                                double Fees, string Cust_IDType, string Mode);

        #endregion

        #region clsNewCustomer

        DataSet fillcustomerView();

        DataSet GetCustDetail();

        bool IsBadprocedure(int custid, int transid, double chkamt, string remark);

        bool Recovery(int custid, int tranid, double chkamt);

        bool Collected(int custid, int tranid, double chkamt);

        DataSet GetCustUtilPayment(string pid);

        bool UpdateUtility(string Cust_ID, string AccountNo, string UtilityCode);

        bool UpdateIsProcessMEMO(string Cust_ID, string AccountNo, string UtilityCode, string Isdelete);

        #endregion
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
