using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TestProject.Models;

namespace TestProject.Data
{
    public class OfficeDbContext : DbContext
    {
        public OfficeDbContext(DbContextOptions<OfficeDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship between Employee and Department
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            // Configure many-to-many relationship between Employee and Project
            modelBuilder.Entity<Employee>()
            .HasMany<Project>(e => e.Projects)
            .WithMany(p => p.Employees)
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeProject",
                x => x.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                x => x.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId")
            )
            .ToTable("EmployeeProjects");

            // Configure many-to-many relationship between Employee and Skill
            modelBuilder.Entity<Employee>()
            .HasMany<Skill>(e => e.Skills)
            .WithMany(s => s.Employees)
            .UsingEntity<Dictionary<string, object>>(
                "EmployeeSkill",
                x => x.HasOne<Skill>().WithMany().HasForeignKey("SkillId"),
                x => x.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId")
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
