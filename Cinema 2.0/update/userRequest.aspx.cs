using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0.update
{
    public partial class userRequest : System.Web.UI.Page
    {
        public List<UserRequest> listUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.listUser = ManagerData.getListUserRequest();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ManagerData.clearUserRequest();
        }
    }
}