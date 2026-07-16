using System;

public class Program
{
    public static string CalculateCompoundInterest(double initial_deposit, int years, double interest_rate)
    {
        double rate = interest_rate / 100;
        double currentAmount = initial_deposit;
        string result = "";
        
        for (int year = 1; year <= years; year++)
        {
            currentAmount *= (1 + rate);
            result += $"Год {year}: {currentAmount:F2} руб.\n";
        }
        
        return result;
    }
}