using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CRM
{
    public partial class frmFilter : DevExpress.XtraEditors.XtraForm
    {
        #region Variables

        string m_sFilter = "";
        public DataTable m_dtFilter = new DataTable();
        FilterBO m_filterBO = new FilterBO();
        DataSet ds = new DataSet();
        DateTime m_deFrom, m_deTo;
        List<FilterBO> oFilterBO = new List<FilterBO>();

        #endregion

        #region Constructor

        public frmFilter()
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

        #region Form Load

        private void frmFilter_Load(object sender, EventArgs e)
        {
            FillEnquiry();

            if (m_dtFilter.Rows.Count > 0)
            {
                if (m_dtFilter.Rows[0]["i_AptId"].ToString() != "")
                {
                    chkApt.Checked = true;
                    RGApt.SetEditValue(m_dtFilter.Rows[0]["i_AptId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_AptSizeId"].ToString() != "")
                {
                    chkAptSize.Checked = true;
                    RGAptSize.SetEditValue(m_dtFilter.Rows[0]["i_AptSizeId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_AptTypeId"].ToString() != "")
                {
                    chkAptType.Checked = true;
                    RGAptType.SetEditValue(m_dtFilter.Rows[0]["i_AptTypeId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_AreaId"].ToString() != "")
                {
                    chkArea.Checked = true;
                    RGArea.SetEditValue(m_dtFilter.Rows[0]["i_AreaId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_BuyerId"].ToString() != "")
                {
                    chkBuyer.Checked = true;
                    RGBuyer.SetEditValue(m_dtFilter.Rows[0]["i_BuyerId"].ToString());
                }
                if (m_dtFilter.Rows[0]["s_City"].ToString() != "")
                {
                    chkCity.Checked = true;
                    RGCity.SetEditValue(m_dtFilter.Rows[0]["s_City"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_CostPrefId"].ToString() != "")
                {
                    chkCostPre.Checked = true;
                    RGCostPre.SetEditValue(m_dtFilter.Rows[0]["i_CostPrefId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_CountryId"].ToString() != "")
                {
                    chkCountry.Checked = true;
                    RGCountry.SetEditValue(m_dtFilter.Rows[0]["i_CountryId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_EmpId"].ToString() != "")
                {
                    chkEmp.Checked = true;
                    RGEmpStatus.SetEditValue(m_dtFilter.Rows[0]["i_EmpId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_ExeId"].ToString() != "")
                {
                    chkExe.Checked = true;
                    RGExe.SetEditValue(m_dtFilter.Rows[0]["i_ExeId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_FacId"].ToString() != "")
                {
                    chkFac.Checked = true;
                    RGFac.SetEditValue(m_dtFilter.Rows[0]["i_FacId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_GuestHouId"].ToString() != "")
                {
                    chkGuestHou.Checked = true;
                    RGGuestHou.SetEditValue(m_dtFilter.Rows[0]["i_GuestHouId"].ToString());
                }
                if (m_dtFilter.Rows[0]["s_Nat"].ToString() != "")
                {
                    chkNat.Checked = true;
                    RGNationality.EditValue = m_dtFilter.Rows[0]["s_Nat"].ToString();
                }
                if (m_dtFilter.Rows[0]["i_PossId"].ToString() != "")
                {
                    chkPoss.Checked = true;
                    RGPoss.SetEditValue(m_dtFilter.Rows[0]["i_PossId"].ToString());
                }
                if (m_dtFilter.Rows[0]["s_Prof"].ToString() != "")
                {
                    chkProfession.Checked = true;
                    RGProfession.EditValue = m_dtFilter.Rows[0]["s_Prof"].ToString();
                }
                if (m_dtFilter.Rows[0]["i_ProjId"].ToString() != "")
                {
                    chkProj.Checked = true;
                    RGProj.SetEditValue(m_dtFilter.Rows[0]["i_ProjId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_ReligionId"].ToString() != "")
                {
                    chkReligion.Checked = true;
                    RGReligion.SetEditValue(m_dtFilter.Rows[0]["i_ReligionId"].ToString());
                }
                if (m_dtFilter.Rows[0]["s_State"].ToString() != "")
                {
                    chkState.Checked = true;
                    RGState.SetEditValue(m_dtFilter.Rows[0]["s_State"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_StayId"].ToString() != "")
                {
                    chkStay.Checked = true;
                    RGStay.SetEditValue(m_dtFilter.Rows[0]["i_StayId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_SubSourId"].ToString() != "")
                {
                    chkSubSou.Checked = true;
                    RGSubSou.SetEditValue(m_dtFilter.Rows[0]["i_SubSourId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_IncomeId"].ToString() != "")
                {
                    chkYearlyInc.Checked = true;
                    RGYearlyIncome.SetEditValue(m_dtFilter.Rows[0]["i_IncomeId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_FinId"].ToString() != "")
                {
                    chkFin.Checked = true;
                    RGFinance.SetEditValue(m_dtFilter.Rows[0]["i_FinId"].ToString());
                }
                if (m_dtFilter.Rows[0]["i_MaritalId"].ToString() != "")
                {
                    chkMaritalS.Checked = true;
                    RGMaritalSta.SetEditValue(m_dtFilter.Rows[0]["i_MaritalId"].ToString());
                }
            }
        }

        #endregion

        #region Button Event

        private void btnOk_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string[] splitF = new string[] { };
            string GetSplitted = "";

            //m_sFilter = "Select LeadId, LeadName,0 CostCentreId from LeadRegister Where 1=1 ";
            m_sFilter = " Select Distinct R.LeadId, R.LeadName,R.LeadDate,A.CostCentreId,R.Mobile,R.Email,IsNull(O.CostCentreName,'')CostCentre," +
                        " IsNull(U.EmployeeName,'') ExecutiveName,R.CallTypeId From dbo.LeadRegister R Left Join dbo.LeadProjectInfo A On A.LeadId=R.LeadId " +
                        " Inner Join dbo.LeadExecutiveInfo E On A.LeadId=E.LeadId  Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.OperationalCostCentre O On O.CostCentreId=A.CostCentreId  " +
                        " Left Join [" + BsfGlobal.g_sWorkFlowDBName + "].dbo.Users U On U.UserId=A.ExecutiveId  " +
                        " Where R.LeadDate Between '" + m_deFrom.ToString("dd-MMM-yyyy") + "' And '" + m_deTo.ToString("dd-MMM-yyyy") + "' ";

            if (chkProj.Checked == true)
            {
                if (CommFun.IsNullCheck(RGProj.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                   
                    //m_sFilter = m_sFilter + " AND LeadId in( Select LeadId from LeadExecutiveInfo where CostCentreId in( " +
                    //            RGProj.EditValue.ToString() + ")) ";
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId From LeadProjectInfo Where CostCentreId in( " +
                        RGProj.EditValue.ToString() + "))";
                }
            }

            if (chkProfession.Checked == true)
            {
                if (CommFun.IsNullCheck(RGProfession.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    splitF = RGProfession.Text.Split(',');
                    for (int i = 0; i <= splitF.Length - 1; i++)
                        GetSplitted += "'" + splitF[i].ToString().Trim() + "' , ";
                    GetSplitted = GetSplitted.Substring(0, GetSplitted.Length - 1);
                    if (GetSplitted != string.Empty) GetSplitted = GetSplitted.TrimEnd(',');

                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadPersonalInfo where Profession in( " +
                        GetSplitted.ToString() + "))" + " OR LeadId in( Select LeadId from LeadCoApplicantInfo where Profession in( " +
                        GetSplitted.ToString() + ")) ";
                }
            }

            if (chkBuyer.Checked == true)
            {
                if (CommFun.IsNullCheck(RGBuyer.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    if (Convert.ToInt32(RGBuyer.EditValue) == 0)
                    {
                        m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId From dbo.LeadRegister A " +
                          "Where LeadId Not in (Select LeadId from dbo.BuyerDetail)) ";
                    }
                    //m_sFilter = m_sFilter + " AND LeadId in( Select LeadId from LeadFlatInfo where Buyer in( " +
                    //    RGBuyer.EditValue.ToString() + ")) ";
                   
                }
            }

            if (chkExe.Checked == true)
            {
                if (CommFun.IsNullCheck(RGExe.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadExecutiveInfo where ExecutiveId in( " +
                        RGExe.EditValue.ToString() + ")) ";
                }
            }

            if (chkReligion.Checked == true)
            {
                if (CommFun.IsNullCheck(RGReligion.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadPersonalInfo where Religion in( " +
                         RGReligion.EditValue.ToString() + "))" + " OR LeadId in( Select LeadId from LeadCoApplicantInfo where Religion in( " +
                         RGReligion.EditValue.ToString() + ")) ";
                }
            }

            if (chkStay.Checked == true)
            {
                if (CommFun.IsNullCheck(RGStay.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadFinance where Stay in( " + RGStay.EditValue.ToString() + ")) ";
                }
            }

            if (chkSubSou.Checked == true)
            {
                if (CommFun.IsNullCheck(RGSubSou.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadProjectInfo where CampaignId in( " + RGSubSou.EditValue.ToString() + ")) ";
                }
            }

            if (chkYearlyInc.Checked == true)
            {
                if (CommFun.IsNullCheck(RGYearlyIncome.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadFinance where Income in( " + RGYearlyIncome.EditValue.ToString() + ")) ";
                }
            }

            if (chkEmp.Checked == true)
            {
                if (CommFun.IsNullCheck(RGEmpStatus.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadFinance where Employment in( " +
                        RGEmpStatus.EditValue.ToString() + ")) ";
                }
            }

            if (chkNat.Checked == true)
            {
                if (CommFun.IsNullCheck(RGNationality.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    splitF = null;
                    GetSplitted = null;
                    splitF = RGNationality.Text.Split(',');
                    for (int i = 0; i <= splitF.Length - 1; i++)
                        GetSplitted += "'" + splitF[i].ToString().Trim() + "' , ";
                    GetSplitted = GetSplitted.Substring(0, GetSplitted.Length - 1);
                    if (GetSplitted != string.Empty) GetSplitted = GetSplitted.TrimEnd(',');

                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadPersonalInfo where Nationality in( " +
                        GetSplitted.ToString() + "))" + " OR LeadId in( Select LeadId from LeadCoApplicantInfo where Nationality in( " +
                        GetSplitted.ToString() + ")) ";
                }
            }

            if (chkCountry.Checked == true)
            {
                if (CommFun.IsNullCheck(RGCountry.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in(Select LeadId from LeadCommAddressInfo where Country in(" + RGCountry.EditValue.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadCoAppAddressInfo where CoCountry in( " + RGCountry.EditValue.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadNRIAddressInfo where Country in( " + RGCountry.EditValue.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadOffAddressInfo where Country in( " + RGCountry.EditValue.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadPermanentAddressInfo where Country in( " + RGCountry.EditValue.ToString() + ")) ";
                }
            }

            if (chkState.Checked == true)
            {
                if (CommFun.IsNullCheck(RGState.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    splitF = null;
                    splitF = RGState.EditValue.ToString().Split(',');

                    GetSplitted = null;
                    for (int i = 0; i <= splitF.Length - 1; i++)
                    {
                        GetSplitted += "'" + splitF[i].ToString().Trim() + "' , ";
                    }
                    GetSplitted = GetSplitted.Substring(0, GetSplitted.Length - 1);
                    if (GetSplitted != string.Empty) { GetSplitted = GetSplitted.TrimEnd(','); }

                    m_sFilter = m_sFilter + " AND R.LeadId in(Select LeadId from LeadCommAddressInfo where StateId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadCoAppAddressInfo where StateId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadNRIAddressInfo where StateId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadOffAddressInfo where StateId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadPermanentAddressInfo where StateId in( " + GetSplitted.ToString() + ")) ";
                }
            }

            if (chkCity.Checked == true)
            {
                if (CommFun.IsNullCheck(RGCity.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    splitF = null;                    
                    splitF = RGCity.EditValue.ToString().Split(',');

                    GetSplitted = null;
                    for (int i = 0; i <= splitF.Length - 1; i++) { GetSplitted += "'" + splitF[i].ToString().Trim() + "' , "; }
                    GetSplitted = GetSplitted.Substring(0, GetSplitted.Length - 1);
                    if (GetSplitted != string.Empty) GetSplitted = GetSplitted.TrimEnd(',');

                    m_sFilter = m_sFilter + " AND R.LeadId in(Select LeadId from LeadCommAddressInfo where CityId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadCoAppAddressInfo where CityId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadNRIAddressInfo where CityId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadOffAddressInfo where CityId in( " + GetSplitted.ToString() + "))" +
                                            " OR R.LeadId in(Select LeadId from LeadPermanentAddressInfo where CityId in( " + GetSplitted.ToString() + ")) ";
                }
            }

            if (chkApt.Checked == true)
            {
                if (CommFun.IsNullCheck(RGApt.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadApartmentInfo where ApartmentId in( " +
                        RGApt.EditValue.ToString() + ")) ";
                }
            }

            if (chkAptSize.Checked == true)
            {
                if (CommFun.IsNullCheck(RGAptSize.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadApartmentInfo where Size in( " +
                        RGAptSize.EditValue.ToString() + ")) ";
                }
            }

            if (chkAptType.Checked == true)
            {
                if (CommFun.IsNullCheck(RGAptType.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadApartmentInfo where TypeId in( " +
                        RGAptType.EditValue.ToString() + ")) ";
                }
            }

            if (chkArea.Checked == true)
            {
                if (CommFun.IsNullCheck(RGArea.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from Area Where Sel=1 And AreaId in( " + RGArea.EditValue.ToString() + ")) ";
                }
            }

            if (chkCostPre.Checked == true)
            {
                if (CommFun.IsNullCheck(RGCostPre.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadApartmentInfo where CostId in( " + RGCostPre.EditValue.ToString() + ")) ";
                }
            }

            if (chkFac.Checked == true)
            {
                if (CommFun.IsNullCheck(RGFac.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from Facility where FacilityId in( " + RGFac.EditValue.ToString() + ")) ";
                }
            }

            if (chkPoss.Checked == true)
            {
                if (CommFun.IsNullCheck(RGPoss.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from Possess where PossessId in( " + RGPoss.EditValue.ToString() + ")) ";
                }
            }

            if (chkFin.Checked == true)
            {
                if (CommFun.IsNullCheck(RGFinance.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    splitF = null;
                    GetSplitted = null;
                    splitF = RGFinance.Text.Split(',');
                    for (int i = 0; i <= splitF.Length - 1; i++)
                        GetSplitted += "'" + splitF[i].ToString().Trim() + "' , ";
                    GetSplitted = GetSplitted.Substring(0, GetSplitted.Length - 1);
                    if (GetSplitted != string.Empty) GetSplitted = GetSplitted.TrimEnd(',');

                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadBankDetails where Need in( " +
                        GetSplitted.ToString() + ")) ";
                }
            }

            if (chkGuestHou.Checked == true)
            {
                if (CommFun.IsNullCheck(RGGuestHou.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadFinance where GuestHouse in( " +
                        RGGuestHou.EditValue.ToString() + ")) ";
                }
            }

            if (chkMaritalS.Checked == true)
            {
                if (CommFun.IsNullCheck(RGMaritalSta.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadPersonalInfo where MarritalStatus in( " +
                        RGMaritalSta.EditValue.ToString() + ")) ";
                }
            }

            if (chkAge.Checked == true)
            {
                if (CommFun.IsNullCheck(txtFrom.EditValue, CommFun.datatypes.vartypestring).ToString() != "" && CommFun.IsNullCheck(txtTo.EditValue, CommFun.datatypes.vartypestring).ToString() != "")
                {
                    m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadPersonalInfo where DATEDIFF(YEAR,DOB,GETDATE()) " +
                        " Between " + txtFrom.EditValue + " And " + txtTo.EditValue + " )";
                }
            }

            if (chkNRI.Checked == true)
            {
                m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadPersonalInfo Where NRI In(0))";
            }

            if (chkVIP.Checked == true)
            {
                m_sFilter = m_sFilter + " AND R.LeadId in( Select LeadId from LeadRegister Where VIP In(1))";
            }
            if (chkLocality.Checked == true)
            {
                splitF = null;
                GetSplitted = null;
                splitF = RGLocality.Text.Split(',');
                for (int i = 0; i <= splitF.Length - 1; i++)
                    GetSplitted += "'" + splitF[i].ToString().Trim() + "',";
                GetSplitted = GetSplitted.Substring(0, GetSplitted.Length - 1);
                if (GetSplitted != string.Empty) GetSplitted = GetSplitted.TrimEnd(',');

                m_sFilter = m_sFilter + " AND R.LeadId in(Select LeadId From LeadCommAddressInfo Where Locality In(" + GetSplitted + "))";
            }

            m_sFilter = m_sFilter + "Order By LeadName";

            m_filterBO.i_AptId = CommFun.IsNullCheck(RGApt.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_AptSizeId = CommFun.IsNullCheck(RGAptSize.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_AptTypeId = CommFun.IsNullCheck(RGAptType.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_AreaId = CommFun.IsNullCheck(RGArea.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_BuyerId = CommFun.IsNullCheck(RGBuyer.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.s_City = CommFun.IsNullCheck(RGCity.EditValue.ToString(), CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_CostPrefId = CommFun.IsNullCheck(RGCostPre.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_CountryId = CommFun.IsNullCheck(RGCountry.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_EmpId = CommFun.IsNullCheck(RGEmpStatus.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_ExeId = CommFun.IsNullCheck(RGExe.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_FacId = CommFun.IsNullCheck(RGFac.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_GuestHouId = CommFun.IsNullCheck(RGGuestHou.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.s_Nat = CommFun.IsNullCheck(RGNationality.Text, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_PossId = CommFun.IsNullCheck(RGPoss.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.s_Prof = CommFun.IsNullCheck(RGProfession.Text, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_ProjId = CommFun.IsNullCheck(RGProj.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_ReligionId = CommFun.IsNullCheck(RGReligion.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.s_State = CommFun.IsNullCheck(RGState.EditValue.ToString(), CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_StayId = CommFun.IsNullCheck(RGStay.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_SubSourId = CommFun.IsNullCheck(RGSubSou.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_IncomeId = CommFun.IsNullCheck(RGYearlyIncome.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_FinId = CommFun.IsNullCheck(RGFinance.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.i_MaritalId = CommFun.IsNullCheck(RGMaritalSta.EditValue, CommFun.datatypes.vartypestring).ToString();
            m_filterBO.s_Locality = CommFun.IsNullCheck(RGLocality.EditValue, CommFun.datatypes.vartypestring).ToString();

            oFilterBO.Add(m_filterBO);

            m_dtFilter = CommFun.GenericListToDataTable(oFilterBO);

            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            m_sFilter = "";            
            Close();
        }

        #endregion

        #region Functions
        
        public DataSet DS
        {
            get { return ds; }
            set { ds = value; }
        }

        private void Clear()
        {
            chkApt.Checked = false;
            chkAptSize.Checked = false;
            chkAptType.Checked = false;
            chkArea.Checked = false;
            chkBuyer.Checked = false;
            chkCity.Checked = false;
            chkCostPre.Checked = false;
            chkCountry.Checked = false;
            chkEmp.Checked = false;
            chkExe.Checked = false;
            chkFac.Checked = false;
            chkFin.Checked = false;
            chkGuestHou.Checked = false;
            chkMaritalS.Checked = false;
            chkNat.Checked = false;
            chkPoss.Checked = false;
            chkProfession.Checked = false;
            chkProj.Checked = false;
            chkReligion.Checked = false;
            chkState.Checked = false;
            chkStay.Checked = false;
            chkSubSou.Checked = false;
            chkYearlyInc.Checked = false;

            RGApt.EditValue = null;
            RGAptSize.EditValue = null;
            RGAptType.EditValue = null;
            RGArea.EditValue = null;
            RGBuyer.EditValue = null;
            RGCity.EditValue = null;
            RGCostPre.EditValue = null;
            RGCountry.EditValue = null;
            RGEmpStatus.EditValue = null;
            RGExe.EditValue = null;
            RGFac.EditValue = null;
            RGGuestHou.EditValue = null;
            RGNationality.EditValue = null;
            RGPoss.EditValue = null;
            RGProfession.EditValue = null;
            RGProj.EditValue = null;
            RGReligion.EditValue = null;
            RGState.EditValue = null;
            RGStay.EditValue = null;
            RGSubSou.EditValue = null;
            RGYearlyIncome.EditValue = null;
            RGMaritalSta.EditValue = null;
            RGFinance.EditValue = null;
        }

        public string Execute(DateTime argFrom,DateTime argTo)
        {
            m_deFrom = argFrom;
            m_deTo = argTo;
            ShowDialog();

            return m_sFilter;
        }

        public void Exe(DataTable dt,DateTime argFrom,DateTime argTo)
        {
            m_dtFilter = dt;
            m_deFrom = argFrom;
            m_deTo = argTo;
        }

        public DataSet GetData()
        {
            BsfGlobal.OpenCRMDB();
            SqlDataAdapter sd = new SqlDataAdapter("EnquiryFilter", BsfGlobal.g_CRMDB);
            sd.SelectCommand.CommandType = CommandType.StoredProcedure;
            sd.SelectCommand.Parameters.Clear();
            sd.SelectCommand.Parameters.AddWithValue("@WorkFlowDB", BsfGlobal.g_sWorkFlowDBName);
            ds = new DataSet();
            sd.Fill(ds);
            sd.Dispose();
            BsfGlobal.g_CRMDB.Close();

            return ds;
        }

        private void FillEnquiry()
        {
            GetData();

            RGExe.Properties.DataSource = ds.Tables[0];
            RGExe.Properties.DisplayMember = "Description";
            RGExe.Properties.ValueMember = "ID";

            RGProj.Properties.DataSource = ds.Tables[1];
            RGProj.Properties.DisplayMember = "Description";
            RGProj.Properties.ValueMember = "ID";

            RGEmpStatus.Properties.DataSource = ds.Tables[2];
            RGEmpStatus.Properties.DisplayMember = "Description";
            RGEmpStatus.Properties.ValueMember = "ID";

            RGYearlyIncome.Properties.DataSource = ds.Tables[3];
            RGYearlyIncome.Properties.DisplayMember = "Description";
            RGYearlyIncome.Properties.ValueMember = "ID";

            RGApt.Properties.DataSource = ds.Tables[4];
            RGApt.Properties.DisplayMember = "Description";
            RGApt.Properties.ValueMember = "ID";

            RGAptSize.Properties.DataSource = ds.Tables[5];
            RGAptSize.Properties.DisplayMember = "Description";
            RGAptSize.Properties.ValueMember = "ID";

            RGAptType.Properties.DataSource = ds.Tables[6];
            RGAptType.Properties.DisplayMember = "Description";
            RGAptType.Properties.ValueMember = "ID";

            RGStay.Properties.DataSource = ds.Tables[7];
            RGStay.Properties.DisplayMember = "Description";
            RGStay.Properties.ValueMember = "ID";

            RGGuestHou.Properties.DataSource = ds.Tables[8];
            RGGuestHou.Properties.DisplayMember = "Description";
            RGGuestHou.Properties.ValueMember = "ID";

            RGCostPre.Properties.DataSource = ds.Tables[9];
            RGCostPre.Properties.DisplayMember = "Description";
            RGCostPre.Properties.ValueMember = "ID";

            RGSubSou.Properties.DataSource = ds.Tables[11];
            RGSubSou.Properties.DisplayMember = "Description";
            RGSubSou.Properties.ValueMember = "ID";

            RGFac.Properties.DataSource = ds.Tables[12];
            RGFac.Properties.DisplayMember = "Description";
            RGFac.Properties.ValueMember = "ID";

            RGArea.Properties.DataSource = ds.Tables[13];
            RGArea.Properties.DisplayMember = "Description";
            RGArea.Properties.ValueMember = "ID";

            RGPoss.Properties.DataSource = ds.Tables[14];
            RGPoss.Properties.DisplayMember = "Description";
            RGPoss.Properties.ValueMember = "ID";

            RGReligion.Properties.DataSource = ds.Tables[15];
            RGReligion.Properties.DisplayMember = "Description";
            RGReligion.Properties.ValueMember = "ID";

            RGNationality.Properties.DataSource = ds.Tables[16];
            RGNationality.Properties.DisplayMember = "Description";

            RGCity.Properties.DataSource = ds.Tables[17];
            RGCity.Properties.DisplayMember = "Description";
            RGCity.Properties.ValueMember = "ID";

            RGState.Properties.DataSource = ds.Tables[18];
            RGState.Properties.DisplayMember = "Description";
            RGState.Properties.ValueMember = "ID";

            RGCountry.Properties.DataSource = ds.Tables[19];
            RGCountry.Properties.DisplayMember = "Description";
            RGCountry.Properties.ValueMember = "ID";

            if ((ds.Tables.Count-1) > 19)
            {
                RGProfession.Properties.DataSource = ds.Tables[20];
                RGProfession.Properties.DisplayMember = "Description";
            }

            //RGBuyer.Properties.DataSource = ds.Tables[21];
            //RGBuyer.Properties.DisplayMember = "Description";
            //RGBuyer.Properties.ValueMember = "ID";
            if ((ds.Tables.Count - 1) > 21)
            {
                RGLocality.Properties.DataSource = ds.Tables[22];
                RGLocality.Properties.DisplayMember = "Description";
            }

        }

        #endregion

        #region Check Edit Events

        private void chkExe_CheckedChanged(object sender, EventArgs e)
        {
            RGExe.SetEditValue(null);
            if (chkExe.Checked == true) RGExe.Enabled = true;
            else
                RGExe.Enabled = false;
        }

        private void chkProfession_CheckedChanged(object sender, EventArgs e)
        {
            RGProfession.SetEditValue(null);
            if (chkProfession.Checked == true) RGProfession.Enabled = true;
            else
                RGProfession.Enabled = false;
        }

        private void chkBuyer_CheckedChanged(object sender, EventArgs e)
        {
            RGBuyer.SetEditValue(null);
            if (chkBuyer.Checked == true) RGBuyer.Enabled = true;
            else
                RGBuyer.Enabled = false;
        }

        private void chkAptType_CheckedChanged(object sender, EventArgs e)
        {
            RGAptType.SetEditValue(null);
            if (chkAptType.Checked == true) RGAptType.Enabled = true;
            else
                RGAptType.Enabled = false;
        }

        private void chkProj_CheckedChanged(object sender, EventArgs e)
        {
            RGProj.SetEditValue(null);
            if (chkProj.Checked == true) RGProj.Enabled = true;
            else
                RGProj.Enabled = false;
        }

        private void chkCity_CheckedChanged(object sender, EventArgs e)
        {
            RGCity.SetEditValue(null);
            if (chkCity.Checked == true) RGCity.Enabled = true;
            else
                RGCity.Enabled = false;
        }

        private void chkSubSou_CheckedChanged(object sender, EventArgs e)
        {
            RGSubSou.SetEditValue(null);
            if (chkSubSou.Checked == true) RGSubSou.Enabled = true;
            else
                RGSubSou.Enabled = false;
        }

        private void chkEmp_CheckedChanged(object sender, EventArgs e)
        {
            RGEmpStatus.SetEditValue(null);
            if (chkEmp.Checked == true) RGEmpStatus.Enabled = true;
            else
                RGEmpStatus.Enabled = false;
        }

        private void chkPoss_CheckedChanged(object sender, EventArgs e)
        {
            RGPoss.SetEditValue(null);
            if (chkPoss.Checked == true) RGPoss.Enabled = true;
            else
                RGPoss.Enabled = false;
        }

        private void chkFac_CheckedChanged(object sender, EventArgs e)
        {
            RGFac.SetEditValue(null);
            if (chkFac.Checked == true) RGFac.Enabled = true;
            else
                RGFac.Enabled = false;
        }

        private void chkArea_CheckedChanged(object sender, EventArgs e)
        {
            RGArea.SetEditValue(null);
            if (chkArea.Checked == true) RGArea.Enabled = true;
            else
                RGArea.Enabled = false;
        }

        private void chkFin_CheckedChanged(object sender, EventArgs e)
        {
            RGFinance.SetEditValue(null);
            if (chkFin.Checked == true)
                RGFinance.Enabled = true;
            else
                RGFinance.Enabled = false;
        }

        private void chkReligion_CheckedChanged(object sender, EventArgs e)
        {
            RGReligion.SetEditValue(null);
            if (chkReligion.Checked == true) RGReligion.Enabled = true;
            else
                RGReligion.Enabled = false;
        }

        private void chkState_CheckedChanged(object sender, EventArgs e)
        {
            RGState.SetEditValue(null);
            if (chkState.Checked == true) RGState.Enabled = true;
            else
                RGState.Enabled = false;
        }

        private void chkMaritalS_CheckedChanged(object sender, EventArgs e)
        {
            RGMaritalSta.SetEditValue(null);
            if (chkMaritalS.Checked == true) RGMaritalSta.Enabled = true;
            else RGMaritalSta.Enabled = false;
        }

        private void chkCountry_CheckedChanged(object sender, EventArgs e)
        {
            RGCountry.SetEditValue(null);
            if (chkCountry.Checked == true) RGCountry.Enabled = true;
            else
                RGCountry.Enabled = false;
        }

        private void chkCostPre_CheckedChanged(object sender, EventArgs e)
        {
            RGCostPre.SetEditValue(null);
            if (chkCostPre.Checked == true) RGCostPre.Enabled = true;
            else
                RGCostPre.Enabled = false;
        }

        private void chkGuestHou_CheckedChanged(object sender, EventArgs e)
        {
            RGGuestHou.SetEditValue(null);
            if (chkGuestHou.Checked == true) RGGuestHou.Enabled = true;
            else
                RGGuestHou.Enabled = false;
        }

        private void chkAptSize_CheckedChanged(object sender, EventArgs e)
        {
            RGAptSize.SetEditValue(null);
            if (chkAptSize.Checked == true) RGAptSize.Enabled = true;
            else
                RGAptSize.Enabled = false;
        }

        private void chkApt_CheckedChanged(object sender, EventArgs e)
        {
            RGApt.SetEditValue(null);
            if (chkApt.Checked == true) RGApt.Enabled = true;
            else
                RGApt.Enabled = false;
        }

        private void chkYearlyInc_CheckedChanged(object sender, EventArgs e)
        {
            RGYearlyIncome.SetEditValue(null);
            if (chkYearlyInc.Checked == true) RGYearlyIncome.Enabled = true;
            else
                RGYearlyIncome.Enabled = false;
        }

        private void chkNat_CheckedChanged(object sender, EventArgs e)
        {
            RGNationality.SetEditValue(null);
            if (chkNat.Checked == true) RGNationality.Enabled = true;
            else
                RGNationality.Enabled = false;
        }

        private void chkStay_CheckedChanged(object sender, EventArgs e)
        {
            RGStay.SetEditValue(null);
            if (chkStay.Checked == true) RGStay.Enabled = true;
            else
                RGStay.Enabled = false;
        }

        #endregion

        private void chkLocality_CheckedChanged(object sender, EventArgs e)
        {
            RGLocality.SetEditValue(null);
            if (chkLocality.Checked == true) RGLocality.Enabled = true;
            else
                RGLocality.Enabled = false;
        }
    }
}
