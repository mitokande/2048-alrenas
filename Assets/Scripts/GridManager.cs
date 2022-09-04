using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{

    public List<TileRow> tiles = new List<TileRow>();
    public List<GameObject> tilePos = new List<GameObject>();
    public GameObject Tile;
    public Sprite[] sprites = new Sprite[14];
    private bool shifting;
    public GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        InitializeRandomTile();
    }

    // Update is called once per frame
    void Update()
    {
        //restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        
        if (!shifting)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SwipeRight();
                Invoke("InitializeRandomTile", 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                SwipeLeft();
                Invoke("InitializeRandomTile", 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SwipeDown();
                Invoke("InitializeRandomTile", 0.2f);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                SwipeUp();
                Invoke("InitializeRandomTile", 0.2f);
            }
            if (isGridFull())
            {
                manager.GameOver();
            }
        }
    }
    public int getFurthestHorizontalTile(TileRow row,string direction)
    {
        int pos = -1;
        if(direction == "right")
        {
            for (int i = 0; i < row.tiles.Count; i++)
            {
                if (row.tiles[i] == null)
                {
                    pos = i;
                }
            }
        }else if (direction == "left")
        {
            for (int i = row.tiles.Count-1;i>=0; i--)
            {
                if (row.tiles[i] == null)
                {
                    pos = i;
                }
            }
        }
        return pos;
    }
    public int getFurthestVerticalTile(int currentTile, string direction)
    {
        int pos = -1;
        if(direction == "down")
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].tiles[currentTile] == null)
                {
                    pos = i;
                }
            }
        }else if (direction == "up")
        {
            for (int i = tiles.Count-1; i>=0; i--)
            {
                if (tiles[i].tiles[currentTile] == null)
                {
                    pos = i;
                }
            }
        }
        return pos;
    }
    public void SwipeDown()
    {
        for(int i = tiles.Count-1;i>=0; i--)
        {
            for(int j = 0; j < tiles[i].tiles.Count; j++)
            {
                if(tiles[i].tiles[j] != null)
                {
                    int emptyTilePos = getFurthestVerticalTile(j, "down");
                    if (emptyTilePos <= i || emptyTilePos == -1)
                    {
                        Merge(i, j, "down");
                        continue;
                    }
                    int emptyTileGlobalPos = (emptyTilePos * 4) + j;
                    tiles[emptyTilePos].tiles[j] = tiles[i].tiles[j];
                    StartCoroutine(move2(emptyTilePos, j, tilePos[emptyTileGlobalPos].transform, "down"));
                    tiles[i].tiles[j] = null;
                }
            }
        }
    }
    public void SwipeUp()
    {
        for (int i = 0; i< tiles.Count; i++)
        {
            for (int j = 0; j < tiles[i].tiles.Count; j++)
            {
                if (tiles[i].tiles[j] != null)
                {
                    int emptyTilePos = getFurthestVerticalTile(j, "up");
                    if (emptyTilePos > i || emptyTilePos == -1)
                    {
                        Merge(i, j, "up");
                        continue;
                    }
                    int emptyTileGlobalPos = (emptyTilePos * 4) + j;
                    tiles[emptyTilePos].tiles[j] = tiles[i].tiles[j];
                    StartCoroutine(move2(emptyTilePos, j, tilePos[emptyTileGlobalPos].transform, "up"));
                    tiles[i].tiles[j] = null;
                }
            }
        }
    }
    public void SwipeRight()
    {
        for(int i = 0; i < tiles.Count; i++)
        {
            for(int j = tiles[i].tiles.Count - 1; j >= 0; j--)
            {
                if(tiles[i].tiles[j] != null)
                {
                    int emptyTilePos = getFurthestHorizontalTile(tiles[i],"right");
                    if (emptyTilePos <= j || emptyTilePos == -1)
                    {
                        Merge(i, j, "right");
                        continue;
                    }
                    int emptyTileGlobalPos = (i * 4) + emptyTilePos;
                    tiles[i].tiles[emptyTilePos] = tiles[i].tiles[j];
                    StartCoroutine(move2(i, emptyTilePos, tilePos[emptyTileGlobalPos].transform,"right"));
                    tiles[i].tiles[j] = null;
                }
            }
        }
        
    }
    public void SwipeLeft()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            for (int j =0;j< tiles[i].tiles.Count; j++)
            {
                if (tiles[i].tiles[j] != null)
                {
                    int emptyTilePos = getFurthestHorizontalTile(tiles[i], "left");
                    if (emptyTilePos > j || emptyTilePos == -1)
                    {
                        Merge(i, j, "left");
                        continue;
                    }
                    int emptyTileGlobalPos = (i * 4) + emptyTilePos;
                    tiles[i].tiles[emptyTilePos] = tiles[i].tiles[j];                    
                    StartCoroutine(move2(i,emptyTilePos, tilePos[emptyTileGlobalPos].transform,"left"));
                    tiles[i].tiles[j] = null;
                }
            }
        }

    }
    IEnumerator move2(int i,int j, Transform b,string direction)
    {
        Tile a = tiles[i].tiles[j];
        while (a.transform.position != b.position)
        {
            shifting = true;
            a.gameObject.transform.position = Vector2.MoveTowards(a.gameObject.transform.position, b.position, 14 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        
        Merge(i,j,direction);
    }
    public void Merge(int i,int j,string direction)
    {
        switch (direction)
        {
            case "left":
                if (j - 1 >= 0 && tiles[i].tiles[j - 1] != null && tiles[i].tiles[j - 1].getValue() == tiles[i].tiles[j].getValue())
                {
                    manager.AddScore(tiles[i].tiles[j].getValue());
                    tiles[i].tiles[j - 1].setValue(tiles[i].tiles[j - 1].getValue() * 2);
                    tiles[i].tiles[j - 1].SetTileSprite(sprites[(int)Mathf.Log(tiles[i].tiles[j - 1].getValue(), 2) - 1]);
                    Destroy(tiles[i].tiles[j].gameObject, 0.08f);
                    tiles[i].tiles[j] = null;
                }
                break;
            case "right":
                if (j + 1 <= 3 && tiles[i].tiles[j + 1] != null && tiles[i].tiles[j + 1].getValue() == tiles[i].tiles[j].getValue())
                {
                    manager.AddScore(tiles[i].tiles[j].getValue());
                    tiles[i].tiles[j + 1].setValue(tiles[i].tiles[j + 1].getValue() * 2);
                    tiles[i].tiles[j + 1].SetTileSprite(sprites[(int)Mathf.Log(tiles[i].tiles[j + 1].getValue(), 2) - 1]);
                    Destroy(tiles[i].tiles[j].gameObject, 0.05f);
                    tiles[i].tiles[j] = null;
                }
                break;
            case "up":
                if (i - 1 >= 0 && tiles[i-1].tiles[j] != null && tiles[i-1].tiles[j].getValue() == tiles[i].tiles[j].getValue())
                {
                    manager.AddScore(tiles[i].tiles[j].getValue());
                    tiles[i-1].tiles[j].setValue(tiles[i-1].tiles[j].getValue() * 2);
                    tiles[i-1].tiles[j].SetTileSprite(sprites[(int)Mathf.Log(tiles[i-1].tiles[j].getValue(), 2) - 1]);
                    Destroy(tiles[i].tiles[j].gameObject, 0.05f);
                    tiles[i].tiles[j] = null;
                }
                break;
            case "down":
                if (i + 1 <= 3 && tiles[i + 1].tiles[j] != null && tiles[i + 1].tiles[j].getValue() == tiles[i].tiles[j].getValue())
                {
                    manager.AddScore(tiles[i].tiles[j].getValue());
                    tiles[i + 1].tiles[j].setValue(tiles[i + 1].tiles[j].getValue() * 2);
                    tiles[i + 1].tiles[j].SetTileSprite(sprites[(int)Mathf.Log(tiles[i + 1].tiles[j].getValue(), 2) - 1]);
                    Destroy(tiles[i].tiles[j].gameObject, 0.05f);
                    tiles[i].tiles[j] = null;
                }
                break;
        }
        shifting = false;

    }
    public void InitializeRandomTile()
    {
        int randomX = Random.Range(1, 5);
        int randomY = Random.Range(1, 5);
        InitializeTile(randomX, randomY);
    }
    public void InitializeTile(int x,int y)
    {
        int position = ((y - 1) * 4) + x;
        if (tiles[y - 1].tiles[x -1] == null)
        {
            Tile newTile = Instantiate(Tile, tilePos[position-1].transform.position, Quaternion.identity, gameObject.transform).GetComponent<Tile>();
            tiles[y - 1].tiles[x - 1] = newTile;
            newTile.setCurrentPosition(position);
            newTile.setValue(2);
            newTile.SetTileSprite(sprites[0]);
        }
    }
    public bool isGridFull()
    {
        for(int i = 0; i < tiles.Count; i++)
        {
            for(int j = 0; j< tiles[i].tiles.Count; j++)
            {
                if(tiles[i].tiles[j] == null)
                {
                    return false;
                }
            }
        }

        return true;
    }
}

[System.Serializable]
public class TileRow{
    public List<Tile> tiles = new List<Tile>();
}