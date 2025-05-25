namespace Snake
{
    public class Program
    {
        static Coord gridDimensions = new Coord(50, 25);
        static Coord direction = new Coord(1, 0);

        static List<Coord> snake = new List<Coord>();
        static Coord appleLocation = new Coord(0, 0);

        static bool gameRunning = true;
        static bool collectedApple = false;

        static int speed = 80;

        static void Main()
        {
            Console.CursorVisible = false;

            GenerateField();

            var startX = gridDimensions.X / 2;
            var startY = gridDimensions.Y / 2;
            snake.Add(new Coord(startX, startY));

            GameLoop();
        }

        static void GenerateField()
        {
            for (int y = 0; y < gridDimensions.Y; y++)
            {
                for (int x = 0; x < gridDimensions.X; x++)
                {
                    if (x == 0 || x == gridDimensions.X - 1 || y == 0 || y == gridDimensions.Y - 1)
                    {
                        Console.Write("#");
                    } else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }

        static void GameLoop()
        {
            Random rnd = new Random();

            do
            {
                appleLocation = new Coord(rnd.Next(1, gridDimensions.X - 2), rnd.Next(1, gridDimensions.Y - 2));
            } while (snake.Contains(appleLocation));
            Console.SetCursorPosition(appleLocation.X, appleLocation.Y);
            Console.Write("0");

            while (gameRunning)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    Coord newDirection = direction;

                    switch (key) 
                    {
                        case ConsoleKey.UpArrow: newDirection = new Coord(0, -1); speed = 150; break;
                        case ConsoleKey.DownArrow: newDirection = new Coord(0, 1); speed = 150; break;
                        case ConsoleKey.LeftArrow: newDirection = new Coord(-1, 0); speed = 80; break;
                        case ConsoleKey.RightArrow: newDirection = new Coord(1, 0); speed = 80; break;
                    }

                    if (newDirection.X != -direction.X || newDirection.Y != -direction.Y) 
                    {
                        direction = newDirection;
                    }

                }

                Coord head = snake[snake.Count - 1];
                Coord newHead = new(head.X + direction.X, head.Y + direction.Y);

                if (newHead.X <= 0 || newHead.X >= gridDimensions.X - 1 ||
                    newHead.Y <= 0 || newHead.Y >= gridDimensions.Y - 1)
                {
                    gameRunning = false;
                    break;
                }

                foreach (var part in snake) 
                {
                    if (newHead.Equals(part))
                    {
                        gameRunning = false;
                        break;
                    }
                }

                Console.SetCursorPosition(newHead.X, newHead.Y);
                Console.Write("■");
                snake.Add(newHead);

                if (!collectedApple)
                {
                    Coord tail = snake[0];
                    Console.SetCursorPosition(tail.X, tail.Y);
                    Console.Write(' ');
                    snake.RemoveAt(0);
                }
                else
                {
                    collectedApple = false;
                }

                if (newHead.Equals(appleLocation))
                {
                    collectedApple = true;
                }

                if (collectedApple)
                {
                    do
                    {
                        appleLocation = new Coord(rnd.Next(1, gridDimensions.X - 2), rnd.Next(1, gridDimensions.Y - 2));
                    } while (snake.Contains(appleLocation));

                    Console.SetCursorPosition(appleLocation.X, appleLocation.Y);
                    Console.Write("0");
                }

                Thread.Sleep(speed);
            }

            Console.SetCursorPosition(0, gridDimensions.Y + 1);
            Console.WriteLine("Game Over!");
        }
    }
}