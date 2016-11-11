using PostSharp.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using PostSharp.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Reflection.Syntax;

namespace PostSharp.Samples.WeakEvent
{
    [MulticastAttributeUsage(MulticastTargets.Class)]
    class WeakEventValidation : ReferentialConstraint
    {
        public override void ValidateCode(object target, Assembly assembly)
        {
            var reflectionService = PostSharpEnvironment.CurrentProject.GetService<ISyntaxReflectionService>();

            // Iterate through all instances of WeakEventAttribute custom attribute. 
            var customAttributes = ReflectionSearch.GetCustomAttributesOfType(typeof(WeakEventAttribute));

            foreach ( var customAttribute in customAttributes )
            {
                // If the aspect allows for strong references, we don't need to validate anything.
                if (((WeakEventAttribute)customAttribute.Attribute).AllowStrongReferences)
                    continue;


                // Iterate through all methods that add an event handler to the event.
                var targetEvent = (EventInfo)customAttribute.Target;

                foreach ( var usage in ReflectionSearch.GetMethodsUsingDeclaration( targetEvent.AddMethod ) )
                {
                    // Decompile the method body into an expression tree and visit it.
                    var methodBody = reflectionService.GetMethodBody(usage.UsingMethod, SyntaxAbstractionLevel.ExpressionTree);
                    var visitor = new Visitor(targetEvent);
                    visitor.VisitMethodBody(methodBody);
                }
            }
        }

        class Visitor : SyntaxTreeVisitor
        {
            EventInfo weakEvent;
            static IAspectRepositoryService aspectRepositoryService = PostSharpEnvironment.CurrentProject.GetService<IAspectRepositoryService>();

            public Visitor(EventInfo weakEvent)
            {
                this.weakEvent = weakEvent;
            }

            public override object VisitMethodCallExpression(IMethodCallExpression expression)
            {
                if ( expression.Method == weakEvent.AddMethod )
                {
                    var newObjectExpression = expression.Arguments[0] as INewObjectExpression;
                    if ( newObjectExpression != null )
                    {
                        var methodPointerExpression = newObjectExpression.Arguments[1] as IMethodPointerExpression;
                        if ( methodPointerExpression != null )
                        {
                            var eventClientType = methodPointerExpression.Method.DeclaringType;

                            if (!typeof(IWeakEventClient).IsAssignableFrom(eventClientType) && !HasWeakEventClientAspect(eventClientType))
                            {
                                Message.Write(eventClientType, SeverityType.Error, "WEAKEVENT", "The type {0} should have the [WeakEventClient] aspect.", eventClientType);
                            }

                        }
                    }
                }

                return base.VisitMethodCallExpression(expression);
            }

            private static bool HasWeakEventClientAspect( Type type )
            {
                // Check if the current type has the aspect.
                if (aspectRepositoryService.HasAspect(type, typeof(WeakEventClientAttribute)))
                {
                    return true;
                }

                // Check if the base type has the aspect.
                if ( type.BaseType != null )
                {
                    return HasWeakEventClientAspect(type.BaseType.IsGenericType ? type.BaseType.GetGenericTypeDefinition() : type.BaseType);
                }

                return false;
            }
        }
    }
}
