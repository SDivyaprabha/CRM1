using System;
using System.Collections.Generic;
using System.Data;
using CRM.DataLayer;
using System.Data.SqlClient;

namespace CRM.BusinessLayer
{
    class UserBL
    {
        #region Methods
        public static DataTable GetUser(){return UserDL.GetUser();}
     
        public static DataSet GetUserDetails(int argUserId, string argActivityId) { return UserDL.GetUserDetails(argUserId); }
     
        public static DataSet GetUserActivityTrans(int argUserId) { return UserDL.GetUserActivityTrans(argUserId); }
   
        public static bool UserFound(int argUserId) { return UserDL.UserFound(argUserId); }
  
        public static void DeleteUser(int argUserId) { UserDL.DeleteUser(argUserId); }
  
        public static DataSet PopulateDetails() { return UserDL.PopulateDetails(); }
   
        public static DataTable GetUserInfo(int argUserId) { return UserDL.GetUserInfo(argUserId); }
   
        public static bool UserNameFound(int argUserId, string argUserName) { return UserDL.UserNameFound(argUserId, argUserName); }
   
        public static DataTable GetUsers()
        {
            return UserDL.GetUsers();
        }
        public static DataSet GetUserPerDetails(int argUserId,string argPerType)
        {
            return UserDL.GetUserPerDetails(argUserId,argPerType);
        }

    
        #endregion

        internal static string GetColumn(string p)
        {
            return UserDL.GetColumn(p);
        }

        internal static string Decrypt(string check_pwd)
        {
            return UserDL.Decrypt(check_pwd);
        }
    }
}
