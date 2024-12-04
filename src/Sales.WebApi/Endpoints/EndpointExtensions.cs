namespace Sales.WebApi.Endpoints
{
    public static class EndpointExtensions
    {
        public static void AddEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapSalesEndpoints();
        }
    }
}
