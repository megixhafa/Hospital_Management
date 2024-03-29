﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using HospitalManagment.Models;
using System.Data.SqlClient;

namespace HospitalManagment
{
    public partial class EditBooking : Form
    {
        public EditBooking(int id, string patientName, string patientLastName, string doctorName, string doctorLastName, string duration, string startTime)
        {
            InitializeComponent();
            bookingId.Text = id.ToString();
            nameTxt.Text = patientName;
            lastnameTxt.Text = patientLastName;
            doctorTxt.Text = doctorName;
            doctorLastTxt.Text = doctorLastName;
            durationTxt.Text = duration;
            startTimeTxt.Text = startTime;  
        }

        private void EditBooking_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            string name = nameTxt.Text;

            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();

                SqlCommand updateCmd = new SqlCommand(
                    "UPDATE booking " +
                    "SET start_time = @startTime " +
                    "WHERE booking.id = @id", conn);
                updateCmd.Parameters.AddWithValue("@startTime", startTimeTxt.Text);
                updateCmd.Parameters.AddWithValue("@id", bookingId.Text);
                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Update Successful!");
                }
                else
                {
                    MessageBox.Show("Error!");
                }

                SqlCommand command = new SqlCommand("SELECT [user].email FROM [user] JOIN patient ON patient.user_id = [user].id WHERE [user].name = @name", conn);
                command.Parameters.AddWithValue("@name", name);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    string email = reader["email"].ToString();

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
                        mail.To.Add(email);
                        mail.Subject = "";
                        mail.IsBodyHtml = false;
                        string body = "Your appointment has been changed to the following date: " + startTimeTxt.Text + Environment.NewLine;
                        mail.Body = body;

                        smtpServer.Send(mail);
                        MessageBox.Show("Mail Sent.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                reader.Close();
                conn.Close();
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm login = new LoginForm();
            login.Show();
        }
    }
}
