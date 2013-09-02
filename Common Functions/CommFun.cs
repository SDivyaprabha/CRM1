using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using Microsoft.VisualBasic;
using System.Globalization;
using DevExpress.Utils.Paint;
using System.Drawing;

namespace CRM
{
    public class CommFun
    {
        #region Variables

        //public static int g_lUserId = 0;
        //public static Boolean g_ModWorkFlow = false;
        //public static Boolean g_WorkFlow = false;
        //public static DevExpress.XtraEditors.PanelControl WPanel;
      
        public static Telerik.WinControls.UI.Docking.DocumentWindow DW1;
        public static Telerik.WinControls.UI.Docking.DocumentWindow DW2;
        public static Telerik.WinControls.UI.RadPanel RP1;
        public static Telerik.WinControls.UI.RadPanel RP2;
        public static DevExpress.XtraBars.Ribbon.RibbonPage RIPMaster;
        public static DevExpress.XtraBars.Ribbon.RibbonPage RIPTrans;
        public static DevExpress.XtraBars.Ribbon.RibbonPage RIPInfo;

        public static Telerik.WinControls.UI.Docking.DocumentWindow StageComp;
        public static Telerik.WinControls.UI.Docking.DocumentWindow CostCentre;
        public static Telerik.WinControls.UI.Docking.DocumentWindow FlatReg;
        public static Telerik.WinControls.UI.Docking.DocumentWindow FlatTypeReg;
        public static Telerik.WinControls.UI.Docking.DocumentWindow FlatE;
        public static Telerik.WinControls.UI.Docking.DocumentWindow FlatTypeE;
        public static Telerik.WinControls.UI.Docking.DocumentWindow ExtraItemM;
        public static Telerik.WinControls.UI.Docking.DocumentWindow FlatTemp;
        public static Telerik.WinControls.UI.Docking.DocumentWindow PaymentS;

        public static Telerik.WinControls.UI.Docking.DocumentWindow PayTempDW1;
        public static Telerik.WinControls.UI.Docking.DocumentWindow FlatTempDW2;

        public static Telerik.WinControls.UI.RadPanel FTypePanel;
        public static Telerik.WinControls.UI.RadPanel FlatPanel;

        public static Telerik.WinControls.UI.RadPanel PayTempPanel;
        public static Telerik.WinControls.UI.RadPanel FlatTempPanel;
        public static string sFormatTotAmt;

        public static string projectDBName = "";
        public static string Freeze { get; set; }
        public static int g_iInvTypeId = 6;
        public static int g_iCurrencyDigit;
        static string m_sUserId = "";
        public static string m_sFuncName = "";
        
        #endregion

        #region Methods
        
        public static DataTable FillRecord(string sql)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static string Insert_SingleQuot(string strString)
        {
            string functionReturnValue = null;
            // ERROR: Not supported in C#: OnErrorStatement

            int lngTmp = 0;
            string strTmp = null;
            lngTmp = Strings.InStr(1, strString, "'", Constants.vbBinaryCompare);

            if (lngTmp <= 0)
            {
                functionReturnValue = strString;
                return functionReturnValue;
            }

            while (lngTmp > 0)
            {
                strTmp = Strings.Mid(strString, 1, lngTmp);
                strTmp = strTmp + "'";
                strTmp = strTmp + Strings.Mid(strString, lngTmp + 1);
                strString = strTmp;
                lngTmp = Strings.InStr(lngTmp + 2, strTmp, "'", Constants.vbBinaryCompare);
            }
            functionReturnValue = strString;
            return functionReturnValue;
        }

        //public static string FormatNum(object argValue, int argDigit)
        //{
        //    string dAmt = "";
        //    if (argValue == null) argValue = 0;
        //    decimal dValue = Convert.ToDecimal(argValue);
        //    string sFormat = "N" + Convert.ToString(argDigit).Trim();
        //    dAmt = dValue.ToString(sFormat, CultureInfo.GetCultureInfo(1094).NumberFormat);
        //    return dAmt;
        //}

        public static DataTable FillRcd(string sql,SqlConnection conn,SqlTransaction tran)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(sql, conn, tran);
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();
                cmd.Dispose();

                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DataTable FillRecord1(string sql)
        {
            BsfGlobal.OpenRateAnalDB();
            DataTable dt = null;
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_RateAnalDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_RateAnalDB.Close();
            }
            return dt;
        }

        public static DataTable FillRecord2(string sql)
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = null;
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static string FormatNum(object argValue, int argDigit)
        {
            decimal dValue = Convert.ToDecimal(CommFun.IsNullCheck(argValue, CommFun.datatypes.vartypenumeric));
            string sFormat = "N" + Convert.ToString(argDigit).Trim();
            return dValue.ToString(sFormat, CultureInfo.GetCultureInfo(1094).NumberFormat);
        }

        public static string FormatNum1(string InputNum, int NumDec)
        {
            string TempStr = null;
            string FormatStr = null;
            int Ctr = 0;
            object TempValue = null;
            TempValue = Convert.ToDecimal(IsNullCheck(InputNum, datatypes.vartypenumeric));
            FormatStr = "###0.";
            for (Ctr = 1; Ctr <= NumDec; Ctr++)
            {
                FormatStr = FormatStr + "0";
            }
            TempStr = Strings.Format(TempValue, FormatStr);
            return TempStr;
        }

        public static void CRMExecute(string argsSql)
        {
            BsfGlobal.OpenCRMDB();
            SqlCommand cmd = new SqlCommand(argsSql, BsfGlobal.g_CRMDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();

        }

        public static DataTable FillCC()
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT CostCentreId,CostCentreName FROM OperationalCostCentre ORDER BY CostCentreName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        public static DataTable FillProjType()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT ProjectTypeId,ProjectTypeName FROM [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ProjectType ORDER BY ProjectTypeName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable FillExec()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select UserId ExecId, Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName " +
                              " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                              " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                              " Where B.PositionType='M'";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static bool CheckData(string TableName, string Field, string argval, string Caption)
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT " + Field + " FROM " + TableName + " WHERE " + Field + "= '" + argval + "'", BsfGlobal.g_CRMDB);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();

            bool chdata = true;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(Caption + " already exists");
                    chdata = false;
                }
            }
            return chdata;
        }

        public static DataTable AddSelectToDataView(DataView dv)
        {
            DataTable dt = dv.ToTable();

            DataRow dRow = dt.NewRow();
            dRow[0] = -1;
            dRow[1] = "--Select--";
            dt.Rows.InsertAt(dRow, 0);

            return dt;
        }

        public static void Clear(Form frm)
        {
            foreach (Control ctl in frm.Controls)
            {
                if (ctl.Controls.Count > 0)
                {
                    if (ctl is TextBox) ((TextBox)ctl).Text = string.Empty;
                    else if (ctl is DevExpress.XtraEditors.LookUpEdit) ((DevExpress.XtraEditors.LookUpEdit)ctl).ItemIndex = 0;
                    else if (ctl is DevExpress.XtraEditors.ComboBoxEdit) ((DevExpress.XtraEditors.ComboBoxEdit)ctl).SelectedIndex = 0;
                }
            }
        }

        public static DataTable LoadExecutive()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "Select UserId ExecId, Case When A.EmployeeName='' Then A.UserName Else A.EmployeeName End As ExecName " +
                              " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users A " +
                              " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Position B on A.PositionId=B.PositionId " +
                              " Where B.PositionType='C'";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable LoadLead()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT LeadId,LeadName FROM LeadRegister ORDER BY LeadName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable LoadReminder()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT ReminderId,ReminderName FROM Reminder ORDER BY ReminderName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable LoadSource()
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                string sSql = "SELECT SourceId,SourceName FROM EnquirySource ORDER BY SourceName";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        public static DataTable AddAllToDataTable(DataTable dt)
        {
            if (dt != null)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = -1;
                newRow[1] = "--All--";
                dt.Rows.InsertAt(newRow, 0);
            }
            return dt;
        }

        public static DataTable AddSelectToDataTable(DataTable dt)
        {
            if (dt != null)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = -1;
                newRow[1] = "--Select--";
                dt.Rows.InsertAt(newRow, 0);
            }
            return dt;
        }

        //public static void InsertLog(DateTime argLogTime, string argRoleName, string argLogType,string argLogDesc, int argUserId, int argRegId, int argCostcentreId, int argCompanyId, string argDBName)
        //{
        //    int identity = 0;
        //    SqlConnection conn;
        //    conn = new SqlConnection();
        //    conn =  BsfGlobal.OpenWorkFlowDB();

        //    SqlTransaction tran = conn.BeginTransaction();
        //    try
        //    {
        //        SqlCommand cmd;
        //        string sSql = "Insert into LogMaster(LogTime,UserId,RoleName,LogType,LogDescription) " +
        //                      "Values('" + String.Format("{0:dd-MMM-yyyy}", argLogTime) + "'," + argUserId + ",'" + argRoleName + "'," +
        //                      "'" + argLogType + "','" + argLogDesc + "') SELECT SCOPE_IDENTITY();";
        //        cmd = new SqlCommand(sSql, conn, tran);
        //        identity = int.Parse(cmd.ExecuteScalar().ToString());
        //        cmd.Dispose();

        //        sSql = "Insert into LogTrans(LogId,RegisterId,CostCentreId,CompanyId,DBName) " +
        //               "Values(" + identity + "," + argRegId + "," +
        //               "" + argCostcentreId + "," + argCompanyId + ",'" + argDBName + "')";
        //        cmd = new SqlCommand(sSql, conn, tran);
        //        cmd.ExecuteNonQuery();
        //        cmd.Dispose();

        //        tran.Commit();
        //    }
        //    catch
        //    {
        //        tran.Rollback();
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        public static DataTable GenericListToDataTable(object list)
        {
            DataTable dt = null;
            Type listType = list.GetType();
            if (listType.IsGenericType)
            {
                //determine the underlying type the List<> contains
                Type elementType = listType.GetGenericArguments()[0];

                //create empty table -- give it a name in case
                //it needs to be serialized
                dt = new DataTable(elementType.Name + "List");

                //define the table -- add a column for each public
                //property or field
                MemberInfo[] miArray = elementType.GetMembers(
                    BindingFlags.Public | BindingFlags.Instance);
                foreach (MemberInfo mi in miArray)
                {
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        dt.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else if (mi.MemberType == MemberTypes.Field)
                    {
                        FieldInfo fi = mi as FieldInfo;
                        dt.Columns.Add(fi.Name, fi.FieldType);
                    }
                }

                //populate the table
                IList il = list as IList;
                foreach (object record in il)
                {
                    int i = 0;
                    object[] fieldValues = new object[dt.Columns.Count];
                    foreach (DataColumn c in dt.Columns)
                    {
                        MemberInfo mi = elementType.GetMember(c.ColumnName)[0];
                        if (mi.MemberType == MemberTypes.Property)
                        {
                            PropertyInfo pi = mi as PropertyInfo;
                            fieldValues[i] = pi.GetValue(record, null);
                        }
                        else if (mi.MemberType == MemberTypes.Field)
                        {
                            FieldInfo fi = mi as FieldInfo;
                            fieldValues[i] = fi.GetValue(record);
                        }
                        i++;
                    }
                    dt.Rows.Add(fieldValues);
                }
            }
            return dt;
        }

        public static DataTable AddNoneToDataTable(DataTable dt)
        {
            if (dt != null)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = -1;
                newRow[1] = "None";
                dt.Rows.InsertAt(newRow, 0);
            }
            return dt;
        }

        public enum datatypes
        {
            vartypenumeric = 0,
            VarTypeDate,
            vartypestring,
            varTypeBoolean
        }

        public static object IsNullCheck(object obj, datatypes ObjectType = datatypes.vartypestring)
        {
            object objReturn = null;
            objReturn = obj;
            if (ObjectType == datatypes.vartypestring & (Information.IsDBNull(obj) || obj ==null))
            {
                objReturn = "";
            }
            else if (ObjectType == datatypes.vartypenumeric)
            {
                if (Information.IsDBNull(obj) == true)
                {
                    objReturn = 0;
                }
                else if (Information.IsNumeric(obj) == false)
                {
                    objReturn = 0;
                }
                else if (obj.ToString() == "")
                {
                    objReturn = 0;
                }
            }
            else if (ObjectType == datatypes.VarTypeDate)
            {
                if (Information.IsDBNull(obj) == true)
                {
                    objReturn = DateTime.MinValue;
                }
                else if (Information.IsDate(obj) == false)
                {
                    objReturn = DateTime.MinValue;
                }
                else if (Convert.ToDateTime(obj) == DateTime.MinValue)
                {
                    objReturn = DateTime.MinValue;
                }
            }
            else if (ObjectType == datatypes.varTypeBoolean & Information.IsDBNull(obj))
            {
                objReturn = false;
            }
            return objReturn;
        }

        public static void CheckListAlertUpdate(string argAlertName,string argAlterDesc, int argCCId, int argFlatId)
        {
            BsfGlobal.OpenWorkFlowDB();
            string sSql = "SELECT AlertId FROM AlertMaster WHERE " +
                          "AlertName= '" + argAlertName + "'";
            SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dr.Close();
            cmd.Dispose();

            int iAlertId = 0;

            if (dt.Rows.Count > 0) { iAlertId = Convert.ToInt32(dt.Rows[0]["AlertId"].ToString()); }
            dt.Dispose();



            sSql = "Insert into AlertTransUsers " +
                   "(AlertId,AlertDescription,AlertDate,UserId) " +
                   "Select " + iAlertId + ",'" + argAlertName + "','" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "',UserId from UserAlertTrans " +
                   "Where UserId Not in (Select UserId from UserCostCentreTrans Where CostCentreId= " + argCCId + ")";
            cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            //if (argFlatId != 0)
            //{
            //    int iBuyerId = 0;

            //    sSql = "Select LeadId from [" + BsfGlobal.g_sCRMDBName + "].dbo.FlatDetails Where FlatId= " + argFlatId;
            //    cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
            //    dr = cmd.ExecuteReader();
            //    dt = new DataTable();
            //    dt.Load(dr);
            //    dr.Close();
            //    cmd.Dispose();

            //    if (dt.Rows.Count > 0) { iBuyerId = Convert.ToInt32(dt.Rows[0]["LeadId"]); }
            //    dt.Dispose();

            //    if (iBuyerId != 0)
            //    {
            //        sSql = "Insert into AlertTransBuyer(AlertId,AlertDescription,AlertDate,BuyerId) " +
            //            "Select " + iAlertId + ",'" + argAlertName + "','" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + "',UserId from UserAlertTrans " +
            //            "Where UserId Not in (Select UserId from UserCostCentreTrans Where CostCentreId= " + argCCId + ")";
            //        cmd = new SqlCommand(sSql, BsfGlobal.g_WorkFlowDB);
            //        cmd.ExecuteNonQuery();
            //        cmd.Dispose();
            //    }
            //}

            BsfGlobal.g_WorkFlowDB.Close();

        }

        public static DataTable GetGroupedBy(DataTable dt, string columnNamesInDt, string groupByColumnNames, string typeOfCalculation)
        {
            //Return its own if the column names are empty

            if (columnNamesInDt == string.Empty || groupByColumnNames == string.Empty)
            {
                return dt;
            }

            //Once the columns are added find the distinct rows and group it bu the numbet

            DataTable _dt = dt.DefaultView.ToTable(true, groupByColumnNames);

            //The column names in data table

            string[] _columnNamesInDt = columnNamesInDt.Split(',');

            for (int i = 0; i < _columnNamesInDt.Length; i = i + 1)
            {
                if (_columnNamesInDt[i] != groupByColumnNames)
                {
                    if (_columnNamesInDt[i] == "Description")
                        _dt.Columns.Add(_columnNamesInDt[i], typeof(string));
                    else if (_columnNamesInDt[i] == "Age" || groupByColumnNames == "CreditDays")
                        _dt.Columns.Add(_columnNamesInDt[i], typeof(int));
                    else
                        _dt.Columns.Add(_columnNamesInDt[i], typeof(decimal));
                }
            }

            //Gets the collection and send it back

            for (int i = 0; i < _dt.Rows.Count; i = i + 1)
            {
                for (int j = 0; j < _columnNamesInDt.Length; j = j + 1)
                {
                    if (_columnNamesInDt[j] != groupByColumnNames)
                    {
                        _dt.Rows[i][j] = dt.Compute(String.Format("{0}({1})", typeOfCalculation, _columnNamesInDt[j]), String.Format("{0} = '{1}'", groupByColumnNames, _dt.Rows[i][groupByColumnNames]));
                    }
                }
            }
            return _dt;
        }

        #endregion

        #region Date Validation

        public static DateTime? ToDMY(string dt)
        {
            DateTime? retVal = null;

            string date = string.Empty;
            string month = string.Empty;
            string year = string.Empty;

            string dmy = string.Empty;

            if (dt.Length == 10)
            {
                string[] split = dt.Split('/');
                date = split[0];
                month = split[1];
                year = split[2];

                dmy = date + "/" + month + "/" + year;

                try
                {
                    retVal = DateTime.Parse(dmy);

                }
                catch
                {
                    retVal = null;
                }

            }
            else
            {

                retVal = null;
            }

            return retVal;
        }


        #endregion

        public static DataTable AddSelectToDataTable_All(DataTable dt, string argStr)
        {
            if (dt != null)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = -1;
                newRow[1] = argStr;
                dt.Rows.InsertAt(newRow, 0);
            }
            return dt;
        }

        public static void SetMyGraphics()
        {
            FieldInfo fi = typeof(XPaint).GetField("graphics", BindingFlags.Static | BindingFlags.NonPublic);
            fi.SetValue(null, new MyXPaint());
        }

        public class MyXPaint : XPaint
        {
            public override void DrawFocusRectangle(Graphics g, Rectangle r, Color foreColor, Color backColor)
            {
                if (!CanDraw(r)) return;
                Brush hb = Brushes.Red;
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, 1, r.Height - 1)); // left
                g.FillRectangle(hb, new Rectangle(r.X, r.Y, r.Width - 1, 1)); // top
                g.FillRectangle(hb, new Rectangle(r.Right - 1, r.Y, 1, r.Height - 1)); // right
                g.FillRectangle(hb, new Rectangle(r.X, r.Bottom - 1, r.Width, 1)); // bottom
            }
        }

        #region Email

        internal static DataTable GetMandrillSetting(int argReportTypeId)
        {
            BsfGlobal.OpenWorkFlowDB();
            DataTable dt = null;
            try
            {
                string sql = string.Empty;
                sql = "SELECT * FROM dbo.MailTemplate Where ReportTypeId=" + argReportTypeId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_WorkFlowDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_WorkFlowDB.Close();
            }
            return dt;
        }

        #endregion

        #region CostCentre Rights

        public static string GetCCTrans()
        {
            BsfGlobal.OpenCRMDB();
            string sCCTransId = "";
            try
            {
                string sSql = "Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + "";
                SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sId = dt.Rows[i]["CostCentreId"].ToString() + ",";
                    sCCTransId = sCCTransId + sId;
                }
                sCCTransId = sCCTransId.TrimEnd(',');
                dt.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return sCCTransId;
        }

        #endregion

        #region OtherExecutive

        public static string GetOtherExecTrans(SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string sSql = "Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users Where UserId=" + BsfGlobal.g_lUserId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                if (dt.Rows.Count > 0) { m_sUserId = Convert.ToString(dt.Rows[0]["UserId"]) + ","; }

                GetUser(Convert.ToInt32(BsfGlobal.g_lUserId), conn, tran);
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return m_sUserId;
        }

        public static void GetUser(int argUserId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string sSql = "Select UserId From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserSuperiorTrans Where sUserId=" + argUserId + "";
                SqlCommand cmd = new SqlCommand(sSql, conn, tran);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dr.Close();
                cmd.Dispose();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    m_sUserId = m_sUserId + dt.Rows[i]["UserId"] + ",";
                    GetUser(Convert.ToInt32(dt.Rows[i]["UserId"]), conn, tran);
                }
                dt.Dispose();
            }
            catch (SqlException ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
        }

        #endregion

        #region Voucher

        public static bool IsValidTransNumber(string sFrmFlag, string sTranFlag, string sTransNo, int iMode, int iCCId, int iCompanyId)
        {
            bool bRetVal = false;
            if (BsfGlobal.g_bAssetDB == true)
            {
                try
                {
                    BsfGlobal.OpenAssetDB();
                    string sSql = "";

                    if (sFrmFlag == "Buyer")
                    {
                        if (sFrmFlag == "Buyer" && (sTranFlag == "  " || sTranFlag == "GE"))
                        {
                            sSql = "Select AllotmentNo From BuyerDetail Where AllotmentNo='" + sTransNo + "' ";
                        }
                        else if (sFrmFlag == "Buyer" && sTranFlag == "CC")
                        {
                            sSql = "Select CCAllotNo From BuyerDetail Where CCAllotNo='" + sTransNo + "' And CostCentreId=" + iCCId + " ";
                        }
                        else if (sFrmFlag == "Buyer" && sTranFlag == "CO")
                        {
                            sSql = "Select COAllotNo From BuyerDetail Where CostCentreId IN (  " +
                                   "Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "]..OperationalCostCentre Where FACostCentreId IN ( " +
                                   "Select CostCentreId From [" + BsfGlobal.g_sWorkFlowDBName + "]..CostCentre Where CompanyId=" + iCompanyId + ")) And COAllotNo='" + sTransNo + "' ";
                        }
                    }

                    if (sSql != "")
                    {
                        SqlCommand cmd = new SqlCommand(sSql, BsfGlobal.g_AssetDB);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        dt.Load(sdr);
                        sdr.Close();
                        cmd.Dispose();

                        if (iMode == 0)
                        {
                            if (dt.Rows.Count == 0) bRetVal = true; else bRetVal = false;
                        }
                        else
                        {
                            if (dt.Rows.Count == 1) bRetVal = true; else bRetVal = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    BsfGlobal.g_AssetDB.Close();
                }
            }
            return bRetVal;
        }

        #endregion
    }
}
