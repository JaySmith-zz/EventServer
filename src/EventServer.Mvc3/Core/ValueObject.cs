using System;
using System.Xml.Serialization;

namespace EventServer.Core
{
    public abstract class ValueObject : IEquatable<ValueObject>, IComparable<ValueObject>, IComparable
    {
        protected ValueObject(string value)
        {
            ValidationUtil.ValidateRequiredStringValue(value, "value");
            Value = value;
        }

        [XmlText]
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }

        public int CompareTo(ValueObject other)
        {
            if (ReferenceEquals(null, other)) return 1;
            if (ReferenceEquals(this, other)) return 0;
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(obj as ValueObject);
        }

        public bool Equals(ValueObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GetType() == other.GetType() && Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ValueObject);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !Equals(left, right);
        }
    }
}