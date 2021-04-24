namespace Unibrics.Utils
{
    public readonly struct ValueOrError<TValue, TError> where TValue : class
    {
        public TValue Value { get; }

        public TError Error { get; }

        public bool HasError => Value == null;

        public bool IsOk => !HasError;

        private ValueOrError(TValue value, TError error)
        {
            Value = value;
            Error = error;
        }

        public static ValueOrError<TValue, TError> FromValue(TValue value)
        {
            return new ValueOrError<TValue, TError>(value, default);
        }

        public static ValueOrError<TValue, TError> FromError(TError error)
        {
            return new ValueOrError<TValue, TError>(default, error);
        }
        
        public static implicit operator ValueOrError<TValue, TError>(TValue value)
        {
            return FromValue(value);
        }
    }
}