using System.Text.Json.Serialization;

namespace Cmpnnt.StreamDeckToolkit.Communication.Registration
{
    /// <summary>
    /// Layout of the keys on the StreamDeck hardware device
    /// </summary>
    public class StreamDeckDeviceSize
    {
        /// <summary>
        /// Number of key rows on the StreamDeck hardware device
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Number of key columns on the StreamDeck hardware device
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        [JsonConstructor]
        public StreamDeckDeviceSize(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        /// <summary>Default constructor for object initializer usage.</summary>
        public StreamDeckDeviceSize() { }

        /// <summary>
        /// Shows class information as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Rows: {Rows} Columns: {Columns}";
        }
    }
}
