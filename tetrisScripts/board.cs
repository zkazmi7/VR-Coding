using Meta.WitAi.TTS.Samples;
using Meta.WitAi.TTS.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public piece activePiece { get;  set; }
    public Vector3Int spawnPosition;
    public TetrominoData[] tetrominoes;
    public int rotationIndex { get; private set; }
    private piece test;
    private int time;
    public float lockDelay = 4f;

    private int lockTime;
    public static int pauseValue = 0;
    public int gameTime = 0;

    [SerializeField] public TTSSpeaker speakk;

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<piece>();

        this.rotationIndex = 0;

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
        lockTime = 0;
    }

    private void Update()
    {
        
        
        
        if (!CanPause())
        {
             
            time++;
            lockTime++;
        }
        
        
        if (time >= 200)
        {
            Clear(activePiece);

            Vector3Int newPosition = activePiece.getPosition();
            newPosition.x += Vector2Int.down.x;
            newPosition.y += Vector2Int.down.y;

            bool valid = IsValidPosition(activePiece, newPosition);

            if (valid)
            {
                activePiece.setPosition(newPosition);
                lockTime = 0;
            }
            Set(activePiece);

            time = 0;
            
        }
        if (lockTime > 250)
        {
            Lock();
        }
        
    }



    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            
            for (int i = -5; i < 5; i++)
            {
                for (int j = -9; j < 12;  j++)
                {
                    tilemap.SetTile(new Vector3Int(i, j), null);
                }
            }
            speakk.Speak("Game over");
            pauseValue = 1;
            
        }
    }


    public void MoveLeft()
    {
        if(!CanPause())
        {
            Clear(activePiece);

            Vector3Int newPosition = activePiece.getPosition();
            newPosition.x += Vector2Int.left.x;
            newPosition.y += Vector2Int.left.y;

            bool valid = IsValidPosition(activePiece, newPosition);

            if (valid)
            {
                activePiece.setPosition(newPosition);
                lockTime = 0;
            }
            Set(activePiece);
        }
        
    }

    public void Pause()
    {
        pauseValue++; 
    }

    private bool CanPause()
    {
        if (pauseValue % 2 != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MoveRight()
    {
        if (!CanPause())
        {
            Clear(activePiece);

            Vector3Int newPosition = activePiece.getPosition();
            newPosition.x += Vector2Int.right.x;
            newPosition.y += Vector2Int.right.y;

            bool valid = IsValidPosition(activePiece, newPosition);

            if (valid)
            {
                activePiece.setPosition(newPosition);
                lockTime = 0;
            }
            Set(activePiece);
        }
    }

    public void MoveDown()
    {
        if (!CanPause())
        {

            int i = 0;

            while (i < 21)
            {
                i++;

                Clear(activePiece);

                Vector3Int newPosition = activePiece.getPosition();
                newPosition.x += Vector2Int.down.x;
                newPosition.y += Vector2Int.down.y;

                bool valid = IsValidPosition(activePiece, newPosition);

                if (valid)
                {
                    activePiece.setPosition(newPosition);

                }
                Set(activePiece);
            }

            Lock();
        }

        
    }


    private bool Move(Vector2Int translation)
    {
        
            Clear(activePiece);

            Vector3Int newPosition = activePiece.getPosition();
            newPosition.x += translation.x;
            newPosition.y += translation.y;

            bool valid = IsValidPosition(activePiece, newPosition);
            if (valid)
            {
                activePiece.setPosition(newPosition);
                lockTime = 0;
            }
            return valid;

        
    }
    private int direction = 1;
    public void Rotate()
    {
        if (!CanPause())
        {
            // Store the current rotation in case the rotation fails
            // and we need to revert
            int originalRotation = rotationIndex;

            // Rotate all of the cells using a rotation matrix
            rotationIndex = Wrap(rotationIndex + direction, 0, 4);
            ApplyRotationMatrix(direction);
            Set();
            lockTime = 0;

            // Revert the rotation if the wall kick tests fail
            /*if (!TestWallKicks(rotationIndex, direction))
            {
                rotationIndex = originalRotation;
                ApplyRotationMatrix(-direction);
                Set();
            }
            else
            {
                activePiece = test;
                Set();
            }*/
        }
    
    }

    private void Lock()
    {
        if(!CanPause())
        {
            Set();
            ClearLines();
            SpawnPiece();
        }
    }

    
    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = data.RotationMatrix;
        test = activePiece;
        

        // Rotate all of the cells using the rotation matrix
        for (int i = 0; i < activePiece.cells.Length; i++)
        {
            Vector3 cell = activePiece.cells[i];

            int x, y;

            switch (activePiece.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    // "I" and "O" are rotated from an offset center point
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            activePiece.cells[i] = new Vector3Int(x, y, 0);
            
        }

        
        for (int i = 0; i <  activePiece.cells.Length; i++)
        {
            if (activePiece.cells[i] != null || tilemap.HasTile(
                activePiece.cells[i]))
                {
                activePiece = test;
                Set(activePiece);
                break;
                
                }
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < activePiece.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = activePiece.data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, activePiece.data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }

    public void Set(piece piece)
    {
        for (int i = 0;i < piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Set()
    {
        if (IsValidPosition(activePiece, activePiece.position))
        {
            for (int i = 0; i < activePiece.cells.Length; i++)
            {
                Vector3Int tilePosition = activePiece.cells[i] + activePiece.position;
                this.tilemap.SetTile(tilePosition, activePiece.data.tile);
            }
        }
    }

    public void Clear(piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < activePiece.cells.Length; i++)
        {
            Vector3Int tilePosition = activePiece.cells[i] + activePiece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(piece piece, Vector3Int position)
    {
        
        
        for (int i = 0; i <  piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;
            
            if(this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        
        int row = -9;

        // Clear from bottom to top
        while (row < 11)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        

        for (int col = -5; col < 5; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        

        // Clear all tiles in the row
        for (int col = -5; col < 5; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < 11)
        {
            for (int col = -5; col < 5; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
    }
    }
