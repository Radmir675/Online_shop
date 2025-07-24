using Microsoft.EntityFrameworkCore;
using OnlineShop.db.Models;
using System;
using System.Collections.Generic;

namespace OnlineShop.db
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dagestanId = "96d166a4-0b6b-494f-ab37-03144b0e9ecb";
            var elbrusId = "490b7dba-960b-4c22-b79e-0405d8d56ccd";
            var altaiId = "e36a90de-320b-416e-8363-f7388dbb4796";
            var baikalId = "fca2c833-87c2-4c9d-9c17-ec6d1cc4e5af";
            var murmanskId = "a7049ef9-f027-4a69-970d-06d7dec5f8ce";
            var uralId = "0615471f-1269-4622-9812-b162a4097dd2";

            var DagestanImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                Link = "/lib/images/Dagestan.jpeg",
                ProductId = Guid.Parse(dagestanId)
            };
            var ElbrusImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                Link = "/lib/images/Elbrus.jpg",
                ProductId = Guid.Parse(elbrusId)
            };
            var AltaiImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                Link = "/lib/images/Altai.jpg",
                ProductId = Guid.Parse(altaiId)
            };
            var BaikalImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                Link = "/lib/images/Baikal.jpg",
                ProductId = Guid.Parse(baikalId)
            };
            var MurmanskImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                Link = "/lib/Images/Murmansk.jpg",
                ProductId = Guid.Parse(murmanskId)
            };
            var UralImage = new Image()
            {
                ImageId = Guid.NewGuid(),
                Link = "/lib/Images/Ural.jpg",
                ProductId = Guid.Parse(uralId)
            };

            List<Image> images = new List<Image>() { DagestanImage, ElbrusImage, AltaiImage, BaikalImage, MurmanskImage, UralImage };
            modelBuilder.Entity<Image>().HasData(images);

            var Dagestan = new Product()
            {
                Id = Guid.Parse(dagestanId),
                Name = "Dagestan Sulak Canyon",
                Cost = 850,
                ShortDescription = "Visiting beautiful places in mountainous Dagestan. See the flight of an eagle.",
            };

            var Elbrus = new Product()
            {
                Id = Guid.Parse(elbrusId),
                Name = "Elbrus mountain 5 642 м.",
                Cost = 2500,
                ShortDescription = "Feel the whole world at your feet!. Prove to yourself that you can do more than you think.",
            };
            var Altai = new Product()
            {
                Id = Guid.Parse(altaiId),
                Name = "Altai \"Golden Mountains\"",
                Cost = 1000,
                ShortDescription = "Find yourself and feel life. Nature could health everybody. Just do it!",
            };
            var Baikal = new Product()
            {
                Id = Guid.Parse(baikalId),
                Name = "Baikal\" Horizonless lake\"",
                Cost = 500,
                ShortDescription = "Relax on the cleanest and freshest lake. Be free!",
            };
            var Murmansk = new Product()
            {
                Id = Guid.Parse(murmanskId),
                Name = "Murmansk Сapital of the arctic",
                Cost = 630,
                ShortDescription = "Feel power of the ice! You can see real northern lights and polar bears!",

            };
            var Ural = new Product()
            {
                Id = Guid.Parse(uralId),
                Name = "Rep. Tatarstan “Grey Ural” ",
                Cost = 1100,
                ShortDescription = "Visit caves, rivers, the Urals. Try the most valuable honey!",
            };

            List<Product> products = new List<Product>() { Dagestan, Elbrus, Altai, Baikal, Murmansk, Ural };
            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}