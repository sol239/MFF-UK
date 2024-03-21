namespace hw1_Prime_Factorization;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // reading input
        string input = Console.ReadLine();
        int number = int.Parse(input);
        
        // uncomment for the 2nd variation of the homework
        //Console.Write($"{number}=");
        
        // prime factorization sequentually
        List<int> ResultList = new List<int>();
        
        while (number != 1)
        {

            // even number sieve
            while (number % 2 == 0)
            {
                ResultList.Add(2);
                number /= 2;
            }
            
            // sieve for odd numbers
            int NextDivider = 3;

            while (number != 1)
            {
                if (number % NextDivider == 0)
                {
                    ResultList.Add(NextDivider);
                    number /= NextDivider;
                }
                else
                {
                    // if the number is not divisable by the divider, moves by +2 to another odd number
                    NextDivider += 2;
                }

            }
        }
        
        // replace " " for "*" for the 2nd variation of the homework
        Console.WriteLine(string.Join(" ", ResultList));

    }
}