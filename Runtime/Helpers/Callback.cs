#if !(ENABLE_IL2CPP || REACT_DISABLE_CLEARSCRIPT)
#define REACT_CLEARSCRIPT
#endif

#if !REACT_DISABLE_JINT
#define REACT_JINT
#endif

using System;
using System.Linq;

#if REACT_JINT
using Jint;
using Jint.Native;
using Jint.Native.Function;
#endif

namespace ReactUnity.Helpers
{
    public class Callback
    {
        public object callback;
#if REACT_JINT
        public Engine Engine;
#endif

        public static Callback From(object value, ReactContext context = null, object thisVal = null)
        {
            if (value == null) return null;
            if (value is string s)
            {
                context.Script.Engine.SetValue("__thisArg", thisVal);
                var fn = context.Script.EvaluateScript(
                    "(function(ts) { delete __thisArg; return (function(event, sender) {\n" + s + "\n}).bind(ts); })(__thisArg)");
                return new Callback(fn);
            }
            if (value is Callback cb) return cb;
#if REACT_JINT
            if (value is Func<JsValue, JsValue[], JsValue> jv) return new Callback(jv, context.Script.Engine.NativeEngine as Engine);
#endif
            return new Callback(value);
        }

#if REACT_JINT
        public Callback(Func<JsValue, JsValue[], JsValue> callback, Engine engine)
        {
            this.callback = callback;
            this.Engine = engine;
        }

        public Callback(FunctionInstance callback)
        {
            this.callback = callback;
        }
#endif

        public Callback(object callback)
        {
            this.callback = callback;
        }

        public object Call()
        {
            return Call(new object[0]);
        }

        public object Call(params object[] args)
        {
            if (callback == null) return null;
            if (args == null) args = new object[0];

            if (callback is Callback c)
            {
                return c.Call(args);
            }
#if REACT_JINT
            else if (callback is JsValue v)
            {
                var fi = v.As<FunctionInstance>();
                return fi.Invoke(args.Select(x => JsValue.FromObject(fi.Engine, x)).ToArray());
            }
            else if (callback is Func<JsValue, JsValue[], JsValue> cb)
            {
                return cb.Invoke(JsValue.Null, args.Select(x => JsValue.FromObject(Engine, x)).ToArray());
            }
#endif
            else if (callback is Delegate d)
            {
                var parameters = d.Method.GetParameters();
                var argCount = parameters.Length;

                if (args.Length < argCount) args = args.Concat(new object[argCount - args.Length]).ToArray();
                if (args.Length > argCount) args = args.Take(argCount).ToArray();
                return d.DynamicInvoke(args);
            }
#if REACT_CLEARSCRIPT
            else if (callback is Microsoft.ClearScript.ScriptObject so)
            {
                return so.Invoke(false, args);
            }
#endif
            else
            {
                return null;
            }
        }
    }
}
