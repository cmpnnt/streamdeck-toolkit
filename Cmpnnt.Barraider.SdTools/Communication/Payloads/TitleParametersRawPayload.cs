using System.Text.Json.Serialization;

namespace BarRaider.SdTools.Communication.Payloads
{
    /// <summary>
    /// Raw payload for TitleParametersRawPayload event (without objects)
    /// </summary>
    public class TitleParametersRawPayload
    {
        /// <summary>
        /// Name of font family
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// Size of font
        /// </summary>
        public uint FontSize { get; set; }

        /// <summary>
        /// Style of font (bold, italic)
        /// </summary>
        public string FontStyle { get; set; }

        /// <summary>
        /// Is there an underling
        /// </summary>
        public bool FontUnderline { get; set; }

        /// <summary>
        /// Should title be shown
        /// </summary>
        public bool ShowTitle { get; set; }

        /// <summary>
        /// Alignment of title (top, middle, bottom)
        /// </summary>
        public string TitleAlignment { get; set; }

        /// <summary>
        /// Color of title
        /// </summary>
        public string TitleColor { get; set; }

        [JsonConstructor]
        public TitleParametersRawPayload(string fontFamily, uint fontSize, string fontStyle, bool fontUnderline, bool showTitle, string titleAlignment, string titleColor)
        {
            FontFamily = fontFamily;
            FontSize = fontSize;
            FontStyle = fontStyle;
            FontUnderline = fontUnderline;
            ShowTitle = showTitle;
            TitleAlignment = titleAlignment;
            TitleColor = titleColor;
        }

        public TitleParametersRawPayload() {}
    }
}
