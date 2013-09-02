using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmDateBetween : DevExpress.XtraEditors.XtraForm
    {
        public bool m_bOk;

        public frmDateBetween()
        {
            InitializeComponent();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            if (!DesignMode && IsHandleCreated)
                BeginInvoke(new MethodInvoker(() =>{base.OnSizeChanged(e);}));
            else
                base.OnSizeChanged(e);
        }

        private void frmDateBetween_Load(object sender, EventArgs e)
        {
            dtFrom.Properties.MinValue = BsfGlobal.g_dStartDate;
            dtFrom.Properties.MaxValue = BsfGlobal.g_dEndDate;
            
            dtTo.Properties.MinValue = BsfGlobal.g_dStartDate;
            dtTo.Properties.MaxValue = BsfGlobal.g_dEndDate;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_bOk = true;
            Close();
        }

        #region Functions
        public bool Execute(ref DateTime arg_dFrom, ref DateTime arg_dTo)
        {
            dtFrom.EditValue = arg_dFrom;
            dtTo.EditValue = arg_dTo;
            ShowDialog();
            if (m_bOk == true)
            {
                arg_dFrom = (DateTime)dtFrom.EditValue;
                arg_dTo= (DateTime)dtTo.EditValue;
            }

            return m_bOk;
        }
        
        
        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bOk = false;
            Close();
        }

        private void frmDateBetween_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                m_bOk = false;
                Close();
            }
        }
    }
}