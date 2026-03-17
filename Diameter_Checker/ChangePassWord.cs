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
    public partial class ChangePassWord : Form
    {
        public ChangePassWord()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Communication.connectDatabase();
            string oldPass="";
            int cnt = 0;
            bool flg = false;
            using (SqlDataReader myReader = new SqlCommand("SELECT * FROM PasswordSetting", Communication.connect).ExecuteReader())
            {
                while (myReader.Read())
                {
                    oldPass = myReader["password"].ToString();
                    cnt++;
                }
            }
            if(this.txtOldpwd.Text != "HallaNo1" && txtOldpwd.Text != oldPass)
            {
                this.lblstt.Text = "Mật khẩu cũ không chính xác!";
            }else if (txtNewpwd.Text.Trim() == "")
            {
                this.lblstt.Text = "Mật khẩu mới không được để trống!";
            }
            else
            {
                flg = true;
                if(cnt > 0)
                {
                    using (SqlCommand command = Communication.connect.CreateCommand())
                    {
                        command.CommandText = "UPDATE PassWordSetting SET password = @pass";
                        command.Parameters.AddWithValue("@pass", txtNewpwd.Text);
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand command1 = Communication.connect.CreateCommand())
                    {
                        command1.CommandText = "Insert into PassWordSetting(password) values(@pass)";
                        command1.Parameters.AddWithValue("@pass", txtNewpwd.Text);
                        command1.ExecuteNonQuery();
                    }
                }
            }
            Communication.connect.Close();
            if (flg) Dispose();

        }
    }
}
