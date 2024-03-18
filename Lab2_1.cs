public class Lab2
{
    public static void doAlgos()
    {
        binarySearch(-0.5F, 1);
        Console.WriteLine("\n----------------------------------------------\n");
        secantSearch(6,6.3F);
        Console.WriteLine("\n----------------------------------------------\n");
        iterSearch(-2);
        Console.WriteLine("\n----------------------------------------------\n");
        iterTwoSearch(-1, 0.25F);
    }

    private static float function(float x)
    {
        return x * x * x - (4.5F) * x * x - (9.21F) * x - (0.383F);
    }

    private static void binarySearch(float leftX, float rightX)
    {
        Console.WriteLine("---");
        Console.WriteLine("left x is " + leftX);
        Console.WriteLine("right x is " + rightX);
        
        float newX = (leftX+rightX)/2;
        Console.WriteLine("new x is " + newX);

        float newResult = function(newX);
        Console.WriteLine("f(new x) = " + newResult);

        if(Math.Abs(newResult) < 0.01)
        {
            Console.WriteLine("Function is 0 with x " + newX);
            return;
        }

        float leftResult = function(leftX);
        Console.WriteLine("f(new x)*f(left x) = " + leftResult*newResult);

        if(leftResult*newResult > 0)
        {
            binarySearch(newX, rightX);
        }
        else
        {
            binarySearch(leftX, newX);
        }
    }

    private static void secantSearch(float currentX, float beforeX)
    {
        Console.WriteLine("---");
        Console.WriteLine("x before is " + beforeX);
        Console.WriteLine("currrent x is " + currentX);

        float newX = currentX - (currentX - beforeX)/(function(currentX) - function(beforeX)*function(currentX));
        Console.WriteLine("f(x before) = " + function(beforeX));
        Console.WriteLine("f(current x) = " + function(currentX));

        Console.WriteLine("new x is " + newX);
        //Console.WriteLine("f(new x) = " + function(newX));

        if(Math.Abs(currentX - beforeX) <= 0.01)
        {
            Console.WriteLine("Function is 0 with x " + newX);
            return;
        }

        secantSearch(newX, currentX);
    }


    private static void iterSearch(float xBefore)
    {
        Console.WriteLine("---");
        float newX = 0.21645F*xBefore*xBefore + 1.443001F * xBefore + 0.0184223F - 0.0481F * xBefore*xBefore*xBefore;

        Console.WriteLine("new x is " +  newX);
        Console.WriteLine("x diff: " + Math.Abs(xBefore - newX));
        if(Math.Abs(xBefore - newX) < 0.01)
        {
            Console.WriteLine("Function is 0 with x " + newX);
            return;
        }

        iterSearch(newX);
    }



    private static void iterTwoSearch(float xBefore, float yBefore)
    {
        Console.WriteLine("---");
        float newX = 0-0.4F-(float)Math.Sin(yBefore);
        float newY = (float) Math.Cos(xBefore+1)/2;

        Console.WriteLine("new x is " +  newX);
        Console.WriteLine("new y is " +  newY);
        Console.WriteLine("x diff: " + Math.Abs(xBefore - newX));
        Console.WriteLine("y diff: " + Math.Abs(yBefore - newY));
        if(Math.Abs(xBefore - newX) < 0.01 || Math.Abs(yBefore - newY) < 0.01)
        {
            Console.WriteLine("Function is 0 with x " + newX);
            Console.WriteLine("Function is 0 with y " + newY);
            return;
        }

        iterTwoSearch(newX, newY);
    }
}