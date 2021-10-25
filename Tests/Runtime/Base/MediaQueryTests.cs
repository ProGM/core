using System.Collections;
using NUnit.Framework;
using ReactUnity.Scripting;
using UnityEngine;

namespace ReactUnity.Tests
{
    public class MediaQueryTests : TestBase
    {
        public MediaQueryTests(JavascriptEngineType engineType) : base(engineType) { }

        [ReactInjectableTest(style: "view { color: white; } @media (min-asd: 600px) { view { color: red; } }")]
        public IEnumerator Injectable_HelloWorld()
        {
            yield return null;
            var tmp = Canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            Assert.AreEqual(Color.white, tmp.color);

            MediaProvider.SetNumber("asd", 600);
            yield return null;
            Assert.AreEqual(Color.red, tmp.color);
        }
    }
}
