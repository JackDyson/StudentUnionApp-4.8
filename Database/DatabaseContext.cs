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


    public DbSet<Student_Union_Test> Student_Union_Test { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<ErrorLog> ErrorLog { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

    #region SQL Queries

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
                Username = username
            };
            ErrorLog.Add(errorLog);
            SaveChanges();
        }
        catch (Exception ex)
        {
            ConnectToApi(ex.InnerException.Message);
        }
    }

    public void ConnectToApi(string innerException)
    {
        //try
        //{
        //    var client = new RestClient("https://abzorbapi.click2sign.co.uk:8075/api/");
        //    // Only run proxy logic on live code
        //    #if (!DEBUG)
        //    // Proxy logic to cater for idiots at IONOS
        //    client.Proxy = new System.Net.WebProxy("winproxy.server.lan", 3128);
        //    #endif
        //    // Set request type
        //    var request = new RestRequest($"Monitor/CheckApiStatusTwo?errorMessage={innerException}", Method.GET);
        //    // Add Authorization Header to request
        //    request.AddHeader("Authorization", "Basic YXdhaXMuaHVzc2FpbkBhYnpvcmIuY28udWs6S2Fscnk5NTQ/");
        //    // Return API response
        //    IRestResponse response = client.Execute(request);
        //}
        //catch (Exception e) { }
    }

    #endregion

    #endregion
}

[Table("Student_Union_Test")]
public class Student_Union_Test
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
}

[Table("ErrorLog")]
public class ErrorLog
{
    [Key]
    public long ErrorLogID { get; set; }
    public string ControllerName { get; set; }
    public string ControllerMethod { get; set; }
    public string ErrorMessage { get; set; }
    public string Username { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = "SU Site";
    public DateTime UpdatedTime { get; set; } = DateTime.Now;
    public string UpdatedBy { get; set; } = "SU Site";
}