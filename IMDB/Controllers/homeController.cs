using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMDB.Models;
using System.Data;

namespace IMDB.Controllers
{
    public class homeController : Controller
    {	
		
		public ActionResult Index()
        {
            return View();
        }
		
        public ActionResult SignUp()
        {
            return View();
        }
		
        public ActionResult UserLogin()
        {
            return View();
        }

        public ActionResult Authenticate(string email, string password)
        {
            if(Session["userID"] == null)//If User is not already logged in
            {
                IMDB.Models.User userDetail = CRUD.UserLoginCheck(email, password);
                if (userDetail.userID > 0)
                {
                    ViewBag.Message = userDetail;
                    Session["userID"] = userDetail.userID;
                    Session["userName"] = userDetail.userName;
                    Session["userEmail"] = userDetail.email;
                    Session["userPassword"] = userDetail.password;
                    Session["isAdmin"] = userDetail.isAdmin;
                }
                else
                {
                    ViewBag.Message = "WRONG USERNAME OR PASSWORD";
                    return View("UserLogin");
                }
            }
            return View();
        }

        public ActionResult Post(string userName, string email, string password, DateTime DOB)
        {
            int result = CRUD.UserSignUpCheck(userName, email, DOB, password);
            if (result == 1)
                ViewBag.Title = "SIGNUP SUCCESSFUL";
            else
                ViewBag.Title = "SIGNUP NOT SUCCESSFUL";
            return View();
        }
		
		public ActionResult WatchList()
        {
            DataTable watch_list = new DataTable();
            watch_list = CRUD.GetWatchList((int)Session["userID"]);
            return View(watch_list);
        }
		
		public ActionResult SearchResult(string movieName)
        {
            DataSet Movies = new DataSet();
            Movies = CRUD.Search(movieName);
            return View(Movies);
        }
		
		  public ActionResult UserLogout()
        {
            Session.Clear();
            return View("Index");
        }
    }
}

