using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    interface IContainer
    {
        void RegisterType<I, C>(bool flag);

        T Resolve<T>();
    }
}
