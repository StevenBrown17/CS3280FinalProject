using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject
{
    class clsSQL
    {
        /// <summary>
        /// This SQL gets all the data on an invoice for a given Invoice ID
        /// </summary> 
        /// <param name="sInvoiceNum">The Invoice ID for the invoice to retrieve all data.</param>
        /// <returns>All data for the given invoice.</returns>
        public string SelectInvoiceID(string sInvoiceNum)
        {
            string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sInvoiceNum;
            return sSQL;
        }

        /// <summary>
        /// This SQL gets all the data on an invoice for a given InvoiceDate
        /// </summary> 
        /// <param name="sInvoiceDate">The InvoiceDate for the invoice to retrieve all data.</param>
        /// <returns>All data for the given invoice.</returns>
        public string SelectInvoiceDate(string sInvoiceDate)
        {
            string sSQL = "SELECT * FROM Invoices WHERE InvoiceDate= " + sInvoiceDate;
            return sSQL;
        }


        

        //TODO: What if there are 2 invoices with the same charge?

        /// <summary>
        /// This SQL gets all the data on an invoice for a given total charge
        /// </summary> 
        /// <param name="sTotalCharge">The total charge for the invoice in order to retrieve all data.</param>
        /// <returns>All data for the given invoice.</returns>
        public string SelectTotalCharge(string sTotalCharge)
        {
            string sSQL = "SELECT * FROM Invoices WHERE TotalCharge= " + sTotalCharge;
            return sSQL;
        }


        /// <summary>
        /// Selects items from an invoice based on the invoice number given
        /// </summary>
        /// <param name="sInvoiceNum"></param>
        /// <returns></returns>
        public string SelectItemsOnInvoice(string sInvoiceNum){
            string sSQL = "SELECT ID.ItemDesc, ID.Cost "+
                          "FROM ItemDesc ID "+
                          "INNER JOIN(Invoices I INNER JOIN LineItems LI ON I.InvoiceNum = LI.InvoiceNum) ON ID.ItemCode = LI.ItemCode "+
                          "WHERE I.InvoiceNum = "+sInvoiceNum;
            return sSQL;
        }//end SelectItemsOnInvoice()




    }//end class
}//end namespace
