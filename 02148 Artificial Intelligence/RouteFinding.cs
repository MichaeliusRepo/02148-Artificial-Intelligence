using System;
using System.Collections.Generic;
using System.Drawing;

namespace _02148_Artificial_Intelligence
{
    public class RouteFinding
    {
        private static Dictionary<Point, CPHNode> Map = new Dictionary<Point, CPHNode>();

        public static void Run(string Path)
        {
            Map.Clear();
            var inputStreets = new List<string>();
            for (int i = 0; i < 4; i++)
            {
                if (i < 2)
                    Console.WriteLine("Type in start street " + (i + 1));
                else
                    Console.WriteLine("Type in goal street " + (i - 1));
                inputStreets.Add(Console.ReadLine());
            }
            var StreetInputTable = new List<List<Point>>();
            for (int i = 0; i < 4; i++)
                StreetInputTable.Add(new List<Point>());
            var file = new System.IO.StreamReader(Path);
            string line;

            while ((line = file.ReadLine()) != null)
            {
                string[] elements = line.Split(' ');
                string streetName = elements[2];

                Point[] points = { new Point(int.Parse(elements[0]), int.Parse(elements[1])), new Point(int.Parse(elements[3]), int.Parse(elements[4])) };
                foreach (Point p in points)
                    if (!Map.ContainsKey(p))
                        Map.Add(p, new CPHNode(p));
                Map[points[0]].AddEdge(Map[points[1]], streetName);

                for (int i = 0; i < 4; i++)
                    if (streetName.Equals(inputStreets[i]))
                    {
                        if (!StreetInputTable[i].Contains(points[0]))
                            StreetInputTable[i].Add(points[0]);
                        if (!StreetInputTable[i].Contains(points[1]))
                            StreetInputTable[i].Add(points[1]);
                    }
            }
            CPHNode start = FindIntersection(StreetInputTable[0], StreetInputTable[1]);
            CPHNode goal = FindIntersection(StreetInputTable[2], StreetInputTable[3]);
            if (start == null) { Console.WriteLine("Could not find start point."); return; }
            if (goal == null) { Console.WriteLine("Could not find end point."); return; }

            var path = AStar.Run(start, goal);
            for (int i = 1; i < path.Count; i++)
            { // Print path
                start = path[i - 1] as CPHNode;
                goal = path[i] as CPHNode;
                int index = start.Edges.IndexOf(goal);
                Console.WriteLine("From " + start.Point + " go through " + start.StreetNames[index] + " to " + goal.Point);
            }
        }

        private static CPHNode FindIntersection(List<Point> list1, List<Point> list2)
        {
            foreach (Point p in list1)
                if (list2.Contains(p))
                    return Map[p];
            return null;
        }

        public class CPHNode : AStar.Node, AStar.INode
        {
            public List<string> StreetNames = new List<string>();
            public Point Point { get; private set; }
            public CPHNode(Point p) { Point = p; }

            public void AddEdge(CPHNode n, string name)
            {
                Edges.Add(n);
                StreetNames.Add(name);
            }

            public override double CalculateCosts(AStar.Node n)
            {
                var node = n as CPHNode;
                double xDiff = node.Point.X - Point.X;
                double yDiff = node.Point.Y - Point.Y;
                return Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
            }

            public override bool IsGoalState(AStar.Node n) { return Equals(n); }
            public override void SetFScore(double i) { FScore = i; }
            public override void SetGScore(double i) { GScore = i; }
        }
    }
}
