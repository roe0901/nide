using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            /*Stopwatch timer = new Stopwatch();
           timer.Start();
          CommonRedisAssistant redis = CommonRedisAssistant.Instance;
           List<StudentModel> a = redis.Get<List<StudentModel>>("Student");
           timer.Stop();
           TimeSpan ts2 = timer.Elapsed;
           Log4netHelper.Info("redis读取"+ a.Count + "学生数据耗时：" + ts2.TotalMilliseconds);*/
            return View();
        }
    }
}