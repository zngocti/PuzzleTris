using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField]
    [Min(6)]
    private int _width = 10;
    [SerializeField]
    [Min(10)]
    private int _height = 20;
//    [SerializeField]
//    [Min(0)]
//    private int _extraHeight = 4;

    [SerializeField]
    private Tilemap _tilemap;
    public Tile miTile;
    public Vector2Int miPos;

    [SerializeField]
    Vector3Int[] _previewPositions;

    [SerializeField]
    Vector3Int _startPosition;

    [SerializeField]
    Vector3Int _heldPiecePosition;

    [SerializeField]
    Vector3Int _temporalPosition;

    public int Width { get => _width; }
    public int Height { get => _height; }

    public Vector3Int[] PreviewPositions { get => _previewPositions; }
    public Vector3Int StartPosition { get => _startPosition; }
    public Vector3Int HeldPiecePosition { get => _heldPiecePosition; }

    public Vector3Int TemporalPosition { get => _temporalPosition; }

    private void Awake()
    {
        /*
        Vector3 pos = new Vector3(_width / 2, _height / 2, 0);
        transform.position += pos;
        pos.z = -10;
        Camera.main.transform.position = pos;
        */
        //_height += _extraHeight;
        
        //columns rows
        /*
        for (int i = 0; i < _width; i++)
        {
            for (int c = 0; c < _height; c++)
            {
                backgroundTiles[i,c] = Instantiate(_tilePrefab);
            }
        }*/
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            _tilemap.SetTile((Vector3Int)miPos, miTile);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!_tilemap.HasTile((Vector3Int)miPos))
            {
                Debug.Log("No hay tile para leer");
                return;
            }

            Debug.Log("listo");
        }
    }

    /*
    public bool IsOccupied(Vector3Int pos)
    {
        return _tilemap.HasTile(pos);
    }*/

    public bool MoveTilesFromTo(Vector3Int[] fromPos, Vector3Int[] toPos)
    {
        if (fromPos.Length != toPos.Length)
        {
            return false;
        }

        Tile[] tilesToMove = new Tile[fromPos.Length];

        for (int i = 0; i < fromPos.Length; i++)
        {
            if (!_tilemap.HasTile(fromPos[i]))
            {
                return false;
            }

            tilesToMove[i] = _tilemap.GetTile<Tile>(fromPos[i]);
            _tilemap.SetTile(fromPos[i], null);
        }

        for (int i = 0; i < toPos.Length; i++)
        {
            _tilemap.SetTile(toPos[i], tilesToMove[i]);
        }

        return true;
    }

    public void SetTile(Vector3Int pos, Tile tile)
    {
        _tilemap.SetTile(pos, tile);
    }

    public void SetTileIfEmpty(Vector3Int pos, Tile tile)
    {
        if (!_tilemap.HasTile(pos))
        {
            _tilemap.SetTile(pos, tile);
        }
    }

    public void RemoveTileIfInPosition(Vector3Int pos, Tile tile)
    {
        if (_tilemap.GetTile(pos) == tile)
        {
            _tilemap.SetTile(pos, null);
        }
    }
}
