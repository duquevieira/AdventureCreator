using JetBrains.Annotations;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

//www.youtube.com/watch?v=rKp9fWvmIww&t=342s

//Problemas: Destroy nao funciona; so calcula posicoes a volta e nao embaixo ou em cima; as posicoes a volta nao dependem da relacao entre 2 objetos (exemplo: mesa nao pode ter nada na diagonal agora, mas poderia so ter prateleiras na diagonal

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem current;
    public GridLayout gridLayout;
    private Grid grid;
    [SerializeField] private Tilemap mainTileMap;
    private PlaceableObject objectToPlace;
    public List<GameObject> objects;
    private List<GameObject> objectsInScene;
    private List<Vector3Int> availableTiles;
    private List<Vector3Int> tilesWithObjects;

    private void Awake()
    {
        current = this;
        grid = gridLayout.GetComponent<Grid>();
        objectsInScene = new List<GameObject>();
        availableTiles = new List<Vector3Int>();
        tilesWithObjects = new List<Vector3Int>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnObjectsWithRules();
        }
        if (!objectToPlace)
        {
            return;
        }
        /*if (Input.GetKeyDown(KeyCode.LeftShift))
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
        }*/
    }

    private void Start()
    {
        for (int i = -5; i < 5; i++)
        {
            for (int j = -5; j < 5; j++)
            {
                availableTiles.Add(new Vector3Int(i, 0, j));
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
        Vector3Int cellPos = mainTileMap.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        return position;
    }

    private void DestroyAllObjects()
    {
        for (int j = 0; j < objectsInScene.Count; j++)
        {
            DestroyImmediate(objectsInScene[j],true);
        }
        objectsInScene = new List<GameObject>();
    }

    public void SpawnObjectsWithRules()
    {
        DestroyAllObjects();
        while (availableTiles.Count > 0)
        {
            List<GameObject> objectsToTry = objects;
            bool placedObject = false;  
            //List<Object> canBeAboveOf = getObjectVerticalAttributes(obj);
            //List<Object> canBeBelowOf = getObjectAttributes(obj)[1];
            //obj.GetComponent<PlaceableObject>();
            //Vector3Int start = gridLayout.WorldToCell(randomTile);   
            //MyTakeArea(randomTile, availableAdjacentPositions);
            Vector3Int randomTile = availableTiles[Random.Range(0, availableTiles.Count)];
            while (objectsToTry.Count > 0 && !placedObject)
            {
                bool objectIsValid = true;
                int objectIndex = Random.Range(0, objectsToTry.Count);
                GameObject obj = objects[objectIndex];
                bool[] availableAdjacentPositions = getObjectAvailableAdjacentPosition(obj);
                List<Vector3Int> nonAvailableTiles = checkAvailableArea(randomTile, availableAdjacentPositions);
                for (int j =0; j< nonAvailableTiles.Count; j++)
                {
                    if (tilesWithObjects.Contains(nonAvailableTiles[j]))
                    {
                        objectIsValid = false;
                        break;
                    }
                }
                if (objectIsValid)
                {
                    Vector3 position = SnapCoordinateToGrid(randomTile);
                    Instantiate(objects[objectIndex], position, Quaternion.identity);
                    objectsInScene.Add(obj);
                    objectsToTry = objects;
                    for (int i = 0; i < nonAvailableTiles.Count; i++)
                    {
                        if (availableTiles.Contains(nonAvailableTiles[i]))
                        {
                            availableTiles.Remove(nonAvailableTiles[i]);
                        }
                    }
                    tilesWithObjects.Add(randomTile);
                    placedObject = true;
                }
                else
                {
                    // try another object
                    objectsToTry.Remove(obj);
                    if (objectsToTry.Count <= 0)
                    {
                        availableTiles.Remove(randomTile);
                    }
                }
            }            
        }
        Debug.Log("There is no more available tiles to place the object");
    }

    /*private List<Object> getObjectVerticalAttributes(GameObject obj)//, bool[] adjacentAvailablePositions)
    {
        List<Object> attributes = new List<Object>();
        if (obj.TryGetComponent<Chair>(out Chair chair))
        {
            chair.setAttributes();
            attributes = chair.getCanBeAboveOf();
            //attributes[1] = chair.getCanBeBelowOf();
            //adjacentAvailablePositions = chair.getAdjacentAvailablePositions();
        }        
        return attributes;
    }*/

    private bool[] getObjectAvailableAdjacentPosition(GameObject obj)//, bool[] adjacentAvailablePositions)
    {
        bool[] availablePositions = new bool[8];
        if (obj.TryGetComponent<Chair>(out Chair chair))
        {
            chair.setAdjacentAvailablePositions();
            availablePositions = chair.getAdjacentAvailablePositions();
        }
        if (obj.TryGetComponent<Table>(out Table table))
        {
            table.setAdjacentAvailablePositions();
            availablePositions = table.getAdjacentAvailablePositions();
        }
        if (obj.TryGetComponent<Shelf>(out Shelf shelf))
        {

            shelf.setAdjacentAvailablePositions();
            availablePositions = shelf.getAdjacentAvailablePositions();
        }
        return availablePositions;
    }

    private List<Vector3Int> checkAvailableArea(Vector3Int newPlacement, bool[] objectToPlaceAvailablePositions)
    {
        List<Vector3Int> nonAvailableTiles = new List<Vector3Int>();
        Vector3Int adjacentPositionsUsed = new Vector3Int();
        nonAvailableTiles.Add(newPlacement);
        if (objectToPlaceAvailablePositions[0] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(0, 0, 1);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[1] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(1, 0, 1);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[2] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(1, 0, 0);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[3] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(1, 0, -1);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[4] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(0, 0, -1);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[5] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(-1, 0, -1);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[6] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(-1, 0, 0);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        if (objectToPlaceAvailablePositions[7] == false)
        {
            adjacentPositionsUsed = newPlacement + new Vector3Int(-1, 0, 1);
            nonAvailableTiles.Add(adjacentPositionsUsed);
        }
        return nonAvailableTiles;
    }

}
