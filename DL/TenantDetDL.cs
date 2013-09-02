using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessObjects;

namespace CRM.DataLayer
{
    class TenantDetDL
    {
        public static DataTable GetTenant()
        {
            DataTable dtTen = null;
            SqlDataAdapter sda;
           // string sSql = "";
            BsfGlobal.OpenCRMDB();

            try
            {
               // sSql = "select TenantId,TenantName from TenantDet where TenantId <> 0 ";

               //// sda = new SqlDataAdapter(sSql, BsfGlobal.OpenWorkFlowDB());
               // sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
               // dtTen = new DataTable();
               // sda.Fill(dtTen);
               // BsfGlobal.g_CRMDB.Close();
                sda = new SqlDataAdapter("SELECT TenantId,TenantName from TenantRegister where TenantId <> 0", BsfGlobal.g_CRMDB);
                dtTen = new DataTable();
                sda.Fill(dtTen);
                BsfGlobal.g_CRMDB.Close();

            }
            catch (Exception e)
            {
                BsfGlobal.CustomException(e.Message, e.StackTrace);
            }
            return dtTen;

          
        }
        public int InsertTenantContact()
        {
            int identity = 0;

            try
            {
                SqlCommand cmd;

                if (TenantDetBO.TenantId == 0)
                {
                    string sSql = String.Format("INSERT INTO TenantTenantRegister(TenantName,Address1,Address2,CityId,StateId,CountryId,Pincode,Mobile,PhoneRes,Email, EmpPlace,Designation,OffAddress,OffPhone,OffEmail,Commands,CostCentreId,BlockId,FlatId) VALUES('{0}', '{1}', '{2}','{3}','{4}', '{5}','{6}','{7}','{8}', '{9}','{10}','{11}', '{12}','{13}','{14}','{15}',{16},{17},{18} ) SELECT SCOPE_IDENTITY()", TenantDetBO.TenantName, TenantDetBO.Address1, TenantDetBO.Address2, TenantDetBO.City, TenantDetBO.State, TenantDetBO.Country, TenantDetBO.Pincode, TenantDetBO.Mobile, TenantDetBO.PhoneRes, TenantDetBO.Email, TenantDetBO.EmpPlace, TenantDetBO.Designation, TenantDetBO.OffAddress, TenantDetBO.OffPhone, TenantDetBO.OffEmail, TenantDetBO.Commands, TenantDetBO.CostCentreId, TenantDetBO.BlockId, TenantDetBO.FlatId);
                    BsfGlobal.OpenCRMDB();
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    identity = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();
                }
                else
                {
                    string sSql = String.Format("UPDATE TenantRegister SET TenantName='{0}', Address1='{1}',Address2='{2}',CityId='{3}', StateId='{4}',CountryId='{5}',Pincode='{6}',Mobile='{7}',PhoneRes='{8}', Email='{9}',EmpPlace='{10}',Designation='{11}', OffAddress='{12}',OffPhone='{13}',OffEmail='{14}',Commands='{15}',CostCentreId={16},BlockId={17},FlatId={18} WHERE TenantId={19}", TenantDetBO.TenantName, TenantDetBO.Address1, TenantDetBO.Address2, TenantDetBO.City, TenantDetBO.State, TenantDetBO.Country, TenantDetBO.Pincode, TenantDetBO.Mobile, TenantDetBO.PhoneRes, TenantDetBO.Email, TenantDetBO.EmpPlace, TenantDetBO.Designation, TenantDetBO.OffAddress, TenantDetBO.OffPhone, TenantDetBO.OffEmail, TenantDetBO.Commands, TenantDetBO.CostCentreId, TenantDetBO.BlockId, TenantDetBO.FlatId, TenantDetBO.TenantId);  
                    BsfGlobal.OpenCRMDB();
                    cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);
                    //identity = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }
            return identity;
        }

        public static void UpdateTenant()
        {
           
            try
            {
                SqlCommand cmd;
                string sSql = String.Format("UPDATE TenantRegister SET TenantName='{0}', Address1='{1}',Address2='{2}',CityId='{3}', StateId='{4}',CountryId='{5}',Pincode='{6}',Mobile='{7}',PhoneRes='{8}', Email='{9}',EmpPlace='{10}',Designation='{11}', OffAddress='{12}',OffPhone='{13}',OffEmail='{14}',Commands='{15}' WHERE TenantId={16}", TenantDetBO.TenantName, TenantDetBO.Address1, TenantDetBO.Address2, TenantDetBO.City, TenantDetBO.State, TenantDetBO.Country, TenantDetBO.Pincode, TenantDetBO.Mobile, TenantDetBO.PhoneRes, TenantDetBO.Email, TenantDetBO.EmpPlace, TenantDetBO.Designation, TenantDetBO.OffAddress, TenantDetBO.OffPhone, TenantDetBO.OffEmail, TenantDetBO.Commands, TenantDetBO.CostCentreId, TenantDetBO.BlockId, TenantDetBO.FlatId, TenantDetBO.TenantId);
                BsfGlobal.OpenCRMDB();
                cmd = new SqlCommand(sSql, BsfGlobal.g_CRMDB);           
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception ce)
            {
                BsfGlobal.CustomException(ce.Message, ce.StackTrace);
            }
            finally
            {
                BsfGlobal.g_CRMDB.Close();
            }

        }

        public static bool InsertTenantDetails()
        {
            int iFTypeId = 0;
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {             
                try
                {
                    string sSql = String.Format("INSERT INTO TenantRegister(TenantName,Address1,Address2,CityId,StateId,CountryId,Pincode,Mobile,PhoneRes,Email, EmpPlace,Designation,OffAddress,OffPhone,OffEmail,Commands,CostCentreId,BlockId,FlatId,TransDate,RefNo) VALUES('{0}', '{1}', '{2}','{3}','{4}', '{5}','{6}','{7}','{8}', '{9}','{10}','{11}', '{12}','{13}','{14}','{15}',{16},{17},{18},'{19}','{20}' ) SELECT SCOPE_IDENTITY()", TenantDetBO.TenantName, TenantDetBO.Address1, TenantDetBO.Address2, TenantDetBO.City, TenantDetBO.State, TenantDetBO.Country, TenantDetBO.Pincode, TenantDetBO.Mobile, TenantDetBO.PhoneRes, TenantDetBO.Email, TenantDetBO.EmpPlace, TenantDetBO.Designation, TenantDetBO.OffAddress, TenantDetBO.OffPhone, TenantDetBO.OffEmail, TenantDetBO.Commands, TenantDetBO.CostCentreId, TenantDetBO.BlockId, TenantDetBO.FlatId, TenantDetBO.TransDate, TenantDetBO.RefNo);
                    cmd = new SqlCommand(sSql, conn, tran);
                    iFTypeId = int.Parse(cmd.ExecuteScalar().ToString());
                    cmd.Dispose();

                    //sSql1 = "INSERT INTO TenantTrans(CostCentreId,BlockId,UnitId,TenantId) values( " + TenantDetBO.CostCentreId + ", " + TenantDetBO.BlockId + ", " + TenantDetBO.UnitId + ", " + iFTypeId + ")";
                    //        //CommFun.CRMExecute(sSql);
                    //        cmd = new SqlCommand(sSql1, conn, tran);
                    //        cmd.ExecuteNonQuery();
                    //        cmd.Dispose();
                        
                    
                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Tenant-Add", "N", "Tenant", iFTypeId, TenantDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, TenantDetBO.RefNo, BsfGlobal.g_lUserId);

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bUpdate = false;
                    System.Windows.Forms.MessageBox.Show(ex.Message, "PMS", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return bUpdate;
        }

        public static bool UpdateTenantDetails()
        {
            bool bUpdate;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
            using (SqlTransaction tran = conn.BeginTransaction())
            {
                string sSql = "";
                try
                {
                    sSql = String.Format("UPDATE TenantRegister SET TenantName='{0}', Address1='{1}',Address2='{2}',CityId='{3}', StateId='{4}',CountryId='{5}',Pincode='{6}',Mobile='{7}',PhoneRes='{8}', Email='{9}',EmpPlace='{10}',Designation='{11}', OffAddress='{12}',OffPhone='{13}',OffEmail='{14}',Commands='{15}',CostCentreId={16},BlockId={17},FlatId={18},TransDate='{19}',RefNo='{20}' WHERE TenantId={21}", TenantDetBO.TenantName, TenantDetBO.Address1, TenantDetBO.Address2, TenantDetBO.City, TenantDetBO.State, TenantDetBO.Country, TenantDetBO.Pincode, TenantDetBO.Mobile, TenantDetBO.PhoneRes, TenantDetBO.Email, TenantDetBO.EmpPlace, TenantDetBO.Designation, TenantDetBO.OffAddress, TenantDetBO.OffPhone, TenantDetBO.OffEmail, TenantDetBO.Commands, TenantDetBO.CostCentreId, TenantDetBO.BlockId, TenantDetBO.FlatId, TenantDetBO.TransDate, TenantDetBO.RefNo, TenantDetBO.TenantId);  
                    cmd = new SqlCommand(sSql, conn, tran);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    //sSql = String.Format("UPDATE TenantTrans SET CostCentreId='{0}', BlockId='{1}',UnitId='{2}' WHERE TenantId={3}", TenantDetBO.CostCentreId, TenantDetBO.BlockId, TenantDetBO.UnitId, TenantDetBO.TenantId);                    
                    //cmd = new SqlCommand(sSql, conn, tran);
                    //        cmd.ExecuteNonQuery();
                    //        cmd.Dispose();
                    
                    tran.Commit();
                    bUpdate = true;
                    BsfGlobal.InsertLog(DateTime.Now, "Tenant-Edit", "E", "Tenant", TenantDetBO.TenantId, TenantDetBO.CostCentreId, 0, BsfGlobal.g_sCRMDBName, TenantDetBO.RefNo, BsfGlobal.g_lUserId);

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    bUpdate = false;
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Asset", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    BsfGlobal.CustomException(ex.Message, ex.StackTrace);
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return bUpdate;
        }


        public static DataTable PopulateCostcentre(int Id)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT CostCentreId,CostCentreName From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre" +
                    " Where ProjectDB in(Select ProjectName from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ConceptionRegister " +
                    " Where BusinessType ='B') and CostCentreId not in (Select CostCentreId " +
                    " From [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.UserCostCentreTrans Where " +
                    " UserId=" + Id + ") Order By CostCentreName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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



        public static DataTable PopulateCity()
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT CityId,CityName FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster ORDER BY CityName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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


        public static DataTable Fill_TenantDet(int argATRegId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT * FROM TenantRegister WHERE TenantId = " + argATRegId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable PopulateBlock(int ProId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT BlockId,BlockName FROM BlockMaster WHERE CostCentreId = " + ProId + "  ORDER BY BlockName";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable PopulateFlat(int ProId, int FlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT A.FlatId,A.FlatNo  FROM FlatDetails A LEFT OUTER JOIN  TenantRegister B on A.FlatId=B.FlatId where B.FlatId IS NULL and A.CostCentreId=" + ProId + " and A.BlockId=" + FlatId + " and A.Status in('S','R') ORDER BY A.FlatNo ";
               
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable PopulateFlatSt(int ProId, int FlatId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT FlatId,FlatNo FROM FlatDetails WHERE CostCentreId=" + ProId + " and BlockId=" + FlatId + " and Status in('S','R') ORDER BY FlatNo";

                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable Fill_CityDet(int cityId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = "SELECT A.CityId,A.CityName,B.StateID,B.StateName,C.CountryId,C.CountryName FROM [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster A inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.StateMaster B on A.StateId=B.StateID inner join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CountryMaster C on C.CountryId=A.CountryId WHERE A.CityId=" + cityId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable Fill_Tenantregister(DateTime frmDate, DateTime toDate)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                string frmdat = string.Format("{0:dd MMM yyyy}", frmDate);
                string tdat = string.Format("{0:dd MMM yyyy}", toDate.AddDays(0));


                sSql = "SELECT A.TenantId,A.TransDate,A.RefNo,C.CostCentreName,D.FlatNo,A.TenantName,B.CityName,A.Mobile,A.Email,A.Approve " +
                      " From TenantRegister A " +
                      " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster B on A.CityId=B.CityId " +
                      " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CostCentreId=C.CostCentreId " +
                      " Left join FlatDetails D on A.FlatId=D.FlatId " +
                      "Where A.TransDate between '" + frmdat + "'  And '" + tdat + "' ORDER BY A.TransDate,A.RefNo";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable Fill_TenantDetChange(int TenantId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
               
                sSql = "SELECT A.TenantId,A.TransDate,A.RefNo,C.CostCentreName,D.FlatNo,A.TenantName,B.CityName,A.Mobile,A.Email,A.Approve " +
                      " From TenantRegister A " +
                      " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.CityMaster B on A.CityId=B.CityId " +
                      " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre C on A.CostCentreId=C.CostCentreId " +
                      " Left join FlatDetails D on A.FlatId=D.FlatId " +
                      "where A.TenantId=" + TenantId + "";
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static DataTable CheckTenant(int TenantId)
        {
            DataTable dt = null;
            SqlDataAdapter sda;
            string sSql = "";
            try
            {
                BsfGlobal.OpenCRMDB();
                sSql = String.Format("SELECT A.TenantId FROM TenantRegister A inner join RentDetail B on A.TenantId=B.TenantId WHERE A.TenantId={0}", TenantId);
                sda = new SqlDataAdapter(sSql, BsfGlobal.g_CRMDB);
                dt = new DataTable();
                sda.Fill(dt);
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

        public static bool DeleteTenantRegister(int RegId, int argCostId, string argRefNo)
        {
            string sSql = "";
            bool bSuccess = false;
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd;
            conn = BsfGlobal.OpenCRMDB();
             SqlTransaction tran = conn.BeginTransaction();

             try
             {
                 sSql = String.Format("DELETE FROM TenantRegister WHERE TenantId={0}", RegId);
                 cmd = new SqlCommand(sSql, conn, tran);
                 cmd.ExecuteNonQuery();
                 cmd.Dispose();

                 BsfGlobal.InsertLog(DateTime.Now, "Tenant-Delete", "D", "Tenant", RegId, argCostId, 0, BsfGlobal.g_sCRMDBName, argRefNo, BsfGlobal.g_lUserId);

                 tran.Commit();
                 bSuccess = true;
                 tran.Dispose();

             }
             catch (Exception ce)
             {
                 tran.Rollback();
                 System.Windows.Forms.MessageBox.Show(ce.Message, "PMS", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                 BsfGlobal.CustomException(ce.Message, ce.StackTrace);
             }
             finally
             {
                 conn.Close();
                 conn.Dispose();
             }
             return bSuccess;
        }
    
    }
}
