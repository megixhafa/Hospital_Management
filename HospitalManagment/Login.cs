using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using HospitalManagment.Models;

namespace HospitalManagment
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        //SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=db_hospital_management; integrated security=SSPI;");
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String username, password;

            username = usernameTxt.Text;
            password = passwordTxt.Text;

            using (SqlConnection conn = DatabaseManager.GetConnection())
            {

                try
                {
                    String query = "SELECT * FROM user_account WHERE username = '" + username + "' AND password = '" + password + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(query, conn);

                    DataTable dtable = new DataTable();
                    sda.Fill(dtable);

                    if (dtable.Rows.Count > 0)
                    {

                        int roleId = Convert.ToInt32(dtable.Rows[0]["role_id"]);

                        role.RoleId = roleId;

                        username = usernameTxt.Text;
                        password = passwordTxt.Text;

                        //page that needed to be loaded next
                        Home home = new Home();
                        home.Show();

                        this.Hide();
                    }

                    else
                    {
                        MessageBox.Show("Invalid login details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        usernameTxt.Clear();
                        passwordTxt.Clear();
                    }
                }
                catch
                {
                    MessageBox.Show("Error! Login not valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }
    }
}