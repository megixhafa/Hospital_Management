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
            ShowBookings();
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
                string Query = "SELECT patient_user.name AS patient_name, " +
                    "patient_user.last_name AS patient_last_name, " +
                    "patient_user.gender AS patient_gender, " +
                    "doctor_user.name AS doctor_name, " +
                    "doctor_user.last_name AS doctor_last_name, " +
                    "equipment.name AS equipment_name, " +
                    "service.name AS service_name, " +
                    "service.duration AS duration, " +
                    "booking.start_time, " +
                    "booking.end_time " +
                    "FROM booking " +
                    "INNER JOIN patient ON booking.patient_id = patient.id " +
                    "INNER JOIN doctor ON booking.doctor_id = doctor.id " +
                    "INNER JOIN equipment ON booking.equipment_id = equipment.id " +
                    "INNER JOIN service ON booking.service_id = service.id " +
                    "INNER JOIN [user] AS patient_user ON patient.user_id = patient_user.id " +
                    "INNER JOIN [user] AS doctor_user ON doctor.user_id = doctor_user.id;";
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

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm login = new LoginForm();
            login.Show();
        }


        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Retrieve the selected row
                DataGridViewRow selectedRow = dgv.Rows[e.RowIndex];

                // Extract the data from the selected row
                string patientName = selectedRow.Cells["patient_name"].Value.ToString();
                string patientLastName = selectedRow.Cells["patient_last_name"].Value.ToString();
                string doctorName = selectedRow.Cells["doctor_name"].Value.ToString();
                string doctorLastName = selectedRow.Cells["doctor_last_name"].Value.ToString();
                string duration = selectedRow.Cells["duration"].Value.ToString();
                string startTime = selectedRow.Cells["start_time"].Value.ToString();

                // Extract other fields as needed

                // Create an instance of the external form and pass the retrieved data
                EditBooking editBooking = new EditBooking(patientName, patientLastName, doctorName, doctorLastName, duration, startTime);
                editBooking.ShowDialog();
            }
        }

        private void dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
