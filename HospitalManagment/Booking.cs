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
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void LoadPatient()
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT u.name, u.last_name FROM [user] u INNER JOIN patient p ON u.id = p.user_id", conn);
                SqlDataReader readerPatients = cmd.ExecuteReader();
                DataTable dtPatients = new DataTable();
                dtPatients.Load(readerPatients);
                comboBox2.DataSource = dtPatients;
                comboBox2.DisplayMember = "Name";
            //    comboBox2.ValueMember = "PatientId";
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
                comboBox1.DisplayMember = "Name";
                //    comboBox2.ValueMember = "PatientId";
                conn.Close();
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}