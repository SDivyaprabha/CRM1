using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BO;

namespace CRM.DL
{
    class IncentiveDL
    {
        #region Methods

        internal static int InsertIncGen(string argMode, IncentiveBO IncGenBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd;
            string s_Date = "";
            string s_From = "";
            string s_To = "";
            int i_IncGenId = 0;

            try
            {
                s_Date = string.Format("{0:dd/MMM/yyyy}", IncGenBO.DE_Date);
                s_From = string.Format("{0:dd/MMM/yyyy}", IncGenBO.DE_From);
                s_To = string.Format("{0:dd/MMM/yyyy}", IncGenBO.DE_To);

                if (argMode == "A")
                {
                    ssql = String.Format("Insert Into dbo.IncentiveRegister(IDate, IRefNo, FDate, TDate, TotalAmount, Narration) Values('{0}', '{1}', '{2}', '{3}', {4}, '{5}') SELECT SCOPE_IDENTITY();", s_Date, IncGenBO.s_RefNo, s_From, s_To, IncGenBO.d_TotalAmount, IncGenBO.s_Narration);
                    scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    i_IncGenId = Convert.ToInt32(scmd.ExecuteScalar().ToString());
                    IncGenBO.i_IncGenId = i_IncGenId;
                    scmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.IncentiveRegister set IDate='{0}', IRefNo='{1}', FDate='{2}', TDate='{3}', TotalAmount={4}, Narration='{5}' Where IncentiveId={6}", s_Date, IncGenBO.s_RefNo, s_From, s_To, IncGenBO.d_TotalAmount, IncGenBO.s_Narration, IncGenBO.i_IncGenId);
                    scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    scmd.ExecuteNonQuery();
                    scmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return i_IncGenId;
        }

        internal static void InsertAmount(string argMode, BO.IncentiveBO IncGenBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand scmd;

            try
            {
                if (argMode == "A")
                {
                    ssql = String.Format("Insert Into dbo.IncentiveRegTrans(IncentiveId, ExecutiveId, Amount) Values({0}, {1}, {2})", IncGenBO.i_IncGenId, IncGenBO.i_ExeId, IncGenBO.d_Amount);
                    scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    scmd.ExecuteNonQuery();
                    scmd.Dispose();
                }
                else
                {
                    ssql = String.Format("Update dbo.IncentiveRegTrans set Amount={0} Where IncentiveId={1} AND ExecutiveId={2}", IncGenBO.d_Amount, IncGenBO.i_IncGenId, IncGenBO.i_ExeId);
                    scmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                    scmd.ExecuteNonQuery();
                    scmd.Dispose();
                }
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        internal static DataTable SelectIncGen()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda;

            try
            {
                ssql = "Select * From dbo.IncentiveRegister";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }

        internal static DataTable SelectIncGenTrans(IncentiveBO IncGenBO)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt_Trans = null;
            SqlDataAdapter sda;

            try
            {
                ssql = string.Format("Select * From dbo.IncentiveRegTrans Where IncentiveId={0}", IncGenBO.i_IncGenId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dt_Trans = new DataTable();
                sda.Fill(dt_Trans);
                dt_Trans.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt_Trans;
        }

        internal static void DeleteIncDet(int argId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            SqlCommand cmd = null;

            try
            {
                ssql = String.Format("Delete From dbo.IncentiveRegister Where IncentiveId={0}", argId);
                cmd = new SqlCommand(ssql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
        }

        internal static DataSet GetIncentive()
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataSet dt = new DataSet();
            SqlDataAdapter sda;

            try
            {
                //ssql = "Select Distinct B.ExecutiveId,Sum(IncValue)IncValue From dbo.IncentiveTrans A "+
                //    " Inner Join dbo.TargetTrans B On A.TargetId=B.TargetId" +
                //    " Inner Join dbo.TargetAmtTrans C On C.TargetId=A.TargetId" +
                //    " Where B.TYear Between " + IncBO.FromYear + " And " + IncBO.ToYear + "" +
                //    " And B.TMonth Between month(" + IncBO.FromMonth + ") And month(" + IncBO.ToMonth + ") Group By B.ExecutiveId";
                ssql = "Select Distinct A.IncentiveType,C.ExecutiveId,IncValue From dbo.TargetMaster A " +
                        " Inner Join dbo.IncentiveTrans B On A.TargetId=B.TargetId" +
                        " Left Join dbo.TargetTrans C On C.TargetId=A.TargetId " +
                        " Left Join dbo.TargetAmtTrans D On D.TargetId=C.TargetId" +
                        " Where C.TYear Between " + IncBO.FromYear + " And " + IncBO.ToYear + " And" +
                        " C.TMonth Between month(" + IncBO.FromMonth + ") And month(" + IncBO.ToMonth + ")";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                sda.Fill(dt,"Inc");
                sda.Dispose();
                dt.Dispose();

                ssql = "Select ExecutiveId,SUM(A.BaseAmt) Amt From dbo.FlatDetails A " +
                    " Inner Join dbo.LeadFlatInfo B on A.FlatId=B.FlatId" +
                    " Inner Join dbo.LeadRegister C on C.LeadId=B.LeadId" +
                    " Group by ExecutiveId";
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                sda.Fill(dt, "Value");
                sda.Dispose();
                dt.Dispose();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return dt;
        }
        #endregion
    }
}
