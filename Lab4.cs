using System.Drawing;

//ohhhhh so bad code here))
class Lab4
{
    static Dictionary<float, float> values = new Dictionary<float, float>();
    public static void doAlgos()
    {
        // Console.WriteLine("f(x): 4*x / (x^4 + 12)");
        // //drawGraphic(function, "main.jpg");
        // Console.WriteLine("Write start value:"); 
        // startValue = float.Parse(Console.ReadLine());
        // Console.WriteLine("Write end value:");
        // endValue = float.Parse(Console.ReadLine());
        // step = (endValue - startValue) / stepAmount;
        // drawGraphic(function, "grapics/main.jpg");

        Console.WriteLine("How many points?");
        int steps = int.Parse(Console.ReadLine());
        Console.WriteLine("format: [x] [y]");
        while (steps > 0)
        {
            string point = Console.ReadLine();
            values.Add(
                float.Parse(point.Split(" ")[0]) == 0 ? 0.001F : float.Parse(point.Split(" ")[0]), 
                float.Parse(point.Split(" ")[1]) == 0 ? 0.001F : float.Parse(point.Split(" ")[1]));
            steps--;
        }

        float sumX = 0;
        float sumY = 0;
        //pirson's coeff
        foreach (var currPoint in values)
        {
            sumX += currPoint.Key;
            sumY += currPoint.Value;
        }
        float up = 0;
        float down = 0;
        foreach (var currPoint in values)
        {
            up += (currPoint.Key - sumX) * (currPoint.Value - sumY);
            down += (float)Math.Pow((currPoint.Key - sumX), 2) + (float)Math.Pow((currPoint.Value - sumY), 2);
        }
        float pirsonCoeff = up / (float)(Math.Sqrt(down));
        Console.WriteLine("Pirson's coefficient: " + pirsonCoeff);
        Console.WriteLine(pirsonCoeff < 0.3F ? "Low connection"
                        : pirsonCoeff < 0.5F ? "Pre-normal connection"
                        : pirsonCoeff < 0.7F ? "Normal connection"
                        : pirsonCoeff < 0.9F ? "High connection"
                        : "Very high connection");






        Console.WriteLine("------------------------");
        Console.WriteLine("Linear approximation");
        LinearApprox(true);
        Console.WriteLine("------------------------");
        Console.WriteLine("Quadratic approximation");
        QuadraticApprox();
        Console.WriteLine("------------------------");
        Console.WriteLine("Cube approximation");
        CubeApprox();
        Console.WriteLine("------------------------");
        Console.WriteLine("Exponential approximation");
        ExponentialApprox();
        Console.WriteLine("------------------------");
        Console.WriteLine("Logarithmic approximation");
        LogApprox();
        Console.WriteLine("------------------------");
        Console.WriteLine("Power approximation");
        PowApprox();
        Console.WriteLine("------------------------");

        Console.WriteLine("The best approximation is " + bestApproxName + " " + bestApproxValue);

        foreach (var coeff in approxValues)
        {
            Console.WriteLine(coeff.Value + " " + coeff.Key);
        }
    }

    delegate float Function(float x);

    static string bestApproxName = "";
    static float bestApproxValue = 0;

    static Dictionary<string, float> approxValues = new Dictionary<string, float>();

    static float QuadraticDeviation(Function approx)
    {
        float result = 0;

        foreach (var point in values)
        {
            result += (float)Math.Pow((approx(point.Key) - point.Value), 2);
        }
        return (float)Math.Sqrt((result / 2));
    }


    static List<float> LinearApprox(bool willPrint)
    {
        float sumX = 0;
        float sumY = 0;

        float sumXX = 0;
        float sumXY = 0;

        foreach (var point in values)
        {
            sumX += point.Key;
            sumY += point.Value;
            sumXX += (point.Key * point.Key);
            sumXY += (point.Key * point.Value);
        }

        List<double[]> matrix = new List<double[]>();
        List<double> constants = new List<double>();

        double[] firstLine = new double[2];
        double[] secondLine = new double[2];

        firstLine[0] = sumXX;
        firstLine[1] = sumX;
        secondLine[0] = sumX;
        secondLine[1] = values.Count;

        constants.Add(sumXY);
        constants.Add(sumY);
        matrix.Add(firstLine);
        matrix.Add(secondLine);

        List<float> solved = MatrixSolver(matrix, constants);

        Console.WriteLine("Coefficients:");
        foreach (var coef in solved)
        {
            Console.WriteLine(coef);
        }
        //solved.Select(x => Console.WriteLine(x));

        Function approx = (x) => { return (solved[0] * x + solved[1]); };

        drawGraphic(approx, "grapics/linear.jpg");

        Console.WriteLine("Quadratic deviation: " + QuadraticDeviation(approx));

        if (willPrint)
        {
            //determ coeff
            float determCoeff = 0;
            float allApprox = 0;
            float up = 0;
            float down = 0;
            foreach (var point in values)
            {
                up += (float)Math.Pow(point.Value - point.Key, 2);
                allApprox += approx(point.Key);
            }
            foreach (var point in values)
            {
                down += (float)Math.Pow(point.Value - allApprox, 2);
            }
            determCoeff = 1 - (up / down);
            Console.WriteLine("Coefficient of determination: " + determCoeff);
            Console.WriteLine(determCoeff >= 0.95F ? "High accuracy"
                        : determCoeff >= 0.75F ? "Normal accuracy"
                        : determCoeff >= 0.5F ? "Low accuracy" : "Bad accuracy!");
            if (determCoeff > bestApproxValue)
            {
                bestApproxValue = determCoeff;
                bestApproxName = "Linear";
            }
            approxValues.Add("Linear", determCoeff);
        }

        return solved;
    }


    static void QuadraticApprox()
    {
        float sumX = 0;
        float sumY = 0;

        float sumXX = 0;
        float sumXY = 0;

        float sumX3 = 0;
        float sumX4 = 0;

        float sumXXY = 0;

        foreach (var point in values)
        {
            sumX += point.Key;
            sumY += point.Value;
            sumXX += (point.Key * point.Key);
            sumXY += (point.Key * point.Value);
            sumX3 += (float)Math.Pow(point.Key, 3);
            sumX4 += (float)Math.Pow(point.Key, 4);
            sumXXY += (point.Key * point.Key) * point.Value;
        }

        List<double[]> matrix = new List<double[]>();
        List<double> constants = new List<double>();

        double[] firstLine = new double[3];
        double[] secondLine = new double[3];
        double[] thirdLine = new double[3];

        firstLine[0] = values.Count;
        firstLine[1] = sumX;
        firstLine[2] = sumXX;
        secondLine[0] = sumX;
        secondLine[1] = sumXX;
        secondLine[2] = sumX3;
        thirdLine[0] = sumXX;
        thirdLine[1] = sumX3;
        thirdLine[2] = sumX4;


        constants.Add(sumY);
        constants.Add(sumXY);
        constants.Add(sumXXY);
        matrix.Add(firstLine);
        matrix.Add(secondLine);
        matrix.Add(thirdLine);

        List<float> solved = MatrixSolver(matrix, constants);

        Console.WriteLine("Coefficients:");
        foreach (var coef in solved)
        {
            Console.WriteLine(coef);
        }

        Function approx = (x) => { return solved[0] + solved[1] * x + solved[2] * x * x; };

        drawGraphic(approx, "grapics/quadro.jpg");

        Console.WriteLine("Quadratic deviation: " + QuadraticDeviation(approx));

        //determ coeff
        float determCoeff = 0;
        float allApprox = 0;
        float up = 0;
        float down = 0;
        foreach (var point in values)
        {
            up += (float)Math.Pow(point.Value - approx(point.Key), 2);
            allApprox += approx(point.Key);
        }
        foreach (var point in values)
        {
            down += (float)Math.Pow(point.Value - allApprox, 2);

        }
        determCoeff = 1 - (up / down);
        Console.WriteLine("Coefficient of determination: " + determCoeff);
        Console.WriteLine(determCoeff >= 0.95F ? "High accuracy"
                        : determCoeff >= 0.75F ? "Normal accuracy"
                        : determCoeff >= 0.5F ? "Low accuracy" : "Bad accuracy!");
        if (determCoeff > bestApproxValue)
        {
            bestApproxValue = determCoeff;
            bestApproxName = "Quadratic";
        }
        approxValues.Add("Quadratic", determCoeff);
    }

    static void CubeApprox()
    {
        float sumX = 0;
        float sumY = 0;

        float sumXX = 0;
        float sumXY = 0;

        float sumX3 = 0;
        float sumXXY = 0;

        float sumX4 = 0;
        float sumX3Y = 0;

        float sumX5 = 0;

        float sumX6 = 0;

        foreach (var point in values)
        {
            float y = point.Value;
            float x = point.Key;
            sumX += x;
            sumY += y;
            sumXX += (x * x);
            sumXY += (x * y);
            sumX3 += (float)Math.Pow(x, 3);
            sumXXY += (x * x) * y;
            sumX4 += (float)Math.Pow(x, 4);
            sumX3Y += (float)Math.Pow(x, 3) * y;
            sumX5 += (float)Math.Pow(x, 5);
            //sumX4Y += (float)Math.Pow(x, 4)*y;
            sumX6 += (float)Math.Pow(x, 6);
        }

        List<double[]> matrix = new List<double[]>();
        List<double> constants = new List<double>();

        double[] firstLine = new double[4];
        double[] secondLine = new double[4];
        double[] thirdLine = new double[4];
        double[] forthLine = new double[4];

        firstLine[0] = values.Count;
        firstLine[1] = sumX;
        firstLine[2] = sumXX;
        firstLine[3] = sumX3;
        secondLine[0] = sumX;
        secondLine[1] = sumXX;
        secondLine[2] = sumX3;
        secondLine[3] = sumX4;
        thirdLine[0] = sumXX;
        thirdLine[1] = sumX3;
        thirdLine[2] = sumX4;
        thirdLine[3] = sumX5;
        forthLine[0] = sumX3;
        forthLine[1] = sumX4;
        forthLine[2] = sumX5;
        forthLine[3] = sumX6;


        constants.Add(sumY);
        constants.Add(sumXY);
        constants.Add(sumXXY);
        constants.Add(sumX3Y);
        matrix.Add(firstLine);
        matrix.Add(secondLine);
        matrix.Add(thirdLine);
        matrix.Add(forthLine);

        List<float> solved = MatrixSolver(matrix, constants);

        Console.WriteLine("Coefficients:");
        foreach (var coef in solved)
        {
            Console.WriteLine(coef);
        }

        Function approx = (x) => { return solved[0] + solved[1] * x + solved[2] * x * x + solved[3] * x * x * x; };

        drawGraphic(approx, "grapics/cube.jpg");

        Console.WriteLine("Quadratic deviation: " + QuadraticDeviation(approx));

        //determ coeff
        float determCoeff = 0;
        float allApprox = 0;
        float up = 0;
        float down = 0;
        foreach (var point in values)
        {
            up += (float)Math.Pow(point.Value - approx(point.Key), 2);
            allApprox += approx(point.Key);
        }
        foreach (var point in values)
        {
            down += (float)Math.Pow(point.Value - allApprox, 2);
        }
        determCoeff = 1 - (up / down);
        Console.WriteLine("Coefficient of determination: " + determCoeff);
        Console.WriteLine(determCoeff >= 0.95F ? "High accuracy"
                        : determCoeff >= 0.75F ? "Normal accuracy"
                        : determCoeff >= 0.5F ? "Low accuracy" : "Bad accuracy!");
        if (determCoeff > bestApproxValue)
        {
            bestApproxValue = determCoeff;
            bestApproxName = "Cube";
        }
        approxValues.Add("Cube", determCoeff);
    }

    static void ExponentialApprox()
    {
        List<float> solved = LinearApprox(false);

        Function approx = (x) => { return (solved[0] * (float)Math.Exp(solved[1] * x)); };

        drawGraphic(approx, "grapics/expo.jpg");

        //Console.WriteLine("Quadratic deviation: " + QuadraticDeviation(approx));

        //determ coeff
        float determCoeff = 0;
        float allApprox = 0;
        float up = 0;
        float down = 0;
        foreach (var point in values)
        {
            up += (float)Math.Pow(point.Value - approx(point.Key), 2);
            allApprox += approx(point.Key);
        }
        foreach (var point in values)
        {
            down += (float)Math.Pow(point.Value - allApprox, 2);
        }
        determCoeff = 1 - (up / down);
        Console.WriteLine("Coefficient of determination: " + determCoeff);
        Console.WriteLine(determCoeff >= 0.95F ? "High accuracy"
                        : determCoeff >= 0.75F ? "Normal accuracy"
                        : determCoeff >= 0.5F ? "Low accuracy" : "Bad accuracy!");
        if (determCoeff > bestApproxValue)
        {
            bestApproxValue = determCoeff;
            bestApproxName = "Exponential";
        }
        approxValues.Add("Exponential", determCoeff);
    }

    static void LogApprox()
    {
        List<float> solved = LinearApprox(false);

        Function approx = (x) => { return (solved[0] * (float)Math.Log(Math.Abs(x)) + solved[1]); };

        drawGraphic(approx, "grapics/log.jpg");

        //Console.WriteLine("Quadratic deviation: " + QuadraticDeviation(approx));

        //determ coeff
        float determCoeff = 0;
        float allApprox = 0;
        float up = 0;
        float down = 0;
        foreach (var point in values)
        {
            up += (float)Math.Pow(point.Value - approx(point.Key), 2);
            allApprox += approx(point.Key);
        }
        foreach (var point in values)
        {
            down += (float)Math.Pow(point.Value - allApprox, 2);
        }
        determCoeff = 1 - (up / down);
        Console.WriteLine("Coefficient of determination: " + determCoeff);
        Console.WriteLine(determCoeff >= 0.95F ? "High accuracy"
                        : determCoeff >= 0.75F ? "Normal accuracy"
                        : determCoeff >= 0.5F ? "Low accuracy" : "Bad accuracy!");
        if (determCoeff > bestApproxValue)
        {
            bestApproxValue = determCoeff;
            bestApproxName = "Logarithmic";
        }
        approxValues.Add("Logatithmic", determCoeff);
    }

    static void PowApprox()
    {
        List<float> solved = LinearApprox(false);

        Function approx = (x) => { return (solved[0] * (float)Math.Pow(Math.Abs(x), solved[1])); };

        drawGraphic(approx, "grapics/pow.jpg");

        //Console.WriteLine("Quadratic deviation: " + QuadraticDeviation(approx));

        //determ coeff
        float determCoeff = 0;
        float allApprox = 0;
        float up = 0;
        float down = 0;
        foreach (var point in values)
        {
            up += (float)Math.Pow(point.Value - approx(point.Key), 2);
            allApprox += approx(point.Key);
        }
        foreach (var point in values)
        {
            down += (float)Math.Pow(point.Value - allApprox, 2);
        }
        determCoeff = 1 - (up / down);
        Console.WriteLine("Coefficient of determination: " + determCoeff);
        Console.WriteLine(determCoeff >= 0.95F ? "High accuracy"
                        : determCoeff >= 0.75F ? "Normal accuracy"
                        : determCoeff >= 0.5F ? "Low accuracy" : "Bad accuracy!");
        if (determCoeff > bestApproxValue)
        {
            bestApproxValue = determCoeff;
            bestApproxName = "Power";
        }
        approxValues.Add("Power", determCoeff);
    }













    static List<float> MatrixSolver(List<double[]> matrix, List<double> constants)
    {
        double errorScaleConst = 0.001;

        Dictionary<int, double[]> equations = new Dictionary<int, double[]>();

        //0 is for constants; last for division
        for (int xNumber = 1; xNumber <= matrix[0].Length; xNumber++)
        {
            List<double> bufList = matrix[xNumber - 1].ToList();
            double division_buf = bufList[xNumber - 1];
            bufList.RemoveAt(xNumber - 1);
            bufList.Insert(xNumber - 1, 0);
            bufList = bufList.Select(number => (0 - number)).ToList();
            bufList.Insert(0, constants[xNumber - 1]);
            bufList.Add(division_buf);

            equations.Add(xNumber, bufList.ToArray());
        }

        Dictionary<int, double[]> rememberX = new Dictionary<int, double[]>();
        rememberX.Add(0, new double[2]); //for constants
        rememberX[0][0] = 1;
        for (int xNumber = 1; xNumber <= matrix[0].Length; xNumber++)
        {
            //constant/xCoef -> cause of x is null now
            double[] newX = new double[2];
            newX[0] = equations[xNumber][0] / equations[xNumber][equations[xNumber].Length - 1];
            rememberX.Add(xNumber, newX);
        }

        int iterationCount = 1;
        bool isErrorScaleLower = false;
        while (!isErrorScaleLower)
        {
            int amountErrorScaleLower = 0;

            for (int xNumber = 1; xNumber <= equations.Count; xNumber++)
            {
                double xBefore = rememberX[xNumber][0];
                double currentX = 0;
                int lastXIndex = equations[xNumber].Length - 1;

                for (int xInEquation = 0; xInEquation < lastXIndex; xInEquation++)
                {
                    currentX = currentX + (equations[xNumber][xInEquation] * rememberX[xInEquation][0]);
                }

                currentX = currentX / equations[xNumber][lastXIndex];
                rememberX[xNumber][0] = currentX;
                double currentErrorScale = Math.Abs((double)((double)(currentX - xBefore)) / currentX);
                rememberX[xNumber][1] = currentErrorScale;
                if (currentErrorScale <= errorScaleConst)
                {
                    //errorScaleForOutput = currentErrorScale;
                    amountErrorScaleLower++;
                }
            }

            iterationCount++;

            if (amountErrorScaleLower == rememberX.Count - 1)
            {
                isErrorScaleLower = true;
            }
            else
            {
                amountErrorScaleLower = 0;
            }
        }

        // Console.WriteLine("Iteration count is " + iterationCount);
        // //Console.WriteLine("Error scale is " + errorScaleForOutput);

        List<float> returnedValues = new List<float>();

        for (int xNumber = 1; xNumber <= rememberX.Count - 1; xNumber++)
        {
            returnedValues.Add((float)rememberX[xNumber][0]);
            //Console.WriteLine("x" + xNumber + " is " + (decimal)rememberX[xNumber][0]);
        }

        return returnedValues;
    }


    static void drawGraphic(Function mainFunc, string fileName)
    {
        // Image bmp = new Bitmap(1000, 1000);
        // Graphics graphic = Graphics.FromImage(bmp);
        // graphic.Clear(Color.FromArgb(255, 255, 255));
        // graphic.TranslateTransform(500, 500);
        // graphic.DrawLine(Pens.Blue, -500, 0, 500, 0);
        // graphic.DrawLine(Pens.Blue, 0, -500, 0, 500);
        // //g.ScaleTransform(1000, 1000);
        // //g.DrawLine(Pens.Black, 0, 0, -100, -100);

        // float xBefore = 0;
        // float yBefore = 0;
        // bool isFirstTime = true;

        // foreach (var point in values)
        // {
        //     Console.WriteLine("x: " + (decimal)point.Key);
        //     Console.WriteLine("y: " + (decimal)point.Value);
        //     Console.WriteLine("phi: " + mainFunc(point.Key));
        //     Console.WriteLine("epsilon: " + (point.Value - mainFunc(point.Key)));
        //     if (isFirstTime)
        //     {
        //         xBefore = point.Key;
        //         yBefore = mainFunc(point.Key);
        //         isFirstTime = false;
        //     }
        //     else
        //     {
        //         float result = mainFunc(point.Key);
        //         graphic.DrawLine(Pens.Black, xBefore * 100, yBefore * 100, (point.Key) * 100, mainFunc(point.Key) * 100);

        //         xBefore = point.Key;
        //         yBefore = mainFunc(point.Key);
        //     }
        // }

        // bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

        // bmp.Save(fileName);
    }
}
