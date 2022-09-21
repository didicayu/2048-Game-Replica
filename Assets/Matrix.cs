using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix : MonoBehaviour
{
    private static GameObject[,] debugMatrix;

    [Header("Debugging settings: ")]
    public bool debugMode = true;
    public int debugWidth = 4, DebugHeight = 4;

    private void Start() {
        if(debugMode){
            debugMatrix = new GameObject[debugWidth,DebugHeight];
            generateDebugMatrix(debugMatrix);
            logDebugMatrixContents(debugMatrix);
        }
    }

    private void Update() {

        if(debugMode){
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                //logDebugMatrix(Matrix.rotateBy90(debugMatrix));
                logDebugMatrixContents(Matrix.rotateBy180(debugMatrix));
            }

            if(Input.GetKeyDown(KeyCode.DownArrow)){
                //logDebugMatrix(Matrix.rotateBy180(debugMatrix));
                logDebugMatrixContents(Matrix.rotateByMinus90(debugMatrix));
            }
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                logDebugMatrixContents(Matrix.rotateBy90(debugMatrix));
            }
            if(Input.GetKeyDown(KeyCode.F)){
                //logDebugMatrix(transpose(debugMatrix));
                logDebugMatrixContents(Matrix.transpose(debugMatrix));
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                //logDebugMatrix(Matrix.rotateByMinus90(debugMatrix));
                logDebugMatrixContents((debugMatrix));
            }
        }

    }
    public static GameObject[,] transpose(GameObject[,] matrix)
    {
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        GameObject[,] result = new GameObject[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[j, i] = matrix[i, j];
            }
        }

        return result;
    }

    public static GameObject[,] flipRows(GameObject[,] matrix){
        
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        GameObject[,] result = new GameObject[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[w-1-i,j] = matrix[i,j];
            }
        }

        return result;
    }

    public static GameObject[,] flipColumns(GameObject[,] matrix){
        
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        GameObject[,] result = new GameObject[h, w];

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[i,h-1-j] = matrix[i,j];
            }
        }

        return result;
    }

    public static GameObject[,] rotateBy90(GameObject[,] matrix){
        
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        GameObject[,] result = new GameObject[h, w];

        result = transpose(matrix);
        result = flipColumns(result);

        return result;
    }

    public static GameObject[,] rotateBy180(GameObject[,] matrix){
        
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        GameObject[,] result = new GameObject[h, w];

        result = rotateBy90(matrix);
        result = transpose(result);

        return result;
    }

    public static GameObject[,] rotateByMinus90(GameObject[,] matrix){
        
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        GameObject[,] result = new GameObject[h, w];

        result = transpose(matrix);
        result = flipRows(result);

        return result;
    }

    public static void logDebugMatrix (GameObject[,] matrix){

        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        string logOutput = "";

        for (int j = h-1; j >= 0; j--)
        {
            for (int i = 0; i < w; i++)
            {
                if(matrix[i,j] != null){
                    var tile = matrix[i,j].GetComponent<Tile>();
                    logOutput += $"<color=green>[{tile.value}]</color> ";
                    //Debug.Log($"Matrix ({i},{j}): <color=green>[{matrix[i,j].name}]</color>");
                }
                else{
                    //Debug.Log($"Matrix ({i},{j}): <color=red>[null]</color>");
                    logOutput += $"<color=red>[0]</color> ";
                }
            }

            logOutput += "\n";
        }

        Debug.Log(logOutput);

    }
    public static void logDebugMatrixContents (GameObject[,] matrix){

        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        string logOutput = "";

        for (int j = h-1; j >= 0; j--)
        {
            for (int i = 0; i < w; i++)
            {
                if(matrix[i,j] != null){
                    logOutput += $"<color=green>[{matrix[i,j].name}]</color> ";
                    //Debug.Log($"Matrix ({i},{j}): <color=green>[{matrix[i,j].name}]</color>");
                }
                else{
                    //Debug.Log($"Matrix ({i},{j}): <color=red>[null]</color>");
                    logOutput += $"<color=red>[NULL]</color> ";
                }
            }

            logOutput += "\n";
        }

        Debug.Log(logOutput);

    }

    public static void generateDebugMatrix (GameObject[,] matrix){
        
        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                matrix[i,j] = new GameObject(((j + i * w)+1).ToString());
                matrix[i,j].transform.position = new Vector2(i,j);
            }
        }
    }

    public static void debugPriorityQueueMatrix(GameObject[,] matrix){

        int w = matrix.GetLength(0);
        int h = matrix.GetLength(1);

        string logOutput = "Priority Queue: ";

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if(matrix[i,j] != null){
                    logOutput += $"<color=cyan>{(Vector2)matrix[i,j].transform.position} = {((j + i * w)+1).ToString()}</color> <color=red>|</color> ";    
                }
            }
        }

        Debug.Log(logOutput);
    }
}
