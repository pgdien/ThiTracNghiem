using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.ViewModel
{
  public  class StudentQuestViewModel
    {
        public Thread thread { get; set; }
        public QuestionOfExam question_exam { get; set; }
        public Student_Thread_Detail student_thread { get; set; }
        
    }
}
