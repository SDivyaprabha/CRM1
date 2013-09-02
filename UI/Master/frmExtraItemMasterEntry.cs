using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;
using System.Text;
using DevExpress.XtraBars;
using CRM.BusinessLayer;
using CRM.BusinessObjects;
using Telerik.WinControls.UI.Docking;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmExtraItemMasterEntry : DevExpress.XtraEditors.XtraForm
    {
        #region Variables
        public DataTable dt;
        public int ExtraItemId = 0;
        public string m_sMode = "";
        bool m_bOk = false;
        public DevExpress.XtraEditors.PanelControl Panel;
        #endregion

        readonly ExtraItemTypeBL oEitBL;


        #region Properties

        public int m_CompId { get; set; }
        public DataTable Dt { get; set; }

        public DataTable dtComp { get; set; }

        public RadPanel Radpanel { get; set; }
        #endregion

        #region Constructor

        public frmExtraItemMasterEntry()
        {
            oEitBL = new ExtraItemTypeBL();
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
        private void frmExtraItemMasterEntry_Load(object sender, EventArgs e)
        {
            Getdatalist();
            comboUnitName();
            if (m_sMode == "E" || m_sMode == "E1")
            {
                if (ExtraItemId != 0)
                {
                    FillData();
                }
            }
        }

        private void frmExtraItemMasterEntry_FormClosed(object sender, FormClosedEventArgs e)
        {

            ////if (BsfGlobal.g_bWorkFlow == true)
            ////{
            ////    if (ExtraItemId != 0)
            ////    {
            ////        Cursor.Current = Cursors.WaitCursor;
            ////        try
            ////        {
            ////            this.Parent.Controls.Owner.Hide();
            ////        }
            ////        catch
            ////        {
            ////        }
            ////        Cursor.Current = Cursors.Default;
            ////    }
            ////    else
            ////    {
            ////        this.Parent.Controls.Owner.Hide();
            ////    }

            ////}


        }
        #endregion


        #region Functions
        public bool Execute(int argPBRegId, string argMode)
        {
            ExtraItemId = argPBRegId;
           // m_CCId = argCCId;
            m_sMode = argMode;
            ShowDialog();
            return m_bOk;
        }

        public void GetComplaint()
        {
            string sql = string.Empty;
            sql = "Select ExtraItemTypeId,ExtraItemTypeName from ExtraItemTypeMaster";
            cboItemType.Properties.DataSource = CommFun.AddSelectToDataTable(CommFun.FillRecord(sql));
            cboItemType.Properties.PopulateColumns();
            cboItemType.Properties.DisplayMember = "ExtraItemTypeName";
            cboItemType.Properties.ValueMember = "ExtraItemTypeId";
            cboItemType.Properties.Columns["ExtraItemTypeId"].Visible = false;
            cboItemType.ItemIndex = 0;
        }

        public void comboUnitName()
        {

            DataTable dt = new DataTable();
            dt = ExtraItemTypeBL.GetUnit();
            cboUnit.Properties.DataSource = dt;
            cboUnit.Properties.ValueMember = "Unit_ID";
            cboUnit.Properties.DisplayMember = "Unit_Name";
            cboUnit.Properties.PopulateColumns();
            cboUnit.Properties.Columns["Unit_ID"].Visible = false;
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                //MessageBox.Show("Please enter numbers only");
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
           public void Getdatalist()
           {
               string sql = string.Empty;

               sql = "Select ExtraItemTypeId,ExtraItemTypeName from ExtraItemTypeMaster ";
               cboItemType.Properties.DataSource = CommFun.AddSelectToDataTable(CommFun.FillRecord(sql));
               cboItemType.Properties.PopulateColumns();
               cboItemType.Properties.DisplayMember = "ExtraItemTypeName";
               cboItemType.Properties.ValueMember = "ExtraItemTypeId";
               cboItemType.Properties.Columns["ExtraItemTypeId"].Visible = false;
               cboItemType.Properties.ShowHeader = false;
               cboItemType.Properties.ShowFooter = false;
               cboItemType.ItemIndex = 0;
           }

           public void UpdateExtra()
           {
               string sql="";
               SqlCommand cmd;
               try
               {

                   if (ExtraItemId == 0)
                   {
                       sql = String.Format("INSERT INTO ExtraItemMaster(ExtraItemTypeId,ItemCode,ItemDescription,UnitId,ExtraRate) VALUES({0}, '{1}', '{2}','{3}','{4}' ) SELECT SCOPE_IDENTITY();", Convert.ToInt32(cboItemType.EditValue), textItemCode.Text, txtDescription.Text, Convert.ToInt32(cboUnit.EditValue), Convert.ToDecimal(txtRate.Text));

                       cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                       ExtraItemId = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                       //CommFun.InsertLog(DateTime.Now, "ExtraItem Register-Add", "N", "Add ExtraItem Register", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                       BsfGlobal.InsertLog(DateTime.Now, "ExtraItem Register-Add", "N", "Add ExtraItem Register", ExtraItemId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                   }
                   else
                   {
                       sql = String.Format("UPDATE ExtraItemMaster SET ExtraItemTypeId={0}, ItemCode='{1}',ItemDescription='{2}',UnitId='{3}', ExtraRate='{4}' WHERE ExtraItemId={5}", Convert.ToInt32(cboItemType.EditValue), textItemCode.Text, txtDescription.Text, Convert.ToInt32(cboUnit.EditValue), Convert.ToDecimal(txtRate.Text), ExtraItemId);
                       cmd = new SqlCommand(sql, BsfGlobal.OpenCRMDB());
                       cmd.ExecuteNonQuery();
                       //CommFun.InsertLog(DateTime.Now, "ExtraItem Register-Edit", "E", "Edit ExtraItem Register", BsfGlobal.g_lUserId, 0, m_iCCId, 0, BsfGlobal.g_sCRMDBName);
                       BsfGlobal.InsertLog(DateTime.Now, "ExtraItem Register-Edit", "E", "Edit ExtraItem Register", ExtraItemId, 0, 0, BsfGlobal.g_sCRMDBName, "", BsfGlobal.g_lUserId);
                   }
               }
               catch (Exception e)
               {

                   throw e;
               }
           }

           public void FillData()
           {
               try
               {
                   string sql = "Select A.ExtraItemId,A.ItemCode,A.ItemDescription,A.ExtraItemTypeId,B.ExtraItemTypeName," +
                                "A.UnitId,A.ExtraRate from ExtraItemMaster A Inner Join ExtraItemTypeMaster B " +
                                "on A.ExtraItemTypeId=B.ExtraItemTypeId WHERE ExtraItemId=" + ExtraItemId + "";
                   DataTable dtEI = new DataTable();
                   dtEI = CommFun.FillRecord(sql);
                    if (dtEI.Rows.Count > 0)
                    {
                        cboItemType.EditValue =Convert.ToInt32(dtEI.Rows[0]["ExtraItemTypeId"].ToString());
                        textItemCode.Text =CommFun.IsNullCheck( dtEI.Rows[0]["ItemCode"].ToString(), CommFun.datatypes.vartypenumeric).ToString();
                        txtDescription.Text =CommFun.IsNullCheck( dtEI.Rows[0]["ItemDescription"].ToString(), CommFun.datatypes.vartypestring).ToString();
                        txtRate.Text =CommFun.IsNullCheck( dtEI.Rows[0]["ExtraRate"].ToString(), CommFun.datatypes.vartypenumeric).ToString();
                        cboUnit.EditValue = Convert.ToInt32(CommFun.IsNullCheck(dtEI.Rows[0]["UnitId"].ToString(), CommFun.datatypes.vartypenumeric).ToString());
                    }

               }
               catch (Exception e)
               {
                   throw e;
               }
           }

           private void ClearEntries()
           {
               cboItemType.Properties.DataSource = null;
               textItemCode.Text = "";
               txtDescription.Text = "";
               txtRate.Text = "";
               Getdatalist();
               comboUnitName();
           }
         #endregion

        #region Button Event
           private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
           {
               if (ExtraItemId == 0)
                   ClearEntries();
               //else
               //{
               //    if (BsfGlobal.g_bWorkFlow == true)
               //    {
               //        Close();
               //        Cursor.Current = Cursors.WaitCursor;
               //        frmExtraItemMasterReg frmProg = new frmExtraItemMasterReg();
               //        frmProg.TopLevel = false;
               //        frmProg.FormBorderStyle = FormBorderStyle.None;
               //        frmProg.Dock = DockStyle.Fill;
               //        frmExtraItemMasterReg.m_oDW.Show();
               //        frmExtraItemMasterReg.t_panel.Controls.Clear();
               //        frmExtraItemMasterReg.t_panel.Controls.Add(frmProg);
               //        frmProg.Show();
               //        Cursor.Current = Cursors.Default;
               //    }
               //    else
               //    {
               //        Close();
               //    }
               //}
               this.Close();
           }

           private void btnItemMaster_Click_1(object sender, EventArgs e)
           {
               using (frmExtraItemType frmCompMaster = new frmExtraItemType())
               {
                   frmCompMaster.ShowDialog();
               }
               GetComplaint();
           }

           private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
           {
               if (textItemCode.Text.Trim() == "")
                {
                    MessageBox.Show("Provide ItemCode", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    textItemCode.Focus();
                    return;
                }
               else if (Convert.ToInt32(cboItemType.EditValue) == -1)
               {
                   MessageBox.Show("Select ItemType", "CRM", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                   cboItemType.Focus();
                   return;
               }
               else if (Convert.ToInt32(cboUnit.EditValue) <= 0)
               {
                   MessageBox.Show("Select Unit");
                   cboUnit.Focus();
                   return;
               }

               m_bOk = true;
               UpdateExtra();
               Close();
           }

           private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
           {
               if (ExtraItemId == 0)
                   ClearEntries();
               //else 
               //{
               //    if (BsfGlobal.g_bWorkFlow == true)
               //    {
               //        Close();
               //        Cursor.Current = Cursors.WaitCursor;
               //        frmExtraItemMasterReg frmProg = new frmExtraItemMasterReg();
               //        frmProg.TopLevel = false;
               //        frmProg.FormBorderStyle = FormBorderStyle.None;
               //        frmProg.Dock = DockStyle.Fill;
               //        frmExtraItemMasterReg.m_oDW.Show();
               //        frmExtraItemMasterReg.t_panel.Controls.Clear();
               //        frmExtraItemMasterReg.t_panel.Controls.Add(frmProg);
               //        frmProg.Show();
               //        Cursor.Current = Cursors.Default;
               //    }
               //    else
               //    {
               //        Close();
               //    }
               //}
               m_bOk = false;
               this.Close();
           }
           #endregion

           private void textItemCode_Leave(object sender, EventArgs e)
           {
               if (Convert.ToInt32(cboItemType.EditValue) != 0 && Convert.ToInt32(cboItemType.EditValue) !=-1)
               {
                   string sql = "Select ItemCode From ExtraItemMaster Where ItemCode = '" + CommFun.IsNullCheck(textItemCode.EditValue.ToString(), CommFun.datatypes.vartypestring) + "' and ExtraItemId <> " + ExtraItemId;
                   DataTable dtEI = new DataTable();
                   dtEI = CommFun.FillRecord(sql);
                   if (dtEI.Rows.Count > 0)
                   {
                       MessageBox.Show("Code Already Exist");
                       textItemCode.EditValue = null;
                       textItemCode.Focus();
                   }
                   else
                   {
                       txtDescription.Focus();
                   }
               }
           }

           private void txtDescription_Leave(object sender, EventArgs e)
           {
               string sql = "Select ItemDescription From ExtraItemMaster Where ItemDescription = '" + CommFun.IsNullCheck(txtDescription.EditValue.ToString(),CommFun.datatypes.vartypestring) + "' and ExtraItemId <> " + ExtraItemId;
               DataTable dtEI = new DataTable();
               dtEI = CommFun.FillRecord(sql);
               if (dtEI.Rows.Count > 0)
               {
                   MessageBox.Show("Item Description Already Exist");
                   txtDescription.EditValue = null;
                   txtDescription.Focus();
               }
               else
               {
                   cboItemType.Focus();
               }
           }

        private void txtDescription_EditValueChanged(object sender, EventArgs e)
        {
        
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Projects.frmUOMListR frm = new Projects.frmUOMListR();
            frm.Execute("U", "");
            comboUnitName();
        }

    }
}
