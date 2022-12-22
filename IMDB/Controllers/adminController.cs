using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMDB.Models;

using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace IMDB.Controllers
{
    public class adminController : Controller
    {
        //
        // GET: /admin/

        public ActionResult Index()
        {
            return View();
        }

    }
}
