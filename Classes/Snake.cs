using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace CSharpGame
{
    class Snake
    {
        /// <summary>
        /// Direction enum
        /// </summary>
        enum Direction { Right, Left, Up, Down };

        /// <summary>
        /// invoked when game ends
        /// </summary>
        /// <param name="points">points gathered</param>
        private void GameOver(int points)
        {
            Thread.Sleep(300);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string x in File.ReadAllLines("GAME_OVER.txt"))
            {
                Console.WriteLine(x);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Rank(points);
            DisplayRank(10);
            Enter();
        }

        /// <summary>
        /// invoked when hash sign collision detected
        /// </summary>
        /// <param name="points">points gathered</param>
        private void YouWon(int points)
        {
            Thread.Sleep(300);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (string x in File.ReadAllLines("YOU_WON.txt"))
            {
                Console.WriteLine(x);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Extra 200 pts");
            Rank(points + 200);
            DisplayRank(10);
            Enter();
        }

        /// <summary>
        /// ranks user score
        /// </summary>
        /// <param name="points">points gathered</param>
        private void Rank(int points)
        {
            RankFileMG file = new RankFileMG("rank.txt");
            if (file.IsScoreRankable(10, points)) {
                Thread.Sleep(500);
                Console.Write("Your name: ");
                string name = "";
                while (name.Length == 0)
                {
                    name = Console.ReadLine();
                }
                file.Append(name + " " + points);
                Thread.Sleep(500);
            }
            else
            {
                Thread.Sleep(1500);
            }
        }

        /// <summary>
        /// displays rank
        /// </summary>
        /// <param name="n">number of rows to show</param>
        private void DisplayRank(int n)
        {
            RankFileMG rankFile = new RankFileMG("rank.txt");
            rankFile.SortRank();
            rankFile.DisplayRank(n);
        }

        /// <summary>
        /// waits for enter key input and clear the screen
        /// </summary>
        private void Enter()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                key = Console.ReadKey(true);
            }
            Console.Clear();
        }

        /// <summary>
        /// game init and start
        /// </summary>
        private void GameStart()
        {
            try
            {
                Console.SetWindowSize(69, 30); //there were problems with this function in Wine environment
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }

            //instruction
            Console.WriteLine("CONTROLS\n");
            Console.WriteLine("WASD or arrow keys\n");
            Console.WriteLine("Press enter to start");
            Enter();

            for (int i = 0; i < Console.WindowWidth - 11; i++)
            {
                Console.Write("-");
            }
            Console.Write("POINTS: ");

            int points = 0;
            Console.SetCursorPosition(Console.WindowWidth - points.ToString().Length, 0);
            Console.Write(points.ToString());
            Console.SetCursorPosition(0, 0);

            Point exit = new Point(0, 0);
            Point head = new Point(0, 1);

            Random rnd = new Random();
            Point apple = new Point(8, 4);
            bool appleEaten = false;

            Console.SetCursorPosition(apple.X, apple.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);

            //initial snake size
            int snakeSize = 5;
            List<Point> snake = new List<Point>();

            Direction direction = Direction.Right;
            Direction lastDirection = Direction.Right;

            Console.SetCursorPosition(head.X, head.Y);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("O");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
            snake.Add(head);

            DateTime timeStart = DateTime.Now;
            DateTime timeInterval = DateTime.Now;

            timeStart = DateTime.Now;
            while (true)
            {
                //receive key input
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo c = Console.ReadKey(true);
                    if (c.Key == ConsoleKey.RightArrow || c.Key == ConsoleKey.D || c.Key == ConsoleKey.L)
                    {
                        if (direction != Direction.Left && lastDirection != Direction.Left)
                        {
                            direction = Direction.Right;
                        }
                    }
                    else if (c.Key == ConsoleKey.LeftArrow || c.Key == ConsoleKey.A || c.Key == ConsoleKey.J)
                    {
                        if (direction != Direction.Right && lastDirection != Direction.Right)
                        {
                            direction = Direction.Left;
                        }
                    }
                    else if (c.Key == ConsoleKey.UpArrow || c.Key == ConsoleKey.W || c.Key == ConsoleKey.I)
                    {
                        if (direction != Direction.Down && lastDirection != Direction.Down)
                        {
                            direction = Direction.Up;
                        }
                    }
                    else if (c.Key == ConsoleKey.DownArrow || c.Key == ConsoleKey.S || c.Key == ConsoleKey.K)
                    {
                        if (direction != Direction.Up && lastDirection != Direction.Up)
                        {
                            direction = Direction.Down;
                        }
                    }
                }


                //time interval between snake moves
                timeInterval = DateTime.Now;
                if (timeInterval.Subtract(timeStart).TotalMilliseconds > 120)
                {
                    timeStart = DateTime.Now;
                    if (snake.Count >= 1)
                    {
                        Console.SetCursorPosition(snake[snake.Count - 1].X, snake[snake.Count - 1].Y);
                        if (appleEaten)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("O");
                            appleEaten = false;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("o");
                        }
                        Console.SetCursorPosition(0, 0);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    //snake move and collision check
                    if (direction == Direction.Right)
                    {
                        head.Right();
                        head.Right();
                        if (head.X >= Console.WindowWidth)
                        {
                            GameOver(points);
                            break;
                        }
                    }
                    else if (direction == Direction.Left)
                    {
                        head.Left();
                        head.Left();
                        if (head.X < 0)
                        {
                            GameOver(points);
                            break;
                        }
                    }
                    else if (direction == Direction.Up)
                    {
                        head.Up();
                        if (head.Y < 1)
                        {
                            GameOver(points);
                            break;
                        }
                    }
                    else if (direction == Direction.Down)
                    {
                        head.Down();
                        if (head.Y >= Console.WindowHeight)
                        {
                            GameOver(points);
                            break;
                        }
                    }
                    snake.Add(head);

                    bool gameOver = false;
                    for (int i = 0; i < snake.Count - 1; i++)
                    {
                        if (head == snake[i])
                        {
                            gameOver = true;
                            break;
                        }
                    }
                    if (gameOver)
                    {
                        GameOver(points);
                        break;
                    }

                    //snake move display
                    Console.SetCursorPosition(head.X, head.Y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("O");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (snake.Count > snakeSize)
                    {
                        Console.SetCursorPosition(snake[0].X, snake[0].Y);
                        Console.Write(" ");
                        snake.RemoveAt(0);
                        Console.SetCursorPosition(0, 0);
                    }

                    //apple setup with random position
                    if (snake[snake.Count - 1] == apple)
                    {
                        points += 10;
                        Console.SetCursorPosition(Console.WindowWidth - points.ToString().Length, 0);
                        Console.Write(points.ToString());
                        Console.SetCursorPosition(0, 0);
                        appleEaten = true;
                        snakeSize++;
                        apple.X = rnd.Next(0, Console.WindowWidth);
                        apple.Y = rnd.Next(1, Console.WindowHeight);
                        bool appleCorrect = false;
                        while (!appleCorrect)
                        {
                            for (int i = 0; i < snake.Count; i++)
                            {
                                if (apple == snake[i])
                                {
                                    appleCorrect = false;
                                    apple.X = rnd.Next(0, Console.WindowWidth);
                                    apple.Y = rnd.Next(1, Console.WindowHeight);
                                    break;
                                }
                                if (apple.X % 2 != 0)
                                {
                                    apple.X++;
                                    if (exit.X > Console.WindowWidth)
                                    {
                                        exit.X -= 2;
                                    }
                                    //appleCorrect = false;
                                    //apple.X = rnd.Next(0, Console.WindowWidth);
                                    break;
                                }
                                appleCorrect = true;
                            }
                        }
                        Console.SetCursorPosition(apple.X, apple.Y);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("0");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(0, 0);
                    }

                    //check if exit
                    if (snake[snake.Count - 1] == exit && exit != new Point(0, 0))
                    {
                        YouWon(points);
                        break;
                    }

                    //exit setup with random position checked
                    if (points >= 200 && exit == new Point(0, 0))
                    {
                        exit.X = rnd.Next(0, Console.WindowWidth);
                        exit.Y = rnd.Next(1, Console.WindowHeight);
                        bool exitCorrect = false;
                        while (!exitCorrect)
                        {
                            for (int i = 0; i < snake.Count; i++)
                            {
                                if (exit == snake[i])
                                {
                                    exitCorrect = false;
                                    exit.X = rnd.Next(0, Console.WindowWidth);
                                    exit.Y = rnd.Next(1, Console.WindowHeight);
                                    break;
                                }
                                if (exit.X % 2 != 0)
                                {
                                    exit.X++;
                                    if (exit.X > Console.WindowWidth)
                                    {
                                        exit.X -= 2;
                                    }
                                    //exitCorrect = false;
                                    //exit.X = rnd.Next(0, Console.WindowWidth);
                                    break;
                                }
                                exitCorrect = true;
                            }
                        }
                        Console.SetCursorPosition(exit.X, exit.Y);
                        Console.ForegroundColor = ConsoleColor.Green;
                        char c = '#';
                        Console.Write(c);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(0, 0);
                    }

                    lastDirection = direction;
                }
            }
        }

        /// <summary>
        /// snake class constructor handling play again
        /// </summary>
        public Snake()
        {
            //play again feature
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            do
            {
                Console.Clear();
                // game starts
                GameStart();
                // game ends
                Console.Clear();
                Console.WriteLine("Press enter to play again, otherwise press ESC");
                key = Console.ReadKey(true);

            } while (key.Key != ConsoleKey.Escape);
        }





    }
}
