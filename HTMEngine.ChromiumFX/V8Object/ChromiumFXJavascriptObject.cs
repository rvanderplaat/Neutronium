﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Chromium;
using Chromium.Remote;
using HTMEngine.ChromiumFX.Convertion;
using MVVM.HTML.Core.JavascriptEngine.JavascriptObject;

namespace HTMEngine.ChromiumFX.V8Object 
{
    internal class ChromiumFXJavascriptObject : IJavascriptObject 
    {
        private readonly CfrV8Value _CfrV8Value;
        public ChromiumFXJavascriptObject(CfrV8Value cfrV8Value) 
        {
            _CfrV8Value = cfrV8Value;
        }

        internal CfrV8Value GetRaw() 
        {
            return _CfrV8Value;
        }

        public void Dispose() 
        {
            _CfrV8Value.Dispose();
        }

        public bool IsUndefined 
        {
            get { return _CfrV8Value.IsUndefined; }
        }
        public bool IsNull 
        {
            get { return _CfrV8Value.IsNull; }
        }

        public bool IsObject 
        {
            get { return _CfrV8Value.IsObject; }
        }

        public bool IsArray 
        {
            get { return _CfrV8Value.IsArray; }
        }

        public bool IsString 
        {
            get { return _CfrV8Value.IsString; }
        }
        public bool IsNumber 
        {
            get { return _CfrV8Value.IsDouble || _CfrV8Value.IsUint || _CfrV8Value.IsInt; }
        }

        public bool IsBool 
        {
            get { return _CfrV8Value.IsBool; }
        }

        public int GetArrayLength() 
        {
            return _CfrV8Value.ArrayLength;
        }

        public bool HasValue(string attributename) 
        {
            return _CfrV8Value.HasValue(attributename);
        }

        public void SetValue(string attributeName, IJavascriptObject element, CreationOption ioption = CreationOption.None) 
        {
            _CfrV8Value.SetValue(attributeName, element.Convert(), (CfxV8PropertyAttribute) ioption);
        }

        public IJavascriptObject Invoke(string functionName, IWebView context, params IJavascriptObject[] parameters) 
        {
            var function = _CfrV8Value.GetValue(functionName);
            return function.ExecuteFunctionWithContext(context.Convert().V8Context, _CfrV8Value, parameters.Convert()).Convert();
        }

        public Task<IJavascriptObject> InvokeAsync(string functionName, IWebView context, params IJavascriptObject[] parameters) 
        {
            return Task.FromResult(Invoke(functionName, context, parameters));
        }

        public void Bind(string functionName, IWebView context, Action<string, IJavascriptObject, IJavascriptObject[]> action) 
        {
            var func = CfrV8Value.CreateFunction(functionName, action.Convert(functionName));
            _CfrV8Value.SetValue(functionName, func, CfxV8PropertyAttribute.None);
        }

        public IJavascriptObject ExecuteFunction(IWebView context) 
        {
            return _CfrV8Value.ExecuteFunction(_CfrV8Value, new CfrV8Value[0]).Convert();
        }

        public bool HasRelevantId() 
        {
            return _CfrV8Value.HasValue("_MappedId");
        }

        public uint GetID() 
        {
            return (_CfrV8Value.HasValue("_MappedId")) ? _CfrV8Value.GetValue("_MappedId").UintValue : 0;
        }

        public string GetStringValue() 
        {
            return _CfrV8Value.StringValue;
        }

        public double GetDoubleValue() 
        {
            return _CfrV8Value.DoubleValue;
        }

        public bool GetBoolValue() 
        {
            return _CfrV8Value.BoolValue;
        }

        public int GetIntValue() 
        {
            return _CfrV8Value.IntValue;
        }

        public IJavascriptObject GetValue(string ivalue) 
        {
            return _CfrV8Value.GetValue(ivalue).Convert();
        }

        public IJavascriptObject GetValue(int index) 
        {
            return _CfrV8Value.GetValue(index).Convert();
        }

        public IJavascriptObject[] GetArrayElements() 
        {
            if (!_CfrV8Value.IsArray)
                return null;

            var length = _CfrV8Value.ArrayLength;
            return Enumerable.Range(0, length).Select(_CfrV8Value.GetValue).Convert();
        }
    }
}
