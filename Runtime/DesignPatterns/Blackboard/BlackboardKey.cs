using System;

namespace TheMazurkaStudio.Utilities
{
    [Serializable]
    public readonly struct BlackboardKey : IEquatable<BlackboardKey>
    {
        private readonly string name;
        private readonly int hashedKey;

        public BlackboardKey(string name)
        {
            this.name = name;
            hashedKey = this.name.ComputeFNV1aHash();
        }


        
        
        public bool Equals(BlackboardKey other) => other.hashedKey == hashedKey;
        public override int GetHashCode() => hashedKey;
        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
    }
}