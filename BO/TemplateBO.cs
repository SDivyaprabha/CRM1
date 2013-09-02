using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CRM.BusinessObjects
{
    class TemplateBO
    {
    }

    class TemplateEntryBO
    {
        public int TempId{set;get;}
        public string TempName { set; get; }
        public string TempType { set; get; }
        public string Attach { set; get; }
        public string View { set; get; }        
    }
}
