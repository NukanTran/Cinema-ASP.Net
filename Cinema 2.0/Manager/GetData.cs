using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cinema_2._0.Manager;

namespace Cinema_2._0.Manager
{
    public class GetData
    {
        static GetData share = new GetData();
        DBCinemaDataContext dbCinema = new DBCinemaDataContext();
        public static List<Event> getEvents()
        {
            return share.dbCinema.Events.ToList();
        }

        public static List<Event> getEventsBeta()
        {
            var listID = new[] { "tin-moi/gia-ve-uu-dai-cho-hoc-sinh-sinh-vien/84", "tin-moi/thu-5-thoa-suc-mam-mam-/97" };
            return share.dbCinema.Events.Where(ev => ev.idProducer == "beta" && listID.Contains(ev.id)).ToList();
        }

        public static List<Film> getFilms()
        {
            return share.dbCinema.Films.OrderByDescending(fl => fl.imdb).ToList();
        }

        public static List<Producer> getListEvent()
        {
            var data = share.dbCinema.Producers.Where(pd => pd.id != "zzzzzzzzzzkhac").OrderBy(pd => pd.id.Length).ThenBy(pd => pd.id).ToList();
            foreach(var i in data)
            {
                i.listEvent = i.Events.OrderByDescending(ev => ev.endTime.HasValue).ThenBy(ev => ev.endTime).ToList();
            }
            return data;
        }

        public static List<Film> getFilmShow()
        {
            return share.dbCinema.Films.Where(fl => fl.premiere <= DateTime.Now.ToUniversalTime().AddHours(7.0)).OrderByDescending(fl => fl.premiere).ToList();
        }
        public static List<Film> getFilmDontShow()
        {
            return share.dbCinema.Films.Where(fl => fl.premiere > DateTime.Now.ToUniversalTime().AddHours(7.0)).OrderBy(fl => fl.premiere).ToList();
        }

        public static List<Film> getFilmByDate(String city, String date)
        {
            return share.dbCinema.Films.Where(fl => fl.Schedules.Where(sh => sh.dateTime.Contains(date) && (sh.Cinema.idLocation == city || city == "")).Count() > 0).OrderByDescending(fl => fl.imdb).ToList();

        }

        public static List<Location> getCity()
        {
            return share.dbCinema.Locations.OrderBy(lc => lc.name).ToList();
        }

        public static List<String> getDate(String city)
        {
            var data = new List<string>();
            var listSchedule = share.dbCinema.Schedules.Where(sh =>(sh.Cinema.idLocation == city || city == "")).OrderBy(sh => sh.dateTime).ToList();
            foreach (var i in listSchedule)
            {
                if (DateTime.ParseExact(i.dateTime, "yyyy-MM-dd HH:mm", null) >= DateTime.Now.ToUniversalTime().AddHours(7.0) && !(data as List<string>).Contains(i.dateTime.Split(' ')[0]))
                {
                    (data as List<string>).Add(i.dateTime.Split(' ')[0]);
                }
            }
            return data;
        }

        public static Film getDetailFilm(String id)
        {
            return share.dbCinema.Films.FirstOrDefault(fl => fl.id == id);
        }
        public static Event getDetailEvent(String id)
        {
            return share.dbCinema.Events.FirstOrDefault(ev => ev.id == id);
        }
        public static Cinema getDetailCinema(String id)
        {
            return share.dbCinema.Cinemas.FirstOrDefault(cm => cm.id == id);
        }

        public static Dictionary<String, Dictionary<String, Dictionary<String, List<Schedule>>>> getDateByFilm(List<Schedule> listSchedule)
        {
            Dictionary < String, Dictionary<String, Dictionary<String, List<Schedule>>>> listDate = 
                new Dictionary<String, Dictionary<String, Dictionary<String, List<Schedule>>>>();
            foreach (var i in listSchedule)
            {
                if (DateTime.ParseExact(i.dateTime, "yyyy-MM-dd HH:mm", null) >= DateTime.Now.ToUniversalTime().AddHours(7.0))
                {
                    if(listDate.ContainsKey(i.dateTime.Split(' ')[0]))
                    {
                        if(listDate[i.dateTime.Split(' ')[0]].ContainsKey(i.Cinema.Producer.name))
                        {
                            if(listDate[i.dateTime.Split(' ')[0]][i.Cinema.Producer.name].ContainsKey(i.Cinema.name))
                            {
                                listDate[i.dateTime.Split(' ')[0]][i.Cinema.Producer.name][i.Cinema.name].Add(i);
                            }
                            else
                            {
                                var listSC = new List<Schedule>();
                                listSC.Add(i);
                                listDate[i.dateTime.Split(' ')[0]][i.Cinema.Producer.name].Add(i.Cinema.name, listSC);
                            }
                        }
                        else
                        {
                            var listSC = new List<Schedule>();
                            listSC.Add(i);
                            var listCinema = new Dictionary<String, List<Schedule>>();
                            listCinema.Add(i.Cinema.name, listSC);
                            listDate[i.dateTime.Split(' ')[0]].Add(i.Cinema.Producer.name, listCinema);
                        }
                    }
                    else
                    {
                        var listSC = new List<Schedule>();
                        listSC.Add(i);
                        var listCinema = new Dictionary<String, List<Schedule>>();
                        listCinema.Add(i.Cinema.name, listSC);
                        var listProducer = new Dictionary<String, Dictionary<String, List<Schedule>>>();
                        listProducer.Add(i.Cinema.Producer.name, listCinema);
                        listDate.Add(i.dateTime.Split(' ')[0], listProducer);
                    }
                }
            }
            return listDate;
        }

        public static List<Schedule> getScheduleByFilm(String id, String city)
        {
            return share.dbCinema.Schedules.Where(sh => sh.idFilm == id && (city == "" || sh.Cinema.idLocation == city)).OrderBy(sc => sc.dateTime).ThenBy(sc => sc.Cinema.Producer.id).ThenBy(sc => sc.Cinema.Producer.id.Length).ThenBy(sc => sc.Cinema.name).ToList();
        }

        public static Dictionary<String, Dictionary<String, List<Schedule>>> getScheduleByCinema(String idFilm, String date, String city)
        {
            Dictionary < String, Dictionary < String, List < Schedule >>> listSchedule = new Dictionary<String, Dictionary<String, List<Schedule>>>();
            var listCinema = share.dbCinema.Cinemas.Where(cm => cm.idLocation == city || city == "").OrderBy(cm => cm.Producer.id).ThenBy(cm => cm.Producer.id.Length).ThenBy(cm => cm.name);

            foreach(var cinema in listCinema)
            {
                var listSC = share.dbCinema.Schedules.Where(sc => sc.idCinema == cinema.id && sc.idFilm == idFilm && (sc.dateTime.Contains(date) || date == "")).ToList();
                if(listSC.Count > 0)
                {
                    if (listSchedule.ContainsKey(listSC[0].Cinema.Producer.name))
                    {
                        if (!listSchedule[listSC[0].Cinema.Producer.name].ContainsKey(listSC[0].Cinema.name))
                        {
                            listSchedule[listSC[0].Cinema.Producer.name].Add(listSC[0].Cinema.name, listSC);
                        }
                    }
                    else
                    {
                        var listTemp = new Dictionary<string, List<Schedule>>();
                        listTemp.Add(listSC[0].Cinema.name, listSC);
                        listSchedule.Add(listSC[0].Cinema.Producer.name, listTemp);
                    }
                }
            }
            return listSchedule;
        }

        public static List<Producer> getListProducer(String city)
        {
            List<Producer> listProducer = new List<Producer>();
            var data = share.dbCinema.Producers.OrderBy(pd => pd.id).ThenBy(pd => pd.id.Length).ToList();
            foreach(var i in data)
            {
                i.listCinema = i.Cinemas.Where(cm => cm.Location.id == city || city == "").OrderBy(cm => cm.name).ToList();
                if(i.listCinema.Count > 0)
                {
                    listProducer.Add(i);
                }
            }
            return listProducer;
        }

    }
}