using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.ViewModel
{
 public   class ResultViewModel
    {
        public Thread thread { get; set; }
       public Subject subject { get; set; }
        public Student_Thread_Info student_info { get; set; }

    }
}
