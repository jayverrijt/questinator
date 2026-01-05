using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Questinator.Data;
using Questinator.Models;
using System.Globalization;

namespace Questinator.Pages
{
    public class StoreModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StoreModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<StoreItem> Items { get; set; } = new();

        public async Task OnGetAsync()
        {
            Items = await _context.StoreItems.ToListAsync();
        }

        public async Task<IActionResult> OnPostBuyAsync(int itemId)
        {
            var item = await _context.StoreItems.FindAsync(itemId);
            if (item == null)
                return NotFound();

            var apiKey = _configuration["Mollie:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("Mollie API key ontbreekt");

            var paymentClient = new PaymentClient(apiKey);

            var amount = new Amount(
                "EUR",
                (item.Price / 100m).ToString("0.00", CultureInfo.InvariantCulture)
            );

            var redirectUrl = $"{Request.Scheme}://{Request.Host}/StoreSuccess";
            var cancelUrl = $"{Request.Scheme}://{Request.Host}/StoreFailed";

            var paymentRequest = new PaymentRequest
            {
                Amount = amount,
                Description = $"Questinator Store: {item.Name}",
                RedirectUrl = redirectUrl,
                CancelUrl = cancelUrl
            };

            var payment = await paymentClient.CreatePaymentAsync(paymentRequest);

            return Redirect(payment.Links.Checkout.Href);
        }
    }
}
