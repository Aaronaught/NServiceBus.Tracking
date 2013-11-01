using System;
using MongoDB.Driver;

namespace NServiceBus.Tracking.Mongo
{
    /// <summary>
    /// Configures the <see cref="OperationProvider"/> in the NServiceBus container.
    /// </summary>
    public static class ConfigureOperationProvider
    {
        /// <summary>
        /// Specifies that the Mongo-based <see cref="OperationProvider"/> should be used for
        /// operation tracking.
        /// </summary>
        /// <param name="configure">The current configuration instance.</param>
        /// <returns>The specified <paramref name="configure"/> instance for additional configuration.</returns>
        public static Configure MongoOperationProvider(this Configure configure)
        {
            return MongoOperationProvider(configure, null);
        }

        /// <summary>
        /// Specifies that the Mongo-based <see cref="OperationProvider"/> should be used for
        /// operation tracking, and specifies how the Mongo collection should be resolved.
        /// </summary>
        /// <param name="configure">The current configuration instance.</param>
        /// <param name="collectionFactory">A delegate to use to retrieve the Mongo collection where
        /// operation data is stored.</param>
        /// <returns>The specified <paramref name="configure"/> instance for additional configuration.</returns>
        public static Configure MongoOperationProvider(this Configure configure,
            Func<MongoCollection<OperationData>> collectionFactory)
        {
            var lifecycle = DependencyLifecycle.InstancePerUnitOfWork;
            if (collectionFactory != null)
                configure.Configurer.ConfigureComponent<IOperationProvider>(() =>
                {
                    var collection = collectionFactory();
                    return new OperationProvider(collection);
                }, lifecycle);
            else
                configure.Configurer.ConfigureComponent<IOperationProvider>(lifecycle);
            return configure;
        }
    }
}