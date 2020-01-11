using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using Model.Dao;
using Model.EF;
using Model.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
namespace DoAn_ThiTracNghiem_Complete.Areas.Admin.Code
{
    public class ImportExport
    {


        public Stream CreateExcelFile(int id_thread, Stream stream = null)
        {

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Nguyễn Tuấn Anh";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Danh sách đề thi";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "This is Comments";
                var dao = new ThreadDao();
                var listExam = dao.ListAllExam(id_thread);

                foreach (int iteam in listExam)
                {  // Add Sheet vào file Excel
                    excelPackage.Workbook.Worksheets.Add("De_" + iteam.ToString());
                }
                var workSheet = excelPackage.Workbook.Worksheets[1];
                foreach (int iteam in listExam)
                {
                    int i = 0;
                    i++;
                    var listQuest = dao.ListAllQuestion(iteam);
                    // Lấy Sheet để thao tác 
                    workSheet = excelPackage.Workbook.Worksheets["De_" + iteam.ToString()];
                    // Đỗ data vào Excel file
                    //  workSheet.Cells[1, 1].LoadFromCollection(listQuest, true, TableStyles.Dark9);
                    BindingFormatForExcel(workSheet, listQuest);


                }
                excelPackage.Save();



                return excelPackage.Stream;
            }
        }

        private void BindingFormatForExcel(ExcelWorksheet worksheet, List<QuestionViewModel> listItems)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 10;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;
            // Tạo header
            worksheet.Cells[1, 1].Value = "STT";
            worksheet.Cells[1, 2].Value = "Câu hỏi";
            worksheet.Cells[1, 3].Value = "Đáp án A";
            worksheet.Cells[1, 4].Value = "Đáp án B";
            worksheet.Cells[1, 5].Value = "Đáp án C";
            worksheet.Cells[1, 6].Value = "Đáp án D";
            worksheet.Cells[1, 7].Value = "Đáp án đúng";
            worksheet.Cells[1, 8].Value = "Chuyên đề";
            //Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            using (var range = worksheet.Cells["A1:H1"])
            {
                // Set PatternType
                range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                // Set Màu cho Background
                range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                // Canh giữa cho các text
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại
                range.Style.Font.SetFromFont(new Font("Arial", 10));
                // Set Border
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                // Set màu ch Border
                range.Style.Border.Bottom.Color.SetColor(Color.Blue);
            }

            // Đỗ dữ liệu từ list vào 
            for (int i = 0; i < listItems.Count; i++)
            {
                var item = listItems[i];
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = item.question_content;
                worksheet.Cells[i + 2, 3].Value = item.A;
                worksheet.Cells[i + 2, 4].Value = item.B;
                worksheet.Cells[i + 2, 5].Value = item.C;
                worksheet.Cells[i + 2, 6].Value = item.D;
                if (item.correct_answer == item.A)
                    worksheet.Cells[i + 2, 7].Value = "A";
                else if (item.correct_answer == item.B)
                    worksheet.Cells[i + 2, 7].Value = "B";
                else if (item.correct_answer == item.C)
                    worksheet.Cells[i + 2, 7].Value = "C";
                else worksheet.Cells[i + 2, 7].Value = "D";

                worksheet.Cells[i + 2, 8].Value = item.thematic_name;

            }

        }


        public Stream CreateExcelResultFile(int id_thread, Stream stream = null)
        {

            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = "Nguyễn Tuấn Anh";
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = "Danh sách đề thi";
                // thêm tí comments vào làm màu 
                excelPackage.Workbook.Properties.Comments = "This is Comments";
                var dao = new ThreadDao();
                var listStudent = dao.ListStudenCompleteThread(id_thread);
                excelPackage.Workbook.Worksheets.Add("DanhSachSinhVien");
                var listResult = dao.GetThreadResult(id_thread);
                var wS = excelPackage.Workbook.Worksheets["DanhSachSinhVien"];

                BindingFormatForExcelResult(wS, listResult, dao.ViewDetail(id_thread).thread_name + "- THỜI GIAN LÀM BÀI" + dao.ViewDetail(id_thread).time_to_do.ToString() + " PHÚT");
                foreach (var iteam in listStudent)
                {  // Add Sheet vào file Excel
                    excelPackage.Workbook.Worksheets.Add("masv_" + iteam.id_student.ToString());
                }
                //var workSheet= excelPackage.Workbook.Worksheets[1];
                foreach (var iteam in listStudent)
                {
                    int i = 0;
                    i++;
                    var listResultStudent = dao.ListQuestionResult(iteam.id_student, id_thread);
                    // Lấy Sheet để thao tác 
                    var workSheet = excelPackage.Workbook.Worksheets["masv_" + iteam.id_student.ToString()];
                    // Đỗ data vào Excel file
                    //  workSheet.Cells[1, 1].LoadFromCollection(listQuest, true, TableStyles.Dark9);
                    BindingFormatForExcelStudentResult(workSheet, listResultStudent, iteam.student_name, iteam.id_student.ToString());


                }
                excelPackage.Save();



                return excelPackage.Stream;
            }
        }
        private void BindingFormatForExcelResult(ExcelWorksheet worksheet, List<StudentThreadInfoViewModel> listItems, string thread_name)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 10;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;
            // Tạo header


            worksheet.Cells[2, 1].Value = "STT";
            worksheet.Cells[2, 2].Value = "Tên sinh viên";
            worksheet.Cells[2, 3].Value = "Ngày sinh";
            worksheet.Cells[2, 4].Value = "Giới tính";
            worksheet.Cells[2, 5].Value = "Điểm";
            worksheet.Cells[2, 6].Value = "Thời gian làm bài (phút)";

            //Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            using (var range = worksheet.Cells["A2:F2"])
            {
                // Set PatternType
                range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                // Set Màu cho Background
                range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                // Canh giữa cho các text
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại
                range.Style.Font.SetFromFont(new Font("Arial", 10));
                // Set Border
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                // Set màu ch Border
                range.Style.Border.Bottom.Color.SetColor(Color.Blue);
            }
            int count = 0;
            // Đỗ dữ liệu từ list vào 
            for (int i = 0; i < listItems.Count; i++)
            {

                var item = listItems[i];
                DateTime birthday = item.student.student_birtday ?? default(DateTime);

                worksheet.Cells[i + 3, 1].Value = i + 1;
                worksheet.Cells[i + 3, 2].Value = item.student.student_name;
                worksheet.Cells[i + 3, 3].Value = birthday.ToShortDateString();
                if (item.student.student_gender.Value)
                {
                    worksheet.Cells[i + 3, 4].Value = "Nam";
                }
                else worksheet.Cells[i + 3, 4].Value = "Nữ";
                worksheet.Cells[i + 3, 5].Value = item.student_thread.score_number;

                TimeSpan? a = item.student_thread.end_time - item.student_thread.start_time;
                TimeSpan timetodo = a ?? default(TimeSpan);
                worksheet.Cells[i + 3, 6].Value = Math.Round(timetodo.TotalMinutes, 0);
                count++;

            }

            worksheet.Cells[1, 1].Value = "DANH SÁCH SINH VIÊN CỦA " + thread_name.ToUpper();
            worksheet.Cells["A1:F1"].Merge = true;
            worksheet.Cells["A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Column(1).Width = 5;
            worksheet.Column(2).Width = 25;
            worksheet.Column(3).Width = 20;
            worksheet.Column(4).Width = 20;
            worksheet.Column(5).Width = 10;
            worksheet.Column(6).Width = 26;
            worksheet.Cells["A3:A100"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["C3:F100"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }


        private void BindingFormatForExcelStudentResult(ExcelWorksheet worksheet, List<StudentQuestViewModel> listItems, string student_name, string id_student)
        {
            // Set default width cho tất cả column
            worksheet.DefaultColWidth = 10;
            // Tự động xuống hàng khi text quá dài
            worksheet.Cells.Style.WrapText = true;
            // Tạo header


            worksheet.Cells[2, 1].Value = "STT";
            worksheet.Cells[2, 2].Value = "Câu hỏi";
            worksheet.Cells[2, 3].Value = "Đáp án A";
            worksheet.Cells[2, 4].Value = "Đáp án B";
            worksheet.Cells[2, 5].Value = "Đáp án C";
            worksheet.Cells[2, 6].Value = "Đáp án D";
            worksheet.Cells[2, 7].Value = "Đáp án đúng";
            worksheet.Cells[2, 8].Value = "Đáp án của sinh viên";
            //Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            using (var range = worksheet.Cells["A2:H2"])
            {
                // Set PatternType
                range.Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                // Set Màu cho Background
                range.Style.Fill.BackgroundColor.SetColor(Color.Aqua);
                // Canh giữa cho các text
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                // Set Font cho text  trong Range hiện tại
                range.Style.Font.SetFromFont(new Font("Arial", 10));
                // Set Border
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                // Set màu ch Border
                range.Style.Border.Bottom.Color.SetColor(Color.Blue);

            }
            int count = 0;
            // Đỗ dữ liệu từ list vào 
            for (int i = 0; i < listItems.Count; i++)
            {
                var item = listItems[i];
                worksheet.Cells[i + 3, 1].Value = i + 1;
                worksheet.Cells[i + 3, 2].Value = item.question_exam.question_content;
                worksheet.Cells[i + 3, 3].Value = item.question_exam.A;
                worksheet.Cells[i + 3, 4].Value = item.question_exam.B;
                worksheet.Cells[i + 3, 5].Value = item.question_exam.C;
                worksheet.Cells[i + 3, 6].Value = item.question_exam.D;
                if (item.question_exam.correct_answer == item.question_exam.A)
                    worksheet.Cells[i + 3, 7].Value = "A";
                else if (item.question_exam.correct_answer == item.question_exam.B)
                    worksheet.Cells[i + 3, 7].Value = "B";
                else if (item.question_exam.correct_answer == item.question_exam.C)
                    worksheet.Cells[i + 3, 7].Value = "C";
                else worksheet.Cells[i + 3, 7].Value = "D";

                if (item.student_thread.student_answer == item.question_exam.A)
                    worksheet.Cells[i + 3, 8].Value = "A";
                else if (item.student_thread.student_answer == item.question_exam.B)
                    worksheet.Cells[i + 3, 8].Value = "B";
                else if (item.student_thread.student_answer == item.question_exam.C)
                    worksheet.Cells[i + 3, 8].Value = "C";
                else worksheet.Cells[i + 3, 8].Value = "D";
                if (item.question_exam.correct_answer == item.student_thread.student_answer) count++;

            }

            worksheet.Cells[1, 1].Value = "Tên sinh viên: " + student_name + "\r\n Mã sinh viên: " + id_student + "\r\n Mã đề thi: " + listItems.FirstOrDefault().question_exam.id_exam + "\r\n Làm được: " + count.ToString() + "/" + listItems.Count.ToString();
            worksheet.Cells["A1:H1"].Merge = true;
            worksheet.Row(1).Height = 100;
            worksheet.Column(2).Width = 50;
            worksheet.Column(1).Width = 5;
            worksheet.Column(3).Width = 30;
            worksheet.Column(4).Width = 30;
            worksheet.Column(5).Width = 30;
            worksheet.Column(6).Width = 30;
            worksheet.Column(7).Width = 15;
            worksheet.Column(8).Width = 20;
            worksheet.Cells["A1:H1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells["A3:A100"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells["G3:H100"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }
        public List<Question> ReadFromExcelfile(string path, string sheetName)
        {
            try
            {
                // Khởi tạo data table
                DataTable dt = new DataTable();

                // Load file excel và các setting ban đầu
                using (ExcelPackage package = new ExcelPackage(new FileInfo(path)))
                {
                    if (package.Workbook.Worksheets.Count < 1)
                    { // Log - Không có sheet nào tồn tại trong file excel của bạn 
                        return null;
                    }
                    // Lấy Sheet đầu tiện trong file Excel để truy vấn 
                    // Truyền vào name của Sheet để lấy ra sheet cần, nếu name = null thì lấy sheet đầu tiên 
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.FirstOrDefault(x => x.Name == sheetName) ?? package.Workbook.Worksheets.FirstOrDefault();
                    // Đọc tất cả các header
                    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                    {
                        dt.Columns.Add(firstRowCell.Text);
                    }
                    // Đọc tất cả data bắt đầu từ row thứ 2
                    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                    {
                        // Lấy 1 row trong excel để truy vấn
                        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                        // tạo 1 row trong data table
                        var newRow = dt.NewRow();
                        foreach (var cell in row)
                        {
                            newRow[cell.Start.Column - 1] = cell.Text;
                        }
                        dt.Rows.Add(newRow);
                    }
                }
                var convertedList = (from rw in dt.AsEnumerable()
                                     select new Question()
                                     {
                                         question_content = rw["Content"].ToString(),
                                         A = Convert.ToString(rw["A"]),
                                         B = Convert.ToString(rw["B"]),
                                         C = Convert.ToString(rw["C"]),
                                         D = Convert.ToString(rw["D"]),
                                         correct_answer = Convert.ToString(rw["Answer"]),
                                         id_thematic = 0,
                                     }).ToList();
                return convertedList;
            }
            catch (Exception )
            {
                return null;
            }
        }
    }
}