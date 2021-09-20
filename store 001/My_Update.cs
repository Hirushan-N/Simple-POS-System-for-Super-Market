using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace store_001
{
    class My_Update
    {
        public static void INVENTORY(int counter,string Sold_quantity,string Product_Id)
        {
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            // UPDATE INVENTORY
            string sqlUpdate = @"UPDATE INVENTORY SET QUANTITY= (SELECT (SELECT QUANTITY FROM INVENTORY WHERE PRODUCT_ID='" + Product_Id + "') - '" + Convert.ToDecimal(Sold_quantity) + "') WHERE PRODUCT_ID='" + Product_Id + "'";
            SqlDataAdapter sda = new SqlDataAdapter();
            sda.UpdateCommand = new SqlCommand(sqlUpdate, con);
            sda.UpdateCommand.ExecuteNonQuery();
            con.Close();
        }
    }
}
