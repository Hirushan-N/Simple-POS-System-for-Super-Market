using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace store_001
{
    class My_AutoCompleate
    {
            
        public static AutoCompleteStringCollection ProductName()
        {

            try
            {
                AutoCompleteStringCollection collection = new AutoCompleteStringCollection();
                SqlConnection con = new SqlConnection(connectDB.connectionstring);
                con.Open();
                string sql = @"SELECT PRODUCT_NAME FROM INVENTORY";
                SqlCommand comobj = new SqlCommand(sql, con);
                SqlDataReader reder = comobj.ExecuteReader();
                while (reder.Read())
                {
                    string list = "";
                    list = list + reder.GetString(0);
                    collection.Add(list);
                }
                return collection;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void Categories(ComboBox combo)
        {

            try
            {
                combo.Items.Clear();
                SqlConnection con = new SqlConnection(connectDB.connectionstring);
                con.Open();
                string sql = @"SELECT CATEGORY FROM INVENTORY ORDER BY CATEGORY ASC";
                SqlCommand comobj = new SqlCommand(sql, con);
                SqlDataReader reader = comobj.ExecuteReader();
                string previous = "";
                while (reader.Read())
                {
                    if (previous != reader.GetString(0))
                    {
                        combo.Items.Add(reader.GetString(0));
                    }
                    previous = reader.GetString(0);
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
