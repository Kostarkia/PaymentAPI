using PaymentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace PaymentAPI.EndPoints
{
    public static class PaymentDetailEndpoints
    {
        public static void MapPaymentDetailEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/PaymentDetail").WithTags(nameof(PaymentDetail));

            group.MapGet("/", async (PaymentDetailContext db) =>
            {
                var data = await db.PaymentDetails.ToListAsync();

                return data;
            })
            .WithName("GetAllPaymentDetails")
            .WithOpenApi();

            group.MapPut("/{paymentDetailID}", async Task<Results<Ok, NotFound>> (Guid paymentDetailID, PaymentDetail paymentDetail, PaymentDetailContext db) =>
            {
                var affected = await db.PaymentDetails
                    .Where(model => model.PaymentDetailID == paymentDetailID)
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
                try
                {
                    var checkIfAlreadyExist = await db.PaymentDetails.AsNoTracking().AnyAsync(a => a.CardNumber == paymentDetail.CardNumber);

                    if (checkIfAlreadyExist)
                    {
                        return Results.BadRequest("Card Number already exists");
                    }

                    db.PaymentDetails.Add(paymentDetail);

                    await db.SaveChangesAsync();

                    return Results.Ok();

                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.ToString());
                }
            })
            .WithName("CreatePaymentDetail");

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
