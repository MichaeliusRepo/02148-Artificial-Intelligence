using System;
using System.Collections.Generic;

namespace _02148_Artificial_Intelligence
{
    public static class InferenceEngine
    {
        private static List<Clause> KnowledgeBase = new List<Clause>();

        public static void Run()
        {
            KnowledgeBase.Clear();
            Console.WriteLine("Not Alpha is retrieved from the first clause written. Enter KB below:");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "") break;
                string[] split = input.Split(' ');
                Clause c = new Clause();
                bool neg = false;
                foreach (string s in split)
                    if (s == "if")
                        neg = true;
                    else if (neg)
                        c.NegativeLiterals.Add(s);
                    else
                        c.PositiveLiterals.Add(s);
                KnowledgeBase.Add(c);
                foreach (string s in c.PositiveLiterals)
                    if (c.NegativeLiterals.Contains(s))
                        KnowledgeBase.Remove(c);
            }
            var NotAlpha = new Clause();
            NotAlpha.NegativeLiterals.Add(KnowledgeBase[0].PositiveLiterals[0]);
            var result = AStar.Run(new ResolutionNode(NotAlpha), new ResolutionNode(new Clause()));
            if (result == null)
                Console.WriteLine("Could not be solved.");
            else
                foreach (ResolutionNode n in result)
                    Console.WriteLine(n.ToString());
        }

        internal class ResolutionNode : AStar.Node, AStar.INode
        {
            Clause Clause;

            internal ResolutionNode(Clause Clause) { this.Clause = Clause; }
            public override double CalculateCosts(AStar.Node n) { return 0; } // It costs nothing to move.
            public override bool IsGoalState(AStar.Node n) { return Clause.GetLength() == 0; }
            public override void SetFScore(double i) { FScore = Clause.GetLength(); }

            public override void SetGScore(double i)
            {
                if (Clause.NegativeLiterals.Count != 0)
                    for (int j = 0; j < KnowledgeBase.Count; j++)
                        if (KnowledgeBase[j].PositiveLiterals.Contains(Clause.NegativeLiterals[0]))
                            Edges.Add(new ResolutionNode(KnowledgeBase[j].Combine(Clause)));
            }

            public override string ToString() { return Clause.ToString(); }
        }

        internal class Clause
        {
            internal List<string> PositiveLiterals = new List<string>();
            internal List<string> NegativeLiterals = new List<string>();
            internal string CombinedHow = string.Empty;

            internal int GetLength() { return PositiveLiterals.Count + NegativeLiterals.Count; }

            internal Clause Combine(Clause otherClause)
            {
                Clause combinedClause = new Clause()
                {
                    PositiveLiterals = new List<string>(otherClause.PositiveLiterals),
                    NegativeLiterals = new List<string>(otherClause.NegativeLiterals)
                };
                foreach (string s in PositiveLiterals)
                    if (otherClause.NegativeLiterals.Contains(s))
                        combinedClause.NegativeLiterals.Remove(s);
                    else if (!combinedClause.PositiveLiterals.Contains(s))
                        combinedClause.PositiveLiterals.Add(s);
                foreach (string s in NegativeLiterals)
                    if (otherClause.PositiveLiterals.Contains(s))
                        combinedClause.PositiveLiterals.Remove(s);
                    else if (!combinedClause.NegativeLiterals.Contains(s))
                        combinedClause.NegativeLiterals.Add(s);
                combinedClause.CombinedHow = combinedClause + "    [using rule " + this + "]";
                KnowledgeBase.Add(combinedClause);
                return combinedClause;
            }

            public override string ToString()
            {
                if (CombinedHow.Equals(string.Empty))
                    return String.Join(" ", PositiveLiterals) + " < " + String.Join(" ", NegativeLiterals);
                else return CombinedHow;
            }
        }
    }
}
