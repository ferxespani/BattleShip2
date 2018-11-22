using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace BattleShip2
{
    class Program
    {
        struct Point
        {
            public int u;
            public int y;
            public Point Set(int X, int Y)
            {
                u = X;
                y = Y;
                return this;
            }
        }

        static void Main(string[] args)
        {
            const string filePath = "map.txt";

            string[,] playerMap = ReadFromFile(filePath);
            string[,] computerMap = new string[12, 12];
            string[,] playerRadar = new string[12, 12];
            string[,] computerRadar = new string[12, 12];

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    playerMap[i, j] = "0";
                }
            }

            playerMap = ReadFromFile(filePath);

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    computerMap[i, j] = "0";
                }
            }

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    playerRadar[i, j] = "0";
                }
            }

            int sizeShip, numberShips, row, column, direction;
            bool isPossibleToPut;
            Random r = new Random();
            for (sizeShip = 1; sizeShip < 5; sizeShip++)
            {
                for (numberShips = 1; numberShips < 6 - sizeShip; numberShips++)
                {
                    do
                    {
                        row = r.Next(1, 11);
                        column = r.Next(1, 11);
                        direction = r.Next(2);
                        isPossibleToPut = IsPossibleToPutShip(computerMap, row, column, direction, sizeShip);
                        if (isPossibleToPut == true)
                            ToPutShip(computerMap, row, column, direction, sizeShip);
                    } while (isPossibleToPut != true);
                }
            }

            Display1(playerMap);
            Display1(playerRadar);

            Play(playerRadar, computerMap, playerMap, computerRadar);

        }

        static void Check(int sizeOfShip, string[,] radar, string[,] map, int x, int y)
        {
            if (map[x, y - 1] == sizeOfShip.ToString() || radar[x, y - 1] == sizeOfShip.ToString() || sizeOfShip == 1)
            {
                for (int k = x - 1; k < x + 2; k++)
                {
                    for (int r = y - sizeOfShip; r < y + 2; r++)
                    {
                        if (k > 0 && k < 11 && r > 0 && r < 11)
                        {
                            map[k, r] = "X";
                            radar[k, r] = "X";
                        }
                    }
                }
            }
            else if (map[x, y + 1] == sizeOfShip.ToString() || radar[x, y + 1] == sizeOfShip.ToString())
            {
                for (int k = x - 1; k < x + 2; k++)
                {
                    for (int r = y - 1; r < y + sizeOfShip + 1; r++)
                    {
                        if (k > 0 && k < 11 && r > 0 && r < 11)
                        {
                            map[k, r] = "X";
                            radar[k, r] = "X";
                        }
                    }
                }
            }
            else if (map[x + 1, y] == sizeOfShip.ToString() || radar[x + 1, y] == sizeOfShip.ToString())
            {
                for (int k = x - 1; k < x + sizeOfShip + 1; k++)
                {
                    for (int r = y - 1; r < y + 2; r++)
                    {
                        if (k > 0 && k < 11 && r > 0 && r < 11)
                        {
                            map[k, r] = "X";
                            radar[k, r] = "X";
                        }
                    }
                }
            }
            else
            {
                for (int k = x - sizeOfShip; k < x + 2; k++)
                {
                    for (int r = y - 1; r < y + 2; r++)
                    {
                        if (k > 0 && k < 11 && r > 0 && r < 11)
                        {
                            map[k, r] = "X";
                            radar[k, r] = "X";
                        }
                    }
                }
            }
        }

        static void Play(string[,] playerRadar, string[,] computerMap, string[,] playerMap, string[,] computerRadar)
        {
            string storage = "";
            int numberShips, numberShips1;
            numberShips = numberShips1 = 10;
            int count1, count2, count3, count4, count5, count6, iteration, x1, y1;
            count1 = count2 = count3 = count4 = count5 = count6 = iteration = x1 = y1 = 0;

            Random r = new Random();

            List<Point> lst = new List<Point>();
            Point point = new Point();

            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    lst.Add(point.Set(i, j));
                }
            }

            do
            {
                int x, y;
                try
                {
                    WriteAt("Enter x: ", 50, 0);
                    x = int.Parse(Console.ReadLine());
                    WriteAt("Enter y: ", 50, 2);
                    y = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Clear();
                    Display1(playerMap);
                    Display1(playerRadar);
                    continue;
                }

                for (int i = 1; i < 11; i++)
                {
                    if (playerRadar[x, y] == "X" || playerRadar[x, y] == "*")
                        break;
                    for (int j = 1; j < 11; j++)
                    {
                        string sizeofShip1 = computerMap[x, y];
                        if (computerMap[x, y] != "0")
                        {
                            playerRadar[x, y] = "*";
                            WriteAt("", 50, 4);
                            Console.Beep(247, 500);
                            Console.Beep(417, 500);
                            Console.Beep(417, 500);
                            if (sizeofShip1 == "1")
                            {
                                Check(int.Parse(sizeofShip1), playerRadar, computerMap, x, y);
                                --numberShips;
                                break;
                            }
                            else if (sizeofShip1 == "2")
                            {
                                ++count1;
                                if (count1 < 2)
                                    break;
                                Check(int.Parse(sizeofShip1), playerRadar, computerMap, x, y);
                                --numberShips;
                                count1 = 0;
                                break;
                            }

                            else if (sizeofShip1 == "3")
                            {
                                ++count2;
                                if (count2 < 3)
                                    break;
                                Check(int.Parse(sizeofShip1), playerRadar, computerMap, x, y);
                                --numberShips;
                                count2 = 0;
                                break;
                            }
                            else if (sizeofShip1 == "4")
                            {
                                ++count3;
                                if (count3 < 4)
                                    break;
                                Check(int.Parse(sizeofShip1), playerRadar, computerMap, x, y);
                                --numberShips;
                                count3 = 0;
                                break;
                            }
                        }
                        else
                        {
                            playerRadar[x, y] = "X";
                            break;
                        }
                    }
                }

                WriteAt("Computer's turn...", 50, 4);


                Point pnt = new Point();
                int randPoint;
                if (iteration == 0)
                {
                    randPoint = r.Next(0, lst.Count);
                    pnt = lst[randPoint];
                    lst.RemoveAt(randPoint);
                    x1 = pnt.u;
                    y1 = pnt.y;
                }
                else
                {
                    string sizeOfShip = storage;
                    if (sizeOfShip == "2")
                    {
                        if (playerMap[x1, y1 + 1] == sizeOfShip && count4 != 2)
                        {
                            ++y1;
                        }
                        else if (playerMap[x1, y1 - 1] == sizeOfShip && count4 != 2)
                        {
                            --y1;
                        }
                        else if (playerMap[x1 + 1, y1] == sizeOfShip && count4 != 2)
                        {
                            ++x1;
                        }
                        else if (playerMap[x1 - 1, y1] == sizeOfShip && count4 != 2)
                        {
                            --x1;
                        }
                    }
                    else if (sizeOfShip == "3")
                    {
                        if (playerMap[x1, y1 + 1] == sizeOfShip && playerMap[x1, y1 - 1] == sizeOfShip && count5 != 2)
                        {
                            ++y1;
                        }
                        else if (playerMap[x1 + 1, y1] == sizeOfShip && playerMap[x1 - 1, y1 - 1] == sizeOfShip && count5 != 2)
                        {
                            ++x1;
                        }
                        else if (playerMap[x1, y1 + 1] == sizeOfShip && count5 != 3)
                        {
                            ++y1;
                        }
                        else if ((playerMap[x1, y1 - 1] == sizeOfShip || playerMap[x1, y1 - 1] == "*") && count5 != 3)
                        {
                            --y1;
                            if (computerRadar[x1, y1 - 1] == sizeOfShip)
                                --y1;
                        }
                        else if (playerMap[x1 + 1, y1] == sizeOfShip && count5 != 3)
                        {
                            ++x1;
                        }
                        else if ((playerMap[x1 - 1, y1] == sizeOfShip || playerMap[x1 - 1, y1] == "*") && count5 != 3)
                        {
                            --x1;
                            if (computerRadar[x1 - 1, y1] == sizeOfShip)
                                --x1;
                        }
                    }
                    else if (sizeOfShip == "4")
                    {
                        if (playerMap[x1, y1 + 1] == sizeOfShip && playerMap[x1, y1 - 1] == sizeOfShip && count6 != 3)
                        {
                            ++y1;
                        }
                        else if (playerMap[x1 + 1, y1] == sizeOfShip && playerMap[x1 - 1, y1] == sizeOfShip && count6 != 3)
                        {
                            ++x1;
                        }
                        else if (playerMap[x1, y1 + 1] == sizeOfShip && count6 != 4)
                        {
                            ++y1;
                        }
                        else if ((playerMap[x1, y1 - 1] == sizeOfShip || playerMap[x1, y1 - 1] == "*") && count6 != 4)
                        {
                            --y1;
                            if (computerRadar[x1, y1 - 1] == "*" || computerRadar[x1, y1 - 1] == sizeOfShip)
                                y1 -= 2;
                        }
                        else if (playerMap[x1 + 1, y1] == sizeOfShip && count6 != 4)
                        {
                            ++x1;
                        }
                        else if ((playerMap[x1 - 1, y1] == sizeOfShip || playerMap[x1 - 1, y1 - 1] == "*") && count6 != 4)
                        {
                            --x1;
                            if (computerRadar[x1, y1 - 1] == "*" || computerRadar[x1, y1 - 1] == sizeOfShip)
                                x1 -= 2;
                        }
                    }
                    else
                    {
                        randPoint = r.Next(0, lst.Count);
                        pnt = lst[randPoint];
                        lst.RemoveAt(randPoint);
                        x1 = pnt.u;
                        y1 = pnt.y;
                    }
                }

                string sizeofShip = "";

                for (int i = 1; i < 11; i++)
                {
                    if (computerRadar[x1, y1] == "X" || computerRadar[x1, y1] == sizeofShip)
                        break;
                    for (int j = 1; j < 11; j++)
                    {
                        sizeofShip = playerMap[x1, y1];
                        if (playerMap[x1, y1] != "0")
                        {
                            storage = playerMap[x1, y1];
                            computerRadar[x1, y1] = playerMap[x1, y1];
                            playerMap[x1, y1] = "*";
                            ++iteration;
                            if (sizeofShip == "1")
                            {
                                Check(int.Parse(sizeofShip), computerRadar, playerMap, x1, y1);
                                --numberShips1;
                                storage = "";
                                break;
                            }
                            else if (sizeofShip == "2")
                            {
                                ++count4;

                                if (count4 != 2)
                                    break;
                                ++iteration;
                                Check(int.Parse(sizeofShip), computerRadar, playerMap, x1, y1);
                                --numberShips1;
                                count4 = 0;
                                storage = "";
                                break;
                            }

                            else if (sizeofShip == "3")
                            {
                                ++count5;

                                if (count5 < 3)
                                    break;
                                ++iteration;
                                Check(int.Parse(sizeofShip), computerRadar, playerMap, x1, y1);
                                --numberShips1;
                                count5 = 0;
                                storage = "";
                                break;
                            }
                            else if (sizeofShip == "4")
                            {
                                ++count6;

                                if (count6 < 4)
                                    break;
                                ++iteration;
                                Check(int.Parse(sizeofShip), computerRadar, playerMap, x1, y);
                                --numberShips1;
                                count6 = 0;
                                storage = "";
                                break;
                            }
                        }
                        else
                        {
                            computerRadar[x1, y1] = "X";
                            playerMap[x1, y1] = "X";
                            iteration = 0;
                            storage = "";
                            break;
                        }
                    }
                }

                Thread.Sleep(2000);
                Console.Clear();
                Display1(playerMap);
                Display1(playerRadar);

            } while (numberShips != 0 || numberShips1 != 0);

        }

        static void WriteAt(string s, int x, int y)
        {
            int origRow = 0;
            int origCol = 0;
            try
            {
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        static string[,] ReadFromFile(string path)
        {
            string[] arr = File.ReadAllLines(path);
            string[,] num = new string[arr.Length, arr[0].Split(' ').Length];
            for (int i = 0; i < arr.Length; i++)
            {
                string[] temp = arr[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                {
                    num[i, j] = temp[j];
                }
            }

            return num;

        }

        static bool IsPossibleToPutShip(string[,] map, int row, int column, int direction, int sizeShip)
        {
            int count = 0;

            if (direction == 0)
            {
                if (column + sizeShip > 11)
                    return false;
            }
            else
            {
                if (row + sizeShip > 11)
                    return false;
            }

            if (direction == 0)
            {
                for (int i = row - 1; i < row + 2; i++)
                {
                    for (int j = column - 1; j < column + sizeShip + 1; j++)
                    {
                        if (map[i, j] == "0")
                            ++count;
                    }
                }
            }
            else
            {
                for (int i = row - 1; i < row + sizeShip + 1; i++)
                {
                    for (int j = column - 1; j < column + 2; j++)
                    {
                        if (map[i, j] == "0")
                            ++count;
                    }
                }
            }

            if (count == 3 * (2 + sizeShip))
            {
                return true;
            }
            else
                return false;
        }

        static void ToPutShip(string[,] map, int row, int column, int direction, int sizeShip)
        {
            for (int i = 0; i < sizeShip; i++)
            {
                map[row, column] = sizeShip.ToString();
                if (direction == 0)
                    ++column;
                else
                    ++row;
            }
        }

        static void Display1(string[,] arr)
        {

            Console.WriteLine("**************************************");
            for (int i = 1; i < 11; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    Console.Write($"{arr[i, j]}   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("**************************************");
        }
    }
}

