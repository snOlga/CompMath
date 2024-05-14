public class Lab6
{
    delegate double function(double x, double y);
    static double h = 0;
    static double endX = 0;
    static double epsilon;
    static List<double> allAsnwers = new List<double>();
    public static void doAlgos()
    {
        function currentFunc = variant1;
        Console.WriteLine("Choose: (write 1,2 or 3)");
        Console.WriteLine("1 -- y' = x + e^(x) - 1");
        Console.WriteLine("2 -- y' = 1 + x^2 + 2xy/(x + 1)");
        Console.WriteLine("3 -- y' = 2xy - y^2 + 5 - x^2");
        int variantValue = int.Parse(Console.ReadLine());
        switch (variantValue)
        {
            case 1: currentFunc = variant1; break;
            case 2: currentFunc = variant2; break;
            case 3: currentFunc = variant3; break;
        }
        Console.WriteLine("Write start x: ");
        double xStart = double.Parse(Console.ReadLine()) + 0.00001;
        Console.WriteLine("Write start y: ");
        double yStart = double.Parse(Console.ReadLine());
        Console.WriteLine("Write end x: ");
        endX = double.Parse(Console.ReadLine());
        Console.WriteLine("Write h: ");
        h = double.Parse(Console.ReadLine());
        Console.WriteLine("Write epsilon: ");
        epsilon = double.Parse(Console.ReadLine());

        double bufferH = h;
        double rungeRule = epsilon + 1;

        Console.WriteLine("\n---------------------Euler Method");
        while (rungeRule > epsilon)
        {
            Console.WriteLine("\n---------------------Runge rule");
            EulerMethod(xStart, yStart, currentFunc);
            h /= 2;
            EulerMethod(xStart, yStart, currentFunc);

            rungeRule = Math.Abs(allAsnwers[0] - allAsnwers[1]);

            if (rungeRule > epsilon)
            {
                allAsnwers.Clear();
            }
            else
            {
                allAsnwers.RemoveAt(0);
            }
        }

        h = bufferH;
        rungeRule = epsilon + 1;

        Console.WriteLine("\n---------------------Better Euler Method");
        while (rungeRule > epsilon)
        {
            Console.WriteLine("\n---------------------Runge rule");
            CoolEuler(xStart, yStart, currentFunc);
            h /= 2;
            CoolEuler(xStart, yStart, currentFunc);

            rungeRule = Math.Abs(allAsnwers[1] - allAsnwers[2]) / 3;

            if (rungeRule > epsilon)
            {
                allAsnwers.RemoveAt(2);
                allAsnwers.RemoveAt(1);
            }
            else
            {
                allAsnwers.RemoveAt(1);
            }
        }

        h = bufferH;
        rungeRule = epsilon + 1;

        Console.WriteLine("\n---------------------Miln Method");
        //Miln(xStart, yStart, currentFunc);
        while (rungeRule > epsilon)
        {
            Console.WriteLine("\n---------------------Runge rule");
            Miln(xStart, yStart, currentFunc);
            h /= 2;
            Miln(xStart, yStart, currentFunc);

            rungeRule = Math.Abs(allAsnwers[2] - allAsnwers[3]) / 15;

            if (rungeRule > epsilon)
            {
                allAsnwers.RemoveAt(3);
                allAsnwers.RemoveAt(2);
            }
            else
            {
                allAsnwers.RemoveAt(2);
            }
        }

        Console.WriteLine("All y:");

        foreach (var y in allAsnwers)
        {
            Console.WriteLine("y: " + y);
            //Console.WriteLine("f: " + currentFunc(endX, y));
        }
    }

    static double variant1(double x, double y)
    {
        return (x + Math.Exp(x) - 1);
    }

    static double variant2(double x, double y)
    {
        return (1 + x * x + 2 * (x * y) / (x + 1));
    }

    static double variant3(double x, double y)
    {
        return (2 * x * y - y * y + 5 - x * x);
    }


    static void EulerMethod(double x, double y, function function)
    {
        while (x <= endX)
        {
            Console.WriteLine($"x: {x} \ny: {y}");
            double func = function(x, y);
            Console.WriteLine($"func: {func}");

            x += h;
            y += h * func;
        }
        Console.WriteLine("---------------------Result");
        Console.WriteLine($"x: {x} \ny: {y}");
        allAsnwers.Add(y);
    }

    static void CoolEuler(double x, double y, function function)
    {
        while (x <= endX)
        {
            Console.WriteLine($"x: {x} \ny: {y}");
            double func = function(x, y);
            Console.WriteLine($"func: {func}");
            double coolFunc = function(x, y + h * func);
            Console.WriteLine($"new func: {coolFunc}");

            x += h;
            y += h / 2 * (func + coolFunc);
        }
        Console.WriteLine("---------------------Result");
        Console.WriteLine($"x: {x} \ny: {y}");
        allAsnwers.Add(y);
    }

    static void Miln(double x, double y, function function)
    {
        List<double> xs = new List<double>();
        List<double> ys = new List<double>();

        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine("first 3 y's: ");
            Console.WriteLine($"x: {x} \ny: {y}");
            double func = function(x, y);
            Console.WriteLine($"func: {func}");
            double coolFunc = function(x, y + h * func);
            Console.WriteLine($"new func: {coolFunc}");
            xs.Add(x);
            ys.Add(y);
            x += h;
            y += h / 2 * (func + coolFunc);
        }
        xs.Add(x);
        ys.Add(y);

        MilnCalc(xs, ys, function, true);
    }

    static void MilnCalc(List<double> x, List<double> y, function function, bool willForecast)
    {
        if (x.Last() > endX)
        {
            Console.WriteLine("---------------------Result");
            Console.WriteLine($"x: {x.Last()} \ny: {y.Last()}");
            allAsnwers.Add(y.Last());
            return;
        }

        double forecast = y[4];
        if (willForecast)
            forecast = y[0] + 4 / 3 * h * (2 * function(x[1], y[1]) - function(x[2], y[2]) + 2 * function(x[3], y[3]));

        Console.WriteLine($"forecast: {forecast}");
        double correction = y[2] + h / 3 * (function(x[2], y[2]) + 4 * function(x[3], y[3]) + function(x[4], forecast));
        Console.WriteLine($"correction: {correction}");

        if (Math.Abs(correction - forecast) > epsilon)
        {
            y[4] = correction;

            Console.WriteLine("correction isn't near the forecast");
            MilnCalc(x, y, function, false);
        }
        else
        {
            x.RemoveAt(0);
            double bufX = x.Last();
            x.Add(bufX + h);

            y.RemoveAt(0);
            y.Add(correction);

            Console.WriteLine("correction is near the forecast");
            MilnCalc(x, y, function, true);
        }
    }
}