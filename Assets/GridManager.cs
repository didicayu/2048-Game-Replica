using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int _width, _height;
    [SerializeField] private GameObject _tile;
    [SerializeField] private GameObject _tileBG;

    [SerializeField] private Transform _camera;

    //[HideInInspector] public GameObject[] _tileArray;

    public void generateGrid(){

        //_tileArray = new GameObject[_width*_height];

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GameObject spawnedTile = Instantiate(_tile, new Vector3(i,j), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j}";

                //_tileArray[i+j*_width] = spawnedTile;
            }
        }

        //Grid center values
        var xCenter = (float)_width / 2 -0.5f;
        var yCenter = (float)_height / 2 -0.5f;

        //Center camera onto the grid
        _camera.transform.position = new Vector3(xCenter, yCenter, -10);

        //put the background tile into the screen
        GameObject backgroundTile = Instantiate(_tileBG, new Vector3(xCenter, yCenter), Quaternion.identity);
        backgroundTile.name = "Background Tile";
        backgroundTile.GetComponent<SpriteRenderer>().size = new Vector2(_width + 0.5f, _height + 0.5f);
    }
}
