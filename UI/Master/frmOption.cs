using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace CRM
{
    public partial class frmOption : DevExpress.XtraEditors.XtraForm
    {  
        #region Variables

        string m_sType = "";

        #endregion
        
        #region Constructor

        public frmOption()
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

        private void frmOption_Load(object sender, EventArgs e)
        {
            radioGroup1.SelectedIndex = 0;
        }
       
        #endregion      

        #region Button Event

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
                m_sType = "D";
            else if (radioGroup1.SelectedIndex == 1)
                m_sType = "S";
            else if (radioGroup1.SelectedIndex == 2)
                m_sType = "O";
            else
                m_sType = "";

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_sType = "";
            Close();
        }
        #endregion

        #region Function
        public string Execute()
        {
            ShowDialog();
            return m_sType;
        }
        #endregion
    }
}
