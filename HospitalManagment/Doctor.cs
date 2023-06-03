using HospitalManagment.Models;
using Microsoft.VisualBasic.ApplicationServices;
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
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace HospitalManagment
{
    public partial class Doctor : Form
    {

        public Doctor()
        {
            InitializeComponent();
        }

        private void Doctor_Load(object sender, EventArgs e)
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
                    SqlCommand userCmd = new SqlCommand(
                        "INSERT INTO [user] (name, last_name, gender, number, email, address) " +
                        "OUTPUT inserted.id " +
                        "VALUES (@name, @lastName, @gender, @number, @email, @address);", conn);
                    userCmd.Transaction = transaction;
                    userCmd.Parameters.AddWithValue("@name", nameTxt.Text);
                    userCmd.Parameters.AddWithValue("@lastName", lastnameTxt.Text);
                    userCmd.Parameters.AddWithValue("@gender", roleScroll.Text);
                    userCmd.Parameters.AddWithValue("@number", numberTxt.Text);
                    userCmd.Parameters.AddWithValue("@email", emailTxt.Text);
                    userCmd.Parameters.AddWithValue("@address", addressTxt.Text);

                    int userId = Convert.ToInt32(userCmd.ExecuteScalar());

                    SqlCommand doctorAccCmd = new SqlCommand(
                    "INSERT INTO user_account(username, password, role_id) " +
                    "VALUES (@username, @password, @roleId); SELECT SCOPE_IDENTITY();", conn);
                    doctorAccCmd.Transaction = transaction;
                    doctorAccCmd.Parameters.AddWithValue("@username", (nameTxt.Text+lastnameTxt.Text).ToLower());
                    doctorAccCmd.Parameters.AddWithValue("@password", PasswordGenerate.GenerateRandomPassword(8));
                    doctorAccCmd.Parameters.AddWithValue("@roleId", 2);

                    int doctorAccId = Convert.ToInt32(doctorAccCmd.ExecuteScalar());

                    // Insert data into the doctor table
                    SqlCommand doctorCmd = new SqlCommand(
                        "INSERT INTO doctor (useraccount_id, user_id, experience, specialization) " +
                        "VALUES (@userAccountId, @userId, @experience, @specialization);", conn);
                    doctorCmd.Transaction = transaction;
                    doctorCmd.Parameters.AddWithValue("@userAccountId", doctorAccId);
                    doctorCmd.Parameters.AddWithValue("@userId", userId);
                    doctorCmd.Parameters.AddWithValue("@experience", experienceTxt.Text);
                    doctorCmd.Parameters.AddWithValue("@specialization", specializationTxt.Text);
                    doctorCmd.ExecuteNonQuery();

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

        private void homeBtn_Click(object sender, EventArgs e)
        {
            Doctor doctor = new Doctor();
            doctor.Show();
            this.Hide();
        }
    }
}
