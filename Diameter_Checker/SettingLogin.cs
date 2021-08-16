using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Diameter_Checker
{
    public class SettingLogin : Form
    {
        private IContainer components = null;

        private TextBox txtPassword;

        private Label label9;

        private Label label12;

        private ComboBox cmbUsername;

        private Button btnExit;

        private Button btnEnter;

        public SettingLogin()
        {
            this.InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if ((this.cmbUsername.Text != "Admin" ? false : this.txtPassword.Text == "0913183822"))
            {
                Communication.loginUser = "Admin";
                (new ComSetting()).ShowDialog();
                base.Close();
            }
            else if ((this.cmbUsername.Text != "Developer" ? true : this.txtPassword.Text != "halla913183822"))
            {
                MessageBox.Show("Wrong Username & Password!", "Warning!");
            }
            else
            {
                Communication.loginUser = "Developer";
                (new ComSetting()).ShowDialog();
                base.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            base.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtPassword = new TextBox();
            this.label9 = new Label();
            this.label12 = new Label();
            this.cmbUsername = new ComboBox();
            this.btnExit = new Button();
            this.btnEnter = new Button();
            base.SuspendLayout();
            this.txtPassword.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.txtPassword.ForeColor = SystemColors.InactiveCaptionText;
            this.txtPassword.Location = new Point(114, 49);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(153, 26);
            this.txtPassword.TabIndex = 87;
            this.label9.AutoSize = true;
            this.label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label9.Location = new Point(34, 53);
            this.label9.Name = "label9";
            this.label9.Size = new Size(71, 16);
            this.label9.TabIndex = 84;
            this.label9.Text = "Password:";
            this.label12.AutoSize = true;
            this.label12.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.label12.Location = new Point(34, 23);
            this.label12.Name = "label12";
            this.label12.Size = new Size(74, 16);
            this.label12.TabIndex = 83;
            this.label12.Text = "Username:";
            this.cmbUsername.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 163);
            this.cmbUsername.ForeColor = SystemColors.InactiveCaptionText;
            this.cmbUsername.FormattingEnabled = true;
            this.cmbUsername.Items.AddRange(new object[] { "Admin", "Developer" });
            this.cmbUsername.Location = new Point(114, 17);
            this.cmbUsername.Name = "cmbUsername";
            this.cmbUsername.Size = new Size(153, 28);
            this.cmbUsername.TabIndex = 86;
            this.cmbUsername.Text = "Admin";
            this.btnExit.ForeColor = Color.Black;
            this.btnExit.Location = new Point(158, 86);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(80, 30);
            this.btnExit.TabIndex = 89;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnEnter.ForeColor = Color.Black;
            this.btnEnter.Location = new Point(77, 86);
            this.btnEnter.Name = "btnEnter";
            this.btnEnter.Size = new Size(80, 30);
            this.btnEnter.TabIndex = 88;
            this.btnEnter.Text = "Login";
            this.btnEnter.UseVisualStyleBackColor = true;
            this.btnEnter.Click += new EventHandler(this.btnEnter_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(306, 128);
            base.Controls.Add(this.btnExit);
            base.Controls.Add(this.btnEnter);
            base.Controls.Add(this.cmbUsername);
            base.Controls.Add(this.txtPassword);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label12);
            base.Name = "SettingLogin";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "SettingLogin";
            base.Load += new EventHandler(this.SettingLogin_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void SettingLogin_Load(object sender, EventArgs e)
        {
        }
    }
}