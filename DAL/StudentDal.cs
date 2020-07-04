using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StudentDAL
    {
        public List<StudentModel> GetList()
        {
            string sql = "select * from student";
            List<StudentModel> smList = DapperHelper<StudentModel>.Query(sql);
            return smList;
        }
    }
}
