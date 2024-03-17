using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class StudentController : Controller
    {
        #region Views

        public ActionResult Index()
        {
            return View();
        }

        #endregion

    }
}