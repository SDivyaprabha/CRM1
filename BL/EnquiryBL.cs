using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class EnquiryBL
    {

        #region Objects
            EnquiryDL oEnqDL;          

	    #endregion
        
        #region variables

            DataTable dtExec;
            DataTable dtReligion;
            DataTable dtCountry;
            DataTable dtSource;
            DataTable dtSubSource;
            DataTable dtEmployment;
            DataTable dtIncome;
            DataTable dtApartment;
            DataTable dtApartSize;
            DataTable dtApartType;
            DataTable dtStay;
            DataTable dtGuestHouse;
            DataTable dtCostPref;
            DataTable dtPosess;
            DataTable dtFacility;
            DataTable dtArea;
            DataTable dtProject;
            DataSet getdata;
            CheckedListBox chkPos;
            CheckedListBox chkArea;
            CheckedListBox chkFacility;

            int enquiryid;
            DateTime enquirydate;
            string enquiryname;
            string profession;
            string organization;
            string nationality;
            string address1;
            string address2;
            string city;
            string state;
            string pincode;
            string phoneoff;
            string phoneres;
            string mobile;
            string fax;
            string email;
            string website;
            string maritalstatus;
            int enquirymodeid;
            int enquirysubmodeid;
            int projectid;
            int executiveid;
            DateTime nextfollowupdate;
            string remarks;
            Boolean active;
            string additionalinfo;
            int employmentid;
            int apartmentid;
            int apartmentsizeid;
            int apartmenttypeid;
            int costpreferenceid;
            int guesthouseid;
            int incomeid;
            int religionid;
            int stayid;
            Boolean buyer;
            int reasonid;
            int freasonid;
            Boolean brokerage;
            decimal commissionper;
            decimal amount;
            DateTime birthdate;
            DateTime weddingdate;
            string country;
            string citizenship;
            int noofchild;
            int achievementid;
            int accountid;
            int buyerid;
            int calltypeid;
            string coappname;
            string coaddress1;
            string coaddress2;
            string coappcity;
            string fathusname;
            int age;
            int coappage;
            string coappfathusname;
            string panno;
            string padrs1;
            string padrs2;
            string pcity;
            string pstate;
            string pcountry;
            string ppincode;
            string bankname;
            string loandetails;
            string disqual;
            int no_of_emp;
            decimal annual_revenue;
            string industry;
            string description;
            int created_UserId;
            int modified_UserId;
            DateTime modified_Date;
            int projtypeid;
            int flatid;
            int Prev;

            #endregion
        
        #region Properties

            public int Projtypeid
            {
                get { return projtypeid; }
                set { projtypeid = value; }
            }
            public int Flatid
            {
                get { return flatid; }
                set { flatid = value; }
            }

            public CheckedListBox ChkPos
            {
                get { return chkPos; }
                set { chkPos = value; }
            }

            public CheckedListBox ChkArea
            {
                get { return chkArea; }
                set { chkArea = value; }
            }

            public CheckedListBox ChkFacility
            {
                get { return chkFacility; }
                set { chkFacility = value; }
            }

             public int Enquiryid
                {
                    get { return enquiryid; }
                    set { enquiryid = value; }
                }

             public DateTime Enquirydate
             {
                 get { return enquirydate; }
                 set { enquirydate = value; }
             }

             public string Enquiryname
             {
                 get { return enquiryname; }
                 set { enquiryname = value; }
             }

             public string Profession
             {
                 get { return profession; }
                 set { profession = value; }
             }

             public string Organization
             {
                 get { return organization; }
                 set { organization = value; }
             }

             public string Nationality
             {
                 get { return nationality; }
                 set { nationality = value; }
             }

             public string Address1
             {
                 get { return address1; }
                 set { address1 = value; }
             }

             public string Address2
             {
                 get { return address2; }
                 set { address2 = value; }
             }

             public string City
             {
                 get { return city; }
                 set { city = value; }
             }

             public string State
             {
                 get { return state; }
                 set { state = value; }
             }

             public string Pincode
             {
                 get { return pincode; }
                 set { pincode = value; }
             }

             public string Phoneoff
             {
                 get { return phoneoff; }
                 set { phoneoff = value; }
             }

             public string Phoneres
             {
                 get { return phoneres; }
                 set { phoneres = value; }
             }

             public string Mobile
             {
                 get { return mobile; }
                 set { mobile = value; }
             }

             public string Fax
             {
                 get { return fax; }
                 set { fax = value; }
             }

             public string Email
             {
                 get { return email; }
                 set { email = value; }
             }

             public string Website
             {
                 get { return website; }
                 set { website = value; }
             }

             public string Maritalstatus
             {
                 get { return maritalstatus; }
                 set { maritalstatus = value; }
             }

             public int Enquirymodeid
             {
                 get { return enquirymodeid; }
                 set { enquirymodeid = value; }
             }

             public int Enquirysubmodeid
             {
                 get { return enquirysubmodeid; }
                 set { enquirysubmodeid = value; }
             }

             public int Projectid
             {
                 get { return projectid; }
                 set { projectid = value; }
             }

             public int Executiveid
             {
                 get { return executiveid; }
                 set { executiveid = value; }
             }

             public DateTime Nextfollowupdate
             {
                 get { return nextfollowupdate; }
                 set { nextfollowupdate = value; }
             }

             public string Remarks
             {
                 get { return remarks; }
                 set { remarks = value; }
             }

             public Boolean Active
             {
                 get { return active; }
                 set { active = value; }
             }

             public string Additionalinfo
             {
                 get { return additionalinfo; }
                 set { additionalinfo = value; }
             }

             public int Employmentid
             {
                 get { return employmentid; }
                 set { employmentid = value; }
             } 
                 
            public int Apartmentid
             {
                 get { return apartmentid; }
                 set { apartmentid = value; }
             }

            public int Apartmentsizeid
            {
                get { return apartmentsizeid; }
                set { apartmentsizeid = value; }
            }

            public int Apartmenttypeid
            {
                get { return apartmenttypeid; }
                set { apartmenttypeid = value; }
            }

            public int Costpreferenceid
            {
                get { return costpreferenceid; }
                set { costpreferenceid = value; }
            }

            public int Guesthouseid
            {
                get { return guesthouseid; }
                set { guesthouseid = value; }
            }

            public int Incomeid
            {
                get { return incomeid; }
                set { incomeid = value; }
            }

            public int Religionid
            {
                get { return religionid; }
                set { religionid = value; }
            }

            public int Stayid
            {
                get { return stayid; }
                set { stayid = value; }
            }

            public Boolean Buyer
            {
                get { return  buyer; }
                set { buyer = value; }
            }

            public int Reasonid
            {
                get { return reasonid; }
                set { reasonid = value; }
            }

            public int Freasonid
            {
                get { return freasonid; }
                set { freasonid = value; }
            }

            public Boolean Brokerage
            {
                get { return brokerage; }
                set { brokerage = value; }
            }

            public decimal Commissionper
            {
                get { return commissionper; }
                set { commissionper = value; }
            }

            public int BrokerId { get; set; }

            public decimal Amount
            {
                get { return amount; }
                set { amount = value; }
            }

            public DateTime Birthdate
            {
                get { return birthdate; }
                set { birthdate = value; }
            }

            public DateTime Weddingdate
            {
                get { return weddingdate; }
                set { weddingdate = value; }
            }

            public string Country
            {
                get { return country; }
                set { country = value; }
            }

            public string Citizenship
            {
                get { return citizenship; }
                set { citizenship = value; }
            }

            public int Noofchild
            {
                get { return noofchild; }
                set { noofchild = value; }
            }

            public int Achievementid
            {
                get { return achievementid; }
                set { achievementid = value; }
            }

            public int Accountid
            {
                get { return accountid; }
                set { accountid = value; }
            }

            public int Buyerid
            {
                get { return buyerid; }
                set { buyerid = value; }
            }
           
            public int CallTypeID
            {
                get { return calltypeid; }
                set { calltypeid = value; }
            }

            public string Coappname
            {
                get { return coappname; }
                set { coappname = value; }
            }

            public string Coaddress1
            {
                get { return coaddress1; }
                set { coaddress1 = value; }
            }

            public string LeadType { get; set; }

            public string Coaddress2
            {
                get { return coaddress2; }
                set { coaddress2 = value; }
            }

            public string Coappcity
            {
                get { return coappcity; }
                set { coappcity = value; }
            }

            public string Fathusname
            {
                get { return fathusname; }
                set { fathusname = value; }
            }

            public int Age
            {
                get { return age; }
                set { age = value; }
            }

            public int Coappage
            {
                get { return coappage; }
                set { coappage = value; }
            }

            public string Coappfathusname
            {
                get { return coappfathusname; }
                set { coappfathusname = value; }
            }

            public string Panno
            {
                get { return panno; }
                set { panno = value; }
            }

            public string Padrs1
            {
                get { return padrs1; }
                set { padrs1 = value; }
            }

            public string Padrs2
            {
                get { return padrs2; }
                set { padrs2 = value; }
            }

            public string Pcity
            {
                get { return pcity; }
                set { pcity = value; }
            }

            public string Pstate
            {
                get { return pstate; }
                set { pstate = value; }
            }

            public string Pcountry
            {
                get { return pcountry; }
                set { pcountry = value; }
            }

            public string Ppincode
            {
                get { return ppincode; }
                set { ppincode = value; }
            }

            public string Bankname
            {
                get { return bankname; }
                set { bankname = value; }
            }

            public string Loandetails
            {
                get { return loandetails; }
                set { loandetails = value; }
            }

            public string Disqual
            {
                get { return disqual; }
                set { disqual = value; }
            }

            public int No_of_emp
            {
                get { return no_of_emp; }
                set { no_of_emp = value; }
            }

            public decimal Annual_revenue
            {
                get { return annual_revenue; }
                set { annual_revenue = value; }
            }

            public string Industry
            {
                get { return industry; }
                set { industry = value; }
            }

            public string Description
            {
                get { return description; }
                set { description = value; }
            }

            public int Created_UserId
            {
                get { return created_UserId; }
                set { created_UserId = value; }
            }

            public int Modified_UserId
            {
                get { return modified_UserId; }
                set { modified_UserId = value; }
            }

            public DateTime Modified_Date
            {
                get { return modified_Date; }
                set { modified_Date = value; }
            }  

        public int PrevCust
        {
            get { return Prev; }
            set { Prev = value; }
            }
        
            #endregion
        
        #region Constructor
                public EnquiryBL()
                {
                    oEnqDL = new CRM.DataLayer.EnquiryDL();
                }
            #endregion
        
        #region Methods

        public DataSet Getdata
        {
            get { return getdata; }
            set { getdata = value; }
        }

        public DataSet GetData()
        {
            getdata= oEnqDL.GetData();
            return getdata;
        }

        public DataTable GetReligion(SqlConnection Con)
        {
            try
            {
                dtReligion = oEnqDL.GetReligion(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtReligion;
        }

        public DataTable GetCountry(SqlConnection Con)
        {
            try
            {
                dtCountry = oEnqDL.GetCountry(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtCountry;
        }

        public DataTable GetApartment(SqlConnection Con)
        {
            try
            {
                dtApartment = oEnqDL.GetApartment(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtApartment;
        }

        public DataTable GetApartmentSize(SqlConnection Con)
        {
            try
            {
                dtApartSize = oEnqDL.GetApartmentsize(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtApartSize;
        }

        public DataTable GetApartmentType(SqlConnection Con)
        {
            try
            {
                dtApartType = oEnqDL.GetApartmentType(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtApartType;
        }

        public DataTable GetStay(SqlConnection Con)
        {
            try
            {
                dtStay = oEnqDL.GetStay(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtStay;
        }

        public DataTable GetGuesthouse(SqlConnection Con)
        {
            try
            {
                dtGuestHouse = oEnqDL.GetGuestHouse(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtGuestHouse;
        }

        public DataTable GetCostPref(SqlConnection Con)
        {
            try
            {
                dtCostPref = oEnqDL.GetCostPreference(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtCostPref;
        }

        public DataTable GetPosesses(SqlConnection Con)
        {
            try
            {
                dtPosess = oEnqDL.GetPosesses(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtPosess;
        }

        public DataTable GetFacility(SqlConnection Con)
        {
            try
            {
                dtFacility = oEnqDL.GetFacility(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtFacility;
        }

        public DataTable GetArea(SqlConnection Con)
        {
            try
            {
                dtArea = oEnqDL.GetArea(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtArea;
        }

        public DataTable GetProject(SqlConnection Con)
        {
            try
            {
                dtProject = oEnqDL.GetProject(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtProject;
        }

        public DataTable GetEmployment(SqlConnection Con)
        {
            try
            {
                dtEmployment = oEnqDL.GetEmployment(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtEmployment;
        }

        public DataTable GetIncome(SqlConnection Con)
        {
            try
            {
                dtIncome = oEnqDL.GetIncome(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtIncome;
        }

        public DataTable GetSource(SqlConnection Con)
        {
            try
            {
                dtSource = oEnqDL.GetSource(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtSource;
        }

        public DataTable GetSubSource(SqlConnection Con,int argSourceId)
        {
            try
            {
                dtSubSource = oEnqDL.GetSubSource(Con, argSourceId);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtSubSource;
        }

        public DataTable GetExecutive(SqlConnection Con)
        {
            try
            {
                dtExec = oEnqDL.GetExecutive(Con);

            }
            catch (Exception e)
            {
                throw e;
            }
            return dtExec;
        }

        public void Update()
        {
            oEnqDL.Update(this);

        }

        #endregion

    }
}
