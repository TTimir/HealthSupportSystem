using DatabaseLayer;
using HealthSupportSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HealthSupportSystem.Controllers
{
    public class QuizAssesmentController : Controller
    {
        private HealthSupportSysdbEntities db = new HealthSupportSysdbEntities();

        // GET: QuizAssesment
        public ActionResult Dashboard()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult ExamDashboard()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ExamDashboard(string room)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<quiz_Category> list = db.quiz_Category.ToList();
            foreach (var item in list)
            {
                if (item.cat_encrypted_string == room)
                {

                    List<quiz_Questions> li = db.quiz_Questions.Where(x => x.q_fk_Cat_id == item.Cat_id).ToList();
                    Queue<quiz_Questions> queue = new Queue<quiz_Questions>();
                    foreach (quiz_Questions a in li)
                    {
                        queue.Enqueue(a);
                    }


                    TempData["examid"] = item.Cat_fk_DoctorID;
                    TempData["questions"] = queue;
                    TempData["score"] = 0;

                    // Set the categoryId in TempData
                    TempData["categoryId"] = item.Cat_id;

                    TempData.Keep();
                    return RedirectToAction("StartQuiz");
                }
                ViewBag.Message = "No Room Found!";
            }

            return View();
        }

        public ActionResult StartQuiz()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                quiz_Questions q = null;
                if (TempData["questions"] != null)
                {
                    Queue<quiz_Questions> qlist = (Queue<quiz_Questions>)TempData["questions"];
                    if (qlist.Count > 0)
                    {
                        q = qlist.Peek();
                        qlist.Dequeue();

                        TempData["questions"] = qlist;
                        TempData.Keep();
                    }
                    else
                    {
                        return RedirectToAction("EndExam");
                    }
                }
                else
                {
                    return RedirectToAction("ExamDashboard");
                }
                TempData.Keep();
                return View(q);
            }
            catch
            {
                return RedirectToAction("ExamDashboard");
            }

        }

        [HttpPost]

        public ActionResult StartQuiz(quiz_Questions q)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            string correctans = null;
            if (!string.IsNullOrEmpty(q.QA))
            {
                correctans = "A";
            }
            else if (!string.IsNullOrEmpty(q.QB))
            {
                correctans = "B";
            }
            else if (!string.IsNullOrEmpty(q.QC))
            {
                correctans = "C";
            }
            else if (!string.IsNullOrEmpty(q.QD))
            {
                correctans = "D";
            }


            if (q.QCorrectAns != null && correctans.Equals(q.QCorrectAns))
            {
                TempData["score"] = Convert.ToInt32(TempData["score"]) + 1;
                //scoreExam = scoreExam + 1;
            }

            TempData.Keep();
            //sc.scoree[2] = scoreExam.ToString();
            return RedirectToAction("StartQuiz");
        }

        public ActionResult EndExam(quiz_Questions q)
        {
            var patient = (PatientTable)Session["Patient"];
            int patientId = patient.PatientID;

            string patientName = string.Empty;
            patientName = db.PatientTables.Where(x => x.PatientID == patientId).Select(x => x.Name).FirstOrDefault();

            if (Session["Patient"] != null && TempData["examid"] != null && TempData["score"] != null)
            {
                int examId = (int)TempData["examid"];
                int score = (int)TempData["score"];
                DateTime examDate = DateTime.Now;

                // Fetch the category ID associated with the exam result
                int categoryId = (int)TempData["categoryId"];
                // Fetch the category name from the database using the category ID
                string categoryName = db.quiz_Category.Where(x => x.Cat_id == categoryId).Select(x => x.Cat_name).FirstOrDefault();


                System.Diagnostics.Debug.WriteLine("Cat Id: " + categoryId);
                System.Diagnostics.Debug.WriteLine("Cat name: " + categoryName);
                System.Diagnostics.Debug.WriteLine("patient ID: " + patientId);
                System.Diagnostics.Debug.WriteLine("patient Name: " + patientName);

                //string categoryName = "Health Self-Assesment";

                // Create a new exam result object
                quiz_Result examResult = new quiz_Result
                {
                    Exam_Date = examDate,
                    Exam_fk_stud = patientId,
                    Patient_name = patientName,
                    Exam_name = categoryName,
                    Patient_Score = score
                };

                // Add the exam result to the database
                db.quiz_Result.Add(examResult);
                db.SaveChanges();

                TempData["patientName"] = patientName;
                TempData["categoryName"] = categoryName;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Add_Category()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            //Session["ad_id"] = 2;
            var doc = (DoctorTable)Session["Doctor"];
            int doc_Id = doc.DoctorID;

            List<quiz_Category> catlist = db.quiz_Category
                .Where(x => x.Cat_fk_DoctorID == doc_Id)
                .OrderByDescending(x => x.Cat_id)
                .ToList();

            ViewData["list"] = catlist;
            return View();
        }

        [HttpPost]
        public ActionResult Add_Category(quiz_Category cat)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            List<quiz_Category> catlist = db.quiz_Category.OrderByDescending(x => x.Cat_id).ToList();
            ViewData["list"] = catlist;
            quiz_Category c = new quiz_Category();
            Random r = new Random();

            var doc = (DoctorTable)Session["Doctor"];

            c.Cat_name = cat.Cat_name;
            c.cat_encrypted_string = CryptMV.Encrypt(cat.Cat_name.Trim() + r.Next().ToString(), true);
            c.Cat_fk_DoctorID = doc.DoctorID;

            db.quiz_Category.Add(c);
            db.SaveChanges();

            return RedirectToAction("Add_Category");
        }

        [HttpGet]
        public ActionResult Add_Questions()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            int docId = doc.DoctorID;
            List<quiz_Category> categories = db.quiz_Category
                                             .Where(x => x.Cat_fk_DoctorID == docId)
                                             .ToList();
            ViewBag.list = new SelectList(categories, "cat_id", "cat_name");
            return View();
        }

        [HttpPost]
        public ActionResult Add_Questions(quiz_Questions questions)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            var doc = (DoctorTable)Session["Doctor"];
            int sessid = doc.DoctorID;

            List<quiz_Category> list = db.quiz_Category.Where(x => x.Cat_id == sessid).ToList();
            ViewBag.list = new SelectList(list, "Cat_id", "Cat_name");

            quiz_Questions qa = new quiz_Questions();
            qa.q_text = questions.q_text;
            qa.QA = questions.QA;
            qa.QB = questions.QB;
            qa.QC = questions.QC;
            qa.QD = questions.QD;
            qa.QCorrectAns = questions.QCorrectAns;
            qa.q_fk_Cat_id = questions.q_fk_Cat_id;

            db.quiz_Questions.Add(qa);
            db.SaveChanges();

            TempData["Message"] = "Questions Added Succesfully!";
            TempData.Keep();

            return RedirectToAction("Add_Questions");
        }

        public ActionResult ViewAllQuestions(int? id, int? page)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return RedirectToAction("Dashboard");
            }

            return View(db.quiz_Questions.Where(x => x.q_fk_Cat_id == id).ToList());
        }

        public ActionResult Report()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            // Fetch exam results from the database
            List<quiz_Result> examResults = db.quiz_Result.ToList();

            // Pass the exam results to the view
            return View(examResults);
        }


        public ActionResult PatientReport()
        {
            // Check if the user is logged in
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            // Retrieve the logged-in patient's information from the session
            var patient = (PatientTable)Session["Patient"];
            if (patient == null)
            {
                // Handle the case where patient information is not found in the session
                return RedirectToAction("Login", "Home");
            }

            // Fetch exam results for the logged-in patient from the database
            int patientId = patient.PatientID;
            List<quiz_Result> patientExamResults = db.quiz_Result
                                                   .Where(x => x.Exam_fk_stud == patientId)
                                                   .ToList();

            // Pass the exam results to the view
            return View(patientExamResults);
        }

        // GET: QuizAssesment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            quiz_Questions question = db.quiz_Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            return View(question);
        }

        // POST: QuizAssesment/Delete/5
        [HttpPost, ActionName("DeleteQuestion")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserName"])))
            {
                return RedirectToAction("Login", "Home");
            }

            quiz_Questions question = db.quiz_Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }

            db.quiz_Questions.Remove(question);
            db.SaveChanges();

            TempData["Message"] = "Question deleted successfully!";
            TempData.Keep();

            return RedirectToAction("ViewAllQuestions", new { id = question.q_fk_Cat_id });
        }


        public ActionResult Index()
        {
            return View();
        }
    }
}