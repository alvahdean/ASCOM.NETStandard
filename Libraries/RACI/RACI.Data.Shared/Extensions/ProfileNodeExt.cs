using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace RACI.Data
{
    public static class ProfileNodeExt
    {
        private static ProfileKeyComparer keyUtil = new ProfileKeyComparer();

        private static string DefaultServiceRoot(string hostOrAddress, uint port = 0, bool ssl = true)
        {
            string host = GetHostname(hostOrAddress);
            string prot = ssl ? "https" : "http";
            port = port == 0 ? 80 : port;

            return String.IsNullOrWhiteSpace(host) ? null : $"{prot}://{host}:{port}/rascom";
        }
        private static string GetHostname(string hostOrAddress) => Dns.GetHostEntry(hostOrAddress)?.HostName;
        private static Uri GetUri(string rawUrl) => String.IsNullOrWhiteSpace(rawUrl) ? null : new Uri(rawUrl);
        private static string GetUrl(string rawUrl) => GetUri(rawUrl)?.AbsoluteUri;

        //#region IQueryable extensions

        //public static bool AnyName<TSource>(this IQueryable<TSource> source,string name)
        //    where TSource: IProfileNode => source.Any(t => keyUtil.Equals(t.Name, name));

        //public static TSource SelectName<TSource>(this IQueryable<TSource> source, string name)
        //    where TSource : IProfileNode =>
        //    source.FirstOrDefault(t => keyUtil.Equals(t.Name, name));

        //public static IOrderedQueryable<TSource> SortByName<TSource, TKey>(this IQueryable<TSource> source)
        //    where TSource : IProfileNode =>
        //    source.OrderBy(t => t.Name, keyUtil);

        //public static IOrderedQueryable<TSource> SortByNameDescending<TSource, TKey>(this IQueryable<TSource> source)
        //    where TSource : IProfileNode =>
        //    source.OrderByDescending(t => t.Name, keyUtil);
        //#endregion

        #region IEnumerable extensions
        public static bool AnyWithName<TSource>(this IEnumerable<TSource> source, string name)
            where TSource : IProfileNode =>
            source.Any(t => keyUtil.Equals(t.Name, name));

        public static TSource WithName<TSource>(this IEnumerable<TSource> source, string name)
            where TSource : IProfileNode =>
            source.FirstOrDefault(t => keyUtil.Equals(t.Name, name));

        public static IOrderedEnumerable<TSource> SortByName<TSource, TKey>(this IEnumerable<TSource> source)
            where TSource : IProfileNode =>
            source.OrderBy(t => t.Name, keyUtil);

        public static IOrderedEnumerable<TSource> SortByNameDescending<TSource, TKey>(this IEnumerable<TSource> source)
            where TSource : IProfileNode =>
            source.OrderByDescending(t => t.Name, keyUtil);

        public static bool AnyWithUrl(this IEnumerable<RaciEndpoint> source, string url)=>
            source.Any(t => keyUtil.Equals(t.ServiceRoot, url));

        public static bool AnyWithUri(this IEnumerable<RaciEndpoint> source, Uri uri)=>
            AnyWithUrl(source,uri?.AbsoluteUri);

        public static RaciEndpoint WithUrl(this IEnumerable<RaciEndpoint> source, string name) =>
            source.FirstOrDefault(t => keyUtil.Equals(t.Name, name));

        #endregion

        public static Uri Uri(this RaciEndpoint ep) => GetUri(ep?.ServiceRoot);
        public static string Hostname(this RaciEndpoint ep) => Uri(ep)?.DnsSafeHost;
        public static string Url(this RaciEndpoint ep) => GetUri(ep?.ServiceRoot).AbsoluteUri;

        public static bool MatchUrl(this RaciEndpoint ep, string url) =>
            keyUtil.Equals(GetUrl(ep.ServiceRoot), GetUrl(url));
        public static bool MatchHost(this RaciEndpoint ep, string host) =>
            keyUtil.Equals(Hostname(ep), GetHostname(host));

    }
}
