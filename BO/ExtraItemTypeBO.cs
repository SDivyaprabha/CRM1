using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRM.BusinessObjects
{
    class ExtraItemTypeBO
    {
        public static int ExtraItemId { set; get; }
        public static int ExtraItemTypeId { set; get; }
        public static string ItemCode { set; get; }
        public static string ItemDescription { set; get; }

        public static string UnitName { set; get; }
        public static decimal ExtraRate { set; get; }

        public static string ExtraItemTypeName { set; get; }
    }
}
