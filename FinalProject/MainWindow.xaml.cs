﻿using System;
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
        Dictionary<String, String> inventoryDictionary;

        DataTable dtInventory, dtInvoice = new System.Data.DataTable();

        public MainWindow() {
            InitializeComponent();
            invoiceDatePicker.SelectedDate = DateTime.Now.Date;
            inventoryDictionary = new Dictionary<String, String>();
            //this.invoiceId = clsUtil.invoiceId;
            //TODO
            invoiceId = "5000"; ////!!!!REMOVE THIS CODE ONCE SEARCH WINDOW IS RETURNING A INVOICENUM!!!
            populateInvoice(invoiceId);

            populateInventory();
            calculateTotal();
        }

        private void Search_Window_Click(object sender, RoutedEventArgs e) {
            ///Create a new variable for the Search Window
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

            String sQuery = mydb.SelectInventoryItems();
            dtInventory = db.FillSqlDataTable(sQuery);

            dgInventoryItems.ItemsSource = dtInventory.DefaultView;

            foreach (DataRow row in dtInventory.Rows) {
                inventoryDictionary.Add(row[1].ToString(), row[0].ToString());
            }
            System.Console.WriteLine("Dictionary");

        }//end populateInventory()

        //method to populate invoice
        public void populateInvoice(String invoiceId) {
            if (invoiceId != "") {
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
                //fields should be clear and ready for input.
                dtInvoice.Columns.Add("ItemDesc");
                dtInvoice.Columns.Add("Cost");

                dgInvoiceItems.ItemsSource = dtInvoice.DefaultView;
            }


        }//end populateInvoice()


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

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e) {

            int index = dgInvoiceItems.SelectedIndex;
            dtInvoice.Rows.RemoveAt(index);

            calculateTotal();
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e) {
            DataSet ds = new DataSet();
            if(invoiceId != "") {

                int iRet=0;                
                String sSQL = mydb.updateDate(invoiceDatePicker.SelectedDate.Value.ToShortDateString(), invoiceId);
                //ds = db.ExecuteSQLStatement(sSQL, ref iRet);
                System.Console.WriteLine(sSQL);

                sSQL = mydb.updateTotalCost(calculateTotal() + "", invoiceId);
                //ds = db.ExecuteSQLStatement(sSQL, ref iRet);

                System.Console.WriteLine(sSQL);


                //foreach (DataRow dr in dtInvoice.Rows) {
                //    foreach (DataColumn col in dtInvoice.Columns) {
                //        Console.Write(dr[col.ColumnName] + " ");
                //    }
                //    Console.WriteLine();
                //}

                for (int i = 0; i < dtInvoice.Rows.Count; i++) {
                    sSQL = mydb.addLineItem(invoiceId, i + 1 + "", inventoryDictionary[dtInvoice.Rows[i][0] + ""]);
                    System.Console.WriteLine(sSQL);

                }



            } else {

            }
        }

        public double calculateTotal() {
            Double total = 0.00;
            if (dtInvoice != null) {
                try {
                    foreach (DataRow row in dtInvoice.Rows) {
                        total += Double.Parse(row[1].ToString());
                    }
                }catch(Exception e) { }
            }

            lblTotal.Content = "$" + total;
            return total;
        }



    }//end class

}//end namespace
