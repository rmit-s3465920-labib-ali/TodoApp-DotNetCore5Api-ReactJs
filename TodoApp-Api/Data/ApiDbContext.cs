using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_RestApi.Models;

namespace ToDo_RestApi.Data
{
    public class ApiDbContext : DbContext
    {
        public virtual DbSet<TodoItem> Items { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {

        }
    }
}
