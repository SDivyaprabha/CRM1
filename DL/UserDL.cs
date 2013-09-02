using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;


namespace CRM.DataLayer
{
    class UserDL
    {

        #region Declaration

        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("MicroMen");        
       
        public static  DataSet _GridviewDatset = new DataSet();
        public static DataSet _AlterGridviewDatset = new DataSet();

        DataTable m_tUsers;

        #endregion

        #region Methods
     
        public static DataSet ConvertArrayListToDataSet(ArrayList arr)
        {
            DataSet dsTemp = new DataSet();
            DataTable Tables = new DataTable();
            dsTemp.Tables.Add(Tables);
            dsTemp.Tables[0].Columns.Add("UserName", System.Type.GetType("System.String"));
            foreach (string str in arr)
            {
                if (str != string.Empty)
                {
                    DataRow myRow = dsTemp.Tables[0].NewRow();
                    myRow[0] = str;
                    dsTemp.Tables[0].Rows.Add(myRow);
                }
            }
            return dsTemp;
        }
        public static string Str_Decrypt(string AnyString)
        {
            string RtnStr = null;
            int No = Strings.Asc(Strings.Right(AnyString, 1));
            int I = 0;
            for (I = 1; I <= Strings.Len(AnyString) - 1; I++)
            {
                RtnStr = RtnStr + Strings.Chr(Strings.Asc(Strings.Mid(AnyString, I, 1)) - No);
            }
            return RtnStr;
        }
        public class ValueDescriptionPair
        {
            public object Value;
            public string Description;
            public ValueDescriptionPair(object NewValue, string NewDescription)
            {
                Value = NewValue;
                Description = NewDescription;
            }
            public override string ToString()
            {
                return Description;
            }
        }

        public static DataTable GetUser()
        {
            string sSql = "";
            sSql = "SELECT UserName From dbo.Users A INNER JOIN dbo.Position B ON A.PositionId=B.PositionId  " +
                    "WHERE B.PositionType='M' AND UserId<>1 " +
                    "UNION ALL " +
                    "SELECT UserName FROM dbo.Users A WHERE UserId=1 ORDER BY UserName ";
            BsfGlobal.OpenWorkFlowDB();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            DataTable dt = new DataTable();
            da.Fill(dt);
            da.Dispose();
            return dt;
        }

        public static string Insert_SingleQuot(string strString)
        {
            string functionReturnValue = null;
            // ERROR: Not supported in C#: OnErrorStatement 
            int lngTmp = Strings.InStr(1, strString, "'", CompareMethod.Binary);
            string strTmp = null;
            //UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            if ((Convert.ToBoolean(lngTmp)))
            {
                functionReturnValue = "";
                return functionReturnValue;
            }
            if (lngTmp <= 0)
            {
                functionReturnValue = strString;
                return functionReturnValue;
            }
            
            while (lngTmp > 0)
            {
                //If lngTmp > 0 Then
                strTmp = Strings.Mid(strString, 1, lngTmp);
                
                strTmp = strTmp + "'";
                strTmp = strTmp + Strings.Mid(strString, lngTmp + 1);
                strString = strTmp;
                functionReturnValue = strTmp;
                // Exit Function
                lngTmp = Strings.InStr(lngTmp + 2, strTmp, "'", CompareMethod.Binary);
                
            }
            //End If
            return functionReturnValue;
        }

        public static string Str_Encrypt(string AnyString, byte Number)
        {
            string RtnStr = null;
            short I = 0;
            for (I = 1; I <= Strings.Len(AnyString); I++)
            {
                RtnStr = RtnStr + Strings.Chr(Strings.Asc(Strings.Mid(AnyString, I, 1)) + Number);
            }
            RtnStr = RtnStr + Strings.Chr(Number);
            return RtnStr;
        }       

        public static void FillDataGridView(string qry, DataGridView mydatagridview)
        {
            SqlDataAdapter adapter = null;

            try
            {
                adapter = new SqlDataAdapter(qry, BsfGlobal.OpenWorkFlowDB());
                using (DataSet ds = new DataSet())
                {
                    adapter.Fill(ds);
                    mydatagridview.DataSource = ds.Tables[0];
                }

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void FillDataGridViewWithDynamicConn(string qry, DataGridView mydatagridview,SqlConnection conn)
        {
            SqlDataAdapter adapter = null;

            try
            {
                adapter = new SqlDataAdapter(qry, conn);
                using (DataSet ds = new DataSet())
                {
                    adapter.Fill(ds);
                    mydatagridview.DataSource = ds.Tables[0];
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
        }    

        public static void DataManipulation(string Sql)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(Sql, BsfGlobal.OpenWorkFlowDB());
                cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public static DataSet GetDataSet(string sql, string datatablename)
        {
            SqlDataAdapter adapter = null;
            DataSet ds = new DataSet();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql, BsfGlobal.OpenWorkFlowDB());
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds, datatablename);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                adapter.Dispose();
                cmd.Dispose();

            }
            return ds;
        }

        public static DataSet GetDataSetWthDynamicConn(string sql, string datatablename,SqlConnection conn)
        {
            SqlDataAdapter adapter = null;
            DataSet ds = new DataSet();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql, conn);
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds, datatablename);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                adapter.Dispose();
                cmd.Dispose();

            }
            return ds;
        }

        public static DataTable GetDataTableWithDynamicConn(string sql,SqlConnection conn)
        {
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql, conn);
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                adapter.Dispose();
                cmd.Dispose();
                conn.Close();

            }
            return dt;
        }

        public static DataTable GetDataTable(string sql)
        {
            SqlDataAdapter adapter = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(sql, BsfGlobal.OpenWorkFlowDB());
                adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                adapter.Dispose();
                cmd.Dispose();

            }
            return dt;
        }

        public static void FillCheckedListBox(string Sql, CheckedListBox checklist)
        {
            using (SqlCommand cmd = new SqlCommand(Sql, BsfGlobal.OpenWorkFlowDB()))
            {
                using (SqlDataReader mysqlDataReader = cmd.ExecuteReader())
                {
                    while (mysqlDataReader.Read())
                    {
                        checklist.Items.Add(mysqlDataReader[0].ToString(), CheckState.Checked);
                    }
                    mysqlDataReader.Close();
                }
            }
        }

        public static void FillCheckedListBoxWithDynamicConn(string Sql, CheckedListBox checklist,SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(Sql, conn))
            {
                SqlDataReader mysqlDataReader = null;
                mysqlDataReader = cmd.ExecuteReader();
                while (mysqlDataReader.Read())
                {
                    checklist.Items.Add(mysqlDataReader[0].ToString(), CheckState.Checked);
                }
                mysqlDataReader.Close();
            }

        }

        public static void FillUnCheckedListBox(string Sql, CheckedListBox checklist)
        {
            using (SqlCommand cmd = new SqlCommand(Sql, BsfGlobal.OpenWorkFlowDB()))
            {
                SqlDataReader mysqlDataReader = null;
                mysqlDataReader = cmd.ExecuteReader();
                while (mysqlDataReader.Read())
                {
                    checklist.Items.Add(mysqlDataReader[0].ToString(), CheckState.Unchecked);
                }
                mysqlDataReader.Close();
            }
        }

        public static void FillUnCheckedListBoxWithDynamicConn(string Sql, CheckedListBox checklist,SqlConnection conn)
        {
            using (SqlCommand cmd = new SqlCommand(Sql, conn))
            {
                SqlDataReader mysqlDataReader = null;
                mysqlDataReader = cmd.ExecuteReader();
                while (mysqlDataReader.Read())
                {
                    checklist.Items.Add(mysqlDataReader[0].ToString(), CheckState.Unchecked);
                }
                mysqlDataReader.Close();
            }

        }

        public static DataView GetDataView(string CommandText, SqlConnection conn)
        {
            DataSet _DataSet = new DataSet();
            SqlDataAdapter _DataAdapter = new SqlDataAdapter(CommandText, conn);
            _DataAdapter.Fill(_DataSet);
            DataTable _DataTable = _DataSet.Tables[0];
            _DataAdapter.Dispose();
            _DataAdapter = null;
            _DataSet.Dispose();
            _DataSet = null;
            DataView _DataView = new DataView(_DataTable);
            _DataTable.Dispose();
            _DataTable = null;
            return _DataView;
        }

        public static void ComboBoxFill(string Sql, ComboBox CmbBox)
        {
            try
            {
                SqlDataReader mysqlDataReader = null;
                SqlCommand cmd = null;
                CmbBox.Items.Clear();
                cmd = new SqlCommand(Sql, BsfGlobal.OpenWorkFlowDB());
                mysqlDataReader = cmd.ExecuteReader();
                while (mysqlDataReader.Read())
                {
                    CmbBox.Items.Add(mysqlDataReader[0].ToString());                   
                  
                }               
                mysqlDataReader.Close();
                cmd.Dispose();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string CheckForData(string Sqlqry)
        {
            SqlDataReader mysqlDataReader = null;
            using (SqlCommand cmd = new SqlCommand(Sqlqry, BsfGlobal.OpenWorkFlowDB()))
            {
                string checker;
                mysqlDataReader = cmd.ExecuteReader();
                if (mysqlDataReader.Read())
                {
                    checker = "yes";
                    mysqlDataReader.Close();
                    return checker;
                }
                else
                {
                    checker = "no";
                    mysqlDataReader.Close();
                    return checker;
                }
            }
        }

        public static string CheckForDataWithDynamicConn(string Sqlqry,SqlConnection conn)
        {
            SqlDataReader mysqlDataReader = null;
            using (SqlCommand cmd = new SqlCommand(Sqlqry, conn))
            {
                string checker;
                mysqlDataReader = cmd.ExecuteReader();
                if (mysqlDataReader.Read())
                {
                    checker = "yes";
                    mysqlDataReader.Close();
                    return checker;
                }
                else
                {
                    checker = "no";
                    mysqlDataReader.Close();
                    return checker;
                }
            }
        }

        public static DataView GetDataView(string CommandText)
        {
            DataView _DataView = GetDataView(CommandText, BsfGlobal.OpenWorkFlowDB());
            return _DataView;
        }

        public static string GetColumn(string strSQL)
        {
            SqlCommand cmd = null;
            string ColumnValue = "";
            try
            {
                cmd = new SqlCommand(strSQL, BsfGlobal.OpenWorkFlowDB());
                ColumnValue = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
            return ColumnValue;
        }

        public static  string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
                    StreamWriter writer = new StreamWriter(cryptoStream);
                    writer.Write(originalString);
                    writer.Flush();
                    cryptoStream.FlushFinalBlock();
                    writer.Flush();
                    return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
            }
        }

        public static  string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString)))
                {
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
                    StreamReader reader = new StreamReader(cryptoStream);
                    return reader.ReadToEnd();
                }
            }
        }

        public static string GetColumnWithDynamicConn(string strSQL,SqlConnection conn)
        {
            SqlCommand cmd = null;
            string ColumnValue = "";
            try
            {
                cmd = new SqlCommand(strSQL, conn);
                ColumnValue = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }

            return ColumnValue;
        }

        public static void DataManipulationWithDynamicConn(string Sql,SqlConnection conn)
        {
            SqlCommand cmd = null;
            try
            {
                cmd = new SqlCommand(Sql, conn);
                cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cmd.Dispose();
            }
        }
        #endregion

        #region New Methods

        public static int InsertUser(string argUserName)
        {
            int identity = 0;
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenWorkFlowDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlCommand cmd;
                    string sSql = String.Format("Insert Into dbo.Users(UserName)Values('{0}') SELECT SCOPE_IDENTITY();", argUserName);
                    BsfGlobal.OpenWorkFlowDB();
                    cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
                    identity = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                    tran.Commit();
                }
                catch (Exception ce)
                {
                    throw ce;
                }
                finally
                {
                    BsfGlobal.g_WorkFlowDB.Close();
                    conn.Dispose();
                }
            }
            return identity;
        }

        public static DataSet GetUserDetails(int argUserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da;
                BsfGlobal.OpenWorkFlowDB();

                string sSql = String.Format("Select A.*,isnull(B.CityName,'') CityName,isnull(C.StateName,'') StateName,isnull(D.CountryName,'') CountryName From"+
                    " dbo.Users A Left Join dbo.CityMaster B on A.CityId=B.CityID Left Join dbo.StateMaster C On"+
                    " A.StateId=C.StateId Left Join dbo.CountryMaster D on A.CountryId=D.CountryId Where UserId = {0}", argUserId);
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "UserMaster");
                da.Dispose();

                sSql = "Select UserId,UserName from Users";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "Users");
                da.Dispose();

                sSql = "Select PositionId,PositionName from Position Order by PositionName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "PositionMaster");
                da.Dispose();

                sSql = "Select DeptId,Dept_Name from Department Order by Dept_Name";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "DepartmentMaster");
                da.Dispose();

                sSql = "Select LevelId,LevelName from LevelMaster Order by LevelName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "LevelMaster");
                da.Dispose();

                //sSql = "Select ActivityId,ActivityName from ActivityMaster Order by ActivityName";
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "ActivityMaster");
                //da.Dispose();


                //sSql = "Select AlertID from UserAlertTrans  Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserAlertTrans");
                //da.Dispose();               

                //sSql = "Select AUserId,OrderId from UserAlternateTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserAlternateTrans");
                //da.Dispose();

                //sSql = "Select sUserId,OrderId from UserSuperiorTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserSuperiorTrans");
                //da.Dispose();

                //sSql = "Select CompanyId from UserCompanyTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserCompanyTrans");
                //da.Dispose();

                //sSql = "Select CostCentreId from UserCostCentreTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserCostCentreTrans");
                //da.Dispose();


                //sSql = "Select DashBoardId from UserDashBoardTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserDashBoardTrans");
                //da.Dispose();

                //sSql = "Select HomePageId from UserHomePageTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserHomePageTrans");
                //da.Dispose();

                //sSql = "Select RoleId,Variant from UserRoleTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserRoleTrans");
                //da.Dispose();


                //sSql = "Select A.HomePageId,A.ModuleId,B.ModuleName,A.HomePageName,A.HomePageType from HomePageMaster A " +
                //       "Inner Join Module B on A.ModuleId=B.ModuleId  Order by A.ModuleId,A.HomePageType,A.HomePageName";
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "HomePageMaster");
                //da.Dispose();

                //sSql = "Select DashBoardId,DashBoardName from DashBoardMaster";
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "DashBoardMaster");
                //da.Dispose();

                //sSql = "Select CompanyId,CompanyName from CompanyMaster";
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "CompanyMaster");
                //da.Dispose();

                sSql = "Select CostCentreId,CostCentreName from OperationalCostCentre";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "CostCentre");
                da.Dispose();
                
                //sSql = "Select A.AlertId,A.ModuleId,B.ModuleName,A.AlertName from AlertMaster A " +
                //        "Inner Join Module B on A.ModuleId=B.ModuleId  Order by A.ModuleId,A.AlertName";
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "AlertMaster");
                //da.Dispose();

                //if (argActivityId != "")
                //{

                //    //sSql = "Select Distinct A.RoleId,B.RoleName,B.RoleType from ActivityRoleTrans A " +
                //    //       "Inner Join TaskTrans B on A.RoleId=B.RoleId " +
                //    //       "Where A.ActivityId in (" + argActivityId + ")";

                //    sSql = "Select Distinct A.RoleId,B.RoleName,B.RoleType,D.ModuleId,D.ModuleName from ActivityRoleTrans A" +
                //        " Inner Join TaskTrans B on A.RoleId=B.RoleId" +
                //        " Inner Join TaskMaster C on C.TaskName=B.TaskName" +
                //        " Inner Join Module D on D.ModuleId=C.Moduleid" +
                //        " Where A.ActivityId in (" + argActivityId + ")";

                //    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //    da.Fill(ds, "ActivityRoleTrans");
                //    da.Dispose();

                //    sSql = "Select A.TaskId,B.TaskName " +
                //           "from ActivityTaskTrans A Inner Join TaskMaster B on A.TaskId=B.TaskId " +
                //           "Where A.ActivityId in (" + argActivityId + ") Order by A.TaskId";
                //    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //    da.Fill(ds, "ActivityTaskTrans");
                //    da.Dispose();
                //}

            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return ds;
        }

        public static DataSet GetUserPerDetails(int argUserId,string argPerType)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da;
                BsfGlobal.OpenWorkFlowDB();
                string sSql = "";

                if (argPerType == "A")
                {
                    sSql = "Select AlertID From dbo.UserAlertTrans Where UserId = " + argUserId;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "AlertTrans");
                    da.Dispose();

                    sSql = "Select A.AlertId,A.ModuleId,B.ModuleName,A.AlertName From dbo.AlertMaster A " +
                            "Inner Join dbo.Module B on A.ModuleId=B.ModuleId Order by A.ModuleId,A.AlertName";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "AlertMaster");
                    da.Dispose();

                }
                //sSql = "Select AUserId,OrderId from UserAlternateTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserAlternateTrans");
                //da.Dispose();

                if (argPerType == "D")
                {
                    sSql = "Select DashBoardId From dbo.UserDashBoardTrans Where UserId = " + argUserId;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "DashBoardTrans");
                    da.Dispose();

                    sSql = "Select DashBoardId,DashBoardName,DashBoardType From dbo.DashBoardMaster";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "DashBoardMaster");
                    da.Dispose();

                }

                if (argPerType == "H")
                {
                    sSql = "Select HomePageId From dbo.UserHomePageTrans Where UserId = " + argUserId;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "HomePageTrans");
                    da.Dispose();

                    sSql = "Select A.HomePageId,A.ModuleId,B.ModuleName,A.HomePageName,A.HomePageType From dbo.HomePageMaster A " +
                           "Inner Join dbo.Module B On A.ModuleId=B.ModuleId  Order by A.ModuleId,A.HomePageType,A.HomePageName";
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "HomePageMaster");
                    da.Dispose();


                }
                if (argPerType == "R")
                {
                    sSql = "Select RoleId,Variant From dbo.UserRoleTrans Where UserId = " + argUserId;
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "RoleTrans");
                    da.Dispose();

                    sSql = String.Format("Select Distinct A.RoleId,B.RoleName,B.RoleType,D.ModuleId,D.ModuleName "+
                        " From dbo.ActivityRoleTrans A Inner Join dbo.TaskTrans B on A.RoleId=B.RoleId"+
                        " Inner Join dbo.TaskMaster C on C.TaskName=B.TaskName Inner Join dbo.Module D On"+
                        " D.ModuleId=C.Moduleid Where A.ActivityId In (Select ActivityId From dbo.UserActivityTrans Where UserId= {0})", argUserId);
                    da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    da.Fill(ds, "ActivityRoleTrans");
                    da.Dispose();

                }

                //sSql = "Select A.TaskId,B.TaskName " +
                //       "from ActivityTaskTrans A Inner Join TaskMaster B on A.TaskId=B.TaskId " +
                //       "Where A.ActivityId in (Select ActivityId from UserActivityTrans Where UserId= " + argUserId + ") Order by A.TaskId";
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "ActivityTaskTrans");
                //da.Dispose();


            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return ds;
        }

        public static DataSet GetUserActivityTrans(int argUserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da;
                BsfGlobal.OpenWorkFlowDB();
                string sSql = "Select ActivityId From dbo.UserActivityTrans Where UserId=" + argUserId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "UserActivityTrans");
                da.Dispose();

                sSql = "Select CompanyId From dbo.UserCompanyTrans Where UserId = " + argUserId;
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "UserCompanyTrans");
                da.Dispose();

                //sSql = "Select CostCentreId from UserCostCentreTrans Where UserId = " + argUserId;
                //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                //da.Fill(ds, "UserCostCentreTrans");
                //da.Dispose();
            }
            catch (Exception ce)
            {

                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return ds;
        }

        public static bool UserFound(int argUserId)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = String.Format("Select UserId From dbo.UserAlternateTrans Where AUserId = {0} "+
                    " Union All Select UserId From dbo.UserSuperiorTrans Where sUserId = {0}"+
                    " Union All Select UserId from dbo.LogMaster Where UserId = {0}", argUserId);

                BsfGlobal.OpenWorkFlowDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
                dt.Dispose();
            }
            catch (Exception ce)
            {

                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }

            return bans;
        }

        public static void DeleteUser(int argUserId)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenWorkFlowDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    SqlCommand cmd;
                    string sSql = "Delete From dbo.Users Where UserId= " + argUserId;
                    BsfGlobal.OpenWorkFlowDB();
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserActivityTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserAlertTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserDashBoardTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserHomePageTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserRoleTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserAlternateTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserSuperiorTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserCompanyTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    sSql = "Delete From dbo.UserCostCentreTrans Where UserId = " + argUserId;
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    tran.Commit();
                }
                catch (Exception ce)
                {
                    tran.Rollback();
                    throw ce;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        //public DataTable GetDepartment()
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "Select DeptId, Dept_Name from Department";
        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {
        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        //public DataTable GetModule()
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "select ModuleId,ModuleName from Module ";

        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {

        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        //public DataSet GetActivityDetails(int argActivityId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter da;
        //        BsfGlobal.OpenWorkFlowDB();

        //        string sSql = "Select ActivityName,NonTask,IntervalType," +
        //                      "IntervalPeriod,ProcessType,ProcessPeriod From ActivityMaster " +
        //                      "Where ActivityId = " + argActivityId;
        //        sSql = sSql + " order by ActivityName";
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "ActivityMaster");

        //        sSql = "Select ActivityId,ActivityName from ActivityMaster Where ActivityId <> " + argActivityId + " " +
        //               "order by ActivityName";
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "PrevActivityList");

        //        sSql = "Select PActivityId from PrevActivityTrans  Where ActivityId = " + argActivityId;
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "PrevActivity");

        //        sSql = "Select RoleName,Task_Name from ActivityRoleTrans Where ActivityId = " + argActivityId;
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "RoleTrans");

        //        sSql = "Select TaskId,TaskName,MuliApprove,NoOfLevel,ValueApprove from ActivityTaskTrans Where ActivityId = " + argActivityId;
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "TaskTrans");

        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {
        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return ds;
        //}

        //public DataTable GetActivityIdfromTrans(int DeptId)
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "Select ActivityId from DepartmentActivityTrans where DeptId=" + DeptId + "";

        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {

        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        //public DataSet GetActivityTransFilter(string argActivityID)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter da;
        //        BsfGlobal.OpenWorkFlowDB();

        //        string sSql = "Select B.TaskTransId,B.RoleName,B.Task_Name,B.RoleType  from ActivityRoleTrans A Inner Join TaskTrans B on A.RoleName = B.RoleName Where A.ActivityId in (" + argActivityID + ")";

        //        BsfGlobal.OpenWorkFlowDB();
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "ActivityRoleTrans");


        //        //sSql = "Select B.TaskTransId,B.RoleName,B.Task_Name,B.RoleType  from ActivityRoleTrans A Inner Join TaskTrans B on A.RoleName = B.RoleName Where A.ActivityId in (" + argActivityID + ") and B.RoleType='V'";
        //        //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        //da.Fill(ds, "V");
        //        //da.Dispose();

        //        //sSql = "Select B.TaskTransId,B.RoleName,B.Task_Name,B.RoleType  from ActivityRoleTrans A Inner Join TaskTrans B on A.RoleName = B.RoleName Where A.ActivityId in (" + argActivityID + ") and B.RoleType='C'";
        //        //da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        //da.Fill(ds, "C");
        //        //da.Dispose();


        //        sSql = "Select * from ActivityTaskTrans Where Activityid in (" + argActivityID + ")";
        //        da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        da.Fill(ds, "ActivityTaskTrans");
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {
        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return ds;
        //}

        ////public DataTable GetTaskTransFilterApproval(string argTaskID)
        ////{
        ////    DataTable dt;
        ////    try
        ////    {
        ////        string sSql = "Select * from ActivityTaskTrans Where Activityid in (" + argTaskID + ")";
        ////        BsfGlobal.OpenWorkFlowDB();
        ////        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        ////        dt = new DataTable();
        ////        da.Fill(dt);
        ////        da.Dispose();
        ////    }
        ////    catch (Exception ce)
        ////    {

        ////        throw ce;
        ////    }
        ////    finally
        ////    {
        ////        BsfGlobal.g_WorkFlowDB.Close();
        ////    }
        ////    return dt;
        ////}

        ////public DataTable GetTaskTransFilterDt(string argTaskID)
        ////{
        ////    DataTable dt;
        ////    try
        ////    {
        ////        string sSql = "Select A.RoleName,A.Roletype From TaskTrans A Inner Join TaskMaster B on A.Task_Name = B.Task_Name Where B.TaskID in (" + argTaskID + ")";
        ////        BsfGlobal.OpenWorkFlowDB();
        ////        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        ////        dt = new DataTable();
        ////        da.Fill(dt);
        ////        da.Dispose();
        ////    }
        ////    catch (Exception ce)
        ////    {

        ////        throw ce;
        ////    }
        ////    finally
        ////    {
        ////        BsfGlobal.g_WorkFlowDB.Close();
        ////    }
        ////    return dt;
        ////}

        //public DataTable GetActivity()
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "Select ActivityId,ActivityName from ActivityMaster ";
        //        sSql = sSql + " order by ActivityName";

        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {

        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        //public DataTable GetPosition()
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "Select PositionId, PositionName from Position";
        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {
        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        //public DataTable GetCompany()
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "Select CompanyId, CompanyName from CompanyMaster";
        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {
        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        //public DataTable GetCostCentre()
        //{
        //    DataTable dt;
        //    try
        //    {
        //        string sSql = "Select CostCentreId, CostCentreName from CostCentre";
        //        BsfGlobal.OpenWorkFlowDB();
        //        SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        da.Dispose();
        //    }
        //    catch (Exception ce)
        //    {
        //        throw ce;
        //    }
        //    finally
        //    {
        //        BsfGlobal.g_WorkFlowDB.Close();
        //    }
        //    return dt;
        //}

        public static DataTable GetUsers()
        {
            DataTable dt;
            try
            {
                const string sSql = "Select UserId,UserName From dbo.Users Order by UserId";
                BsfGlobal.OpenWorkFlowDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception ce)
            {

                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataSet PopulateDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter da;
                BsfGlobal.OpenWorkFlowDB();

                string sSql = "Select DeptId,Dept_Name From dbo.Department Order by Dept_Name";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "Department");
                da.Dispose();

                sSql = "Select PositionId,PositionName From dbo.Position Order by PositionName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "PositionMaster");
                da.Dispose();

                sSql = "Select LevelId,LevelName From dbo.LevelMaster Order by LevelName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "LevelMaster");
                da.Dispose();

                sSql = "Select CityId,CityName From dbo.CityMaster Order by CityName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "CityMaster");
                da.Dispose();

                sSql = "Select StateId,StateName From dbo.StateMaster Order by StateName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "StateMaster");
                da.Dispose();

                sSql = "Select CountryId,CountryName From dbo.CountryMaster Order by CountryName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                da.Fill(ds, "CountryMaster");
                da.Dispose();

            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return ds;
        }

        public static DataTable GetUserInfo(int argUserId)
        {
            DataTable dt;
            try
            {
                string sSql = String.Format("Select A.*,isnull(B.CityName,'') CityName,isnull(C.StateName,'') StateName,"+
                    " isnull(D.CountryName,'') CountryName,isnull(E.Dept_Name,'') DeptName,isnull(F.PositionName,'') PositionName,"+
                    " isnull(G.LevelName,'') LevelName  From dbo.Users A Left Join dbo.CityMaster B on A.CityId=B.CityID"+
                    " Left Join dbo.StateMaster C on A.StateId=C.StateId Left Join dbo.CountryMaster D on A.CountryId=D.CountryId"+
                    " Left Join dbo.Department E on A.DeptId=E.DeptID Left Join dbo.Position F on A.PositionId=F.PositionId"+
                    " Left Join dbo.LevelMaster G on A.LevelId=G.LevelId Where UserId = {0}", argUserId);
                BsfGlobal.OpenWorkFlowDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static bool UserNameFound(int argUserId, string argUserName)
        {
            bool bans = false;
            try
            {
                DataTable dt;
                string sSql = String.Format("Select UserId From dbo.Users Where UserId <> {0} And UserName = '{1}'", argUserId, argUserName);
                BsfGlobal.OpenWorkFlowDB();
                SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                da.Dispose();
                if (dt.Rows.Count > 0) { bans = true; }
                dt.Dispose();
            }
            catch (Exception ce)
            {

                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }

            return bans;
        }

        public static DataTable GetRARigts(int argUserId,string argType)
        {
            DataTable dt = new DataTable();
            try
            {
                string sSql = "";
                if (argType == "T")
                {
                    sSql = String.Format("Select A.TypeId Id,A.TypeName ResourceTypeName,Case When B.ResourceTypeId is Null"+
                        " then Convert(bit,1,1) else Convert(bit,0,0) End Sel from [{0}].dbo.ResourceType A"+
                        " Left join dbo.UserResourceTypeTrans B on A.TypeId=B.ResourceTypeId and B.UserId= {1}", BsfGlobal.g_sRateAnalDBName, argUserId);
                }
                else if (argType == "G")
                {
                    sSql = String.Format("Select A.Resource_Group_ID Id,A.Resource_Group_Name ResourceGroupName,"+
                        " Case When B.Resource_Group_ID is Null then Convert(bit,1,1) else Convert(bit,0,0) End Sel From"+
                    " [{0}].dbo.Resource_Group A Left Join dbo.UserResourceGroupTrans B "+
                    " On A.Resource_Group_ID=B.Resource_Group_ID And B.UserId= {1}", BsfGlobal.g_sRateAnalDBName, argUserId);
                }
                else if (argType == "W")
                {
                    sSql = String.Format("Select A.Work_Group_ID Id,A.Work_Group_Name WorkGroupName,"+
                        " Case When B.Work_Group_ID is Null then Convert(bit,1,1) else Convert(bit,0,0) End Sel "+
                        " From [{0}].dbo.Work_Group A Left Join dbo.UserWorkGroupTrans B "+
                        " On A.Work_Group_ID=B.Work_Group_ID and B.UserId= {1}", BsfGlobal.g_sRateAnalDBName, argUserId);
                }
                if (sSql != "")
                {
                    BsfGlobal.OpenWorkFlowDB();
                    SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                    dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();
                }
            }
            catch (Exception ce)
            {

                throw ce;
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static void InsertRARights(int argUserId, string argIds,string argType)
        {
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenWorkFlowDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                try
                {
                    string sSql = "";
                    SqlCommand cmd;
                    if (argType == "T")
                    {
                        sSql = "Delete From dbo.UserResourceTypeTrans Where UserId = " + argUserId;
                    }
                    if (argType == "G")
                    {
                        sSql = "Delete From dbo.UserResourceGroupTrans Where UserId = " + argUserId;
                    }
                    if (argType == "W")
                    {
                        sSql = "Delete From dbo.UserWorkGroupTrans Where UserId = " + argUserId;
                    }
                    if (sSql != "")
                    {
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    if (argIds != "")
                    {
                        sSql = "";
                        if (argType == "T")
                        {
                            sSql = String.Format("Insert Into dbo.UserResourceTypeTrans(UserId,ResourceTypeId) Select {0},TypeId From [{1}].dbo.ResourceType Where TypeId in ({2})",argUserId, BsfGlobal.g_sRateAnalDBName, argIds);
                        }
                        else
                            if (argType == "G")
                            {
                                sSql = String.Format("Insert Into dbo.UserResourceGroupTrans(UserId,Resource_Group_ID) Select {0},Resource_Group_ID from [{1}].dbo.Resource_Group Where Resource_Group_ID in ({2})", argUserId, BsfGlobal.g_sRateAnalDBName, argIds);
                            }
                            else
                                if (argType == "W")
                                {
                                    sSql = "Insert Into dbo.UserWorkGroupTrans(UserId,Work_Group_ID) " + "Select " + argUserId + ",Work_Group_ID from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.Work_Group Where Work_Group_ID in (" + argIds + ")";
                                }
                        if (sSql != "")
                        {
                            cmd = new SqlCommand(sSql, conn, tran);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        public DataTable GetUserhierarchical()
        {
            DataTable dt;
            int iUserId = 0;
            m_tUsers = new DataTable();
            m_tUsers.Columns.Add("Id", typeof(Int32));
            m_tUsers.Columns.Add("ParentId", typeof(Int32));
            m_tUsers.Columns.Add("UserName", typeof(string));
            m_tUsers.Columns.Add("Department", typeof(string));
            m_tUsers.Columns.Add("Position", typeof(string));
            m_tUsers.Columns.Add("Level", typeof(string));

            DataRow dr;
            string sSql = "Select A.UserId,A.UserName,isnull(B.PositionName,'')  PositionName,isnull(C.Dept_Name,'') Dept_Name,isnull(D.LevelName,'') LevelName From dbo.Users A " +
                             "Left Join dbo.Position B On A.PositionId=B.PositionId " +
                             "Left Join dbo.Department C On A.DeptId =C.DeptId " +
                             "Left Join dbo.LevelMaster D On A.LevelId=D.LevelId " +
                             "Where A.UserId Not In(Select UserId From dbo.UserSuperiorTrans) And PowerUser=0";
            BsfGlobal.OpenWorkFlowDB();
            using (SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB))
            {
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iUserId = Convert.ToInt32(dt.Rows[i]["UserId"].ToString());
                    dr = m_tUsers.NewRow();
                    dr["Id"] = iUserId;
                    dr["ParentId"] = 0;
                    dr["UserName"] = dt.Rows[i]["UserName"].ToString();
                    dr["Department"] = dt.Rows[i]["Dept_Name"].ToString();
                    dr["Position"] = dt.Rows[i]["PositionName"].ToString();
                    dr["Level"] = dt.Rows[i]["LevelName"].ToString();
                    m_tUsers.Rows.Add(dr);
                    GetSuperiorUsers(iUserId);
                }
            }
            BsfGlobal.g_WorkFlowDB.Close();
            return m_tUsers;
        }

        public DataTable GetDepartmentView()
        {
            DataTable dt = new DataTable();
            int iUserId = 0;
            m_tUsers = new DataTable();
            m_tUsers.Columns.Add("Id", typeof(Int32));
            m_tUsers.Columns.Add("ParentId", typeof(Int32));
            m_tUsers.Columns.Add("UserName", typeof(string));
            m_tUsers.Columns.Add("Position", typeof(string));
            m_tUsers.Columns.Add("Level", typeof(string));

            DataRow dr;
            BsfGlobal.OpenWorkFlowDB();
            int iMId;

            string sSql = "Select Max(DeptId) MDId From dbo.Department";
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
                iMId = Convert.ToInt32(dt.Rows[0]["MDId"].ToString());
            else
                iMId = 0;
            da.Dispose();
            dt.Dispose();

            sSql = "Select A.DeptId,A.UserId,A.UserName,isnull(B.PositionName,'')  PositionName,isnull(D.LevelName,'') LevelName From dbo.Users A " +
                    "Left Join dbo.Position B on A.PositionId=B.PositionId " +
                    "Left Join dbo.LevelMaster D on A.LevelId=D.LevelId ";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            using (DataTable dtT = new DataTable())
            {
                da.Fill(dtT);
                da.Dispose();
                DataView dv;
                DataTable dTV;
                sSql = "Select DeptId,Dept_Name From dbo.Department Where DeptId In (Select DeptId From dbo.Users) Order by Dept_Name";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iUserId = Convert.ToInt32(dt.Rows[i]["DeptId"].ToString());
                    dr = m_tUsers.NewRow();
                    dr["Id"] = iUserId;
                    dr["ParentId"] = 0;
                    dr["UserName"] = dt.Rows[i]["Dept_Name"].ToString();
                    dr["Position"] = "";
                    dr["Level"] = "";
                    m_tUsers.Rows.Add(dr);
                    dv = new DataView(dtT);
                    dv.RowFilter = "DeptId = " + iUserId;
                    dTV = new DataTable();
                    dTV = dv.ToTable();
                    for (int j = 0; j < dTV.Rows.Count; j++)
                    {
                        iMId = iMId + 1;
                        dr = m_tUsers.NewRow();
                        dr["Id"] = iMId;
                        dr["ParentId"] = iUserId;
                        dr["UserName"] = dTV.Rows[j]["UserName"].ToString();
                        dr["Position"] = dTV.Rows[j]["PositionName"].ToString();
                        dr["Level"] = dTV.Rows[j]["LevelName"].ToString();
                        m_tUsers.Rows.Add(dr);
                    }
                    dv.Dispose();
                    dTV.Dispose();
                }
            }
            BsfGlobal.g_WorkFlowDB.Close();
            return m_tUsers;
        }

        public DataTable GetPositionView()
        {
            DataTable dt = new DataTable();
            int iUserId = 0;
            m_tUsers = new DataTable();
            m_tUsers.Columns.Add("Id", typeof(Int32));
            m_tUsers.Columns.Add("ParentId", typeof(Int32));
            m_tUsers.Columns.Add("UserName", typeof(string));
            m_tUsers.Columns.Add("Department", typeof(string));
            m_tUsers.Columns.Add("Level", typeof(string));

            DataRow dr;
            BsfGlobal.OpenWorkFlowDB();
            int iMId;

            string sSql = "Select Max(PositionId) MDId From dbo.Position";
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
                iMId = Convert.ToInt32(dt.Rows[0]["MDId"].ToString());
            else
                iMId = 0;
            da.Dispose();
            dt.Dispose();

            sSql = "Select A.PositionId,A.UserId,A.UserName,isnull(B.Dept_Name,'')  Department,isnull(D.LevelName,'') LevelName From dbo.Users A " +
                    "Left Join dbo.Department B on A.DeptId=B.DeptId " +
                    "Left Join dbo.LevelMaster D on A.LevelId=D.LevelId ";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            using (DataTable dtT = new DataTable())
            {
                da.Fill(dtT);
                da.Dispose();
                DataView dv;
                DataTable dTV;
                sSql = "Select PositionId,PositionName From dbo.Position Where PositionId in (Select PositionId From dbo.Users) Order by PositionName";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iUserId = Convert.ToInt32(dt.Rows[i]["PositionId"].ToString());
                    dr = m_tUsers.NewRow();
                    dr["Id"] = iUserId;
                    dr["ParentId"] = 0;
                    dr["UserName"] = dt.Rows[i]["PositionName"].ToString();
                    dr["Department"] = "";
                    dr["Level"] = "";
                    m_tUsers.Rows.Add(dr);
                    dv = new DataView(dtT);
                    dv.RowFilter = "PositionId = " + iUserId;
                    dTV = new DataTable();
                    dTV = dv.ToTable();
                    for (int j = 0; j < dTV.Rows.Count; j++)
                    {
                        iMId = iMId + 1;
                        dr = m_tUsers.NewRow();
                        dr["Id"] = iMId;
                        dr["ParentId"] = iUserId;
                        dr["UserName"] = dTV.Rows[j]["UserName"].ToString();
                        dr["Department"] = dTV.Rows[j]["Department"].ToString();
                        dr["Level"] = dTV.Rows[j]["LevelName"].ToString();
                        m_tUsers.Rows.Add(dr);
                    }
                    dv.Dispose();
                    dTV.Dispose();
                }
            }
            BsfGlobal.g_WorkFlowDB.Close();
            return m_tUsers;
        }

        public DataTable GetLevelView()
        {
            DataTable dt = new DataTable();
            int iUserId = 0;
            m_tUsers = new DataTable();
            m_tUsers.Columns.Add("Id", typeof(Int32));
            m_tUsers.Columns.Add("ParentId", typeof(Int32));
            m_tUsers.Columns.Add("UserName", typeof(string));
            m_tUsers.Columns.Add("Department", typeof(string));
            m_tUsers.Columns.Add("Position", typeof(string));

            DataRow dr;
            BsfGlobal.OpenWorkFlowDB();
            int iMId;

            string sSql = "Select Max(LevelId) MDId From dbo.LevelMaster";
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
                iMId = Convert.ToInt32(dt.Rows[0]["MDId"].ToString());
            else
                iMId = 0;
            da.Dispose();
            dt.Dispose();

            sSql = "Select A.LevelId,A.UserId,A.UserName,isnull(B.Dept_Name,'')  Department,isnull(D.PositionName,'') PositionName From dbo.Users A " +
                    "Left Join dbo.Department B on A.DeptId=B.DeptId " +
                    "Left Join dbo.Position D on A.PositionId=D.PositionId ";
            da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            using (DataTable dtT = new DataTable())
            {
                da.Fill(dtT);
                da.Dispose();
                DataView dv;
                DataTable dTV;
                sSql = "Select LevelId,LevelName From dbo.LevelMaster Where LevelId In (Select LevelId From dbo.Users) Order by OrderId";
                da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    iUserId = Convert.ToInt32(dt.Rows[i]["LevelId"].ToString());
                    dr = m_tUsers.NewRow();
                    dr["Id"] = iUserId;
                    dr["ParentId"] = 0;
                    dr["UserName"] = dt.Rows[i]["LevelName"].ToString();
                    dr["Department"] = "";
                    dr["Position"] = "";
                    m_tUsers.Rows.Add(dr);
                    dv = new DataView(dtT);
                    dv.RowFilter = "LevelId = " + iUserId;
                    dTV = new DataTable();
                    dTV = dv.ToTable();
                    for (int j = 0; j < dTV.Rows.Count; j++)
                    {
                        iMId = iMId + 1;
                        dr = m_tUsers.NewRow();
                        dr["Id"] = iMId;
                        dr["ParentId"] = iUserId;
                        dr["UserName"] = dTV.Rows[j]["UserName"].ToString();
                        dr["Department"] = dTV.Rows[j]["Department"].ToString();
                        dr["Position"] = dTV.Rows[j]["PositionName"].ToString();
                        m_tUsers.Rows.Add(dr);
                    }
                    dv.Dispose();
                    dTV.Dispose();
                }
            }
            BsfGlobal.g_WorkFlowDB.Close();
            return m_tUsers;
        }

        public void GetSuperiorUsers(int argId)
        {
            DataRow drT;
            int iUserId = 0;
            string sSql = String.Format("Select A.UserId,B.UserName,isnull(C.PositionName,'')  PositionName,isnull(D.Dept_Name,'') Dept_Name,isnull(E.LevelName,'') LevelName "+
                " From dbo.UserSuperiorTrans A  Inner Join dbo.Users B On A.UserId=B.UserId Left Join dbo.Position C "+
                " On B.PositionId=C.PositionId Left Join dbo.Department D On B.DeptId =D.DeptId Left Join dbo.LevelMaster E On"+
                " B.LevelId=E.LevelId Where sUserId={0}", argId);
            DataTable dtT = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
            da.Fill(dtT);
            for (int i = 0; i < dtT.Rows.Count; i++)
            {
                iUserId = Convert.ToInt32(dtT.Rows[i]["UserId"].ToString());
                drT = m_tUsers.NewRow();
                drT["Id"] = iUserId;
                drT["ParentId"] = argId;
                drT["UserName"] = dtT.Rows[i]["UserName"].ToString();
                drT["Department"] = dtT.Rows[i]["Dept_Name"].ToString();
                drT["Position"] = dtT.Rows[i]["PositionName"].ToString();
                drT["Level"] = dtT.Rows[i]["LevelName"].ToString();
                m_tUsers.Rows.Add(drT);
                GetSuperiorUsers(iUserId);
            }
            da.Dispose();
            dtT.Dispose();
        }

        #endregion
    }
}
