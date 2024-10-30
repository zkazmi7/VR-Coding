using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get ; set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void initialize()
    {
        this.cells = data.Cells[this.tetromino];
        this.wallKicks = data.WallKicks[this.tetromino];
    }
}

