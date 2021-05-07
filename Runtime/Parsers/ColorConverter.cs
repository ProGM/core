using ReactUnity.Styling.Types;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ReactUnity.Styling.Parsers
{
    public class ColorConverter : IStyleParser, IStyleConverter
    {
        IStyleConverter floatDs = ConverterMap.FloatConverter;
        static char[] splitChars = new char[] { ',', ' ' };
        static Regex rgbRegex = new Regex("rgba?\\((?<values>.*)\\)");

        static Dictionary<string, string> KnownColors = new Dictionary<string, string> {
            { "aliceblue", "#f0f8ff" },
            { "antiquewhite", "#faebd7" },
            { "aqua", "#00ffff" },
            { "aquamarine", "#7fffd4" },
            { "azure", "#f0ffff" },
            { "beige", "#f5f5dc" },
            { "bisque", "#ffe4c4" },
            { "black", "#000000" },
            { "blanchedalmond", "#ffebcd" },
            { "blue", "#0000ff" },
            { "blueviolet", "#8a2be2" },
            { "brown", "#a52a2a" },
            { "burlywood", "#deb887" },
            { "cadetblue", "#5f9ea0" },
            { "chartreuse", "#7fff00" },
            { "chocolate", "#d2691e" },
            { "coral", "#ff7f50" },
            { "cornflowerblue", "#6495ed" },
            { "cornsilk", "#fff8dc" },
            { "crimson", "#dc143c" },
            { "cyan", "#00ffff" },
            { "darkblue", "#00008b" },
            { "darkcyan", "#008b8b" },
            { "darkgoldenrod", "#b8860b" },
            { "darkgray", "#a9a9a9" },
            { "darkgreen", "#006400" },
            { "darkgrey", "#a9a9a9" },
            { "darkkhaki", "#bdb76b" },
            { "darkmagenta", "#8b008b" },
            { "darkolivegreen", "#556b2f" },
            { "darkorange", "#ff8c00" },
            { "darkorchid", "#9932cc" },
            { "darkred", "#8b0000" },
            { "darksalmon", "#e9967a" },
            { "darkseagreen", "#8fbc8f" },
            { "darkslateblue", "#483d8b" },
            { "darkslategray", "#2f4f4f" },
            { "darkslategrey", "#2f4f4f" },
            { "darkturquoise", "#00ced1" },
            { "darkviolet", "#9400d3" },
            { "deeppink", "#ff1493" },
            { "deepskyblue", "#00bfff" },
            { "dimgray", "#696969" },
            { "dimgrey", "#696969" },
            { "dodgerblue", "#1e90ff" },
            { "firebrick", "#b22222" },
            { "floralwhite", "#fffaf0" },
            { "forestgreen", "#228b22" },
            { "fuchsia", "#ff00ff" },
            { "gainsboro", "#dcdcdc" },
            { "ghostwhite", "#f8f8ff" },
            { "goldenrod", "#daa520" },
            { "gold", "#ffd700" },
            { "gray", "#808080" },
            { "green", "#008000" },
            { "greenyellow", "#adff2f" },
            { "grey", "#808080" },
            { "honeydew", "#f0fff0" },
            { "hotpink", "#ff69b4" },
            { "indianred", "#cd5c5c" },
            { "indigo", "#4b0082" },
            { "ivory", "#fffff0" },
            { "khaki", "#f0e68c" },
            { "lavenderblush", "#fff0f5" },
            { "lavender", "#e6e6fa" },
            { "lawngreen", "#7cfc00" },
            { "lemonchiffon", "#fffacd" },
            { "lightblue", "#add8e6" },
            { "lightcoral", "#f08080" },
            { "lightcyan", "#e0ffff" },
            { "lightgoldenrodyellow", "#fafad2" },
            { "lightgray", "#d3d3d3" },
            { "lightgreen", "#90ee90" },
            { "lightgrey", "#d3d3d3" },
            { "lightpink", "#ffb6c1" },
            { "lightsalmon", "#ffa07a" },
            { "lightseagreen", "#20b2aa" },
            { "lightskyblue", "#87cefa" },
            { "lightslategray", "#778899" },
            { "lightslategrey", "#778899" },
            { "lightsteelblue", "#b0c4de" },
            { "lightyellow", "#ffffe0" },
            { "lime", "#00ff00" },
            { "limegreen", "#32cd32" },
            { "linen", "#faf0e6" },
            { "magenta", "#ff00ff" },
            { "maroon", "#800000" },
            { "mediumaquamarine", "#66cdaa" },
            { "mediumblue", "#0000cd" },
            { "mediumorchid", "#ba55d3" },
            { "mediumpurple", "#9370db" },
            { "mediumseagreen", "#3cb371" },
            { "mediumslateblue", "#7b68ee" },
            { "mediumspringgreen", "#00fa9a" },
            { "mediumturquoise", "#48d1cc" },
            { "mediumvioletred", "#c71585" },
            { "midnightblue", "#191970" },
            { "mintcream", "#f5fffa" },
            { "mistyrose", "#ffe4e1" },
            { "moccasin", "#ffe4b5" },
            { "navajowhite", "#ffdead" },
            { "navy", "#000080" },
            { "oldlace", "#fdf5e6" },
            { "olive", "#808000" },
            { "olivedrab", "#6b8e23" },
            { "orange", "#ffa500" },
            { "orangered", "#ff4500" },
            { "orchid", "#da70d6" },
            { "palegoldenrod", "#eee8aa" },
            { "palegreen", "#98fb98" },
            { "paleturquoise", "#afeeee" },
            { "palevioletred", "#db7093" },
            { "papayawhip", "#ffefd5" },
            { "peachpuff", "#ffdab9" },
            { "peru", "#cd853f" },
            { "pink", "#ffc0cb" },
            { "plum", "#dda0dd" },
            { "powderblue", "#b0e0e6" },
            { "purple", "#800080" },
            { "rebeccapurple", "#663399" },
            { "red", "#ff0000" },
            { "rosybrown", "#bc8f8f" },
            { "royalblue", "#4169e1" },
            { "saddlebrown", "#8b4513" },
            { "salmon", "#fa8072" },
            { "sandybrown", "#f4a460" },
            { "seagreen", "#2e8b57" },
            { "seashell", "#fff5ee" },
            { "sienna", "#a0522d" },
            { "silver", "#c0c0c0" },
            { "skyblue", "#87ceeb" },
            { "slateblue", "#6a5acd" },
            { "slategray", "#708090" },
            { "slategrey", "#708090" },
            { "snow", "#fffafa" },
            { "springgreen", "#00ff7f" },
            { "steelblue", "#4682b4" },
            { "tan", "#d2b48c" },
            { "teal", "#008080" },
            { "thistle", "#d8bfd8" },
            { "tomato", "#ff6347" },
            { "turquoise", "#40e0d0" },
            { "violet", "#ee82ee" },
            { "wheat", "#f5deb3" },
            { "white", "#ffffff" },
            { "whitesmoke", "#f5f5f5" },
            { "yellow", "#ffff00" },
            { "yellowgreen", "#9acd32" },
        };


        public object FromString(string value)
        {
            if (value == null) return SpecialNames.CantParse;
            if (KnownColors.TryGetValue(value, out var known)) value = known;
            if (value == "clear" || value == "transparent") return Color.clear;
            if (ColorUtility.TryParseHtmlString(value, out var color)) return color;


            var rgbMatch = rgbRegex.Match(value);
            if (rgbMatch.Success)
            {
                var values = rgbMatch.Groups["values"].Value;
                return FromArray(values.Split(splitChars, System.StringSplitOptions.RemoveEmptyEntries), 1f / 255);
            }

            if (value.Contains(',') || value.Contains(' ')) return FromArray(value.Split(splitChars, System.StringSplitOptions.RemoveEmptyEntries));
            return SpecialNames.CantParse;
        }

        public object Convert(object value)
        {
            if (value is Color c) return c;
            if (value is string s)
            {
                var res = FromString(s);
                if (res is Color) return res;
            }
            else if (value is IEnumerable en) return FromArray(en);

            if (value == null) return SpecialNames.CantParse;
            var fl = floatDs.Convert(value);
            if (fl is float f) return new Color(f, f, f);
            return FromString(value?.ToString());
        }

        private object FromArray(IEnumerable obj, float rgbScale = 1, float alphaScale = 1)
        {
            var arr = obj.OfType<object>().ToArray();
            var len = arr.Length;

            if (len == 0) return Color.clear;

            var v0 = arr.ElementAtOrDefault(0);
            var v1 = arr.ElementAtOrDefault(1);
            var v2 = arr.ElementAtOrDefault(2);
            var v3 = arr.ElementAtOrDefault(3);


            var v0f = floatDs.Convert(v0);
            var v1f = floatDs.Convert(v1);
            var v2f = floatDs.Convert(v2);
            var v3f = floatDs.Convert(v3);

            if (!(v0f is float))
            {
                if (len == 2 || len == 3)
                {
                    var startColor = Convert(v0);
                    var endColor = Convert(v2);

                    if (startColor is Color s)
                    {
                        if (endColor is Color e)
                        {
                            var t = v1f as float? ?? 0.5f;
                            return Color.LerpUnclamped(s, e, t);
                        }
                        else
                        {
                            var t = v1f as float? ?? 1;
                            s.a = t;
                            return s;
                        }
                    }
                }
            }

            var r = v0f as float? ?? 0;
            var g = v1f as float? ?? 0;
            var b = v2f as float? ?? 0;
            var a = v3f as float? ?? 1;

            return new Color(r * rgbScale, g * rgbScale, b * rgbScale, a * alphaScale);
        }
    }
}
