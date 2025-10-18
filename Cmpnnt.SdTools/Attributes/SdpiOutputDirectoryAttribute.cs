using System;

namespace Cmpnnt.SdTools.Attributes
{
    /// <summary>
    /// SdpiOutputDirectoryAttribute - Specifies the output directory for the generated SDPI HTML file.
    /// The HTML filename will be the name of the decorated class with a .html extension.
    /// Defaults to `PropertyInspector/`
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SdpiOutputDirectoryAttribute : Attribute
    {
        /// <summary>
        /// The output directory for the generated SDPI HTML file
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Constructor for SdpiOutputDirectoryAttribute
        /// </summary>
        /// <param name="outputDirectory">The output directory for the SDPI inspector HTML file.</param>
        public SdpiOutputDirectoryAttribute(string outputDirectory = "PropertyInspector/")
        {
            OutputDirectory = outputDirectory;
        }
    }
}