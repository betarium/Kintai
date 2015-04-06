using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Profile;

namespace Kintai.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

            // https://msdn.microsoft.com/en-us/library/ms178342(v=vs.140).aspx
            TextBox fullNameTextBox =
              (TextBox)RegisterUserWizardStep.ContentTemplateContainer.FindControl("FullName");

            ProfileBase prof = ProfileBase.Create(RegisterUser.UserName);
            prof.SetPropertyValue("FullName", fullNameTextBox.Text);
            prof.Save();
            // MembershipUser current = Membership.GetUser(user.Name);
            // Membership.UpdateUser(user);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

        protected void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            CreateUserWizard cuw = (CreateUserWizard)sender;
            //cuw.Email = cuw.UserName;
            cuw.UserName = cuw.Email;
        }

    }
}
