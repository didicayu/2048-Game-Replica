using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int value;
    public TMP_Text textValue;
    public Color[] squareColors = new Color[11];
    public SpriteRenderer spriteRenderer;

    private void Start() {

        if(Random.Range(0f,1f) < 0.1f){
            value = 4;
        }
        
        else{
            value = 2;
        }

        updateTile();
    }

    public void updateTile(){
        textValue.text = value.ToString();

        if(value >= 2 && value <= 2048 && isPowerOfTwo(value)){
            spriteRenderer.color = squareColors[(int)Mathf.Log(value,2)-1];

            if(value <= 4){
                textValue.color = new Color32(119, 110, 101, 255); //Color Text Marro-Negre
            }else{
                textValue.color = new Color32(249,246,242,255); //Color Text Blanc pero un xic fosc
            }

        }else{
            spriteRenderer.color = Color.black;
            textValue.color = Color.white;
        }
    }

    static bool isPowerOfTwo(int n)
    {
        if (n == 0)
            return false;
 
        return (int)(Mathf.Ceil((Mathf.Log(n) / Mathf.Log(2)))) == (int)(Mathf.Floor(((Mathf.Log(n) / Mathf.Log(2)))));
    }
}
