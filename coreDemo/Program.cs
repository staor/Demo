
using System;
using Utils;

namespace coreDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Console.WriteLine("Hello World!");
            Console.WriteLine("你好!");
            try
            {
                int a = 0;
                int b = 10 / a;
            }
            catch (Exception ex)
            {
                UtilesHelper.Error(ex);
                //throw;
            }
        }
    }
}
