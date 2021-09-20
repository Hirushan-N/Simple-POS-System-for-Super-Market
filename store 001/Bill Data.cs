using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace store_001
{
    public partial class Bill_Data : Form
    {
        public Bill_Data()
        {
            InitializeComponent();
        }

        private void Bill_Data_Load(object sender, EventArgs e)
        {
            
        }

        private void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                dgBill.Rows.Clear();

                SqlConnection con = new SqlConnection(connectDB.connectionstring);
                con.Open();

                

                string sql = @"SELECT BILL_DETAILS.BILL_DATE , SOLD_OUT.PRODUCT_ID , INVENTORY.PRODUCT_NAME , INVENTORY.RETAIL_PRICE , SOLD_OUT.SOLD_QUANTITY , SOLD_OUT.TOTAL_AMOUNT ,  BILL_DETAILS.BILL_AMOUNT   FROM ((BILL_DETAILS INNER JOIN SOLD_OUT ON SOLD_OUT.BILL_ID=BILL_DETAILS.BILL_ID) INNER JOIN INVENTORY ON SOLD_OUT.PRODUCT_ID=INVENTORY.PRODUCT_ID) WHERE BILL_DETAILS.BILL_ID='" + txtID.Text + "'";
                SqlCommand command = new SqlCommand(sql, con);
                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    lblID.Text = txtID.Text;

                    while (reader.Read())
                    {
                        lblDate.Text = reader.GetDateTime(0).ToShortDateString();

                        lblBillAmount.Text = reader.GetDecimal(6).ToString();
                        dgBill.Rows.Add
                            (
                            new object[]
                            {
                                reader.GetInt32(1).ToString(),reader.GetString(2),reader.GetDecimal(3),reader.GetDecimal(4),"Rs. "+reader.GetDecimal(5)
                            }
                                
                        );
                    }
                }

                
            }

        }

        private void txtID_Enter(object sender, EventArgs e)
        {
            if(txtID.Text == "  Enter bill ID hear")
            {
                txtID.Text = "";
            }
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                txtID.Text = "  Enter bill ID hear";
            }
        }
        Boolean Dragable;
        int mouseX;
        int mouseY;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Dragable = true;
            mouseX = Cursor.Position.X - this.Left;
            mouseY = Cursor.Position.Y - this.Top;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragable)
            {
                this.Top = Cursor.Position.Y - mouseY;
                this.Left = Cursor.Position.X - mouseX;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Dragable = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
