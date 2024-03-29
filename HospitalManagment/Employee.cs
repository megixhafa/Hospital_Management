﻿using HospitalManagment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HospitalManagment
{
    public partial class Employee : Form
    {
        public Employee()
        {
            InitializeComponent();
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string username = (nameTxt.Text + lastnameTxt.Text).ToLower();
                    string password = PasswordGenerate.GenerateRandomPassword(8);

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

                    SqlCommand employeeAccCmd = new SqlCommand(
                    "INSERT INTO user_account(username, password, role_id) " +
                    "VALUES (@username, @password, @roleId); SELECT SCOPE_IDENTITY();", conn);
                    employeeAccCmd.Transaction = transaction;
                    employeeAccCmd.Parameters.AddWithValue("@username", username);
                    employeeAccCmd.Parameters.AddWithValue("@password", password);
                    employeeAccCmd.Parameters.AddWithValue("@roleId", 3);

                    int employeeAccId = Convert.ToInt32(employeeAccCmd.ExecuteScalar());

                    // Insert data into the doctor table
                    SqlCommand employeeCmd = new SqlCommand(
                        "INSERT INTO employee (useraccount_id, user_id) " +
                        "VALUES (@userAccountId, @userId);", conn);
                    employeeCmd.Transaction = transaction;
                    employeeCmd.Parameters.AddWithValue("@userAccountId", employeeAccId);
                    employeeCmd.Parameters.AddWithValue("@userId", userId);
                    employeeCmd.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Data inserted successfully.");

                    string email = emailTxt.Text;

                    try
                    {
                        SmtpClient smtpServer = new SmtpClient();
                        MailMessage mail = new MailMessage();

                        smtpServer.EnableSsl = true;
                        smtpServer.UseDefaultCredentials = false;
                        smtpServer.Credentials = new NetworkCredential("megi.xhafa2000@gmail.com", "ddvwpsmvrvbvvvwl");
                        smtpServer.Port = 587;
                        smtpServer.Host = "smtp.gmail.com";

                        mail.From = new MailAddress("megi.xhafa2000@gmail.com");
                        mail.To.Add(emailTxt.Text);
                        mail.Subject = "";
                        mail.IsBodyHtml = false;
                        string body = "Your credentials" + Environment.NewLine +
                                      "Username: " + username + Environment.NewLine +
                                      "Password: " + password + Environment.NewLine;
                        mail.Body = body;

                        smtpServer.Send(mail);
                        MessageBox.Show("Mail Sent.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
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
            Home home = new Home();
            this.Hide();
            home.Show();
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
    }
}
