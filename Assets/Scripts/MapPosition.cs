using System;

[Serializable]
public class MapPosition : IEquatable<MapPosition>
{
    public int X;
    public int Y;

    public MapPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as MapPosition);
    }

    public bool Equals(MapPosition other)
    {
        return other != null &&
               X == other.X &&
               Y == other.Y;
    }

    public override int GetHashCode()
    {
        var hashCode = 1861411795;
        hashCode = hashCode * -1521134295 + X.GetHashCode();
        hashCode = hashCode * -1521134295 + Y.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return X + "," + Y;
    }

    public static bool operator ==(MapPosition obj1, MapPosition obj2)
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return true;
        }

        if (ReferenceEquals(obj1, null))
        {
            return ReferenceEquals(obj2, null);
        }
        if (ReferenceEquals(obj2, null))
        {
            return false;
        }
        return  obj1.X == obj2.X && obj1.Y == obj2.Y;
    }

    public static bool operator !=(MapPosition obj1, MapPosition obj2)
    {
        return !(obj1 == obj2);
    }
}