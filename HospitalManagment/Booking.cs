using HospitalManagment.Models;
using System.Data;
using System.Data.SqlClient;
using QRCoder;
using System.Net.Mail;
using System.Net;
using System.Drawing.Imaging;
using System.Reflection.Metadata;
using iTextSharp.text.pdf;
using iTextSharp.text;
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

                    // Insert data into the booking table and retrieve the generated ID
                    SqlCommand bookingCmd = new SqlCommand(
                        "INSERT INTO booking (doctor_id, patient_id, service_id, equipment_id, has_priority, start_time, end_time) " +
                        "VALUES (@doctor_id, @patient_id, @service_id, @equipment_id, @has_priority, @start_time, @end_time); SELECT SCOPE_IDENTITY();", conn);
                    bookingCmd.Transaction = transaction;
                    bookingCmd.Parameters.AddWithValue("@doctor_id", doctorId);
                    bookingCmd.Parameters.AddWithValue("@patient_id", patientId);
                    bookingCmd.Parameters.AddWithValue("@service_id", serviceId);
                    bookingCmd.Parameters.AddWithValue("@equipment_id", equipmentId);
                    bookingCmd.Parameters.AddWithValue("@has_priority", comboBox3.Text);
                    bookingCmd.Parameters.AddWithValue("@start_time", startTime);
                    bookingCmd.Parameters.AddWithValue("@end_time", endTime);
                    int bookingId = Convert.ToInt32(bookingCmd.ExecuteScalar());

                    transaction.Commit();

                    bookingIdTxt.Text = bookingId.ToString();

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

        private void billBtn_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DatabaseManager.GetConnection())
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Retrieve the booking details based on the generated booking ID
                    string query = @"
            SELECT
                u_patient.name AS patient_name,
                u_patient.last_name AS patient_last_name,
                u_doctor.name AS doctor_name,
                u_doctor.last_name AS doctor_last_name,
                b.start_time,
                s.name AS service_name,
                u_patient.email AS patient_email
            FROM
                booking AS b
                INNER JOIN patient AS p ON b.patient_id = p.id
                INNER JOIN [user] AS u_patient ON p.user_id = u_patient.id
                INNER JOIN doctor AS d ON b.doctor_id = d.id
                INNER JOIN [user] AS u_doctor ON d.user_id = u_doctor.id
                INNER JOIN service AS s ON b.service_id = s.id
            WHERE
                b.id = @bookingId";

                    SqlCommand command = new SqlCommand(query, conn, transaction);
                    command.Parameters.AddWithValue("@bookingId", Convert.ToInt32(bookingIdTxt.Text));
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string patientName = reader["patient_name"].ToString();
                        string patientLastName = reader["patient_last_name"].ToString();
                        string doctorName = reader["doctor_name"].ToString();
                        string doctorLastName = reader["doctor_last_name"].ToString();
                        string startTime = reader["start_time"].ToString();
                        string serviceName = reader["service_name"].ToString().ToUpper();
                        string email = reader["patient_email"].ToString();

                        string bookingData = $"Service: {serviceName} APPOINTMENT" + Environment.NewLine +
                                           $"Patient: {patientName} {patientLastName}" + Environment.NewLine +
                                           $"Doctor: {doctorName} {doctorLastName}" + Environment.NewLine +
                                           $"Start Time: {startTime}";

                        billTxt.Text = bookingData;

                        // Use the same bookingData for QR code encoding
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(bookingData, QRCodeGenerator.ECCLevel.Q);
                        QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData);

                        // Convert QR code to bitmap image
                        Bitmap qrCodeImage = qrCode.GetGraphic(3);

                        QRBox.Image = qrCodeImage;

                        // Save the QR code image to a memory stream
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            ms.Seek(0, SeekOrigin.Begin);

                            // Create a new MailMessage instance
                            MailMessage mail = new MailMessage();

                            // Set the sender and recipient addresses
                            mail.From = new MailAddress("megi.xhafa2000@gmail.com");
                            mail.To.Add(email);

                            // Set the subject and body of the email
                            mail.Subject = "Your Appointment Details";
                            mail.IsBodyHtml = false;
                            mail.Body = "Your appointment details:" + Environment.NewLine + billTxt.Text;

                            // Attach the image from the memory stream
                            Attachment attachment = new Attachment(ms, "QRCode.png", "image/png");
                            mail.Attachments.Add(attachment);

                            // Send the email using SmtpClient
                            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
                            smtpServer.EnableSsl = true;
                            smtpServer.UseDefaultCredentials = false;
                            smtpServer.Credentials = new NetworkCredential("megi.xhafa2000@gmail.com", "ddvwpsmvrvbvvvwl");

                            smtpServer.Send(mail);
                            MessageBox.Show("Mail Sent.");
                        }
                    }

                    reader.Close();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating the prescription: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void scanQR_Click(object sender, EventArgs e)
        {
            if (QRBox.Image != null)
            {
                MessageBox.Show("QR Scanned!");
            }
            else
            {
                MessageBox.Show("No QR code image found.");
            }
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();

            // Set the file path for the PDF
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "appointment.pdf");

            // Create a new FileStream to write the PDF file
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Create a new PdfWriter using the FileStream
                PdfWriter writer = PdfWriter.GetInstance(document, fileStream);

                // Open the PDF document for writing
                document.Open();

                // Create a new paragraph and add the text from the textbox
                Paragraph paragraph = new Paragraph(billTxt.Text);

                // Add the paragraph to the document
                document.Add(paragraph);

                // Check if the PictureBox has an image
                if (QRBox.Image != null)
                {
                    // Convert the PictureBox image to iTextSharp's Image
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance((QRBox.Image as Bitmap), ImageFormat.Jpeg);

                    // Add the image to the document
                    document.Add(image);
                }

                // Close the document
                document.Close();
            }

            MessageBox.Show("PDF downloaded successfully!");

        }
    }
}