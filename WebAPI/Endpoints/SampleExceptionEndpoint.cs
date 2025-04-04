namespace WebAPI.Endpoints
{
    public static class SampleExceptionEndpoint
    {
        public static void MapSampleExceptionEndpoint(this WebApplication app)
        {
            app.MapGet("/exception", () =>
            {
                throw new InvalidOperationException("Sample Exception");
            });
        }
    }
}
