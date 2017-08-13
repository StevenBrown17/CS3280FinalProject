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
        /// This SQL gets all invoices
        /// </summary> 
        /// <returns>All data for the given invoice.</returns>
        public string populateAllInvoices()
        {
            string sSQL = "SELECT * FROM Invoices";
            return sSQL;
        }

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
            String sSQL = "SELECT ItemDesc.ItemCode,ItemDesc.ItemDesc, ItemDesc.Cost " +
                          "FROM ItemDesc;";
            return sSQL;
        }


        /// <summary>
        /// gets the date of the selected invoice number so we can populate the datepicker.
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public String SelectInvoiceDateFromNum(String invoiceNum) {
            String SQL = "SELECT Invoices.[InvoiceDate] "+
                         "FROM Invoices " +
                         "WHERE Invoices.InvoiceNum = "+invoiceNum +";";

            return SQL;
        }

        /// <summary>
        /// Finds if item is in an invoice.
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public string CheckIfItemIsInAnInvoice(string ItemCode)
        {
            string sSQL = "SELECT DISTINCT ItemCode " +
                          "FROM LineItems " +
                          "WHERE ItemCode = " + ItemCode;
            return sSQL;
        }

        /// <summary>
        /// Adds an inventory item to the database.
        /// </summary>
        /// <param name="ItemDesc"></param>
        /// <param name="Cost"></param>
        /// <returns></returns>
        public string AddInventoryItem(string ItemDesc, string Cost)
        {
            string sSQL = "INSERT INTO ItemDesc(ItemDesc, Cost) " +
                          "VALUES('" + ItemDesc + "', " + Cost + ");";

            return sSQL;
        }

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
        /// <param name="ItemCode">primary key for the ItemDesc table.</param>
        /// <param name="ItemDesc">Contains the description of the item.</param>
        /// <param name="Cost">Contains the cost of the item.</param>
        /// <returns></returns>
        public string EditInventoryItem(string ItemCode, string ItemDesc, string Cost)
        {
            string sSQL = "UPDATE ItemDesc " +
                          "SET ItemDesc = '" + ItemDesc + "', Cost = " + Cost +
                          " WHERE ItemCode = " + ItemCode;
            return sSQL;
        }
        
        /// <summary>
        /// Delete an Invoice
        /// </summary>
        /// <param name="sDeleteInvoice"></param>
        /// <returns></returns>
        public string DeleteInvoice(string sInvoiceNum)
        {
            string sSQL = "DELETE * FROM Invoices WHERE invoiceNum = " + sInvoiceNum;
            return sSQL;
        }

        /// <summary>
        /// deletes all items from LineItems with associated invoice number
        /// </summary>
        /// <param name="sInvoiceNum"></param>
        /// <returns></returns>
        public string DeleteLineItems(string sInvoiceNum) {
            string sSQL = "DELETE * FROM LineItems WHERE `invoiceNum` = " + sInvoiceNum;
            return sSQL;
        }


        /// <summary>
        /// inserts record into Invoice table
        /// </summary>
        /// <param name="invoiceDate"></param>
        /// <param name="totalCharge"></param>
        /// <returns></returns>
        public String addInvoice(String invoiceDate, String totalCharge) { //DATE TO BE IN FORMAT MM/DD/YYY
            String SQL = "INSERT INTO Invoices ( InvoiceDate, TotalCharge) VALUES ( #" + invoiceDate + "#, " + totalCharge + " );";

            return SQL;
        }

        /// <summary>
        /// inserts record into LineItems
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="lineItemNum"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public string addLineItem(String invoiceNum, String lineItemNum, String itemCode) {
            String SQL = "INSERT INTO LineItems ( InvoiceNum, LineItemNum, ItemCode) VALUES (" + invoiceNum +", " + lineItemNum + ", " + itemCode + " );";

            return SQL;
        }


        public String updateInvoiceDate(String date, String invoiceNum) { //DATE TO BE IN FORMAT MM/DD/YYY
            String SQL = "UPDATE Invoices " +
                         "SET `InvoiceDate` = #" + date + "# " +
                         "WHERE `InvoiceNum` = " + invoiceNum + ";";

            return SQL;
        }

        public String updateTotalCharge(String cost, String invoiceNum) {
            String SQL = "UPDATE Invoices " +
                         "SET `TotalCharge` = " + cost +
                         " WHERE `InvoiceNum` = " + invoiceNum + ";";

            return SQL;
        }


        /// <summary>
        /// Gets latest invoice entered.
        /// </summary>
        /// <returns></returns>
        public String latestInvoice() {
            String SQL = "select max(InvoiceNum) from invoices";
            return SQL;
        }




        // <summary>
        /// SQL query to get all the invoice #'s by date ***ADDED BY Martha
        /// </summary>
        /// <returns></returns>
        public string invoiceWithDate(String sDate)
        {
            string sSQL = "SELECT Invoices.InvoiceNum, Invoices.InvoiceDate "
                           + "FROM ItemDesc INNER JOIN (Invoices INNER JOIN LineItems ON Invoices.InvoiceNum = LineItems.InvoiceNum) ON ItemDesc.ItemCode = LineItems.ItemCode "
                           + "WHERE(((Invoices.InvoiceDate) =#" + sDate + "#))";
            return sSQL;
        }

        /// <summary>
        /// Method to Generate the SQL statement to select the
        /// date of an invoice
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public string SelectInvoiceDate2(string sDate) {
            string sSQL = "SELECT * FROM Invoices WHERE InvoiceDate = #" + sDate + "#";

            return sSQL;
        }


    }//end class
}//end namespace
