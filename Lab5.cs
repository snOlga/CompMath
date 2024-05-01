public class Lab5
{
    static List<List<decimal>> table = new List<List<decimal>>(); //first is x
    static decimal mainX = 0;
    public static void doAlgos()
    {
        Console.WriteLine("Amount of points: ");
        int amount = int.Parse(Console.ReadLine());
        Console.WriteLine("Write x,y in format [x y]");
        for (int i = 0; i < amount; i++)
        {
            string line = Console.ReadLine();
            List<decimal> buffer = new List<decimal>();
            buffer.Add(decimal.Parse(line.Split(" ")[0]));
            buffer.Add(decimal.Parse(line.Split(" ")[1]));
            table.Add(buffer);
        }
        getDifTable();

        Console.WriteLine("Write x:");
        mainX = decimal.Parse(Console.ReadLine());

        LagrangePolynomial();
        NewtonInterpolation();
        GaussInterpolation();
    }

    static void getDifTable()
    {
        table.Sort((list1, list2) => (list1[0].CompareTo(list2[0])));
        for (int i = 2; i <= table.Count; i++)
        {
            for (int j = 0; j < table.Count - i + 1; j++)
            {
                table[j].Add(table[j + 1][i - 1] - table[j][i - 1]);
            }
        }

        foreach (var item in table)
        {
            Console.WriteLine(String.Join(" ", item));
        }
    }

    static void LagrangePolynomial()
    {
        decimal result = 0;

        for (int i = 0; i < table.Count - 1; i++)
        {
            decimal down = 1;
            decimal up = 1;
            for (int j = 0; j < table.Count - 1; j++)
            {
                if (i != j)
                {
                    up *= (mainX - table[j][0]);
                    down *= (table[i][0] - table[j][0]);
                }
            }
            decimal a = up / down * table[i][1];
            result += (up / down * table[i][1]);
        }
        Console.WriteLine("LagrangePolynomial: " + result);
    }

    static decimal NewtonInterpolation()
    {
        decimal result = 0;

        List<int> rememberIndexes = new List<int>();
        rememberIndexes.Add(0);

        for (int i = 1; i < table.Count - 1; i++)
        {
            decimal part = 1;
            for (int j = 0; j < i; j++)
            {
                part *= (mainX - table[j][0]);
            }

            rememberIndexes.Add(i);

            result += (FuncInNewton(rememberIndexes) * part);
        }
        result += table[0][1];

        Console.WriteLine("NewtonInterpolation: " + result);
        return result;
    }

    static decimal FuncInNewton(List<int> indexes)
    {
        decimal func1 = 0;
        decimal func2 = 0;
        if (indexes.Count != 2)
        {
            func1 = FuncInNewton(indexes[0..(indexes.Count - 1)]);
            func2 = FuncInNewton(indexes[1..(indexes.Count)]);
        }
        else
        {
            func1 = table[indexes[indexes.Count - 1]][1];
            func2 = table[indexes[0]][1];
        }

        decimal result = (func1 - func2) / (table[indexes[indexes.Count - 1]][0] - table[indexes[0]][0]);
        return result;
    }


    static decimal GaussInterpolation()
    {
        if (table.Count % 2 == 0)
        {
            Console.WriteLine("Can't calculate GaussInterpolation :(");
            Console.WriteLine("Count of values should be odd!");
            return 0;
        }

        decimal h = table[1][0] - table[0][0];

        for (int i = 1; i < table.Count - 1; i++)
        {
            if (table[i][0] - table[i - 1][0] != h)
            {
                Console.WriteLine("Can't calculate GaussInterpolation :(");
                Console.WriteLine("x's should be equidistant!");
            }
        }

        int centerIndex = table.Count / 2;

        decimal t = (mainX - table[centerIndex][0]) / h;

        decimal result = 0;
        result += table[centerIndex][1];

        int indexForDelta = 2;

        if (mainX < table[centerIndex][0])
        {
            for (int i = centerIndex - 1; i >= 0; i--)
            {
                int currentNfromFormula = Math.Abs(i - centerIndex);
                decimal up1 = 1;
                decimal up2 = 1;
                for (int j = 0; j < currentNfromFormula; j++)
                {
                    up1 *= (t - j) * up2;
                    up2 *= up1 * (t + j + 1);
                }


                decimal bigFraction1 = up1 / Factorial(2 * currentNfromFormula - 1) * table[i][indexForDelta];
                indexForDelta++;
                decimal bigFraction2 = up2 / Factorial(2 * currentNfromFormula) * (t + currentNfromFormula) * table[i][indexForDelta];
                indexForDelta++;

                result += (bigFraction1 + bigFraction2);
            }
        }
        else
        {
            for (int i = centerIndex; i > 0; i--)
            {
                int currentNfromFormula = Math.Abs(i - centerIndex);
                decimal up1 = 1;
                decimal up2 = 1;
                for (int j = 0; j <= currentNfromFormula; j++)
                {
                    up1 *= (t - j) * up2;
                    up2 *= up1 * (t + j);
                }


                decimal bigFraction1 = up1 / Factorial(2 * currentNfromFormula - 1) * table[i][indexForDelta];
                indexForDelta++;
                decimal bigFraction2 = 0;
                if (indexForDelta != table[i].Count)
                    bigFraction2 = up2 / Factorial(2 * currentNfromFormula) * table[i][indexForDelta];
                indexForDelta++;

                result += (bigFraction1 + bigFraction2);
            }
        }

        Console.WriteLine("GaussInterpolation: " + result);
        return result;
    }

    static int Factorial(int number)
    {
        if (number <= 1)
            return 1;
        else
            return number * Factorial(number - 1);
    }
}