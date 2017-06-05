using System;

public struct Vector3<T>
{
    public T x;
    public T y;
    public T z;
    public Vector3(T _x, T _y, T _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
}
public struct Vector2<T>
{
    public T x;
    public T y;
    public Vector2(T _x, T _y)
    {
        x = _x;
        y = _y;
    }
}
public class ObjectMap
{
    public Vector2<int> zero = new Vector2<int>(0, 0);
    public enum direction { N = 0, S = 1, W = 2, E = 3 }
    public char[,] MapArray;

    private int[,] iMapArray;
    //public ObjectMap(int x, int y)
    //{
    // int k = 0;
    // iMapArray = new int[x, y];
    // for (int i = 0; i < x; i++)
    // for (int j = 0; j < y; j++)
    // {
    // 
    // iMapArray[i, j] = k;
    // k++;
    // }
    //}
    public ObjectMap(Vector2<int> vec)
    {
        int k = 64;
        MapArray = new char[vec.x, vec.y];
        for (int i = 0; i < vec.x; i++)
            for (int j = 0; j < vec.y; j++)
            {
                MapArray[i, j] = '.';
                k++;
            }
    }
    public ObjectMap(int x, int y)
    {
        int k = 50;
        MapArray = new char[x, y];
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
            {
                MapArray[i, j] = '.';
                k++;
            }
    }
    public void PrintMap()
    {
        for (int i = 0; i < MapArray.GetLength(0); i++)
            for (int j = 0; j < MapArray.GetLength(1); j++)
                if (j % MapArray.GetLength(1) == MapArray.GetLength(1) - 1)
                    Console.WriteLine(MapArray[i, j]);
                else
                    Console.Write(MapArray[i, j]);
    }
    public Vector2<int> DrawCross(int centerX, int centerY, char symbolx = 'E',char symboly='E')
    {
        for (int i = 0; i < MapArray.GetLength(0); i++)
            MapArray[i, centerY] = symbolx;
        for (int j = 0; j < MapArray.GetLength(1); j++)
            MapArray[centerX, j] = symboly;
        return new Vector2<int>(centerX, centerY);
    }
    public void DrawSquare(Vector2<int> startpos, Vector2<int> endpos, char symbol = 'E')
    {
        for (int i = startpos.x; i < endpos.x; i++)
            for (int j = startpos.y; j < endpos.y; j++)
                MapArray[i, j] = symbol;
    }
    public void DrawLine(Vector2<int> startpos, Vector2<int> endpos, char symbol = 'E')
    {
        int length = (int)Math.Sqrt(Math.Pow((endpos.x - startpos.x), 2) + Math.Pow((endpos.y - startpos.y), 2));
        int x = 0;
        int y = 0;
        if (endpos.x > 0)
            x = 1;
        if (endpos.y > 0)
            y = 1;
        for (int i = 0; i < length; i++)
        {
            MapArray[startpos.x + i * x, startpos.y + i * y] = symbol;
        }
    }
    public void DrawPoint(Vector2<int> pos, char symbol = 'E')
    {
        MapArray[pos.x, pos.y] = symbol;
    }
    public void UpdateValues(char toupdate, char updatevalue)
    {
        for (int i = 0; i < MapArray.GetLength(0); i++)
            for (int j = 0; j < MapArray.GetLength(1); j++)
            {
                if (MapArray[i, j] == toupdate)
                    MapArray[i, j] = updatevalue;
            }
    }
    public void UpdateValue(Vector2<int> pos, char updatevalue)
    {
        MapArray[pos.x, pos.y] = updatevalue;

    }
    public void DrawLineV(Vector2<int> startpos, int k = 0, int b = 0, int length = 5, char symbol = 'E')
    {
        for (int i = 0; i < length; i++)
        {
            MapArray[i + startpos.x, k * i + b + startpos.y] = symbol;
        }
    }
    public void DrawLineH(Vector2<int> startpos, int k = 0, int b = 0, int length = 5, char symbol = 'E')
    {
        for (int i = 0; i < length; i++)
        {
            MapArray[k * i + b + startpos.y, i + startpos.x] = symbol;
        }
    }
    public char GetNeighbourhood(int x, int y, direction dir)
    {
        switch (dir)
        {
            case direction.N:
                if (x - 1 >= 0)
                    return MapArray[x - 1, y];
                else
                    return '0';

            case direction.S:
                if (x + 1 <= MapArray.GetUpperBound(0))
                    return MapArray[x + 1, y];
                else
                    return '0';
            case direction.W:
                if (y - 1 >= 0)
                    return MapArray[x, y - 1];
                else
                    return '0';
            case direction.E:
                if (y + 1 <= MapArray.GetUpperBound(1))
                    return MapArray[x, y + 1];
                else
                    return '0';
        }
        return '0';
    }
    public int GetEqualNeibCount(Vector2<int> pos)
    {
        char selected = MapArray[pos.x, pos.y];
        int c = 0;
        foreach (direction s in Enum.GetValues(typeof(direction)))
        {
            if (selected == GetNeighbourhood(pos.x, pos.y, s))
                c++;

        }
        return c;

    }
}


