using System;
using System.Collections.Generic;
using System.Diagnostics;

public delegate void Notify();

public class Game{

    public Notify changed;

    public int[,] gameArea;
    public int rows;
    public int column;

    public Game(int columns, int rows)
    {
        this.gameArea = Make2DArray(columns, rows);
        this.rows = rows;
        this.column = columns;
    }

    private int[,] Make2DArray(int columns, int rows) => new int[columns, rows];

    public void NewGeneration()
    {

        int[,] NewCopy = Make2DArray(this.column, this.rows);

        for (int i = 0; i < this.column; i++)
        {
            for (int j = 0; j < this.rows; j++)
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
    public void Clear()
    {
        this.gameArea = new int[this.column, this.rows];
    }
}
class Program
{
    static void Main(string[] args)
    {
        Game game = new Game(14 * 5, 14 * 5);
        GameView.run(game);
    }
}