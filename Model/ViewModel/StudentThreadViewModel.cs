using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.ViewModel
{
  public  class StudentThreadViewModel
    {
        public Student student
        {
            get;
            set;
        }

        public Student_Thread_Info student_thread
        {
            get;
            set;
        }

        public Thread thread
        {
            get;
            set;
        }

        public int countStudent
        {
            get;
            set;
        }

        public Subject subject
        {
            get;
            set;
        }

    }
}
