using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockBox.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Identity.Context
{
    public class StockBoxIdentityDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public StockBoxIdentityDbContext(
            DbContextOptions<StockBoxIdentityDbContext> options)
            : base(options)
        {
        }
    }
}