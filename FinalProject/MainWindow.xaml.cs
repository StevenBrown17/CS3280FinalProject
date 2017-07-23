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

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Create an object of type clsDataAccess to access the database
        /// </summary>
        clsDataAccess db = new clsDataAccess();
        clsSQL mydb = new clsSQL();

        DataTable dt;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Search_Window_Click(object sender, RoutedEventArgs e)
        {
            ///Create a new varible for the Search Window
            SearchWindow searchWin = new SearchWindow();
            ///Call it to show
            searchWin.Show();
            ///Close the main window
            this.Close();

        }

        private void invoiceDataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchWindow search = new SearchWindow();
           
            Console.Write("textbox Value: " + search.invoiceGrid1.SelectedItem);

            //get query needed find invoice
            String sQuery2= mydb.SelectInvoiceID(search.invoiceGrid1.SelectedItem.ToString());

            //datatable used to fget table data. 
            dt = db.FillSqlDataTable(sQuery2);

            //fill the datagrid
            invoiceDataGrid2.ItemsSource = dt.DefaultView;
        }
    }
}
