using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Context
{
    public class EfDbContext : DbContext
    {
        public EfDbContext() : base("EfDbContext")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }
    }

    public class EfDbInitializer : CreateDatabaseIfNotExists<EfDbContext>//DropCreateDatabaseIfModelChanges<EfDbContext> //DropCreateDatabaseAlways<EfDbContext>
    {
        protected override void Seed(EfDbContext context)
        {
            context.Roles.AddRange(new List<Role> {
                new Role { Name = "Guest", DisplayName="Гость" },
                new Role { Name = "User", DisplayName="Пользователь" },
                new Role { Name = "Administrator", DisplayName="Администратор" },
            });

            context.SaveChanges();

            context.Groups.AddRange(new List<Group> {
                new Group { Name = "Guests", DisplayName="Гости", RoleId=1 },
                new Group { Name = "Users", DisplayName="Пользователи", RoleId=2 },
                new Group { Name = "Administrators", DisplayName="Администраторы", RoleId=3 },
            });

            context.SaveChanges();

            context.Users.AddRange(new List<User> {
                new User { Login = "Guest", Password="111111", GroupId=1 },
                new User { Login = "User", Password="111111", GroupId=2 },
                new User { Login = "Admin", Password="111111", GroupId=3 },
            });

            context.SaveChanges();

            context.ProductTypes.AddRange(new List<ProductType> {
                new ProductType { Name="Electric", Description="Струны для электрогитары" },
                new ProductType { Name="Bass", Description="Струны для бас-гитары" },
                new ProductType { Name="Acoustic", Description="Струны для акустической гитары" },
            });

            context.SaveChanges();

            context.Products.AddRange(new List<Product> {
                new Product { ProductTypeId=3, Name="ELIXIR 16152", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="acoustic_elixir_id1.jpg", Cost=1200.00},
                new Product { ProductTypeId=2, Name="DUNLOP DBS50110", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="bass_dunlop_id1.jpg", Cost=2300.00},
                new Product { ProductTypeId=1, Name="ERNIE BALL 2220", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_ernieball_id1.jpg", Cost=450.00},
                new Product { ProductTypeId=1, Name="GHS ZAKK WYLDE GBZW", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_ghs_id1.jpg", Cost=550.00},
                new Product { ProductTypeId=1, Name="DR DIMEBAG'S DBG-9/50", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_dr_id1.jpg", Cost=700.00},
                new Product { ProductTypeId=1, Name="D'ADDARIO PRO STEEL EPS520", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_daddario_id1.jpg", Cost=480.00},
                new Product { ProductTypeId=1, Name="LA BELLA SIX HRS-R", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_labella_id1.jpg", Cost=510.00},
                new Product { ProductTypeId=1, Name="FORTE MY AUTUMN MA61162", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_forte_id1.png", Cost=420.00},
                new Product { ProductTypeId=1, Name="KERLY KXW-1046", Description="Подробное описание струн", ShortDescription="Краткое описание струн", ImageLink="electric_kerly_id1.jpg", Cost=540.00}
            });

            context.SaveChanges();
        }
    }
}
