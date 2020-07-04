using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class RedisController : Controller
    {
        // GET: Redis
        public ActionResult Index()
        {
            CommonRedisAssistant redis = CommonRedisAssistant.Instance;
            string a = redis.Get<string>("test");
            return View();
        }
    }
}