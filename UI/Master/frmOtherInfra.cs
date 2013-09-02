using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CRM.BusinessObjects;
using CRM.BusinessLayer;

namespace CRM
{
    public partial class frmOtherInfra : Form
    {
        #region Variables

        public decimal m_dNetAmount = 0; decimal m_dAmount = 0, m_dPercent = 0;
        string m_sType = "", m_sAmountType = ""; 
        int m_iOCId = 0; int m_iFlatTypeId = 0; int m_iFlatId = 0;
        List<OtherInfra> OtherAreaBO = new List<OtherInfra>();
        public DataTable dtRetn = new DataTable();
        DataTable dtlist = new DataTable();

        #endregion

        #region Constructor

        public frmOtherInfra()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            else
                base.OnSizeChanged(e);
        }

        #endregion

        #region Functions

        public void Execute(int argOtherCostId, int argFlatTypeId, int argFlatId, string argType)
        {
            m_iOCId = argOtherCostId;
            m_iFlatTypeId = argFlatTypeId;
            m_iFlatId = argFlatId;
            m_sType = argType;
            ShowDialog();
        }

        private void PopulateEditData()
        {
            DataTable dt = new DataTable();
            if (dtRetn != null)
            {
                if (dtRetn.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtRetn);
                    dv.RowFilter = "OtherCostId=" + m_iOCId + "";
                    dt = dv.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        if (CommFun.IsNullCheck(dt.Rows[0]["AmountType"], CommFun.datatypes.vartypestring).ToString() == "B")
                        { radioGroup1.SelectedIndex = 0; }
                        else
                        { radioGroup1.SelectedIndex = 1; }
                        txtPer.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Percent"], CommFun.datatypes.vartypenumeric));
                        txtNetAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                    }
                }
            }

            if (m_sType == "FlatType")
            {
                dt = OtherCostBL.GetFlatTypeOtherInfra(m_iFlatTypeId, m_iOCId);
                if (dt.Rows.Count > 0)
                {
                    if (CommFun.IsNullCheck(dt.Rows[0]["AmountType"], CommFun.datatypes.vartypestring).ToString() == "B")
                    { radioGroup1.SelectedIndex = 0; }
                    else
                    { radioGroup1.SelectedIndex = 1; }
                    txtPer.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Percent"], CommFun.datatypes.vartypenumeric));
                    txtNetAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                }
            }
            else if (m_sType == "Flat" || m_sType == "GlobalRate")
            {
                dt = OtherCostBL.GetFlatOtherInfra(m_iFlatId, m_iOCId);
                if (dt.Rows.Count > 0)
                {
                    if (CommFun.IsNullCheck(dt.Rows[0]["AmountType"], CommFun.datatypes.vartypestring).ToString() == "B")
                    { radioGroup1.SelectedIndex = 0; }
                    else
                    { radioGroup1.SelectedIndex = 1; }
                    txtPer.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Percent"], CommFun.datatypes.vartypenumeric));
                    txtNetAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                }
            }
        }
       
        private void BindColumn()
        {
            if (dtRetn != null)
            {
                if (dtRetn.Rows.Count == 0)
                {
                    dtRetn.Columns.Add("FlatTypeId", typeof(int));
                    dtRetn.Columns.Add("FlatId", typeof(int));
                    dtRetn.Columns.Add("OtherCostId", typeof(int));
                    dtRetn.Columns.Add("AmountType", typeof(string));
                    dtRetn.Columns.Add("Percent", typeof(decimal));
                    dtRetn.Columns.Add("Amount", typeof(decimal));
                }
            }
        }

        #endregion

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            if (m_sType == "FlatType")
            {
                OtherAreaBO.Add(new OtherInfra()
                {
                    OtherCostId = m_iOCId,
                    FlatTypeId = m_iFlatTypeId,
                    FlatId = 0,
                    AmountType = m_sAmountType,
                    Percent = Convert.ToDecimal(CommFun.IsNullCheck(txtPer.EditValue, CommFun.datatypes.vartypenumeric)),
                    Amount = Convert.ToDecimal(CommFun.IsNullCheck(txtNetAmt.EditValue, CommFun.datatypes.vartypenumeric))
                });
            }
            else if (m_sType == "Flat" || m_sType == "GlobalRate")
            {
                OtherAreaBO.Add(new OtherInfra()
                {
                    OtherCostId = m_iOCId,
                    FlatTypeId = 0,
                    FlatId = m_iFlatId,
                    AmountType = m_sAmountType,
                    Percent = Convert.ToDecimal(CommFun.IsNullCheck(txtPer.EditValue, CommFun.datatypes.vartypenumeric)),
                    Amount = Convert.ToDecimal(CommFun.IsNullCheck(txtNetAmt.EditValue, CommFun.datatypes.vartypenumeric)),
                });
            }

            dt = CommFun.GenericListToDataTable(OtherAreaBO);

            BindColumn();

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dtRetn.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dtRetn);
                            dv.RowFilter = "OtherCostId=" + dt.Rows[0]["OtherCostId"] + "";
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                for (int j = 0; j < dtRetn.Rows.Count; j++)
                                {
                                    if (Convert.ToInt32(dtRetn.Rows[j]["OtherCostId"]) == Convert.ToInt32(dv.ToTable().Rows[0]["OtherCostId"]))
                                    {
                                        dtRetn.Rows[j]["FlatTypeId"] = dt.Rows[i]["FlatTypeId"];
                                        dtRetn.Rows[j]["FlatId"] = dt.Rows[i]["FlatId"];
                                        dtRetn.Rows[j]["OtherCostId"] = dt.Rows[i]["OtherCostId"];
                                        dtRetn.Rows[j]["AmountType"] = dt.Rows[i]["AmountType"];
                                        dtRetn.Rows[j]["Percent"] = dt.Rows[i]["Percent"];
                                        dtRetn.Rows[j]["Amount"] = dt.Rows[i]["Amount"];
                                    }
                                }
                            }
                            else
                            {
                                DataRow dr = dtRetn.NewRow();
                                dt.AcceptChanges();
                                if (m_sType == "FlatType")
                                { dr["FlatTypeId"] = dt.Rows[i]["FlatTypeId"].ToString(); dr["FlatId"] = 0; }
                                else { dr["FlatTypeId"] = 0; dr["FlatId"] = dt.Rows[i]["FlatId"].ToString(); }
                                dr["OtherCostId"] = dt.Rows[i]["OtherCostId"].ToString();
                                dr["AmountType"] = dt.Rows[i]["AmountType"].ToString();
                                dr["Percent"] = dt.Rows[i]["Percent"].ToString();
                                dr["Amount"] = dt.Rows[i]["Amount"].ToString();

                                dtRetn.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            DataRow dr = dtRetn.NewRow();
                            dt.AcceptChanges();
                            if (m_sType == "FlatType")
                            { dr["FlatTypeId"] = dt.Rows[i]["FlatTypeId"].ToString(); dr["FlatId"] = 0; }
                            else { dr["FlatTypeId"] = 0; dr["FlatId"] = dt.Rows[i]["FlatId"].ToString(); }
                            dr["OtherCostId"] = dt.Rows[i]["OtherCostId"].ToString();
                            dr["AmountType"] = dt.Rows[i]["AmountType"].ToString();
                            dr["Percent"] = dt.Rows[i]["Percent"].ToString();
                            dr["Amount"] = dt.Rows[i]["Amount"].ToString();

                            dtRetn.Rows.Add(dr);
                        }
                    }
                }
            }

            m_dNetAmount = Convert.ToDecimal(CommFun.IsNullCheck(txtNetAmt.EditValue, CommFun.datatypes.vartypenumeric));
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void frmOtherInfra_Load(object sender, EventArgs e)
        {
            radioGroup1.SelectedIndex = 0;
            DataTable dt = new DataTable();
            dt = OtherCostBL.GetOtherInfraAmount(m_iFlatTypeId, m_iFlatId);
            if (dt.Rows.Count > 0)
            {
                if (radioGroup1.SelectedIndex == 0)
                { txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["BaseAmt"]); m_sAmountType = "B"; }
                else { txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["NetAmt"]); m_sAmountType = "N"; }
            }
            PopulateEditData();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = OtherCostBL.GetOtherInfraAmount(m_iFlatTypeId, m_iFlatId);
            if (dt.Rows.Count > 0)
            {
                if (radioGroup1.SelectedIndex == 0)
                { txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["BaseAmt"]); m_sAmountType = "B"; }
                else { txtAmt.EditValue = Convert.ToDecimal(dt.Rows[0]["NetAmt"]); m_sAmountType = "N"; }
                txtNetAmt.EditValue = decimal.Round(Convert.ToDecimal(txtAmt.EditValue) * Convert.ToDecimal(txtPer.EditValue) / 100, 0);
            }
        }

        private void txtPer_EditValueChanged(object sender, EventArgs e)
        {
            txtNetAmt.EditValue = decimal.Round(Convert.ToDecimal(txtAmt.EditValue) * Convert.ToDecimal(txtPer.EditValue) / 100, 0);
        }

    }
}
