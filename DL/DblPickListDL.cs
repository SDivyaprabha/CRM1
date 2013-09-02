using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CRM.BusinessLayer;

namespace CRM.DataLayer
{
    class DblPickListDL
    {
        public int Update(DblPickListBL oPickList, SqlConnection Con, string TableName)
        {
            SqlCommand cmd;
            string sSql;
            int i = 0;

            try
            {
                if (TableName == "Income")
                {
                    for (int j = 0; j < oPickList.DtGetData.Rows.Count; j++)
                    {
                        if (oPickList.DtGetData.Rows[j][0].ToString() == "")
                        {
                            sSql = "INSERT INTO dbo.Income (IncomeFrom,IncomeTo) Values ('" + oPickList.DtGetData.Rows[j][1].ToString() + "','" + oPickList.DtGetData.Rows[j][2].ToString() + "')";
                            cmd = new SqlCommand(sSql, Con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            sSql = "Update dbo.Income SET IncomeFrom= '" + oPickList.DtGetData.Rows[j][1].ToString() + "',IncomeTo= '" + oPickList.DtGetData.Rows[j][2].ToString() + "' WHERE IncomeID='" + Convert.ToInt32(oPickList.DtGetData.Rows[j][0]) + "' ";
                            cmd = new SqlCommand(sSql, Con);
                            cmd.ExecuteNonQuery();
                        }


                    }
                }
                if (TableName == "CostPreference")
                {
                    for (int j = 0; j < oPickList.DtGetData.Rows.Count; j++)
                    {
                        if (oPickList.DtGetData.Rows[j][0].ToString() == "")
                        {
                            sSql = "INSERT INTO dbo.CostPreference (CostPreferenceFrom,CostPreferenceTo) Values ('" + oPickList.DtGetData.Rows[j][1].ToString() + "','" + oPickList.DtGetData.Rows[j][2].ToString() + "')";
                            cmd = new SqlCommand(sSql, Con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            sSql = "Update dbo.CostPreference SET CostPreferenceFrom= '" + oPickList.DtGetData.Rows[j][1].ToString() + "',CostPreferenceTo= '" + oPickList.DtGetData.Rows[j][2].ToString() + "' WHERE CostPreferenceID='" + Convert.ToInt32(oPickList.DtGetData.Rows[j][0]) + "' ";
                            cmd = new SqlCommand(sSql, Con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                if (TableName == "ApartmentSize")
                {
                    for (int j = 0; j < oPickList.DtGetData.Rows.Count; j++)
                    {
                        if (oPickList.DtGetData.Rows[j][0].ToString() == "")
                        {
                            sSql = "INSERT INTO dbo.ApartmentSize (ApartmentSizeFrom,ApartmentSizeTo) Values ('" + oPickList.DtGetData.Rows[j][1].ToString() + "','" + oPickList.DtGetData.Rows[j][2].ToString() + "')";
                            cmd = new SqlCommand(sSql, Con);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            sSql = "Update dbo.ApartmentSize SET ApartmentSizeFrom= '" + oPickList.DtGetData.Rows[j][1].ToString() + "',ApartmentSizeTo= '" + oPickList.DtGetData.Rows[j][2].ToString() + "' WHERE ApartmentSizeID='" + Convert.ToInt32(oPickList.DtGetData.Rows[j][0]) + "' ";
                            cmd = new SqlCommand(sSql, Con);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return i;
        }

        public DataTable GetIncome(SqlConnection Con)
        {
            DataTable dtIncome=null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT IncomeId as Id,IncomeFrom as FFrom,IncomeTo as FTo FROM dbo.Income", Con);
                dtIncome = new DataTable();
                sda.Fill(dtIncome);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtIncome;

        }
        public DataTable GetCostPreference(SqlConnection Con)
        {
            DataTable dtIncome=null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT CostPreferenceId as Id,CostPreferenceFrom as FFrom,CostPreferenceTo as FTo FROM dbo.CostPreference", Con);
                dtIncome = new DataTable();
                sda.Fill(dtIncome);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtIncome;

        }
        public DataTable GetApartmentSize(SqlConnection Con)
        {
            DataTable dtApartmentSize=null;
            SqlDataAdapter sda;

            try
            {
                sda = new SqlDataAdapter("SELECT ApartmentSizeId as Id,ApartmentSizeFrom as FFrom,ApartmentSizeTo as FTo FROM dbo.ApartmentSize", Con);
                dtApartmentSize = new DataTable();
                sda.Fill(dtApartmentSize);

            }
            catch (Exception ex)
            {
                BsfGlobal.CustomException(ex.Message, ex.StackTrace);
            }
            return dtApartmentSize;
        }
    }
}
