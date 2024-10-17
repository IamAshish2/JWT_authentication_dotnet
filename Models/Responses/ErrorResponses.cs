namespace JWT_AUTHENTICATION.Models.Responses
{
    public class ErrorResponses
    {
        public IEnumerable<string> ErrorMessages { get; set; } = null!;
        public ErrorResponses(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public ErrorResponses(string errorMessage) : this(new List<string>() { errorMessage }) { }

    }
}
