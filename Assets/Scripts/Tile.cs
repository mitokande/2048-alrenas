using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    [SerializeField]
    private int value;
    [SerializeField]
    private int currentPosition;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setCurrentPosition(int currentPos)
    {
        this.currentPosition = currentPos;
    }
    public int getCurrentPosition()
    {
        return this.currentPosition;
    }
    public void setValue(int value)
    {
        this.value = value;
    }
    public int getValue()
    {
        return this.value;
    }
    public void SetTileSprite(Sprite sprite)
    {
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public Sprite GetTileSprite()
    {
        return this.GetComponent<SpriteRenderer>().sprite;
    }
}
