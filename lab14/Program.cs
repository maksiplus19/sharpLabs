﻿// «Построитель СДНФ/СКНФ». С использованием стандартных коллекций реализовать программу, которая для любой
// булевой формулы строит СДНФ и СКНФ. Программа должна не только выводить ответ, но и выводить трассировку своей
// работы. При этом входными данными программы является текстовый файл, в котором находится произвольное число
// булевых формул. Результатом работы программы является файл с СДНФ и СКНФ для каждой формулы. Срок сдачи до 30 мая.

using System;
using System.Linq;
using System.Text;

namespace lab14
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Operation.Tracing += Console.WriteLine;
            // var op = new Operation("a+b");
            // var oOp = new Operation("a|b&c");
            //
            // var opCollection = new Operations {op, oOp};
            // opCollection.Save("C:/Users/viktor/RiderProjects/Labs/lab14/test.txt");
            //
            // var loadedCollection = Operations.Load("C:/Users/viktor/RiderProjects/Labs/lab14/test.txt");
            // foreach (Operation operation in loadedCollection)
            // {
            //     Console.WriteLine(operation);
            //     Console.WriteLine(operation.PCNF());
            //     Console.WriteLine(operation.PDNF());
            // }
            
            var t = new Test();
            t.Save();

            Console.WriteLine(Test.Load().ToString());

            // var table = op.CalculateTable();
            // foreach (var bools in table)
            // {
            //     foreach (var b in bools)
            //     {
            //         Console.Write(b ? "1 " : "0 ");
            //     }
            //
            //     Console.WriteLine();
            // }
        }

        public static void Tracing(string text) => Console.WriteLine(text);
    }
}