using HospitalManagment.Models;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Data;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using System.Data.SqlClient;


namespace HospitalManagment
{
    public partial class BookingTimetable : Form
    {
        public BookingTimetable()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm login = new LoginForm();
            login.Show();
        }

        private void BookingTimetable_Load(object sender, EventArgs e)
        {
            LoadChart();
        }

        private void LoadChart()
        {


        }


    }
}
