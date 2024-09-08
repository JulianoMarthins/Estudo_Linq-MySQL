using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_MySQL.Entities {
    internal class Product {

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }



        public Product(int id, string name, double price, Category category) {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
        }

        public Product() {

        }


        public override string ToString() {
            string valorFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Price);
            return $"{Id}: {Name} {valorFormatado} {Category.Name} {Category.Tier}";
        }

    }
}
