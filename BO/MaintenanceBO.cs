using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class MaintenanceBO
    {
        public static int MainId { set; get; }
        public static string RefNo { set; get; }
        public static int CostCentreId { set; get; }
        public static int FlatId { set; get; }

        public static string RegDate { set; get; }
        public static string StartDate { set; get; }
        public static string EndDate { set; get; }

        public static string Duration { set; get; }

        public static decimal Period { set; get; }
        public static string IntDuration { set; get; }
        public static decimal IntRate { set; get; }
        public static string Terms { set; get; }
        public static int GracePeriod { set; get; }
        public static string Approve { set; get; }
    }
}
