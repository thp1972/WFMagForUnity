using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MathUtility
{
    /// <summary>
    // The formula made famous by Pythagoras, also used internally by
    // Vector3.Distance and several other standard functions.
    /// </summary>
    public static float Hypo((float, float) sides)
    {
        return Mathf.Sqrt(sides.Item1 * sides.Item1 + sides.Item2 * sides.Item1);
    }
}

