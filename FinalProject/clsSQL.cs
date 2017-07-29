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
        public String SelectItemsOnInvoice(string sInvoiceNum){
            String sSQL = "SELECT ID.ItemDesc, ID.Cost "+
                          "FROM ItemDesc ID "+
                          "INNER JOIN(Invoices I INNER JOIN LineItems LI ON I.InvoiceNum = LI.InvoiceNum) ON ID.ItemCode = LI.ItemCode "+
                          "WHERE I.InvoiceNum = "+sInvoiceNum;
            return sSQL;
        }//end SelectItemsOnInvoice()


        /// <summary>
        /// Selects inventory items and associated cost.
        /// </summary>
        /// <returns></returns>
        public String SelectInventoryItems() {
            String sSQL = "SELECT ItemDesc.ItemDesc, ItemDesc.Cost " +
                          "FROM ItemDesc;";
            return sSQL;
        }

        /*public string AddInventoryItem()
        {
            string sSQL = "test";
            return sSQL;
        }*/

        /// <summary>
        /// Deletes an entry from the ItemDesc table.
        /// </summary>
        /// <param name="ItemCode">Primary Key for the ItemDesc table.</param>
        /// <returns></returns>
        public string DeleteInventoryItem(string ItemCode)
        {
            string sSQL = "DELETE FROM ItemDesc " +
                          "WHERE ItemCode = " + ItemCode;
            return sSQL;
        }

        /// <summary>
        /// Edit an entry from the ItemDesc table.
        /// </summary>
        /// <param name="ItemCode">Priamry key for the ItemDesc table.</param>
        /// <param name="ItemDesc">Contains the description of the item.</param>
        /// <param name="Cost">Contains the cost of the item.</param>
        /// <returns></returns>
        public string EditInventoryItem(string ItemCode, string ItemDesc, string Cost)
        {
            string sSQL = "UPDATE ItemDesc " +
                          "SET ItemDesc = " + ItemDesc + ", Cost = " + Cost + " " +
                          "WHERE ItemCode = " + ItemCode;
            return sSQL;
        }
    }//end class
}//end namespace
