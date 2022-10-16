using System;
namespace ArchiNET.Droid.Assets
{
    public interface ICloneable
    {
        object Clone();
    }
    public enum TokenType
    {
        PropertyName,StartObject,EndObject,StartArray,EndArray,String,Int,Null,Undefined
    }
    public abstract class JsonToken : ICloneable
    {
        public virtual object Value { get; }
        public virtual TokenType Type { get; }
        public JsonToken(object value,TokenType type)
        {
            Value = value;
            Type = type;
        }
        public abstract object Clone();
    }
    public sealed class SimpleJsonToken : JsonToken
    {
        public override object Value { get; }
        public override TokenType Type { get; }
        public SimpleJsonToken(object value, TokenType type) : base(value, type)
        {
            
        }
        public override object Clone()
        {
            return new SimpleJsonToken(Value, Type);
        }
    }
}