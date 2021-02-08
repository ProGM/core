using ReactUnity.Types;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ReactUnity.Styling.Parsers
{
    public class VideoReferenceConverter : IStyleParser, IStyleConverter
    {

        private static Regex DataRegex = new Regex(@"^data:(?<mime>[\w/\-\.]+)?(;(?<encoding>\w+))?,?(?<data>.*)", RegexOptions.Compiled);
        private static Regex ProceduralRegex = new Regex("^procedural://");
        private static Regex GlobalRegex = new Regex("^globals?://");
        private static Regex ResourceRegex = new Regex("^res(ources?)?://");
        private static Regex FileRegex = new Regex("^file://");
        private static Regex HttpRegex = new Regex("^https?://");

        public object Convert(object value)
        {
            if (value == null) return VideoReference.None;
            if (value is Texture2D t) return new VideoReference(AssetReferenceType.Object, t);
            if (value is Sprite s) return new VideoReference(AssetReferenceType.Object, s.texture);
            if (value is Object o) return new VideoReference(AssetReferenceType.Object, o);
            return FromString(ParserMap.UrlConverter.Convert(value) as string);
        }

        public object FromString(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return VideoReference.None;
            if (FileRegex.IsMatch(value)) return new VideoReference(AssetReferenceType.File, FileRegex.Replace(value, ""));
            if (HttpRegex.IsMatch(value)) return new VideoReference(AssetReferenceType.Url, value);
            if (GlobalRegex.IsMatch(value)) return new VideoReference(AssetReferenceType.Global, GlobalRegex.Replace(value, ""));
            if (ProceduralRegex.IsMatch(value)) return new VideoReference(AssetReferenceType.Procedural, ProceduralRegex.Replace(value, ""));
            if (ResourceRegex.IsMatch(value)) return new VideoReference(AssetReferenceType.Resource, ResourceRegex.Replace(value, ""));

            var dataMatch = DataRegex.Match(value);
            if (dataMatch.Success)
            {
                var mime = dataMatch.Groups["mime"].Value;
                var encoding = dataMatch.Groups["encoding"].Value;
                var data = dataMatch.Groups["data"].Value;
                return new VideoReference(AssetReferenceType.Data, data);
            }

            return new VideoReference(AssetReferenceType.Auto, value);
        }
    }
}
