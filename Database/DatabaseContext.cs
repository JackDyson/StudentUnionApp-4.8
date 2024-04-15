using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Text;

public class DatabaseContext : DbContext
{
    #region Constructor

#if DEBUG
    public DatabaseContext() : base("LocalConnection")
    {
        Database.SetInitializer<DatabaseContext>(null);
    }
#else
    public DatabaseContext() : base("LiveConnection")
    {
        Database.SetInitializer<DatabaseContext>(null);
    }
#endif
    #endregion


    public DbSet<Students> Students { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Email_Templates> Email_Templates { get; set; }
    public DbSet<ErrorLog> ErrorLog { get; set; }
    public DbSet<Societies> Societies { get; set; }
    public DbSet<Positions> Positions { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

    #region SQL Queries

    #region Students

    /// <summary>
    ///     Returns all students
    /// </summary>
    /// <returns></returns>
    public List<Students> GetStudents()
    {
        return Students.ToList();
    }

    /// <summary>
    ///     Adds a new student
    /// </summary>
    /// <param name="clubName"></param>
    /// <param name="position"></param>
    /// <param name="studentName"></param>
    /// <param name="preferredName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="agreementSigned"></param>
    /// <param name="trainingComplete"></param>
    /// <param name="membershipPurchased"></param>
    /// <param name="foodCertified"></param>
    public void AddStudent(string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
    {
        try
        {
            Students student = new Students()
            {
                Club_Name = clubName,
                Position = position,
                Student_Name = studentName,
                Preferred_Name = preferredName,
                Phone_Number = phoneNumber,
                Email_Address = emailAddress,
                Agreement_Signed = agreementSigned,
                Training_Complete = trainingComplete,
                Membership_Purchased = membershipPurchased,
                Food_Certified = foodCertified
            };
            Students.Add(student);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "AddStudent", ex.Message);
        }
    }

    /// <summary>
    ///     Updates an existing student
    /// </summary>
    /// <param name="id"></param>
    /// <param name="clubName"></param>
    /// <param name="position"></param>
    /// <param name="studentName"></param>
    /// <param name="preferredName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="agreementSigned"></param>
    /// <param name="trainingComplete"></param>
    /// <param name="membershipPurchased"></param>
    /// <param name="foodCertified"></param>
    public void UpdateStudent(int id, string clubName, string position, string studentName, string preferredName, string phoneNumber, string emailAddress, bool agreementSigned, bool trainingComplete, bool membershipPurchased, bool foodCertified)
    {
        try
        {
            Students student = Students.Find(id);
            student.Club_Name = clubName;
            student.Position = position;
            student.Student_Name = studentName;
            student.Preferred_Name = preferredName;
            student.Phone_Number = phoneNumber;
            student.Email_Address = emailAddress;
            student.Agreement_Signed = agreementSigned;
            student.Training_Complete = trainingComplete;
            student.Membership_Purchased = membershipPurchased;
            student.Food_Certified = foodCertified;
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "UpdateStudent", ex.Message);
        }
    }

    /// <summary>
    ///     Deletes an existing student
    /// </summary>
    /// <param name="id"></param>
    public void DeleteStudent(int id)
    {
        try
        {
            Students student = Students.Find(id);
            Students.Remove(student);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "DeleteStudent", ex.Message);
        }
    }

    #endregion

    #region Staff

    /// <summary>
    ///     Returns all staff members
    /// </summary>
    /// <returns></returns>
    public List<Staff> GetStaff()
    {
        return Staff.ToList();
    }

    /// <summary>
    ///     Adds a new staff member
    /// </summary>
    /// <param name="name"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="role"></param>
    public void AddStaff(string name, string email, string password, string role)
    {
        try
        {
            Staff staff = new Staff()
            {
                Name = name,
                Email = email,
                Password = password,
                Role = role,
                Reset_Password = true
            };
            Staff.Add(staff);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "AddStaff", ex.Message);
        }
    }

    /// <summary>
    ///     Updates an existing staff member
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="role"></param>
    /// <param name="passwordReset"></param>
    public void UpdateStaff(int id, string name, string role, bool passwordReset)
    {
        try
        {
            Staff staff = Staff.Find(id);
            staff.Name = name;
            staff.Role = role;
            staff.Reset_Password = passwordReset;
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "UpdateStaff", ex.Message);
        }
    }

    /// <summary>
    ///     Deletes an existing staff member
    /// </summary>
    /// <param name="id"></param>
    public void DeleteStaff(int id)
    {
        try
        {
            Staff staff = Staff.Find(id);
            Staff.Remove(staff);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "DeleteStaff", ex.Message);
        }
    }

    /// <summary>
    ///     Updates the password of a staff member
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public void UpdatePassword(string email, string password)
    {
        try
        {
            Staff staff = Staff.FirstOrDefault(s => s.Email == email);
            staff.Password = password;
            staff.Reset_Password = false;
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "UpdatePassword", ex.Message);
        }
    }

    #endregion

    #region Societies

    /// <summary>
    ///     Returns all societies
    /// </summary>
    /// <returns></returns>
    public List<Societies> GetSocieties()
    {
        return Societies.ToList();
    }

    /// <summary>
    ///     Adds a new society
    /// </summary>
    /// <param name="societyName"></param>
    public void AddSociety(string societyName)
    {
        try
        {
            Societies society = new Societies()
            {
                Society_Name = societyName
            };
            Societies.Add(society);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "AddSociety", ex.Message);
        }
    }

    /// <summary>
    ///     Deletes an existing society
    /// </summary>
    /// <param name="id"></param>
    public void DeleteSociety(int id)
    {
        try
        {
            Societies society = Societies.Find(id);
            Societies.Remove(society);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "DeleteSociety", ex.Message);
        }
    }

    #endregion

    #region Positions

    /// <summary>
    ///     Returns all positions
    /// </summary>
    /// <returns></returns>
    public List<Positions> GetPositions()
    {
        return Positions.ToList();
    }

    /// <summary>
    ///     Adds a new position
    /// </summary>
    /// <param name="positionName"></param>
    public void AddPosition(string positionName)
    {
        try
        {
            Positions position = new Positions()
            {
                Position_Name = positionName
            };
            Positions.Add(position);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "AddPosition", ex.Message);
        }
    }

    /// <summary>
    ///     Deletes an existing position
    /// </summary>
    /// <param name="id"></param>
    public void DeletePosition(int id)
    {
        try
        {
            Positions position = Positions.Find(id);
            Positions.Remove(position);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "DeletePosition", ex.Message);
        }
    }

    #endregion

    #region Email Templates

    /// <summary>
    ///     Returns all email templates
    /// </summary>
    /// <returns></returns>
    public List<Email_Templates> GetEmailTemplates() 
    { 
        return Email_Templates.ToList();
    }

    /// <summary>
    ///     Returns an email template by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Email_Templates GetEmailTemplate(int id)
    {
        return Email_Templates.Find(id);
    }

    /// <summary>
    ///     Adds a new email template
    /// </summary>
    /// <param name="title"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    public void AddEmailTemplate(string title, string subject, string content)
    {
        try
        {
            Email_Templates template = new Email_Templates()
            {
                Title = title,
                Subject = subject,
                Email_Content = content
            };
            Email_Templates.Add(template);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "AddEmailTemplate", ex.Message);
        }
    }

    /// <summary>
    ///     Updates an existing email template
    /// </summary>
    /// <param name="id"></param>
    /// <param name="title"></param>
    /// <param name="subject"></param>
    /// <param name="content"></param>
    public void UpdateEmailTemplate(int id, string title, string subject, string content)
    {
        try
        {
            Email_Templates template = Email_Templates.Find(id);
            template.Title = title;
            template.Subject = subject;
            template.Email_Content = content;
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "UpdateEmailTemplate", ex.Message);
        }
    }

    /// <summary>
    ///     Deletes an existing email template
    /// </summary>
    /// <param name="id"></param>
    public void DeleteEmailTemplate(int id)
    {
        try
        {
            Email_Templates template = Email_Templates.Find(id);
            Email_Templates.Remove(template);
            SaveChanges();
        }
        catch (Exception ex)
        {
            AddErrorLog("EmailSetUpController", "DeleteEmailTemplate", ex.Message);
        }
    }

    #endregion

    #region Error Log

    /// <summary>
    ///     Inserts a log message into the Error log table
    /// </summary>
    /// <param name="controller">Controller name</param>
    /// <param name="methodName">Controller method name</param>
    /// <param name="error">Error message</param>
    /// <param name="username">Username if available</param>
    public void AddErrorLog(string controller, string methodName, string error, string username = null)
    {
        try
        {
            ErrorLog errorLog = new ErrorLog()
            {
                ControllerName = controller,
                ControllerMethod = methodName,
                ErrorMessage = error,
                Username = "Jack"
            };
            ErrorLog.Add(errorLog);
            SaveChanges();
        }
        catch (Exception ex) { }
    }

    #endregion

    #endregion
}

# region Tables

[Table("Students")]
public class Students
{
    [Key]
    public int Student_ID { get; set; }
    public string Club_Name { get; set; }
    public string Position { get; set; }
    public string Student_Name { get; set; }
    public string Preferred_Name { get; set; }
    public string Phone_Number { get; set; }
    public string Email_Address { get; set; }
    public bool Agreement_Signed { get; set; }
    public bool Training_Complete { get; set; }
    public bool Membership_Purchased { get; set; }
    public bool Food_Certified { get; set; }

}

[Table("Staff")]
public class Staff
{
    [Key]
    public int Staff_ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool Reset_Password { get; set; }
}

[Table("ErrorLog")]
public class ErrorLog
{
    [Key]
    public int ErrorLogID { get; set; }
    public string ControllerName { get; set; }
    public string ControllerMethod { get; set; }
    public string ErrorMessage { get; set; }
    public string Username { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = "SU Site";
    public DateTime UpdatedTime { get; set; } = DateTime.Now;
    public string UpdatedBy { get; set; } = "SU Site";
}

[Table("Email_Templates")]
public class Email_Templates
{
    [Key]
    public int ID { get; set;}
    public string Title { get; set; }
    public string Subject { get; set; }
    public string Email_Content { get; set; }
}

[Table("Societies")]
public class Societies
{
    [Key]
    public int ID { get; set; }
    public string Society_Name { get; set; }

}

[Table("Society_Positions")]
public class Positions
{
    [Key]
    public int ID { get; set; }
    public string Position_Name { get; set; }
}

#endregion