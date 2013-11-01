using System;

namespace NServiceBus.Tracking.Sample.Fakes
{
    static class ConfigureOperationProvider
    {
        public static Configure FakeOperationProvider(this Configure configure)
        {
            configure.Configurer.ConfigureComponent<OperationProvider>(DependencyLifecycle.SingleInstance);
            return configure;
        }
    }
}