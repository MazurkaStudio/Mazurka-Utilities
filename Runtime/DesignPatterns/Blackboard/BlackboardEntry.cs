﻿using System;

namespace TheMazurkaStudio.Utilities
{
    [Serializable]
    public class BlackboardEntry<T>
    {
        public BlackboardEntry(BlackboardKey key, T value)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();

        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }
    }
}