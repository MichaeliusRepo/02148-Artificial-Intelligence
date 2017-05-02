using System;

namespace _02148_Artificial_Intelligence
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n Type 0 for CPH Map \n Type 1 for Manhatten Map \n Type 2 for Inference Engine");
                string input = Console.ReadLine();
                if (input == "0")
                    RouteFinding.Run(@"Data\DataCPH.txt");
                else if (input == "1")
                    RouteFinding.Run(@"Data\DataManhatten.txt");
                else if (input == "2")
                    InferenceEngine.Run();
            }
        }
    }
}
