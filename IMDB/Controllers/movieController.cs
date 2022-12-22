using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMDB.Models;
using System.Text.RegularExpressions;
namespace IMDB.Controllers
{
    public class movieController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SearchMovie(string movieName = " ")
        {
            if (Session["CurrentMovie"] == null)
            {
                Movie movieDetail = CRUD.SearchMovie(movieName); //MovieName used as result catcher. Inspired from c++. LOL. XD 
                Session["CurrentMovie"] = movieDetail;
                if (movieDetail.m_ID > 0)
                {
                    return View();//Sends to the respective movies view
                }
                else
                {
                    return View("MovieNotFound", movieName);
                }
            }
            else
            {
                return View();
            }

        }
        public ActionResult MovieNotFound()
        {
            return View();
        }
        public ActionResult MovieInWatchList()
        {
            return View();
        }
        public ActionResult RateMovie(int rating = 5)
        {
            Movie curr_movie = (Movie)Session["CurrentMovie"];

            if (Session["userID"] != null)
            {
                if (Session["CurrentMovie"] != null)
                {
                    if (CRUD.AddUserRating((int)Session["userID"], curr_movie.m_ID, rating) > 0)
                    {
                        Movie updated_movie = CRUD.SearchMovie(curr_movie.m_name);
                        Session["CurrentMovie"] = updated_movie;
                        return View("SearchMovie");
                    }

                    else
                        return View();
                }
                else
                {
                    return View("Index");
                }
            }
            return RedirectToAction("UserLogin", "Home");
        }
        public ActionResult AddtoWatchList(int movieID)
        {
            if (Session["userID"] != null)
            {
                CRUD.AddMovieToWatchList((int)Session["userID"], movieID);
                return RedirectToAction("SearchMovie", "Movie");
            }
            else
            {
                return RedirectToAction("UserLogin", "Home");
            }
        }
        public ActionResult RemoveFromWatchList(int movieID)
        {
            if (Session["userID"] != null)
            {
                CRUD.DeleteMovieFromWatchList((int)Session["userID"], movieID);
            }
            return RedirectToAction("SearchMovie", "Movie");
        }
        public ActionResult AddMovieToDB(string movieName = " ", DateTime releaseDate = new DateTime(), string rating = "5.0", string description = " ", string posterLink = " ", string trailerLink = " ")
        {
            CRUD.AddMovieToDB(movieName, rating, description, releaseDate, posterLink, trailerLink);
            return RedirectToAction("Index", "Admin");
        }

    }
}
