using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQListening
{
   
        public class ApplicationContext : DbContext
        {
            public DbSet<RabbitMQListening.InfoData> InfoDatas { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=RabbitMQDb;Username=postgres;Password=Murad3645");
            }
        }

    }

