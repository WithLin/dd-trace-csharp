using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Datadog.Trace.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Datadog.Trace.ClrProfiler.IntegrationTests
{
    public class AspNetCoreMvc2Tests : TestHelper
    {
        private const int AgentPort = 9000;
        private const int Port = 9001;

        public AspNetCoreMvc2Tests(ITestOutputHelper output)
            : base("AspNetCoreMvc2", output)
        {
        }

        public static IEnumerable<object[]> Urls
        {
            get
            {
                yield return new object[] { $"http://localhost:{Port}/", "GET /" };
                yield return new object[] { $"http://localhost:{Port}/api/delay/0", "GET /api/delay/0" };
            }
        }

        /*
        [Theory]
        [MemberData(nameof(Urls))]
        [Trait("Category", "EndToEnd")]
        public void SubmitsTracesIis(string url)
        {
            using (var agent = new MockTracerAgent(AgentPort))
            {
                using (var iis = StartIISExpress(AgentPort, Port))
                {
                    try
                    {
                        var request = WebRequest.Create(url);
                        using (var response = (HttpWebResponse)request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            Output.WriteLine($"[http] {response.StatusCode} {reader.ReadToEnd()}");
                        }
                    }
                    catch (WebException wex)
                    {
                        Output.WriteLine($"[http] exception: {wex}");
                        if (wex.Response is HttpWebResponse response)
                        {
                            using (var stream = response.GetResponseStream())
                            using (var reader = new StreamReader(stream))
                            {
                                Output.WriteLine($"[http] {response.StatusCode} {reader.ReadToEnd()}");
                            }
                        }
                    }
                }

                var spans = agent.WaitForSpans(1);
                Assert.True(spans.Count > 0, "expected at least one span");
                foreach (var span in spans)
                {
                    Assert.Equal(Integrations.AspNetWebApi2Integration.OperationName, span.Name);
                    Assert.Equal(SpanTypes.Web, span.Type);
                    Assert.Equal("GET api/environment", span.Resource);
                }
            }
        }
        */

        [Theory]
        [MemberData(nameof(Urls))]
        [Trait("Category", "EndToEnd")]
        public void SubmitsTracesSelfHosted(string url, string expectedResourceName)
        {
            using (var agent = new MockTracerAgent(AgentPort))
            using (Process process = StartSample(AgentPort))
            {
                try
                {
                    var request = WebRequest.Create(url);
                    using (var response = (HttpWebResponse)request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Output.WriteLine($"[http] {response.StatusCode} {reader.ReadToEnd()}");
                    }
                }
                catch (WebException wex)
                {
                    Output.WriteLine($"[http] exception: {wex}");
                    if (wex.Response is HttpWebResponse response)
                    {
                        using (var stream = response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            Output.WriteLine($"[http] {response.StatusCode} {reader.ReadToEnd()}");
                        }
                    }
                }

                var spans = agent.WaitForSpans(1);
                Assert.True(spans.Count > 0, "expected at least one span");
                foreach (var span in spans)
                {
                    Assert.Equal(Integrations.AspNetWebApi2Integration.OperationName, span.Name);
                    Assert.Equal(SpanTypes.Web, span.Type);
                    Assert.Equal(expectedResourceName, span.Resource);
                }

                process.Kill();
            }
        }
    }
}
