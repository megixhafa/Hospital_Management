using HospitalManagment;
using HospitalManagment.Models;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace HospitalManagment
{
    public partial class Booking : Form
    {
        public Booking()
        {
            InitializeComponent();
            LoadPatient();
            LoadDoctor();
            LoadService();
            LoadEquipment();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        public void LoadPatient()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT u.name, u.last_name FROM [user] u INNER JOIN patient p ON u.id = p.user_id", conn);
                SqlDataReader readerPatients = cmd.ExecuteReader();
                DataTable dtPatients = new DataTable();
                dtPatients.Load(readerPatients);
                comboBox2.DataSource = dtPatients;
                comboBox2.DisplayMember = "name";

                conn.Close();
            }
        }

        private void LoadDoctor()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT u.name, u.last_name FROM [user] u INNER JOIN doctor d ON u.id = d.user_id", conn);
                SqlDataReader readerPatients = cmd.ExecuteReader();
                DataTable dtPatients = new DataTable();
                dtPatients.Load(readerPatients);
                comboBox1.DataSource = dtPatients;
                comboBox1.DisplayMember = "name";
                conn.Close();
            }
        }

        private void LoadService(){ 
        
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT name FROM service", conn);
                SqlDataReader readerPatients = cmd.ExecuteReader();
                DataTable dtPatients = new DataTable();
                dtPatients.Load(readerPatients);
                comboBox4.DataSource = dtPatients;
                comboBox4.DisplayMember = "name";

                conn.Close();
            }
        }

        private void LoadEquipment()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT name FROM equipment", conn);
                SqlDataReader readerPatients = cmd.ExecuteReader();
                DataTable dtPatients = new DataTable();
                dtPatients.Load(readerPatients);
                comboBox5.DataSource = dtPatients;
                comboBox5.DisplayMember = "name";
                conn.Close();
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    DateTime startTime = dateTimePicker.Value;
                    TimeSpan selectedTime = startTime.TimeOfDay;
                    string hourMinute = selectedTime.ToString("hh\\:mm");

                    var minutesToAdd = (int)numericUpDown1.Value;
                    DateTime endTime = startTime.AddMinutes(minutesToAdd);

                    SqlCommand doctor = new SqlCommand("SELECT d.id FROM doctor d INNER JOIN [user] u ON u.id = d.user_id WHERE u.name = @name", conn);
                    doctor.Transaction = transaction;
                    doctor.Parameters.AddWithValue("@name", comboBox1.Text);
                    int doctorId = Convert.ToInt32(doctor.ExecuteScalar());

                    SqlCommand patient = new SqlCommand("SELECT p.id FROM patient p INNER JOIN [user] u ON u.id = p.user_id WHERE u.name = @name", conn);
                    patient.Transaction = transaction;
                    patient.Parameters.AddWithValue("@name", comboBox2.Text);
                    int patientId = Convert.ToInt32(patient.ExecuteScalar());

                    SqlCommand service = new SqlCommand("SELECT id FROM service WHERE name = @name", conn);
                    service.Transaction = transaction;
                    service.Parameters.AddWithValue("@name", comboBox4.Text);
                    int serviceId = Convert.ToInt32(service.ExecuteScalar());

                    SqlCommand equipment = new SqlCommand("SELECT id FROM equipment WHERE name = @name", conn);
                    equipment.Transaction = transaction; 
                    equipment.Parameters.AddWithValue("@name", comboBox5.Text);
                    int equipmentId = Convert.ToInt32(equipment.ExecuteScalar());

                    // Insert data into the user table and retrieve the generated ID
                    SqlCommand bookingCmd = new SqlCommand(
                        "INSERT INTO booking (doctor_id, patient_id, service_id, equipment_id, has_priority, start_time, end_time) " +
                        "VALUES (@doctor_id, @patient_id, @service_id, @equipment_id, @has_priority, @start_time, @end_time); SELECT SCOPE_IDENTITY();", conn);
                    bookingCmd.Transaction = transaction;
                    bookingCmd.Parameters.AddWithValue("@doctor_id", doctorId);
                    bookingCmd.Parameters.AddWithValue("@patient_id", patientId);
                    bookingCmd.Parameters.AddWithValue("@service_id", serviceId);
                    bookingCmd.Parameters.AddWithValue("@equipment_id", equipmentId);
                    bookingCmd.Parameters.AddWithValue("@has_priority", comboBox3.Text);
                    bookingCmd.Parameters.AddWithValue("@start_time", selectedTime);
                    bookingCmd.Parameters.AddWithValue("@end_time", endTime);
                    bookingCmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data inserted successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error occurred during insertion: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void Booking_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}