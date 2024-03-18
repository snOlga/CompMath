using ZedGraph;

public class Lab2_2
{
    delegate float function(float x);
    public static void doAlgos()
    {
        Console.WriteLine("1. ln(x) + x");
        Console.WriteLine("2. 1/x + x^2");
        Console.WriteLine("3. 1/x + sqrt(x) - 3");

        string whatNumber = Console.ReadLine();

        function func;
        function dervFunc;

        switch (whatNumber)
        {
            case "1": func = variant1; dervFunc = derivativeVariant1; break;
            case "2": func = variant2; dervFunc = derivativeVariant2; break;
            case "3": func = variant3; dervFunc = derivativeVariant3; break;
            default: return;
        }

        float leftX = float.Parse(Console.ReadLine());
        float rightX = float.Parse(Console.ReadLine());

        if (func(leftX) > 0 && func(rightX) > 0 || func(leftX) < 0 && func(rightX) < 0)
        {
            Console.WriteLine("This segment has zero or more than one root!");
            return;
        }

        binarySearch(func, leftX, rightX);
        Console.WriteLine("---------------------------------");
        NewtonSearch(func, dervFunc, (leftX + rightX) / 2);
        Console.WriteLine("---------------------------------");
        simpleIterationSearch(func, dervFunc, leftX, rightX);

    }

    private static float variant1(float x)
    {
        return (float)Math.Log(x) + x;
    }
    private static float derivativeVariant1(float x)
    {
        return (float)1 / x + 1;
    }
    //---

    private static float variant2(float x)
    {
        return 1 / x + x * x;
    }
    private static float derivativeVariant2(float x)
    {
        return 2 * x - 1 / (x * x);
    }

    //---

    private static float variant3(float x)
    {
        return 1 / x + (float)Math.Sqrt(x) - 3;
    }
    private static float derivativeVariant3(float x)
    {
        return (float)Math.Sqrt(1 / x) * 0.5F - 1 / (x * x);
    }


    private static void binarySearch(function func, float leftX, float rightX)
    {
        Console.WriteLine("---");
        Console.WriteLine("left x is " + leftX);
        Console.WriteLine("right x is " + rightX);

        float newX = (leftX + rightX) / 2;
        Console.WriteLine("new x is " + newX);

        float newResult = func(newX);
        Console.WriteLine("f(new x) = " + newResult);

        if (Math.Abs(newResult) < 0.01)
        {
            Console.WriteLine("\nFunction is 0 with x " + newX);
            return;
        }

        float leftResult = func(leftX);
        Console.WriteLine("f(new x)*f(left x) = " + leftResult * newResult);

        if (leftResult * newResult > 0)
        {
            binarySearch(func, newX, rightX);
        }
        else
        {
            binarySearch(func, leftX, newX);
        }
    }


    private static void NewtonSearch(function func, function dervFunc, float xBefore)
    {
        Console.WriteLine("---");
        Console.WriteLine("x before is " + xBefore);
        Console.WriteLine("f(x before) = " + func(xBefore));
        Console.WriteLine("f'(x before) = " + dervFunc(xBefore));

        float newX = xBefore - (func(xBefore) / dervFunc(xBefore));

        Console.WriteLine("new x is " + newX);

        if (Math.Abs(newX - xBefore) < 0.01)
        {
            Console.WriteLine("\nFunction is 0 with x " + newX);
            return;
        }

        NewtonSearch(func, dervFunc, newX);
    }


    private static void simpleIterationSearch(function func, function dervFunc, float leftX, float rightX)
    {
        //function iterFunc;

        float lambda = (dervFunc(leftX) > 0 ? 1 : (0 - 1)) / (Math.Max(dervFunc(leftX), dervFunc(rightX)));

        if (1 + lambda * (dervFunc(leftX)) >= 1 && 1 + lambda * (dervFunc(rightX)) >= 1)
        {
            Console.WriteLine("There's no convergence condition!");
            return;
        }

        iterSearch(iterFunc, (leftX + rightX) / 2);

        float iterFunc(float x) => x + lambda * func(x);
    }

    private static void iterSearch(function func, float xBefore)
    {
        Console.WriteLine("---");
        float newX = func(xBefore);

        Console.WriteLine("new x is " + newX);
        Console.WriteLine("x diff: " + Math.Abs(xBefore - newX));
        if (Math.Abs(xBefore - newX) < 0.01)
        {
            Console.WriteLine("Function is 0 with x " + newX);
            return;
        }

        iterSearch(func, newX);
    }
}