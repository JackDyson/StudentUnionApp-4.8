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

        /// <summary>
        ///     Returns a list of societies from the database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Deletes an existing society from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Adds a society to the database
        /// </summary>
        /// <param name="society"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Returns a list of positions from the database
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Deletes an existing position from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        ///     Add a position to the database
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
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