class Lab3()
{
    delegate float method(float left, float right, float parts);
    delegate float func(float x);

    static List<float[]> funcParts = new List<float[]>();
    public static void doAlgos()
    {
        Console.WriteLine("Write function: ");
        string functionStr = Console.ReadLine();
        parseFunction(functionStr);

        Console.WriteLine("Write left x: ");
        float left = float.Parse(Console.ReadLine());
        Console.WriteLine("Write right x: ");
        float right = float.Parse(Console.ReadLine());
        Console.WriteLine("Write accuracy: ");
        mainEpsion = float.Parse(Console.ReadLine());

        Console.WriteLine("---\nLeft rectangles:");
        rungeRule(left, right, leftRectangles, 1);
        Console.WriteLine("---\nRight rectangles:");
        rungeRule(left, right, rightRectangles, 1);
        Console.WriteLine("---\nCenter rectangles:");
        rungeRule(left, right, centerRectangles, 2);
        Console.WriteLine("---\nTrapezoid method:");
        rungeRule(left, right, trapezoidMethod, 2);
        Console.WriteLine("---\nSimpson's method:");
        rungeRule(left, right, simpsonMethod, 4);
    }

    //func parser--------------------------------------------------------
    public static void parseFunction(string line)
    {
        //format: +-[coeff]x+-[pow] +-[coeff]x+-[pow] ...
        string numberStr = "";
        float coeff = 1;
        float pow = 1;
        bool isCoeff = true;
        bool isPow = false;
        foreach (var letter in line)
        {
            if (letter == 'x')
            {
                coeff = float.Parse(numberStr);
                numberStr = "";
                isCoeff = false;
                isPow = true;
                continue;
            }

            if (isCoeff)
            {
                numberStr += letter;
            }

            if (isPow && letter == ' ')
            {
                pow = float.Parse(numberStr);
                numberStr = "";
                //numberStr += letter;
                isCoeff = true;
                isPow = false;
                float[] coeffAndPow = new float[2];
                coeffAndPow[0] = coeff;
                coeffAndPow[1] = pow;

                funcParts.Add(coeffAndPow);
            }

            if (isPow)
            {
                numberStr += letter;
            }
        }
        //funcParts.Add((float x) => coeff * (float)Math.Pow(x, pow));
    }

    static float microFunc(float coeff, float pow, float x)
    {
        return coeff * (float)Math.Pow(x, pow);
    }
    //func parser--------------------------------------------------------

    static float mainEpsion;

    static float Function(float x)
    {
        float result = 0;
        foreach (float[] onePart in funcParts)
        {
            result += microFunc(onePart[0], onePart[1], x);
        }
        return result;
    }

    static void rungeRule(float left, float right, method currentMethod, float k)
    {
        float startParts = 10;
        float valueBefore = currentMethod(left, right, startParts);
        startParts += startParts;
        float nextValue = currentMethod(left, right, startParts);
        while (Math.Abs(nextValue - valueBefore) / (Math.Pow(2, k) - 1) > mainEpsion)
        {
            valueBefore = currentMethod(left, right, startParts);
            startParts += startParts;
            nextValue = currentMethod(left, right, startParts);

            if (startParts > 50000)
            {
                Console.WriteLine("Can't calculate with such accuracy :(");
                return;
            }
        }

        Console.WriteLine("Result: " + nextValue);
        Console.WriteLine("n: " + startParts);
    }

    static float leftRectangles(float left, float right, float parts)
    {
        float h = (right - left) / parts;
        float result = 0;

        while (left < right)
        {
            result += Function(left);
            left += h;
        }
        return result * h;
    }

    static float rightRectangles(float left, float right, float parts)
    {
        float h = (right - left) / parts;
        float result = 0;
        float iterationRight = left + h;

        while (iterationRight <= right)
        {
            result += Function(iterationRight);
            iterationRight += h;
        }
        return result * h;
    }

    static float centerRectangles(float left, float right, float parts)
    {
        float h = (right - left) / parts;
        left = left + h / 2;
        float result = 0;

        while (left < right)
        {
            result += Function(left);
            left += h;
        }
        return result * h;
    }


    static float trapezoidMethod(float left, float right, float parts)
    {
        float h = (right - left) / parts;
        float result = 0;
        result += Function(left) / 2F;
        left += h;

        while (left < right)
        {
            result += Function(left);
            left += h;
        }

        result += Function(right) / 2F;
        return result * h;
    }

    static float simpsonMethod(float left, float right, float parts)
    {
        float h = (right - left) / parts;
        float result = 0;
        result += Function(left);
        left += h;

        //четная
        float evenResult = 0;
        //нечетная
        float oddResult = 0;

        int index = 1;

        while (left < right)
        {
            if (index % 2 == 0)
            {
                evenResult += Function(left);
            }
            else
            {
                oddResult += Function(left);
            }
            left += h;
            index++;
        }

        result += Function(right);
        result += evenResult * 4 + oddResult * 2;
        return result * h / 3;
    }
}