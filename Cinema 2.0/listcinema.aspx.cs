using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0
{
    public partial class listcinema : System.Web.UI.Page
    {
        protected String city = "";
        protected List<Producer> listCinema;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                city = Request["city"];
                if (city != "")
                {
                    HttpCookie cityCk = new HttpCookie("city", city);
                    cityCk.Expires = DateTime.Now.AddYears(100);
                    Response.Cookies.Add(cityCk);
                }
                else
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vui lòng chọp thành phố hiện tại của bạn, để xem thông tin chính xác hơn!');", true);
                }
            }
            catch (Exception)
            {
                try
                {
                    HttpCookie cityCookie = Request.Cookies["city"];
                    if (cityCookie != null)
                    {
                        city = cityCookie.Value;
                    }
                }
                catch (Exception) { }
            }
            if (city == null)
            {
                city = "";
            }
            listCinema = GetData.getListProducer(city);
        }
    }
}