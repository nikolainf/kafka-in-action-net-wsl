// See https://aka.ms/new-console-template for more information
using ConsoleApp1;

Console.WriteLine("Hello, World!");


DataBaseContext ctx = new();

var people = ctx.GetAllPeople();
