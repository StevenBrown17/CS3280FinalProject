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

        /// <summary>
        /// create a variable name for the sql class
        /// </summary>
        clsSQL mydb = new clsSQL();

        /// <summary>
        /// create a variable name for invoiceNum
        /// </summary>
        String invoiceId;

        /// <summary>
        /// dictionary for inventory
        /// </summary>
        Dictionary<String, String> inventoryDictionary;

        /// <summary>
        /// data table for inventory
        /// </summary>
        DataTable dtInventory, dtInvoice = new System.Data.DataTable();
        
        /// <summary>
        /// searchWindow object ***ADDED by Martha
        /// </summary>
        SearchWindow searchWin;

        /// <summary>
        /// used to keep get InvoiceID ***ADDED BY Martha
        /// </summary>
        String sInvoiceNum;

        /// <summary>
        /// main window intialization
        /// </summary>
        public MainWindow() {

            ///initialize the window
            InitializeComponent();

            ///invoice picker method
            invoiceDatePicker.SelectedDate = DateTime.Now.Date;

            ///inventory dictiony
            inventoryDictionary = new Dictionary<String, String>();

            //this.invoiceId = clsUtil.invoiceId;
            invoiceId = ""; //set to empty string. Methods below check this condition

            ///populate the inventory grid
            populateInventory();

            ///this is needed to set up the table  so we can add inventory items
            populateInvoice(invoiceId);
            
            ///calulate total variable
            calculateTotal();
        }

        /// <summary>
        /// Search Window Click ***EDITED BY Martha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Window_Click(object sender, RoutedEventArgs e) {
            ///try catch block to catch errors
            try
            {
                ///instantiate search window
                searchWin = new SearchWindow();

                ///launches the searchWindow to find and retrieve InvoiceID
                searchWin.ShowDialog();

                ///sets the returned InvoiceID
                sInvoiceNum = searchWin.sInvoiceNum;

                ///write the invoice ID 
                Console.WriteLine("InvoiceID = " + sInvoiceNum);

                ///invoice id takes in invoiceNum
                invoiceId = sInvoiceNum;

                ///populates the invoice grid with user selected invoice
                populateInvoice(invoiceId);

                ///calculate total method
                calculateTotal();

                ///close search window
                searchWin.Close();
            }
            catch (Exception)///catch exceptions
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }

        }//end search click


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
            ///try catch block to handle exceptions
            try
            {
                //get data from database and load into dgInventoryItems
                //Method should be ran in initialize method

                ///sQuery for inventory items
                String sQuery = mydb.SelectInventoryItems();
                ///database query
                dtInventory = db.FillSqlDataTable(sQuery);
                ///inventory items set to default view 
                dgInventoryItems.ItemsSource = dtInventory.DefaultView;
                ///foreach loop to populate the grid
                foreach (DataRow row in dtInventory.Rows)
                {
                    inventoryDictionary.Add(row[1].ToString(), row[0].ToString());
                }
            }
            catch (Exception)///catch exceptions
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }

        }//end populateInventory()


        /// <summary>
        /// If there is an invoice number, get the data from the database and populate the datatable and dataset.
        /// If there is not in invoice given, set up the table to be able to accept inventory items.
        /// </summary>
        /// <param name="invoiceId"></param>
        public void populateInvoice(String invoiceId) {
            ///try catch block to handle exceptions
            try
            {
                ///if statement to check invoice ID and populate invoice
                if (invoiceId != "")
                {
                    ///set delete invoice to enabled
                    btnDeleteInvoice.IsEnabled = true;
                    ///set label with number
                    lblInvoiceNumber.Content = invoiceId;
                    ///create a string to hold invoice id info
                    String sQuery = mydb.SelectItemsOnInvoice(invoiceId);
                    ///execute query
                    dtInvoice = db.FillSqlDataTable(sQuery);
                    ///populate grid
                    dgInvoiceItems.ItemsSource = dtInvoice.DefaultView;
                    ///create a return variable
                    int iRet = 0;
                    ///create a data set
                    DataSet ds = new DataSet();
                    ///call a sql method
                    String sSQL = mydb.SelectInvoiceDateFromNum(invoiceId);
                    ///place info in data set
                    ds = db.ExecuteSQLStatement(sSQL, ref iRet);
                    ///create temp variable to hold info
                    String date = ds.Tables[0].Rows[0][0].ToString();
                    ///take care of the date format
                    invoiceDatePicker.SelectedDate = DateTime.Parse(date);


                    //pull data from database
                    //UPDATE INVOICE LABEL WITH INVOICEID - lblInvoiceId
                }
                else
                {
                    ///if all that fails than do this
                    btnDeleteInvoice.IsEnabled = false;
                    //fields should be clear and ready for input.
                    dtInvoice.Columns.Add("ItemDesc");
                    dtInvoice.Columns.Add("Cost");
                    ///populate invoice items
                    dgInvoiceItems.ItemsSource = dtInvoice.DefaultView;
                }
            }
            catch (Exception)///catch exceptions
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }

        }//end populateInvoice()


        /// <summary>
        /// adds the selected inventory item to the invoice datatable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInventory_Click(object sender, RoutedEventArgs e) {
            ///try catch block
            try
            {
                ///create datarow for inventory items
                DataRowView dataRow = (DataRowView)dgInventoryItems.SelectedItem;
                //int index = dgInventoryItems.CurrentCell.Column.DisplayIndex;
                ///this below capture certian cells of info
                string item = dataRow.Row.ItemArray[1].ToString();
                string cost = dataRow.Row.ItemArray[2].ToString();
                DataRow dr = dtInvoice.NewRow();
                dr[0] = item;
                dr[1] = cost;
                dtInvoice.Rows.Add(dr);
                calculateTotal();
                //dgInvoiceItems.Items.Add(new Item {item = dataRow.Row.ItemArray[0].ToString(), price = dataRow.Row.ItemArray[1].ToString() });
            }
            catch (Exception)///catch exception
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// removes selected index of the invoice datatable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e) {
            ///creates varibale index to remove items
            int index = dgInvoiceItems.SelectedIndex;
            ///calls remove function
            dtInvoice.Rows.RemoveAt(index);
            ///calls calculate total method
            calculateTotal();
        }

        /// <summary>
        /// if given invoice id Updates the date and total charge. delete everything in the LineItem table, and write the data again with the data in the datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e) {
            try
            {
                ///if statment to check and compare dates
                if (invoiceDatePicker.SelectedDate == null) { invoiceDatePicker.SelectedDate = DateTime.Now.Date; }// if no date is picked, default to today
                int count = 0;//keeps track of records inserted
                ///if invoice id is null
                if (invoiceId != "")
                {

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
                    for (int i = 0; i < dtInvoice.Rows.Count; i++)
                    {
                        sSQL = mydb.addLineItem(invoiceId, i + 1 + "", inventoryDictionary[dtInvoice.Rows[i][0] + ""]);
                        System.Console.WriteLine(sSQL);
                        db.ExecuteNonQuery(sSQL);
                        count++;
                    }

                    MessageBox.Show("Invoice: " + invoiceId + " added successfully!\n" + count + " items added");


                }
                else
                {
                    //TODO Figure out why this isn't writing
                    /*
                     * DO I USE ExecuteScalarSQL   or    ExecuteNonQuery
                     * Code here for adding to database when no InvoiceId was given.
                     */

                    String sSQL = mydb.addInvoice(invoiceDatePicker.SelectedDate.Value.ToShortDateString(), calculateTotal() + "");
                    db.ExecuteNonQuery(sSQL);
                    sSQL = mydb.latestInvoice();
                    invoiceId = db.ExecuteScalarSQL(sSQL);

                    for (int i = 0; i < dtInvoice.Rows.Count; i++)
                    {
                        sSQL = mydb.addLineItem(invoiceId, i + 1 + "", inventoryDictionary[dtInvoice.Rows[i][0] + ""]);
                        System.Console.WriteLine(sSQL);
                        db.ExecuteNonQuery(sSQL);
                        count++;//keeps track of items added.
                    }

                    MessageBox.Show("Invoice: " + invoiceId + " added successfully!\n" + count + " items added");


                }//end else
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }

        }//end add/update click

        /// <summary>
        /// delete invoice button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteInvoice_Click(object sender, RoutedEventArgs e) {
            ///try catch block
            try
            {
                ///check to see what the message box is showing
                if (MessageBox.Show("Are you sure you want to delete Invoice Number: " + invoiceId + "?", "Delete Invoice?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    ///if that fails do this
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
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }


        /// <summary>
        /// calculates the total by getting the cost row in the invoice data table, and adding them together.
        /// </summary>
        /// <returns></returns>
        public double calculateTotal() {
            ///created a double to hold total
            Double total = 0.00;
            ///if invoice is not null execute the calculate total
            if (dtInvoice != null) {
                try {
                    foreach (DataRow row in dtInvoice.Rows) {
                        total += Double.Parse(row[1].ToString());
                    }
                } catch (Exception) {
                    MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
                }
            }
            ///update lable
            lblTotal.Content = "$" + total;
            ///return total
            return total;
        }//end calculate total()

    }//end class

}//end namespace
