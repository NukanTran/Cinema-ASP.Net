using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cinema_2._0.API;
using Cinema_2._0.Manager;

namespace Cinema_2._0.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        DBCinemaDataContext dbCinema = new DBCinemaDataContext();


        [Route("login")]
        public JsonObject Login([FromBody]User body)
        {
            ManagerData.update();
            String status = "ok";
            User user = null;
            if (body.email == null || body.email.Length <= 0 
                || body.password == null || body.password.Length <= 0
                || dbCinema.Users.Where(u => u.email == body.email && u.password == body.password).Count() <= 0)
            {
                status = "Tài khoản hoặc mật khẩu không chính xác!";
            }
            else
            {
                user = dbCinema.Users.FirstOrDefault(u => u.email == body.email && u.password == body.password);
                user.listCinema = new List<Cinema>();
                user.listEvent = new List<Event>();
                user.listFilm = new List<Film>();
                foreach (var i in user.FavoriteCinemas)
                {
                    var cinema = i.Cinema;
                    cinema.FavoriteCinemas.Clear();
                    cinema.Schedules.Clear();
                    cinema.Location = null;
                    cinema.Producer = null;
                    user.listCinema.Add(cinema);
                }
                foreach (var i in user.FavoriteEvents)
                {
                    var ev = i.Event;
                    ev.FavoriteEvents.Clear();
                    ev.Producer = null;
                    user.listEvent.Add(ev);
                }
                foreach (var i in user.FavoriteFilms)
                {
                    var film = i.Film;
                    film.FavoriteFilms.Clear();
                    film.Schedules.Clear();
                    user.listFilm.Add(film);
                }
                user.password = "";
                user.FavoriteCinemas.Clear();
                user.FavoriteEvents.Clear();
                user.FavoriteFilms.Clear();
                user.Notifies.Clear();
                user.Tokens.Clear();
                user.Location = null;
            }
            return new JsonObject(status, 0, 0, user);
        }

        //register
        [Route("register")]
        public JsonObject Register([FromBody]User user)
        {
            ManagerData.update();
            String status = "ok";
            if(user.email == null || user.email.Length <= 0)
            {
                status = "Thông tin đăng ký không hợp lệ!";
            }
            else
            {
                if(dbCinema.Users.Where(u => u.email == user.email).Count() > 0)
                {
                    status = "Email này đã được dùng đăng ký tài khoản!";
                }
                else
                {
                    dbCinema.Users.InsertOnSubmit(user);
                    dbCinema.SubmitChanges();
                }
            }
            return new JsonObject(status, 0, 0, null);
        }


        [HttpPost]
        [Route("like")]
        public JsonObject Like([FromBody]BodyPost body)
        {
            string categorie = body.categorie;
            ManagerData.update();
            String status = "ok";
            string[] id = body.id.Split('#');
            switch (categorie)
            {
                case "cinema":
                    if (dbCinema.Users.Where(u => u.email == id[0]).Count() <= 0)
                    {
                        status = "Tài khoản không tồn tại!";
                    }
                    else if (dbCinema.Cinemas.Where(c => c.id == id[1]).Count() > 0)
                    {
                        if (dbCinema.FavoriteCinemas.Where(f => f.email == id[0] && f.idCinema == id[1]).Count() <= 0)
                        {
                            FavoriteCinema favorite = new FavoriteCinema();
                            favorite.email = id[0];
                            favorite.idCinema = id[1];
                            dbCinema.FavoriteCinemas.InsertOnSubmit(favorite);
                            dbCinema.SubmitChanges();
                        }
                    }
                    else
                    {
                        status = "Rạp phim không tồn tại!";
                    }
                    break;
                case "film":
                    if (dbCinema.Users.Where(u => u.email == id[0]).Count() <= 0)
                    {
                        status = "Tài khoản không tồn tại!";
                    }
                    else if (dbCinema.Films.Where(f => f.id == id[1]).Count() > 0)
                    {
                        if (dbCinema.FavoriteFilms.Where(f => f.email == id[0] && f.idFilm == id[1]).Count() <= 0)
                        {
                            FavoriteFilm favorite = new FavoriteFilm();
                            favorite.email = id[0];
                            favorite.idFilm = id[1];
                            dbCinema.FavoriteFilms.InsertOnSubmit(favorite);
                            dbCinema.SubmitChanges();
                        }
                    }
                    else
                    {
                        status = "Phim không tồn tại!";
                    }
                    break;
                case "event":
                    if (dbCinema.Users.Where(u => u.email == id[0]).Count() <= 0)
                    {
                        status = "Tài khoản không tồn tại!";
                    }
                    else if (dbCinema.Events.Where(e => e.id == id[1]).Count() > 0)
                    {
                        if (dbCinema.FavoriteEvents.Where(f => f.email == id[0] && f.idEvent == id[1]).Count() <= 0)
                        {
                            FavoriteEvent favorite = new FavoriteEvent();
                            favorite.email = id[0];
                            favorite.idEvent = id[1];
                            dbCinema.FavoriteEvents.InsertOnSubmit(favorite);
                            dbCinema.SubmitChanges();
                        }
                    }
                    else
                    {
                        status = "Khuyến mãi không tồn tại!";
                    }
                    break;
                default:
                    status = "fail";
                    break;
            }
            
            return new JsonObject(status, 0, 0, null);
        }


        [HttpPost]
        [Route("unlike")]
        public JsonObject UnLike([FromBody]BodyPost body)
        {
            string categorie = body.categorie;
            ManagerData.update();
            String status = "ok";
            string[] id = body.id.Split('#');
            switch (categorie)
            {
                case "cinema":
                    if (dbCinema.Users.Where(u => u.email == id[0]).Count() <= 0)
                    {
                        status = "Tài khoản không tồn tại!";
                    }
                    else if (dbCinema.Cinemas.Where(c => c.id == id[1]).Count() > 0)
                    {
                        if (dbCinema.FavoriteCinemas.Where(f => f.email == id[0] && f.idCinema == id[1]).Count() > 0)
                        {
                            dbCinema.FavoriteCinemas.DeleteOnSubmit(dbCinema.FavoriteCinemas.FirstOrDefault(f => f.email == id[0] && f.idCinema == id[1]));
                            dbCinema.SubmitChanges();
                        }
                    }
                    else
                    {
                        status = "Rạp phim không tồn tại!";
                    }
                    break;
                case "film":
                    if (dbCinema.Users.Where(u => u.email == id[0]).Count() <= 0)
                    {
                        status = "Tài khoản không tồn tại!";
                    }
                    else if (dbCinema.Films.Where(f => f.id == id[1]).Count() > 0)
                    {
                        if (dbCinema.FavoriteFilms.Where(f => f.email == id[0] && f.idFilm == id[1]).Count() > 0)
                        {
                            dbCinema.FavoriteFilms.DeleteOnSubmit(dbCinema.FavoriteFilms.FirstOrDefault(f => f.email == id[0] && f.idFilm == id[1]));
                            dbCinema.SubmitChanges();
                        }
                    }
                    else
                    {
                        status = "Phim không tồn tại!";
                    }
                    break;
                case "event":
                    if (dbCinema.Users.Where(u => u.email == id[0]).Count() <= 0)
                    {
                        status = "Tài khoản không tồn tại!";
                    }
                    else if (dbCinema.Events.Where(e => e.id == id[1]).Count() > 0)
                    {
                        if (dbCinema.FavoriteEvents.Where(f => f.email == id[0] && f.idEvent == id[1]).Count() > 0)
                        {
                            dbCinema.FavoriteEvents.DeleteOnSubmit(dbCinema.FavoriteEvents.FirstOrDefault(f => f.email == id[0] && f.idEvent == id[1]));
                            dbCinema.SubmitChanges();
                        }
                    }
                    else
                    {
                        status = "Khuyến mãi không tồn tại!";
                    }
                    break;
                default:
                    status = "fail";
                    break;
            }

            return new JsonObject(status, 0, 0, null);
        }

        [HttpGet]
        [Route("sdfgsd/{lat}/{log}")]
        public JsonObject Get(double lat, double log)
        {
            ManagerData.update();
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
            //var user = dbCinema.Users.FirstOrDefault(us => us.email.Trim() == body.id.Trim());
            //if (user != null)
            //{
            //    user.idLocation = data;
            //    dbCinema.SubmitChanges();
            //}
            return new JsonObject(status, total, numberOfPage, data);
        }

    }
}