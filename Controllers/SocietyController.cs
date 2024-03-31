using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentUnionApp.Controllers
{
    public class SocietyController : Controller
    {
        DatabaseContext DatabaseContext = new DatabaseContext();
        // GET: Society
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        #region Database Methods

        // get list of societies
        [Authorize]
        public ActionResult GetSocieties()
        {
            try
            {
                return Json(DatabaseContext.Societies.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // delete society
        [Authorize]
        public ActionResult DeleteSociety(int id)
        {
            try
            {
                Societies society = DatabaseContext.Societies.Find(id);
                DatabaseContext.Societies.Remove(society);
                DatabaseContext.SaveChanges();
                return Json(new { success = true, message = "Society deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // add society
        [Authorize]
        public ActionResult AddSociety(Societies society)
        {
            try
            {
                DatabaseContext.Societies.Add(society);
                DatabaseContext.SaveChanges();
                return Json(new { success = true, message = "Society added successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // get list of positions
        [Authorize]
        public ActionResult GetPositions()
        {
            try
            {
                return Json(DatabaseContext.Positions.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // delete position
        [Authorize]
        public ActionResult DeletePosition(int id)
        {
            try
            {
                Positions position = DatabaseContext.Positions.Find(id);
                DatabaseContext.Positions.Remove(position);
                DatabaseContext.SaveChanges();
                return Json(new { success = true, message = "Position deleted successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // add position
        [Authorize]
        public ActionResult AddPosition(Positions position)
        {
            try
            {
                DatabaseContext.Positions.Add(position);
                DatabaseContext.SaveChanges();
                return Json(new { success = true, message = "Position added successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}