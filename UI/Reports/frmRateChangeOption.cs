using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CRM
{
    public partial class frmRateChangeOption : Form
    {
        #region Variables

        int m_iCCId = 0;
        string m_sType = "";

        #endregion

        #region Constructor

        public frmRateChangeOption()
        {
            InitializeComponent();
        }

        #endregion

        #region Form Event

        private void frmRateChangeOption_Load(object sender, EventArgs e)
        {
            m_sType = "All";
        }

        #endregion

        #region Functions

        public void Execute(int argCCId)
        {
            m_iCCId = argCCId;
            ShowDialog();
        }

        #endregion

        #region SelectedIndex Changed

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                m_sType = "All";
            }
            else if (radioGroup1.SelectedIndex == 1)
            {
                m_sType = "Flat Type";
            }
            else if (radioGroup1.SelectedIndex == 2)
            {
                m_sType = "Selected Flats";
            }
            else if (radioGroup1.SelectedIndex == 3)
            {
                m_sType = "Sold";
            }
            else if (radioGroup1.SelectedIndex == 4)
            {
                m_sType = "UnSold";
            }
            else if (radioGroup1.SelectedIndex == 5)
            {
                m_sType = "With Receipt";
            }
            else if (radioGroup1.SelectedIndex == 6)
            {
                m_sType = "OtherCost";
            }
        }

        #endregion

        #region Button Event

        private void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmUnitChanges frm = new frmUnitChanges();
            frm.Execute(m_iCCId,m_sType);
            Close();
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        #endregion

    }
}
