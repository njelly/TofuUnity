using UnityEditor;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    /// <summary>
    /// Attribute that require implementation of the provided interface.
    /// Source: https://www.patrykgalach.com/2020/01/27/assigning-interface-in-unity-inspector/
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        // Interface type.
        public System.Type requiredType { get; private set; }
        /// <summary>
        /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
        /// </summary>
        /// <param name="type">Interface type.</param>
        public RequireInterfaceAttribute(System.Type type)
        {
            this.requiredType = type;
        }
    }
}