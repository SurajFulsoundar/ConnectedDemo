using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace ConnectedDemo
{
    public partial class Product : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;
        public Product()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
        {
            try
            {
                List<Category> list = new List<Category>();
                string qry = "Select * from cat";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Category cat = new Category();
                        cat.C_id = Convert.ToInt32(reader["C_id"]);
                        cat.C_name = reader["C_Name"].ToString();
                        list.Add(cat);
                    }
                }
                // display dname & on selection of dname we need did
                comcname.DataSource = list;
                comcname.DisplayMember = "C_Name";
                comcname.ValueMember = "C_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.C_Name from prod p inner join cat c on c.C_id = p.C_id where p.P_id = @P_id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@P_id", Convert.ToInt32(txtpid.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        txtpname.Text = reader["P_Name"].ToString();
                        txtprice.Text = reader["Price"].ToString();                       
                        comcname.Text = reader["C_Name"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into prod values(@P_name,@Price,@C_id)";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@P_name", txtpname.Text);
                cmd.Parameters.AddWithValue("@Price", Convert.ToInt32(txtprice.Text));
                cmd.Parameters.AddWithValue("@C_id", Convert.ToInt32(comcname.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();

                if (result >= 1)
                {
                    MessageBox.Show("Record inserted");
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();
        }

        private void ClearFields()
        {
            txtpid.Clear();
            txtpname.Clear();
            txtprice.Clear();
            comcname.Refresh();
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "Update prod set P_Name = @P_Name,Price=@Price,C_id = @C_id where P_id = @P_id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameters
                cmd.Parameters.AddWithValue("@P_Name", txtpname.Text);
                cmd.Parameters.AddWithValue("@Price", Convert.ToInt32(txtprice.Text));
                cmd.Parameters.AddWithValue("@C_id", Convert.ToInt32(comcname.SelectedValue));
                cmd.Parameters.AddWithValue("@P_id", Convert.ToInt32(txtpid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record Updated");
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "Delete from prod where P_id = @P_id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@P_id", Convert.ToInt32(txtpid.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record Deleted");
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Record Not Found");
                }
                               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            GetAllEmps();

        }

        public void GetAllEmps()
        {
            string qry = "select p.*,c.C_name from prod p inner join cat c on c.C_id = p.C_id";
            cmd= new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();
        }

        private void btnshowall_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllEmps();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           

            

        }
    
    }
}
