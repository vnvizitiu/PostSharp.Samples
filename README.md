# PostSharp.Samples

## List of examples

| Example                                   | Description                                               | Demonstrated PostSharp features                                                           |
| :---------------------------------------- | :-------------------------------------------------------- | :--------------------------------------------------------------------------------------- |
| [PostSharp.Samples.CustomLogging](PostSharp.Samples.CustomLogging/README.md)           | Logs method calls including parameter values.             | Simple features of OnMethodBoundaryAspect, LocationInterceptionAspect.                   |
| [PostSharp.Samples.CustomCaching](PostSharp.Samples.CustomCaching/README.md)           | Caches the results of methods calls                       | OnMethodBoundaryAspect: FlowBehavior, MethodExecutionTag.                                |
| [PostSharp.Samples.ExceptionHandling](PostSharp.Samples.ExceptionHandling/README.md)       | Add parameter values to call stack in exception details. Report and then swallow exceptions in entry points.  | OnExceptionAspect including FlowBehavior.  |
| [PostSharp.Samples.AutoRetry](PostSharp.Samples.AutoRetry/README.md)               | Automatically retries a method call when it fails.        | MethodInterceptionAspect                                                                 |
| [PostSharp.Samples.WeakEvent](PostSharp.Samples.WeakEvent/README.md)               | Prevents memory leaks due to events.                      | EventInterceptionAspect, IInstanceScopedAspect, InstanceLevelAspect, IntroduceInterface  |
| [PostSharp.Samples.ValidateResourceString](PostSharp.Samples.ValidateResourceString/README.md)    | Prints a build-time warning when incorrect resource string name is passed to parameter.      | ReferentialConstraint, ReflectionSearch, SyntaxTreeVisitor |
| [PostSharp.Samples.SessionState](PostSharp.Samples.SessionState/README.md)            | Stores a field or property in the session state or page view state.       | LocationInterceptionAspect, IInstanceScopedAspect, ImportMember           |
| [PostSharp.Samples.Xaml](PostSharp.Samples.Xaml/README.md)                    | Demonstrates a few ready-made aspects that are useful for XAML | NotifyPropertyChanged, Recordable, Code Contracts, Background, ReaderWriterSynchronized     |
| [PostSharp.Samples.Transactions](PostSharp.Samples.Transactions/README.md)            | Automatically executes a method inside a transaction. | OnMethodBoundaryAspect : MethodExecutionTag                                              |
| [PostSharp.Samples.Profiling](PostSharp.Samples.Profiling/README.md)               | Measure different execution times of methods, including async methods. | OnMethodBoundaryAspect : async methods, MethodExecutionTag 
| [PostSharp.Samples.Encryption](PostSharp.Samples.Encryption/README.md)              | Automatically encrypts and decrypts parameter and fields/properties | IAspectProvider, MethodInterceptionAspect, IAdviceProvider, field imports |
| [PostSharp.Samples.Threading.PingPong](PostSharp.Samples.Threading.PingPong/README.md)      | The classic educational ping-pong example.                  | Actor |
| [PostSharp.Samples.Threading.ThreadDispatching](PostSharp.Samples.Threading.ThreadDispatching/README.md) | A simple WPF progress bar updated from a background thread. | Background, Dispatched |
| [PostSharp.Samples.MiniProfiler](PostSharp.Samples.MiniProfiler/README.md)            | Measures method execution time with MiniProfiler of StackExchange. | OnMethodBoundaryAspect, MulticastAttribute. |
| [PostSharp.Samples.Persistence](PostSharp.Samples.Persistence/README.md)             | Persists fields or properties into the Windows registry or `app.config`. | LocationInterceptionAspect |
| [PostSharp.Samples.AutoDataContract](PostSharp.Samples.AutoDataContract/README.md)        | Automatically adds `[DataContract]` and `[DataMember]` attributes to derived classes and all properties | IAspectProvider, CustomAttributeIntroductionAspect, aspect inheritance. |


## How to use these examples

* To **browse online**, use the samples browser at <http://samples.postsharp.net/>.
* To **download**, go to <https://www.github.com/postsharp/PostSharp.Samples>.



 