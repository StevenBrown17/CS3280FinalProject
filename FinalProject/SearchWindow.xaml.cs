using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.Windows.Navigation;
using System.Configuration;


/// <summary>
/// search window
/// </summary>
namespace FinalProject {

    /// <summary>
    /// Interaction logic for SearchWindow.xaml By Martha Canizales
    /// </summary>
    public partial class SearchWindow : Window {

        /// <summary>
        /// User drop down menu selected variables
        /// </summary>
        public String sInvoiceNum;

        /// <summary>
        /// User drop down menu selected variables
        /// </summary>
        public String sDate;

        /// <summary>
        /// Create an object of type clsDataAccess to access the database
        /// </summary>
        clsDataAccess db = new clsDataAccess();

        /// <summary>
        /// bringing in the sql class
        /// </summary>
        clsSQL mydb = new clsSQL();

        /// <summary>
        /// datatable variable name
        /// </summary>
        DataTable dt;

        public SearchWindow()
        {
            ///initialize components
            InitializeComponent();
            ///populate invoices
            populateInvoices();

            ///////////////////////////////////////////////////////////////////////////
            ///automatically load drop down menu for Invoice ID
            try
            {
                //Create a DataSet to hold the data
                DataSet ds;

                //Number of return values
                int iRet = 0;

                //Get all the values from the Invoice ID table
                ds = db.ExecuteSQLStatement("SELECT InvoiceNum FROM Invoices", ref iRet);

                //Loop through all the values returned
                for (int i = 0; i < iRet; i++)
                {
                    //Add InvoiceID's to the dropdown box
                    invoiceIDComboBox.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
            ////////////////////////////////////////////////////////////////////////////
            ///automatically load drop down menu for Invoice Date
            try
            {
                ///Create a DataSet to hold the data
                DataSet ds;

                ///Number of return values
                int iRet = 0;

                ///Get all the values from the Invoice Date table
                ds = db.ExecuteSQLStatement("SELECT InvoiceDate FROM Invoices", ref iRet);

                ///Loop through all the values returned
                for (int i = 0; i < iRet; i++)
                {
                    ///Add Invoice Dates to the dropdown box

                    invoiceDateComboBox.Items.Add(ds.Tables[0].Rows[i][0]).ToString();
                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
            //////////////////////////////////////////////////////////////////////////////
            ///automatically load drop down menu for Invoice Total Charge
            try
            {
                //Create a DataSet to hold the Total Charge
                DataSet ds;

                //Number of return values
                int iRet = 0;

                //Get all the values from the Invoice Total Charge
                ds = db.ExecuteSQLStatement("SELECT TotalCharge FROM Invoices", ref iRet);

                //Loop through all the values returned
                for (int i = 0; i < iRet; i++)
                {
                    //Add Invoice total Charge to the dropdown box
                    invoiceAmountComboBox.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }


        /// <summary>
        /// select button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            ///When the user presses the "Select" button, the "Search Window" closes
            ///and the main form opens displaying the selected form

            ///The selected form will be captured and passed to the main form with the sInvoiceNum variable
            ///which is the variable of the "user" selected invoice

            if (invoiceGrid1.SelectedItem != null) {
                DataRowView dataRow = (DataRowView)invoiceGrid1.SelectedItem;
                sInvoiceNum = dataRow.Row.ItemArray[0].ToString();
                ///Close the main window
                this.Hide();
            }
            
        }

        /// <summary>
        /// clear button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///The clear button will repopulate the gridbox to it's original state showing
                ///all available invoices in list form

                ///It will also reset the drop down menus (combo boxes) back to it's null default. 

                ///Close the main window
                this.Close();
                ///Create a new variable for the Main Window
                SearchWindow Search = new SearchWindow();
                ///Call it to show
                Search.Show();
            }
            catch { }

        }

        /// <summary>
        /// invoice id fill combo box, fills grid with user selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoiceIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ///Disable other drop down menu's if this is selected
                if (invoiceIDComboBox.IsEnabled)
                {
                    invoiceDateComboBox.IsEnabled = false;
                    invoiceAmountComboBox.IsEnabled = false;
                }

                ///The two other drop down menu's will be disabled when this is being used

                ///When selected this combo box will show a drop down list with invoice ID's
                ///The user will be able to highlight/select an ID and it will be received
                ///by the dataGrid1 to show that specific invoice only
                Console.WriteLine("textbox Value: " + invoiceIDComboBox.SelectedItem);


                //get query needed find invoice
                String sQuery = mydb.SelectInvoiceID(invoiceIDComboBox.SelectedItem.ToString());

                //datatable used to get table data. 
                dt = db.FillSqlDataTable(sQuery);

                //fill the datagrid
                invoiceGrid1.ItemsSource = dt.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// invoice date fills combo box, fills grid with user selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoiceDateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                sDate = invoiceDateComboBox.Text;
                ///Disable other drop down menu's if this is selected
                if (invoiceDateComboBox.IsEnabled)
                {
                    invoiceIDComboBox.IsEnabled = false;
                    invoiceAmountComboBox.IsEnabled = false;
                }
                ///The two other drop down menu's will be disabled when this is being used

                ///When selected, this combo box will show a drop down list with invoice Dates
                ///The user will be able to highlight/select a date and it will be received
                ///by the dataGrid1 to show that specific invoice only
                ///

                String selectedItem = invoiceDateComboBox.SelectedItem.ToString();
                Console.WriteLine("Searching Date Value before: " + selectedItem);


                //remove timestamps from selectedItem
                var newSelItem = selectedItem.Split(' ')[0];
                Console.WriteLine("Searching Date Value After: " + newSelItem);

                //get query needed find invoice
                String query = mydb.SelectInvoiceDate2(newSelItem);
                Console.WriteLine(query);
                String sQuery = mydb.SelectInvoiceDate2(invoiceDateComboBox.SelectedItem.ToString());

                //datatable used to get table data. 
                dt = db.FillSqlDataTable(sQuery);

                //fill the datagrid
                invoiceGrid1.ItemsSource = dt.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }

        }

        /// <summary>
        /// invoice amount fills combo box, fills grid with user selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoiceAmountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ///Disable other drop down menu's if this is selected
                if (invoiceAmountComboBox.IsEnabled)
                {
                    invoiceIDComboBox.IsEnabled = false;
                    invoiceDateComboBox.IsEnabled = false;
                }

                ///The two other drop down menu's will be disabled when this is being used

                ///When selected, this combo box will show a drop down list with invoice Total Charge
                ///The user will be able to highlight/select an TotalCharge and it will be received
                ///by the dataGrid1 to show that specific invoice only
                ///

                Console.Write("textbox Value: " + invoiceAmountComboBox.SelectedItem.ToString());

                //get query needed find invoice
                String sQuery = mydb.SelectTotalCharge(invoiceAmountComboBox.SelectedItem.ToString());

                //datatable used to get table data. 
                dt = db.FillSqlDataTable(sQuery);

                //fill the datagrid
                invoiceGrid1.ItemsSource = dt.DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }

        }

        /// <summary>
        /// this is useless but messed stuff up if I get rid of it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invoiceGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///This data grid by default will call in an sql command to populate it with
            ///a list of all Invoices available

            ///If the user chooses to use any of the drop down menu's
            ///It will then repopulate any selections made by the drop down menu's and populate
            ///the dataGrid1 with the highlighted InvoiceNum, InvoiceDate, or TotalCharge
        }

        /// <summary>
        /// gets the inventory data from the database.
        /// I am also creating a dictionary to refer back to. I use this in the Add/Update click event.
        /// This prevents additional reads to the database to retrieve the ItemCode.
        /// </summary>
        public void populateInvoices()
        {
            try
            {
                //get data from database and load into datagrid1
                //Method should be ran in initialize method

                //get query needed find invoice
                String sQuery = mydb.populateAllInvoices();

                //datatable used to get table data. 
                dt = db.FillSqlDataTable(sQuery);

                //fill the datagrid
                invoiceGrid1.ItemsSource = dt.DefaultView;
            }
            catch { }

        }//end populateInventory()
    }
}
