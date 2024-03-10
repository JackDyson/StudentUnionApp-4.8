using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

public class StudentUnionContext : DbContext
{
    public StudentUnionContext() : base("name=StudentUnionDb")
    {
    }

    public DbSet<Student_Union_Test> Student_Union_Test { get; set; }
    public DbSet<Staff> Staff { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
}

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

public class Staff
{
    [Key]
    public int Staff_ID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}