using Microsoft.EntityFrameworkCore;    
using System.ComponentModel.DataAnnotations.Schema;

public class User {
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Role Role { get; set; }

    public string GetGender() {
        string gender = "";
        switch (Gender) {
            case Gender.Male: gender = "Male";
            break;
            case Gender.Female: gender = "Female";
            break;
            case Gender.Null: gender = "Not Specified";
            break;
        }
        return gender;
    }
    
    public string GetRole() {
        string role = "";
        switch (Role) {
            case Role.Student: role = "Student";
            break;
            case Role.Teacher: role = "Teacher";
            break;
            case Role.Parent: role = "Parent";
            break;
        }
        return role;
    }

    // Navigation Properties
    public List<Cart> Cart { get; set; } = [];  
}

public class Product {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }

    // Navigation Properties
    public List<Cart> Cart { get; set; } = [];
}

public class Cart {
    public int CartId { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public User User { get; set; } = new();
    public Product Product { get; set; } = new();
}

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options) {
    public DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Cart> Cart { get; set; }
}

public enum Gender {
    Male = 0,
    Female = 1,
    Null = 2
}

public enum Role {
    Student = 0,
    Teacher = 1,
    Parent = 2
}
