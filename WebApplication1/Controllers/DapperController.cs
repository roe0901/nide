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
            Log4netHelper.Info("sql读取学生数据耗时：" + ts.TotalMilliseconds);

            RedisHelper.KeyDelete("Students");
            //将数据写入redis
            for (int i = 0; i < smList.Count; i++)
            {
                RedisHelper.HashSet<StudentModel>("Students", i.ToString(),smList[i]);
            }

           


            Stopwatch timer2 = new Stopwatch();
            timer2.Start();

            List<StudentModel> smList2 = RedisHelper.HashGetAll<StudentModel>("Students");

            timer2.Stop();
            TimeSpan ts2 = timer2.Elapsed;
            Log4netHelper.Info("redis读取学生数据耗时：" + ts2.TotalMilliseconds);



            return View();
        }
    }
}