using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection; 

namespace FinalProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        /// <summary>
        /// Create an object of type clsDataAccess to access the database
        /// </summary>
        clsDataAccess db = new clsDataAccess();
        clsSQL mydb = new clsSQL();
        String invoiceId;
        Dictionary<String, String> inventoryDictionary;

        DataTable dtInventory, dtInvoice = new System.Data.DataTable();

        /// <summary>
        /// searchWindow object ***ADDED by Martha
        /// </summary>
        SearchWindow searchWin;

        /// <summary>
        /// used to keep get InvoiceID ***ADDED BY Martha
        /// </summary>
        String sInvoiceNum;

        public MainWindow() {
            InitializeComponent();
            invoiceDatePicker.SelectedDate = DateTime.Now.Date;
            inventoryDictionary = new Dictionary<String, String>();
            //this.invoiceId = clsUtil.invoiceId;
            invoiceId = ""; //set to empty string. Methods below check this condition
            populateInventory();
            populateInvoice(invoiceId);//this is needed to set up the table  so we can add inventory items
            
            calculateTotal();
        }

        /// <summary>
        /// Search Window Click ***EDITED BY Martha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Window_Click(object sender, RoutedEventArgs e) {
            try
            {
                //instantiate search window
                searchWin = new SearchWindow();

                //launches the searchWindow to find and retrieve InvoiceID
                searchWin.ShowDialog();

                //sets the returned InvoiceID
                sInvoiceNum = searchWin.sInvoiceNum;
                Console.WriteLine("InvoiceID = " + sInvoiceNum);

                invoiceId = sInvoiceNum;
                populateInvoice(invoiceId);
                calculateTotal();
                searchWin.Close();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }//end search click

        /// <summary>
        /// Handle the error ***ADDED BY Martha
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }


        /// <summary>
        /// opens edit inventory window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInventory_Click(object sender, RoutedEventArgs e) {
            // Creates an EditWindow object
            EditWindow wndEdit = new EditWindow();
            // Shows the edit window
            wndEdit.Show();
            // Closes the main window
            this.Close();
        }//end inventory click


        /// <summary>
        /// gets the inventory data from the database.
        /// I am also creating a dictionary to refer back to. I use this in the Add/Update click event.
        /// This prevents additional reads to the database to retrieve the ItemCode.
        /// </summary>
        public void populateInventory() {
            //get data from database and load into dgInventoryItems
            //Method should be ran in initialize method

            String sQuery = mydb.SelectInventoryItems();
            dtInventory = db.FillSqlDataTable(sQuery);

            dgInventoryItems.ItemsSource = dtInventory.DefaultView;

            foreach (DataRow row in dtInventory.Rows) {
                inventoryDictionary.Add(row[1].ToString(), row[0].ToString());
            }

        }//end populateInventory()


        /// <summary>
        /// If there is an invoice number, get the data from the database and populate the datatable and dataset.
        /// If there is not in invoice given, set up the table to be able to accept inventory items.
        /// </summary>
        /// <param name="invoiceId"></param>
        public void populateInvoice(String invoiceId) {
            if (invoiceId != "") {
                btnDeleteInvoice.IsEnabled = true;
                lblInvoiceNumber.Content = invoiceId;

                String sQuery = mydb.SelectItemsOnInvoice(invoiceId);
                dtInvoice = db.FillSqlDataTable(sQuery);

                dgInvoiceItems.ItemsSource = dtInvoice.DefaultView;

                int iRet = 0;
                DataSet ds = new DataSet();
                String sSQL = mydb.SelectInvoiceDateFromNum(invoiceId);
                ds = db.ExecuteSQLStatement(sSQL, ref iRet);
                String date = ds.Tables[0].Rows[0][0].ToString();

                invoiceDatePicker.SelectedDate = DateTime.Parse(date);


                //pull data from database
                //UPDATE INVOICE LABEL WITH INVOICEID - lblInvoiceId
            } else {
                btnDeleteInvoice.IsEnabled = false;
                //fields should be clear and ready for input.
                dtInvoice.Columns.Add("ItemDesc");
                dtInvoice.Columns.Add("Cost");

                dgInvoiceItems.ItemsSource = dtInvoice.DefaultView;
            }


        }//end populateInvoice()


        /// <summary>
        /// adds the selected inventory item to the invoice datatable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInventory_Click(object sender, RoutedEventArgs e) {

            DataRowView dataRow = (DataRowView)dgInventoryItems.SelectedItem;
            //int index = dgInventoryItems.CurrentCell.Column.DisplayIndex;
            string item = dataRow.Row.ItemArray[1].ToString();
            string cost = dataRow.Row.ItemArray[2].ToString();
            DataRow dr = dtInvoice.NewRow();
            dr[0] = item;
            dr[1] = cost;
            dtInvoice.Rows.Add(dr);
            calculateTotal();
            //dgInvoiceItems.Items.Add(new Item {item = dataRow.Row.ItemArray[0].ToString(), price = dataRow.Row.ItemArray[1].ToString() });
        }

        /// <summary>
        /// removes selected index of the invoice datatable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e) {

            int index = dgInvoiceItems.SelectedIndex;
            dtInvoice.Rows.RemoveAt(index);

            calculateTotal();
        }

        /// <summary>
        /// if given invoice id Updates the date and total charge. delete everything in the LineItem table, and write the data again with the data in the datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e) {

            if (invoiceDatePicker.SelectedDate == null) { invoiceDatePicker.SelectedDate = DateTime.Now.Date; }// if no date is picked, default to today
            int count = 0;//keeps track of records inserted
            if (invoiceId != "") {

                //TODO Figure out why this isn't writing
                //updates the date, even if there were no changes. 
                String sSQL = mydb.updateInvoiceDate(invoiceDatePicker.SelectedDate.Value.ToShortDateString(), invoiceId);
                db.ExecuteNonQuery(sSQL);
                System.Console.WriteLine(sSQL);

                //updates the cost of the associated invoiceId
                sSQL = mydb.updateTotalCharge(calculateTotal() + "", invoiceId);
                db.ExecuteNonQuery(sSQL);

                System.Console.WriteLine(sSQL);

                //removing all LineItems associated with that invoice number, and adding them again with the added/removed items
                sSQL = mydb.DeleteLineItems(invoiceId);
                db.ExecuteNonQuery(sSQL);

                
                //grabs the data from the DataTable, runs a sql statement adding each line individually.
                for (int i = 0; i < dtInvoice.Rows.Count; i++) {
                    sSQL = mydb.addLineItem(invoiceId, i + 1 + "", inventoryDictionary[dtInvoice.Rows[i][0] + ""]);
                    System.Console.WriteLine(sSQL);
                    db.ExecuteNonQuery(sSQL);
                    count++;
                }

                MessageBox.Show("Invoice: " + invoiceId + " added successfully!\n" + count + " items added");


            } else {
                //TODO Figure out why this isn't writing
                /*
                 * DO I USE ExecuteScalarSQL   or    ExecuteNonQuery
                 * Code here for adding to database when no InvoiceId was given.
                 */

                String sSQL = mydb.addInvoice(invoiceDatePicker.SelectedDate.Value.ToShortDateString(), calculateTotal() + "");
                db.ExecuteNonQuery(sSQL);
                sSQL = mydb.latestInvoice();
                invoiceId = db.ExecuteScalarSQL(sSQL);

                for (int i = 0; i < dtInvoice.Rows.Count; i++) {
                    sSQL = mydb.addLineItem(invoiceId, i + 1 + "", inventoryDictionary[dtInvoice.Rows[i][0] + ""]);
                    System.Console.WriteLine(sSQL);
                    db.ExecuteNonQuery(sSQL);
                    count++;//keeps track of items added.
                }

                MessageBox.Show("Invoice: " + invoiceId + " added successfully!\n" + count + " items added");


            }//end else

        }//end add/update click

        private void btnDeleteInvoice_Click(object sender, RoutedEventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete Invoice Number: "+invoiceId +"?", "Delete Invoice?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) {
                //do no stuff
            } else {
                String sSQL = mydb.DeleteLineItems(invoiceId);
                db.ExecuteNonQuery(sSQL);

                sSQL = mydb.DeleteInvoice(invoiceId);
                db.ExecuteNonQuery(sSQL);
                
                //easiest way to reset all values is to open a new window.
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
                
                
            }
        }


        /// <summary>
        /// calculates the total by getting the cost row in the invoice data table, and adding them together.
        /// </summary>
        /// <returns></returns>
        public double calculateTotal() {
            Double total = 0.00;
            if (dtInvoice != null) {
                try {
                    foreach (DataRow row in dtInvoice.Rows) {
                        total += Double.Parse(row[1].ToString());
                    }
                } catch (Exception) {
                    MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
                }
            }

            lblTotal.Content = "$" + total;
            return total;
        }//end calculate total()



    }//end class

}//end namespace
