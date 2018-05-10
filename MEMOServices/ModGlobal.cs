using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEMOServices
{
    public static class ModGlobal
    {
        public struct ApplicationSettings
        {
            public string App;
            private static string AppPath;
            private static string ParentDirPath;
            private static string MemoEventLogPath;
            private static int Station_ID;
            private static string ComPort;
            private static string PrinterType;
            private static string ScannerType;
            private static string LicImgpath;
            private static string TempDirPath;
            private static int Exe_ID;
            private static string ImgPdf;
            private static long MaxCustRecords;
            private static double MOSFV;
            private static double Highdollaramt;
            private static string MemoEmailId;
            private static string PrintReceipt;
            private static string EmailUserId;
            private static string EmailuserPwd;
            private static string MemoEmailId1;
            private static string SmtpServer;
            private static string SmtpUserId;
            private static string SmtpPassword;
            private static string FTPIPAddress;
            private static string FTPUserName;
            private static string FTPPassword;
            private static string FTPPort;
            private static string SFTPIPAddress;
            private static string SFTPUserName;
            private static string SFTPPassword;
            private static string SFTPPort;
        }

        public struct Customer
        {
            private static Int64 Cust_Id;
            private static string Cust_Name;
            private static string Cust_FName;
            private static string Cust_MName;
            private static string Cust_LName;
            private static string Cust_Address1;
            private static string Cust_Address2;
            private static string Cust_City;
            private static string Cust_State;
            private static string Cust_Zip;
            private static string Cust_Phone1;
            private static string Cust_Phone2;
            private static string Cust_DOB;
            private static string Cust_SSN;
            private static string Cust_LicenceID;
            private static string Cust_DLState;
            private static string Cust_DLIssuedOn;
            private static string Cust_DLExpiresOn;
            private static string Cust_Height;
            private static string Cust_Weight;
            private static string Cust_Sex;
            private static string CreatedOn;
            private static string CreatedBy;
            private static string ISOFACVERIFIED;
            private static string Cust_Message;
            private static string ChkFees;
            private static string Cust_DBA;
            private static string Cust_EIN;
            private static string StoreId;
            private static string Cust_BadChecks;
            private static string Cust_CheckCashed;
            private static string Cust_IDType;
            private static double TodayTotal;
            public string Cust_Test;
        }

        public struct Store
        {
            //public string Store;
            private static string AgentId;
            private static string Store_ID;
            private static string Store_Name;
            private static string Store_Add1;
            private static string Store_Add2;
            private static string Store_City;
            private static string Store_State;
            private static string Store_Zip;
            private static string Store_Phone;
            private static string Store_AccNO;
            private static string Store_RtngNo;
            private static string Store_FTP;
            private static string Store_EIN;
            private static string manager;
            private static int MID;
            private static int ServiceId;
            private static double ServiceFee;
            private static string ServiceName;
            private static string MemoStoreComm;
            private static string MemoServiceComm;
            private static string ComplianceOfficer;
        }

        public struct Password
        {
            public string test;
            private static string UserName;
            //private static string Password;
            private static Guid BatchID;
        }

        public struct Employee
        {
            public string Emp;
            private static string Emp_ID;
            private static string EmpName;
            private static string EmpIsManager;
            private static string PassCode;
            private static bool MEMOISMANAGER;
            private static string MEMOID;
        }
    }
}