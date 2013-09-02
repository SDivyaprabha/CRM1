using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class TemplateDL
    {
        #region Methods

        public DataTable GetTemplate(string argType)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select TemplateId,TemplateName,Case When TempDoc is Null Then Convert(bit,0,0) Else Convert(bit,1,1) End FileFound from Template " +
                      "Where TemplateType = '" + argType + "' Order by TemplateName";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                throw new Exception(ee.Message);
            }
            return dt;
        }


        public int InsertTempname(string argTempName,string argTempType)
        {
            int identity = 0;
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                string sSql = "Insert into Template(TemplateName,TemplateType) Values('" + argTempName + "','" + argTempType + "') SELECT SCOPE_IDENTITY();";
                SqlCommand Command = new SqlCommand(sSql, conn, tran);
                identity = int.Parse(Command.ExecuteScalar().ToString());
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }
            return identity;
        }

        public void UpdateTemplate(int argTempId, string argTempName)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;

            try
            {
                sSql = "Update Template Set TemplateName= '" + argTempName + "' Where TemplateId = " + argTempId;
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }


        public void TemplateAttach(int argTemId, byte[] argImageData, System.IO.FileStream fileMode)
        {
            BsfGlobal.OpenCRMDB();            
            string sSql="";
            SqlCommand cmd = null;
            try
            {
                if (argImageData != null)
                {
                    sSql = "Update Template Set TempDoc= @Doc  Where TemplateId = " + argTemId;

                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    cmd.Parameters.Add("@Doc", SqlDbType.Binary, Convert.ToInt32(fileMode.Length)).Value = argImageData;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
               
            }

            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

        }

        public bool CheckTemplateUsed(int argTempId)
        {
            bool bAns = false;
            BsfGlobal.OpenCRMDB();
            string sSql = "Select TemplateId From CostCentreTemplate Where TemplateId=" + argTempId + " " +
                          "Union All " +
                          "Select TemplateId from FlatTemplateUpload Where TemplateId=" + argTempId;
            SqlDataAdapter sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            sda.Dispose();
            if (dt.Rows.Count > 0) { bAns = true; }
            dt.Dispose();
            BsfGlobal.g_CRMDB.Close();
            return bAns;
        }


        public void DeleteTempate(int argId)
        {
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Delete from Template Where TemplateId = " + argId;
                SqlCommand Command = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                Command.ExecuteNonQuery();
                BsfGlobal.g_CRMDB.Close();
            }
            catch
            {
            }
           
        }
        public void RemoveTempate(int argId)
        {
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Update Template Set TempDoc = null Where TemplateId = " + argId;
                SqlCommand Command = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                Command.ExecuteNonQuery();
                BsfGlobal.g_CRMDB.Close();
            }
            catch
            {
            }

        }

        public void RemoveTemplateDoc(int argFlatId,int argTempId)
        {
            try
            {
                BsfGlobal.OpenCRMDB();
                string sSql = "Delete from FlatTemplateDocTrans Where FlatId= " + argFlatId + " and TemplateId= " + argTempId;
                SqlCommand Command = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                Command.ExecuteNonQuery();
                BsfGlobal.g_CRMDB.Close();
            }
            catch
            {
            }

        }


        public byte[] GetDocTemp(int argTempId)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader OleDbReader1 = null;
            byte[] data = null;

            Cmd.CommandText = "SELECT TempDoc From Template Where TemplateId=" + argTempId + " and TempDoc is not null";
            Cmd.Connection = BsfGlobal.OpenCRMDB();
            OleDbReader1 = Cmd.ExecuteReader();
            OleDbReader1.Read();
            if (OleDbReader1.HasRows == false)
                return data;

            long Len1 = OleDbReader1.GetBytes(0, 0, null, 0, 0);
            byte[] Array1 = new byte[Convert.ToInt32(Len1) + 1];
            OleDbReader1.GetBytes(0, 0, Array1, 0, Convert.ToInt32(Len1));
            data = Array1;           

            Cmd.Dispose();
            BsfGlobal.g_CRMDB.Close();

            return data;
        }        

        //public DataTable GetTemplateList()
        //{
        //    DataTable dt;
        //    SqlDataAdapter sda;
        //    string sql = "";
        //    BsfGlobal.OpenCRMDB();

        //    try
        //    {
        //        sql = "Select TemplateId,TemplateName,Case When TempDoc is Null Then Convert(bit,1,1) Else Convert(bit,0,0) End FileFound from Template Order by TemplateName";
        //        sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
        //        dt = new DataTable();
        //        sda.Fill(dt);
        //        sda.Dispose();
        //        BsfGlobal.g_CRMDB.Close();
        //    }
        //    catch (SqlException ee)
        //    {
        //        throw new Exception(ee.Message);
        //    }
        //    return dt;
        //}

        public DataTable GetFlatDetails(int argFlatId)
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                sql = "SELECT LD.LeadName BuyerName,P1.FatherName,CASE When P1.DOB='01 Jan 1900' Then 0 Else DATEDIFF(Year,P1.DOB,GetDate()) END as Age, " +
                        " B.BlockName,L.LevelName,LC.Address1, LC.Address2,LC.City,LC.State,CM.CountryName,LC.PinCode,LC.PanNo, " +
                        " ISNULL(F.FlatNo,'')FlatNo,ISNULL(F.USLand,0)USLand,ISNULL(F.Area,0)Area,ISNULL(E.FinaliseDate,'') FinalizationDate,GETDATE() SystemDate, " +
                        " O.CostCentreName,WC.Address1 CCAddress1,WC.Address2 CCAddress2,CCM.CityName CCCity,WC.Pincode CCPincode, " +
                        " WM.CompanyName,WM.Address1 CompanyAddress,WM.Phone CompanyPhone, "+
                        " CA.CoApplicantName,BM.BankName,CASE When CA.DOB='01 Jan 1900' Then 0 Else DATEDIFF(Year,CA.DOB,GetDate()) END as CoApplicantAge," +
                        " P2.CoAddress1 CoApplicantAddress1, P2.CoAddress2 CoApplicantAddress2, P2.CoPanNo CoApplicantPANNo, " +
                        " BD.Branch BankBranch,BD.Address1 BankAddress,BD.ContPerson BankContactPerson," +
                        " E.LoanAccNo BankLoanAccNo,LD.Mobile MobileNo,LD.Email, ISNULL(FT.TypeName,'') FlatType, "+
                        " ISNULL(F.USLand,0) UDS, [UDS(in words)]=dbo.NumberToWords(ISNULL(F.USLand,0)), " +
                        " ISNULL(F.Rate,0) Rate, [Rate(in words)]=dbo.NumberToWords(ISNULL(F.Rate,0)), " +
                        " ISNULL(F.LandRate,0) LandCost, [LandCost(in words)]=dbo.NumberToWords(ISNULL(F.LandRate,0)), " +
                        " ISNULL(F.AdvAmount,0) Advance, [Advance(in words)]=dbo.NumberToWords(ISNULL(F.AdvAmount,0)), " +
                        " (ISNULL(F.LandRate,0)-ISNULL(F.AdvAmount,0)) SaleAgreementLandValue, "+
                        " [SaleAgreementLandValue(in words)]=dbo.NumberToWords((ISNULL(F.LandRate,0)-ISNULL(F.AdvAmount,0))), " +
                        " ISNULL(F.OtherCostAmt, 0) OtherCostAmt, " +
                        " RegistrationValue=ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=1 And B.FlatId=" + argFlatId + "),0), " +
                        " [RegistrationValue(in words)]=dbo.NumberToWords(ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=1 And B.FlatId=" + argFlatId + "),0)), " +
                        " LegalCharges=ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=7 And B.FlatId=" + argFlatId + "),0), " +
                        " [LegalCharges(in words)]=dbo.NumberToWords(ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=7 And B.FlatId=" + argFlatId + "),0)), " +
                        " CorpusFund=ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=8 And B.FlatId=" + argFlatId + "),0), " +
                        " [CorpusFund(in words)]=dbo.NumberToWords(ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=8 And B.FlatId=" + argFlatId + "),0)), " +
                        " MaintenanceCharge=ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=3 And B.FlatId=" + argFlatId + "),0), " +
                        " [MaintenanceCharge(in words)]=dbo.NumberToWords(ISNULL((Select B.Amount From dbo.OtherCostMaster A Inner Join dbo.FlatOtherCost B On A.OtherCostId=B.OtherCostId Where A.OCTypeId=3 And B.FlatId=" + argFlatId + "),0)), " +
                        " ISNULL(F.BaseAmt,0) BasicCost,[BasicCost(in words)]=dbo.NumberToWords(ISNULL(F.BaseAmt,0)), " +                        
                        " ISNULL(F.NetAmt,0) FlatCost, [FlatCost(in words)]=dbo.NumberToWords(ISNULL(F.NetAmt,0)), " +
                        " ISNULL(F.NetAmt,0) Amount, [Amount(in words)]=dbo.NumberToWords(ISNULL(F.NetAmt,0)), " +
                        " (ISNULL(F.NetAmt,0)+ISNULL(F.QualifierAmt,0)) NetAmount, [NetAmount(in words)]=dbo.NumberToWords(ISNULL(F.NetAmt,0)+ISNULL(F.QualifierAmt,0)), " +
                        " PaidAmount=(Select Sum(ISNULL(PaidAmount,0)) From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + "), " +
                        " [PaidAmount(in words)]=dbo.NumberToWords((Select Sum(ISNULL(PaidAmount,0)) From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + ")), " +
                        " BalanceAmount=(Select Sum(ISNULL(NetAmount,0)-ISNULL(PaidAmount,0))BalanceAmount From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + "), " +
                        " [BalanceAmount(in words)]=dbo.NumberToWords((Select Sum(ISNULL(NetAmount,0)-ISNULL(PaidAmount,0))BalanceAmount From dbo.PaymentScheduleFlat Where FlatId=" + argFlatId + ")), " +
                        " ISNULL(P4.TypeName,'') CarParkType, ISNULL(F.QualifierAmt,0) QualifierAmount FROM FlatDetails F " +
                        " INNER JOIN FlatType FT ON F.FlatTypeId=FT.FlatTypeId  " +
                        " LEFT JOIN LevelMaster L ON F.LevelId=L.LevelId " +
                        " LEFT JOIN BlockMaster B ON F.BlockId=B.BlockId " +
                        " LEFT JOIN BuyerDetail E ON E.FlatId=F.FlatId AND E.LeadId=F.LeadId " +
                        " LEFT JOIN LeadRegister LD ON E.LeadId=LD.LeadId " +
                        " LEFT JOIN LeadCommAddressInfo LC ON LD.LeadId=LC.LeadId " +
                        " LEFT JOIN Countrymaster CM ON LC.Country=CM.CountryId " +
                        " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=F.CostCentreId" +
                        " LEFT JOIN LeadCoApplicantInfo CA On LD.LeadId=CA.LeadId" +
                        " LEFT JOIN BankDetails BD On BD.BranchId=E.BranchId" +
                        " LEFT JOIN BankMaster BM On BD.BankId=BM.BankId" +
                        " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CostCentre WC On WC.CostCentreId=B.CostCentreId" +
                        " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CompanyMaster WM On WM.CompanyId=WC.CompanyId" +
                        " LEFT JOIN [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster CCM ON CCM.CityId=WC.CityId " +
                        " LEFT JOIN dbo.LeadPersonalInfo P1 ON P1.LeadId=LD.LeadId " +
                        " LEFT JOIN dbo.LeadCoAppAddressInfo P2 ON P2.LeadId=LD.LeadId " +
                        " LEFT JOIN dbo.FlatCarPark P3 ON F.FlatId=P3.FlatId AND P3.TotalCP<>0 " +
                        " LEFT JOIN dbo.CarParkTypeMaster P4 ON P3.TypeId=P4.TypeId " +
                        " INNER JOIN dbo.ProjectInfo P5 ON F.CostCentreId=P5.CostCentreId " +
                        " WHERE F.FlatId=" + argFlatId;
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                dt.Columns.Add("ChequeNo", typeof(string));
                dt.Columns.Add("ReceiptDate", typeof(string));

                sql = "Select A.ChequeNo,A.ReceiptDate From ReceiptRegister A Inner Join ReceiptTrans B On A.ReceiptId=B.ReceiptId " +
                     " Where B.FlatId=" + argFlatId + " Group By A.ChequeNo,A.ReceiptDate";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                DataTable dtR = new DataTable();
                sda.Fill(dtR);
                sda.Dispose();

                string sChequeNo = "", sReceiptDate = "";
                for (int i = 0; i < dtR.Rows.Count; i++)
                {
                    string sNo = dtR.Rows[i]["ChequeNo"].ToString();

                    DateTime dChequeDate = Convert.ToDateTime(CommFun.IsNullCheck(dtR.Rows[i]["ReceiptDate"], CommFun.datatypes.VarTypeDate));                    
                    string sDate = "";
                    if (dChequeDate == DateTime.MinValue)
                        sDate = "";
                    else
                        sDate = string.Format("{0:dd-MMM-yyyy}", dChequeDate);

                    sChequeNo = sChequeNo + sNo + ",";
                    sReceiptDate = sReceiptDate + sDate + ",";
                }
                sChequeNo = sChequeNo.TrimEnd(',');
                sReceiptDate = sReceiptDate.TrimEnd(',');

                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["ChequeNo"] = sChequeNo.ToString();
                    dt.Rows[0]["ReceiptDate"] = sReceiptDate.ToString();
                }

                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                throw new Exception(ee.Message);
            }
            return dt;
        }


        public void InsertTempTable(DataTable argdtCaption)
        {
            SqlConnection conn;
            SqlCommand cmd = null;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";
            try
            {
                sSql = "Truncate table TempFieldList";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();

                sSql = "Insert into TempFieldList(Description) Select Description from TemplateFieldList";
                cmd = new SqlCommand(sSql, conn, tran);
                cmd.ExecuteNonQuery();                
                tran.Commit();
                UpdateCaption(argdtCaption);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void UpdateCaption(DataTable argdtCaption)
        {
            SqlConnection conn;
            SqlCommand cmd = null;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";
            try
            {
                sSql = "Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["BuyerName"].ToString() + "' Where Description='<Name>'";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["BlockName"].ToString() + "' Where Description='<Block>' ";
                
                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["LevelName"].ToString() + "' Where Description='<Level>' ";
                
                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["Address1"].ToString() + "' Where Description='<Address1>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["Address2"].ToString() + "' Where Description='<Address2>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["City"].ToString() + "' Where Description='<City>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["State"].ToString() + "' Where Description='<State>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["CountryName"].ToString() + "' Where Description='<Country>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["PinCode"].ToString() + "' Where Description='<PinCode>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["PanNo"].ToString() + "' Where Description='<PANNo>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["FlatNo"].ToString() + "' Where Description='<FlatNo>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["USLand"].ToString() + "' Where Description='<UDS>' ";

                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["Area"].ToString() + "' Where Description='<Area>' ";
                
                sSql = sSql + " Update  TempFieldList Set Caption='" + argdtCaption.Rows[0]["Rate"].ToString() + "' Where Description='<Rate>' ";

                cmd = new SqlCommand(sSql,  conn , tran);
                cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new Exception(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable GetTempMegreList()
        {
            DataTable dt;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                sql = "Select * from TempFieldList";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                throw new Exception(ee.Message);
            }
            return dt;
        }


        #endregion

        #region CostCentre/Flat Wise Methods

        public DataTable GetCCTemplate(string frmWhere,int argCCId,int argFlatId)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                if (frmWhere == "P")
                {
                    sql = "Select A.TemplateId,A.TemplateName,Case When B.TemplateDoc is Null Then Convert(bit,0,0) Else Convert(bit,1,1) End FileFound from Template A " +
                            "Left Join CostCentreTemplate B on A.TemplateId=B.TemplateId and B.CostCentreId= " + argCCId + " " +
                            "Where A.TemplateType='Project' Order by A.TemplateName";
                    sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
                }
                else if (frmWhere == "Flat-Doc")
                {
                    sql = "Select A.TemplateId,A.TemplateName,Case When B.TemplateDoc is Null Then Convert(bit,0,0) Else Convert(bit,1,1) End FileFound from Template A " +
                            "Left Join FlatTemplateUpload B on A.TemplateId=B.TemplateId and B.FlatId= " + argFlatId + " " +
                            "Where A.TemplateType='Flat-Doc' Order by A.TemplateName";
                    sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
                }   
                else 
                {
                    sql = "Select A.TemplateId,A.TemplateName,Case When B.TemplateDoc is Null Then Convert(bit,0,0) Else Convert(bit,1,1) End FileFound from Template A " +
                            "Left Join FlatTemplateUpload B on A.TemplateId=B.TemplateId and B.FlatId= " + argFlatId + " " +
                            "Where A.TemplateType='Flat' Order by A.TemplateName";
                    sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                    dt = new DataTable();
                    sda.Fill(dt);
                    sda.Dispose();
                }              

                BsfGlobal.g_CRMDB.Close();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dt;
        }

        public void InsertCCTempname(string frmWhere,int argTempID)
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            string sSql = "";
            try
            {
                if (frmWhere=="P")
                    sSql = "Insert into CostCentreTemplate(TemplateId) Values('" + argTempID + "')";
                else
                    sSql = "Insert into FlatTemplateUpload(TemplateId) Values('" + argTempID + "')";

                SqlCommand Command = new SqlCommand(sSql, conn, tran);
                Command.ExecuteNonQuery();                
                tran.Commit();
            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public void UpdateCCTempate(string frmWhere, int argTemId, byte[] argImageData, int argCCId, int argFlatId, System.IO.FileStream fileMode, string argExt)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            SqlCommand cmd = null;
            try
            {
                if (frmWhere == "P")
                {
                    sSql = "Update CostCentreTemplate Set CostCentreId = " + argCCId + ",TemplateId= " + argTemId + " Where CostCentreId = " + argCCId + " and TemplateId= " + argTemId;
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    int i= cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (i == 0)
                    {
                        sSql = "Insert into CostCentreTemplate(CostCentreId,TemplateId) Values (" + argCCId + "," + argTemId + ")";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }


                    if (argImageData != null)
                    {

                        sSql = "Update CostCentreTemplate Set TemplateDoc= @Doc,Extension='" + argExt + "'  Where TemplateId = " + argTemId + " and CostCentreId = " + argCCId;

                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.Parameters.Add("@Doc", SqlDbType.Binary, Convert.ToInt32(fileMode.Length)).Value = argImageData;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                else
                {
                    sSql = "Update FlatTemplateUpload Set FlatId = " + argFlatId + ",TemplateId= " + argTemId + " Where FlatId = " + argFlatId + " and TemplateId= " + argTemId;
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    int i = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    if (i == 0)
                    {
                        sSql = "Insert into FlatTemplateUpload(FlatId,TemplateId) Values (" + argFlatId + "," + argTemId + ")";
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    if (argImageData != null)
                    {
                        sSql = "Update FlatTemplateUpload Set TemplateDoc= @Doc,Extension='" + argExt + "'  Where TemplateId = " + argTemId + " and FlatId = " + argFlatId;
                        cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                        cmd.Parameters.Add("@Doc", SqlDbType.Binary, Convert.ToInt32(fileMode.Length)).Value = argImageData;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
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

        public void DeleteCCTempate(string frmWhere, int argTempId,int argCCId,int argFlatId)
        {
            BsfGlobal.OpenCRMDB();
            string sSql = "";
            try
            {
                if (frmWhere == "P")
                    sSql = "Delete from CostCentreTemplate Where TemplateId = " + argTempId + " and CostCentreId= " + argCCId;
                else
                    sSql = "Delete from FlatTemplateUpload Where TemplateId = " + argTempId + " and FlatId = " + argFlatId;
                SqlCommand Command = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                Command.ExecuteNonQuery();
         
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


        public DataTable GetCCTemplateView(string frmWhere, int argID,int argFlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                if (frmWhere == "P")
                    sql = "Select A.CostCentreId ID,B.TemplateName,A.TemplateDoc,A.Extension from CostCentreTemplate A" +
                             "Inner join Template on B A.TemplateId=B.TemplateId  Where A.CostCentreId=" + argID + "";
                else
                    sql = "Select A.FlatId ID,B.TemplateName,A.TemplateDoc,A.Extension from FlatTemplateUpload A " +
                              "Inner join Template on B A.TemplateId=B.TemplateId  Where A.FlatId=" + argFlatId + "";

                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return dt;
        }

        public System.Drawing.Bitmap GetImage(int argCCId, int argFLatId, int argTemplateId, string argfrmWhere)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader OleDbReader1 = null;
            System.Drawing.Bitmap BImg = null;
            try
            {
                if (argfrmWhere == "P")
                    Cmd.CommandText = "SELECT TemplateDoc From CostCentreTemplate Where TemplateId=" + argTemplateId + " And CostCentreId=" + argCCId;
                else
                    Cmd.CommandText = "SELECT TemplateDoc From FlatTemplateUpload Where TemplateId=" + argTemplateId + " And FlatId =" + argFLatId;

                Cmd.Connection = BsfGlobal.OpenCRMDB();
                OleDbReader1 = Cmd.ExecuteReader();
                OleDbReader1.Read();
                if (OleDbReader1.HasRows == false)
                    return BImg;

                long Len1 = OleDbReader1.GetBytes(0, 0, null, 0, 0);
                byte[] Array1 = new byte[Convert.ToInt32(Len1) + 1];
                OleDbReader1.GetBytes(0, 0, Array1, 0, Convert.ToInt32(Len1));

                System.IO.MemoryStream MemoryStream1 = new System.IO.MemoryStream(Array1);
                BImg = new System.Drawing.Bitmap(MemoryStream1);

                //PicDoc.SizeMode = PictureBoxSizeMode.StretchImage;
                //PicDoc.Image = BImg;

                Cmd.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return BImg;
        }


        public System.Drawing.Image GetPDF(int argCCId, int argFLatId, int argTemplateId, string argfrmWhere)
        {
            SqlCommand Cmd = new SqlCommand();
            System.Drawing.Image BImg = null;
            try
            {
                if (argfrmWhere == "P")
                    Cmd.CommandText = "SELECT TemplateDoc From CostCentreTemplate Where CCTemplateId=" + argTemplateId + " and CostCentreId=" + argCCId;
                else
                    Cmd.CommandText = "SELECT TemplateDoc From FlatTemplateUpload Where FlatTemplateId=" + argTemplateId + " and FlatId =" + argFLatId;

                Cmd.Connection = BsfGlobal.OpenCRMDB();
                //OleDbReader1 = Cmd.ExecuteReader();
                //OleDbReader1.Read();
                //if (OleDbReader1.HasRows == false)
                //    return BImg;

                //long Len1 = OleDbReader1.GetBytes(0, 0, null, 0, 0);
                byte[] Array1 = (byte[])Cmd.ExecuteScalar();
                //OleDbReader1.GetBytes(0, 0, Array1, 0, Convert.ToInt32(Len1));

                using (System.IO.MemoryStream MemoryStream1 = new System.IO.MemoryStream(Array1, 0, Array1.Length))
                {
                    MemoryStream1.Write(Array1, 0, Array1.Length);
                    BImg = System.Drawing.Image.FromStream(MemoryStream1, true);
                }

                Cmd.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return BImg;
        }

        public DataTable GetFlatTemplateList()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                sql = "Select TemplateId,TemplateName,TempDoc from dbo.Template Where TemplateType='Flat' and TempDoc is not Null";
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return dt;
        }

        public string GetDocumentPath()
        {
            string sPath = "";
            BsfGlobal.OpenWorkFlowDB();
            try
            {
                string sql = "Select DocumentFilePath from CompanyMailSetting";
                SqlDataAdapter sda = new SqlDataAdapter(sql, BsfGlobal.g_WorkFlowDB);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();

                if (dt.Rows.Count > 0)
                {
                    sPath = CommFun.IsNullCheck(dt.Rows[0]["DocumentFilePath"].ToString(), CommFun.datatypes.vartypestring).ToString();
                }
                dt.Dispose();

                BsfGlobal.g_WorkFlowDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return sPath;
        }

        public DataTable GetFlatDocDetails(int argFTempId,int argCCId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();
            try
            {
                sql = "Select A.FlatId,F.FlatNo,C.BlockName,A.DocPath from FlatTemplateDocTrans A " +
                    "Inner Join FlatDetails F on F.FlatId=A.FlatId " +
                    "Left Join BlockMaster C on F.BlockId=C.BlockId Where A.TemplateId=" + argFTempId + " and F.CostCentreId = " + argCCId;
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return dt;
        }

        public DataTable GetFlatDocCreate(int argCCId,int argTempId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
                sql = "Select A.FlatId,A.FlatNo,B.BlockName,'' Path,Convert(bit,0,0) Sel from FlatDetails A " +
                      "Left Join BlockMaster B on A.BlockId=B.BlockId " +
                      "Where A.Status='S' and A.CostCentreId=" + argCCId + " and " +
                      "A.FlatId not in (Select FlatId from FlatTemplateDocTrans Where TemplateId=" + argTempId + ")";
                  
                sda = new SqlDataAdapter(sql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
                sda.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return dt;
        }


        public void InsertFlatDocTrans(DataTable argDtTrans, int argTempId)
        {
            SqlConnection conn;
            conn = new SqlConnection();
            conn = BsfGlobal.OpenCRMDB();
            SqlTransaction tran = conn.BeginTransaction();
            SqlCommand cmd = null;
            string sSql = "";
            try
            {
                //sSql = "Delete from  FlatTemplateDocTrans where TemplateId=" + argTempId;
                //cmd = new SqlCommand(sSql, conn, tran);
                //cmd.ExecuteNonQuery();

                for (int s = 0; s < argDtTrans.Rows.Count; s++)
                {
                    sSql = "Insert into FlatTemplateDocTrans(FlatId,TemplateId,DocPath) Values(" + argDtTrans.Rows[s]["FlatId"].ToString() + "," + argTempId + ",'" + argDtTrans.Rows[s]["Path"].ToString() + "')";
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                }
                tran.Commit();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            finally
            {
                conn.Close();
            }
        }

        public string CCGetExtension(int argTempId, string argFrom)
        {
            SqlCommand Cmd = null;
            BsfGlobal.OpenCRMDB();
            string extn = "";
            string sql = "";
            try
            {
                if (argFrom == "P")
                    sql = "SELECT Extension From CostCentreTemplate Where TemplateId=" + argTempId + "";
                else
                    sql = "SELECT Extension From FlatTemplateUpload Where TemplateId=" + argTempId + "";

                Cmd = new SqlCommand(sql, BsfGlobal.g_CRMDB);
                extn = CommFun.IsNullCheck(Cmd.ExecuteScalar(), CommFun.datatypes.vartypestring).ToString();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return extn;
        }

        public string CompGetExtension(int argTempId)
        {
            SqlCommand Cmd = null;
            BsfGlobal.OpenCRMDB();
            string extn = "";
            string sql = "";
            try
            {
                sql = "SELECT Extension From CompTemplate Where TemplateId=" + argTempId + "";
                Cmd = new SqlCommand(sql, BsfGlobal.g_CRMDB);
                extn = CommFun.IsNullCheck(Cmd.ExecuteScalar(), CommFun.datatypes.vartypestring).ToString();

                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return extn;
        }

        public byte[] CCGetDocTemp(int argTempId,string argFrom)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader OleDbReader1 = null;
            string sql = "";
            byte[] data = null;
            try
            {
                if (argFrom == "P")
                    sql = "SELECT TemplateDoc From CostCentreTemplate Where TemplateId=" + argTempId + " and TemplateDoc is not null";
                else
                    sql = "SELECT TemplateDoc From FlatTemplateUpload Where TemplateId=" + argTempId + " and TemplateDoc is not null";
                Cmd.CommandText = sql.ToString();
                Cmd.Connection = BsfGlobal.OpenCRMDB();
                OleDbReader1 = Cmd.ExecuteReader();
                OleDbReader1.Read();
                if (OleDbReader1.HasRows == false)
                    return data;

                long Len1 = OleDbReader1.GetBytes(0, 0, null, 0, 0);
                byte[] Array1 = new byte[Convert.ToInt32(Len1) + 1];
                OleDbReader1.GetBytes(0, 0, Array1, 0, Convert.ToInt32(Len1));
                data = Array1;

                Cmd.Dispose();
                BsfGlobal.g_CRMDB.Close();
            }
            catch (SqlException ee)
            {
                BsfGlobal.CustomException(ee.Message, ee.StackTrace);
            }
            return data;
        }        

        #endregion

    }
}
