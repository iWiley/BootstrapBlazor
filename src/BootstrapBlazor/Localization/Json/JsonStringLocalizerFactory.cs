﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BootstrapBlazor.Localization.Json;

/// <summary>
/// IStringLocalizerFactory 实现类
/// </summary>
internal class JsonStringLocalizerFactory : ResourceManagerStringLocalizerFactory
{
    private ILoggerFactory LoggerFactory { get; set; }

    [NotNull]
    private string? TypeName { get; set; }

    private IServiceProvider ServiceProvider { get; set; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="options"></param>
    /// <param name="jsonLocalizationOptions"></param>
    /// <param name="localizationOptions"></param>
    /// <param name="loggerFactory"></param>
    public JsonStringLocalizerFactory(
        IServiceProvider provider,
        IOptionsMonitor<BootstrapBlazorOptions> options,
        IOptions<JsonLocalizationOptions> jsonLocalizationOptions,
        IOptions<LocalizationOptions> localizationOptions,
        ILoggerFactory loggerFactory) : base(localizationOptions, loggerFactory)
    {
        ServiceProvider = provider;
        jsonLocalizationOptions.Value.FallbackCulture = options.CurrentValue.FallbackCulture;
        jsonLocalizationOptions.Value.EnableFallbackCulture = options.CurrentValue.EnableFallbackCulture;
        LoggerFactory = loggerFactory;
        options.OnChange(OnChange);

        [ExcludeFromCodeCoverage]
        void OnChange(BootstrapBlazorOptions op)
        {
            jsonLocalizationOptions.Value.EnableFallbackCulture = op.EnableFallbackCulture;
            jsonLocalizationOptions.Value.FallbackCulture = op.FallbackCulture;
        }
    }

    /// <summary>
    /// GetResourcePrefix 方法
    /// </summary>
    /// <param name="typeInfo"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected override string GetResourcePrefix(TypeInfo typeInfo)
    {
        var typeName = typeInfo.FullName;
        if (string.IsNullOrEmpty(typeName))
        {
            throw new InvalidOperationException($"{nameof(typeInfo)} full name is null or String.Empty.");
        }

        if (typeInfo.IsGenericType)
        {
            var index = typeName.IndexOf('`');
            typeName = typeName[..index];
        }
        TypeName = typeName;

        return base.GetResourcePrefix(typeInfo);
    }

    private IResourceNamesCache ResourceNamesCache { get; } = new ResourceNamesCache();

    /// <summary>
    /// Creates a <see cref="ResourceManagerStringLocalizer"/> for the given input
    /// </summary>
    /// <param name="assembly">The assembly to create a <see cref="ResourceManagerStringLocalizer"/> for</param>
    /// <param name="baseName">The base name of the resource to search for</param>
    /// <returns></returns>
    protected override ResourceManagerStringLocalizer CreateResourceManagerStringLocalizer(Assembly assembly, string baseName) => new JsonStringLocalizer(
            ServiceProvider,
            assembly,
            TypeName,
            baseName,
            LoggerFactory.CreateLogger<JsonStringLocalizer>(),
            ResourceNamesCache);
}
