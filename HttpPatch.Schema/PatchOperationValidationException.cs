using System.Runtime.Serialization;

namespace HttpPatch.Schema
{
    [Serializable]
    public class PatchOperationValidationException : Exception
    {
        public PatchOperationValidationException()
        {
        }

        public PatchOperationValidationException(string message) : base(message)
        {
        }

        public PatchOperationValidationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected PatchOperationValidationException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context)
        {
        }
    }
}