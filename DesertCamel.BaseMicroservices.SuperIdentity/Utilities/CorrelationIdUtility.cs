namespace DesertCamel.BaseMicroservices.SuperIdentity.Utilities
{
    public class CorrelationIdUtility : ICorrelationIdUtility
    {
        private string _correlationId = Guid.NewGuid().ToString();

        public string Get() => _correlationId;

        public void Set(string correlationId)
        {
            _correlationId = correlationId;
        }
    }

    public interface ICorrelationIdUtility
    {
        string Get();
        void Set(string correlationId);
    }
}
