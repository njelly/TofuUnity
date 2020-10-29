using System;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    public class RequireTypeAttribute : PropertyAttribute
    {
        public Type requireType;
        
        public RequireTypeAttribute(Type requireType)
        {
            this.requireType = requireType;
        }
    }
}