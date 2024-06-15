namespace Shared;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSucess, Error error) : base(isSucess, error)
        => _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value is not available on a failure result.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);

}
