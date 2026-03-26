using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Payloads
{
    /// <summary>
    /// Coordinates of the current key
    /// </summary>
    public class KeyCoordinates
    {
        /// <summary>
        /// Column of the current key
        /// </summary>
        [JsonPropertyName("column")]
        public int Column { get; set; }

        /// <summary>
        /// Row of the current key
        /// </summary>
        [JsonPropertyName("row")]
        public int Row { get; set; }

        /// <summary>JSON deserialization constructor.</summary>
        [JsonConstructor]
        public KeyCoordinates(int column, int row)
        {
            Column = column;
            Row = row;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public KeyCoordinates() { }
    }
}

