using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.BO
{
    class FinalizationBO
    {
        #region Methods

        public string s_ExeName { get; set; }
        public string s_FlatName { get; set; }
        public string s_Status { get; set; }
        public string s_CustomerType { get; set; }
        public string s_PaymentOption { get; set; }

        public decimal d_LoanPer { get; set; }
        public string i_LoanAccNo { get; set; }
        public decimal d_BrokerComm { get; set; }
        public decimal d_BrokerAmt { get; set; }

        public DateTime DE_ValidUpto { get; set; }
        public DateTime DE_Final { get; set; }

        public int i_CostCentreId { get; set; }
        public int i_AccountId { get; set; }
        public int i_FlatId { get; set; }
        public int i_LeadId { get; set; }
        public int i_BankId { get; set; }
        public int i_BrokerId { get; set; }
        public int i_PostExecId { get; set; }
        public decimal d_AdvAmt { get; set; }

        #endregion
    }
}
