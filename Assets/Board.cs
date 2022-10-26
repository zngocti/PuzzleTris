using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField]
    private int _width = 10;
    [SerializeField]
    private int _height = 20;

    public Tilemap _tilemap;
    public Tile miTile;
    public Vector2Int miPos;

    private void Awake()
    {
        Vector3 pos = new Vector3(_width / 2, _height / 2, 0);
        transform.position += pos;
        pos.z = -10;
        Camera.main.transform.position = pos;

        _height += 4;

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
            _tilemap.GetTile<MiTile>((Vector3Int)miPos).nombre();
            Debug.Log("listo");
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!_tilemap.HasTile((Vector3Int)miPos))
            {
                Debug.Log("No hay tile para mover");
                return;
            }
            _tilemap.GetTile<MiTile>((Vector3Int)miPos).gameObject.transform.Translate(Vector3.up);
        }
    }
}
