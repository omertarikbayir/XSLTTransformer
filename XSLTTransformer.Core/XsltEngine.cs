using Saxon.Api;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace XSLTTransformer.Core
{
    /// <summary>
    /// High-performance XSLT engine using Saxon-HE 10.
    /// </summary>
    public sealed class XsltEngine
    {
        private readonly Processor _processor;
        private readonly ConcurrentDictionary<string, XsltExecutable> _cache;

        public XsltEngine()
        {
            _processor = new Processor();
            _cache = new ConcurrentDictionary<string, XsltExecutable>();
        }

        public string Transform(string xml, string xslt, TransformationOptions? options = null)
        {
            AppLogger.Info("XSLT transform started");
            options ??= new TransformationOptions();
            var key = xslt.GetHashCode().ToString();
            var executable = _cache.GetOrAdd(key, _ => Compile(xslt));
            var transformer = executable.Load();

            foreach (var kv in options.Parameters)
                transformer.SetParameter(new QName(kv.Key), new XdmAtomicValue(kv.Value?.ToString() ?? string.Empty));

            var builder = _processor.NewDocumentBuilder();
            var source = builder.Build(new StringReader(xml));
            transformer.InitialContextNode = source;

            var serializer = _processor.NewSerializer();
            serializer.SetOutputProperty(Serializer.INDENT, "yes");
            using var writer = new StringWriter();
            serializer.SetOutputWriter(writer);
            transformer.Run(serializer);

            AppLogger.Info("XSLT transform completed");
            return writer.ToString();
        }

        private XsltExecutable Compile(string xslt)
        {
            var compiler = _processor.NewXsltCompiler();
            return compiler.Compile(new StringReader(xslt));
        }
    }

    public sealed class TransformationOptions
    {
        public System.Collections.Generic.Dictionary<string, object> Parameters { get; set; } = new();
    }
}