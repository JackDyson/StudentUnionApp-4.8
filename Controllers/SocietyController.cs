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
        public ActionResult Index()
        {
            return View();
        }

        // get list of societies
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
    }
}