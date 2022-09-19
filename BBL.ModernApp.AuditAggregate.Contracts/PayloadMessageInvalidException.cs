using System.ComponentModel.DataAnnotations;

namespace BBL.ModernApp.AuditAggregate.Contracts
{
    [Serializable]
    public sealed class PayloadMessageInvalidException : Exception
    {
        private List<ValidationResult> validationResults;
        public List<ValidationResult> ValidationResults { get => validationResults; }

        public PayloadMessageInvalidException(List<ValidationResult> validationResults)
             : base("Payload Creation has Invalid Properties Value.")
        {
            this.validationResults = validationResults;
        }

    }
}