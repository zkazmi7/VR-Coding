using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class piece : MonoBehaviour
{
    public board board { get;  set; }
    public TetrominoData data { get;  set; }
    public Vector3Int position { get;  set; }
    public Vector3Int[] cells { get;  set; }

   


    public void Initialize(board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }
        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int) data.cells[i];
        }

    }

    private void Update()
    {
        
    }

    public Vector3Int getPosition()
    {
        return position;
    }

    public void setPosition(Vector3Int position)
    {
        this.position = position;
    }

    public void  MoveLeft()
    {
        this.board.Clear(this);

        Vector3Int newPosition = this.position;
        newPosition.x += Vector2Int.left.x;
        newPosition.y += Vector2Int.left.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
        }

        this.board.Set(this);

        //return valid;
    }
}
