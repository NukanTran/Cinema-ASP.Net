using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cinema_2._0.Manager;

namespace Cinema_2._0.API
{
    [RoutePrefix("api/data")]
    public class DataController : ApiController
    {
        DBCinemaDataContext dbCinema = new DBCinemaDataContext();

        //Get Location, Producer
        [HttpGet]
        [Route("location")]
        public JsonObject GetLocation()
        {
            
            string status = "ok";
            int total = 0;
            object data = new object();

            data = dbCinema.Locations.OrderBy(lc => lc.name).ToList();
            total = ((List<Location>)data).Count();
            foreach (var local in (List<Location>)data)
            {
                local.Users.Clear();
                local.Cinemas.Clear();
                local.Users.Clear();
            }
            return new JsonObject(status, total, total, data);
        }

        [HttpGet]
        [Route("producer")]
        public JsonObject GetProducer()
        {

            string status = "ok";
            int total = 0;
            object data = new object();
            data = dbCinema.Producers.OrderBy(pd => pd.id.Length).ThenBy(pd => pd.id).ToList();
            total = ((List<Producer>)data).Count;
            foreach (var producer in (List<Producer>)data)
            {
                producer.Cinemas.Clear();
                producer.Events.Clear();
            }
            return new JsonObject(status, total, total, data);
        }

        //Get Film
        [HttpGet]
        [Route("film/{show}/{location}/{page}")]
        public JsonObject Get(bool show, string location, int page)
        {
            
            const int numberOfPage = 10;
            string status = "ok";
            int total = 0;
            var data = new List<Film>();
            if (show)
            {
                var listData = ManagerData.getListFilm(dbCinema.Films.Where(fl => fl.premiere <= DateTime.Now.ToUniversalTime().AddHours(7.0)).OrderByDescending(fl => fl.premiere).ToList(), location);
                total = listData.Count;
                for (int i = (page - 1) * numberOfPage; i < listData.Count && i < page * numberOfPage; i++)
                {
                    listData[i].FavoriteFilms.Clear();
                    listData[i].Schedules.Clear();
                    data.Add(listData[i]);
                }
            }
            else
            {
                var listData = ManagerData.getListFilm(dbCinema.Films.Where(fl => fl.premiere > DateTime.Now.ToUniversalTime().AddHours(7.0)).OrderBy(fl => fl.premiere).ToList(), "");
                total = listData.Count;
                for (int i = (page - 1) * numberOfPage; i < listData.Count && i < page * numberOfPage; i++)
                {
                    listData[i].FavoriteFilms.Clear();
                    listData[i].Schedules.Clear();
                    data.Add(listData[i]);
                }
            }
            return new JsonObject(status, total, numberOfPage, data);
        }

        //Get Event
        [HttpPost]
        [Route("event")]
        public JsonObject Post([FromBody]BodyPost body)
        {
            int page = body.page;
            body.id = body.id.Trim();
            const int numberOfPage = 10;
            string status = "ok";
            int total = 0;
            object data = new object();
            data = dbCinema.View_Events.Where(ev => ev.idProducer == body.id && (ev.endTime == null
            || ev.endTime >= DateTime.Now.ToUniversalTime().AddHours(7.0))).OrderByDescending(ev => ev.endTime.HasValue).ThenBy(ev => ev.endTime)
            .Skip((page - 1) * numberOfPage).Take(numberOfPage).ToList();
            total = dbCinema.Events.Where(ev => ev.idProducer == body.id && (ev.endTime == null
            || ev.endTime >= DateTime.Now.ToUniversalTime().AddHours(7.0))).Count();
            return new JsonObject(status, total, numberOfPage, data);
        }

        //Get Cinema, DetailFilm, Schedule
        [HttpPost]
        [Route("categorie")]
        public JsonObject PostCategorie([FromBody]BodyPost body)
        {
            string categorie = body.categorie;
            body.id = body.id.Trim();
            string status = "ok";
            int total = 0;
            object data = new object();
            switch (categorie)
            {
                case "cinema":
                    data = dbCinema.Producers.Where(pd => pd.Cinemas.Where(cm => cm.idLocation == body.id).Count() > 0).OrderBy(pd => pd.id.Length).ThenBy(pd => pd.id).ToList();
                    foreach(Producer producer in data as List<Producer>)
                    {
                        var listCinema = producer.Cinemas.Where(cm => cm.idLocation == body.id).OrderBy(cm => cm.name).ToList();
                        foreach(var cinema in listCinema)
                        {
                            cinema.Producer = null;
                            cinema.Location = null;
                            cinema.Schedules.Clear();
                            cinema.FavoriteCinemas.Clear();
                        }
                        producer.Events.Clear();
                        producer.Cinemas.Clear();
                        producer.listCinema = listCinema;
                    }
                    total = ((List<Producer>)data).Count();
                    break;
                case "cinemabyfilm":
                    string[] idc = body.id.Split('#');
                    data = dbCinema.Producers.Where(pd => pd.Cinemas.Where(cm => cm.idLocation == idc[0] && dbCinema.Schedules.Where(sh => sh.idFilm == idc[1]).Select(sh => sh.idCinema).Contains(cm.id)).Count() > 0).OrderBy(pd => pd.id.Length).ThenBy(pd => pd.id).ToList();
                    foreach (Producer producer in data as List<Producer>)
                    {
                        var listCinema = producer.Cinemas.Where(cm => cm.idLocation == idc[0] && dbCinema.Schedules.Where(sh => sh.idFilm == idc[1]).Select(sh => sh.idCinema).Contains(cm.id)).OrderBy(cm => cm.name).ToList();
                        foreach (var cinema in listCinema)
                        {
                            cinema.Producer = null;
                            cinema.Location = null;
                            cinema.Schedules.Clear();
                            cinema.FavoriteCinemas.Clear();
                        }
                        producer.Events.Clear();
                        producer.Cinemas.Clear();
                        producer.listCinema = listCinema;
                    }
                    total = ((List<Producer>)data).Count();
                    break;
                case "detailfilm":
                    if(dbCinema.Films.Where(fl => fl.id == body.id).Count() > 0)
                    {
                        data = dbCinema.Films.FirstOrDefault(fl => fl.id == body.id);
                        ((Film)data).FavoriteFilms.Clear();
                        ((Film)data).Schedules.Clear();
                    }
                    else
                    {
                        status = "fail2";
                    }
                    break;
                case "schedule":
                    string[] ids = body.id.Split('#');
                    if (dbCinema.Films.Where(fl => fl.id == ids[0]).Count() > 0)
                    {
                        data = ManagerData.getListScheduleView(dbCinema.View_Schedules.Where(sh => sh.idFilm == ids[0] && sh.idCinema == ids[1]).OrderBy(sh => sh.dateTime).ToList());
                        total = ((List<View_Schedule>)data).Count();
                    }
                    else
                    {
                        status = "fail1";
                    }
                    break;
                case "datebycinema":
                    data = new List<string>();
                    var listSchedule = dbCinema.Schedules.Where(sh => sh.idCinema == body.id).OrderBy(sh => sh.dateTime).ToList();
                    foreach(var i in listSchedule)
                    {
                        if(DateTime.ParseExact(i.dateTime, "yyyy-MM-dd HH:mm", null) >= DateTime.Now.ToUniversalTime().AddHours(7.0) && ((data as List<string>).Count == 0 || !(data as List<string>).Contains(i.dateTime.Split(' ')[0])))
                        {
                            (data as List<string>).Add(i.dateTime.Split(' ')[0]);
                        }
                    }
                    total = (data as List<string>).Count;
                    break;
                case "filmbydate":
                    string[] idd = body.id.Split('#');
                    data = dbCinema.Films.Where(fl => fl.Schedules.Where(sh => sh.idCinema == idd[0] && sh.dateTime.Contains(idd[1])).Count() > 0).OrderByDescending(fl => fl.imdb).ToList();
                    foreach(var i in (data as List<Film>))
                    {
                        i.FavoriteFilms.Clear();
                        i.Schedules.Clear();
                        var listSH = dbCinema.Schedules.Where(sh => sh.idCinema == idd[0] && sh.idFilm == i.id && sh.dateTime.Contains(idd[1])).ToList();
                        foreach (var j in listSH)
                        {
                            j.Cinema = null;
                            j.Film = null;
                        }
                        i.listSchedule = listSH;
                    }
                    total = (data as List<Film>).Count;
                    break;
                default:
                    status = "fail0";
                    break;
            }
            return new JsonObject(status, total, total, data);
        }


        [HttpGet]
        [Route("city/{lat}/{log}/{email}")]
        public JsonObject Get(double lat, double log, string email)
        {
            
            const int numberOfPage = 10;
            string status = "ok";
            int total = 0;
            string data = "";
            double min = -1;
            double tempLat = 0.0;
            double tempLog = 0.0;
            var listCinema = dbCinema.Cinemas.ToList();
            foreach (var cinema in listCinema)
            {
                try
                {
                    tempLat = Double.Parse(cinema.latitude);
                    tempLog = Double.Parse(cinema.longitude);

                    if (min == -1)
                    {
                        data = cinema.idLocation;
                        min = (Math.Pow(tempLat - lat, 2) + Math.Pow(tempLog - log, 2));
                    }
                    if (min > (Math.Pow(tempLat - lat, 2) + Math.Pow(tempLog - log, 2)))
                    {
                        data = cinema.idLocation;
                        min = (Math.Pow(tempLat - lat, 2) + Math.Pow(tempLog - log, 2));
                    }
                }
                catch (Exception) { }
            }
            var user = dbCinema.Users.FirstOrDefault(us => us.email.Trim() == email.Replace("~", ".").Trim());
            if (user != null)
            {
                user.idLocation = data;
                dbCinema.SubmitChanges();
            }
            return new JsonObject(status, total, numberOfPage, data);
        }

    }

    public class BodyPost
    {
        public string categorie { get; set; }
        public int page { get; set; }
        public string id { get; set; }
    }
    public class JsonObject
    {
        public string status;
        public int total;
        public int numberOfPgae;
        public object data;
        public JsonObject(string status, int total, int numberOfPgae, object data)
        {
            this.status = status;
            this.total = total;
            this.numberOfPgae = numberOfPgae;
            this.data = data;
        }
    }
}
