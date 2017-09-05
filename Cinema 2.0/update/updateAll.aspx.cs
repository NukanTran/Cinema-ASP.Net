using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0.update
{
    public partial class updateAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<table border='1'>");
            Response.Write("<tr><td> Events </td><td>" + ManagerData.updateEvents().Count + "</td></tr>");
            //ManagerData.clearSchedule();
            //ManagerData.clearSchedule();
            Response.Write("<tr><td> Films </td><td>" + ManagerData.updateFilms().Count + "</td></tr>");
            //Response.Write("<tr><td> Schedules </td><td>" + ManagerData.updateSchedules().Count + "</td></tr>");
            Response.Write("</table>");
        }
    }
}