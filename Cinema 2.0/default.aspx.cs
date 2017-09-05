using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0
{
    public partial class _default : System.Web.UI.Page
    {
        public String title = ManagerData.title;
        protected String city = "";
        protected String date;
        protected List<Film> listFilm;
        protected List<String> listDate;
        protected String[] dmy;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                city = Request["city"];
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
            listDate = GetData.getDate("");
            if (city == null){
                city = "";
            }
            try
            {
                date = Request["date"];
                listFilm = GetData.getFilmByDate(city, date);
            }
            catch (Exception) {
                if (listDate.Count > 0)
                {
                    date = listDate[0];
                    listFilm = GetData.getFilmByDate(city, date);
                }
                else
                {
                    date = DateTime.Now.ToUniversalTime().AddHours(7.0).ToString("yyyy-MM-dd");
                    listFilm = GetData.getFilmByDate(city, date);
                }
            }
        }
    }
}