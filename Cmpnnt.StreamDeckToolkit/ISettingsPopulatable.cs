using System.Text.Json;
using Cmpnnt.StreamDeckToolkit.Attributes;

namespace Cmpnnt.StreamDeckToolkit
{
    /// <summary>
    /// Implemented by settings classes that support AOT-compatible JSON merge/patch population.
    /// Apply <see cref="SdSettingsAttribute"/> to a <c>partial</c> settings
    /// class and the source generator will implement this interface automatically.
    /// </summary>
    public interface ISettingsPopulatable
    {
        /// <summary>
        /// Merges property values from a <see cref="JsonElement"/> into this settings instance.
        /// Only properties present in the JSON object are updated; all others are left unchanged.
        /// </summary>
        /// <param name="element">The JSON object containing the values to merge.</param>
        /// <returns>The number of properties successfully populated.</returns>
        int PopulateFromJson(JsonElement element);
    }
}
