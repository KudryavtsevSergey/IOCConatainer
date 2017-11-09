using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IOCContainer
{
    public class Container : IContainer
    {
        Dictionary<Type, RegistrationModel> instanceRegistry = new Dictionary<Type, RegistrationModel>();

        public void RegisterType<I, C>(bool flag = false)
        {
            if (instanceRegistry.ContainsKey(typeof(I)) == true)
            {
                instanceRegistry.Remove(typeof(I));
            }

            instanceRegistry.Add(
                typeof(I),
                    new RegistrationModel
                    {
                        Flag = flag,
                        ObjectType = typeof(C)
                    }
                );
        }

        public I Resolve<I>()
        {
            return (I)Resolve(typeof(I));
        }

        private object Resolve(Type t)
        {
            object obj = null;

            if (instanceRegistry.ContainsKey(t) == true)
            {
                RegistrationModel model = instanceRegistry[t];

                if (model != null)
                {
                    Type typeToCreate = model.ObjectType;

                    ConstructorInfo[] consInfo = typeToCreate.GetConstructors();

                    var dependentCtor = consInfo.FirstOrDefault(
                        item => item.CustomAttributes.FirstOrDefault(
                            att => att.AttributeType == typeof(DependencyAttributeContainer)
                            ) != null
                            );

                    if (dependentCtor == null)
                    {
                        // use the default constructor to create
                        obj = CreateInstance(model);
                        
                    }
                    else
                    {
                        // We found a constructor with dependency attribute
                        ParameterInfo[] parameters = dependentCtor.GetParameters();
                        if (parameters.Count() == 0)
                        {
                            // Futile dependency attribute, use the default constructor only
                            obj = CreateInstance(model);

                        }
                        else
                        {
                            // valid dependency attribute, lets create the dependencies first and pass them in constructor
                            List<object> arguments = new List<object>();
                            foreach (var param in parameters)
                            {
                                Type type = param.ParameterType;
                                arguments.Add(this.Resolve(type));
                            }

                            obj = CreateInstance(model, arguments.ToArray());

                        }
                    }

                }
            }
            return obj;
        }

        private void SetterInject(Type type, object obj)
        {
            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fieldInfo)
            {
                var attributes = field.GetCustomAttributes();

                foreach (DependencyAttributeContainer atribute in attributes)
                {
                    Type typeOfField = field.FieldType;
                    field.SetValue(obj, Resolve(typeOfField));
                }
            }
        }

        private object CreateInstance(RegistrationModel model, object[] arguments = null)
        {
            object returnedObj = null;
            Type typeToCreate = model.ObjectType;

            returnedObj = model.Flag ? SingletonCreationService.GetInstance().GetSingleton(typeToCreate, arguments): InstanceCreationService.GetInstance().GetNewObject(typeToCreate, arguments);

            SetterInject(typeToCreate, returnedObj);

            return returnedObj;
        }
    }
}
