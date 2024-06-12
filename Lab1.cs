public class Lab1
{
    public static void doAlgos()
    {
        List<double[]> matrix = new List<double[]>();
        List<double> constants = new List<double>();

        //unused number
        Console.ReadLine();

        ///Console.WriteLine("Write amount of matrix lines:");
        bool hasExp = true;
        int lines = 0;
        while (hasExp)
        {
            try
            {
                lines = int.Parse(Console.ReadLine());
                hasExp = false;
            }
            catch (Exception)
            {
                hasExp = true;
            }
        }

        //Console.WriteLine("Write error scale:");
        hasExp = true;
        double errorScaleConst = 0;
        while (hasExp)
        {
            try
            {
                errorScaleConst = double.Parse(Console.ReadLine().Replace(".", ","));
                hasExp = false;
            }
            catch (Exception)
            {
                hasExp = true;
            }
        }

        //Console.WriteLine("Write matrix:");
        for (int i = 0; i < lines; i++)
        {
            string[] numbers = Console.ReadLine().Replace(".", ",").Split(" ");
            try
            {
                matrix.Add(Array.ConvertAll(numbers[0..(numbers.Length - 1)], double.Parse));
                constants.Add(double.Parse(numbers[numbers.Length - 1]));
            }
            catch (Exception)
            {
                //Console.WriteLine("Exeption");
                i--;
            }
        }

        // Console.WriteLine("Write the column of matrix constants:");
        // for (int i = 0; i < lines; i++)
        // {
        //     constants.Add(double.Parse(Console.ReadLine()));
        // }

        //is matrix ok; if not - change lines if we can
        double errorScaleForOutput = 0;
        int equalsAmount = 0;
        for (int index = 0; index < matrix.Count; index++)
        {
            double currentDiagElement = Math.Abs(matrix[index][index]);
            double elementAbsSum = matrix[index].Select(number => Math.Abs(number)).Sum() - currentDiagElement;

            if (currentDiagElement == elementAbsSum)
            {
                equalsAmount++;
                continue;
            }
            if (currentDiagElement < elementAbsSum)
            {
                double[] tempColumn = matrix.Select(str => str[index]).ToArray();
                tempColumn = tempColumn.Select(Math.Abs).ToArray();
                double searchedNumber = tempColumn[index];
                int indexOfSearched = index;

                bool foundNumber = false;
                for (int columnIndex = index + 1; columnIndex < tempColumn.Length; columnIndex++)
                {
                    searchedNumber = tempColumn[columnIndex];
                    double sumAroundNumber = matrix[columnIndex].Select(Math.Abs).Sum() - searchedNumber;
                    if (searchedNumber > sumAroundNumber)
                    {
                        foundNumber = true;
                        indexOfSearched = columnIndex;
                        break;
                    }
                    if (searchedNumber == sumAroundNumber)
                    {
                        foundNumber = true;
                        indexOfSearched = columnIndex;
                        equalsAmount++;
                        break;
                    }
                }

                if (!foundNumber)
                {
                    equalsAmount = matrix.Count;
                    break;
                }
                else
                {
                    double[] tempStr = matrix[index];
                    double[] tempMaxStr = matrix[indexOfSearched];
                    matrix.RemoveAt(index);
                    matrix.Add(tempStr);
                    matrix.RemoveAt(indexOfSearched - 1);
                    matrix.Insert(index, tempMaxStr);
                    index--;
                }
            }
        }
        if (equalsAmount == matrix.Count)
        {
            //Console.WriteLine("That's incorrect matrix!");
            return;
        }

        //Console.WriteLine("------------------------");

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

        //Console.WriteLine("Iteration count is " + iterationCount);

        //Console.WriteLine("Error scale is " + errorScaleForOutput);

        for (int xNumber = 1; xNumber <= rememberX.Count - 1; xNumber++)
        {
            Console.Write((decimal)rememberX[xNumber][0] + " ");
        }
    }
}