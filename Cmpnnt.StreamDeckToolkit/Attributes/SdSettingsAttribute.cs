using System;

namespace Cmpnnt.StreamDeckToolkit.Attributes
{
    /// <summary>
    /// Marks a settings class for AOT-compatible source-generated JSON population.
    /// The class must also be declared as <c>partial</c> for the source generator to implement it
    /// <see cref="ISettingsPopulatable"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SdSettingsAttribute : Attribute { }
}
