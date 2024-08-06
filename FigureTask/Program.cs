using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FigureTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double areaOfTheFirstFigure = 0;
            double areaOfTheSecondFigure = 0;

            double[,] triangleCoordinates11 = { { 12, 15 }, { 0, 0 }, { -1, -13 } };
            double[,] triangleCoordinates12 = { { 123, 153 }, { 30, 30 }, { -31, -133 } };
            double[,] retangleCoordinates11 = { { 12, 15 }, { 0, 0 }, { -1, -13 }, { -21, -213 } };

            Figure triangle11 = new Triangle(triangleCoordinates11);
            Figure triangle12 = new Triangle(triangleCoordinates12);
            Figure retangle11 = new Rectangle(retangleCoordinates11);

            var Figure1 = new Check(new List<Figure>() { triangle11, triangle12, retangle11 });

            double[,] retangleCoordinates21 = { { 123, 153 }, { 302, 30 }, { -31, -133 }, { -1, -13 } };
            double[,] retangleCoordinates22 = { { 13, 315 }, { 320, 330 }, { -133, -13 }, { -1, -123 } };
            double[,] triangleCoordinates21 = { { 182, 15 }, { 50, 80 }, { -41, -183 } };

            Figure retangle21 = new Rectangle(retangleCoordinates21);
            Figure retangle22 = new Rectangle(retangleCoordinates22);
            Figure triangle21 = new Triangle(triangleCoordinates21);

            var Figure2 = new Check(new List<Figure>() { retangle21, retangle22, triangle21 });

            if (Figure1.IsValid() == 0 && Figure2.IsValid() != 0)
            {
                Console.WriteLine("Вторая фигура больше, так как приметивы первой фигуры не пересекаются");
            }
            else if (Figure2.IsValid() == 0 && Figure1.IsValid() != 0)
            {
                Console.WriteLine("Первая фигура больше, так как приметивы второй фигуры не пересекаются");
            }
            else if (Figure1.IsValid() == 0 && Figure2.IsValid() == 0)
            {
                Console.WriteLine("Обе фигуры не создаются, так приметивы обеих фигур не имеют пересечений");
            }
            else
            {
                foreach (var figure in Figure1.Figures)
                {
                    areaOfTheFirstFigure += figure.GetArea();
                }

                foreach (var figure in Figure2.Figures)
                {
                    areaOfTheFirstFigure += figure.GetArea();
                }

                if (areaOfTheFirstFigure > areaOfTheSecondFigure)
                {
                    Console.WriteLine($"Первая фигура больше второй на {areaOfTheFirstFigure - areaOfTheSecondFigure}");
                }
                else if (areaOfTheFirstFigure < areaOfTheSecondFigure)
                {
                    Console.WriteLine($"Вторая фигура больше первой на {areaOfTheSecondFigure - areaOfTheFirstFigure}");
                }
                else
                {
                    Console.WriteLine("Фигуры равны");
                }
            }
        }
    }
    abstract class Figure
    {
        public abstract double GetArea();
        public abstract double[,] GetCoordinates();
    }
    class Triangle : Figure
    {
        public double[,] coordinates;
        public double area;

        public Triangle(double[,] inputCoordinates)
        {
            if (inputCoordinates.GetLength(0) != 3 || inputCoordinates.GetLength(1) != 2)
                throw new ArgumentException("Массив должен быть размером [3,2]");
            coordinates = inputCoordinates;
        }
        public override double GetArea()
        {
            double x1 = coordinates[0, 0], y1 = coordinates[0, 1];
            double x2 = coordinates[1, 0], y2 = coordinates[1, 1];
            double x3 = coordinates[2, 0], y3 = coordinates[2, 1];

            area = 0.5 * Math.Abs(x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            return area;
        }

        public override double[,] GetCoordinates()
        {
            return coordinates;
        }
    }
    class Rectangle : Figure
    {
        public double[,] coordinates;
        public double area;

        public Rectangle(double[,] inputCoordinates)
        {
            if (inputCoordinates.GetLength(0) != 4 || inputCoordinates.GetLength(1) != 2)
                throw new ArgumentException("Массив должен быть размером [4,2]");
            coordinates = inputCoordinates;
        }
        public override double GetArea()
        {
            double length = Math.Abs(coordinates[1, 0] - coordinates[0, 0]);
            double width = Math.Abs(coordinates[3, 1] - coordinates[0, 1]);
            area = length * width;
            return area;
        }
        public override double[,] GetCoordinates()
        {
            return coordinates;
        }

    }
    abstract class PropertiesOfFigures
    {
        public abstract int IsValid();
    }
    class Check : PropertiesOfFigures
    {
        public List<Figure> Figures { get; }
        public int linkFigure = 0;

        public Check(List<Figure> figures)
        {
            Figures = figures;
        }

        public override int IsValid()
        {
            for (int i = 0; i < Figures.Count; i++)
            {
                for (int j = i + 1; j < Figures.Count; j++)
                {
                    if (AreFiguresConnected(Figures[i], Figures[j]))
                    {
                        linkFigure++;
                    }
                }
            }
            return linkFigure;
        }

        private bool AreFiguresConnected(Figure figure1, Figure figure2)
        {
            double[,] coords1 = figure1.GetCoordinates();
            double[,] coords2 = figure2.GetCoordinates();

            for (int i = 0; i < coords1.GetLength(0); i++)
            {
                for (int j = 0; j < coords2.GetLength(0); j++)
                {
                    if (coords1[i, 0] == coords2[j, 0] && coords1[i, 1] == coords2[j, 1])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
