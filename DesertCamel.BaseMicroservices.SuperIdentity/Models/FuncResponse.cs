namespace DesertCamel.BaseMicroservices.SuperIdentity.Models
{
    public class FuncResponse<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public bool IsError()
        {
            return !String.IsNullOrWhiteSpace(ErrorMessage);
        }
    }
}
