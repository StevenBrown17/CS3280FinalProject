# CS3280FinalProject
Group 4 Final Project - CS3280
Instructions for prototype

            The first part due for the group project is a prototype of the final project. This part of the assignment is to get 
            each group member engaged and to assign each role for the assignment.

            I recommend the group project be broken up so that each member is responsible for everything for a single Window.

            The prototype of the group project will consist of a preliminary design of the UI, how each person’s code will interface 
            with other code, and the SQL for the assignment in a class. Since the group project is made up of three Windows, and there are three group members, it usually makes sense to assign one person, one of the three forms

 

GUI

            All of the screens should be created with all controls needed to complete the requirements. For instance, on the search screen,
            there should be 3 drop down boxes for selection, a DataGrid, and select and cancel buttons. Once each screen has been created 
            the flow of the program needs to be completed. So, for example, on the main form, there should be a menu with the selection of 
            “Search for Invoice” that when clicked should open the search window, then when the user clicks the “Select” or “Cancel” buttons 
            the search window should close and the main form get focus. Interfaces

            This part of the assignment is to put together a plan on how each screen will pass the data to the other screens. This will be 
            done by putting the appropriate comments in the sections of stubbed out code to explain how the data will be passed around. 
            This will get you thinking about how each screen will interface with the others. So for example, on the search screen, behind 
            the button click event for the “Select” button, there should be a detailed comment about how the selected InvoiceID will be 
            passed back to the main form. For example, if a property is set in the Search screen window with the selected Invoice ID, then 
            the comment will explain how the variable is set and the Main screen may access this data via a property.
            
SQL

            This part of the assignment is to create a class that contains the main pieces of SQL used throughout the project. This class 
            will be nothing but methods that contain different statements of SQL. Make sure to create SQL statements that will help in 
            meeting all requirements that use the database. So, SQL statements needed will be to select different types of data on each 
            window, to update/insert/delete data on each form. Use Microsoft Access to run the queries ahead of time to make sure the 
            queries give you the expected data and work correctly. Your SQL statements should be tested and working. Below is an example 
            of a class/method that should be used as a guide for your code.


   class clsSQL

   {

       /// <summary>

       /// This SQL gets all data on an invoice for a given InvoiceID.

       /// </summary>

       /// <param name="sInvoiceID">The InvoiceID for the invoice to retrieve all data.</param>

       /// <returns>All data for the given invoice.</returns>

       public string SelectInvoiceData(string sInvoiceID)

       {

           string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sInvoiceID;

           return sSQL;

       }

   }
