using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Service.TestInterView
{
    public class InterView : IInterView
    {

        public InterView() { }

        public async Task<string> RefFunction(int refOutValue)
        {
            int req = 10;
            var refResult = Ref(ref req);
            //var outResult = Out(out refOutValue);
            return refResult;
        }
        public string Ref(ref int id)
        {

            string returnText = "Next-" + id.ToString();
            id = id + 1;
            return returnText;
        }

        public async Task<string> OutFunction(int refOutValue)
        {
            int req = 10;

            var outResult = Out(out req);
            return outResult;
        }
        public string Out(out int idd)
        {
            idd = 0;
            string returnText = "Next-" + idd.ToString();
            idd += 10;
            return returnText;
        }

        public async Task<string> ReverseString(string str)
        {

            // string str = "Educative";
            char[] stringArray = str.ToCharArray();
            Console.WriteLine(stringArray);
            Array.Reverse(stringArray);
            string reversedStr = new string(stringArray);
            return reversedStr;
        }
    }

    #region sealed class
    //sealed class can not be inherted
    //sealed class BaseClass
    //{
    //    public string Out()
    //    {

    //        return "test";
    //    }
    //}
    //public class DrivedClass : BaseClass
    //{
    //    public string Out()
    //    {

    //        return "test";
    //    }
    //}
    #endregion sealed class

    #region compile time polymorphism 
    public class TestData
    {
        public int Add(int a, int b, int c)
        {
            return a + b + c;
        }
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
    #endregion  compile time polymorphism 

    #region run time polymorphism 
    public class Drawing
    {
        public virtual double Area()
        {
            return 0;
        }
    }

    public class Circle : Drawing
    {
        public double Radius { get; set; }
        public Circle()
        {
            Radius = 5;
        }
        public override double Area()
        {
            return (3.14) * Math.Pow(Radius, 2);
        }
    }

    public class Square : Drawing
    {
        public double Length { get; set; }
        public Square()
        {
            Length = 6;
        }
        public override double Area()
        {
            return Math.Pow(Length, 2);
        }
    }
    #endregion  run time polymorphism 
}
