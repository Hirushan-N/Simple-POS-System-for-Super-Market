using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace store_001
{
    class My_BillTable
    {
        static DataTable DT = new DataTable();
        public static DataTable SetRecord(string id,string name,string price,decimal quantity,string netamount)
        {
            if (DT.Rows.Count == 0)
            {

                DataColumn dc01 = new DataColumn("Product Id", typeof(string));
                DataColumn dc02 = new DataColumn("Product Name", typeof(string));
                DataColumn dc03 = new DataColumn("Unit Price", typeof(string));
                DataColumn dc04 = new DataColumn("Quantity", typeof(string));
                DataColumn dc05 = new DataColumn("Net Amount", typeof(string));

                if (!DT.Columns.Contains("Product Id"))
                {
                    DT.Columns.Add(dc01);
                }
                if (!DT.Columns.Contains("Product Name"))
                {
                    DT.Columns.Add(dc02);
                }
                if (!DT.Columns.Contains("Unit Price"))
                {
                    DT.Columns.Add(dc03);
                }
                if (!DT.Columns.Contains("Quantity"))
                {
                    DT.Columns.Add(dc04);
                }
                if (!DT.Columns.Contains("Net Amount"))
                {
                    DT.Columns.Add(dc05);
                }

                DT.Rows.Add(id,name,price,quantity,netamount);

            }
            else
            {
                DT.Rows.Add(id, name, price, quantity, netamount);
                
            }
            return DT;

        }
        public static void DTRowsClear()
        {
            DT.Rows.Clear();
        }
    }
}
