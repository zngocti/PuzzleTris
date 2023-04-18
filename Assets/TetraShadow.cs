using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraShadow : Shadow<TetraPiece>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetShadow(Board board, Vector3Int[] piece, Dictionary<Vector3Int, TetraPiece> piecesInBoard)
    {
        if (piecesInBoard == null)
        {
            return;
        }

        //remuevo la sombra anterior (si lo hago despues y las piezas tienen diferente length va a terminar creando un nuevo array
        RemoveShadow(board);

        if (_shadowPosition == null)
        {
            _shadowPosition = new Vector3Int[piece.Length];
        }
        else if (_shadowPosition.Length != piece.Length)
        {
            _shadowPosition = new Vector3Int[piece.Length];
        }


        //coloco la posicion de la sombra y bajo hasta chocarme con algo que no sea la pieza actual
        bool stop = false;

        TetraPiece tempOut;

        Vector3Int[] newPos = new Vector3Int[piece.Length];

        for (int a = 1; !stop; a++)
        {
            for (int i = 0; i < piece.Length; i++)
            {
                newPos[i].x = piece[i].x;
                newPos[i].y = piece[i].y - a;
            }

            for (int i = 0; i < piece.Length; i++)
            {
                if (newPos[i].y < 0)
                {
                    stop = true;
                    break;
                }

                if (piecesInBoard.TryGetValue(newPos[i], out tempOut))
                {
                    if (!tempOut.IsCurrentPiece)
                    {
                        stop = true;
                        break;
                    }
                }
            }

            if (stop)
            {
                if (a == 1)
                {
                    //si la sombra deberia estar donde esta la pieza no es necesario hacer nada
                    return;
                }
                for (int i = 0; i < piece.Length; i++)
                {
                    _shadowPosition[i].x = piece[i].x;
                    _shadowPosition[i].y = piece[i].y - a + 1;
                }
            }
        }

        for (int i = 0; i < _shadowPosition.Length; i++)
        {
            board.SetTileIfEmpty(_shadowPosition[i], _shadowTile);
        }
    }
}
