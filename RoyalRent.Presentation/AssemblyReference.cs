using System.Reflection;

namespace RoyalRent.Presentation;

/// <summary>
/// Provides assembly reference for the presentation layer.
/// Used for assembly scanning and registration in dependency injection.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// Gets the current presentation assembly for registration and scanning purposes.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
