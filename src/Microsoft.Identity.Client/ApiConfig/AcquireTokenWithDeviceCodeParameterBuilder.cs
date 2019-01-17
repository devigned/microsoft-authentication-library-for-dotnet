﻿// ------------------------------------------------------------------------------
// 
// Copyright (c) Microsoft Corporation.
// All rights reserved.
// 
// This code is licensed under the MIT License.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.ApiConfig
{
    /// <summary>
    /// </summary>
    public sealed class AcquireTokenWithDeviceCodeParameterBuilder :
        AbstractPcaAcquireTokenParameterBuilder<AcquireTokenWithDeviceCodeParameterBuilder>
    {
        /// <inheritdoc />
        public AcquireTokenWithDeviceCodeParameterBuilder(IPublicClientApplication publicClientApplication)
            : base(publicClientApplication)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="publicClientApplication"></param>
        /// <param name="scopes"></param>
        /// <param name="deviceCodeResultCallback"></param>
        /// <returns></returns>
        internal static AcquireTokenWithDeviceCodeParameterBuilder Create(
            IPublicClientApplication publicClientApplication,
            IEnumerable<string> scopes,
            Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            return new AcquireTokenWithDeviceCodeParameterBuilder(publicClientApplication)
                   .WithScopes(scopes).WithDeviceCodeResultCallback(deviceCodeResultCallback);
        }

        /// <summary>
        /// Sets the Callback delegate that will be called so that your application can
        /// interact with the user to direct them to authenticate (to a specific URL, with a code)
        /// </summary>
        /// <param name="deviceCodeResultCallback">callback containing information to show the user about how to authenticate 
        /// and enter the device code.</param>
        /// <returns>The builder to chain the .With methods</returns>
        public AcquireTokenWithDeviceCodeParameterBuilder WithDeviceCodeResultCallback(
            Func<DeviceCodeResult, Task> deviceCodeResultCallback)
        {
            Parameters.DeviceCodeResultCallback = deviceCodeResultCallback;
            return this;
        }

        /// <inheritdoc />
        internal override Task<AuthenticationResult> ExecuteAsync(IPublicClientApplicationExecutor executor, CancellationToken cancellationToken)
        {
            return executor.ExecuteAsync((IAcquireTokenWithDeviceCodeParameters)Parameters, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void Validate()
        {
            base.Validate();

            if (Parameters.DeviceCodeResultCallback == null)
            {
                throw new ArgumentNullException(
                    nameof(Parameters.DeviceCodeResultCallback), 
                    "A deviceCodeResultCallback must be provided for Device Code authentication to work properly");
            }
        }
    }
}