﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;

namespace BuildNotification.Runtime.Tooling.DatabaseModule.Extensions
{
    /// <summary>
    /// Utility class for creating and unwrapping <see cref="Exception"/> instances.
    /// </summary>
    internal static class Error
    {
        private const string HttpScheme = "http";
        private const string HttpsScheme = "https";

        /// <summary>
        /// Formats the specified resource string using <see cref="M:CultureInfo.CurrentCulture"/>.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The formatted string.</returns>
        internal static string Format(string format, params object[] args)
        {
            return String.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentException"/> with the provided properties.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        internal static ArgumentException Argument(string messageFormat, params object[] messageArgs)
        {
            return new ArgumentException(Format(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="ArgumentException"/> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        internal static ArgumentException Argument(string parameterName, string messageFormat, params object[] messageArgs)
        {
            return new ArgumentException(Format(messageFormat, messageArgs), parameterName);
        }

        /// <summary>
        /// Creates an <see cref="ArgumentNullException"/> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        internal static ArgumentNullException ArgumentNull(string parameterName)
        {
            return new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Creates an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        internal static InvalidOperationException InvalidOperation(string messageFormat, params object[] messageArgs)
        {
            return new InvalidOperationException(Format(messageFormat, messageArgs));
        }

        /// <summary>
        /// Creates an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="innerException">Inner exception</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception"/>.</returns>
        internal static InvalidOperationException InvalidOperation(Exception innerException, string messageFormat, params object[] messageArgs)
        {
            return new InvalidOperationException(Format(messageFormat, messageArgs), innerException);
        }
    }
}