using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Piece : MonoBehaviour
{
    protected bool _isPivot = false;
    protected bool _isCurrentPiece = false;
    protected bool _isInUse = false;

    [SerializeField]
    protected Tile _tile;

    [SerializeField]
    protected ArrayLayout _form = new ArrayLayout();

    [SerializeField]
    protected ArrayLayout _pivotLocation = new ArrayLayout();

    public bool IsPivot { get => _isPivot; }
    public bool IsCurrentPiece { get => _isCurrentPiece; }
    public bool IsInUse { get => _isInUse; }

    public Tile Tile { get => _tile; }
    public ArrayLayout Form { get => _form; }

    public ArrayLayout PivotLocation { get => _pivotLocation; }

    protected int _currentPosition = 0;

    public int CurrentPosition { get => _currentPosition; }

    private int _pieceId = 0;
    public int GetPieceId { get => _pieceId; }

    private void Awake()
    {
        _pieceId = Utilities.GetID();
    }

    public void SetPiece(Tile newTile, ArrayLayout newForm, ArrayLayout newPivotLocation, bool isPivot = false)
    {
        _tile = newTile;
        _isPivot = isPivot;

        for (int i = 0; i < _form.rows.Length; i++)
        {
            for (int c = 0; c < _form.rows[i].row.Length; c++)
            {
                _form.rows[i].row[c] = newForm.rows[i].row[c];
                _pivotLocation.rows[i].row[c] = newPivotLocation.rows[i].row[c];
            }
        }
    }

    public void SetPiece(Piece myPieceData)
    {
        SetPiece(myPieceData._tile, myPieceData._form, myPieceData.PivotLocation, myPieceData.IsPivot);
    }

    public void SetPiece(Piece myPieceData, bool isPivot)
    {
        SetPiece(myPieceData._tile, myPieceData._form, myPieceData.PivotLocation, isPivot);
    }

    public Vector3Int[] GetOriginCoordinates()
    {
        int amount = 0;
        int firstNumPivot = 0;
        int secondNumPivot = 0;

        for (int i = 0; i < _form.rows.Length; i++)
        {
            for (int c = 0; c < _form.rows[i].row.Length; c++)
            {
                if (_form.rows[i].row[c])
                {
                    amount++;
                }

                if (_pivotLocation.rows[i].row[c])
                {
                    firstNumPivot = i;
                    secondNumPivot = c;
                }
            }
        }

        Vector3Int[] temporal = new Vector3Int[amount];
        amount = 0;

        //i = 0, c = 0 es la esquina superior izquierda
        //i es Y y al bajar aumenta, siendo i = 4, c = 0 la esquina inferior izquierda
        //c es X, funciona como siempre
        for (int i = 0; i < _form.rows.Length; i++)
        {
            for (int c = 0; c < _form.rows[i].row.Length; c++)
            {
                if (_form.rows[i].row[c])
                {
                    //como c funciona igual quee X lo coloco directamnte
                    //pero la i al ser Y en direccion opuesta tengo que invertir la direccion de ambos num primero usando el length y restando
                    //luego puedo hacer el calculo de pivote de forma normal
                    //hacerlo antes no funcionaria igual porque al ir in direccion contraria se tendria que invertir la resta
                    temporal[amount].x = c - secondNumPivot;
                    temporal[amount].y = (_form.rows.Length - i) - (_form.rows.Length - firstNumPivot);
                    amount++;
                }
            }
        }

        return temporal;
    }

    public void SetAsPivot(bool isPivot = true)
    {
        _isPivot = isPivot;
    }

    public void SetInUse()
    {
        _isInUse = true;
    }

    public void SetAsCurrentPiece(bool isCurrent = true)
    {
        _isCurrentPiece = isCurrent;
    }
}
