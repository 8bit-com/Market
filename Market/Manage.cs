using System;
using System.Collections.Generic;
using System.Linq;

namespace Market
{
    public class ManageShowcase
    {
        private List<Showcase> showcas;

        private int _showcaseLimit = 10;

        private int IdShowCount = 100; 
        private int IdProdCount = 100;

        public  void CreateShowcase()
        {
            showcas =  new List<Showcase>();

            //тестовые объекты
            {
                showcas.Add(new Showcase(name: "NewShowcase1", volume: 30, id: 1));
                showcas.Add(new Showcase(name: "NewShowcase2", volume: 30, id: 2));
                showcas.Add(new Showcase(name: "NewShowcase3", volume: 10, id: 3));
                showcas[0].Products.Add(new ProductOnDisplay(name: "Milk", volume: 1, id: 11, price: 1, quantity: 2));
                showcas[0].Products.Add(new ProductOnDisplay(name: "Bread", volume: 1, id: 12, price: 1, quantity: 4));
                showcas[0].Products.Add(new ProductOnDisplay(name: "Snikers", volume: 1, id: 13, price: 1, quantity: 1));
                showcas[2].Products.Add(new ProductOnDisplay(name: "Yogurt", volume: 1, id: 14, price: 1, quantity: 7));
                showcas[1].Products.Add(new ProductOnDisplay(name: "Ice Cream", volume: 1, id: 15, price: 1, quantity: 5));
            }
        }

        public List<Showcase> LoadShowcase()
        {
            return showcas;
        }

        public bool Add(bool firstMenu, int i, int sumVolume = 0)
        {           
            sumVolume = showcas[i].Products.Select(x => x.Volume).Sum() * showcas[i].Products.Select(x => x.Quantity).Sum();

            if (firstMenu)
            {
                if (showcas.Count < _showcaseLimit)
                {
                    showcas.Add(new Showcase("NewShowcase!", 20, GetId(firstMenu)));
                    return true;
                }
                return false;
            }
            else
            {
                if (sumVolume < showcas[i].Volume)
                {
                    showcas[i].Products.Add(new ProductOnDisplay("NewProduct", 1, GetId(firstMenu), 1, 1));
                    return true;
                }
                return false;
            }
        }

        private int GetId(bool firstMenu)
        {
            if (firstMenu)
            {
                IdShowCount++;
                return IdShowCount;
            }
            else
            {
                IdProdCount++;
                return IdProdCount;
            }
            
        }

        public bool Edit(bool firstMenu, int Iter, int lastIter, int itr3, string str)
        {
            if (firstMenu)
            {
                switch (itr3)
                {
                    case 0:
                        showcas[Iter].Name = str;
                        return true;
                    case 1:
                        showcas[Iter].Volume = Int32.Parse(str);
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                switch (itr3)
                {
                    case 0:
                        showcas[lastIter].Products[Iter].Name = str;
                        return true;
                    case 1:
                        if (CheckVolume(lastIter, str, false, 0))
                        {
                            showcas[lastIter].Products[Iter].Volume = Int32.Parse(str);
                            return true;
                        }
                        else return false;
                        
                    case 2:
                        showcas[lastIter].Products[Iter].Price = Int32.Parse(str);
                        return true;
                    case 3:
                        if (CheckVolume(lastIter, str, true, Iter))
                        {
                            showcas[lastIter].Products[Iter].Quantity = Int32.Parse(str);
                            return true;
                        }
                        else return false;
                        
                    default:
                        return false;
                }
            }
        }

        private bool CheckVolume(int i, string str, bool qwuant, int i2)
        {
            int sumVolume = 0;

            if (qwuant)
            {
                sumVolume = showcas[i].Products.Select(x => x.Volume * x.Quantity).Sum() - (showcas[i].Products[i2].Volume * showcas[i].Products[i2].Quantity) + (showcas[i].Products[i2].Volume * Int32.Parse(str));
            }
            else
            {
                sumVolume = showcas[i].Products.Select(x => x.Volume * x.Quantity).Sum() + Int32.Parse(str);
            }
            

            if (sumVolume < showcas[i].Volume)
            {
                return true;
            }
            return false;
        }

        public bool Delete(bool firstMenu, int firstIter, int lastIter)
        {
            if (firstMenu)
            {
                if (showcas[lastIter].Products.Count == 0)
                {
                    showcas.RemoveAt(lastIter);
                    return true;
                }
                else return false;
            }
            else
            {
                if (showcas[lastIter].Products.Count > 0)
                {
                    showcas[lastIter].Products.RemoveAt(firstIter);
                    return true;
                }
                else return false;
            }
        }
    }
}