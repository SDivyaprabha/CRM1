using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class ProjectSalesBL
    {
        internal static DataTable Get_Project_Sales(DateTime argAsOnDate)
        {
            return ProjectSalesDL.Get_Project_Sales(argAsOnDate);
        }

        internal static DataTable Get_Block_Sales(int arg_iProjectId, DateTime argAsOnDate)
        {
            return ProjectSalesDL.Get_Block_Sales(arg_iProjectId, argAsOnDate);
        }

        internal static DataTable Get_Level_Sales(int arg_iProjectId, int arg_iBlockId, DateTime argAsOnDate)
        {
            return ProjectSalesDL.Get_Level_Sales(arg_iProjectId, arg_iBlockId, argAsOnDate);
        }

        internal static DataTable Get_Flat_Sales(int arg_iProjectId, int arg_iBlockId, int m_iLevelId, DateTime argAsOnDate)
        {
            return ProjectSalesDL.Get_Flat_Sales(arg_iProjectId, arg_iBlockId, m_iLevelId, argAsOnDate);
        }

        internal static DataTable Get_BWProject_Sales(string argFromDate, string argToDate)
        {
            return ProjectSalesDL.Get_BWProject_Sales(argFromDate,argToDate);
        }

        internal static DataTable Get_BWBlock_Sales(int arg_iProjectId, string argFromDate, string argToDate)
        {
            return ProjectSalesDL.Get_BWBlock_Sales(arg_iProjectId, argFromDate,argToDate);
        }

        internal static DataTable Get_BWLevel_Sales(int arg_iProjectId, int arg_iBlockId, string argFromDate, string argToDate)
        {
            return ProjectSalesDL.Get_BWLevel_Sales(arg_iProjectId,arg_iBlockId, argFromDate, argToDate);
        }

        internal static DataTable Get_BWFlat_Sales(int arg_iProjectId, int arg_iBlockId, int arg_iLevelId, string argFromDate, string argToDate)
        {
            return ProjectSalesDL.Get_BWFlat_Sales(arg_iProjectId, arg_iBlockId, arg_iLevelId, argFromDate,argToDate);
        }

    }
}
