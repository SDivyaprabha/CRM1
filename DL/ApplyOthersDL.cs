using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DL
{
    class ApplyOthersDL
    {
        #region Methods

        internal static DataTable GetFlatType(int argCostCentreId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda = null;

            try
            {
                ssql = string.Format("Select FlatTypeId,TypeName from dbo.FlatType Where ProjId={0}", argCostCentreId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        internal static DataTable GetCheckList(int argFlatTypeId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda = null;

            try
            {
                ssql = string.Format("Select A.CheckListId,B.CheckListName from dbo.FlatTypeChecklist A"+
                    " Inner Join dbo.CheckListMaster B on A.CheckListId=B.CheckListId"+
                    " Where A.FlatTypeId={0}", argFlatTypeId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        internal static DataTable GetFlatTypeCheckList(int argFlatTypeId,int argCheckListId)
        {
            BsfGlobal.OpenCRMDB();
            string ssql = "";
            DataTable dt = null;
            SqlDataAdapter sda = null;

            try
            {
                ssql = string.Format("Select D.FlatId, D.FlatNo, A.ExpCompletionDate,"+
                    " Case When C.CheckListId IS NULL THEN CONVERT(bit, 0, 0) else CONVERT(bit, 1, 1) End as Status,"+
                    " C.CompletionDate,C.ExecutiveId,C.Remarks from dbo.FlatTypeChecklist A Inner Join"+
                    " dbo.CheckListMaster B on A.CheckListId = B.CheckListId Inner Join dbo.FlatDetails D on"+
                    " A.FlatTypeId = D.FlatTypeId Left Join dbo.FlatChecklist C on A.CheckListId = C.CheckListId"+
                    " and C.FlatId = D.FlatId Where A.FlatTypeId ={0} And A.CheckListId={1}",argFlatTypeId, argCheckListId);
                sda = new SqlDataAdapter(ssql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        internal static void InsertFlatCheckList(DataTable dt, int argCheckListId, int argFlatTypeId)
        {
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = string.Format("Delete From dbo.FlatChecklist Where FlatId in (Select FlatId from dbo.FlatDetails Where FlatTypeId={0})"+
                        " AND CheckListId={1}", argFlatTypeId, argCheckListId);
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sSql = "INSERT INTO dbo.FlatChecklist (FlatId,CheckListId,ExpCompletionDate,CompletionDate,ExecutiveId,Status,Remarks) VALUES" +
                            " (" + dt.Rows[i]["FlatId"] + "," + argCheckListId + "," +
                            "'" + Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["ExpCompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy") + "'," +
                            "'" + Convert.ToDateTime(CommFun.IsNullCheck(dt.Rows[i]["CompletionDate"], CommFun.datatypes.VarTypeDate)).ToString("dd-MMM-yyyy") + "'," +
                            "" + CommFun.IsNullCheck(dt.Rows[i]["ExecutiveId"], CommFun.datatypes.vartypenumeric) + ",1,'" + dt.Rows[i]["Remarks"] + "')";
                        cmd = new SqlCommand(sSql, conn, tran);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }

                    tran.Commit();

                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

        }

        #endregion
    }
}
