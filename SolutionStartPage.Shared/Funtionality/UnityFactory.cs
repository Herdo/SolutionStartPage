namespace SolutionStartPage.Shared.Funtionality
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;

    public static class UnityFactory
    {
        /////////////////////////////////////////////////////////
        #region Fields

        private static readonly IUnityContainer PrivateContainer;
        private static bool _isInitialized;
        private static bool _isConfigured;

        #endregion

        /////////////////////////////////////////////////////////
        #region Constructors

        static UnityFactory()
        {
            PrivateContainer = new UnityContainer();
            _isInitialized = false;
            _isConfigured = false;
        }

        #endregion

        /////////////////////////////////////////////////////////
        #region Public Methods

        #region Util

        /// <summary>
        /// Initializes the factory.
        /// </summary>
        /// <param name="initializeAction">The action to run in the factory to initialize the container.</param>
        public static void Initialize(Action<IUnityContainer> initializeAction)
        {
            if (_isInitialized) return;
            _isInitialized = true;
            initializeAction(PrivateContainer);
        }

        /// <summary>
        /// Configures the factory.
        /// </summary>
        /// <param name="configureAction">The action to run in the factory to configure the container.</param>
        /// <remarks><see cref="Initialize"/> must be executed, before the Factory can be configured.</remarks>
        public static void Configure(Action<IUnityContainer> configureAction)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Cannot configure the UnityFactory, if it's not initialized." +
                                                    "Use 'UnityFactory.Initialize' first.");
            }

            if (_isConfigured) return;
            _isConfigured = true;
            configureAction(PrivateContainer);
        } 

        /// <summary>
        /// Check if a particular type has been registered with the container with the
        /// default name, or the specific name, if set.
        /// </summary>
        /// <typeparam name="T">Type to check registration for.</typeparam>
        /// <param name="nameToCheck">The name to lookup.</param>
        /// <returns>True if this type has been registered, false if not.</returns>
        public static bool IsRegistered<T>(string nameToCheck = null)
        {
            return String.IsNullOrWhiteSpace(nameToCheck)
                ? PrivateContainer.IsRegistered<T>()
                : PrivateContainer.IsRegistered<T>(nameToCheck);
        }

        #endregion

        #region Registration

        /// <summary>
        /// Register a <see cref="Microsoft.Practices.Unity.LifetimeManager"/> for the given type with
        /// the container.  No type mapping is performed for this type.
        /// </summary>
        /// <typeparam name="T">The type to apply the lifetimeManager to.</typeparam>
        /// <param name="lifetimeManager">The <see cref="Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetimeManager
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<T>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return PrivateContainer.RegisterType<T>(lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <typeparam name="TFrom"><see cref="System.Type"/> that will be requested.</typeparam>
        /// <typeparam name="TTo"><see cref="System.Type"/> that will actually be returned.</typeparam>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            return PrivateContainer.RegisterType<TFrom, TTo>(injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will
        /// use the given Microsoft.Practices.Unity.LifetimeManager.
        /// </summary>
        /// <param name="from"><see cref="System.Type"/> that will be requested.</param>
        /// <param name="to"><see cref="System.Type"/> that will actually be returned.</param>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetimeManager">The <see cref="Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetimeManager
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager,
            params InjectionMember[] injectionMembers)
        {
            return PrivateContainer.RegisterType(from, to, name, lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <param name="t">Type of instance to register (may be an implemented interface instead of
        /// the full type).</param>
        /// <param name="name">Name for registration.</param>
        /// <param name="instance">Object to returned.</param>
        /// <param name="lifetimeManager"><see cref="Microsoft.Practices.Unity.LifetimeManager"/> object that controls how this instance
        /// will be managed by the container.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        /// <remarks>Instance registration is much like setting a type as a singleton, except
        /// that instead of the container creating the instance the first time it is
        /// requested, the user creates the instance ahead of type and adds that instance
        /// to the container.</remarks>
        public static IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetimeManager)
        {
            return PrivateContainer.RegisterInstance(t, name, instance, lifetimeManager);
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <typeparam name="T">Type of instance to register (may be an implemented interface instead of
        /// the full type).</typeparam>
        /// <param name="instance">Object to returned.</param>
        /// <returns>The Microsoft.Practices.Unity.UnityContainer object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        /// <remarks>Instance registration is much like setting a type as a singleton, except
        /// that instead of the container creating the instance the first time it is
        /// requested, the user creates the instance ahead of type and adds that instance
        /// to the container.
        /// This overload does a default registration and has the container take over
        /// the lifetime of the instance.</remarks>
        public static IUnityContainer RegisterInstance<T>(T instance)
        {
            return PrivateContainer.RegisterInstance(instance);
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <typeparam name="T">The type to apply the lifetimeManager to.</typeparam>
        /// <param name="instance">Object to returned.</param>
        /// <param name="name">Name for registration.</param>
        /// <param name="lifetimeManager">The <see cref="Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetimeManager
        /// of the returned instance.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterInstance<T>(T instance, string name, LifetimeManager lifetimeManager)
        {
            return PrivateContainer.RegisterInstance(typeof(T), name, instance, lifetimeManager);
        }

        /// <summary>
        /// Register an instance with the container.
        /// </summary>
        /// <typeparam name="T">The type to apply the lifetimeManager to.</typeparam>
        /// <param name="instance">Object to returned.</param>
        /// <param name="lifetimeManager">The <see cref="Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetimeManager
        /// of the returned instance.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterInstance<T>(T instance, LifetimeManager lifetimeManager)
        {
            return PrivateContainer.RegisterInstance(instance, lifetimeManager);
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will
        /// use the given <see cref="Microsoft.Practices.Unity.LifetimeManager"/>.
        /// </summary>
        /// <typeparam name="TFrom"><see cref="System.Type"/> that will be requested.</typeparam>
        /// <typeparam name="TTo"><see cref="System.Type"/> that will actually be returned.</typeparam>
        /// <param name="lifetimeManager">The <see cref="Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetimeManager
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            return PrivateContainer.RegisterType<TFrom, TTo>(lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container.
        /// </summary>
        /// <typeparam name="TFrom"><see cref="System.Type"/> that will be requested.</typeparam>
        /// <typeparam name="TTo"><see cref="System.Type"/> that will actually be returned.</typeparam>
        /// <param name="name">Name of this mapping.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        /// <remarks>This method is used to tell the container that when asked for type <typeparamref name="TFrom"/>,
        /// actually return an instance of type <typeparamref name="TTo"/>. This is very useful for getting
        /// instances of interfaces.</remarks>
        public static IUnityContainer RegisterType<TFrom, TTo>(string name, params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            return PrivateContainer.RegisterType<TFrom, TTo>(name, injectionMembers);
        }

        /// <summary>
        /// Register a type mapping with the container, where the created instances will
        /// use the given <see cref="Microsoft.Practices.Unity.LifetimeManager"/>.
        /// </summary>
        /// <typeparam name="TFrom"><see cref="System.Type"/> that will be requested.</typeparam>
        /// <typeparam name="TTo"><see cref="System.Type"/> that will actually be returned.</typeparam>
        /// <param name="name">Name to use for registration, null if a default registration.</param>
        /// <param name="lifetimeManager">The <see cref="Microsoft.Practices.Unity.LifetimeManager"/> that controls the lifetimeManager
        /// of the returned instance.</param>
        /// <param name="injectionMembers">Injection configuration objects.</param>
        /// <returns>The <see cref="Microsoft.Practices.Unity.UnityContainer"/> object that this method was
        /// called on (this in C#, Me in Visual Basic).</returns>
        public static IUnityContainer RegisterType<TFrom, TTo>(string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            where TTo : TFrom
        {
            return PrivateContainer.RegisterType<TFrom, TTo>(name, lifetimeManager, injectionMembers);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Resolve an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of object to get from the container.</typeparam>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            return PrivateContainer.Resolve<T>(overrides);
        }

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of object to get from the container.</typeparam>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        public static T Resolve<T>(string name, params ResolverOverride[] overrides)
        {
            return PrivateContainer.Resolve<T>(name, overrides);
        }

        /// <summary>
        /// Resolve an instance of the default requested type from the container, if <typeparamref name="T"/> is registered.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of object to get from the container.</typeparam>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object or the default for <typeparamref name="T"/>.</returns>
        public static T ResolveIfRegistered<T>(params ResolverOverride[] overrides)
        {
            var result = default(T);

            if (PrivateContainer.IsRegistered<T>())
                result = PrivateContainer.Resolve<T>(overrides);

            return result;
        }

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container, if <typeparamref name="T"/> is registered.
        /// </summary>
        /// <typeparam name="T"><see cref="System.Type"/> of object to get from the container.</typeparam>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <param name="overrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object or the default for <typeparamref name="T"/>.</returns>
        public static T ResolveIfRegistered<T>(string name, params ResolverOverride[] overrides)
        {
            var result = default(T);

            if (PrivateContainer.IsRegistered<T>(name))
                result = PrivateContainer.Resolve<T>(name, overrides);

            return result;
        }

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <param name="t"><see cref="System.Type"/> of object to get from the container.</param>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <param name="resolverOverrides">Any overrides for the resolve call.</param>
        /// <returns>The retrieved object.</returns>
        public static object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return PrivateContainer.Resolve(t, name, resolverOverrides);
        }

        /// <summary>
        /// Return instances of all registered types requested.
        /// </summary>
        /// <param name="t">The type requested.</param>
        /// <param name="resolverOverrides">Any overrides for the resolve calls.</param>
        /// <returns>Set of objects of type t.</returns>
        /// <remarks>This method is useful if you've registered multiple types with the same
        /// <see cref="System.Type"/> but different names.
        /// Be aware that this method does NOT return an instance for the default (unnamed)
        /// registration.</remarks>
        public static IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return PrivateContainer.ResolveAll(t, resolverOverrides);
        }

        #endregion

        #endregion
    }
}