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
        /// Create an object of type clsDataAccess to access the database
        /// </summary>
        clsDataAccess db = new clsDataAccess();
        clsSQL mydb = new clsSQL();

        DataTable dt;

        public SearchWindow()
        {
            InitializeComponent();

            //automatically load drop down menu for Invoice ID
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


        private void selectButton_Click(object sender, RoutedEventArgs e)
        {
            ///When the user presses the "Select" button, the "Search Window" closes
            ///and the main form opens displaying the selected form

            ///The selected form will be captured and passed to the main form with the sInvoiceNum variable
            ///which is the variable of the "user" selected invoice

            ///Close the main window
            this.Hide();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            ///The clear button will repopulate the gridbox to it's original state showing
            ///all available invoices in list form

            ///It will also reset the drop down menus (combo boxes) back to it's null default. 

            ///Create a new varible for the Main Window
            SearchWindow Search = new SearchWindow();
            ///Call it to show
            Search.Show();
            ///Close the main window
            this.Close();

        }

        private void invoiceIDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sInvoiceNum = invoiceIDComboBox.Text;
            ///Disable other drop down menu's if this is selected
            if (invoiceIDComboBox.IsEnabled)
            {
                invoiceDateComboBox.IsEnabled = false;
                invoiceAmountComboBox.IsEnabled = false;


            }

            ///The two other drop down menu's will be disbled when this is being used

            ///When selected this combo box will show a drop down list with invoice ID's
            ///The user will be able to highlight/select an ID and it will be recieved
            ///by the dataGrid1 to show that specific invoice only
            Console.WriteLine("textbox Value: " + invoiceIDComboBox.SelectedItem);
            sInvoiceNum = invoiceIDComboBox.SelectedItem.ToString();

            //get query needed find invoice
            String sQuery = mydb.SelectInvoiceID(invoiceIDComboBox.SelectedItem.ToString());

            //datatable used to fget table data. 
            dt = db.FillSqlDataTable(sQuery);

            //fill the datagrid
            invoiceGrid1.ItemsSource = dt.DefaultView;
        }

        private void invoiceDateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Disable other drop down menu's if this is selected
            if (invoiceDateComboBox.IsEnabled)
            {
                invoiceIDComboBox.IsEnabled = false;
                invoiceAmountComboBox.IsEnabled = false;
            }
            ///The two other drop down menu's will be disabled when this is being used

            ///When selected, this combo box will show a drop down list with invoice Dates
            ///The user will be able to highlight/select a date and it will be recieved
            ///by the dataGrid1 to show that specific invoice only
            ///

            String selectedItem = invoiceDateComboBox.SelectedItem.ToString();
            Console.WriteLine("Searching Date Value before: " + selectedItem);


            //remove timestamp from selectedItem
            var newSelItem = selectedItem.Split(' ')[0];
            Console.WriteLine("Searching Date Value After: " + newSelItem);

            //get query needed find invoice
            String query = mydb.invoiceWithDate(newSelItem);
            Console.WriteLine(query);
            String sQuery = mydb.SelectInvoiceDate(invoiceDateComboBox.SelectedItem.ToString());

            //datatable used to fget table data. 
            dt = db.FillSqlDataTable(sQuery);

            //fill the datagrid
            invoiceGrid1.ItemsSource = dt.DefaultView;
        }

        private void invoiceAmountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Disable other drop down menu's if this is selected
            if (invoiceDateComboBox.IsEnabled)
            {
                invoiceIDComboBox.IsEnabled = false;
                invoiceDateComboBox.IsEnabled = false;
            }

            ///The two other drop down menu's will be disabled when this is being used

            ///When selcted, this combo box will show a drop down list with invoice Total Charge
            ///The user will be able to highlight/select an TotalCharge and it will be recieved
            ///by the dataGrid1 to show that specific invoice only
            ///

            Console.Write("textbox Value: " + invoiceAmountComboBox.SelectedItem.ToString());

            //get query needed find invoice
            String sQuery = mydb.SelectTotalCharge(invoiceAmountComboBox.SelectedItem.ToString());

            //datatable used to fget table data. 
            dt = db.FillSqlDataTable(sQuery);

            //fill the datagrid
            invoiceGrid1.ItemsSource = dt.DefaultView;

        }

        private void invoiceGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///This data grid by default will call in an sql command to populate it with
            ///a list of all Invoices available

            ///If the user chooses to use any of the drop down menu's
            ///It will then repopulate any selections made by the drop down menu's and populate
            ///the dataGrid1 with the highlighted InvoiceNum, InvoiceDate, or TotalCharge
        }
    }
}
