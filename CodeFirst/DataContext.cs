using System;
//using AspNet.Identity.SQLite;
using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst
{
    public class DataContext:DbContext
    {
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<User> Users { get; set; }


        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Chat.db");
        }
    }
/*
    public class ApplicationDbContext : SQLiteDatabase
    {
        public DbSet<Message> Messages { get; set; }
       
        public ApplicationDbContext(string connectionName)
            : base(connectionName)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext("Filename=Chat.db");
        }
    }*/
}
