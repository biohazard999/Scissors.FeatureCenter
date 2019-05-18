using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Scissors.Utils.Exceptions;

namespace Scissors.Utils
{
    /// <summary>
    /// Helper class for common method parameter validation tasks.
    /// </summary>
    public static class Guard
    {
        private static class Constants
        {
            /// <summary>ParameterNull '{0}' should not be NULL!</summary>
            public const string ParameterNull = "Parameter '{0}' must not be NULL!";

            /// <summary>parameter '{0}' should not be empty!</summary>
            public const string ParameterEmpty = "Parameter '{0}' must not be empty!";

            /// <summary>Parameter '{0}' with a value of '{1}' is not between '{2}' and '{3}!</summary>
            public const string ParameterNotInRange =
                "Parameter '{0}' with a value of '{1}' is not between '{2}' and '{3}!";

            /// <summary>&lt;no parameter name&gt;</summary>
            public const string NoParameterName = "<no parameter name>";
        }

        /// <summary>
        /// Asserts the not null.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ScissorsArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        public static void AssertNotNull(object param, string paramName, string message = null)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                if(message != null)
                {
                    throw new ScissorsArgumentNullException(paramName, message);
                }

                throw new ScissorsArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// Asserts the not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector">The selector.</param>
        /// <exception cref="ScissorsArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static void AssertNotNull<T>(Expression<Func<T>> selector)
        {
            var memberSelector = (MemberExpression)selector.Body;
            var constantSelector = (ConstantExpression)memberSelector.Expression;
            var value = ((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);

            if (value == null)
            {
                var name = ((MemberExpression)selector.Body).Member.Name;
                throw new ScissorsArgumentNullException(name);
            }
        }

        /// <summary>
        /// Asserts the not empty.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ScissorsArgumentNullException"></exception>
        /// <exception cref="ScissorsArgumentException"></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(string param, string paramName)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                throw new ScissorsArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(param))
            {
                throw new ScissorsArgumentException(Constants.ParameterEmpty, paramName);
            }
        }

        /// <summary>
        /// Asserts the not empty.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <exception cref="ScissorsArgumentNullException"></exception>
        /// <exception cref="ScissorsArgumentException">String must not be empty.</exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(Expression<Func<string>> selector)
        {
            var memberSelector = (MemberExpression)selector.Body;
            var constantSelector = (ConstantExpression)memberSelector.Expression;
            var value = (string)((FieldInfo)memberSelector.Member).GetValue(constantSelector.Value);

            if (value == null)
            {
                var paramName = ((MemberExpression)selector.Body).Member.Name;
                throw new ScissorsArgumentNullException(paramName);
            }

            if (string.IsNullOrEmpty(value))
            {
                var paramName = ((MemberExpression)selector.Body).Member.Name;
                throw new ScissorsArgumentException("String must not be empty.", paramName);
            }
        }

        /// <summary>
        /// Asserts the not empty.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ScissorsArgumentNullException"></exception>
        /// <exception cref="ScissorsArgumentException"></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty(ICollection param, string paramName)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                throw new ScissorsArgumentNullException(paramName, Constants.ParameterNull);
            }

            if (param.Count == 0)
            {
                throw new ScissorsArgumentException(paramName, Constants.ParameterEmpty).WithAdditionalInfo("param", param);
            }
        }

        /// <summary>
        /// Asserts the not empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <exception cref="ScissorsArgumentNullException"></exception>
        /// <exception cref="ScissorsArgumentException"></exception>
        [DebuggerStepThrough]
        public static void AssertNotEmpty<T>(ICollection<T> param, string paramName)
        {
            paramName = string.IsNullOrWhiteSpace(paramName) ? Constants.NoParameterName : paramName;

            if (param == null)
            {
                throw new ScissorsArgumentNullException(paramName, Constants.ParameterNull);
            }

            if (param.Count == 0)
            {
                throw new ScissorsArgumentException(paramName, Constants.ParameterEmpty).WithAdditionalInfo("param", param);
            }
        }

        /// <summary>
        /// Asserts the in range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <exception cref="ScissorsArgumentOutOfRangeException"></exception>
        [DebuggerStepThrough]
        public static void AssertInRange<T>(T param, string paramName, T min, T max) where T : IComparable
        {
            AssertNotNull(param, nameof(param));
            AssertNotNull(min, nameof(min));
            AssertNotNull(max, nameof(max));

            paramName = paramName ?? Constants.NoParameterName;

            if (param.CompareTo(min) < 0 || param.CompareTo(max) > 0)
            {
                var message = string.Format(Constants.ParameterNotInRange, paramName, param, min, max);
                throw new ScissorsArgumentOutOfRangeException(paramName, string.Format(message));
            }
        }

        /// <summary>
        /// Asserts the file exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="FileNotFoundException">The '{filePath}</exception>
        [DebuggerStepThrough]
        public static void AssertFileExists(string filePath)
        {
            AssertNotEmpty(filePath, nameof(filePath));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The '{filePath}' does not exist.", filePath);
            }
        }
    }
}