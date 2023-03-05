using AspNetCoreCRUD.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreCRUD.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions options) : base(options) { }

        /// <summary>Сущность студента</summary>
        public DbSet<Student> Students { get; set; }
    }
}
