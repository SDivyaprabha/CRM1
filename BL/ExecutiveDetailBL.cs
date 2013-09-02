using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace CRM.BusinessLayer
{
    class ExecutiveDetailBL

    {
        #region Variables
          DataTable dtDesignation;
          int execId;
          string execName;
          int desigId;
          string phoneRes;
          string mobile;
          string email;
          string address;
          string eduQual;
          DateTime dob;
          string fatherName;
          DateTime doj;
          string remarks;
          int flag;

        #endregion

        #region Objects
            CRM.DataLayer.ExecutiveDetailDL oExecDetailDL;
        #endregion

        #region Constructor
        public ExecutiveDetailBL()
        {
            oExecDetailDL = new CRM.DataLayer.ExecutiveDetailDL();
        }
        #endregion
        #region Properties
        public int ExecId
        {
            get { return execId; }
            set { execId = value; }
        }
        public string ExecName
        {
            get { return execName; }
            set { execName = value; }
        }

        public int DesigId
        {
            get { return desigId; }
            set { desigId = value; }
        }
        public int Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        public string PhoneRes
        {
            get { return phoneRes; }
            set { phoneRes = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string EduQual
        {
            get { return eduQual; }
            set { eduQual = value; }
        }
        public DateTime DOB
        {
            get { return dob; }
            set { dob = value; }
        }
        public string Fathername
        {
            get { return fatherName; }
            set { fatherName = value; }
        }
        public DateTime DOJ
        {
            get { return doj; }
            set { doj = value; }
        }
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

                
        public DataTable DtDesignation
        {
            get { return dtDesignation; }
            set { dtDesignation = value; }
        }
        public DataTable GetDesignation(SqlConnection Con)
        {
            try
            {
                dtDesignation = oExecDetailDL.GetDesignation(Con);

            }
            catch(Exception e)
            {
                throw e;

            }
            return dtDesignation;
        }

        public void Update(SqlConnection Con)
        {
            oExecDetailDL.Update(this, Con);

        }
        #endregion
    }
}
