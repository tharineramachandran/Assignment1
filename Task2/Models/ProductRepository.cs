using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task2.Models
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> products = new List<Product>();
        private int _nextId = 1;

        public ProductRepository()
        {
             
            Add(new Product { Name = "Nails", Category = "Hardware", Price = 1  });
            Add(new Product { Name = "Gauge", Category = "Hardware", Price = 3.75M });
            Add(new Product { Name = "Hammer", Category = "Hardware", Price = 16.99M });

            Add(new Product { Name = "Screw", Category = "Hardware", Price = 3.75M });
            Add(new Product { Name = "Pipe", Category = "Hardware", Price = 16.99M }); 
        }

        public IEnumerable<Product> GetAll()
        {
            return products;
        }

        public Product Get(int id)
        {
            return products.Find(p => p.Id == id);
        }

        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Id = _nextId++;
            products.Add(item);
            return item;
        }
         

        public Product Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = products.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                throw new ArgumentNullException("item");
            }
            products.RemoveAt(index);
            products.Add(item);
            return item;
        }

     
        public bool Remove(int id)
        {
            products.RemoveAll(p => p.Id == id);
            return true;

        }
    }

}

