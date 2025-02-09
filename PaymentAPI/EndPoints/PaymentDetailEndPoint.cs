using PaymentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
namespace PaymentAPI.EndPoints
{
    public static class PaymentDetailEndpoints
    {
        public static void MapPaymentDetailEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/PaymentDetail").WithTags(nameof(PaymentDetail));

            group.MapGet("/", async (PaymentDetailContext db) =>
            {
                var data = db.PaymentDetails.ToList();

                return data;
            })
            .WithName("GetAllPaymentDetails")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<PaymentDetail>, NotFound>> (Guid paymentdetailid, PaymentDetailContext db) =>
            {
                return await db.PaymentDetails.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.PaymentDetailID == paymentdetailid)
                    is PaymentDetail model
                        ? TypedResults.Ok(model)
                        : TypedResults.NotFound();
            })
            .WithName("GetPaymentDetailById")
            .WithOpenApi();

            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (string paymentDetailID, PaymentDetail paymentDetail, PaymentDetailContext db) =>
            {
                var id = Guid.Parse(paymentDetailID);

                var affected = await db.PaymentDetails
                    .Where(model => model.PaymentDetailID == id)
                    .ExecuteUpdateAsync(setters => setters
                      .SetProperty(m => m.PaymentDetailID, paymentDetail.PaymentDetailID)
                      .SetProperty(m => m.CardOwnerName, paymentDetail.CardOwnerName)
                      .SetProperty(m => m.CardNumber, paymentDetail.CardNumber)
                      .SetProperty(m => m.ExpirationDate, paymentDetail.ExpirationDate)
                      .SetProperty(m => m.SecurityCode, paymentDetail.SecurityCode)
                      );
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("UpdatePaymentDetail")
            .WithOpenApi();

            group.MapPost("/", async (PaymentDetail paymentDetail, PaymentDetailContext db) =>
            {
                db.PaymentDetails.Add(paymentDetail);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/PaymentDetail/{paymentDetail.PaymentDetailID}", paymentDetail);
            })
            .WithName("CreatePaymentDetail")
            .WithOpenApi();

            group.MapDelete("/{paymentDetailID}", async (Guid paymentDetailID, PaymentDetailContext db) =>
            {
                var affected = await db.PaymentDetails
                    .Where(model => model.PaymentDetailID == paymentDetailID)
                    .ExecuteDeleteAsync();

                return affected == 1 ? Results.Ok() : Results.NotFound();
            })
            .WithName("DeletePaymentDetail")
            .WithOpenApi();



        }
    }
}
