// See https://aka.ms/new-console-template for more information
using ConsoleApp3;
using System.Linq.Expressions;

Dictionary<int, string> map = new()
{
    {1, "one" },
    {2, "two" }
};

foreach (var key in map)
{
    Console.WriteLine(key.Key + "=" + map[key.Key]);
}