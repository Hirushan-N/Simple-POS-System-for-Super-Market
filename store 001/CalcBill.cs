using AForge.Video.DirectShow;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using ZXing;

namespace store_001
{
    public partial class CalcBill : Form
    {

        public CalcBill()
        {
            InitializeComponent();
        }



        private void billcal_Load(object sender, EventArgs e)
        {
         
            

            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Normal;
            BtxtProductName.AutoCompleteCustomSource = My_AutoCompleate.ProductName();
            BtxtProductName.Focus();
            TotalBillAmount = 0;
            bgrd.Visible = false;
            blblPLUSmark.Visible = false;
            BtxtProductName.BackColor = Color.Yellow;

            Underline01.Visible = false;
            Underline02.Visible = false;
            Underline03.Visible = false;
            Underline04.Visible = false;
            Underline05.Visible = false;

            blblbillid.Text = My_Genarate.Bill_ID();

        }


        decimal TotalBillAmount = 0;

        private void styledtagrid()
        {
            bgrd.BorderStyle = BorderStyle.None;
            bgrd.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            bgrd.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            bgrd.DefaultCellStyle.SelectionBackColor = Color.Blue;
            bgrd.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            bgrd.BackgroundColor = Color.FromArgb(30, 30, 30);
            bgrd.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            bgrd.EnableHeadersVisualStyles = false;
            bgrd.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            bgrd.ColumnHeadersDefaultCellStyle.Font = new Font("MS Reference Sans Serif", 10);
            bgrd.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(37, 37, 38);
            bgrd.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
        //new stack
        private void BtxtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Underline01.Visible = true;
                Underline02.Visible = true;
                Underline03.Visible = true;


                SqlConnection con = new SqlConnection(connectDB.connectionstring);
                con.Open();
                string sql = @"SELECT PRODUCT_ID,RETAIL_PRICE FROM INVENTORY WHERE PRODUCT_NAME= '" + BtxtProductName.Text + "'";
                SqlCommand objectcom = new SqlCommand(sql, con);
                SqlDataReader reder = objectcom.ExecuteReader();

                if (reder.HasRows)
                {
                    while (reder.Read())
                    {

                        blblitemid.Text = reder.GetInt32(0).ToString();
                        blblunitprice.Text = reder.GetDecimal(1).ToString();

                    }
                }
                else
                {
                    MessageBox.Show("invalide input");
                }

                BtxtProductName.BackColor = Color.White;
                BnumaricQuantity.Focus();
                BnumaricQuantity.BackColor = Color.Red;
            }
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("Are you Sure Want to Print this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    print();
                }

            }
        }

        private void BnumaricQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Underline04.Visible = true;
                Underline05.Visible = true;

                BtxtNetAmount.Text = (Convert.ToDecimal(blblunitprice.Text) * Convert.ToDecimal(BnumaricQuantity.Value)).ToString();
                if (!string.IsNullOrEmpty(blblBillAmount.Text))
                {
                    blblPLUSmark.Visible = true;
                    blblNetAnmountADD.Text = BtxtNetAmount.Text;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;  // desable "ding" sound

                //DESIGN
                BnumaricQuantity.BackColor = Color.White;
                BtxtNetAmount.Focus();
                BtxtNetAmount.BackColor = Color.Yellow;

            }
        }

        private void BtxtNetAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (bgrd.Height < 642)
                {
                    bgrd.Height += 25;
                }
                Underline01.Visible = false;
                Underline02.Visible = false;
                Underline03.Visible = false;
                Underline04.Visible = false;
                Underline05.Visible = false;

                TotalBillAmount = TotalBillAmount + Convert.ToDecimal(BtxtNetAmount.Text);
                blblBillAmount.Text = TotalBillAmount.ToString();

                styledtagrid();// design data gridview style
                bgrd.Visible = true;
                bgrd.DataSource = My_BillTable.SetRecord(blblitemid.Text, BtxtProductName.Text, blblunitprice.Text, BnumaricQuantity.Value, BtxtNetAmount.Text);

                BtxtNetAmount.BackColor = Color.White;
                BtxtProductName.BackColor = Color.Yellow;
                BtxtProductName.Focus();

                //clear fields
                blblPLUSmark.Visible = false;
                blblitemid.Text = "";
                blblunitprice.Text = "";
                BtxtProductName.Text = "";
                BnumaricQuantity.Value = 0;
                BtxtNetAmount.Text = "";
                blblNetAnmountADD.Text = "";
            }
        }

        private void print()
        {
            My_InsertInto.BILL_DETAILS(blblbillid.Text, dateTimePicker1.Value, Convert.ToDecimal(blblBillAmount.Text));

            int count = 0;
            for (int i = 0; i < bgrd.Rows.Count - 1; i++)
            {

                My_InsertInto.SOLD_OUT(i, blblbillid.Text, bgrd.Rows[i].Cells[0].Value.ToString(), Convert.ToDecimal(bgrd.Rows[i].Cells[3].Value), Convert.ToDecimal(bgrd.Rows[i].Cells[4].Value));
                My_Update.INVENTORY(i, bgrd.Rows[i].Cells[3].Value.ToString(), bgrd.Rows[i].Cells[0].Value.ToString());

                count = i;

            }

            TotalBillAmount = 0;
            My_BillTable.DTRowsClear();
            blblBillAmount.Text = "";
            blblitemid.Text = "";
            blblunitprice.Text = "";
            BtxtProductName.Text = "";
            BnumaricQuantity.Value = 0;
            BtxtNetAmount.Text = "";

            blblbillid.Text = My_Genarate.Bill_ID();
            BtxtProductName.Focus();
            BtxtProductName.BackColor = Color.Yellow;

            bgrd.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void btnRestoreDown_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                btnRestoreDown.Text = "2";
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                btnRestoreDown.Text = "1";
            }
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



        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Black;
        }

        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Red;
        }

        private void btnRestoreDown_MouseEnter(object sender, EventArgs e)
        {
            btnRestoreDown.BackColor = Color.Red;
        }

        private void btnRestoreDown_MouseLeave(object sender, EventArgs e)
        {
            btnRestoreDown.BackColor = Color.Black;
        }

        private void btnMinimize_MouseEnter(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.Red;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.BackColor = Color.Black;
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

       

        private void bgrd_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }

        }


        
        private void bgrd_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            MessageBox.Show("user deleted row");
        }
        Boolean Dragable;
        int mouseX;
        int mouseY;
        private void CalcBill_MouseDown(object sender, MouseEventArgs e)
        {
            Dragable = true;
            mouseX = Cursor.Position.X - this.Left;
            mouseY = Cursor.Position.Y - this.Top;
        }

        private void CalcBill_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragable)
            {
                this.Top = Cursor.Position.Y - mouseY;
                this.Left = Cursor.Position.X - mouseX;
            }
        }

        private void CalcBill_MouseUp(object sender, MouseEventArgs e)
        {
            Dragable = false;
        }

        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            Dragable = true;
            mouseX = Cursor.Position.X - this.Left;
            mouseY = Cursor.Position.Y - this.Top;
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (Dragable)
            {
                this.Top = Cursor.Position.Y - mouseY;
                this.Left = Cursor.Position.X - mouseX;
            }
        }

        private void panel4_MouseUp(object sender, MouseEventArgs e)
        {
            Dragable = false;
        }

       
    }

}
