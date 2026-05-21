
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Inventory_SalesManagementSystem
{
    public partial class ManageCustomer : Form
    {
        public ManageCustomer()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\Inventorydb.mdf;Integrated Security=True;Connect Timeout=30"
        );

        // Load all customers
        void populate()
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM CustomerTbl";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                CustomerGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Add Customer
        private void button1_Click(object sender, EventArgs e)
        {
            if (Customerid.Text == "" || CustomernameTb.Text == "" || CustomerPhoneTb.Text == "")
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            try
            {
                Con.Open();
                string query = "INSERT INTO CustomerTbl (CustId, CustName, CustPhone) VALUES (@Id, @Name, @Phone)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", Customerid.Text);
                cmd.Parameters.AddWithValue("@Name", CustomernameTb.Text);
                cmd.Parameters.AddWithValue("@Phone", CustomerPhoneTb.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Customer added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message);
            }
            finally
            {
                Con.Close();
                populate();
            }
        }

        // Update Customer
        private void button2_Click(object sender, EventArgs e)
        {
            if (Customerid.Text == "")
            {
                MessageBox.Show("Select a customer to update!");
                return;
            }

            try
            {
                Con.Open();
                string query = "UPDATE CustomerTbl SET CustName=@Name, CustPhone=@Phone WHERE CustId=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Name", CustomernameTb.Text);
                cmd.Parameters.AddWithValue("@Phone", CustomerPhoneTb.Text);
                cmd.Parameters.AddWithValue("@Id", Customerid.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("✅ Customer updated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message);
            }
            finally
            {
                Con.Close();
                populate();
            }
        }

        // Delete Customer
        private void button3_Click(object sender, EventArgs e)
        {
            if (Customerid.Text == "")
            {
                MessageBox.Show("Enter Customer ID to delete!");
                return;
            }

            try
            {
                Con.Open();
                string query = "DELETE FROM CustomerTbl WHERE CustId=@Id";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@Id", Customerid.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("🗑️ Customer deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting customer: " + ex.Message);
            }
            finally
            {
                Con.Close();
                populate();
            }
        }

        // On Form Load
        private void ManageCustomer_Load(object sender, EventArgs e)
        {
            populate();
        }

        // Handle grid click safely
        private void CustomerGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = CustomerGV.Rows[e.RowIndex];
                Customerid.Text = row.Cells["CustId"].Value.ToString();
                CustomernameTb.Text = row.Cells["CustName"].Value.ToString();
                CustomerPhoneTb.Text = row.Cells["CustPhone"].Value.ToString();

                try
                {
                    Con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT COUNT(*) FROM OrderTbl WHERE CustId=@Id", Con);
                    sda.SelectCommand.Parameters.AddWithValue("@Id", Customerid.Text);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    OrderLabel.Text = dt.Rows[0][0].ToString();

                    SqlDataAdapter sda1 = new SqlDataAdapter("SELECT SUM(TotalAmt) FROM OrderTbl WHERE CustId=@Id", Con);
                    sda1.SelectCommand.Parameters.AddWithValue("@Id", Customerid.Text);
                    DataTable dt1 = new DataTable();
                    sda1.Fill(dt1);
                    AmountLabel.Text = dt1.Rows[0][0].ToString();

                    SqlDataAdapter sda2 = new SqlDataAdapter("SELECT MAX(OrderDate) FROM OrderTbl WHERE CustId=@Id", Con);
                    sda2.SelectCommand.Parameters.AddWithValue("@Id", Customerid.Text);
                    DataTable dt2 = new DataTable();
                    sda2.Fill(dt2);
                    DateLabel.Text = dt2.Rows[0][0].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading order stats: " + ex.Message);
                }
                finally
                {
                    Con.Close();
                }
            }
        }

        // Search (optional)
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (CustomernameTb.Text == "")
                {
                    MessageBox.Show("Enter name to search!");
                    return;
                }

                Con.Open();
                string query = "SELECT * FROM CustomerTbl WHERE CustName LIKE @Search";
                SqlDataAdapter da = new SqlDataAdapter(query, Con);
                da.SelectCommand.Parameters.AddWithValue("@Search", "%" + CustomernameTb.Text + "%");
                DataSet ds = new DataSet();
                da.Fill(ds);
                CustomerGV.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }

        // Refresh Button
        private void button6_Click(object sender, EventArgs e)
        {
            populate();
        }

        // Back Button
        private void button4_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }


        private void UsersGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

