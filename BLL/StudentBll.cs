using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{

    public class StudentBLL
    {
        private StudentDAL sDAL = new StudentDAL();
        public List<StudentModel> GetList()
        {
            return sDAL.GetList();
        }
    }
}
