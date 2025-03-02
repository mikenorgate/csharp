using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace k8s
{
    public partial class Kubernetes
    {
        /// <inheritdoc/>
        public async Task<Watcher<T>> WatchObjectAsync<T>(string path, string @continue = null,
            string fieldSelector = null, bool? includeUninitialized = null, string labelSelector = null,
            int? limit = null, bool? pretty = null, int? timeoutSeconds = null, string resourceVersion = null,
            Dictionary<string, List<string>> customHeaders = null, Action<WatchEventType, T> onEvent = null,
            Action<Exception> onError = null, Action onClosed = null,
            CancellationToken cancellationToken = default)
        {
            // Tracing
            var shouldTrace = ServiceClientTracing.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = ServiceClientTracing.NextInvocationId.ToString();
                var tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("path", path);
                tracingParameters.Add("continue", @continue);
                tracingParameters.Add("fieldSelector", fieldSelector);
                tracingParameters.Add("includeUninitialized", includeUninitialized);
                tracingParameters.Add("labelSelector", labelSelector);
                tracingParameters.Add("limit", limit);
                tracingParameters.Add("pretty", pretty);
                tracingParameters.Add("timeoutSeconds", timeoutSeconds);
                tracingParameters.Add("resourceVersion", resourceVersion);
                ServiceClientTracing.Enter(invocationId, this, nameof(WatchObjectAsync), tracingParameters);
            }

            // Construct URL
            var uriBuilder = new UriBuilder(BaseUri);
            if (!uriBuilder.Path.EndsWith("/", StringComparison.InvariantCulture))
            {
                uriBuilder.Path += "/";
            }

            uriBuilder.Path += path;

            var query = new StringBuilder();
            // Don't sent watch, because setting that value will cause the WatcherDelegatingHandler to kick in. That class
            // "eats" the first line, which is something we don't want.
            // query = QueryHelpers.AddQueryString(query, "watch", "true");
            if (@continue != null)
            {
                Utilities.AddQueryParameter(query, "continue", @continue);
            }

            if (!string.IsNullOrEmpty(fieldSelector))
            {
                Utilities.AddQueryParameter(query, "fieldSelector", fieldSelector);
            }

            if (includeUninitialized != null)
            {
                Utilities.AddQueryParameter(query, "includeUninitialized",
                    includeUninitialized.Value ? "true" : "false");
            }

            if (!string.IsNullOrEmpty(labelSelector))
            {
                Utilities.AddQueryParameter(query, "labelSelector", labelSelector);
            }

            if (limit != null)
            {
                Utilities.AddQueryParameter(query, "limit", limit.Value.ToString());
            }

            if (pretty != null)
            {
                Utilities.AddQueryParameter(query, "pretty", pretty.Value ? "true" : "false");
            }

            if (timeoutSeconds != null)
            {
                Utilities.AddQueryParameter(query, "timeoutSeconds", timeoutSeconds.Value.ToString());
            }

            if (!string.IsNullOrEmpty(resourceVersion))
            {
                Utilities.AddQueryParameter(query, "resourceVersion", resourceVersion);
            }

            uriBuilder.Query =
                query.Length == 0
                    ? ""
                    : query.ToString(
                        1,
                        query.Length - 1); // UriBuilder.Query doesn't like leading '?' chars, so trim it

            // Create HTTP transport objects
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());

            // Set Credentials
            if (Credentials != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }

            // Set Headers
            if (customHeaders != null)
            {
                foreach (var header in customHeaders)
                {
                    if (httpRequest.Headers.Contains(header.Key))
                    {
                        httpRequest.Headers.Remove(header.Key);
                    }

                    httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            // Send Request
            if (shouldTrace)
            {
                ServiceClientTracing.SendRequest(invocationId, httpRequest);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var httpResponse = await HttpClient
                .SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            if (shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(invocationId, httpResponse);
            }

            cancellationToken.ThrowIfCancellationRequested();

            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                var responseContent = string.Empty;

                var ex = new HttpOperationException(string.Format(
                    "Operation returned an invalid status code '{0}'",
                    httpResponse.StatusCode));
                if (httpResponse.Content != null)
                {
#if NET5_0
                    responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
                    responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
                }

                ex.Request = new HttpRequestMessageWrapper(httpRequest, responseContent);
                ex.Response = new HttpResponseMessageWrapper(httpResponse, string.Empty);

                httpRequest.Dispose();
                httpResponse?.Dispose();
                throw ex;
            }

            return new Watcher<T>(
                async () =>
            {
                var stream = await httpResponse.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var reader = new StreamReader(stream);

                return reader;
            }, onEvent, onError, onClosed);
        }
    }
}
