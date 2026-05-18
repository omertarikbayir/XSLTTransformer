using XSLTTransformer.Core;
using Xunit;

namespace XSLTTransformer.Tests;

public class XsltEngineTests
{
    [Fact]
    public void Transform_SimpleIdentity_ReturnsOutput()
    {
        var engine = new XsltEngine();
        const string xml = "<root><item>test</item></root>";
        const string xslt = "<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'><xsl:template match='/'><xsl:copy-of select='.'/></xsl:template></xsl:stylesheet>";
        
        var result = engine.Transform(xml, xslt);
        Assert.Contains("test", result);
    }
}

public class BatchProcessorTests
{
    [Fact]
    public async Task ProcessAsync_EmptyList_ReturnsEmpty()
    {
        var engine = new XsltEngine();
        var bp = new BatchProcessor(engine);
        var result = await bp.ProcessAsync(new List<(string, string, string)>());
        Assert.Empty(result);
    }
}