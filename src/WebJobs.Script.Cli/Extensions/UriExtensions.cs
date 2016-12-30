﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Script.WebHost.Models;

namespace WebJobs.Script.Cli.Extensions
{
    internal static class UriExtensions
    {
        public static async Task<bool> IsServerRunningAsync(this Uri server)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var rootResponse = await client.GetAsync(server);
                    var statusResponse = await client.GetAsync(new Uri(server, "admin/host/status"));
                    statusResponse.EnsureSuccessStatusCode();
                    var hostStatus = await statusResponse.Content.ReadAsAsync<HostStatus>();

                    return rootResponse.StatusCode == HttpStatusCode.OK &&
                        hostStatus.WebHostSettings != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}