using Avalonia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aura.UI.Neumorphism
{
    internal static class Utils
    {
        internal static Rect WithoutPosition(this Rect rect) => new Rect(0,0, rect.Width, rect.Height);
    }
}
