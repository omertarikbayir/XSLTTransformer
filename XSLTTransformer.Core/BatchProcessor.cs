using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XSLTTransformer.Core
{
    public sealed class BatchTransformationResult
    {
        public string InputName { get; set; } = "";
        public string Output { get; set; } = "";
        public bool Success { get; set; }
        public string Error { get; set; } = "";
    }

    public sealed class BatchProcessor
    {
        private readonly XsltEngine _engine;

        public BatchProcessor(XsltEngine engine) => _engine = engine;

        public async Task<IReadOnlyList<BatchTransformationResult>> ProcessAsync(
            IEnumerable<(string Name, string Xml, string Xslt)> items,
            TransformationOptions? options = null,
            IProgress<int>? progress = null,
            CancellationToken ct = default)
        {
            AppLogger.Info("Batch process started");
            var results = new List<BatchTransformationResult>();
            var total = items.Count();
            var completed = 0;

            await Task.Run(() =>
            {
                Parallel.ForEach(items, new ParallelOptions { CancellationToken = ct }, item =>
                {
                    var result = new BatchTransformationResult { InputName = item.Name };
                    try
                    {
                        result.Output = _engine.Transform(item.Xml, item.Xslt, options);
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        result.Error = ex.Message;
                        result.Success = false;
                    }

                    lock (results)
                    {
                        results.Add(result);
                        progress?.Report(Interlocked.Increment(ref completed) * 100 / total);
                    }
                });
            }, ct);

            AppLogger.Info("Batch process completed");
            return results;
        }
    }
}