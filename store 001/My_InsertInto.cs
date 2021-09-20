using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace store_001
{
    class My_InsertInto
    {
        public static void INVENTORY(string productID,string name,string quantity,string category,DateTime date,string wholesale,string retail)
        {
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();
            try
            {
                string sql = @"INSERT INTO INVENTORY(PRODUCT_ID,PRODUCT_NAME,QUANTITY,CATEGORY,ENTERED_DATE,WHOLESALE_PRICE,RETAIL_PRICE)
                                VALUES('" + Convert.ToInt32(productID) + "','" + name + "','" + Convert.ToDecimal(quantity) + "','" + category + "','" + date + "','" + Convert.ToDecimal(wholesale) + "','" + Convert.ToDecimal(retail) + "' )";
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.InsertCommand = new SqlCommand(sql, con);
                adp.InsertCommand.ExecuteNonQuery();

                MessageBox.Show("sucsessfull added");
                con.Close();

                //sql is over

                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void SOLD_OUT(int counter,string Bill_Id,String Product_Id,decimal Sold_quantity,decimal TOT_AMOUNT)
        {
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string sqlInsert = @"INSERT INTO SOLD_OUT(DATA_ID,BILL_ID,PRODUCT_ID,SOLD_QUANTITY,TOTAL_AMOUNT)
                                VALUES('" + My_Genarate.DataRecord_ID() + "','" + Bill_Id + "','" + Product_Id + "','" + Sold_quantity + "','" + TOT_AMOUNT + "')";
            SqlDataAdapter adp = new SqlDataAdapter();
            adp.InsertCommand = new SqlCommand(sqlInsert, con);
            adp.InsertCommand.ExecuteNonQuery();
            adp.InsertCommand.Dispose();
            adp.Dispose();

            con.Close();
        }

        public static void BILL_DETAILS(string BillId,DateTime date,decimal BillAmount)
        {
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string sqlin = @"INSERT INTO BILL_DETAILS(BILL_ID,BILL_DATE,BILL_AMOUNT)
                                VALUES('" + BillId + "','" + date + "','" + BillAmount + "')";
            SqlDataAdapter adpp = new SqlDataAdapter();
            adpp.InsertCommand = new SqlCommand(sqlin, con);
            adpp.InsertCommand.ExecuteNonQuery();
            adpp.InsertCommand.Dispose();
            adpp.Dispose();
        }

    }

}
