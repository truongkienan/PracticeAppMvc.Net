using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PracticeAppMvc.Net.Models;

namespace PracticeAppMvc.Net.Data
{
    public class PracticeAppMvcNetContext : DbContext
    {
        public PracticeAppMvcNetContext (DbContextOptions<PracticeAppMvcNetContext> options)
            : base(options)
        {
        }

        public DbSet<PracticeAppMvc.Net.Models.Category> Category { get; set; } = default!;
    }
}
