using AndreasReitberger.Parser.Gcode;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace GCCode.Net.WorkClasses
{
    public class Parser
    {

        public static Gcode ParseGcode(string testfile)
        {
            //testfile = @"Gcodes\Test.gcode";

            var prog = new Progress<int>(percent =>
            {
                //Debug.WriteLine($"Parsed: {percent}%");
            });


            if (File.Exists(testfile))
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                var gcode = GcodeParser.Instance.FromFileAsync(testfile, null, cts.Token, false).Result;
                if (gcode != null)
                {
                    Debug.WriteLine($"Gcode parsed in: {gcode.ParsingDuration}");
                }
                else
                {
                    Debug.WriteLine($"Parsed gcoe was null: {testfile}");

                }
                return gcode;


            }
            else
            {


                Debug.WriteLine($"{testfile} not found!");
                return null;

            }



        }

        public static (List<Point>, Point) Get_Points(Gcode code)
        {
            if (code is null) return (null, default(Point));

            var points = new List<Point>();


            var point = Point.Zero;

            //Значения максимальных габаритов модели
            var max_point = Point.Zero;
            foreach (var Com1 in code.Commands)
            {
                foreach (var Com2 in Com1)
                {


                    bool is_changed = false;
                    if (!Double.IsInfinity(Com2.X))
                    {
                        point.X = Com2.X;
                        max_point.X = Math.Max(max_point.X, Math.Abs(Com2.X));
                        is_changed = true;
                    }
                    if (!Double.IsInfinity(Com2.Y))
                    {
                        point.Y = Com2.Y;
                        max_point.Y = Math.Max(max_point.Y, Math.Abs(Com2.Y));
                        is_changed = true;
                    }
                    if (!Double.IsInfinity(Com2.Z))
                    {
                        point.Z = Com2.Z;
                        max_point.Z = Math.Max(max_point.Z, Math.Abs(Com2.Z));
                        is_changed = true;
                    }

                    if (is_changed) points.Add(point);


                }



            }




            return (points, max_point);

        }

        public static (List<List<Point>>, Point) GetPointsByLayers(Gcode code)
        {
            if (code is null) return (default(List<List<Point>>), default(Point));

            List<List<Point>> result =  new List<List<Point>>();

            var points = new List<Point>();


            var point = Point.Zero;


            var max_point = Point.Zero;
            foreach (var Com1 in code.Commands)
            {
                foreach (var Com2 in Com1)
                {


                    bool is_changed = false;
                    if (!Double.IsInfinity(Com2.X))
                    {
                        point.X = Com2.X;
                        max_point.X = Math.Max(max_point.X, Math.Abs(Com2.X));
                        is_changed = true;
                    }
                    if (!Double.IsInfinity(Com2.Y))
                    {
                        point.Y = Com2.Y;
                        max_point.Y = Math.Max(max_point.Y, Math.Abs(Com2.Y));
                        is_changed = true;
                    }
                    if (!Double.IsInfinity(Com2.Z))
                    {
                        point.Z = Com2.Z;
                        max_point.Z = Math.Max(max_point.Z, Math.Abs(Com2.Z));
                        is_changed = true;
                    }

                    if (is_changed) points.Add(point);


                }
                result.Add(points);
                points  = new List<Point>();

            }




            return (result, max_point);
        }


        public struct Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }

            public static Point Zero
            {

                get
                {
                    return default(Point);
                }
            }




            public static double GetLenght(Point p1, Point p2)
            {

                double l_x = p2.X - p1.X;
                double l_y = p2.Y - p1.Y;
                double l_z = p2.Z - p1.Z;

                return Math.Sqrt(l_x * l_x + l_y * l_y + l_z * l_z);


            }


        }


    }
}
