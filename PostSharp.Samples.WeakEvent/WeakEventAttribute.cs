using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace PostSharp.Samples.WeakEvent
{
    /// <summary>
    ///     Aspect that, when applied to an event, prevents the target event from holding a strong reference to event handlers.
    ///     Therefore, the aspect prevents the event to prevent clients to be garbage collected.
    ///     Client classes must have the <see cref="WeakEventClientAttribute" /> aspect, otherwise an exception will be thrown
    ///     at runtime (if <see cref="AllowStrongReferences" /> is set to <c>true</c>), or a
    ///     strong reference will be created.
    /// </summary>
    [PSerializable]
    [LinesOfCodeAvoided(6)]
    [WeakEventValidation]
    public sealed class WeakEventAttribute : EventInterceptionAspect, IInstanceScopedAspect
    {
        [PNonSerialized] private WeakEventHandler weakEventHandler;


        /// <summary>
        ///     Initializes a new <see cref="WeakEventAttribute" />
        /// </summary>
        /// <param name="allowStrongReferences">
        ///     Determines whether the current aspect is allowed to create a strong reference if the client does not have
        ///     the <see cref="WeakEventClientAttribute" /> aspect. The default value is <c>false</c>.
        /// </param>
        public WeakEventAttribute(bool allowStrongReferences = false)
        {
            AllowStrongReferences = allowStrongReferences;
        }

        /// <summary>
        ///     Determines whether the current aspect is allowed to create a strong reference if the client does not have
        ///     the <see cref="WeakEventClientAttribute" /> aspect.
        /// </summary>
        public bool AllowStrongReferences { get; set; }


        /// <summary>
        ///     Creates an instance of the aspect for a specific instance of the target class (when the target event is not
        ///     static). A call of this method is followed by a call of RuntimeInitializeInstance.
        /// </summary>
        object IInstanceScopedAspect.CreateInstance(AdviceArgs adviceArgs)
        {
            return MemberwiseClone();
        }

        /// <summary>
        ///     Initializes the current instance of the aspect (when the target event is not static).
        /// </summary>
        void IInstanceScopedAspect.RuntimeInitializeInstance()
        {
            weakEventHandler.Initialize();
        }

        /// <summary>
        ///     Initializes the current instance of the aspect (whether target event is static or not).
        /// </summary>
        public override void RuntimeInitialize(EventInfo eventInfo)
        {
            if (eventInfo.AddMethod.IsStatic)
            {
                weakEventHandler.Initialize();
            }
        }

        /// <summary>
        ///     Method invoked when (instead of) a new handler is added to the target event.
        /// </summary>
        /// <param name="args">Context information.</param>
        public override void OnAddHandler(EventInterceptionArgs args)
        {
            var weakEventClient = args.Handler.Target as IWeakEventClient;
            var supportsWeakReference = weakEventClient != null;

            // Throw an exception if the client does not support weak events and we are not allowed to hold strong references.
            if (!supportsWeakReference && args.Handler.Target != null && !AllowStrongReferences)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Attempt to add a reference to the weak event {0} from type {1}, which does not implement the IWeakEventClient interface.",
                        args.Event, args.Handler.Target.GetType()));
            }

            // Add the handler to our own list.
            if (weakEventHandler.AddHandler(args.Handler, supportsWeakReference))
            {
                // If it is the first handler we are adding, add a fake handler to ourselves to the target event.
                // Whichever handler will add here will be passed to OnInvokeHandler, so it is safe and convenient to pass null.
                args.AddHandler(null);
            }

            // Register the handler to the client to prevent garbage collection of the handler.
            if (supportsWeakReference)
            {
                weakEventClient.RegisterEventHandler(args.Handler);
            }
        }

        /// <summary>
        ///     Method invoked when (instead of) a new handler is removed from the target event.
        /// </summary>
        /// <param name="args">Context information.</param>
        public override void OnRemoveHandler(EventInterceptionArgs args)
        {
            var weakEventClient = args.Handler.Target as IWeakEventClient;

            var supportsWeakReference = weakEventClient != null;

            // Remove the handler from our own list.
            if (weakEventHandler.RemoveHandler(args.Handler))
            {
                // If this is the last handler, remove the fake handler to ourselves from the target event.
                args.RemoveHandler(null);
            }

            // Remove the handler from the client.
            if (supportsWeakReference)
            {
                weakEventClient.UnregisterEventHandler(args.Handler);
            }
        }

        /// <summary>
        ///     Method invoked when (instead of) a the target event is raised.
        /// </summary>
        /// <param name="args">Context information.</param>
        public override void OnInvokeHandler(EventInterceptionArgs args)
        {
            // Note that args.Handler == null because it's the value we added in OnAddHandler, but it really does not matter.

            weakEventHandler.InvokeHandler(args.Arguments.ToArray());
        }

    }
}