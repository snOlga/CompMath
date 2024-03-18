public class Lab2_3
{
    public static void doAlgos()
    {
        Console.WriteLine("sin(x+y) - 1,4x = 0");
        Console.WriteLine("x^2 + y^2 - 1 = 0");

        Console.WriteLine("Write x: ");
        double x = double.Parse(Console.ReadLine());
        Console.WriteLine("Write y: ");
        double y = double.Parse(Console.ReadLine());

        try
        {
            NewtonSearch(x, y);
        }
        catch (StackOverflowException e)
        {
            Console.WriteLine("Bad point :(");
            return;
        }
    }

    private static double f(double x, double y)
    {
        return Math.Sin(x + y) - 1.4 * x;
    }
    private static double g(double x, double y)
    {
        return x * x + y * y - 1;
    }

    private static double dfdx(double x, double y)
    {
        return Math.Cos(x + y) - 1.4;
    }
    private static double dfdy(double x, double y)
    {
        return Math.Cos(x + y);
    }
    private static double dgdx(double x, double y)
    {
        return 2 * x;
    }
    private static double dgdy(double x, double y)
    {
        return 2 * y;
    }


    public static int nestingCounter = 0;
    private static void NewtonSearch(double xBefore, double yBefore)
    {
        if (nestingCounter >= 100)
        {
            Console.WriteLine("Bad point :(");
            return;
        }
        nestingCounter++;

        double xCoef1 = dfdx(xBefore, yBefore);
        double yCoef1 = dfdy(xBefore, yBefore);

        double xCoef2 = dgdx(xBefore, yBefore);
        double yCoef2 = dgdy(xBefore, yBefore);

        double const1 = 0 - f(xBefore, yBefore);
        double const2 = 0 - g(xBefore, yBefore);

        double deltaX = (const1 - (yCoef1 * const2 / yCoef2)) / (xCoef1 - (yCoef1 * xCoef2 / yCoef2));
        double deltaY = ((const2 - xCoef2 * deltaX) / yCoef2);


        double newX = xBefore + deltaX;
        double newY = yBefore + deltaY;

        if (Math.Abs(newX - xBefore) <= 0.01 || Math.Abs(newY - yBefore) <= 0.01)
        {
            Console.WriteLine("Function is 0 with x " + newX + "\nIteration count: " + nestingCounter);
            return;
        }

        NewtonSearch(newX, newY);
    }
}