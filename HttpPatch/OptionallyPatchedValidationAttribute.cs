using System.ComponentModel.DataAnnotations;

namespace HttpPatch;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
public abstract class OptionallyPatchedValidationAttribute : Attribute
{
    protected OptionallyPatchedValidationAttribute(ValidationAttribute validationAttribute)
    {
        ValidationAttribute = validationAttribute;
    }
    public ValidationAttribute ValidationAttribute { get; }
}

public sealed class MinLengthIfPatchedAttribute : OptionallyPatchedValidationAttribute
{
    public MinLengthIfPatchedAttribute(int minLength)
        : base(new MinLengthAttribute(minLength))
    {
        MinLength = minLength;
    }

    public int MinLength { get; }
}

public sealed class MaxLengthIfPatchedAttribute : OptionallyPatchedValidationAttribute
{
    public MaxLengthIfPatchedAttribute(int maxLength)
        : base(new MaxLengthAttribute(maxLength))
    {
        MaxLength = maxLength;
    }

    public int MaxLength { get; }
}


public sealed class RequiredIfPatchedAttribute : OptionallyPatchedValidationAttribute
{
    public RequiredIfPatchedAttribute()
        : base(new RequiredAttribute())
    {
    }
}

public sealed class RangeIfPatchedAttribute : OptionallyPatchedValidationAttribute
{
    public double Minimum { get; }

    public double Maximum { get; }

    public RangeIfPatchedAttribute(double minimum, double maximum)
        : base(new RangeAttribute(minimum, maximum))
    {
        Minimum = minimum;
        Maximum = maximum;
    }
}