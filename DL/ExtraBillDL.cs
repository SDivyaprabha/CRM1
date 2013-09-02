using System;
using System.Collections.Generic;
using System.Linq;
using CRM.BusinessObjects;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class ExtraBillDL
    {
        #region Methods

        public static DataTable GetExtraBill(int argFlatId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select F.FlatId,F.ExtraItemId,F.Rate,F.Quantity,F.Amount,M.ItemCode,M.ItemDescription,U.Unit_Name" +
                        " From dbo.ExtraBillTrans T " +
                        " Inner Join dbo.FlatExtraItem F On T.ExtraItemId=F.ExtraItemId " +
                        " Inner Join dbo.ExtraBillRegister R On R.BillRegId=T.BillRegId And F.FlatId=R.FlatId " +
                        " Inner Join dbo.ExtraItemMaster M On M.ExtraItemId=F.ExtraItemId" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM U On U.Unit_ID=M.UnitId" +
                        " Where R.FlatId=" + argFlatId + " And F.Approve='Y'";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataSet GetRegisterDetails(int argBillRegId, string argProjName)
        {
            BsfGlobal.OpenCRMDB();
            DataSet ds = new DataSet();
            try
            {
                string sql = "Select * from dbo.ExtraBillRegister Where BillRegId=" + argBillRegId;
                SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Register");
                sda.Dispose();

                sql = "Select * from dbo.ExtraBillRateQ Where BillRegId=" + argBillRegId;
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Qualifier");
                sda.Dispose();

                sql = "Select A.ExtraItemId,L.LeadName, B.ItemCode Code,B.ItemDescription Description,C.Unit_Name Unit,A.Qty," +
                        " A.Rate,str(A.Qty) + ' ' + C.Unit_Name WorkingQty,A.Amount,A.NetAmount from dbo.ExtraBillTrans A " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster B on A.ExtraItemId=B.ExtraItemId " +
                        " Left Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM C on B.UnitId=C.Unit_Id " +
                        " Inner Join dbo.ExtraBillRegister R On R.BillRegId=A.BillRegId " +
                        " Inner Join BuyerDetail D On D.FlatId=R.FlatId" +
                        " Inner Join LeadRegister L On L.LeadId=D.LeadId" +
                        " Where A.BillRegId=" + argBillRegId + "";

                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                sda.Fill(ds, "Trans");
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
            return ds;
        }

        public DataTable GetExtraBillList(DateTime frmDate, DateTime  toDate)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
            string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(1));
            try
            {
                sql = "Select E.LeadName,A.CostCentreId,A.BillRegId,A.BillDate,A.BillNo,B.FlatNo,C.CostCentreName,C.ProjectDB,D.BlockName,"+
                    " A.NetAmount BillAmount from dbo.ExtraBillRegister A " +
                    " Inner Join dbo.FlatDetails B on A.FlatId=B.FlatId " +
                    " Left Join dbo.BlockMaster D on B.BlockId=D.BlockId " +
                    " Inner Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CostCentreId=C.CostCentreId " +
                    " Inner Join dbo.LeadRegister E On B.LeadId=E.LeadId " +
                    " Where A.BillDate between '" + frmdat + "'  And '" + tdat + "' Order by A.BillDate,A.BillNo";
                    

                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetProject()
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                //sql = "Select CostCentreId,CostCentreName,ProjectDB from ["+ BsfGlobal.g_sWorkFlowDBName +"].dbo.OperationalCostCentre Where CostCentreId<>0 Order By CostCentreName";
                sql = "Select CostCentreId,CostCentreName,ProjectDB from [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre " +
                        " Where ProjectDB in(Select ProjectName from ["+BsfGlobal.g_sRateAnalDBName+"].dbo.ConceptionRegister "+
                        " Where BusinessType ='B') and CostCentreId not in (Select CostCentreId "+
                        " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where UserId=" + BsfGlobal.g_lUserId + ") Order By CostCentreName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetFlatNo(int argCCId)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select Distinct FlatId,FlatNo from dbo.FlatDetails Where CostCentreId=" + argCCId + " And Status='S'";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public DataTable GetExtraItemDetails(int argFlatId,string argProjName)
        {
            DataTable dt=null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select F.ExtraItemId,T.Rate,T.Qty,T.Amount,L.LeadName,M.ItemCode Code,M.ItemDescription Description,U.Unit_Name Unit," +
                        " str(T.Qty) + ' ' + U.Unit_Name WorkingQty,T.NetAmount From dbo.ExtraBillTrans T " +
                        " Inner Join dbo.FlatExtraItem F On T.ExtraItemId=F.ExtraItemId " +
                        " Inner Join dbo.ExtraBillRegister R On R.BillRegId=T.BillRegId And F.FlatId=R.FlatId " +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster M On M.ExtraItemId=F.ExtraItemId" +
                        " Inner Join [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM U On U.Unit_ID=M.UnitId" +
                        "  Inner Join BuyerDetail B On B.FlatId=F.FlatId" +
                        " Inner Join LeadRegister L On L.LeadId=B.LeadId" +
                        " Where R.FlatId=" + argFlatId + " And F.Approve='Y'";

                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dt;
        }

        public int InsertExtraBill(ExtraBillRegBO argBRegBo, DataTable argBTrans, DataTable argQTrans, int argFlatId)
        {
            int iBillRegId = 0;
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string frmdat = string.Format("{0:dd MMM yyyy}", argBRegBo.BillDate);

                string Sql = "DELETE FROM dbo.ExtraBillRegister WHERE FlatId=" + argFlatId;
                SqlCommand cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Sql = "DELETE FROM dbo.ExtraBillTrans WHERE BillRegId=" + argBRegBo.BillRegId;
                cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Sql = "DELETE FROM dbo.ExtraBillRateQ WHERE BillRegId=" + argBRegBo.BillRegId;
                cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Sql = "DELETE FROM dbo.ExtraBillRateQAbs WHERE BillRegId=" + argBRegBo.BillRegId;
                cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //Bill Register
                Sql = "Insert Into dbo.ExtraBillRegister(BillDate,BillNo,CostCentreId,FlatId,BillAmount,NetAmount,Narration) " +
                        "Values ('" + frmdat + "','" + argBRegBo.RefNo + "'," + argBRegBo.CCId + "," + argBRegBo.FlatId + "," + argBRegBo.BillAmt +
                        "," + argBRegBo.NetAmt + ",'" + argBRegBo.Narration + "') Select SCOPE_IDENTITY();";
                cmd = new SqlCommand(Sql, conn, tran);
                iBillRegId = Convert.ToInt32(CommFun.IsNullCheck(cmd.ExecuteScalar(), CommFun.datatypes.vartypenumeric));
                cmd.Dispose();

                //BillTrans
                for (int u = 0; u < argBTrans.Rows.Count; u++)
                {
                    Sql = "Insert Into dbo.ExtraBillTrans(BillRegId,ExtraItemId,Qty,Rate,Amount,NetAmount) " +
                          "Values (" + iBillRegId + "," + argBTrans.Rows[u]["EItemId"].ToString() + "," + argBTrans.Rows[u]["Qty"].ToString() +
                          "," + argBTrans.Rows[u]["Rate"].ToString() + "," + argBTrans.Rows[u]["Amt"].ToString() + "," + argBTrans.Rows[u]["NetAmt"].ToString() + ")";
                    cmd = new SqlCommand(Sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                //Bill Qualifier
                decimal d_TotalTaxAmt = 0;
                for (int u = 0; u < argQTrans.Rows.Count; u++)
                {
                    d_TotalTaxAmt = d_TotalTaxAmt + Convert.ToInt32(argQTrans.Rows[u]["Amount"]);

                    Sql = "Insert Into dbo.ExtraBillRateQ(BillRegId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount, "+
                            " ExpValue,ExpPerValue,SurValue,EdValue,FlatId,HEDPer,NetPer,TaxablePer,TaxableValue) Values(" + iBillRegId + 
                            "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() +
                            "'," + argQTrans.Rows[u]["ExpPer"].ToString() + ",'" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() +
                            "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() +
                            "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," + argQTrans.Rows[u]["ExpPerValue"].ToString() +
                            "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EdValue"].ToString() +
                            "," + argBRegBo.FlatId + "," + argQTrans.Rows[u]["HEDPer"].ToString() + "," + argQTrans.Rows[u]["NetPer"].ToString() +
                            "," + argQTrans.Rows[u]["TaxablePer"].ToString() + "," + argQTrans.Rows[u]["TaxableValue"].ToString() + ")";
                    cmd = new SqlCommand(Sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                if (argQTrans.Rows.Count > 0)
                {
                    Sql = "Insert Into dbo.ExtraBillRateQAbs(BillRegId,QualifierId,Amount,Add_Less_Flag) " +
                              "Values (" + iBillRegId + "," + argQTrans.Rows[0]["QualifierId"].ToString() + "," + d_TotalTaxAmt +
                              ",'" + argQTrans.Rows[0]["Add_Less_Flag"].ToString() + "')";
                    cmd = new SqlCommand(Sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                tran.Rollback();
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
            return iBillRegId;
        }

        public void UpdateExtraBill(ExtraBillRegBO argBRegBo, DataTable argBTrans, DataTable argQTrans)
        {   
            SqlConnection conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string frmdat = string.Format("{0:dd MMM yyyy}", argBRegBo.BillDate);

                // Bill Register
                string Sql = "Update dbo.ExtraBillRegister set BillDate='" + frmdat + "',BillNo='" + argBRegBo.RefNo + "',CostCentreId=" + argBRegBo.CCId + "," +
                             "FlatId=" + argBRegBo.FlatId + ", BillAmount=" + argBRegBo.BillAmt + ",NetAmount=" + argBRegBo.NetAmt + ",Narration='" + argBRegBo.Narration + "' " +
                             "Where BillRegId=" + argBRegBo.BillRegId + " ";
                SqlCommand cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                int iBillRegId = argBRegBo.BillRegId;

                Sql = "DELETE FROM dbo.ExtraBillTrans WHERE BillRegId=" + iBillRegId;
                cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Sql = "Delete from dbo.ExtraBillRateQ where BillRegId=" + iBillRegId;
                cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                Sql = "Delete from dbo.ExtraBillRateQAbs where BillRegId=" + iBillRegId;
                cmd = new SqlCommand(Sql, conn, tran);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                //BillTrans
                for (int u = 0; u < argBTrans.Rows.Count; u++)
                {
                    Sql = "Insert Into dbo.ExtraBillTrans (BillRegId,ExtraItemId,Qty,Rate,Amount,NetAmount) " +
                          "Values (" + iBillRegId + "," + argBTrans.Rows[u]["EItemId"].ToString() + "," + argBTrans.Rows[u]["Qty"].ToString() +
                          "," + argBTrans.Rows[u]["Rate"].ToString() + "," + argBTrans.Rows[u]["Amt"].ToString() + "," + argBTrans.Rows[u]["NetAmt"].ToString() + ")";
                    cmd = new SqlCommand(Sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                //Bill Qualifier
                decimal d_TotalTaxAmt = 0;
                for (int u = 0; u < argQTrans.Rows.Count; u++)
                {
                    d_TotalTaxAmt = d_TotalTaxAmt + Convert.ToInt32(argQTrans.Rows[u]["Amount"]);

                    Sql = "Insert Into dbo.ExtraBillRateQ(BillRegId,QualifierId,Expression,ExpPer,Add_Less_Flag,SurCharge,EDCess,Amount, " +
                            " ExpValue,ExpPerValue,SurValue,EdValue,FlatId,HEDPer,NetPer,TaxablePer,TaxableValue) Values(" + iBillRegId +
                            "," + argQTrans.Rows[u]["QualifierId"].ToString() + ",'" + argQTrans.Rows[u]["Expression"].ToString() +
                            "'," + argQTrans.Rows[u]["ExpPer"].ToString() + ",'" + argQTrans.Rows[u]["Add_Less_Flag"].ToString() +
                            "'," + argQTrans.Rows[u]["SurCharge"].ToString() + "," + argQTrans.Rows[u]["EDCess"].ToString() + "," + argQTrans.Rows[u]["Amount"].ToString() +
                            "," + argQTrans.Rows[u]["ExpValue"].ToString() + "," + argQTrans.Rows[u]["ExpPerValue"].ToString() +
                            "," + argQTrans.Rows[u]["SurValue"].ToString() + "," + argQTrans.Rows[u]["EdValue"].ToString() +
                            "," + argBRegBo.FlatId + "," + argQTrans.Rows[u]["HEDPer"].ToString() + "," + argQTrans.Rows[u]["NetPer"].ToString() +
                            "," + argQTrans.Rows[u]["TaxablePer"].ToString() + "," + argQTrans.Rows[u]["TaxableValue"].ToString() + ")";
                    cmd = new SqlCommand(Sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                } 
                
                if (argQTrans.Rows.Count > 0)
                {
                    Sql = "Insert Into dbo.ExtraBillRateQAbs(BillRegId,QualifierId,Amount,Add_Less_Flag) " +
                              "Values (" + iBillRegId + "," + argQTrans.Rows[0]["QualifierId"].ToString() + "," + d_TotalTaxAmt +
                              ",'" + argQTrans.Rows[0]["Add_Less_Flag"].ToString() + "')";
                    cmd = new SqlCommand(Sql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                tran.Rollback();
            }
            finally
            {
                conn.Dispose();
                conn.Close();
            }
        }

        #endregion
    }
}
