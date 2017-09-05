using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0
{
    public partial class detailcinema : System.Web.UI.Page
    {
        public String title = ManagerData.title;
        protected Cinema detailCinema = new Cinema();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                String city = Request["city"];
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
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vui lòng chọp thành phố hiện tại của bạn, để xem thông tin chính xác hơn!');", true);
            }
            try
            {
                String id = Request["id"];
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + id + "')", true);
                if (id != null)
                {
                    detailCinema = GetData.getDetailCinema(id);
                    if(detailCinema == null)
                    {
                        Response.Redirect("listcinema.aspx");
                    }
                }
                else
                {
                    Response.Redirect("listcinema.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("listcinema.aspx");
            }
        }
    }
}