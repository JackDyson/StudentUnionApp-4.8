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

    /// <summary>
    ///     Takes in a template Excel file and processes the data to upload students to the database
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public ActionResult ProcessBase64File(string file)
    {
        // Set EPPlus license context
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

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
                            // check if the worksheet has the correct column headers
                            if (worksheet.Cells[1, 1].Value?.ToString().ToUpper() != "CLUB NAME" ||
                                worksheet.Cells[1, 2].Value?.ToString().ToUpper() != "POSITION" ||
                                worksheet.Cells[1, 3].Value?.ToString().ToUpper() != "STUDENT NAME" ||
                                worksheet.Cells[1, 4].Value?.ToString().ToUpper() != "PREF. NAME" ||
                                worksheet.Cells[1, 5].Value?.ToString().ToUpper() != "PHONE" ||
                                worksheet.Cells[1, 6].Value?.ToString().ToUpper() != "EMAIL" ||
                                worksheet.Cells[1, 7].Value?.ToString().ToUpper() != "AGREEMENT" ||
                                worksheet.Cells[1, 8].Value?.ToString().ToUpper() != "TRAINING" ||
                                worksheet.Cells[1, 9].Value?.ToString().ToUpper() != "MEMBERSHIP" ||
                                worksheet.Cells[1, 10].Value?.ToString().ToUpper() != "FOOD")
                            {
                                return Json(new { success = false, message = "Incorrect file template. Please download the template in order to upload" });
                            }
                            
                            // Create a list of students to upload
                            List<UploadStudentModel> dataList = new List<UploadStudentModel>();

                            // Loop through the rows of the worksheet and add the student data to the list
                            for (int row = 3; row <= worksheet.Dimension.End.Row; row++) 
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

                                // if the student has all the required data, add them to the list
                                if (!string.IsNullOrEmpty(obj.ClubName) && !string.IsNullOrEmpty(obj.Position) && !string.IsNullOrEmpty(obj.Name) && !string.IsNullOrEmpty(obj.Email))
                                {
                                    dataList.Add(obj);
                                }

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

