using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace store_001
{
    class My_Report
    {

        public static int ForDate = 0;
        public static int ForMonth = 1;
        public static int ForYear = 2;
        /// <summary>
        /// ///////////FUNCTION FOR CREATE DATA TABLE
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="ProductId"></param>
        /// <param name="ProductName"></param>
        /// <param name="TotalQuantity"></param>
        /// <param name="NetAmount"></param>
        /// <returns></returns>
        private static DataTable CreateDT(DataTable DT, int ProductId, string ProductName, decimal TotalQuantity, decimal NetAmount)
        {
            if (TotalQuantity != 0 && NetAmount != 0)
            {

                if (DT.Rows.Count == 0)
                {

                    DataColumn dc01 = new DataColumn("Product Id", typeof(int));
                    DataColumn dc02 = new DataColumn("Product Name", typeof(string));
                    DataColumn dc04 = new DataColumn("Total Quantity", typeof(decimal));
                    DataColumn dc05 = new DataColumn("Net Amount", typeof(decimal));

                    if (!DT.Columns.Contains("Product Id"))
                    {
                        DT.Columns.Add(dc01);
                    }
                    if (!DT.Columns.Contains("Product ProductName"))
                    {
                        DT.Columns.Add(dc02);
                    }
                    if (!DT.Columns.Contains("Quantity"))
                    {
                        DT.Columns.Add(dc04);
                    }
                    if (!DT.Columns.Contains("Net Amount"))
                    {
                        DT.Columns.Add(dc05);
                    }

                    DT.Rows.Add(ProductId, ProductName, Math.Round(TotalQuantity,3), NetAmount);

                }
                else
                {
                    DT.Rows.Add(ProductId, ProductName, TotalQuantity, NetAmount);

                }

            }
            return DT;
        }

        public static DataTable GetSource(DateTime SelectedDate)//////get report by date
        {
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            int rowcount = 0;

            string sqlrowcount = @"SELECT COUNT(*) FROM INVENTORY";
            SqlCommand comcount = new SqlCommand(sqlrowcount, con);
            SqlDataReader readercount = comcount.ExecuteReader();
            if (readercount.HasRows)
            {
                while (readercount.Read())
                {
                    rowcount = readercount.GetInt32(0) +1;
                   
                }
            }


            for (int i = 0; i < rowcount; i++)
            {
                int ProductId = 0;
                string ProductName = "";
                decimal TotalQuantity = 0;
                decimal NetAmount = 0;

                string SQL1 = @"Select PRODUCT_ID , PRODUCT_NAME From (Select  Row_Number() Over (Order By PRODUCT_ID) As RowNum , *From INVENTORY) AS NUMBERS WHERE RowNum = '" + i + "'";
                SqlCommand com1 = new SqlCommand(SQL1, con);
                SqlDataReader reader1 = com1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        ProductId = reader1.GetInt32(0);
                        ProductName = reader1.GetString(1);
                    }
                }
                com1.Dispose();
                reader1.Close();



                string SQL2 = @"SELECT SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT FROM SOLD_OUT INNER JOIN BILL_DETAILS ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID WHERE BILL_DETAILS.BILL_DATE='" + SelectedDate + "' and SOLD_OUT.PRODUCT_ID='" + ProductId + "'";
                SqlCommand com2 = new SqlCommand(SQL2, con);
                SqlDataReader reader2 = com2.ExecuteReader();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        TotalQuantity = TotalQuantity + reader2.GetDecimal(0);

                        NetAmount = NetAmount + reader2.GetDecimal(1);

                    }
                }
                com2.Dispose();
                reader2.Close();

                DT = CreateDT(DT, ProductId, ProductName, TotalQuantity, NetAmount);



            }
            return DT;
        }


        public static DataTable GetSource(DateTime SelectedDate, ComboBox comboBox)//////get report by category & date
        {


            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string CategoryName = "";


            int rowcount = 0;

            string sqlrowcount = @"SELECT COUNT(*) FROM INVENTORY";
            SqlCommand comcount = new SqlCommand(sqlrowcount, con);
            SqlDataReader readercount = comcount.ExecuteReader();
            if (readercount.HasRows)
            {
                while (readercount.Read())
                {
                    rowcount = readercount.GetInt32(0)+1;
                }
            }

            for (int i = 0; i < rowcount; i++)
            {
                int ProductId = 0;
                string ProductName = "";
                decimal TotalQuantity = 0;
                decimal NetAmount = 0;

                CategoryName = comboBox.Text;

                string SQL1 = @"Select PRODUCT_ID , PRODUCT_NAME From (Select  Row_Number() Over (Order By PRODUCT_ID) As RowNum , *From INVENTORY) AS NUMBERS WHERE RowNum = '" + i + "' AND CATEGORY = '" + CategoryName + "'";
                SqlCommand com1 = new SqlCommand(SQL1, con);
                SqlDataReader reader1 = com1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        ProductId = reader1.GetInt32(0);
                        ProductName = reader1.GetString(1);

                    }
                }
                com1.Dispose();
                reader1.Close();



                string SQL2 = @"SELECT SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT FROM SOLD_OUT INNER JOIN BILL_DETAILS ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID WHERE BILL_DETAILS.BILL_DATE='" + SelectedDate + "' and SOLD_OUT.PRODUCT_ID='" + ProductId + "'";
                SqlCommand com2 = new SqlCommand(SQL2, con);
                SqlDataReader reader2 = com2.ExecuteReader();
                if (reader2.HasRows)
                {

                    while (reader2.Read())
                    {
                        TotalQuantity = TotalQuantity + reader2.GetDecimal(0);

                        NetAmount = NetAmount + reader2.GetDecimal(1);

                    }
                }
                com2.Dispose();
                reader2.Close();

                DT = CreateDT(DT, ProductId, ProductName, TotalQuantity, NetAmount);

            }
            return DT;
        }

        public static DataTable GetSource(DateTime SelectedDate , String ProductProductName)//////get report by productname & date
        {
            int ProductId = 0;
            string ProductName = "";
            decimal TotalQuantity = 0;
            decimal NetAmount = 0;

            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string SQL1 = @"Select PRODUCT_ID FROM INVENTORY WHERE PRODUCT_NAME = '" + ProductProductName + "'";
            SqlCommand com1 = new SqlCommand(SQL1, con);
            SqlDataReader reader1 = com1.ExecuteReader();
            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    ProductId = reader1.GetInt32(0);
                    ProductName = ProductProductName;
                }
            }
            com1.Dispose();
            reader1.Close();


            string SQL2 = @"SELECT SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT FROM SOLD_OUT INNER JOIN BILL_DETAILS ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID WHERE BILL_DETAILS.BILL_DATE='" + SelectedDate + "' and SOLD_OUT.PRODUCT_ID='" + ProductId + "'";
            SqlCommand com2 = new SqlCommand(SQL2, con);
            SqlDataReader reader2 = com2.ExecuteReader();
            if (reader2.HasRows)
            {

                while (reader2.Read())
                {
                    TotalQuantity = TotalQuantity + reader2.GetDecimal(0);

                    NetAmount = NetAmount + reader2.GetDecimal(1);

                }
            }
            com2.Dispose();
            reader2.Close();

            DT = CreateDT(DT, ProductId, ProductName, TotalQuantity, NetAmount);

            return DT;
        }


        ///profit
        ///
        public static Decimal Profit(DateTime SelectedDate,int SelectedRadio)//////get profit by date
        {
            //DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();
            int ProductCount = 0;
            int rowcount = 0;

            string sqlrowcount = @"SELECT COUNT(*) FROM INVENTORY";
            SqlCommand comcount = new SqlCommand(sqlrowcount, con);
            SqlDataReader readercount = comcount.ExecuteReader();
            if (readercount.HasRows)
            {
                while (readercount.Read())
                {
                    rowcount = readercount.GetInt32(0);
                }
            }

            
            string Where_Clause = "";

            if (SelectedRadio == 0)
            {
                
                Where_Clause = "WHERE BILL_DETAILS.BILL_DATE = '" + SelectedDate + "'";
            }

            else if (SelectedRadio == 1)
            {
                
                Where_Clause = "WHERE MONTH(BILL_DETAILS.BILL_DATE) =" + SelectedDate.Month + " AND YEAR(BILL_DETAILS.BILL_DATE)=" + SelectedDate.Year + "";
            }

            else if (SelectedRadio == 2)
            {
                
                Where_Clause = "WHERE YEAR(BILL_DETAILS.BILL_DATE)= " + SelectedDate.Year + "";
            }



            decimal PROFIT = 0;
            for (int i = 0; i <= rowcount; i++)
            {
                int productID = 0;
                decimal PriceTaken = 0;
                decimal TOTpricetaken = 0;
                


                string SQL1 = @"Select PRODUCT_ID , WHOLESALE_PRICE  From (Select  Row_Number() Over (Order By PRODUCT_ID) As RowNum , *From INVENTORY) AS NUMBERS WHERE RowNum = '" + i + "'";
                SqlCommand com1 = new SqlCommand(SQL1, con);
                SqlDataReader reader1 = com1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        productID = reader1.GetInt32(0);
                        PriceTaken = reader1.GetDecimal(1);

                    }

                    


                   

                }
                com1.Dispose();
                reader1.Close();



                
                //2ND STEP
                string SQL2 = @"SELECT SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT FROM SOLD_OUT INNER JOIN BILL_DETAILS ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID "+Where_Clause+ " and SOLD_OUT.PRODUCT_ID = '" + productID + "'";
                SqlCommand com2 = new SqlCommand(SQL2, con);
                SqlDataReader reader2 = com2.ExecuteReader();

                decimal TotalQuantity = 0;
                decimal TOTnetAmount = 0;

                if (reader2.HasRows)
                {
                    ProductCount++;
                    

                    while (reader2.Read())
                    {
                        TotalQuantity = TotalQuantity + reader2.GetDecimal(0);


                        TOTnetAmount = TOTnetAmount + reader2.GetDecimal(1);

                    }

                }

                TOTpricetaken = TOTpricetaken + (PriceTaken * TotalQuantity);
                PROFIT = PROFIT + (TOTnetAmount - TOTpricetaken);
                com2.Dispose();
                reader2.Close();
                


            }
            return Math.Round(PROFIT, 2, MidpointRounding.ToEven);

        }

        public static Decimal Profit(DateTime SelectedDate, string prodname)//////get profit by productname & date
        {
            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            int productID = 0;
            decimal PriceTaken = 0;
            decimal TotalQuantity = 0;

            decimal TOTnetAmount = 0;
            decimal TOTpricetaken = 0;
            decimal PROFIT = 0;
            

            


            string SQL1 = @"Select PRODUCT_ID , WHOLESALE_PRICE  From INVENTORY WHERE PRODUCT_NAME='" + prodname + "'";
            SqlCommand com1 = new SqlCommand(SQL1, con);
            SqlDataReader reader1 = com1.ExecuteReader();
            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    productID = reader1.GetInt32(0);
                    PriceTaken = reader1.GetDecimal(1);

                }
            }
            com1.Dispose();
            reader1.Close();



            string SQL2 = @"SELECT SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT FROM SOLD_OUT INNER JOIN BILL_DETAILS ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID WHERE BILL_DETAILS.BILL_DATE='" + SelectedDate + "' and SOLD_OUT.PRODUCT_ID='" + productID + "'";
            SqlCommand com2 = new SqlCommand(SQL2, con);
            SqlDataReader reader2 = com2.ExecuteReader();


            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    TotalQuantity = TotalQuantity + reader2.GetDecimal(0);


                    TOTnetAmount = TOTnetAmount + reader2.GetDecimal(1);

                }

            }

            TOTpricetaken = TOTpricetaken + (PriceTaken * TotalQuantity);
            PROFIT = PROFIT + (TOTnetAmount - TOTpricetaken);

            com2.Dispose();
            reader2.Close();

            
            return Math.Round(PROFIT, 2, MidpointRounding.ToEven);
        }

        public static Decimal Profit(DateTime SelectedDate,ComboBox comboBox)//////get report by date & combo
        {
            string CategoryName = comboBox.Text;
           

            DataTable DT = new DataTable();
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            
           
            int rowcount = 0;

            string sqlrowcount = @"SELECT COUNT(*) FROM INVENTORY";
            SqlCommand comcount = new SqlCommand(sqlrowcount, con);
            SqlDataReader readercount = comcount.ExecuteReader();
            if (readercount.HasRows)
            {
                while (readercount.Read())
                {
                    rowcount = readercount.GetInt32(0);
                    
                }
            }
            
            decimal PROFIT = 0;

            for (int i = 0; i <= rowcount; i++)
            {
                int productID = 0;
                decimal PriceTaken = 0;
                decimal TOTpricetaken = 0;


                string SQL1 = @"Select PRODUCT_ID , WHOLESALE_PRICE  From (Select  Row_Number() Over (Order By PRODUCT_ID) As RowNum , *From INVENTORY) AS NUMBERS WHERE RowNum = '" + i + "' AND CATEGORY = '"+CategoryName+"'";
                SqlCommand com1 = new SqlCommand(SQL1, con);
                SqlDataReader reader1 = com1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                    {
                        productID = reader1.GetInt32(0);
                        PriceTaken = reader1.GetDecimal(1);
                       


                    }
                }
                com1.Dispose();
                reader1.Close();



                string SQL2 = @"SELECT SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT FROM SOLD_OUT INNER JOIN BILL_DETAILS ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID WHERE BILL_DETAILS.BILL_DATE='" + SelectedDate + "' and SOLD_OUT.PRODUCT_ID='" + productID + "'";
                SqlCommand com2 = new SqlCommand(SQL2, con);
                SqlDataReader reader2 = com2.ExecuteReader();

                decimal TotalQuantity = 0;
                decimal TOTnetAmount = 0;

                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                       
                        TotalQuantity = TotalQuantity + reader2.GetDecimal(0);
                        TOTnetAmount = TOTnetAmount + reader2.GetDecimal(1);
                      
                    }
                    

                }
                
                TOTpricetaken = TOTpricetaken + (PriceTaken * TotalQuantity);
                PROFIT = PROFIT + (TOTnetAmount - TOTpricetaken);
                
                com2.Dispose();
                reader2.Close();


                


            }
            con.Close();
            return Math.Round(PROFIT, 2, MidpointRounding.ToEven);

        }

        public static int GetBillCount(DateTime SelectedDate,int SelectedRadio)
        {
            int BillCount = 0;

            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string Where_Clause = "";

            if (SelectedRadio == 0)
            {

                Where_Clause = "WHERE BILL_DATE = '" + SelectedDate + "'";
            }

            else if (SelectedRadio == 1)
            {

                Where_Clause = "WHERE MONTH(BILL_DATE) =" + SelectedDate.Month + " AND YEAR(BILL_DATE)=" + SelectedDate.Year + "";
            }

            else if (SelectedRadio == 2)
            {

                Where_Clause = "WHERE YEAR(BILL_DATE)= " + SelectedDate.Year + "";
            }

            string SQL1 = @"SELECT COUNT(*) FROM BILL_DETAILS " + Where_Clause;
            SqlCommand com1 = new SqlCommand(SQL1, con);
            SqlDataReader reader = com1.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BillCount = reader.GetInt32(0);

                }
            }

            reader.Close();
            con.Close();

            return BillCount;
        }


        public static int GetProductCount(DateTime SelectedDate, int SelectedRadio)
        {
            int ProductCount = 0;

            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string Where_Clause = "";

            if (SelectedRadio == 0)
            {

                Where_Clause = "WHERE BILL_DATE = '" + SelectedDate + "'";

            }

            else if (SelectedRadio == 1)
            {

                Where_Clause = "WHERE MONTH(BILL_DETAILS.BILL_DATE) =" + SelectedDate.Month + " AND YEAR(BILL_DETAILS.BILL_DATE)=" + SelectedDate.Year + "";

            }

            else if (SelectedRadio == 2)
            {

                Where_Clause = "WHERE YEAR(BILL_DETAILS.BILL_DATE)= " + SelectedDate.Year + "";

            }

            string SQL1 = @"SELECT COUNT(*) FROM ((SOLD_OUT INNER JOIN INVENTORY ON SOLD_OUT.PRODUCT_ID=INVENTORY.PRODUCT_ID) INNER JOIN BILL_DETAILS ON BILL_DETAILS.BILL_ID=SOLD_OUT.BILL_ID )  " + Where_Clause + " GROUP BY INVENTORY.CATEGORY";
            SqlCommand com1 = new SqlCommand(SQL1, con);
            SqlDataReader reader = com1.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductCount = ProductCount + reader.GetInt32(0);

                }
            }
            reader.Close();
            con.Close();
            return ProductCount;
        }


        public static int GetCategoryCount(DateTime SelectedDate,int SelectedRadio)
        {
            int CategoryCount = 0;

            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string Where_Clause = "";

            if (SelectedRadio == 0)
            {

                Where_Clause = "WHERE BILL_DATE = '" + SelectedDate + "'";

            }

            else if (SelectedRadio == 1)
            {

                Where_Clause = "WHERE MONTH(BILL_DETAILS.BILL_DATE) =" + SelectedDate.Month + " AND YEAR(BILL_DETAILS.BILL_DATE)=" + SelectedDate.Year + "";

            }

            else if (SelectedRadio == 2)
            {

                Where_Clause = "WHERE YEAR(BILL_DETAILS.BILL_DATE)= " + SelectedDate.Year + "";

            }

            string SQL1 = @"SELECT COUNT(*) FROM ((SOLD_OUT INNER JOIN INVENTORY ON SOLD_OUT.PRODUCT_ID=INVENTORY.PRODUCT_ID) INNER JOIN BILL_DETAILS ON BILL_DETAILS.BILL_ID=SOLD_OUT.BILL_ID )  "+Where_Clause+" GROUP BY INVENTORY.CATEGORY";
            SqlCommand com1 = new SqlCommand(SQL1, con);
            SqlDataReader reader = com1.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CategoryCount++;
                }
            }
            reader.Close();
            con.Close();
            return CategoryCount;
        }

        public static void Indicator(DateTime SelectedDate,int SelectedRadio,Label ProfitIND,Label BillIND, Label ProductIND, Label CategoryIND, Label ProfitPercentage, Label BillPercentage, Label ProductPercentage, Label CategoryPercentage)
        {
            ProfitIND.Visible = false;
            BillIND.Visible = false;
            ProductIND.Visible = false;
            CategoryIND.Visible = false;
            ProfitPercentage.Visible = false;
            BillPercentage.Visible = false;
            ProductPercentage.Visible = false;
            CategoryPercentage.Visible = false;


            decimal Profit1 = 0;
            decimal Profit2 = 0;

            decimal BillC1 = 0;
            decimal BillC2 = 0;

            decimal ProdC1 = 0;
            decimal ProdC2 = 0;

            decimal CatC1 = 0;
            decimal CatC2 = 0;


            Profit1 = Profit(SelectedDate, SelectedRadio);
            BillC1 = GetBillCount(SelectedDate, SelectedRadio);
            ProdC1 = GetProductCount(SelectedDate, SelectedRadio);
            CatC1 = GetCategoryCount(SelectedDate, SelectedRadio);

            if (SelectedRadio==0)
            {
                BillC2 = GetBillCount(SelectedDate.AddDays(-1), SelectedRadio);

                ProdC2 = GetProductCount(SelectedDate.AddDays(-1), SelectedRadio);

                CatC2 = GetCategoryCount(SelectedDate.AddDays(-1), SelectedRadio);

                Profit2 = Profit(SelectedDate.AddDays(-1), SelectedRadio);
            }
            else if (SelectedRadio == 1)
            {
                BillC2 = GetBillCount(SelectedDate.AddMonths(-1), SelectedRadio);

                ProdC2 = GetProductCount(SelectedDate.AddMonths(-1), SelectedRadio);

                CatC2 = GetCategoryCount(SelectedDate.AddMonths(-1), SelectedRadio);

                Profit2 = Profit(SelectedDate.AddMonths(-1), SelectedRadio);
            }
            else if (SelectedRadio == 2)
            {
                BillC2 = GetBillCount(SelectedDate.AddYears(-1), SelectedRadio);

                ProdC2 = GetProductCount(SelectedDate.AddYears(-1), SelectedRadio);

                CatC2 = GetCategoryCount(SelectedDate.AddYears(-1), SelectedRadio);

                Profit2 = Profit(SelectedDate.AddYears(-1), SelectedRadio);
            }
            decimal percentage = 0;
            if(Profit2!=0)
            {
                MessageBox.Show(Profit1.ToString());
                MessageBox.Show(Profit2.ToString());
                percentage = Math.Round((((Profit1 - Profit2) / Profit2) * 100), 2);
                ProfitPercentage.Text = percentage.ToString() + " %";
                ProfitPercentage.Visible = true;

                if (percentage > 0)
                {
                    ProfitIND.Visible = true;
                    ProfitIND.Text = "↑";
                    ProfitIND.ForeColor = Color.Green;
                }
                else if (percentage < 0)
                {
                    ProfitIND.Visible = true;
                    ProfitIND.Text = "↓";
                    ProfitIND.ForeColor = Color.Red;

                }
                else
                    ProfitIND.Visible = false;
            }


            if (BillC2 != 0)
            {
                percentage = Math.Round((((BillC1 - BillC2) / BillC2) * 100), 2);
                BillPercentage.Text = percentage.ToString() + " %";
                BillPercentage.Visible = true;

                if (percentage > 0)
                {
                    BillIND.Visible = true;
                    BillIND.Text = "↑";
                    BillIND.ForeColor = Color.Green;
                }
                else if (percentage < 0)
                {
                    BillIND.Visible = true;
                    BillIND.Text = "↓";
                    BillIND.ForeColor = Color.Red;

                }
                else
                    BillIND.Visible = false;
            }


            if (ProdC2 != 0)
            {
                percentage = Math.Round((((ProdC1 - ProdC2) / ProdC2) * 100), 2);
                ProductPercentage.Text = percentage.ToString() + " %";
                ProductPercentage.Visible = true;


                if (percentage > 0)
                {
                    ProductIND.Visible = true;
                    ProductIND.Text = "↑";
                    ProductIND.ForeColor = Color.Green;
                }
                else if (percentage < 0)
                {
                    ProductIND.Visible = true;
                    ProductIND.Text = "↓";
                    ProductIND.ForeColor = Color.Red;

                }
                else
                    ProductIND.Visible = false;
            }


            if (CatC2 != 0)
            {
                percentage = Math.Round((((CatC1 - CatC2) / CatC2) * 100), 2);
                CategoryPercentage.Text = percentage.ToString() + " %";
                CategoryPercentage.Visible = true;

                if (percentage > 0)
                {
                    CategoryIND.Visible = true;
                    CategoryIND.Text = "↑";
                    CategoryIND.ForeColor = Color.Green;
                }
                else if (percentage < 0)
                {
                    CategoryIND.Visible = true;
                    CategoryIND.Text = "↓";
                    CategoryIND.ForeColor = Color.Red;

                }
                else
                    CategoryIND.Visible = false;
            }


        }

    }
}
