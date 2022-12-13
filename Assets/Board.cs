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
    [SerializeField]
    [Min(0)]
    private int _extraHeight = 4;

    [SerializeField]
    private Tilemap _tilemap;
    public Tile miTile;
    public Vector2Int miPos;

    private void Awake()
    {
        Vector3 pos = new Vector3(_width / 2, _height / 2, 0);
        transform.position += pos;
        pos.z = -10;
        Camera.main.transform.position = pos;
        
        _height += _extraHeight;
        
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

    public bool IsOccupied(Vector3Int pos)
    {
        return _tilemap.HasTile(pos);
    }
}
