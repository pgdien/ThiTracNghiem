using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using Model.Common;
namespace Model.Dao
{
    public class QuestionDao
    {
        private ThiTracNghiemDbContext db;

        public QuestionDao()
        {
            db = new ThiTracNghiemDbContext();
        }

        public IEnumerable<QuestionViewModel> ListQuestionInSubject(int id)
        {
            return from Question in db.Questions
                   join Thematic in db.Thematics on Question.id_thematic equals Thematic.id_thematic
                   where Thematic.id_subject == id
                   select new QuestionViewModel
                   {
                       id_question = Question.id_question,
                       id_thematic = Question.id_thematic,
                       question_content = Question.question_content,
                       img = Question.img,
                       A = Question.A,
                       B = Question.B,
                       C = Question.C,
                       D = Question.D,
                       correct_answer = Question.correct_answer,
                       is_change = Question.is_change,
                       is_essay = Question.is_essay,
                       id_subject = Thematic.id_subject,
                       thematic_name = Thematic.thematic_name
                   };
        }

        public bool Delete(int id)
        {
            try
            {
                Question result = db.Questions.Find(id);
                db.Questions.Remove(result);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int InsertList(List<Question> item, int id_thematic)
        {
            try
            {
                foreach (Question entity in item)
                {
                    if (entity.question_content != "")
                    {
                        if (entity.correct_answer == "A")
                        {
                            entity.correct_answer = entity.A;
                        }
                        else if (entity.correct_answer == "B")
                        {
                            entity.correct_answer = entity.B;
                        }
                        else if (entity.correct_answer == "C")
                        {
                            entity.correct_answer = entity.C;
                        }
                        else
                        {
                            entity.correct_answer = entity.D;
                        }
                        entity.id_thematic = id_thematic;
                        entity.is_change = 1;
                        db.Questions.Add(entity);
                    }
                }
                return db.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }

        public int Insert(Question entity)
        {
            if (entity.correct_answer == "A")
            {
                entity.correct_answer = entity.A;
            }
            else if (entity.correct_answer == "B")
            {
                entity.correct_answer = entity.B;
            }
            else if (entity.correct_answer == "C")
            {
                entity.correct_answer = entity.C;
            }
            else
            {
                entity.correct_answer = entity.D;
            }
            db.Questions.Add(entity);
            db.SaveChanges();
            return entity.id_question;
        }

        public bool Update(Question entity)
        {
            try
            {
                Question question = db.Questions.SingleOrDefault((Question x) => x.id_question == entity.id_question);
                question.id_thematic = entity.id_thematic;
                question.img = entity.img;
                question.is_change = entity.is_change;
                question.question_content = entity.question_content;
                question.A = entity.A;
                question.B = entity.B;
                question.C = entity.C;
                question.D = entity.D;
                if (entity.correct_answer == "A")
                {
                    entity.correct_answer = entity.A;
                }
                else if (entity.correct_answer == "B")
                {
                    entity.correct_answer = entity.B;
                }
                else if (entity.correct_answer == "C")
                {
                    entity.correct_answer = entity.C;
                }
                else
                {
                    entity.correct_answer = entity.D;
                }
                question.correct_answer = entity.correct_answer;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddQuestionsToExam(int id_exam, int id_question)
        {
            QuestionOfExam tmp = new QuestionOfExam();
            tmp.id_exam = id_exam;
            tmp.id_question = id_question;
            Question q = db.Questions.SingleOrDefault((Question x) => x.id_question == id_question);
            string[] answer2 = new string[4]
            {
            q.A,
            q.B,
            q.C,
            q.D
            };
            answer2 = ShuffleArray.Randomize(answer2);
            tmp.A = answer2[0];
            tmp.B = answer2[1];
            tmp.C = answer2[2];
            tmp.D = answer2[3];
            tmp.correct_answer = q.correct_answer;
            tmp.question_content = q.question_content;
            tmp.img = q.img;
            db.QuestionOfExams.Add(tmp);
            db.SaveChanges();
        }

        public List<Question> GetQuestionsByThematic(int id_thematic, int quest_of_thematic)
        {
            return (from x in db.Questions
                    where x.id_thematic == id_thematic
                    orderby Guid.NewGuid()
                    select x).Take(quest_of_thematic).ToList();
        }

        public Question ViewDetail(int id)
        {
            return db.Questions.SingleOrDefault((Question x) => x.id_question == id);
        }
    }

}
