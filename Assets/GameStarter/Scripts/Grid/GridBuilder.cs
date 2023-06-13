
using UnityEngine;

public class GridBuilder<T> 
{
    private int _width;
    private int _height;
    private float _cellSize;
    private Vector3 _originPosition;
    public GenericGrid<T> Build()
    {
        return new GenericGrid<T>(_width, _height, _cellSize, _originPosition);
    }

    public GridBuilder<T> WithSize(int width, int height)
    {
        _width = width;
        _height = height;
        return this;
    }
    public GridBuilder<T> WithCellSize(float cellSize)
    {
        _cellSize = cellSize;
        return this;
    }
    public GridBuilder<T> WithOrigin(Vector3 origin)
    {
        _originPosition = origin;
        return this;
    }
}
