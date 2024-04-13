using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Shualeduri
    
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //SqlConnection con = new SqlConnection("Data Source=TAMO\\MSSQLSERVER02;Initial Catalog=Products_crud;Integrated Security=True;Trust Server Certificate=True");
        SqlConnection con = new SqlConnection("Data Source=TAMO\\MSSQLSERVER02;Initial Catalog=Products_crud;Integrated Security=True;");
        async Task LoadAllRecords(string PTableViewStr, DataGridView dataGridView)
        {
            try
            {
                con.Open();
                SqlCommand com = new SqlCommand(PTableViewStr, con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading records: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void Insert_Click(object sender, EventArgs e)
        {
            con.Open();
            string status = "";
            if (radioButton1.Checked)
            {
                status = radioButton1.Text;
            }
            else
            {
                status = radioButton2.Text;
            }
            SqlCommand com = new SqlCommand("exec Product_Insert '" + int.Parse(textBox1.Text) + "', '" + textBox2.Text + "', '" + comboBox1.Text + "','" + status +"','"+ DateTime.Parse(dateTimePicker1.Text)+ "'", con);

            com.ExecuteNonQuery();
            MessageBox.Show("Successfully Saved");

            con.Close();
            LoadAllRecords("exec Product_View", dataGridView1);
        }



        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadAllRecords("exec Product_View", dataGridView1);
            await LoadAllRecords("exec fav_view", dataGridView2);
            MessageBox.Show("Hi");
        }

        private void Update_Click(object sender, EventArgs e)
        {
            /*
             string status = "";
             if (radioButton1.Checked)
             {
                 status = radioButton1.Text;
             }
             else
             {
                 status = radioButton2.Text;
             }
             SqlCommand com = new SqlCommand("exec Product_Update '" + int.Parse(textBox1.Text) + "', '" + textBox2.Text + "', '" + comboBox1.Text + "','" + status +"','"+ DateTime.Parse(dateTimePicker1.Text)+ "'", con);

             com.ExecuteNonQuery();
             MessageBox.Show("Successfully Updated");
            */
            con.Open();
            string status = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;


            SqlCommand com = new SqlCommand("Product_Update", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@ProductId", int.Parse(textBox1.Text));
            com.Parameters.AddWithValue("@ItemName", textBox2.Text);
            com.Parameters.AddWithValue("@Color", comboBox1.Text);
            com.Parameters.AddWithValue("@Status", status);
            com.Parameters.AddWithValue("@ExpiryDate", DateTime.Parse(dateTimePicker1.Text));

            com.ExecuteNonQuery();
            MessageBox.Show("Successfully Updated");


            con.Close();
            LoadAllRecords("exec Product_View", dataGridView1);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand com = new SqlCommand("Product_Delete", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@ProductId", int.Parse(textBox1.Text));

            com.ExecuteNonQuery();
            MessageBox.Show("Successfully Deleted");


            con.Close();
            LoadAllRecords("exec Product_View", dataGridView1);

        }

        private void Search_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand com = new SqlCommand("Product_Search", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@ProductId", int.Parse(textBox1.Text));

            com.ExecuteNonQuery();


            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string iteamName = row["ItemName"].ToString();
                string color = row["Color"].ToString();
                textBox2.Text = iteamName;
                comboBox1.Text =color;
                dataGridView1.DataSource = dt;
                MessageBox.Show("Successfully Searched");
            }
            else
            {
                MessageBox.Show("No records found for the provided ProductId.");
            }


            con.Close();


        }

        private void addToFavorites_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand com = new SqlCommand("Add_to_Favorites", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@ProductId", int.Parse(textBox1.Text));

            com.ExecuteNonQuery();
            MessageBox.Show("Successfully added to favorites");


            con.Close();
             LoadAllRecords("exec fav_view", dataGridView2);
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
