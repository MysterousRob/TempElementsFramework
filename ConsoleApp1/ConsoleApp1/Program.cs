using System;
using System.Collections.Generic;
using TempElementsLib;
using TempElementsLib.Models;

namespace TempElementsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var historyManager = new TempElementsList())
            {
                Stack<TempTxtFile> historyStack = new Stack<TempTxtFile>();

                Console.WriteLine("--- Simulating Image Editor History ---");

                var state1 = historyManager.AddElement<TempTxtFile>();
                state1.WriteLine("State 1: Original Image");
                historyStack.Push(state1);

                var state2 = historyManager.AddElement<TempTxtFile>();
                state2.WriteLine("State 2: Filter Applied");
                historyStack.Push(state2);

                Console.WriteLine($"Current History Count: {historyStack.Count}");
                Console.WriteLine($"Latest State Content: {historyStack.Peek().ReadAllText().Trim()}");

                Console.WriteLine("\n--- User clicks 'Undo' ---");
                var undoneState = historyStack.Pop();

                historyManager.DeleteElement(undoneState);

                Console.WriteLine($"New History Count: {historyStack.Count}");
            }

            Console.WriteLine("\nApplication closed. All temporary history files have been wiped.");
        }
    }
}