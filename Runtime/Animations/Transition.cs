using ReactUnity.Animations;
using ReactUnity.Styling;
using System.Collections.Generic;
using UnityEngine;

namespace ReactUnity
{
    public class TransitionList
    {
        public Dictionary<string, Transition> Transitions { get; } = new Dictionary<string, Transition>();
        public Transition All { get; private set; }

        public TransitionList()
        {

        }

        public TransitionList(Transition tr)
        {
            AddTransition(tr);
        }

        public TransitionList(string value)
        {
            var splits = value.Split(',');

            foreach (var split in splits)
            {
                var tr = new Transition(split);
                AddTransition(tr);
            }
        }

        private void AddTransition(Transition tr)
        {
            if (tr.Property == null || tr.Property == "all") Transitions["all"] = All = tr;
            else Transitions[tr.Property] = tr;
        }
    }

    public class Transition
    {
        public float Delay { get; } = 0;
        public float Duration { get; } = 0;
        public string Property { get; } = "all";
        public TimingFunctions.TimingFunction TimingFunction { get; } = TimingFunctions.GetTimingFunction(TimingFunctionType.Linear);
        public bool Valid { get; } = true;

        public Transition() { }

        public Transition(string value)
        {
            var splits = value.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);


            if (splits.Length == 0)
            {
                Valid = false;
                return;
            }

            var offset = 1;
            var firstChar = splits[0][0];

            if (char.IsDigit(firstChar) || firstChar == '.')
            {
                offset = 0;
            }
            else
            {
                Property = splits[0];
                if (splits.Length < 2)
                {
                    Valid = false;
                    return;
                }
            }


            var dur = ConverterMap.DurationConverter.Convert(splits[offset]);
            if (dur is float f) Duration = f;
            else Valid = false;

            if (splits.Length > offset + 1)
            {
                var next = splits[offset + 1];
                var last = splits.Length > offset + 2 ? splits[offset + 2] : null;
                var nextIsDelay = false;

                firstChar = next[0];
                if (char.IsDigit(firstChar) || firstChar == '.') nextIsDelay = true;

                var delayString = nextIsDelay ? next : last ?? "0";
                var easing = nextIsDelay ? last ?? "linear" : next;

                dur = ConverterMap.DurationConverter.Convert(delayString);
                if (dur is float delay) Delay = delay;
                else Valid = false;

                var easingFunction = TimingFunctions.GetTimingFunction(easing);
                if (easingFunction != null) TimingFunction = easingFunction;
            }
        }
    }

    public static class StandardTransitions
    {
        public static int Interpolate(int start, int end, float t)
        {
            return Mathf.RoundToInt((end - start) * t + start);
        }

        public static float Interpolate(float start, float end, float t)
        {
            return (end - start) * t + start;
        }
    }
}
