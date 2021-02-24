using System;
using System.Collections.Generic;
using System.Text;

namespace YDK.ComponentSystem
{
    [Serializable]
    public class Field<T>
    {
        public readonly FieldType Type;
        public readonly string Name;
        private T _value;

        public Field(string name, FieldType fieldType)
        {
            _value = default(T);
            this.Type = fieldType;
            this.Name = name;
        }

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            this._value = value;
        }
    }

    [Serializable]
    public enum FieldType
    {
        Number,
        Text,
        LongText,
        File,
        NoDisplay
    }
}
