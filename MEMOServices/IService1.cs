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
