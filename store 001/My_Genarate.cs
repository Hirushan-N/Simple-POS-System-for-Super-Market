using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace store_001
{
    class My_Genarate
    {
        static string BillID;
        public static String Bill_ID()
        {
            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string sql = @"select BILL_ID from BILL_DETAILS where BILL_ID=(select max(BILL_ID) from BILL_DETAILS)";
            SqlCommand objectcom = new SqlCommand(sql, con);

            SqlDataReader reder = objectcom.ExecuteReader();
            if (reder.HasRows)
            {
                while (reder.Read())
                {
                    BillID = (Convert.ToInt32(reder.GetString(0)) + 1).ToString();
                }
            }
            else
            {
                BillID = "10";
            }
            return BillID;
        }

        public static string DataRecord_ID()
        {
            string DataRecordID = "";

            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();
            string sqlID = @"select DATA_ID from SOLD_OUT where DATA_ID=(select max(DATA_ID) from SOLD_OUT)";
            SqlCommand objectcom = new SqlCommand(sqlID, con);
            SqlDataReader reder = objectcom.ExecuteReader();

            
            if (reder.HasRows)
            {
                while (reder.Read())
                {
                    DataRecordID = (Convert.ToInt32(reder.GetString(0)) + 1).ToString(); ;


                }
            }
            else
            {
                DataRecordID = "10"; //DEFAULT ID FOR FIRST DATA RECORD
            }
            objectcom.Dispose();
            return DataRecordID;
        }

       static string ProductID;
        public static string Product_ID()
        {
            

            SqlConnection con = new SqlConnection(connectDB.connectionstring);
            con.Open();

            string sql = @"select PRODUCT_ID from INVENTORY where PRODUCT_ID=(select max(PRODUCT_ID) from INVENTORY)";
            SqlCommand objectcom = new SqlCommand(sql, con);

            SqlDataReader reader = objectcom.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProductID = (reader.GetInt32(0) + 1).ToString();
                }
            }
            else
            {
                ProductID = "10";
            }
            return ProductID;
        }
    }
}
