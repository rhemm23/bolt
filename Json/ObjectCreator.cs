using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System;

namespace Bolt {

  delegate object ObjectActivator(params object[] args);

  public static class ObjectCreator {
    private static readonly ConcurrentDictionary<Type, ObjectActivator> objectActivators;

    static ObjectCreator() {
      objectActivators = new ConcurrentDictionary<Type, ObjectActivator>();
    }

    public static object NewInstance(Type type, params object[] args) {
      return GetActivator(type)(args);
    }

    private static ObjectActivator GetActivator(Type type) {
      ObjectActivator activator;
      if(!objectActivators.TryGetValue(type, out activator)) {
        activator = BuildActivator(type.GetConstructors()[0]);
        objectActivators.TryAdd(type, activator);
      }
      return activator;
    }

    private static ObjectActivator BuildActivator(ConstructorInfo ctor) {
      ParameterInfo[] paramsInfo = ctor.GetParameters();

      ParameterExpression param = 
        Expression.Parameter(typeof(object[]), "args");

      Expression[] argsExpessions = 
        new Expression[paramsInfo.Length];

      for(int i = 0; i < paramsInfo.Length; i++) {
        Expression index = Expression.Constant(i);
        Type paramType = paramsInfo[i].ParameterType;

        Expression paramAccessor = 
          Expression.ArrayIndex(param, index);

        Expression paramCast =
          Expression.Convert(paramAccessor, paramType);

        argsExpessions[i] = paramCast;
      }

      NewExpression newExpression = 
        Expression.New(ctor, argsExpessions);

      LambdaExpression lambda =
        Expression.Lambda(typeof(ObjectActivator), newExpression, param);

      return (ObjectActivator)lambda.Compile();
    }
  }
}
