using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _tile;
    [SerializeField] private GridManager _gridManager;
    private GameObject[,] _2DusedTiles;
    private GameObject[,] tilesPriorityQueue;

    private readonly Vector2[] directions = {new Vector2(1,0), new Vector2(-1,0), new Vector2(0,1), new Vector2(0,-1)}; // dreta, esquerra, adalt, abaix

    int height;
    int width;

    private void Awake() {

        width = _gridManager._width;
        height = _gridManager._height;

        _2DusedTiles = new GameObject[width,height];
        _gridManager.generateGrid();
        spawnStartingTiles();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        //Debug.Log(movementDirection);
        makeAMove(movementDirection); //Sends movement vector to check if we can make the movement

        if(Input.GetKeyDown(KeyCode.F3)){
            Matrix.logDebugMatrix(_2DusedTiles);
        }
        if(Input.GetKeyDown(KeyCode.F4)){
            Matrix.logDebugMatrix(tilesPriorityQueue);
        }

        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)){
            lastCaseNum = 0;
        }
    }

    void makeAMove(Vector2 dir){
        setPriorityQueue(dir); // Sets tiles priority queue according to the movement direction
    }

    int lastCaseNum = 0;
    bool canSpawnNewTile = false;
    void setPriorityQueue(Vector2 dir){

        int caseNum = arrayContainsV2(directions,dir);
        

        switch (caseNum)
        {
            case 1:
                //Debug.Log("dreta");
                //tilesPriorityQueue = Matrix.rotateBy90(_2DusedTiles);
                tilesPriorityQueue = Matrix.rotateBy180(_2DusedTiles);
                break;

            case 2:
                //Debug.Log("esquerra");
                //tilesPriorityQueue = Matrix.rotateByMinus90(_2DusedTiles);
                tilesPriorityQueue = _2DusedTiles;
                break;

            case 3:
                //Debug.Log("adalt");
                //tilesPriorityQueue = _2DusedTiles;
                tilesPriorityQueue = Matrix.rotateBy90(_2DusedTiles);
                break;

            case 4:
                //Debug.Log("abaix");
                //tilesPriorityQueue = Matrix.rotateBy180(_2DusedTiles);
                tilesPriorityQueue = Matrix.rotateByMinus90(_2DusedTiles);
                break;

            default:
                break;
        }

        if(caseNum != 0 && caseNum != lastCaseNum){

            for (int i = 0; i < width; i++) // for loop to ensure all tiles are moved to their max
            {
                moveTiles(dir); //tries to move each tile towards the direction vector if possible
            }

            if(canSpawnNewTile){
                spawnTile();
                canSpawnNewTile = false;
            }
            
            lastCaseNum = caseNum;
        }
    }

    void moveTiles(Vector2 dir){

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject selectedTile = tilesPriorityQueue[i,j];

                if(selectedTile != null){ //Checks if the tile exists on the given position
                    moveTile(selectedTile, dir); //Tries to move said Tile
                }   
            }
        }
    }

    void moveTile(GameObject tile, Vector2 dir){
        //Debug.Log($"Mogut la tile {tile.name} cap a {dir}");
        Vector2 oldPos = (Vector2)tile.transform.position;
        Vector2 newPos = oldPos + dir;

        (bool onGrid, bool Occupied) = validPosition(oldPos, newPos);

        if(onGrid){ //Checks if the position we want to move in exists inside the grid
            if(!Occupied){ // checks if it is not occupied
                updateGameState(tile, oldPos, newPos); //moves the tile to the new position
            }
            else{ // if it is occupied by another tile we want to check wether we can merge said tiles. 
                mergeTiles(tile, oldPos,newPos);
            }
        }
    }

    void mergeTiles(GameObject tile, Vector2 oldPos, Vector2 newPos){

        int x = (int)newPos.x;
        int y = (int)newPos.y;

        Tile oldTile = tile.GetComponent<Tile>();
        Tile newTile = _2DusedTiles[x, y].GetComponent<Tile>();

        if(oldTile.value == newTile.value){ // will only merge tiles with same value

            newTile.value += oldTile.value;
            newTile.updateTile();

            Destroy(oldTile.gameObject); //Destroys the old tile and then we set the value of the remaining tile to it's new value

            updateGameState(newTile.gameObject, oldPos, newPos);
        }
    }

    (bool isOnGrid, bool isOccupied) validPosition(Vector2 ogPos, Vector2 newPos){

        int x = (int)newPos.x;
        int y = (int)newPos.y;


        if((x < width && x >= 0) && (y < height && y >= 0)){
            //Valid grid position checked
            if(_2DusedTiles[x,y] == null){
                //There are no tiles on the new position
                return (true,false);
            }else{
                return (true,true);
            }
        }

        return (false,false);
    }

    void spawnStartingTiles(){

        var firstTile = spawnTile();

        var secondTile = spawnTile();
    }

    void updateGameState(GameObject tile){
        int x = (int)tile.transform.position.x;
        int y = (int)tile.transform.position.y;
        _2DusedTiles[x,y] = tile;
        tilesPriorityQueue = _2DusedTiles;

        canSpawnNewTile = true; //If we have updated the grid we know a new tile can be spawned
    }

    void updateGameState(GameObject tile, Vector2 oldPos, Vector3 newPos){
        int x = (int)newPos.x;
        int y = (int)newPos.y;

        int oldX = (int)oldPos.x;
        int oldY = (int)oldPos.y;

        _2DusedTiles[x,y] = tile;
        _2DusedTiles[oldX,oldY] = null;

        tilesPriorityQueue = _2DusedTiles;

        tile.transform.position = newPos;

        canSpawnNewTile = true; //If we have updated the grid we know a new tile can be spawned
    }

    GameObject spawnTile(){
        var spawnedTile = Instantiate(_tile, getRandomTilePos(),Quaternion.identity);
        updateGameState(spawnedTile);
        return spawnedTile;
    }

    private Vector2 getRandomTilePos(){

        int x = Random.Range(0, width);
        int y = Random.Range(0, height);

        Vector2 pos = new Vector2(x,y);

        if(_2DusedTiles[x,y] != null){
            return getRandomTilePos();
        }

        return new Vector2(x,y);
    }

    int arrayContainsV2(Vector2[] arr, Vector2 v2){
        for (int i = 0; i < arr.Length; i++)
        {
            if(arr[i] == v2){
                return i+1;
            }
        }

        return 0;
    }
    
}
