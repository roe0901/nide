using Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace WebApplication1.Controllers
{
    public class YourDiaryController : Controller
    {
        // GET: YourDiary
        public ActionResult Index()
        {
            Log4netHelper.Info("ip:"+Request.UserHostAddress+";时间："+DateTime.Now);
            List<string> dateLs = new List<string>();
            List<string> dateIDLs = new List<string>();


            ViewBag.dateLs = dateLs;
            ViewBag.dateIDLs = dateIDLs;

            return View();
        }

        [HttpPost]
        public string getDiary()
        {
            string times = Request["times"];
            int type = Convert.ToInt32(Request["type"]);

            int timesYear = Convert.ToDateTime(times).Year;
            int timesMonth = Convert.ToDateTime(times).Month;
            string result = "";
            Common.CommonHelper.HttpGet("https://nideriji.cn/api/diary/simple_by_month/" + timesYear + "/" + timesMonth + "/", out result, type);
            dynamic obj = JsonConvert.DeserializeObject(result);
            if (obj.diaries != null || obj.diaries != "")
            {
                if (obj.diaries[times] != null)
                {
                    Common.CommonHelper.HttpGet("https://nideriji.cn/api/diary/" + obj.diaries[times] + "/", out result, type);
                    result = Common.CommonHelper.Unicode2String(result);
                    /*dynamic obj2 = JsonConvert.DeserializeObject(result);
                    result = obj2.diary.content;*/
                }
                else
                {
                    result = "0";
                }

            }
            else
            {
                result = "0";
            }

            return result;
        }

       
    }
}