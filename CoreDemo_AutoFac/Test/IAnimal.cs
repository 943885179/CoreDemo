using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_AutoFac.Test
{
    public interface IAnimal
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        int GetArg(int arg);
    }
    public class Dog : IAnimal
    {
        public string Name {
            get { return "Dog"; }
        }

        public int GetArg(int arg)
        {
            return arg;
        }
    }
    public class Cat : IAnimal
    {
        public string Name
        {
            get { return "Cat"; }
        }
        public int GetArg(int arg)
        {
            return arg;
        }
    }

}
