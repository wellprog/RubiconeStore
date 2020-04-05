using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Shared.Model;

namespace RubiconeStoreBack.DataAccess
{
    public class DbStore : DbContext
    {
        private readonly string _connectionString;
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Check> Checks { get; set; }
        public DbSet<CheckSell> CheckSells { get; set; }
        public DbSet<Sell> Sells { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<GoodCategory> GoodCategories { get; set; }
        public DbSet<GoodProperty> GoodProperties { get; set; }
        public DbSet<GoodPropertyValue> GoodPropertyValues { get; set; }

        public DbStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }
    }
}
