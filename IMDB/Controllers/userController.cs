using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMDB.Models;

namespace IMDB.Controllers
{
    public class userController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChangeUserName(string userName)
        {
            CRUD.UpdateUserName((int)Session["userID"], userName);
            Session["userName"] = userName;
            return RedirectToAction("Index", "User");
        }
        public ActionResult ChangePassword(string password)
        {
            CRUD.UpdateUserPassword((int)Session["userID"], password);
            Session["userPassword"] = password;
            return RedirectToAction("Index", "User");
        }

    }
}
