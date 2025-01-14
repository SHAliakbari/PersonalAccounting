using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PersonalAccounting.Domain.Data;
using System.Security.Claims;

namespace PersonalAccounting.BlazorApp
{
    public static class AuthorizationPolicies
    {
        public static void RegisterPolicies(WebApplicationBuilder builder, AuthorizationOptions options)
        {
            options.AddPolicy("AdminOnly",
               policy =>
               {
                   policy.RequireAuthenticatedUser()
                   .RequireRole("admin");
               });

            options.AddPolicy("ViewReceipt", policy =>
            policy.RequireAuthenticatedUser()
              .RequireAssertion(context =>
              {
                  var receiptId = int.Parse(context.Resource.ToString()); // Get receipt ID from resource
                  var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                  using (var scope = builder.Services.BuildServiceProvider().CreateScope())
                  {
                      var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                      var receipt = dbContext.Receipts
                          .Include(r => r.Items)
                          .ThenInclude(ri => ri.Shares)
                          .FirstOrDefault(r => r.Id == receiptId);

                      if (receipt == null) return false; // Receipt not found

                      return receipt.CreateUserName == userId || receipt.PaidByUserName == userId || receipt.Items.Any(ri => ri.Shares.Any(s => s.UserId == userId));
                  }
              }));

            options.AddPolicy("EditReceipt", policy =>
                policy.RequireAuthenticatedUser()
                      .RequireAssertion(context =>
                      {
                          var receiptId = int.Parse(context.Resource.ToString()); // Get receipt ID from resource
                          var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                          using (var scope = builder.Services.BuildServiceProvider().CreateScope())
                          {
                              var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                              var receipt = dbContext.Receipts.FirstOrDefault(r => r.Id == receiptId);
                              return receipt != null && receipt.CreateUserName == userId;
                          }

                      }));

            options.AddPolicy("DeleteReceipt", policy =>
                policy.RequireAuthenticatedUser()
                      .RequireAssertion(context =>
                      {
                          var receiptId = int.Parse(context.Resource.ToString()); // Get receipt ID from resource
                          var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                          using (var scope = builder.Services.BuildServiceProvider().CreateScope())
                          {
                              var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                              var receipt = dbContext.Receipts.FirstOrDefault(r => r.Id == receiptId);
                              return receipt != null && receipt.CreateUserName == userId;
                          }
                      }));
        }
    }
}
