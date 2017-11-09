using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IOCContainer;
using System.Runtime.CompilerServices;

namespace labaratory5
{
    class Program
    {
        static void Main(string[] args)
        {
            Container container = new Container();

            container.RegisterType<IClassA, ClassA>();
            container.RegisterType<IClassB, ClassB>();
            container.RegisterType<IClassC, ClassC>();

            IClassA A = container.Resolve<IClassA>();

            IClassB B = container.Resolve<IClassB>();

            IClassC C = container.Resolve<IClassC>();
            C.ConsoleLog();

            Console.ReadLine();
        }
    }

    public interface IClassA
    {
        void getMessage();
    }

    public interface IClassB
    {
        void getMessage();
    }

    public interface IClassC
    {
        void ConsoleLog();
    }

    public class ClassA : IClassA
    {
        public void getMessage()
        {
            Console.WriteLine("This is Sparta!");
        }
    }

    public class ClassB : IClassB
    {
        public void getMessage()
        {
            Console.WriteLine("This is Betta!");
        }
    }

    public class ClassC : IClassC
    {
        [IOCContainer.DependencyAttributeContainer]
        public readonly IClassA a;
        public readonly IClassB b;

        [IOCContainer.DependencyAttributeContainer]
        public ClassC(IClassB b)
        {
            this.b = b;
        }



        public void ConsoleLog()
        {
            a.getMessage();
            b.getMessage();
        }
    }

}
