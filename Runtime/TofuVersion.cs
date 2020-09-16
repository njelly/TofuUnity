using System;
using UnityEngine;

namespace Tofunaut.TofuUnity
{
    /// <summary>
    /// Represents a version in the form of integers [major].[minor].[build], as well as utility methods and 
    /// functions for validating versions and converting to and from a string.
    /// </summary>
    public class TofuVersion
    {
        public const char Delimiter = '.';

        public readonly int major;
        public readonly int minor;
        public readonly int build;

        public TofuVersion(string s)
        {
            string[] parts = s.Split(Delimiter);
            if (parts.Length != 3)
            {
                Debug.LogError("A Version string must be in the format [major].[minor].[build], i.e., three integers separated by the character '.'");
            }
            if (!int.TryParse(parts[0], out major))
            {
                Debug.LogError($"Could not parse the string [{parts[0]}] to an integer for the major version");
            }
            if (!int.TryParse(parts[1], out minor))
            {
                Debug.LogError($"Could not parse the string [{parts[1]}] to an integer for the minor version");
            }
            if (!int.TryParse(parts[2], out build))
            {
                Debug.LogError($"Could not parse the string [{parts[2]}] to an integer for the build version");
            }
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}{1}{3}", major, Delimiter, minor, build);
        }

        public static implicit operator string(TofuVersion v)
        {
            return v.ToString();
        }

        public static implicit operator TofuVersion(string s)
        {
            return new TofuVersion(s);
        }

        /// <summary>
        /// Returns true if the major version of required and current are equal, AND the minor version of required is less than or equal to the current. The build version is ignored.
        /// </summary>
        public static bool IsValid(TofuVersion required, TofuVersion current)
        {
            if (required.major != current.major)
            {
                return false;
            }
            if (required.minor > current.minor)
            {
                return false;
            }

            return true;
        }
    }
}
