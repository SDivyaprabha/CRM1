using System;
using System.Data;
using System.Data.SqlClient;

namespace CRM.DataLayer
{
    class AdditionalItemDL
    {
        #region Methods

        public static DataTable GetExtraItemList(string argItemTranId,string argType,int argCCId,int argFlatTypeId)
        {
            BsfGlobal.OpenCRMDB();
            DataTable dt = null;
            try
            {
                String sSql = "";
                if (argType == "Project")
                {
                    sSql = "Select A.ExtraItemId,A.ItemCode,A.ItemDescription,B.ExtraItemTypeName,C.Unit_Name Unit, " +
                            " A.ExtraRate Rate,Convert(bit,0,0) Sel from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster A " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster B ON A.ExtraItemTypeId=B.ExtraItemTypeId " +
                            " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM C ON A.UnitId=C.Unit_Id " +
                            " Where 1=1";

                }
                else if (argType == "FlatType")
                {
                    sSql = " Select A.ExtraItemId,A.ItemCode,A.ItemDescription,C.ExtraItemTypeName," +
                            " B.Unit_Name Unit,A.RAQty Qty,A.Rate,A.Value Amount,A.Value NetAmount,Convert(bit,0,0) Sel " +
                            " from [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster A "+
                            " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM B ON A.UnitID=B.Unit_ID " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster C ON A.ExtraItemTypeId=C.ExtraItemTypeId " +
                            " Where A.CostCentreId= " + argCCId + " AND A.Approve='Y'";

                }
                else if (argType == "Flat")
                {
                    //sSql = "Select A.ExtraItemId,B.ItemCode,B.ItemDescription,C.ExtraItemTypeName,A.Qty,A.ExtraRate Rate,D.Unit_Name Unit," +
                    //        " str(A.Qty) + ' ' + D.Unit_Name WorkingQty,A.Amount,A.NetAmount,Convert(bit,0,0) Sel From dbo.FlatTypeExtraItem A" +
                    //        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster B ON A.ExtraItemId=B.ExtraItemId " +
                    //        " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster C ON C.ExtraItemTypeId=B.ExtraItemTypeId" +
                    //        " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM D ON D.Unit_ID=B.UnitId " +
                    //        " Where FlatTypeId= " + argFlatTypeId;

                    sSql = "Select A.ExtraItemId,B.ItemCode,B.ItemDescription,C.ExtraItemTypeName,A.ExtraRate Rate,D.Unit_Name Unit," +
                            " A.Qty,A.Amount,A.NetAmount,Convert(bit,0,0) Sel From dbo.FlatTypeExtraItem A" +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemMaster B ON A.ExtraItemId=B.ExtraItemId " +
                            " INNER JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.ExtraItemTypeMaster C ON C.ExtraItemTypeId=B.ExtraItemTypeId" +
                            " LEFT JOIN [" + BsfGlobal.g_sRateAnalDBName + "].dbo.UOM D ON D.Unit_ID=B.UnitId " +
                            " Where FlatTypeId= " + argFlatTypeId;
                }
                if (argItemTranId != "")
                {
                    sSql = sSql + " AND A.ExtraItemId NOT IN(" + argItemTranId + ")";
                }
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

        #endregion
    }
}
