using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0
{
    public partial class listfilm : System.Web.UI.Page
    {
        public String title = ManagerData.title;
        protected List<Film> listFilmShow;
        protected List<Film> listFilmDontShow;
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
            listFilmShow = GetData.getFilmShow();
            listFilmDontShow = GetData.getFilmDontShow();
        }
    }
}