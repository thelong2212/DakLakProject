using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using WebApplication5.Models;

namespace WebApplication5
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=test")
        {
        }

        public virtual DbSet<Student_test> Student_test { get; set; }
        public virtual DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student_test>()
                .Property(e => e.LastName)
                .IsFixedLength();
            modelBuilder.Entity<Course>()
                .Property(e => e.Title)
                .IsFixedLength();
        }
    }
}
