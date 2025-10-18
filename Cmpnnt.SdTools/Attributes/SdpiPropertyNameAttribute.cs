using System;

namespace Cmpnnt.SdTools.Attributes
{
    /// <summary>
    /// SdpiPropertyNameAttribute - Specifies the name of an SDPI property on a class property.
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SdpiPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// The name of the property in SDPI
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructor for SdpiPropertyNameAttribute
        /// </summary>
        /// <param name="name">The name of the SDPI property.</param>
        public SdpiPropertyNameAttribute(string name)
        {
            Name = name;
        }
    }
}