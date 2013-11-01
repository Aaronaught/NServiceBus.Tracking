using System;

namespace NServiceBus.Tracking
{
    /// <summary>
    /// Configures the operation tracking extension.
    /// </summary>
    public static class ConfigureOperationMutator
    {
        /// <summary>
        /// Enables operation tracking using the <see cref="OperationMutator"/>.
        /// </summary>
        /// <param name="configure">The current configuration instance.</param>
        /// <returns>The specified <paramref name="configure"/> instance for additional configuration.</returns>
        public static Configure OperationTracking(this Configure configure)
        {
            configure.Configurer.ConfigureComponent<OperationMutator>(DependencyLifecycle.InstancePerUnitOfWork);
            return configure;
        }
    }
}