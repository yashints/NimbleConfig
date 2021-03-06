﻿using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NimbleConfig.Core.Extensions;

namespace NimbleConfig.Core.ConfigurationReaders
{
    public class GenericNonValueTypeConfigurationReader: IConfigurationReader
    {
        public object Read(IConfiguration configuration, Type configType, string key)
        {
            var valueType = configType.GetGenericTypeOfConfigurationSetting();
            var elementType = valueType.GetElementType();

            // Handle Complex Arrays
            if (valueType.IsArray && elementType != null && !elementType.IsValueType)
            {
                return configuration.GetSection(key).GetChildren()
                    .Select(config => config.Get(elementType))
                    .ToArray();
            }

            return configuration.GetSection(key).Get(valueType);
        }
    }
}
