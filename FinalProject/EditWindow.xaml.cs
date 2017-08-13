using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
        Dictionary<string, string> inventoryDictionary;
        string sSQL;
        string selectedFunction;
        string itemCode;

        /// <summary>
        /// edit window
        /// </summary>
        public EditWindow()
        {
            InitializeComponent();
            inventoryDictionary = new Dictionary<string, string>();
            populateDatagridInv();
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

        private void populateDatagridInv()
        {
            sSQL = mydb.SelectInventoryItems();
            dt = db.FillSqlDataTable(sSQL);

            dataGrid.ItemsSource = dt.DefaultView;

            foreach(DataRow row in dt.Rows)
            {
                inventoryDictionary.Add(row[1].ToString(), row[0].ToString());
            }
        }

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            selectedFunction = "Add";
            DescriptionCostCanvas.Visibility = Visibility.Visible;
            clear();

            // Make description and cost textboxes editable
            txtItemDesc.IsReadOnly = false;
            txtItemCost.IsReadOnly = false;
            dataGrid.IsEnabled = false;
            btnAddItem.IsEnabled = false;
            btnEditItem.IsEnabled = false;
            btnDeleteItem.IsEnabled = false;
        }

        /// <summary>
        /// Edit an existing item in the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            selectedFunction = "Edit";
            // When an item is updated, only update description and cost. This will be done through SQL.

            DescriptionCostCanvas.Visibility = Visibility.Visible;

            txtItemDesc.IsReadOnly = false;
            txtItemCost.IsReadOnly = false;
            btnAddItem.IsEnabled = false;
            btnEditItem.IsEnabled = false;
            btnDeleteItem.IsEnabled = false;
        }

        /// <summary>
        /// Delete an item from the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            selectedFunction = "Delete";
            // Prevent user from deleting an item that is in the current invoice.
            // Display a warning message to the user.
            // Delete item from database using SQL.

            /*btnAddItem.IsEnabled = false;
            btnEditItem.IsEnabled = false;
            btnDeleteItem.IsEnabled = false;
            btnSave.IsEnabled = true;*/

            try
            {
                ///check to see what the message box is showing
                if (MessageBox.Show("Are you sure you want to delete item: " + txtItemDesc.Text + "?", "Delete item?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    //do no stuff
                }
                else
                {
                    ///if that fails do this
                    /*String sSQL = mydb.DeleteLineItems(invoiceId);
                    db.ExecuteNonQuery(sSQL);

                    sSQL = mydb.DeleteInvoice(invoiceId);
                    db.ExecuteNonQuery(sSQL);

                    //easiest way to reset all values is to open a new window.
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();*/
                    sSQL = mydb.DeleteInventoryItem(itemCode);
                    db.ExecuteNonQuery(sSQL);

                    EditWindow ew = new EditWindow();
                    ew.Show();
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Populates textboxes with the correct info once any rows are selected in the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Populate item description textbox.
            if(dataGrid.SelectedIndex != -1)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];

                // Set item code.
                itemCode = row[0].ToString();

                // Populate item description textbox.
                txtItemDesc.Text = row[1].ToString();

                // Populate item cost textbox.
                txtItemCost.Text = row[2].ToString();

                btnDeleteItem.IsEnabled = true;
            }
        }

        /// <summary>
        /// Calls clear(), then resets the screen to the default.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clear();

            DescriptionCostCanvas.Visibility = Visibility.Hidden;
            txtItemDesc.IsReadOnly = true;
            txtItemCost.IsReadOnly = true;
            dataGrid.IsEnabled = true;
            btnAddItem.IsEnabled = true;
            btnEditItem.IsEnabled = true;
            btnDeleteItem.IsEnabled = false;
        }

        /// <summary>
        /// Resets everything to default value.
        /// </summary>
        private void clear()
        {
            // Deselects all cells.
            dataGrid.UnselectAllCells();

            // Removes all text from textboxes.
            txtItemDesc.Text = "";
            txtItemCost.Text = "";

            // Disables the save button and datagrid.
            btnSave.IsEnabled = false;
            dataGrid.IsEnabled = true;
        }

        /// <summary>
        /// Saves all the changes made and places information into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            switch (selectedFunction)
            {
                case "Add":
                    sSQL = mydb.AddInventoryItem(txtItemDesc.Text, txtItemCost.Text);
                    db.ExecuteNonQuery(sSQL);
                    break;
                case "Edit":
                    sSQL = mydb.EditInventoryItem(itemCode, txtItemDesc.Text, txtItemCost.Text);
                    db.ExecuteNonQuery(sSQL);
                    break;
                case "Delete":
                    sSQL = mydb.DeleteInventoryItem(itemCode);
                    db.ExecuteNonQuery(sSQL);
                    break;
                default:
                    break;
            }
            
            clear();

            txtItemDesc.IsReadOnly = true;
            txtItemCost.IsReadOnly = true;
            btnAddItem.IsEnabled = true;
            btnEditItem.IsEnabled = true;
            btnDeleteItem.IsEnabled = true;
            DescriptionCostCanvas.Visibility = Visibility.Hidden;

            inventoryDictionary = new Dictionary<string, string>();
            populateDatagridInv();
        }

        /// <summary>
        /// Only allows numbers in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// If an item is being added or edited and there is text in both textboxes, it enables the save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textChanged(object sender, TextChangedEventArgs e)
        {
            if(btnAddItem.IsEnabled == false || btnEditItem.IsEnabled == false)
            {
                if (txtItemDesc.Text != "" && txtItemCost.Text != "")
                {
                    btnSave.IsEnabled = true;
                }
                else
                {
                    btnSave.IsEnabled = false;
                }
            }
        }
    }
}
