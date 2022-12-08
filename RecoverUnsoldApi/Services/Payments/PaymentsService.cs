using System.Text.Json;
using RecoverUnsoldApi.Dto;
using RecoverUnsoldDomain.Data;
using RecoverUnsoldDomain.Entities;

namespace RecoverUnsoldApi.Services.Payments;

public class PaymentsService: IPaymentsService
{
    private readonly DataContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    
    public PaymentsService(DataContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task CreatePayment(Guid orderId, string transactionId)
    {
        _context.Payments.Add(new Payment
        {
            OrderId = orderId,
            TransactionId = transactionId
        });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckPaymentSuccess(string transactionId)
    {
        var httpClient = _httpClientFactory.CreateClient("Kkiapay");
        var jsonContent = JsonContent.Create(new KkiapayTransactionDto { TransactionId = transactionId });
        var response = await httpClient.PostAsync("/api/v1/transactions/status",jsonContent);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        var kkiapayResponse = await JsonSerializer.DeserializeAsync<KkiapayResponseDto>(await response.Content.ReadAsStreamAsync());
        return kkiapayResponse?.Status.ToLowerInvariant() == "success";
    }
}