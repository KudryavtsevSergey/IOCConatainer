using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    internal class InstanceCreationService
    {
        static InstanceCreationService instance = null;

        static InstanceCreationService()
        {
            instance = new InstanceCreationService();
        }

        private InstanceCreationService()
        { }

        public static InstanceCreationService GetInstance()
        {
            return instance;
        }

        public object GetNewObject(Type t, object[] arguments = null)
        {
            object obj = null;

            try
            {
                obj = Activator.CreateInstance(t, arguments);
            }
            catch
            {

            }

            return obj;
        }
    }
}
