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
        /// <summary>
        /// Dictionary for inventory.
        /// </summary>
        Dictionary<string, string> inventoryDictionary;
        DataSet ds = new DataSet();
        /// <summary>
        /// Contains SQL statements.
        /// </summary>
        string sSQL;
        /// <summary>
        /// Returns the number of selected rows.
        /// </summary>
        int iRetVal = 0;
        /// <summary>
        /// Shows if (add/edit) is currently selected.
        /// </summary>
        string selectedFunction = "";
        /// <summary>
        /// Contains the current ItemCode.
        /// </summary>
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
            this.Close();
        }

        /// <summary>
        /// Populates datagrid with all items.
        /// </summary>
        private void populateDatagridInv()
        {
            try
            {
                // Gets all items from the database
                sSQL = mydb.SelectInventoryItems();
                dt = db.FillSqlDataTable(sSQL);

                dataGrid.ItemsSource = dt.DefaultView;

                // Inserts items into the datagrid.
                foreach (DataRow row in dt.Rows)
                {
                    inventoryDictionary.Add(row[1].ToString(), row[0].ToString());
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedFunction = "Add";
                DescriptionCostCanvas.Visibility = Visibility.Visible;
                lblErrorCantDeleteItem.Visibility = Visibility.Hidden;
                clear();

                // Make description and cost textboxes editable
                txtItemDesc.IsReadOnly = false;
                txtItemCost.IsReadOnly = false;

                // Disallows the user from using the datagrid and function buttons.
                dataGrid.IsEnabled = false;
                btnAddItem.IsEnabled = false;
                btnEditItem.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Edit an existing item in the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                selectedFunction = "Edit";

                DescriptionCostCanvas.Visibility = Visibility.Visible;
                lblErrorCantDeleteItem.Visibility = Visibility.Hidden;

                // Make description and cost textboxes editable
                txtItemDesc.IsReadOnly = false;
                txtItemCost.IsReadOnly = false;

                // Disallows the user from using the function buttons.
                btnAddItem.IsEnabled = false;
                btnEditItem.IsEnabled = false;
                btnDeleteItem.IsEnabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Delete an item from the inventory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Prevent user from deleting an item that is in the current invoice.
                // Display a warning message to the user.
                // Delete item from database using SQL.

                lblErrorCantDeleteItem.Visibility = Visibility.Hidden;

                sSQL = mydb.CheckIfItemIsInAnInvoice(itemCode);
                ds = db.ExecuteSQLStatement(sSQL, ref iRetVal);

                if(iRetVal == 0)
                {
                    ///check to see what the message box is showing
                    if (MessageBox.Show("Are you sure you want to delete item: " + txtItemDesc.Text + "?", "Delete item?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        //do no stuff
                    }
                    else
                    {
                        sSQL = mydb.DeleteInventoryItem(itemCode);
                        db.ExecuteNonQuery(sSQL);

                        EditWindow ew = new EditWindow();
                        ew.Show();
                        this.Close();
                    }
                }
                else
                {
                    lblErrorCantDeleteItem.Visibility = Visibility.Visible;
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
            try
            {
                // Populate item description textbox.
                if (dataGrid.SelectedIndex != -1)
                {
                    lblErrorCantDeleteItem.Visibility = Visibility.Hidden;

                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];

                    // Set item code.
                    itemCode = row[0].ToString();

                    // Populate item description textbox.
                    txtItemDesc.Text = row[1].ToString();

                    // Populate item cost textbox.
                    txtItemCost.Text = row[2].ToString();

                    if (selectedFunction == "")
                    {
                        btnDeleteItem.IsEnabled = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Calls clear(), then resets the screen to the default.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clear();
                
                DescriptionCostCanvas.Visibility = Visibility.Hidden;
                txtItemDesc.IsReadOnly = true;
                txtItemCost.IsReadOnly = true;
                dataGrid.IsEnabled = true;
                btnAddItem.IsEnabled = true;
                btnEditItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = false;
                selectedFunction = "";
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Resets everything to default value.
        /// </summary>
        private void clear()
        {
            try
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
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Saves all the changes made and places information into the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Executes the add/edit function.
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
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// Only allows numbers in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }

        /// <summary>
        /// If an item is being added or edited and there is text in both textboxes, it enables the save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (btnAddItem.IsEnabled == false || btnEditItem.IsEnabled == false)
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
            catch (Exception)
            {
                MessageBox.Show(MethodInfo.GetCurrentMethod().DeclaringType.Name);
            }
        }
    }
}
