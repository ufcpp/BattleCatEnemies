namespace BattleCat;

public struct Array3<T>
{
    private readonly byte _length;
    private readonly T _x1, _x2, _x3;

    private Array3(byte length, T x1, T x2 = default!, T x3 = default!) => (_length, _x1, _x2, _x3) = (length, x1, x2, x3);

    public Array3(T x1) : this(1, x1) { }
    public Array3(T x1, T x2) : this(2, x1, x2) { }
    public Array3(T x1, T x2, T x3) : this(3, x1, x2, x3) { }

    public void Deconstruct(out T x1, out T x2)
    {
        if (2 > _length) throw new IndexOutOfRangeException();
        (x1, x2) = (_x1, _x2);
    }

    public void Deconstruct(out T x1, out T x2, out T x3)
    {
        if (3 > _length) throw new IndexOutOfRangeException();
        (x1, x2, x3) = (_x1, _x2, _x3);
    }

    public int Length => _length;

    public T this[int index]
    {
        get
        {
            if ((uint)index >= _length) throw new IndexOutOfRangeException();
            return index switch
            {
                0 => _x1,
                1 => _x2,
                2 => _x3,
                _ => throw new IndexOutOfRangeException(),
            };
        }
    }
}

public static class ArrayExtensions
{
    public static Array3<T2> Select<T1, T2>(this Array3<T1> array, Func<T1, T2> selector) => array.Length switch
    {
        1 => new(selector(array[0])),
        2 => new(selector(array[0]), selector(array[1])),
        3 => new(selector(array[0]), selector(array[1]), selector(array[2])),
        _ => new(),
    };

    public static Array3<T> Sort<T>(this Array3<T> array)
        where T : IComparable<T>
    {
        if (array.Length <= 1) return array;

        if (array.Length == 2)
        {
            var (x1, x2) = array;
            if (x1.CompareTo(x2) < 0) (x1, x2) = (x2, x1);
            return new(x1, x2);
        }
        else
        {
            var (x1, x2, x3) = array;
            if (x1.CompareTo(x2) > 0) (x1, x2) = (x2, x1);
            if (x1.CompareTo(x3) > 0) (x1, x3) = (x3, x1);
            if (x2.CompareTo(x3) > 0) (x2, x3) = (x3, x2);
            return new(x1, x2, x3);
        }
    }
}
