namespace DesertCamel.BaseMicroservices.SuperIdentity.Models
{
    public class FuncListResponse<T>
    {
        public List<T> Data { get; set; }
        public long Total { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsError()
        {
            return !String.IsNullOrWhiteSpace(ErrorMessage);
        }
    }
}
