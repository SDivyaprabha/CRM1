using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CRM.DataLayer;

namespace CRM.BusinessLayer
{
    class StagewiseReceivableBL
    {
        public static DataTable GetProject()
        {
            return StagewiseReceivableDL.GetProject();
        }

        public static DataTable GetBlock(int argCCId)
        {
            return StagewiseReceivableDL.GetBlock(argCCId);
        }

        public static DataTable GetPayment(int argCCId)
        {
            return StagewiseReceivableDL.GetPayment(argCCId);
        }

        public static DataSet GetProjectStageRec(int argCCId, int argPayTypeId, DateTime argDate,int argFromActual)
        {
            return StagewiseReceivableDL.GetProjectStageRec(argCCId, argPayTypeId, argDate,argFromActual);
        }

        public static DataSet GetBlockStageRec(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            return StagewiseReceivableDL.GetBlockStageRec(argCCId, argPayTypeId, argDate,argFromActual);
        }

        public static DataSet GetBuyerStageRec(int argCCId, int argBlockId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            return StagewiseReceivableDL.GetBuyerStageRec(argCCId ,argBlockId, argPayTypeId, argDate,argFromActual);
        }

        public static DataSet GetBuyerStageRecReport(int argCCId, int argPayTypeId, DateTime argDate, int argFromActual)
        {
            return StagewiseReceivableDL.GetBuyerStageRecReport(argCCId, argPayTypeId, argDate,argFromActual);
        }

    }
}
