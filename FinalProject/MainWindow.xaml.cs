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

        DataTable dt;

        public MainWindow() {
            InitializeComponent();
            this.invoiceId = clsUtil.invoiceId;
            populateInvoice(invoiceId);
            populateInventory();
        }

        private void Search_Window_Click(object sender, RoutedEventArgs e) {
            ///Create a new varible for the Search Window
            SearchWindow searchWin = new SearchWindow();
            ///Call it to show
            searchWin.Show();
            ///Close the main window
            this.Close();

        }//end search click


        private void btnInventory_Click(object sender, RoutedEventArgs e) {
            // Creates an EditWindow object
            EditWindow wndEdit = new EditWindow();
            // Shows the edit window
            wndEdit.Show();
            // Closes the main window
            this.Close();
        }//end inventory click


        //method to populate inventory
        public void populateInventory() {
            //get data from database and load into dgInventoryItems
            //Method should be ran in initialize method
        }//end populateInventory()

        //method to populate invoice
        public void populateInvoice(String invoiceId) {
            if(invoiceId != "") {
                //pull data from database
                //UPDATE INVOICE LABEL WITH INVOICEID - lblInvoiceId
            } else {
                //fields should be clear and ready for input.
            }
            
        }//end populateInvoice()

        public void checkDate() {
            //check to see if there is a date in the date picker.
            //if there is not a date selected, prevent user from adding/updating.
        }//end checkDate




    }//end class
}//end namespace
