using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected String url;
        public String city = "";
        public String title = ManagerData.title;
        //protected List<Event> listEvent = GetData.getEventsBeta();
        protected List<Location> listCity = GetData.getCity();

        public void Page_Init(object o, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ManagerData.update();
            try
            {
                city = Request["city"];
                if (city == null)
                {
                    city = "";
                }
                if (city != "")
                {
                    HttpCookie cityCk = new HttpCookie("city", city);
                    cityCk.Expires = DateTime.Now.AddYears(100);
                    Response.Cookies.Add(cityCk);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vui lòng chọp thành phố hiện tại của bạn, để xem thông tin chính xác hơn!');", true);
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vui lòng chọp thành phố hiện tại của bạn, để xem thông tin chính xác hơn!');", true);
            }
        }
  
    }
}