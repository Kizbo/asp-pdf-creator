using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTML2PDF.Controllers
{
    public class UstawieniaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fullPath = Request.MapPath("~/App_Data/uploads/logo");
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                string extension = Path.GetExtension(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "logo" + extension);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }
    }

   

}