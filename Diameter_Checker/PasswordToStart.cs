using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            if (this.txtPassword.Text == "123456")
            {
                Communication.isStartPassword = true;
                this.Close();
            }
            else
            {
                this.lblInfo.Text = "Mật khẩu không đúng!";
            }
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
    }
}
