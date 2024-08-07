using System;
using System.Collections.Generic;
using System.Linq;

namespace FigureTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double areaOfTheFirstFigure = 0;
            double areaOfTheSecondFigure = 0;


            double[,] triangleCoordinates11 = { { 0, 0 }, { 4, 0 }, { 2, 3 } };
            double[,] triangleCoordinates12 = { { 2, 1 }, { 6, 1 }, { 4, 4 } };
            double[,] rectangleCoordinates11 = { { 1, 1 }, { 3, 1 }, { 3, 3 }, { 1, 3 } };

            Figure triangle11 = new Triangle(triangleCoordinates11);
            Figure triangle12 = new Triangle(triangleCoordinates12);
            Figure rectangle11 = new Rectangle(rectangleCoordinates11);

            var Figure1 = new Check(new List<Figure>() { triangle11, triangle12, rectangle11 });


            double[,] rectangleCoordinates21 = { { 2, 2 }, { 5, 2 }, { 5, 4 }, { 2, 4 } };
            double[,] rectangleCoordinates22 = { { 3, 3 }, { 6, 3 }, { 6, 5 }, { 3, 5 } };
            double[,] triangleCoordinates21 = { { 4, 0 }, { 8, 0 }, { 6, 3 } };

            Figure rectangle21 = new Rectangle(rectangleCoordinates21);
            Figure rectangle22 = new Rectangle(rectangleCoordinates22);
            Figure triangle21 = new Triangle(triangleCoordinates21);

            var Figure2 = new Check(new List<Figure>() { rectangle21, rectangle22, triangle21 });

            double[,] triangleCoordinates31 = { { 1, 1 }, { 5, 1 }, { 3, 4 } };
            Figure triangle31 = new Triangle(triangleCoordinates31);

            var Figure3 = new Check(new List<Figure>() { triangle31 });

            var intersectionArea1and2 = GetIntersectionArea(Figure1.Figures, Figure2.Figures);
            var intersectionArea1and3 = GetIntersectionArea(Figure1.Figures, Figure3.Figures);
            var intersectionArea2and3 = GetIntersectionArea(Figure2.Figures, Figure3.Figures);

            var unionArea1and2 = GetUnionArea(Figure1.Figures, Figure2.Figures);
            var unionArea1and3 = GetUnionArea(Figure1.Figures, Figure3.Figures);
            var unionArea2and3 = GetUnionArea(Figure2.Figures, Figure3.Figures);

            Console.WriteLine($"Область пересечения Figure1 и Figure2: {intersectionArea1and2}");
            Console.WriteLine($"Область пересечения Figure1 и Figure3: {intersectionArea1and3}");
            Console.WriteLine($"Область пересечения Figure2 и Figure3: {intersectionArea2and3}");


            Console.WriteLine($"Область объединения с пересечением Figure1 и Figure2: {unionArea1and2}");
            Console.WriteLine($"Область объединения с пересечением Figure1 и Figure3: {unionArea1and3}");
            Console.WriteLine($"Область объединения с пересечением Figure2 и Figure3: {unionArea2and3}");

            Console.WriteLine($"Область объединения без пересечения Figure1 и Figure2: {unionArea1and2 - intersectionArea1and2}");
            Console.WriteLine($"Область объединения без пересечения Figure1 и Figure3: {unionArea1and3 - intersectionArea1and3}");
            Console.WriteLine($"Область объединения без пересечения Figure2 и Figure3: {unionArea2and3 - intersectionArea2and3}");

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
                    areaOfTheSecondFigure += figure.GetArea();
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

        public static double GetIntersectionArea(List<Figure> figures1, List<Figure> figures2)
        {
            var intersectionPoints = new List<Point>();

            foreach (var figure1 in figures1)
            {
                foreach (var figure2 in figures2)
                {
                    intersectionPoints.AddRange(GetIntersection(figure1.GetPoints(), figure2.GetPoints()));
                }
            }

            return CalculateArea(intersectionPoints);
        }

        public static double GetUnionArea(List<Figure> figures1, List<Figure> figures2)
        {
            var unionPoints = new List<Point>();

            foreach (var figure in figures1)
            {
                unionPoints.AddRange(figure.GetPoints());
            }

            foreach (var figure in figures2)
            {
                unionPoints.AddRange(figure.GetPoints());
            }

            return CalculateArea(unionPoints.Distinct().ToList());
        }

        public static List<Point> GetIntersection(List<Point> polygon1, List<Point> polygon2)
        {
            var intersection = new List<Point>();

            for (int i = 0; i < polygon1.Count; i++)
            {
                int nextI = (i + 1) % polygon1.Count;
                for (int j = 0; j < polygon2.Count; j++)
                {
                    int nextJ = (j + 1) % polygon2.Count;
                    var intersectPoint = GetIntersectionPoint(
                        polygon1[i], polygon1[nextI],
                        polygon2[j], polygon2[nextJ]);
                    if (intersectPoint != null)
                    {
                        intersection.Add(intersectPoint.Value);
                    }
                }
            }

            foreach (var point in polygon1)
            {
                if (IsPointInPolygon(point, polygon2))
                {
                    intersection.Add(point);
                }
            }

            foreach (var point in polygon2)
            {
                if (IsPointInPolygon(point, polygon1))
                {
                    intersection.Add(point);
                }
            }

            return intersection.Distinct().ToList();
        }

        private static Point? GetIntersectionPoint(Point p1, Point p2, Point q1, Point q2)
        {
            double a1 = p2.Y - p1.Y;
            double b1 = p1.X - p2.X;
            double c1 = a1 * p1.X + b1 * p1.Y;

            double a2 = q2.Y - q1.Y;
            double b2 = q1.X - q2.X;
            double c2 = a2 * q1.X + b2 * q1.Y;

            double delta = a1 * b2 - a2 * b1;
            if (Math.Abs(delta) < 1e-10)
            {
                return null;
            }

            double x = (b2 * c1 - b1 * c2) / delta;
            double y = (a1 * c2 - a2 * c1) / delta;

            if (IsPointOnLineSegment(new Point(x, y), p1, p2) && IsPointOnLineSegment(new Point(x, y), q1, q2))
            {
                return new Point(x, y);
            }

            return null;
        }

        private static bool IsPointOnLineSegment(Point p, Point p1, Point p2)
        {
            return p.X >= Math.Min(p1.X, p2.X) && p.X <= Math.Max(p1.X, p2.X)
                   && p.Y >= Math.Min(p1.Y, p2.Y) && p.Y <= Math.Max(p1.Y, p2.Y);
        }

        private static bool IsPointInPolygon(Point point, List<Point> polygon)
        {
            int count = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                int next = (i + 1) % polygon.Count;
                Point p1 = polygon[i];
                Point p2 = polygon[next];

                if (point.Y > Math.Min(p1.Y, p2.Y) && point.Y <= Math.Max(p1.Y, p2.Y)
                    && point.X <= Math.Max(p1.X, p2.X) && p1.Y != p2.Y)
                {
                    double xinters = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                    if (p1.X == p2.X || point.X <= xinters)
                    {
                        count++;
                    }
                }
            }

            return count % 2 != 0;
        }

        private static double CalculateArea(List<Point> points)
        {
            if (points.Count < 3) return 0;

            double area = 0;
            int j = points.Count - 1;

            for (int i = 0; i < points.Count; i++)
            {
                area += (points[j].X + points[i].X) * (points[j].Y - points[i].Y);
                j = i;
            }

            return Math.Abs(area / 2.0);
        }

        public struct Point
        {
            public double X { get; }
            public double Y { get; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }

            public override string ToString() => $"({X}, {Y})";
        }
    }

    abstract class Figure
    {
        public abstract double GetArea();
        public abstract List<Program.Point> GetPoints();
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

        public override List<Program.Point> GetPoints()
        {
            var points = new List<Program.Point>();
            for (int i = 0; i < coordinates.GetLength(0); i++)
            {
                points.Add(new Program.Point(coordinates[i, 0], coordinates[i, 1]));
            }
            return points;
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

        public override List<Program.Point> GetPoints()
        {
            var points = new List<Program.Point>();
            for (int i = 0; i < coordinates.GetLength(0); i++)
            {
                points.Add(new Program.Point(coordinates[i, 0], coordinates[i, 1]));
            }
            return points;
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
            var coords1 = figure1.GetPoints();
            var coords2 = figure2.GetPoints();

            for (int i = 0; i < coords1.Count; i++)
            {
                for (int j = 0; j < coords2.Count; j++)
                {
                    if (coords1[i].X == coords2[j].X && coords1[i].Y == coords2[j].Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}



