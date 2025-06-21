namespace Trixx.Common.Exceptions
{
    [Serializable]
    public class TrixxException : Exception
    {
        public TrixxException()
        {
        }

        public TrixxException(string message) : base(message)
        {
        }

        public TrixxException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
