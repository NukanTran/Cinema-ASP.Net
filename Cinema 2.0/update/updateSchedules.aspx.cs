using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cinema_2._0.Manager;

namespace Cinema_2._0.update
{
    public partial class updateSchedules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ManagerData.clearSchedule();
            Response.Write("<table border='1'>");
            foreach (var schedule in ManagerData.updateSchedules())
            {
                Response.Write("<tr> <td>" + schedule.id + "</td><td>" + schedule.dateTime + "</td><td>" + schedule.slot + "</td> </tr>");
            }
            Response.Write("</table></td></tr>");
            ManagerData.clearFilm();
        }
    }
}