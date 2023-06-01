using HospitalManagment.Models;

namespace HospitalManagment
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {
            int roleId = role.RoleId;

            // Enable or disable buttons based on the role
            if (roleId == 1)
            {
                doctorBtn.Enabled = true;
                receptionBTN.Enabled = true;
                patientBTN.Enabled = true;
                labBtn.Enabled = true;
                equipmentBtn.Enabled = true;
            }
            else if (roleId == 2)
            {
                labBtn.Enabled = true;
                equipmentBtn.Enabled = true;
            }
            else if (roleId == 3)
            {
                patientBTN.Enabled = true;
            }
        }

        private void equipmentBtn_Click(object sender, EventArgs e)
        {

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
    }
}
