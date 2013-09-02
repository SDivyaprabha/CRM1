using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BusinessObjects
{
    public class FlatTypeCarBO
    {
        public int CCId { set; get; }
        public int FlatTypeId { set; get; }
        public int TypeId { set; get; }
        public int TotalCP { set; get; }
    }
    public class FlatCarParkBO
    {
        public int CCId { set; get; }
        public int FlatId { set; get; }
        public int TypeId { set; get; }
        public int TotalCP { set; get; }
    }
}
