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
using System.Windows.Shapes;

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        /// <summary>
        /// create a variable for the database
        /// </summary>
        clsDataAccess db = new clsDataAccess();
        /// <summary>
        /// create a variable for the sql class
        /// </summary>
        clsSQL mydb = new clsSQL();
        /// <summary>
        /// create a variable for a data table
        /// </summary>
        DataTable dt;

        /// <summary>
        /// edit window
        /// </summary>
        public EditWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// btn return to main click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReturnToMain_Click(object sender, RoutedEventArgs e)
        {
            // Creates a MainWindow object
            MainWindow wndMain = new MainWindow();
            // Shows the main window
            wndMain.Show();
            // Closes the edit window
            this.Close();
        }

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            // Create a SQL statement to insert a new item into the data base.
        }

        /// <summary>
        /// Edit an existing item in the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            // When an item is updated, only update description and cost. This will be done through SQL.
        }

        /// <summary>
        /// Delete an item from the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            // Prevent user from deleting an item that is in the current invoice.
                // Display a warning message to the user.
            // Delete item from database using SQL.
        }
    }
}
