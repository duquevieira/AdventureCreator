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

// www.youtube.com/watch?v=rKp9fWvmIww&t=342s

// Problemas: Destroy nao funciona; so calcula posicoes a volta e nao embaixo ou em cima; as posicoes a volta nao dependem da relacao entre 2 objetos (exemplo: mesa nao pode ter nada na diagonal agora, mas poderia so ter prateleiras na diagonal

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Current;
    public GridLayout GridLayout;
    private Grid _grid;
    [SerializeField] private Tilemap _mainTileMap;
    [SerializeField] private List<GameObject> _objects;
    private PlaceableObject _objectToPlace;
    private List<GameObject> _objectsInScene;
    private List<Vector3Int> _availableTiles;
    private List<Vector3Int> _tilesWithObjects;

    private void Awake()
    {
        Current = this;
        _grid = GridLayout.GetComponent<Grid>();
        _objectsInScene = new List<GameObject>();
        _availableTiles = new List<Vector3Int>();
        _tilesWithObjects = new List<Vector3Int>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnObjectsWithRules();
        }
        if (!_objectToPlace)
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
                Vector3Int start = GridLayout.WorldToCell(objectToPlace.getStartPosition());
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
                _availableTiles.Add(new Vector3Int(i, 0, j));
            }
        }
    }

    // returns the position of the mouse in World coordinates
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

    // gets the world position of the object 
    // returns the position of the object snapped to the grid
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = _mainTileMap.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);
        return position;
    }

    // Destroy all objects that are currently in the scene
    private void DestroyAllObjects()
    {
        for (int j = 0; j < _objectsInScene.Count; j++)
        {
            DestroyImmediate(_objectsInScene[j],true);
        }
        _objectsInScene = new List<GameObject>();
    }

    // Spawn the objects following the object specific adjacent rules 
    public void SpawnObjectsWithRules()
    {
        DestroyAllObjects();
        while (_availableTiles.Count > 0)
        {
            List<GameObject> objectsToTry = _objects;
            bool placedObject = false;  
            //List<Object> canBeAboveOf = getObjectVerticalAttributes(obj);
            //List<Object> canBeBelowOf = getObjectAttributes(obj)[1];
            //obj.GetComponent<PlaceableObject>();
            //Vector3Int start = GridLayout.WorldToCell(randomTile);   
            //MyTakeArea(randomTile, availableAdjacentPositions);
            Vector3Int randomTile = _availableTiles[Random.Range(0, _availableTiles.Count)];
            while (objectsToTry.Count > 0 && !placedObject)
            {
                bool objectIsValid = true;
                int objectIndex = Random.Range(0, objectsToTry.Count);
                GameObject obj = _objects[objectIndex];
                bool[] availableAdjacentPositions = getObjectAvailableAdjacentPosition(obj);
                List<Vector3Int> nonAvailableTiles = checkAvailableArea(randomTile, availableAdjacentPositions);
                for (int j =0; j< nonAvailableTiles.Count; j++)
                {
                    if (_tilesWithObjects.Contains(nonAvailableTiles[j]))
                    {
                        objectIsValid = false;
                        break;
                    }
                }
                if (objectIsValid)
                {
                    Vector3 position = SnapCoordinateToGrid(randomTile);
                    Instantiate(_objects[objectIndex], position, Quaternion.identity);
                    _objectsInScene.Add(obj);
                    objectsToTry = _objects;
                    for (int i = 0; i < nonAvailableTiles.Count; i++)
                    {
                        if (_availableTiles.Contains(nonAvailableTiles[i]))
                        {
                            _availableTiles.Remove(nonAvailableTiles[i]);
                        }
                    }
                    _tilesWithObjects.Add(randomTile);
                    placedObject = true;
                }
                else
                {
                    // try another object
                    objectsToTry.Remove(obj);
                    if (objectsToTry.Count <= 0)
                    {
                        _availableTiles.Remove(randomTile);
                    }
                }
            }            
        }
        Debug.Log("There is no more available tiles to place the object");
    }

    // Returns a bool[] of size 8, representing the available adjacent position of a given object.
    // The first position a the array is the top, and goes clock-wise until the last position, which is the diagonal top-left
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

    // Checks what adjacent positions are valid and returns the position of the adjacent tiles not available to put the object
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


}
