using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0
{
    public partial class filmschedule : System.Web.UI.Page
    {
        public String title = ManagerData.title;
        protected String city = "";
        protected String date = "";
        protected Film detailFilm = new Film();
        protected Dictionary<String, Dictionary<String, Dictionary<String, List<Schedule>>>> listDate;
        protected String[] dmy;
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
                    try
                    {
                        HttpCookie cityCookie = Request.Cookies["city"];
                        if (cityCookie != null)
                        {
                            city = cityCookie.Value;
                        }
                    }
                    catch (Exception) { }
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vui lòng chọp thành phố hiện tại của bạn, để xem thông tin chính xác hơn!');", true);
                }

            }
            catch (Exception)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Vui lòng chọp thành phố hiện tại của bạn, để xem thông tin chính xác hơn!');", true);
            }
            try
            {
                date = Request["date"];
            }
            catch (Exception)
            {
                date = DateTime.Now.ToUniversalTime().AddHours(7.0).ToString("yyyy-MM-dd");
            }
            if (city == null)
            {
                city = "";
            }
            try
            {
                String id = Request["id"];             
                if (id != null)
                {
                    Film film = GetData.getDetailFilm(id);
                    if(film != null)
                    {
                        listDate = GetData.getDateByFilm(GetData.getScheduleByFilm(film.id, city));
                        var propInfo = film.GetType().GetProperties();
                        foreach (var item in propInfo)
                        {
                            detailFilm.GetType().GetProperty(item.Name).SetValue(detailFilm, item.GetValue(film, null), null);
                        }
                    }
                    else
                    {
                        Response.Redirect("default.aspx");
                    }
                }
                else
                {
                    Response.Redirect("default.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("default.aspx");
            }
        }
    }
}