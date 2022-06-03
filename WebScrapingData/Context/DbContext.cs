using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebScrapingData.Model;

namespace WebScrapingData.Context
{
    public class CustomerReviewContext : DbContext 
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public string DbPath { get; }
        public CustomerReviewContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "scraping.db");
        }
            // The following configures EF to create a Sqlite database file in the
            // special "local" folder for your platform.
            protected override void OnConfiguring(DbContextOptionsBuilder options)
                => options.UseSqlite($"Data Source={DbPath}");
    }
}