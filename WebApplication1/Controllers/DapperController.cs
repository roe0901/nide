using BLL;
using Common;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch timer = new Stopwatch();
            timer.Start();

            StudentBLL sBLL = new StudentBLL();
            List<StudentModel> smList= sBLL.GetList();

            timer.Stop();
            TimeSpan ts = timer.Elapsed;
            Log4netHelper.Info("sql读取"+smList.Count+"学生数据耗时：" + ts.TotalMilliseconds);

            //将数据写入redis
            CommonRedisAssistant redis = CommonRedisAssistant.Instance;
            redis.Set<List<StudentModel>>("Student", smList);


            Stopwatch timer2 = new Stopwatch();
            timer2.Start();

            List<StudentModel> smList2 = redis.Get<List<StudentModel>>("Student");

            timer2.Stop();
            TimeSpan ts2 = timer2.Elapsed;
            Log4netHelper.Info("redis读取" + smList2.Count + "学生数据耗时：" + ts2.TotalMilliseconds);



            return View();
        }
    }
}