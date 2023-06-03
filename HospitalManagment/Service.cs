using HospitalManagment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalManagment
{
    public partial class Service : Form
    {
        public Service()
        {
            InitializeComponent();
        }

        private void Service_Load(object sender, EventArgs e)
        {

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert data into the user table and retrieve the generated ID
                    SqlCommand serviceCmd = new SqlCommand(
                        "INSERT INTO service (name, description) " +
                        "VALUES (@name, @description);", conn);
                    serviceCmd.Transaction = transaction;
                    serviceCmd.Parameters.AddWithValue("@name", serviceTxt.Text);
                    serviceCmd.Parameters.AddWithValue("@description", descriptionTxt.Text);

                    // Insert data into the doctor table
                    SqlCommand equipmentCmd = new SqlCommand(
                        "IF EXISTS (SELECT 1 FROM equipment WHERE name = @name) " +
                        "   UPDATE equipment SET quantity = quantity + 1 WHERE name = @name " +
                        "ELSE " +
                        "   INSERT INTO equipment (name, quantity) VALUES (@name, 1);", conn);
                    equipmentCmd.Transaction = transaction;
                    equipmentCmd.Parameters.AddWithValue("@name", equipmentTxt.Text);

                    equipmentCmd.ExecuteNonQuery();

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
    }
}
