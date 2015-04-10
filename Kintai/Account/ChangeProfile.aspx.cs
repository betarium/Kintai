using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kintai.Account
{
    public partial class ChangeProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FullName.Text = (string)HttpContext.Current.Profile.GetPropertyValue("FullName");
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FullName.Text))
            {
                throw new ArgumentNullException("FullName");
            }
            HttpContext.Current.Profile.SetPropertyValue("FullName", FullName.Text);
            InfoMessage.Text = "保存しました。";
        }

    }
}