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
    public partial class updateEvents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ManagerData.clearEvent();
            Response.Write("<table border='1'>");
            foreach (var ev in ManagerData.updateEvents())
            {
                Response.Write("<tr> <td>" + ev.id + "</td> <td>" + ev.name + "</td> <td>" + ev.idProducer + "</td> <td>" + ev.time + "</td> </tr>");
            }
            Response.Write("</table>");
        }

    }
}