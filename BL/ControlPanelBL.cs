using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRM.DataLayer;
using System.Data;

namespace CRM.BusinessLayer
{
    class ControlPanelBL
    {
        public static DataTable GetUserDetails()
        {
            return ControlPanelDL.GetUserDetails();
        }

        public static void UpdatePanel(bool argAddr, bool argLive, int argUserId)
        {
            ControlPanelDL.UpdatePanel(argAddr, argLive, argUserId);
        }

    }
}
