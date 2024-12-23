using System.Numerics;

// ReSharper disable MemberCanBePrivate.Global

namespace RangeFinder.Core;

/// <summary>
/// A basic implementation of INumericRange.
/// This record is defined because the query requires at least one concrete type.
/// It is sealed to encourage users to implement their own INumericRange using custom types
/// to manage additional data within the elements, while avoiding inheritance constraints.
/// </summary>
public sealed record NumericRange<TNumber>(TNumber Start, TNumber End)
    : INumericRange<TNumber> where TNumber: INumber<TNumber>
{
    [Obsolete("This constructor should be used only for serialization purposes.")]
    public NumericRange() : this(TNumber.Zero, TNumber.Zero) { }
}