using System;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public class LocationData
    {
        [Serializable]
        public class Chunk
        {
            public long x;
            public long y;

            public Chunk(long x, long y) { this.x = x; this.y = y; }

            public Chunk(Chunk other) { this.x = other.x; this.y = other.y; }

            public override string ToString()
            {
                return "(" + x + ", " + y + ")";
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (obj is Chunk)
                {
                    Chunk other = (Chunk)obj;
                    if (x == other.x && y == other.y) return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                int prime1 = 17;
                int prime2 = 23;

                unchecked
                {
                    long hash = prime1;

                    hash = hash * prime2 + x;
                    hash = hash * prime2 + y;
                    return (int)hash;
                }
            }

            public static bool operator ==(Chunk operand1, Chunk operand2)
            {
                if (operand1 == null)
                {
                    if (operand2 == null) return true;
                    else return false;
                }
                else return operand1.Equals(operand2);
            }

            public static bool operator !=(Chunk operand1, Chunk operand2)
            {
                if (operand1 == null)
                {
                    if (operand2 == null) return false;
                    else return true;
                }
                return !operand1.Equals(operand2);
            }
        }

        [Serializable]
        public class Indices
        {
            public int i;
            public int j;

            public Indices(int i, int j) { this.i = i; this.j = j; }

            public Indices(Indices other) { this.i = other.i; this.j = other.j; }

            public override string ToString()
            {
                return "(" + i + ", " + j + ")";
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (obj is Indices)
                {
                    Indices other = (Indices)obj;
                    if (i == other.i && j == other.j) return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                int prime1 = 17;
                int prime2 = 23;

                unchecked
                {
                    long hash = prime1;

                    hash = hash * prime2 + i;
                    hash = hash * prime2 + j;
                    return (int)hash;
                }
            }
            public static bool operator ==(Indices operand1, Indices operand2)
            {
                if (operand1 == null)
                {
                    if (operand2 == null) return true;
                    else return false;
                }
                else return operand1.Equals(operand2);
            }

            public static bool operator !=(Indices operand1, Indices operand2)
            {
                if (operand1 == null)
                {
                    if (operand2 == null) return false;
                    else return true;
                }
                return !operand1.Equals(operand2);
            }
        }

        [Serializable]
        public class Coordinates
        {
            public Chunk chunk;
            public Indices indices;

            public Coordinates(long x, long y, int i, int j)
            {
                chunk = new Chunk(x, y);
                indices = new Indices(i, j);
            }

            public Coordinates(Coordinates other)
            {
                chunk = new Chunk(other.chunk);
                indices = new Indices(other.indices);
            }

            public override string ToString()
            {
                return chunk + ":" + indices;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;

                if (obj is Coordinates)
                {
                    Coordinates other = (Coordinates)obj;
                    if (chunk.Equals(other.chunk) && indices.Equals(other.indices)) return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                int prime1 = 17;
                int prime2 = 23;

                unchecked
                {
                    long hash = prime1;

                    hash = hash * prime2 + chunk.x;
                    hash = hash * prime2 + chunk.y;
                    hash = hash * prime2 + indices.i;
                    hash = hash * prime2 + indices.j;
                    return (int)hash;
                }
            }

            public static bool operator ==(Coordinates operand1, Coordinates operand2)
            {
                if (operand1 == null)
                {
                    if (operand2 == null) return true;
                    else return false;
                }
                else return operand1.Equals(operand2);
            }

            public static bool operator !=(Coordinates operand1, Coordinates operand2)
            {
                if (operand1 == null)
                {
                    if (operand2 == null) return false;
                    else return true;
                }
                return !operand1.Equals(operand2);
            }
        }

        public int mapIndex;
        public Coordinates coordinates;

        public LocationData(int mapIndex, Coordinates coordinates)
        {
            this.mapIndex = mapIndex; this.coordinates = coordinates;
        }
    }
}
