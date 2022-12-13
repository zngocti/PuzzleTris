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
    protected ArrayLayout _form;

    public bool IsPivot { get => _isPivot; }
    public bool IsCurrentPiece { get => _isCurrentPiece; }
    public bool IsInUse { get => _isInUse; }
    public ArrayLayout Form { get => _form;  }

    public void SetPiece(Tile newTile, ArrayLayout newForm, bool isPivot = false)
    {
        _tile = newTile;
        _isPivot = isPivot;

        for (int i = 0; i < _form.rows.Length; i++)
        {
            for (int c = 0; c < _form.rows[i].row.Length; c++)
            {
                _form.rows[i].row[c] = newForm.rows[i].row[c];
            }
        }
    }
}
