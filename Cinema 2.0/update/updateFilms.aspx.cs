using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fizzler;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text;
using Cinema_2._0.Manager;

namespace Cinema_2._0.API
{

    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ManagerData.clearSchedule();
            //ManagerData.clearFilm();
            Response.Write("<table border='1'>");
            foreach (var film in ManagerData.updateFilms())
            {
                Response.Write("<tr> <td>" + film.id + "</td> <td>" + film.linkPoster + "</td><td>");
                Response.Write("<table border='1'>");
                foreach (var schedule in film.Schedules)
                {
                    Response.Write("<tr> <td>" + schedule.idCinema + "</td><td>" + schedule.dateTime + "</td><td>" + schedule.slot + "</td> </tr>");
                }
                Response.Write("</table></td></tr>");
            }
            Response.Write("</table>");
        }

    }
}