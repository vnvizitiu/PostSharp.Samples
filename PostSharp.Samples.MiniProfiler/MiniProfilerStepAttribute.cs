﻿using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using PostSharp.Serialization;
using StackExchange.Profiling;

namespace PostSharp.Samples.MiniProfiler
{
    [PSerializable]
    [LinesOfCodeAvoided(2)]
    public sealed class MiniProfilerStepAttribute : OnMethodBoundaryAspect
    {
        private string methodName;

        public MiniProfilerStepAttribute()
        {
            this.ApplyToStateMachine = true;
        }

        public override bool CompileTimeValidate(MethodBase method)
        {
            // Don't apply the aspect to constructors, property getters, and so on.
            if (method.IsSpecialName)
                return false;

            return true;
        }

        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this.methodName = method.DeclaringType.Name + "." + method.Name;
        }


        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = StackExchange.Profiling.MiniProfiler.Current?.Step(this.methodName);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            ((IDisposable)args.MethodExecutionTag)?.Dispose();
        }

    }
}
