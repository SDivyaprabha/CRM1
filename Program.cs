using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;

namespace CRM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            //CultureInfo _oCultureInfo = new CultureInfo("en-US", false);

            //int[] sGroupFormat = { 3, 2 };

            //NumberFormatInfo oNumberFormatInfo = new NumberFormatInfo() { NumberGroupSizes = sGroupFormat };

            //_oCultureInfo.NumberFormat = oNumberFormatInfo;

            //Application.CurrentCulture = _oCultureInfo;

            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.OfficeSkins).Assembly);
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.BonusSkins).Assembly);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BsfGlobal.GetDBString();
            BsfGlobal.GetPassString();
            if (BsfGlobal.CheckDB() == false) { Application.Exit(); return; }
            if (BsfGlobal.g_bWFDB == false)
            {
                MessageBox.Show("Work Flow DB Not Found");
                Application.Exit();
                return;
            }
            BsfGlobal.LoadFunctionalNames();
            //BsfGlobal.GetPermission(BsfGlobal.g_lUserId);
            //BsfGlobal.GetPermission(1);
            //BsfGlobal.CheckPowerUser(1);
            //BsfGlobal.g_lUserId = 1;
            BsfGlobal.g_sCurrencyName = "Rupees";
            BsfGlobal.g_sDigitFormat = "n2";
            BsfGlobal.g_sDigitFormatS = "{0:n2}";

            // Currency Decimal Point 
            CultureInfo _oCultureInfo = new CultureInfo("en-US", false);

            int[] sGroupFormat = { 9, 0 };

            if (CommFun.g_iCurrencyDigit == 1) { int[] sGroupFormat1 = { 3, 3 }; sGroupFormat = sGroupFormat1; }
            if (CommFun.g_iCurrencyDigit == 2) { int[] sGroupFormat1 = { 3, 0 }; sGroupFormat = sGroupFormat1; }
            if (CommFun.g_iCurrencyDigit == 3 || CommFun.g_iCurrencyDigit == 0) { int[] sGroupFormat1 = { 3, 2 }; sGroupFormat = sGroupFormat1; }

            NumberFormatInfo oNumberFormatInfo = new NumberFormatInfo() { NumberGroupSizes = sGroupFormat };
            DateTimeFormatInfo oDateFormatInfo = new DateTimeFormatInfo() { ShortDatePattern = "dd/MM/yyyy" };

            _oCultureInfo.NumberFormat = oNumberFormatInfo;
            _oCultureInfo.DateTimeFormat = oDateFormatInfo;

            Application.CurrentCulture = _oCultureInfo;

            Application.Run(new frmLogin());
            Application.DoEvents();
        }
    }
}