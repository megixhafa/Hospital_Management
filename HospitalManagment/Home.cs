using HospitalManagment.Models;
using Microsoft.VisualBasic.ApplicationServices;
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

                    ShowDoctor();

                }

                else if (roleId == 2)
                {
                    labBtn.Enabled = true;

                    ShowBookings();
                }

                else if (roleId == 3)
                {
                    patientBTN.Enabled = true;
                    equipmentBtn.Enabled = true;
                    labBtn.Enabled = true;
                    ShowPatient();
                }
            }
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

        private void equipmentBtn_Click(object sender, EventArgs e)
        {
            ShowEquipment();
        }

        private void doctorBtn_Click(object sender, EventArgs e)
        {
            ShowDoctor();
        }

        private void patientBTN_Click(object sender, EventArgs e)
        {
            ShowPatient();
        }

        private void receptionBTN_Click(object sender, EventArgs e)
        {
            ShowReceptionist();
        }

        private void labBtn_Click(object sender, EventArgs e)
        {
            ShowBookings();
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
                dt.ExtendedProperties["Table"] = "doctor";
                sda.Fill(dt);
                dgv.DataSource = dt;
                conn.Close();
            }
        }

        private void ShowEquipment()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string Query = "SELECT * from equipment";
                SqlCommand command = new SqlCommand(Query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                dt.ExtendedProperties["Table"] = "equipment";
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
                dt.ExtendedProperties["Table"] = "patient";
                sda.Fill(dt);
                dgv.DataSource = dt;
                conn.Close();
            }
        }

        private void ShowReceptionist()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string Query = "SELECT u.name, u.last_name, u.gender, u.number, u.email, u.address\r\nFROM employee e\r\nJOIN [user] u ON e.user_id = u.id;\r\n";
                SqlCommand command = new SqlCommand(Query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                dt.ExtendedProperties["Table"] = "employee";
                sda.Fill(dt);
                dgv.DataSource = dt;
                conn.Close();
            }
        }

        private void ShowBookings()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                string Query = "SELECT patient.name AS patient_name, patient.last_name AS patient_last_name, patient.gender AS patient_gender, " +
                                "doctor.name AS doctor_name, doctor.last_name AS doctor_last_name, doctor.gender AS doctor_gender, equipment.name AS equipment_name, " +
                                "service.name AS service_name " +
                                "FROM booking " +
                                "INNER JOIN [user] AS patient ON booking.patient_id = patient.id " +
                                "INNER JOIN [user] AS doctor ON booking.doctor_id = doctor.id " +
                                "INNER JOIN equipment ON booking.equipment_id = equipment.id " +
                                "INNER JOIN service ON booking.service_id = service.id;";
                SqlCommand command = new SqlCommand(Query, conn);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                dt.ExtendedProperties["Table"] = "booking";
                sda.Fill(dt);
                dgv.DataSource = dt;
                conn.Close();
            }
        }

        private void addbtn_Click(object sender, EventArgs e)
        {

            string selectedTableName = GetSelectedTableName();

            // Perform the appropriate action based on the selected table
            switch (selectedTableName)
            {
                case "doctor":
                    Doctor doctor = new Doctor();
                    doctor.Show();
                    break;

                case "patient":
                    Patient patient = new Patient();
                    patient.Show();
                    break;

                case "employee":
                    Employee employee = new Employee();
                    employee.Show();
                    break;

                case "equipment":
                    Service service = new Service();
                    service.Show();
                    break;

                case "booking":
                    Booking booking = new Booking();
                    booking.Show();
                    break;

                default:
                    // Handle the case where no table is selected or unsupported table
                    MessageBox.Show("No table selected or unsupported table.");
                    break;
            }
        }

        private string GetSelectedTableName()
        {
            if (dgv.DataSource is DataTable dataTable && dataTable.ExtendedProperties.ContainsKey("Table"))
            {
                // Retrieve the table name from the extended properties of the DataTable
                return dataTable.ExtendedProperties["Table"].ToString();
            }
            else
            {
                // Handle the case where the DataSource is not a DataTable or table name is not found
                return string.Empty;
            }
        }

    }
}
