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
    public partial class demo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<table border='1'>");
            foreach (var cinema in ManagerData.updateCinemas())
            {
                Response.Write("<tr> <td>" + cinema.id + "</td> <td>" + cinema.name + "</td> <td>" + cinema.idProducer + "</td> <td>" + cinema.idLocation + "</td> </tr>");
            }
            Response.Write("</table>");
        }

    }
}