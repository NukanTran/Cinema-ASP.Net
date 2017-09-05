using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using System.Text;
using System.Threading;


using System.Net;
using System.IO;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using System.Web.Http;
using System.Xml.Serialization;
using System.Collections.Specialized;

namespace Cinema_2._0.Manager
{
    public class ManagerData
    {
        public static String title = "Lịch chiếu phim đặt vé và tra cứu thông tin khuyến mãi, vé xem phim, phim mới, rạp phim. ";
        static ManagerData share = new ManagerData();
        DBCinemaDataContext dbCinema = new DBCinemaDataContext();
        HtmlWeb htmlWeb = new HtmlWeb()
        {
            AutoDetectEncoding = false,
            OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
        };

        public static void update()
        {
            try
            {
                if (DateTime.ParseExact(share.dbCinema.UpdateHistories.FirstOrDefault(ht => ht.key.Trim() == "updateAll").date.Trim(), "dd/MM/yyyy HH:mm:ss", null) < DateTime.Now.ToUniversalTime().AddHours(2.0))
                {
                    share.dbCinema.UpdateHistories.FirstOrDefault(ht => ht.key.Trim() == "updateAll").date = DateTime.Now.ToUniversalTime().AddHours(7.0).ToString("dd/MM/yyyy HH:mm:ss");
                    share.dbCinema.SubmitChanges();
                    (new Thread(new ThreadStart(share.updateAll))).Start();
                }
            }
            catch (Exception e)
            {
                share.log("UpdateAll***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
        }

        void updateAll()
        {
            ManagerData.updateEvents();
            ManagerData.updateFilms();
        }

        public static void clearFilm()
        {
            try
            {
                share.dbCinema.Films.DeleteAllOnSubmit(getListFilmEnd(share.dbCinema.Films.ToList()));
                //share.dbCinema.Films.DeleteAllOnSubmit(share.dbCinema.Films);
                share.dbCinema.SubmitChanges();
            }
            catch (Exception e)
            {
                share.log("Clear film***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
        }

        private void log(string log)
        {
            string path = HttpRuntime.AppDomainAppPath + "\\log.txt";
            File.WriteAllText(path, log);
        }

        public static void clearEvent()
        {
            share.dbCinema.Events.DeleteAllOnSubmit(share.dbCinema.Events.Where(ev => ev.endTime != null
                    && ev.endTime < DateTime.Now.ToUniversalTime().AddHours(7.0)));
            share.dbCinema.SubmitChanges();
        }

        public static void clearEventBeta()
        {
            share.dbCinema.Events.DeleteAllOnSubmit(share.dbCinema.Events.Where(ev => ev.idProducer == "beta"));
            share.dbCinema.SubmitChanges();
        }

        public static void clearSchedule()
        {
            share.dbCinema.Schedules.DeleteAllOnSubmit(getListScheduleEnd(share.dbCinema.Schedules.ToList()));
            share.dbCinema.SubmitChanges();
        }

        public static void clearUserRequest()
        {
            share.dbCinema.UserRequests.DeleteAllOnSubmit(share.dbCinema.UserRequests);
            share.dbCinema.SubmitChanges();
        }

        public static List<UserRequest> getListUserRequest()
        {
            return share.dbCinema.UserRequests.ToList();
        }

        public static List<Film> getListFilm(List<Film> listData, string location)
        {
            var list = new List<Film>();
            foreach (var i in listData)
            {
                if (i.premiere >= DateTime.Now.ToUniversalTime().AddHours(7.0) || getListSchedule(i.Schedules.ToList(), location).Count > 0)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        public static List<Film> getListFilmEnd(List<Film> listData)
        {
            var list = new List<Film>();
            foreach (var i in listData)
            {
                if (i.premiere < DateTime.Now.ToUniversalTime().AddHours(7.0) && getListSchedule(i.Schedules.ToList(), "").Count < 1)
                {
                    list.Add(i);
                }
            }
            return list;
        }

        public static List<Schedule> getListSchedule(List<Schedule> listData, string location)
        {
            var list = new List<Schedule>();
            foreach (var i in listData)
            {
                if (DateTime.ParseExact(i.dateTime, "yyyy-MM-dd HH:mm", null) >= DateTime.Now.ToUniversalTime().AddHours(7.0) 
                    && (location == "" || i.Cinema.idLocation.Trim().ToUpper() == location.Trim().ToUpper()))
                {
                    list.Add(i);
                }
            }
            return list;
        }

        public static List<View_Schedule> getListScheduleView(List<View_Schedule> listData)
        {
            var list = new List<View_Schedule>();
            foreach (var i in listData)
            {
                if (DateTime.ParseExact(i.dateTime, "yyyy-MM-dd HH:mm", null) >= DateTime.Now.ToUniversalTime().AddHours(7.0))
                {
                    var date = DateTime.Now.ToUniversalTime().AddHours(7.0);
                    list.Add(i);
                }
            }
            return list;
        }


        public static List<Schedule> getListScheduleEnd(List<Schedule> listData)
        {
            var list = new List<Schedule>();
            foreach (var i in listData)
            {
                if (DateTime.ParseExact(i.dateTime, "yyyy-MM-dd HH:mm", null) < DateTime.Now.ToUniversalTime().AddHours(7.0))
                {
                    list.Add(i);
                }
            }
            return list;
        }

        static public List<Cinema> updateCinemas()
        {
            var listProducer = share.loadProducers();
            var listLocation = share.loadLocations();
            if (listProducer.Count > 0)
            {
                share.dbCinema.Producers.InsertAllOnSubmit(listProducer);
            }
            if (listLocation.Count > 0)
            {
                share.dbCinema.Locations.InsertAllOnSubmit(listLocation);
            }
            try
            {
                share.dbCinema.SubmitChanges();
            }
            catch (Exception e)
            {
                share.log("UpdateLocation***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
            var listCinema = share.loadCinemas();
            if (listCinema.Count > 0)
            {
                share.dbCinema.Cinemas.InsertAllOnSubmit(listCinema);
            }
            try
            {
                share.dbCinema.SubmitChanges();
            }
            catch (Exception e)
            {
                share.log("UpdateCinema***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
            return listCinema;
        }

        static public List<Event> updateEvents()
        {
            ManagerData.clearEvent();
            var listProducer = share.loadProducers();
            if (listProducer.Count > 0)
            {
                share.dbCinema.Producers.InsertAllOnSubmit(listProducer);
            }
            try
            {
                share.dbCinema.SubmitChanges();
            }
            catch (Exception e)
            {
                share.log("UpdateProducer***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
            var listEvent = share.loadEvents();
            if (listEvent.Count > 0)
            {
                share.dbCinema.Events.InsertAllOnSubmit(listEvent);
            }
            try
            {
                share.dbCinema.SubmitChanges();
            }
            catch (Exception e)
            {
                share.log("UpdateEvent***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
            ////ManagerData.updateEventsBeta();
            return listEvent;
        }

        static public List<Event> updateEventsBeta()
        {
            var listEvent = share.loadEventsBeta();
            if (listEvent.Count > 0)
            {
                //ManagerData.clearEventBeta();
                share.dbCinema.Events.InsertAllOnSubmit(listEvent);
            }
            share.dbCinema.SubmitChanges();
            return listEvent;
        }

        static public List<Film> updateFilms()
        {
            updateCinemas();
            var listFilm = share.loadFilms();
            ManagerData.clearFilm();
            //if (listFilm.Count > 0)
            //{
            //    ManagerData.clearFilm();
            //    share.dbCinema.Films.InsertAllOnSubmit(listFilm);
            //}
            //share.dbCinema.SubmitChanges();
            return listFilm;
        }

        static public List<Schedule> updateSchedules()
        {
            updateCinemas();
            var listFilm = share.dbCinema.Films.ToList();
            var listSchedule = share.loadSchedules(ref listFilm);
            if (listSchedule.Count > 0)
            {
                share.dbCinema.Schedules.InsertAllOnSubmit(listSchedule);
            }
            try
            {
                share.dbCinema.SubmitChanges();
            }
            catch (Exception e)
            {
                share.log("UpdateSchedule***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
            }
            return listSchedule;
        }

        List<Location> loadLocations()
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net/rap");
            var listLocation = new List<Location>();
            var local = new Location();
            var listHtmlLocation = document.DocumentNode.QuerySelector("select.form-control");
            for (int i = 4; i < listHtmlLocation.ChildNodes.Count; i++)
            {
                if (listHtmlLocation.ChildNodes[i].OuterHtml.Trim() != "")
                {
                    if (listHtmlLocation.ChildNodes[i].Name == "option")
                    {
                        local = new Location();
                        local.id = listHtmlLocation.ChildNodes[i].Attributes["value"].Value.Trim();
                    }
                    else
                    {
                        if (dbCinema.Locations.Where(lc => lc.id == local.id).Count() < 1)
                        {
                            local.name = listHtmlLocation.ChildNodes[i].InnerText.Trim();
                            listLocation.Add(local);
                        }
                    }
                }
            }
            return listLocation;
        }

        List<Producer> loadProducers()
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net/khuyen-mai");
            var listProducer = new List<Producer>();
            var producer = new Producer();
            var listHtmlProducer = document.DocumentNode.QuerySelectorAll("li > a.glide-to");
            foreach (var htmlProducer in listHtmlProducer)
            {
                producer = new Producer();
                producer.id = htmlProducer.Attributes["href"].Value.Replace("#khuyen-mai-", "").Trim();
                if (dbCinema.Producers.Where(pd => pd.id == producer.id).Count() < 1)
                {
                    producer.name = htmlProducer.InnerText.Trim();
                    listProducer.Add(producer);
                }
            }
            return listProducer;
        }

        List<Cinema> loadCinemas()
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net/rap");
            var listCinema = new List<Cinema>();
            var cinema = new Cinema();
            var listHtmlCinema = document.DocumentNode.QuerySelectorAll("a.list-group-item").ToList();
            foreach (var htmlCinema in listHtmlCinema)
            {
                cinema = new Cinema();
                string url = htmlCinema.Attributes["href"].Value.Trim();
                cinema.id = url.Split('/')[2];
                if (dbCinema.Cinemas.Where(cm => cm.id == cinema.id).Count() < 1)
                {
                    cinema.idLocation = htmlCinema.Attributes["class"].Value.Replace("list-group-item cinema-option city-", "").Trim();
                    cinema.name = htmlCinema.QuerySelector("h4").InnerText.Trim();
                    cinema.idProducer = "zzzzzzzzzzkhac";
                    foreach (var producer in dbCinema.Producers)
                    {
                        if (cinema.name.ToUpper().Contains(producer.name.ToUpper()))
                        {
                            cinema.idProducer = producer.id;
                        }
                    }
                    loadCinema(ref cinema, url);
                    listCinema.Add(cinema);
                }
            }
            return listCinema;
        }

        void loadCinema(ref Cinema cinema, string url)
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net" + url);
            var htmlInfo = document.DocumentNode.QuerySelectorAll("table.cinema-info-table > tr > td").ToList();
            if (htmlInfo.Count == 4)
            {
                cinema.address = htmlInfo[1].InnerText.Trim();
                cinema.phoneNumber = htmlInfo[3].InnerText.Trim();
                cinema.linkImage = document.DocumentNode.QuerySelector("img.center-block").Attributes["src"].Value;
                if (cinema.linkImage.Length > 200)
                {
                    cinema.linkImage = "http://www.cinenagina.com/wp-content/uploads/2016/02/Cinema-Vox-large-banner21.jpg";
                }
                cinema.intro = document.DocumentNode.QuerySelector("div.cinema-description").InnerText.Trim();
            }
            try
            {
                var htmlMaps = document.DocumentNode.QuerySelector("button#cinema-map-show-btn");
                String urlMaps = htmlMaps.Attributes["data-url"].Value.ToString().Trim();
                String longitude = urlMaps.Substring(urlMaps.IndexOf("!2d") + 3, lengthSubstring(urlMaps, urlMaps.IndexOf("!2d") + 3));
                String latitude = urlMaps.Substring(urlMaps.IndexOf("!3d") + 3, lengthSubstring(urlMaps, urlMaps.IndexOf("!3d") + 3));
                Double.Parse(longitude).ToString();
                Double.Parse(latitude).ToString();
                cinema.longitude = longitude;
                cinema.latitude = latitude;
            }
            catch (Exception)
            {
                cinema.longitude = "";
                cinema.latitude = "";
            }
        }

        int lengthSubstring(String str, int startAt)
        {
            for (int i = 1; i < str.Length - startAt - 1; i++)
            {
                try
                {
                    Double.Parse(str.Substring(startAt, i));
                }
                catch (Exception)
                {
                    return i - 1;
                }
            }
            return str.Length - startAt - 1;
        }

        List<Event> loadEvents()
        {
            var listEvent = new List<Event>();
            var listProducer = dbCinema.Producers.ToList();
            foreach (var producer in listProducer)
            {
                var ev = new Event();
                HtmlDocument document = htmlWeb.Load("http://lichchieu.net/khuyen-mai/" + producer.id);
                var listHtmlEvent = document.DocumentNode.QuerySelectorAll("ul.list-unstyled > li > div").ToList();
                foreach (var htmlEvent in listHtmlEvent)
                {
                    ev = new Event();
                    string url = htmlEvent.QuerySelector("div.fixed-ratio-content > a").Attributes["href"].Value.Trim();
                    ev.id = url.Split('/')[3];
                    if (dbCinema.Events.Where(et => et.id == ev.id).Count() < 1)
                    {
                        ev.idProducer = producer.id;
                        ev.linkPoster = htmlEvent.QuerySelector("img.lazyload").Attributes["data-src"].Value.Trim();
                        ev.name = htmlEvent.QuerySelector("h3.offer-title").InnerText.Trim();
                        ev.time = htmlEvent.QuerySelector("div.period").InnerText.Trim();
                        if (!ev.time.ToUpper().Contains("TỪ") && ev.time != null)
                        {
                            int index = ev.time.LastIndexOf(" ");
                            ev.endTime = DateTime.ParseExact(ev.time.Substring(ev.time.LastIndexOf(" ") + 1), "d/M/yyyy", null);
                        }

                        if ((ev.endTime >= DateTime.Now.ToUniversalTime().AddHours(7.0) || ev.endTime == null) && !ev.name.Contains("(Hết Hạn)"))
                        {
                            loadEvent(ref ev, url);
                            listEvent.Add(ev);
                        }
                    }
                }
            }
            return listEvent;
        }

        void loadEvent(ref Event ev, string url)
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net" + url);
            ev.intro = document.DocumentNode.QuerySelector("div.offer-cnt").InnerText.Trim();
            //try
            //{
            //    ev.intro = document.DocumentNode.QuerySelector("div.col-right-detail-content-new-offer").InnerText.Trim();
            //}
            //catch (Exception)
            //{
            //}

        }

        List<Event> loadEventsBeta()
        {
            HtmlDocument document = htmlWeb.Load("https://www.betacineplex.vn/tin-moi-va-uu-dai");
            var listEvent = new List<Event>();
            var ev = new Event();
            var listHtmlEvent = document.DocumentNode.QuerySelectorAll("div.promo-item > a").ToList();
            foreach (var htmlEvent in listHtmlEvent)
            {
                ev = new Event();
                ev.idProducer = "beta";
                string url = htmlEvent.Attributes["href"].Value.Trim();
                ev.id = "b" + url.Split('/')[2];
                if (dbCinema.Events.Where(e => e.id == ev.id).Count() <= 0)
                {
                    ev.linkPoster = "https://www.betacineplex.vn" + htmlEvent.QuerySelector("img").Attributes["src"].Value.Trim();
                    loadEventBeta(ref ev, url);
                    listEvent.Add(ev);
                }
            }
            return listEvent;
        }

        void loadEventBeta(ref Event ev, string url)
        {
            HtmlDocument document = htmlWeb.Load("https://www.betacineplex.vn/" + url);
            ev.name = document.DocumentNode.QuerySelector("div.more-details > h2").InnerText.Trim();
            ev.intro = document.DocumentNode.QuerySelector("div.contentNews").InnerText.Trim();

        }

        List<Film> loadFilms()
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net/phim");
            var listFilm = new List<Film>();
            var film = new Film();
            var listHtmlFilm = document.DocumentNode.QuerySelectorAll("ul.movie-list > li > div").ToList();
            foreach (var htmlFilm in listHtmlFilm)
            {
                film = new Film();
                string url = htmlFilm.QuerySelector("h3 > a").Attributes["href"].Value.Trim();
                film.id = url.Split('/')[2];
                try
                {
                    if (htmlFilm.QuerySelector("div.hot-ribbon").InnerText.ToUpper().Trim() == "HOT")
                    {
                        film.isHot = true;
                    }
                    else
                    {
                        film.isHot = false;
                    }
                }
                catch (Exception)
                {
                    film.isHot = false;
                }
                var htmlTableInfo = htmlFilm.QuerySelectorAll("table > tr > td").ToList();
                try
                {
                    film.imdb = Double.Parse(htmlTableInfo[0].InnerText.Trim());
                }
                catch (Exception)
                {
                    film.imdb = 0;
                }
                loadSchedulesByFilm(ref film, url);
                if (dbCinema.Films.Where(fl => fl.id == film.id).Count() < 1)
                {
                    film.name = htmlFilm.QuerySelector("h3 > a").FirstChild.InnerText.Trim();
                    film.linkPoster = htmlFilm.QuerySelector("img.poster").Attributes["data-srcset"].Value.Trim();
                    var list = film.linkPoster.Split('=', ' ');
                    film.linkPoster = list[list.Count() - 2].Replace("-400", "");
                    film.length = htmlTableInfo[1].InnerText.Trim();
                    film.genre = htmlTableInfo[2].InnerText.Trim();
                    film.classification = htmlTableInfo[3].InnerText.Trim();
                    film.actor = htmlTableInfo[4].InnerText.Trim();
                    loadFilm(ref film, url);
                    listFilm.Add(film);
                    dbCinema.Films.InsertOnSubmit(film);
                }
                else
                {
                    var f = dbCinema.Films.FirstOrDefault(fl => fl.id == film.id);
                    dbCinema.Schedules.DeleteAllOnSubmit(f.Schedules);
                    f.Schedules = film.Schedules;
                    f.isHot = film.isHot;
                    f.imdb = film.imdb;
                }
                dbCinema.SubmitChanges();
                try
                {
                    dbCinema.SubmitChanges();
                }
                catch (Exception e)
                {
                    share.log("UpdateFilm***" + film.name + "***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
                }
            }
            //loadSchedules(ref listFilm);

            return listFilm;
        }

        void loadFilm(ref Film film, string url)
        {
            HtmlDocument document = htmlWeb.Load("http://lichchieu.net" + url.Replace("/phim/", "/thong-tin-phim/"));
            var htmlInfo = document.DocumentNode.QuerySelectorAll("table > tr").ToList();
            var stringPremiere = "";
            foreach (var item in htmlInfo)
            {
                switch (item.ChildNodes[1].InnerText.Trim())
                {
                    case "Ngày khởi chiếu":
                        stringPremiere += item.ChildNodes[3].InnerText.Trim();
                        break;
                    case "Phân loại người xem":
                        film.classification = item.ChildNodes[3].InnerText.Trim();
                        break;
                    case "Đạo diễn":
                        film.director = item.ChildNodes[3].InnerText.Trim();
                        if (film.classification.Length > 10)
                        {
                            film.classification = film.classification.Substring(0, 3);
                        }
                        break;
                    case "Năm":
                        stringPremiere += "/" + item.ChildNodes[3].InnerText.Trim();
                        break;
                    case "Sản xuất":
                        film.country = item.ChildNodes[3].InnerText.Trim();
                        break;
                }
            }
            try
            {
                film.premiere = DateTime.ParseExact(stringPremiere, "dd/MM/yyyy", null);
            }
            catch (Exception) { }
            var htmlTrailer = document.DocumentNode.QuerySelectorAll("script").ToList();
            film.linkTrailer = subTrailer(htmlTrailer[htmlTrailer.Count - 2].InnerHtml.Trim());
            film.intro = document.DocumentNode.QuerySelector("div.movie-description").InnerText.Trim();
        }

        String subTrailer(String text)
        {
            try
            {
                String trailer = text.Substring(text.IndexOf("'//"), text.IndexOf("?origin=") - text.IndexOf("'//"));
                return trailer.Replace("'//", "https://").Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }

        List<Schedule> loadSchedules(ref List<Film> listFilm)
        {
            var listSchedule = new List<Schedule>();
            var schedule = new Schedule();
            String idCinema;
            foreach (var film in listFilm)
            {
                HtmlDocument document = share.htmlWeb.Load("http://lichchieu.net" + film.id);
                film.linkBanner = document.DocumentNode.QuerySelector("img.img-responsive").Attributes["src"].Value.Trim();
                var listHtmlDate = document.DocumentNode.QuerySelectorAll("div.date-picker > ul > li > a").ToList();
                for (int i = 0; i < listHtmlDate.Count; i++)
                {
                    if (i > 0)
                    {
                        document = share.htmlWeb.Load("http://lichchieu.net" + listHtmlDate[i].Attributes["href"].Value.Trim());
                    }
                    var listHtmlSchedule = document.DocumentNode.QuerySelectorAll("div.cinema-group > div.panel").ToList();
                    foreach (var htmlSchedule in listHtmlSchedule)
                    {

                        idCinema = htmlSchedule.QuerySelector("div.panel-sub-title > a").Attributes["href"].Value.Trim();
                        var listHtmlValue = htmlSchedule.QuerySelectorAll("div.cinema-schedule > a").ToList();
                        foreach (var value in listHtmlValue)
                        {
                            schedule = new Schedule();
                            //schedule.id = idCinema + listHtmlDate[i].Attributes["href"].Value.Trim() + "/" + value.InnerText.Trim();
                            //if (share.dbCinema.Schedules.Where(sh => sh.id == schedule.id).Count() < 1
                            //    && film.Schedules.Where(sh => sh.id == schedule.id).Count() < 1)
                            //{
                            //    schedule.idFilm = film.id;
                            //    schedule.slot = htmlSchedule.QuerySelector("span.slot-group-label").InnerText.Replace(" ", "").Trim();
                            //    schedule.idCinema = idCinema.Trim();
                            //    if (value.Attributes["href"].Value.Contains("javascript:alert("))
                            //    {
                            //        schedule.linkTicket = value.Attributes["href"].Value.Trim();
                            //    }
                            //    else
                            //    {
                            //        schedule.linkTicket = "http://lichchieu.net" + value.Attributes["href"].Value.Trim();
                            //    }
                            //    schedule.dateTime = listHtmlDate[i].Attributes["href"].Value.Trim().Substring(listHtmlDate[i].Attributes["href"].Value.Trim().Length - 10, 10) + " " + value.InnerText.Trim();
                            //    listSchedule.Add(schedule);
                            //    film.Schedules.Add(schedule);
                            //}
                            schedule.idFilm = film.id;
                            schedule.slot = htmlSchedule.QuerySelector("span.slot-group-label").InnerText.Replace(" ", "").Trim();
                            schedule.idCinema = idCinema.Trim();
                            if (value.Attributes["href"].Value.Contains("javascript:alert("))
                            {
                                schedule.linkTicket = value.Attributes["href"].Value.Trim();
                            }
                            else
                            {
                                schedule.linkTicket = "http://lichchieu.net" + value.Attributes["href"].Value.Trim();
                            }
                            schedule.dateTime = listHtmlDate[i].Attributes["href"].Value.Trim().Substring(listHtmlDate[i].Attributes["href"].Value.Trim().Length - 10, 10) + " " + value.InnerText.Trim();
                            listSchedule.Add(schedule);
                            film.Schedules.Add(schedule);
                        }

                    }
                }
                dbCinema.Films.DeleteAllOnSubmit(dbCinema.Films.Where(f => f.id == film.id));
                dbCinema.Films.InsertOnSubmit(film);
                //dbCinema.SubmitChanges();
                try
                {
                    dbCinema.SubmitChanges();
                }
                catch (Exception e)
                {
                    share.log("UpdateFilm***" + film.name + "***" + DateTime.Now.ToUniversalTime().AddHours(7.0).ToString() + "***" + e.Message);
                }
            }
            return listSchedule;
        }

        void loadSchedulesByFilm(ref Film film, string url)
        {
            var schedule = new Schedule();
            String idCinema;
            HtmlDocument document = share.htmlWeb.Load("http://lichchieu.net" + url);
            film.linkBanner = document.DocumentNode.QuerySelectorAll("head > meta").ToList()[5].Attributes["content"].Value.Trim();
            var listHtmlDate = document.DocumentNode.QuerySelectorAll("div.date-picker > ul > li > a").ToList();
            for (int i = 0; i < listHtmlDate.Count; i++)
            {
                if (i > 0)
                {
                    document = share.htmlWeb.Load("http://lichchieu.net" + listHtmlDate[i].Attributes["href"].Value.Trim());
                }
                var listHtmlSchedule = document.DocumentNode.QuerySelectorAll("div.cinema-group > div.panel").ToList();
                foreach (var htmlSchedule in listHtmlSchedule)
                {
                    idCinema = htmlSchedule.QuerySelector("div.panel-sub-title > a").Attributes["href"].Value.Trim();
                    var listHtmlValue = htmlSchedule.QuerySelectorAll("div.cinema-schedule > a").ToList();

                    foreach (var value in listHtmlValue)
                    {
                        schedule = new Schedule();
                        schedule.idFilm = film.id;
                        schedule.idCinema = idCinema.Trim().Split('/')[2];
                        schedule.id = schedule.idFilm + "-" + schedule.idCinema + "-" + film.Schedules.Count;
                        if (film.Schedules.Where(sh => sh.id == schedule.id).Count() < 1)
                        {
                            schedule.slot = htmlSchedule.QuerySelector("span.slot-group-label").InnerText.Replace(" ", "").Trim();
                            if (value.Attributes["href"].Value.Contains("javascript:alert("))
                            {
                                string strEncoded = value.Attributes["href"].Value.Trim().Replace("http://lichchieu.net/chuyen-tiep?url=", "");
                                schedule.linkTicket = HttpUtility.UrlDecode(WebUtility.UrlDecode(strEncoded));
                            }
                            else
                            {
                                string strEncoded = value.Attributes["href"].Value.Trim().Replace("/chuyen-tiep?url=", "");
                                schedule.linkTicket = HttpUtility.UrlDecode(WebUtility.UrlDecode(strEncoded));
                            }
                            if (schedule.linkTicket.Contains("alert("))
                            {
                                schedule.linkTicket = "null";
                            }
                            schedule.dateTime = listHtmlDate[i].Attributes["href"].Value.Trim().Substring(listHtmlDate[i].Attributes["href"].Value.Trim().Length - 10, 10) + " " + value.InnerText.Trim();
                            film.Schedules.Add(schedule);
                        }
                        else
                        {
                            string strEncoded = value.Attributes["href"].Value.Trim().Replace("/chuyen-tiep?url=", "");
                            schedule.linkTicket = HttpUtility.UrlDecode(WebUtility.UrlDecode(strEncoded));
                        }
                        schedule.dateTime = listHtmlDate[i].Attributes["href"].Value.Trim().Substring(listHtmlDate[i].Attributes["href"].Value.Trim().Length - 10, 10) + " " + value.InnerText.Trim();
                        film.Schedules.Add(schedule);
                    }

                }
            }
        }

    }
}