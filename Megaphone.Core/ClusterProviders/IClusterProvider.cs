﻿using Megaphone.Core.Util;
using System;
using System.Threading.Tasks;

namespace Megaphone.Core.ClusterProviders
{
    public interface IClusterProvider
    {
        Task<ServiceInformation[]> FindServiceInstancesAsync(string name);

        Task RegisterServiceAsync(string serviceName, string serviceId, string version, Uri uri);

        Task BootstrapClientAsync();

        Task KvPutAsync(string key, object value);

        Task<T> KvGetAsync<T>(string key);
    }

    public static class ClusterProviderExtensions
    {
        public static async Task<ServiceInformation> FindServiceInstanceAsync(this IClusterProvider self, string serviceName)
        {
            var res = await self.FindServiceInstancesAsync(serviceName).ConfigureAwait(false);
            if (res.Length == 0)
                throw new Exception("Could not find service");

            return res[ThreadLocalRandom.Current.Next(0, res.Length)];
        }
    }
}