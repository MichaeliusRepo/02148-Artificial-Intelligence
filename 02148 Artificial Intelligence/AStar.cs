using System.Collections.Generic;

namespace _02148_Artificial_Intelligence
{
    public class AStar
    {
        public static List<Node> Run(Node start, Node goal)
        {
            var ClosedSet = new List<Node>();
            var OpenSet = new List<Node>() { start };
            var CameFrom = new Dictionary<Node, Node>();
            start.SetGScore(0);
            start.SetFScore(start.CalculateCosts(goal));

            while (OpenSet.Count != 0)
            {
                Node current = OpenSet[0];
                foreach (Node n in OpenSet)
                    if (n.FScore < current.FScore)
                        current = n;
                if (current.IsGoalState(goal))
                    return ReconstructPath(CameFrom, current);

                OpenSet.Remove(current);
                ClosedSet.Add(current);

                foreach (Node neighbor in current.Edges)
                {
                    if (ClosedSet.Contains(neighbor))
                        continue;
                    double tentative_GScore = current.GScore + current.CalculateCosts(neighbor);
                    if (!OpenSet.Contains(neighbor))
                        OpenSet.Add(neighbor);
                    else if (tentative_GScore >= neighbor.GScore)
                        continue;
                    CameFrom.Add(neighbor, current);
                    neighbor.SetGScore(tentative_GScore);
                    neighbor.SetFScore(neighbor.GScore + neighbor.CalculateCosts(goal));
                }
            }
            return null;
        }

        private static List<Node> ReconstructPath(Dictionary<Node, Node> CameFrom, Node current)
        {
            var TotalPath = new List<Node>() { current };
            while (CameFrom.ContainsKey(current))
            {
                current = CameFrom[current];
                TotalPath.Add(current);
            }
            TotalPath.Reverse();
            return TotalPath;
        }

        public interface INode
        {
            double CalculateCosts(Node n);
            bool IsGoalState(Node n);
            void SetFScore(double i);
            void SetGScore(double i);
        }

        public abstract class Node : INode
        {
            public List<Node> Edges = new List<Node>();
            public double FScore = 0;
            public double GScore = 0;

            public abstract double CalculateCosts(Node n);
            public abstract bool IsGoalState(Node n);
            public abstract void SetFScore(double i);
            public abstract void SetGScore(double i);
        }

    }
}
