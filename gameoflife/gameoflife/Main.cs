using System;
using System.Collections.Generic;

class Game
{
    int[,] gameArea;
    int rows;
    int column;
    int scale;
    public Game(int columns, int rows)
    {
        this.gameArea = Make2DArray(columns, rows);
        this.rows = rows;
        this.column = columns;
        this.scale = 1;
    }
    private int[,] Make2DArray(int columns, int rows) => new int[columns, rows];

    public void NewGeneration()
    {
        int[,] NewCopy = Make2DArray(this.column, this.rows);

        for (int i = 0; i < this.column * scale; i++)
        {
            for (int j = 0; j < this.rows * scale; j++)
            {

                int Neighbours = this.ComputeNeighbours(i, j);

                switch (this.gameArea[i, j])
                {
                    case 1:
                        if (Neighbours == 2 || Neighbours == 3) { NewCopy[i, j] = 1; }
                        break;
                    case 0:
                        if (Neighbours == 3) { NewCopy[i, j] = 1; }
                        break;
                }
            }
        }
        this.gameArea = NewCopy;
    }
    public void RandomFill()
    {
        Random r = new Random();
        for (int i = 0; i < this.column * scale; i++)
        {
            for (int j = 0; j < this.rows * scale; j++)
            {
                this.gameArea[i, j] = r.Next(2);
            }
        }
    }
    private int ComputeNeighbours(int i, int j)
    {
        int sum = 0;
        for (int column = -1; column < 2; column++)
        {
            for (int rows = -1; rows < 2; rows++)
            {

                int tempI = i + column; int tempJ = j + rows;

                if (tempI >= this.column) { tempI -= this.column; }
                else if (tempI < 0) { tempI += this.column; }

                if (tempJ >= this.rows) { tempJ -= this.rows; }
                else if (tempJ < 0) { tempJ += this.rows; }

                sum += this.gameArea[tempI, tempJ];
            }
        }
        return sum - this.gameArea[i, j];
    }
    public void Draw()
    {
        for (int i = 0; i < this.column * scale; i++)
        {
            for (int j = 0; j < this.rows * scale; j++)
            {
                Console.Write($"{this.gameArea[i, j]}|");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game(15, 15);
        game.RandomFill();
        game.Draw();
        for (int i = 0; i < 100; i++)
            game.NewGeneration();
        game.Draw();
    }
}


//public static void Main(string[] args)
//{
//    Application.Init();
//    MainWindow win = new MainWindow();
//    win.Show();
//    Application.Run();
//}