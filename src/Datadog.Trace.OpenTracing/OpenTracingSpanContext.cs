﻿using System.Collections.Generic;
using Datadog.Trace.Logging;
using OpenTracing;

namespace Datadog.Trace.OpenTracing
{
    internal class OpenTracingSpanContext : ISpanContext
    {
        private static ILog _log = LogProvider.For<OpenTracingSpanContext>();

        public OpenTracingSpanContext(Span span)
        {
            Span = span;
            Context = Span.Context;
        }

        public OpenTracingSpanContext(SpanContext context)
        {
            Context = context;
        }

        internal Span Span { get; }

        internal SpanContext Context { get; }

        public override bool Equals(object obj)
        {
            var spanContext = obj as OpenTracingSpanContext;
            if (spanContext == null)
            {
                return false;
            }

            return Context.ParentId == spanContext.Context.ParentId &&
                   Context.SpanId == spanContext.Context.SpanId &&
                   Context.ServiceName == spanContext.Context.ServiceName;
        }

        public override int GetHashCode()
        {
            return Context.ParentId.GetHashCode() ^
                   Context.SpanId.GetHashCode() ^
                   Context.ServiceName.GetHashCode();
        }

        public IEnumerable<KeyValuePair<string, string>> GetBaggageItems()
        {
            _log.Debug("SpanContext.GetBaggageItems is not implemented by Datadog.Trace");
            yield break;
        }
    }
}