using XSLTTransformer.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<XsltEngine>();
builder.Services.AddSingleton<BatchProcessor>(sp => new BatchProcessor(sp.GetRequiredService<XsltEngine>()));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapPost("/transform", (TransformRequest req, XsltEngine engine) =>
{
    AppLogger.Info("API transform request received");
    var result = engine.Transform(req.Xml, req.Xslt);
    return Results.Ok(new { result });
}).WithName("Transform").WithOpenApi();

app.MapPost("/batch", async (BatchRequest req, BatchProcessor bp) =>
{
    var items = req.Items.Select(i => (i.Name, i.Xml, i.Xslt));
    var res = await bp.ProcessAsync(items);
    return Results.Ok(res);
}).WithName("Batch").WithOpenApi();

app.Run();

record TransformRequest(string Xml, string Xslt);
record BatchItem(string Name, string Xml, string Xslt);
record BatchRequest(List<BatchItem> Items);
