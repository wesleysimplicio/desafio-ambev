using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sales.Data.Events;
using Sales.Domain.Interfaces.Events;
using Sales.Domain.Interfaces.Services;
using Sales.Domain.Models;

namespace Sales.WebApi.Endpoints
{
    public static class SalesEndpoints
    {
        public static void MapSalesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Sales/{idSale}", HandleGetById)
              .WithName("Sale")
              .WithOpenApi();

            app.MapGet("/api/Sales", HandleGetAll)
               .WithName("AllSales")
               .WithOpenApi();

            app.MapPut("/api/Sales/{idSale}", HandleUpdate)
               .WithName("Update")
               .WithOpenApi();

            app.MapDelete("/api/Sales/{idSale}", HandleDelete)
               .WithName("DeleteSale")
               .WithOpenApi();

            app.MapPost("/api/Sales", HandleCreate)
               .WithName("AddSale")
               .WithOpenApi();
        }


        private static async Task<IResult> HandleGetById([FromServices] ISaleService service, ILogger<Program> logger, int idSale)
        {
            var entidade = await service.GetByIdAsync(idSale);
            return entidade != null ? Results.Ok(entidade) : Results.NotFound($"Venda com ID {idSale} não encontrada.");
        }

        private static async Task<IResult> HandleGetAll([FromServices] ISaleService service, ILogger<Program> logger)
        {
            var entidade = await service.GetAllAsync();
            return Results.Ok(entidade);
        }     

        private static async Task<IResult> HandleCreate(
            [FromServices] ISaleService service,
            [FromServices] IValidator<SaleModel> validator,
             IEventDispatcher eventDispatcher,
            ILogger<Program> logger,
            [FromBody] SaleModel saleReq)
        {
            var validationResult = validator.Validate(saleReq);

            if (!validationResult.IsValid)
            {
                logger.LogWarning($"Object inválido, erros: {string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage))}");
                return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var entidade = await service.AddAsync(saleReq);

            await eventDispatcher.DispatchAsync(new EventoSale
            {
                IdSale = entidade.Object.IdSale,
                NumberSale = entidade.Object.NumberSale,
                DataCriacao = DateTime.UtcNow,
                Tipo = TipoEvento.CompraCriada
            });

            return Results.Created($"/api/Sales/{entidade.Object.NumberSale}", entidade);
        }

        private static async Task<IResult> HandleUpdate([FromServices] ISaleService service, IEventDispatcher eventDispatcher, ILogger<Program> logger, int idSale, [FromBody] SaleModel sale)
        {
            var entidade = await service.UpdateAsync(idSale, sale);

            await eventDispatcher.DispatchAsync(new EventoSale
            {
                IdSale = entidade.Object.IdSale,
                NumberSale = entidade.Object.NumberSale,
                DataCriacao = DateTime.UtcNow,
                Tipo = TipoEvento.CompraAlterada
            });

            return Results.Ok(entidade);
        }

        private static async Task<IResult> HandleDelete(int idSale, [FromServices] ISaleService service, IEventDispatcher eventDispatcher, ILogger<Program> logger)
        {
            var entidade = await service.DeleteAsync(idSale);

            await eventDispatcher.DispatchAsync(new EventoSale
            {
                IdSale = entidade.Object.IdSale,
                NumberSale = entidade.Object.NumberSale,
                DataCriacao = DateTime.UtcNow,
                Tipo = TipoEvento.CompraCancelada
            });

            return Results.Ok(entidade);
        }
    }
}
