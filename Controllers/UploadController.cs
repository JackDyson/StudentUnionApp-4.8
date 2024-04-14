using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Management;
using System.Web.Mvc;
using OfficeOpenXml;
using StudentUnionApp.Models;

public class Upload : Controller
{
    DatabaseContext _context = new DatabaseContext();

    [HttpPost]
    [Authorize]
    public ActionResult ProcessBase64File(string file)
    {
        // Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or LicenseContext.Commercial

        if (!string.IsNullOrEmpty(file))
        {
            try
            {
                // Decode base64 string to byte array
                byte[] bytes = Convert.FromBase64String(file);

                // Convert byte array to stream
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    using (ExcelPackage package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null)
                        {
                            // Assuming your object type is ExampleObject
                            List<UploadStudentModel> dataList = new List<UploadStudentModel>();

                            for (int row = 3; row <= worksheet.Dimension.End.Row; row++) // Assuming data starts from row 2
                            {
                                UploadStudentModel obj = new UploadStudentModel();
                                {
                                    obj.ClubName = worksheet.Cells[row, 1].Value?.ToString();
                                    obj.Position = worksheet.Cells[row, 2].Value?.ToString();
                                    obj.Name = worksheet.Cells[row, 3].Value?.ToString();
                                    obj.PreferredName = worksheet.Cells[row, 4].Value?.ToString();
                                    obj.Phone = worksheet.Cells[row, 5].Value?.ToString();
                                    obj.Email = worksheet.Cells[row, 6].Value?.ToString();
                                    obj.Agreement = worksheet.Cells[row, 7].Value?.ToString().ToUpper() == "TRUE";
                                    obj.Training = worksheet.Cells[row, 8].Value?.ToString().ToUpper() == "TRUE";
                                    obj.Membership = worksheet.Cells[row, 9].Value?.ToString().ToUpper() == "TRUE";
                                    obj.Food = worksheet.Cells[row, 10].Value?.ToString().ToUpper() == "TRUE";
                                };

                                dataList.Add(obj);
                            }
                            // Upload data to database
                            int uploadedStudents = 0;
                            foreach (var item in dataList)
                            {
                                List<Students> databaseStudents = _context.GetStudents();
                                List<Societies> databaseClubs = _context.GetSocieties();
                                List<Positions> databasePositions = _context.GetPositions();
                                // check if the student already exists in the database and skip if they do
                                if (databaseStudents.Any(s => s.Student_Name == item.Name && s.Email_Address == item.Email && s.Club_Name == item.ClubName))
                                {
                                    continue;
                                }
                                // check if the club does not exist in the database and add it if it does not
                                if (!databaseClubs.Any(c => c.Society_Name == item.ClubName))
                                {
                                    _context.AddSociety(item.ClubName);
                                }
                                // check if the position does not exist in the database and add it if it does not
                                if (!databasePositions.Any(p => p.Position_Name == item.Position))
                                {
                                    _context.AddPosition(item.Position);
                                }
                                // Add the student to the database
                                _context.AddStudent(item.ClubName, item.Position, item.Name, item.PreferredName, item.Phone, item.Email, item.Agreement, item.Training, item.Membership, item.Food);
                                uploadedStudents++;
                            }

                            return Json(new { success = true, message = uploadedStudents.ToString() + " Students Were Successfully Uploaded" });
                        }
                        else
                        {
                            return Json(new { success = false, message = "No data found in the Excel file" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        else
        {
            return Json(new { success = false, message = "No file uploaded" });
        }
    }

}

