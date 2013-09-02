using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using CRM.BusinessLayer;
using DevExpress.XtraEditors;


namespace CRM
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {

        #region Declaration   

        int pwdBool;
        const int UidBool = 0; 

        #endregion

        #region objects
        #endregion

        #region Constructor

        public frmLogin()
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

        #region Click Event's

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();            
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            SuspendLayout();
            Cursor.Current = Cursors.WaitCursor;
            PopulateAutoCompleteXtra(txtUser);
            ResumeLayout();
        }
        private void cmdOK_Click(object sender, EventArgs e)
        
        {            
            try
            {
                string check_userName = UserBL.GetColumn(String.Format("SELECT UserName FROM Users WHERE UserName='{0}'", txtUser.Text.Trim()));
                string check_pwd = UserBL.GetColumn(String.Format("SELECT ISNULL(Password,'') FROM Users WHERE UserName='{0}'", txtUser.Text.Trim()));
                if ((check_userName != ""))
                {                   
                    //UidBool = check_userName.CompareTo(txtUser.Text.Trim());
                    if (check_pwd != "") { check_pwd = UserBL.Decrypt(check_pwd); }
                    txtPassword.Text = check_pwd;
                    pwdBool = check_pwd.CompareTo(txtPassword.Text.Trim());

                    if ((UidBool == 0) && (pwdBool == 0))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        BsfGlobal.g_sUserName = txtUser.Text.Trim();
                        BsfGlobal.g_lUserId = Convert.ToInt32(UserBL.GetColumn(String.Format("SELECT UserId FROM Users WHERE UserName='{0}'", txtUser.Text.Trim())));
                        BsfGlobal.CheckPowerUser(BsfGlobal.g_lUserId);
                        BsfGlobal.GetPermission(BsfGlobal.g_lUserId);
                        CommFun.g_iCurrencyDigit = 2;
                        BsfGlobal.InsertLog(DateTime.Now, "Login", "S", "Login", 0, 0, 0, BsfGlobal.g_sWorkFlowDBName,"",BsfGlobal.g_lUserId);
                        //Application.Run(new frmNewMain());
                        frmHomeScreen HomePage = new frmHomeScreen();
                        Hide();
                        ShowInTaskbar = false;

                        HomePage.Show();
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Password");
                    }

                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void frmLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                SendKeys.Send("{TAB}");
            else if (e.KeyChar == 27)
                Close();
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SendKeys.Send("{TAB}");
            else if (e.KeyCode == Keys.Escape)
                Close();
        }

        public static void PopulateAutoCompleteXtra(TextEdit argobject)
        {
            argobject.MaskBox.AutoCompleteCustomSource.Clear();
            argobject.MaskBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            argobject.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            
            DataTable Dt = new DataTable();
            Dt = UserBL.GetUser();
            for (int lCount = 0; lCount <= Dt.Rows.Count - 1; lCount++)
            {
                argobject.MaskBox.AutoCompleteCustomSource.Add((string)Dt.Rows[lCount]["UserName"]);
            }
            Dt.Dispose();
        }
        
    }
}
