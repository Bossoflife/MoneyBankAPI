namespace MoneyBankAPI.Models
{
    public class Response
    {
        public string ResponseId => $"{Guid.NewGuid()}";
        public string? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public object? Date { get; set; }
    }
}
