using HospitalManagment.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace HospitalManagment
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            ShowDoctor();
        }


        private void Home_Load(object sender, EventArgs e)
        {
            int roleId = role.RoleId;

            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                // Enable or disable buttons based on the role
                if (roleId == 1)
                {
                    doctorBtn.Enabled = true;
                    receptionBTN.Enabled = true;
                    patientBTN.Enabled = true;
                    labBtn.Enabled = true;
                    equipmentBtn.Enabled = true;

                    try
                    {
                        String query = "select * from ";
                        SqlDataAdapter sda = new SqlDataAdapter(query, conn);

                        DataTable dtable = new DataTable();
                        sda.Fill(dtable);

                        if (dtable.Rows.Count > 0)
                        {
                            
                        }
                    }
                    catch
                    {

                    }
                }

                else if (roleId == 2)
                {
                    labBtn.Enabled = true;
                    equipmentBtn.Enabled = true;
                }
                else if (roleId == 3)
                {
                    patientBTN.Enabled = true;
                }
            }
        }

        private void equipmentBtn_Click(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Do you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void doctorBtn_Click(object sender, EventArgs e)
        {
            ShowDoctor();
        }

        private void patientBTN_Click(object sender, EventArgs e)
        {
            ShowPatient();
        }

        private void ShowDoctor()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string Query = "SELECT d.experience, d.specialization, u.name, u.last_name, u.gender, u.number, u.email, u.address\r\nFROM doctor d\r\nJOIN [user] u ON d.user_id = u.id;\r\n";
                SqlCommand command = new SqlCommand(Query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgv.DataSource = dt;
                conn.Close();
            }
        }

        private void ShowPatient()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string Query = "SELECT u.name, u.last_name, u.gender, u.number, u.email, u.address\r\nFROM patient p\r\nJOIN [user] u ON p.user_id = u.id;\r\n";
                SqlCommand command = new SqlCommand(Query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgv.DataSource = dt;
                conn.Close();
            }
        }
    }
}
