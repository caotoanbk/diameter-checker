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

namespace Diameter_Checker
{
    public partial class PasswordToStart : Form
    {
        public PasswordToStart()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void handleOkClick()
        {
            Communication.connectDatabase();
            string oldPass = "";

            using (SqlDataReader myReader = new SqlCommand("SELECT * FROM PasswordSetting", Communication.connect).ExecuteReader())
            {
                while (myReader.Read())
                {
                    oldPass = myReader["password"].ToString();
                }
            }
            if (this.txtPassword.Text == "HallaNo1" || txtPassword.Text == oldPass)
            {
                Communication.isStartPassword = true;
                this.Close();
            }
            else
            {
                this.lblInfo.Text = "Mật khẩu không đúng!";
            }
            Communication.connect.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            handleOkClick();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                handleOkClick();
            }
        }

        private void PasswordToStart_Load(object sender, EventArgs e)
        {
            if (!Communication.isPreviousNG)
            {
                label1.Text = "Please input password to continue";
            }
        }
    }
}
