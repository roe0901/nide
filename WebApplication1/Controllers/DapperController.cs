using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DapperController : Controller
    {
        // GET: Dapper
        public ActionResult Index()
        {
            StudentBLL sBLL = new StudentBLL();
            sBLL.GetList();
            return View();
        }
    }
}