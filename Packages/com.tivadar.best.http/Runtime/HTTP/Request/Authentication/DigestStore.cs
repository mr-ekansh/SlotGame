using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Best.HTTP.Request.Authentication
{
    /// <summary>
    /// Stores and manages already received digest infos.
    /// </summary>
    public static class DigestStore
    {
        private static ConcurrentDictionary<string, Digest> Digests = new ConcurrentDictionary<string, Digest>();
        
        /// <summary>
        /// Array of algorithms that the plugin supports. It's in the order of priority(first has the highest priority).
        /// </summary>
        private static string[] SupportedAlgorithms = new string[] { "digest", "basic" };

        public static Digest Get(Uri uri)
        {
            if (Digests.TryGetValue(uri.Host, out var digest))
                if (!digest.IsUriProtected(uri))
                    return null;
            return digest;
        }

        /// <summary>
        /// It will retrieve or create a new Digest for the given Uri.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static Digest GetOrCreate(Uri uri) => Digests.GetOrAdd(uri.Host, new Digest(uri));

        public static void Remove(Uri uri) => Digests.TryRemove(uri.Host, out var _);

        public static void Clear() => Digests.Clear();

        public static string FindBest(List<string> authHeaders)
        {
            if (authHeaders == null || authHeaders.Count == 0)
                return string.Empty;

            List<string> headers = new List<string>(authHeaders.Count);
            for (int i = 0; i < authHeaders.Count; ++i)
                headers.Add(authHeaders[i].ToLowerInvariant());

            for (int i = 0; i < SupportedAlgorithms.Length; ++i)
            {
                int idx = headers.FindIndex((header) => header.StartsWith(SupportedAlgorithms[i]));
                if (idx != -1)
                    return authHeaders[idx];
            }

            return string.Empty;
        }
    }
}
