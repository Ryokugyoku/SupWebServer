namespace SupWebServer.System;

// BusinessModel/Attributes/ServiceAttribute.cs
using Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    // 既定は Scoped
    public ServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        => Lifetime = lifetime;
}