using System;
using System.Collections.Generic;
using System.Linq;

namespace Market
{
    public class Shop
    {
        ManageShowcase manage;
        List<Showcase> showcase;

        enum Message { DEL = 1, EXIT, ADD, SUCC, NEW, F1, LIMIT, MISTAKE }

        public void Start()
        {
            manage = new ManageShowcase();

            manage.CreateShowcase();

            showcase = manage.LoadShowcase();

            ShowTitle();

            Menu(firstMenu: true);
        }

        private void Menu(bool firstMenu, int firstIterator = 0)
        {
            int lastIterator = 0;

            bool exit = false;

            int limit;

            ShowInfo(firstIterator, firstMenu, lastIterator, false);

            do
            {
                limit = GetLimit(firstMenu, firstIterator);

                if (Console.KeyAvailable)
                {
                    if (firstMenu)
                        Iterator(ref firstIterator, limit, firstMenu, ref exit);
                    else
                        Iterator(ref lastIterator, limit, firstMenu, ref exit, firstIterator);

                    ShowInfo(firstIterator, firstMenu, lastIterator, false);                    
                }

                ShowItem(GetStr(firstMenu, firstIterator), firstMenu, firstMenu ? firstIterator : lastIterator, true);

                ShowInfo(firstIterator, firstMenu, lastIterator, true);

            } while (!exit);

            ShowItem(GetStr(firstMenu, firstIterator), firstMenu, firstMenu ? firstIterator : lastIterator, print: false);            

            firstMenu = true;
        }        

        private void Iterator(ref int iterator, int limit, bool firstMenu, ref bool exit, int firstIt = 0)
        {            
            var key = Console.ReadKey(true).Key;

            //navigation
            if (key == ConsoleKey.UpArrow && iterator > 0)
            {
                iterator--;
            }
            if (key == ConsoleKey.DownArrow && iterator < limit - 1)
            {
                iterator++;
            }
            if (key == ConsoleKey.RightArrow && firstMenu)
            {
                Menu(firstMenu: false, iterator);
            }
            if (key == ConsoleKey.LeftArrow && !firstMenu)
            {
                exit = true;
            }
            if (key == ConsoleKey.F1)
            {
                ShowMessage(true, iterator, firstMenu, 0, Message.F1);

                if (Answer())
                    ShowMessage(false, iterator, firstMenu, 0, Message.F1);                
                else
                    ShowMessage(false, iterator, firstMenu, 0, Message.F1);                
            }
            if (key == ConsoleKey.Escape)
            {
                if (firstMenu)
                {
                    ShowMessage(true, iterator, firstMenu, 0, Message.EXIT);

                    if (Answer())
                    {
                        exit = true;
                        ShowMessage(false, iterator, firstMenu, 0, Message.EXIT);
                    }
                    else ShowMessage(false, iterator, firstMenu, 0, Message.EXIT);
                }
                else
                {
                    exit = true;
                }
            }

            //control
            if (key == ConsoleKey.Add)
            {
                if (manage.Add(firstMenu, firstIt))
                {
                    ShowMessage(true, iterator, firstMenu, 0, Message.NEW);
                    Answer();
                    ShowMessage(false, iterator, firstMenu, 0, Message.NEW);
                }
                else
                {
                    ShowMessage(true, iterator, firstMenu, 0, Message.NEW);
                    Answer();
                    ShowMessage(false, iterator, firstMenu, 0, Message.NEW);
                }
            }

            if (key == ConsoleKey.Delete)
            {
                ShowMessage(true, firstIt, firstMenu, 0, Message.DEL);

                if (Answer())
                {
                    ShowMessage(false, iterator, firstMenu, iterator, Message.DEL);

                    ShowItem(GetStr(firstMenu, iterator), firstMenu, 0, false);

                    if (manage.Delete(firstMenu, firstIt, iterator))
                    {
                        ShowItem(GetStr(firstMenu, iterator), firstMenu, 0, true);
                        ShowMessage(true, iterator, firstMenu, 0, Message.SUCC);
                        Answer();
                        ShowMessage(false, iterator, firstMenu, 0, Message.SUCC);
                        iterator = 0;
                    }
                    else
                    {
                        ShowMessage(true, iterator, firstMenu, 0, Message.MISTAKE);
                        Answer();
                        ShowMessage(false, iterator, firstMenu, 0, Message.MISTAKE);
                    }
                }
                else
                {
                    ShowMessage(print: false, iterator, firstMenu, 0, Message.DEL);
                }
            }

            if (key == ConsoleKey.Enter)
            {
                Edit(ref iterator, limit, firstMenu, ref exit, firstIt = 0);
            }
        }

        private void Edit(ref int iterator, int limit, bool firstMenu, ref bool exit, int firstIt = 0)
        {
            int itr3 = 0;

            ShowListEdit(ref iterator, limit, firstMenu, ref exit, firstIt = 0, print:true, itr3);

            do
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.UpArrow && itr3 > 0)
                    {
                        itr3--;
                    }
                    if (key == ConsoleKey.DownArrow && itr3 < (firstMenu ? 1:3))
                    {
                        itr3++;
                    }
                    if (key == ConsoleKey.Enter)
                    {
                        ShowEditItem(ref iterator, limit, firstMenu, ref exit, firstIt, true, itr3);
                        ShowEditItem(ref iterator, limit, firstMenu, ref exit, firstIt, false, itr3);
                    }
                    if (key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    ShowListEdit(ref iterator, limit, firstMenu, ref exit, firstIt = 0, true, itr3);
                }

            } while (true);

            ShowListEdit(ref iterator, limit, firstMenu, ref exit, firstIt = 0, print: false, itr3);
        }

        private void ShowEditItem(ref int iterator, int limit, bool firstMenu, ref bool exit, int firstIt = 0, bool print = true, int itr3 = 0, string str = "")
        {
            Console.BackgroundColor = print? ConsoleColor.Blue: ConsoleColor.Black;
            Console.SetCursorPosition(46, 10 + itr3);
            string[] name = { "Name", "Volume", "Price", "Quantity" };
            Console.Write(print ? $"New {name[itr3]}:           " : "                                         ");
            Console.SetCursorPosition(57, 10 + itr3);
            if (print)
            {
                str = Console.ReadLine();
            }            
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void SendRespondEditItem()
        {

        }

        private void ShowListEdit(ref int iterator, int limit, bool firstMenu, ref bool exit, int firstIt = 0, bool print = true, int iter3 = 0)
        {
            Console.SetCursorPosition(28, 8);
            Console.WriteLine(print?"-----------------------------------------": "                                           ");
            Console.SetCursorPosition(28, 9);
            Console.WriteLine(print ? "|                Edit                   |": "                                         ");
            int count = 0;
            ConsoleColor color = ConsoleColor.Green;
            string[] name = { "Name", "Volume", "Price", "Quantity" };
            foreach (var item in GetListEditStr(firstMenu, iterator, firstIt))
            {
                Console.ForegroundColor = iter3 == count ? color : ConsoleColor.White;

                Console.SetCursorPosition(28, count + 10);

                Console.WriteLine(print ? $"|     {name[count]}:  " : "                           ");
                Console.SetCursorPosition(45, count + 10);
                Console.Write(print ? $" {item}                           " : "                            ");
                Console.SetCursorPosition(68, count + 10);
                Console.WriteLine("|");
                count++;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(28, count + 10);
            Console.WriteLine(print?"-----------------------------------------": "                                         ");
            
        }

        private List<string> GetListEditStr(bool firstMenu, int i, int i2)
        {
            if (firstMenu)
                return new List<string> { showcase[i].Name, showcase[i].Volume.ToString(), };
            else
                return new List<string> { showcase[i2].Products[i].Name, showcase[i2].Products[i].Volume.ToString(), showcase[i2].Products[i].Price.ToString(), showcase[i2].Products[i].Quantity.ToString() };
        }

        private int GetLimit(bool firstMenu, int i)
        {
            if (firstMenu)
                return showcase.Count;
            else
                return showcase[i].Products.Count;
        }

        private List<string> GetStr(bool firstMenu, int i)
        {
            if (firstMenu)
                return showcase.Select(x => x.Id.ToString() + ". " + x.Name).ToList();
            else
                return showcase[i].Products.Select(x => x.Id.ToString() + ". " + x.Name).ToList();
        }

        private bool Answer()
        {
            do
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.Enter)
                    {
                        return true;
                    }
                    if (key == ConsoleKey.Escape)
                    {
                        return false;
                    }
                }

            } while (true);
        }

        private void ShowItem(List<string> list, bool firstMenu, int i, bool print)
        {
            int count = 0;

            Console.SetCursorPosition(10, 9);

            ConsoleColor color = ConsoleColor.Green;

            foreach (var item in list)
            {
                Console.ForegroundColor = i == count ? color: ConsoleColor.White;

                Console.SetCursorPosition(firstMenu ? 10 : 32, count + 9);

                Console.WriteLine(print ? item: "                    ");

                count++;
            }
            if (list.Count < 1)
            {
                Console.SetCursorPosition(firstMenu ? 10 : 32, count + 9);
                Console.WriteLine(print ? "........." : "             ");
            }
        }
        
        private void ShowTitle()
        {
            Console.CursorVisible = false;

            //Шапка
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\t\t\t\t#################");
            Console.Write("\t\t\t\t## ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("PYATEROCHKA");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" ##");
            Console.WriteLine("\t\t\t\t#################\n\n\n");
            Console.ResetColor();

            //Список
            Print(10, "ВИТРИНЫ:", false);
            Print(32, "ПРОДУКТЫ:", false);
            Print(60, "ИНФОРМАЦИЯ:");
            Print(10, "Id Name", false, ConsoleColor.White);
            Print(32, "Id Name", true, ConsoleColor.White);
            Print(10, "       ", true);

            //Подсказка
            Console.SetCursorPosition(0, 25);
            Print(0, "========================================================================================================================");
            Print(2, "← ↑ → ↓   - навигация              Enter - редактировать элемент           Escape - выход");
            Print(2, "Delete  - удалить элемент        +      - добавить элемент                F1     - справка");
        }

        private void Print(int x, string str, bool ent = true, ConsoleColor color = ConsoleColor.DarkGray)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, Console.CursorTop);
            Console.Write(str + (ent ? "\n" : ""));
        }

        private void ShowMessage(bool print, int iterator, bool firstMenu, int y, Message message)
        {
            Console.SetCursorPosition(30, 9);
            Print(30, print ? "-----------------------------------------------" : "                                               ");
            Print(30, print ? "|                                             |" : "                                               ");
            Print(30, print ? "|" : "  ", false);
            switch ((int)message)
            {
                case 1:
                    
                    
                    Print(Console.CursorLeft, print ? $"   DELETE {(firstMenu ? showcase[iterator].Name : showcase[iterator].Products[y].Name)}? " : "                        ", false, ConsoleColor.Red);
                    Print(Console.CursorLeft, print ? "(Ent / Esc)        " : "                           ", ent: false);
                    break;
                case 2:
                    Print(Console.CursorLeft, print ? "            EXIT? " : "                ", false, ConsoleColor.Red);
                    Print(Console.CursorLeft, print ? "(Ent / Esc)               " : "             ", false);
                    break;
                case 3:
                    Print(Console.CursorLeft, print ? $"   Add a new  {(firstMenu ? "Showcase" : "Product")}? " : "                        ", false, ConsoleColor.Red);
                    Print(Console.CursorLeft, print ? "(Ent / Esc)        " : "                           ", ent: false);
                    break;
                case 4:
                    Print(Console.CursorLeft, print ? $"                 Success! " : "                                 ", ent: false, ConsoleColor.Red);
                    break;
                case 5:
                    Print(Console.CursorLeft, print ? $"            New  {(firstMenu ? "Showcase" : "Product")} is added             " : "                                      ", false, ConsoleColor.Red);
                    break;
                case 6:
                    Print(30, print ? "|                  Справка                    |" : "                                       ");
                    Print(30, print ? "|                                             |" : "                                       ", false);
                    break;
                case 7:
                    Print(Console.CursorLeft, print ? $" Нельзя добавить {(firstMenu ? "Showcase" : "Product")}, превышен лимит " : "                                                        ", false, ConsoleColor.Red);
                    break;
                case 8:
                    Print(30, print ? "|                  Ошибка                     |" : "                                        ");
                    Print(30, print ? "|                                             |" : "                                        ", false);
                    break;
                default:
                    break;
            }

            Print(75, print ? " |" : "    ");
            Print(30, print ? "|                                             |" : "                                               ");
            Print(30, print ? "-----------------------------------------------" : "                                               ");
        }

        private void ShowInfo(int iterator, bool firstMenu = true, int y = 0, bool print = true)
        {
            if (showcase[iterator].Products.Count > 0)
            {
                Console.CursorTop = 9;
                if (!print)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Print(60, "                                 ");
                    }
                }

                Console.CursorTop = 9;
                Print(60, "Name: ", false);
                Print(Console.CursorLeft, firstMenu ? showcase[iterator].Name : showcase[iterator].Products[y].Name, true, ConsoleColor.White);
                Print(60, "Id: ", false);
                Print(Console.CursorLeft, (firstMenu ? showcase[iterator].Id : showcase[iterator].Products[y].Id).ToString(), true, ConsoleColor.White);
                Print(60, "Volume: ", false);
                Print(Console.CursorLeft, (firstMenu ? showcase[iterator].Volume : showcase[iterator].Products[y].Volume).ToString(), true, ConsoleColor.White);
                Print(60, "Create date: ", false);
                Print(Console.CursorLeft, firstMenu ? showcase[iterator].DateCreate.ToString() : "", true, ConsoleColor.White);
                Print(60, "Price: ", false);
                Print(Console.CursorLeft, firstMenu ? "" : showcase[iterator].Products[y].Price.ToString(), true, ConsoleColor.White);
                Print(60, "Quantity: ", false);
                Print(Console.CursorLeft, firstMenu ? "" : showcase[iterator].Products[y].Quantity.ToString(), true, ConsoleColor.White);
            }
        }        
    }
}