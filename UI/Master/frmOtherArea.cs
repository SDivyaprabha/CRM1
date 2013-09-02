using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using CRM.BusinessObjects;

namespace CRM
{
    public partial class frmOtherArea : DevExpress.XtraEditors.XtraForm
    {

       #region Variables

        //decimal m_dArea = 0;
        //decimal m_dRate = 0;
        public decimal m_dAmount = 0;
        //int m_iUnit = 0;
        string m_sType="";
        int m_iOCId = 0; int m_iFlatTypeId = 0; int m_iFlatId = 0;
        List<OtherArea> OtherAreaBO = new List<OtherArea>();
        public DataTable dtRetn = new DataTable();
        DataTable dtlist = new DataTable();
        #endregion

        #region Constructor

        public frmOtherArea()
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

        #region Form Event

        private void frmOtherArea_Load(object sender, EventArgs e)
        {
            FillUOM();
            EditData();
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

        private void FillUOM()
        {
            DataTable dt = new DataTable();
            dt = UnitDirBL.UOM();
            cboUnit.Properties.DataSource = dt;
            cboUnit.Properties.PopulateColumns();
            cboUnit.Properties.DisplayMember = "Unit_Name";
            cboUnit.Properties.ValueMember = "Unit_ID";
            cboUnit.Properties.Columns["Unit_ID"].Visible = false;
            cboUnit.Properties.ShowFooter = false;
            cboUnit.Properties.ShowHeader = false;
        }

        private void EditData()
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
                        txtArea.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Area"], CommFun.datatypes.vartypenumeric));
                        txtRate.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric));
                        cboUnit.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Unit"], CommFun.datatypes.vartypenumeric));
                        txtAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                    }
                }
            }

            if (m_sType == "FlatType")
            {
                dt = OtherCostBL.GetFlatTypeOtherArea(m_iFlatTypeId, m_iOCId);
                if (dt.Rows.Count > 0)
                {
                    txtArea.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Area"], CommFun.datatypes.vartypenumeric));
                    txtRate.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric));
                    cboUnit.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Unit"], CommFun.datatypes.vartypenumeric));
                    txtAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
                }
            }
            else if (m_sType == "Flat" || m_sType == "GlobalRate")
            {
                dt = OtherCostBL.GetFlatOtherArea(m_iFlatId, m_iOCId);
                if (dt.Rows.Count > 0)
                {
                    txtArea.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Area"], CommFun.datatypes.vartypenumeric));
                    txtRate.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Rate"], CommFun.datatypes.vartypenumeric));
                    cboUnit.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dt.Rows[0]["Unit"], CommFun.datatypes.vartypenumeric));
                    txtAmt.EditValue = Convert.ToDecimal(CommFun.IsNullCheck(dt.Rows[0]["Amount"], CommFun.datatypes.vartypenumeric));
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
                    dtRetn.Columns.Add("Area", typeof(decimal));
                    dtRetn.Columns.Add("Rate", typeof(decimal));
                    dtRetn.Columns.Add("Unit", typeof(int));
                    dtRetn.Columns.Add("Amount", typeof(decimal));

                    //for (int m = 0; m < dtRetn.Rows.Count; m++)
                    //{
                    //    DataRow dr = dtlist.NewRow();
                    //    dtRetn.AcceptChanges();
                    //    dr["FlatTypeId"] = dtRetn.Rows[m]["FlatTypeId"].ToString();
                    //    dr["FlatId"] = dtRetn.Rows[m]["FlatId"].ToString();
                    //    dr["OtherCostId"] = dtRetn.Rows[m]["OtherCostId"].ToString();
                    //    dr["Area"] = dtRetn.Rows[m]["Area"].ToString();
                    //    dr["Rate"] = dtRetn.Rows[m]["Rate"].ToString();
                    //    dr["Unit"] = dtRetn.Rows[m]["Unit"].ToString();
                    //    dr["Amount"] = dtRetn.Rows[m]["Amount"].ToString();

                    //    dtlist.Rows.Add(dr);

                    //}
                }
                //else
                //{
                //    dtRetn = new DataTable();
                //    dtRetn.Columns.Add("FlatTypeId");
                //    dtRetn.Columns.Add("FlatId");
                //    dtRetn.Columns.Add("OtherCostId");
                //    dtRetn.Columns.Add("Area");
                //    dtRetn.Columns.Add("Rate");
                //    dtRetn.Columns.Add("Unit");
                //    dtRetn.Columns.Add("Amount");
                //}
            }
        }
        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataTable dt = new DataTable();
            if (m_sType == "FlatType" )
            {
                OtherAreaBO.Add(new OtherArea()
                {
                    OtherCostId = m_iOCId,
                    FlatTypeId = m_iFlatTypeId,
                    FlatId = 0,
                    Area = Convert.ToDecimal(CommFun.IsNullCheck(txtArea.EditValue, CommFun.datatypes.vartypenumeric)),
                    Rate = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric)),
                    Unit = Convert.ToInt32(CommFun.IsNullCheck(cboUnit.EditValue, CommFun.datatypes.vartypenumeric)),
                    Amount = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.EditValue, CommFun.datatypes.vartypenumeric))
                });
            }
            else if (m_sType == "Flat" || m_sType == "GlobalRate")
            {
                OtherAreaBO.Add(new OtherArea()
                {
                    OtherCostId = m_iOCId,
                    FlatTypeId = 0,
                    FlatId = m_iFlatId,
                    Area = Convert.ToDecimal(CommFun.IsNullCheck(txtArea.EditValue, CommFun.datatypes.vartypenumeric)),
                    Rate = Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric)),
                    Unit = Convert.ToInt32(CommFun.IsNullCheck(cboUnit.EditValue, CommFun.datatypes.vartypenumeric)),
                    Amount = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.EditValue, CommFun.datatypes.vartypenumeric)),
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
                                        dtRetn.Rows[j]["Area"] = dt.Rows[i]["Area"];
                                        dtRetn.Rows[j]["Rate"] = dt.Rows[i]["Rate"];
                                        dtRetn.Rows[j]["Unit"] = dt.Rows[i]["Unit"];
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
                                dr["Area"] = dt.Rows[i]["Area"].ToString();
                                dr["Unit"] = dt.Rows[i]["Unit"].ToString();
                                dr["Rate"] = dt.Rows[i]["Rate"].ToString();
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
                            dr["Area"] = dt.Rows[i]["Area"].ToString();
                            dr["Unit"] = dt.Rows[i]["Unit"].ToString();
                            dr["Rate"] = dt.Rows[i]["Rate"].ToString();
                            dr["Amount"] = dt.Rows[i]["Amount"].ToString();

                            dtRetn.Rows.Add(dr);
                        }
                    }
                }
            }
            
            m_dAmount = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.EditValue, CommFun.datatypes.vartypenumeric));
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_dAmount = Convert.ToDecimal(CommFun.IsNullCheck(txtAmt.EditValue, CommFun.datatypes.vartypenumeric));
            Close();
        }

        private void txtArea_EditValueChanged(object sender, EventArgs e)
        {
            txtAmt.EditValue = decimal.Round(Convert.ToDecimal(CommFun.IsNullCheck(txtArea.EditValue, CommFun.datatypes.vartypenumeric)) * Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric))).ToString();
        }

        private void txtRate_EditValueChanged(object sender, EventArgs e)
        {
           txtAmt.EditValue = decimal.Round(Convert.ToDecimal(CommFun.IsNullCheck(txtArea.EditValue, CommFun.datatypes.vartypenumeric)) * Convert.ToDecimal(CommFun.IsNullCheck(txtRate.EditValue, CommFun.datatypes.vartypenumeric))).ToString();
        }

        #endregion
    }
}
