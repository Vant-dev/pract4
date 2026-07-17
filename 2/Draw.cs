using System;

class Program
{
    public static void Draw(int N)
    {
       
        if (N % 2 == 0 || N <= 0)
        {
            Console.WriteLine("N должно быть нечетным положительным числом");
            return;
        }

        int center = N / 2; 

     
        for (int row = 0; row <= center; row++)
        {
            for (int col = 0; col < N; col++)
            {

                if ((col == center - row || col == center + row) && row != 0)
                {
                    Console.Write("X");
                }
                else if (row == 0 && col == center) 
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }

        for (int row = center - 1; row >= 0; row--)
        {
            for (int col = 0; col < N; col++)
            {
                if (row == 0 && col == center) 
                {
                    Console.Write("X");
                }
                else if (col == center - row || col == center + row)
                {
                    Console.Write("X");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    static void Main()
    {
        Draw(5);
    }
}
