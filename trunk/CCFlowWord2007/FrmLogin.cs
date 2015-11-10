using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BP.Web;
using BP.Port;

namespace BP.Comm
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        public int loginNum;

        #region Load

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            var user = BP.WF.Profile.GetLoginNo();

            if (string.IsNullOrEmpty(user))
            {
                this.CB_SaveInfo.Checked = false;
            }
            else
            {
                this.CB_SaveInfo.Checked = true;
                this.TB_Pass.Text = "pub";
            }
        }

        #endregion

        #region btn Events

        private void Btn_OK_Click(object sender, EventArgs e)
        {
            WebUser.SignOut();
            try
            {
                loginNum++;
                if (loginNum > 5)
                    throw new Exception("@ You enter the wrong password frequently , The system rejects your retry , Please contact the administrator to resolve this issue .");

                Emp ue = new Emp(this.TB_User.Text);
                if (ue.Pass.Equals(this.TB_Pass.Text) == false)
                    throw new Exception("@ The password you entered is incorrect . \t\n@ Note that the password is case sensitive .");

                // Perform the login .
                BP.WF.Dev2Interface.Port_Login(ue.No, ue.Pass);


                WebUser.IsSaveInfo = this.CB_SaveInfo.Checked;
                WebUser.IsSavePass = this.CB_SavePass.Checked;

                WebUser.Sigin(ue);
                WebUser.FK_Flow = null;
                WebUser.FK_Node = 0;
                WebUser.WorkID = 0;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Log error ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Btn_C_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region Control Events

        private void CB_SavePass_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CB_SavePass.Checked)
            {
                this.CB_SaveInfo.Checked = true;
                this.CB_SaveInfo.Enabled = false;
            }
            else
            {
                this.CB_SaveInfo.Enabled = true;
            }
        }

        #endregion
    }
}
