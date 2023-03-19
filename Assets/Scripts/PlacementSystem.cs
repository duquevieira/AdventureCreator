using JetBrains.Annotations;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

//www.youtube.com/watch?v=rKp9fWvmIww&t=342s

//Notas: Sala nao e escalavel; So o ultimo objeto e pode rodar; Os objetos tao a ser postos sempre na mesma ordem; O random da posicao dos objetos ta feito mal; 

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem current;
    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap mainTileMap;
    //[SerializeField] private TileBase usedTile;
    private PlaceableObject objectToPlace;
    public GameObject[] objects;
    private List<Vector3> tileWorldLocations;
    private List<GameObject> objectsInScene;

    private void Awake()
    {
        current = this;
        grid = gridLayout.GetComponent<Grid>();
        objectsInScene = new List<GameObject>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnOnAllTiles();
        }
        if (!objectToPlace)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            objectToPlace.Rotate();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CanBePlaced(objectToPlace))
            {
                objectToPlace.Place();
                Vector3Int start = gridLayout.WorldToCell(objectToPlace.getStartPosition());
                //TakeArea(start, objectToPlace.Size);
            }
            else
            {
                Destroy(objectToPlace.gameObject);
            }
        }
    }

    public static Vector3 getMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void SpawnObjectsRandomlyAldrabado(GameObject[] objects)
    {
        for (int j = 0; j < objectsInScene.Count; j++)
        {
            Destroy(objectsInScene[j]);
        }
        objectsInScene = new List<GameObject>();
        int i = 0;
        List<Vector3> usedTiles = new List<Vector3>();
        while (i < objects.Length)
        {
            Vector3 randomTile = new Vector3(Random.Range(-5, 4), 0, Random.Range(-5, 4));
            if (!usedTiles.Contains(randomTile))
            {
                Vector3 position = SnapCoordinateToGrid(randomTile);
                GameObject obj = Instantiate(objects[i], position, Quaternion.identity);
                obj.AddComponent<PlaceableObject>();
                objectToPlace = obj.GetComponent<PlaceableObject>();
                Debug.Log(objectToPlace);
                obj.AddComponent<ObjectDrag>();
                usedTiles.Add(randomTile);
                objectsInScene.Add(obj);
                i++;
            }
        }
    }

    public void SpawnOnAllTiles()
    {
        for (int j = 0; j < objectsInScene.Count; j++)
        {
            Destroy(objectsInScene[j]);
        }
        objectsInScene = new List<GameObject>();
        List<Vector3> usedTiles = new List<Vector3>();
        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                int objectIndex = Random.Range(0, objects.Length);
                Vector3 tile = new Vector3(i, 0, j);
                if (!usedTiles.Contains(tile))
                {
                    Vector3 position = SnapCoordinateToGrid(tile);
                    GameObject obj = Instantiate(objects[objectIndex], position, Quaternion.identity);
                    obj.AddComponent<PlaceableObject>();
                    objectToPlace = obj.GetComponent<PlaceableObject>();
                    Debug.Log(objectToPlace);
                    obj.AddComponent<ObjectDrag>();
                    usedTiles.Add(tile);
                    objectsInScene.Add(obj);
                }
            }
        }
    }

    public void SpawnObjectsRandomly(GameObject[] objects)
    {
        tileWorldLocations = new List<Vector3>();
        foreach (var pos in mainTileMap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = mainTileMap.CellToWorld(localPlace);
            if (mainTileMap.HasTile(localPlace))
            {
                tileWorldLocations.Add(place);
            }
        }
    }

    /* public void InitializeFloor(GameObject floor)
     {
         BoundsInt bounds = mainTileMap.cellBounds;
         TileBase[] allTiles = mainTileMap.GetTilesBlock(bounds);
         for (int x = 0; x< bounds.size.x; x++)
         {
             for (int y = 0; y < bounds.size.y; y++)
             {
                 TileBase tile = allTiles[x + y * bounds.size.x];
                 Instantiate(prefab, tile, Quaternion.identity);
             }
         }
     }*/

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.getStartPosition());
        //area.size = placeableObject.Size;
        TileBase[] baseArray = GetTilesBlock(area, mainTileMap);
        /*foreach (var b in baseArray)
        {
            if (b == whiteTile)
            {
                return false;
            }
        } */
        return true;
    }

    /*public void TakeArea(Vector3Int start, Vector3 size)
    {
        mainTileMap.BoxFill(start, whiteTile, start.x, start.y, 
            start.x + start.x, start.y + start.y);
    }*/

}
