using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.ClientServices;
using System.Web.Routing;
using System.IO;
using System.Globalization;
using System.Net;

namespace Cinema_2._0
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        DBCinemaDataContext dbCinema = new DBCinemaDataContext();
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // Code that runs on application startup
            //string path = Server.MapPath("~") + "\\count.txt";
            //if (!File.Exists(path))
            //    File.WriteAllText(path, "0");
            //Application["Access"] = int.Parse(File.ReadAllText(path));
            Application["Access"] = int.Parse(dbCinema.UpdateHistories.FirstOrDefault(ud => ud.key == "CountAccess").date.Trim());
        }
        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
            //string path = Server.MapPath("~") + "\\count.txt";

            //File.WriteAllText(path, Application["Access"].ToString());
            dbCinema.UpdateHistories.FirstOrDefault(ud => ud.key == "CountAccess").date = Application["Access"].ToString();
            dbCinema.SubmitChanges();
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
        void Session_Start(object sender, EventArgs e)
        {
            string path = Server.MapPath("~") + "\\count.txt";
            // Code that runs when a new session is started
            if (Application["Online"] == null)
                Application["Online"] = 3;
            else
                Application["Online"] = (int)Application["Online"] + 1;
            try
            {
                Application["Access"] = (int)Application["Access"] + 1;
                dbCinema.UpdateHistories.FirstOrDefault(ud => ud.key == "CountAccess").date = Application["Access"].ToString();
                dbCinema.SubmitChanges();
            }
            catch (Exception) { }
            try
            {
                UserRequest req = new UserRequest();
                req.ip = Request.UserHostAddress;
                req.id = (int)Application["Access"];
                req.time = DateTime.Now.ToUniversalTime().AddHours(7.0).ToString();
                req.note = new WebClient().DownloadString("http://ipinfo.io/" + req.ip + "/country").Trim() + "-" + Request.Url.PathAndQuery;
                dbCinema.UserRequests.InsertOnSubmit(req);
                dbCinema.SubmitChanges();
            }
            catch (Exception) { }
        }

        void Session_End(object sender, EventArgs e)
        {
            int i = (int)Application["Online"];
            if (i > 1)
                Application["Online"] = i - 1;
            try
            {
                dbCinema.UpdateHistories.FirstOrDefault(ud => ud.key == "CountAccess").date = Application["Access"].ToString();
                dbCinema.SubmitChanges();
            }
            catch (Exception) { }
        }
    }
}