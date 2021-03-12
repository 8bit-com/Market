using System;
using System.Collections.Generic;

namespace Market
{
    public class Showcase : Product
    {
        public Showcase(string name, int volume, int id) : base(name, volume, id)
        {
            DateCreate = DateTime.Now;
            Products = new List<ProductOnDisplay>(); 
        }

        public List<ProductOnDisplay> Products { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateDelete { get; set; }
    }
}