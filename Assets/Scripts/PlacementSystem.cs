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
using static Object;
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
    private Dictionary<Vector3Int, Object> _tilesWithObjects2;

    private void Awake()
    {
        Current = this;
        _grid = GridLayout.GetComponent<Grid>();
        _objectsInScene = new List<GameObject>();
        _availableTiles = new List<Vector3Int>();
        _tilesWithObjects = new List<Vector3Int>();
        _tilesWithObjects2 = new Dictionary<Vector3Int, Object>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnObjectsWithRules2();
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
    /*public void SpawnObjectsWithRules()
    {
        DestroyAllObjects();
        while (_availableTiles.Count > 0)
        {
            List<GameObject> objectsToTry = _objects;
            Debug.Log("wtff " + _objects.Count);
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
                    Debug.Log("Coloquei");
                }
                else
                {
                    // try another object
                    objectsToTry.Remove(obj);
                    Debug.Log("Não coloquei");
                    Debug.Log("Object count " + objectsToTry.Count);
                    Debug.Log("tiles available: " + _availableTiles.Count);
                    if (objectsToTry.Count <= 0)
                    {
                        _availableTiles.Remove(randomTile);
                    }
                }
            }            
        }
        Debug.Log("There is no more available tiles to place the object");
    }*/

    private void SpawnObjectsWithRules2()
    {
        DestroyAllObjects();
        while (_availableTiles.Count > 0)
        {
            List<int> objectsToTryIndexes = new List<int>();
            for (int i = 0; i < _objects.Count; i++)
            {
                objectsToTryIndexes.Add(i);
            }
            int numberObjectsAvailable = _objects.Count;
            bool placedObject = false;
            Vector3Int randomTile = _availableTiles[Random.Range(0, _availableTiles.Count)];
            Debug.Log("RandomTile: " + randomTile);
            while (numberObjectsAvailable > 0 && !placedObject)
            {
                Debug.Log("Numero de objetos a tentar = " + numberObjectsAvailable);
                int objectIndex = Random.Range(0, numberObjectsAvailable);
                Debug.Log("ObjectIndex= " + objectIndex);
                Debug.Log("ObjectToTryIndex = " + objectsToTryIndexes[objectIndex]);
                GameObject obj = _objects[objectsToTryIndexes[objectIndex]];
                Debug.Log("Objeto: " + obj);
                Dictionary<ObjectTypes, int[]> availableAdjacentPositionsPercentages = getObjectAvailableAdjacentPosition2(obj);
                Dictionary<Vector3Int, Object> adjacentObjectsAndPositions = getAdjacentObjects(randomTile);
                bool canPlace = compareProbabilities(availableAdjacentPositionsPercentages, adjacentObjectsAndPositions, randomTile);
                if (canPlace)
                {
                    Vector3 position = SnapCoordinateToGrid(randomTile);
                    Instantiate(_objects[objectsToTryIndexes[objectIndex]], position, Quaternion.identity);
                    _objectsInScene.Add(obj);
                    _availableTiles.Remove(randomTile);
                    _tilesWithObjects2.Add(randomTile,getObjectType(obj));
                    placedObject = true;
                    Debug.Log("Coloquei");
                } else
                {
                    // try another object
                    numberObjectsAvailable--;
                    objectsToTryIndexes.RemoveAt(objectIndex);
                    for (int i=0;i<numberObjectsAvailable;i++)
                    {
                        Debug.Log("Indices disponiveis: " + objectsToTryIndexes[i]);
                    }
                    Debug.Log("Não coloquei");
                    if (numberObjectsAvailable == 0)
                    {
                        _availableTiles.Remove(randomTile);
                        Debug.Log("Não coloquei nada neste tile");
                    }
                }
            }
        }
        Debug.Log("There is no more available tiles to place the object");
    }

    // Returns a bool[] of size 8, representing the available adjacent position of a given object.
    // The first position a the array is the top, and goes clock-wise until the last position, which is the diagonal top-left
    /*private bool[] getObjectAvailableAdjacentPosition(GameObject obj)//, bool[] adjacentAvailablePositions)
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
    }*/

    private Dictionary<ObjectTypes,int[]> getObjectAvailableAdjacentPosition2(GameObject obj)
    {
        Dictionary<ObjectTypes, int[]> objectAvailablePosition2 = new Dictionary<ObjectTypes, int[]>();
        if (obj.TryGetComponent<Chair>(out Chair chair)){
            chair.setAdjacentAvailablePositions2();
            objectAvailablePosition2 = chair.getAdjacentAvailablePositions2();
        }
        if (obj.TryGetComponent<Table>(out Table table))
        {
            table.setAdjacentAvailablePositions2();
            objectAvailablePosition2 = table.getAdjacentAvailablePositions2();
        }
        if (obj.TryGetComponent<Shelf>(out Shelf shelf))
        {
            shelf.setAdjacentAvailablePositions2();
            objectAvailablePosition2 = shelf.getAdjacentAvailablePositions2();
        }
        if (obj.TryGetComponent<Rug>(out Rug rug))
        {
            rug.setAdjacentAvailablePositions2();
            objectAvailablePosition2 = rug.getAdjacentAvailablePositions2();
        }
        return objectAvailablePosition2;
    }

    private Object getObjectType(GameObject obj)
    {
        if (obj.TryGetComponent<Chair>(out Chair chair)){
            return chair;
        }
        if (obj.TryGetComponent<Table>(out Table table))
        {
            return table;
        }
        if (obj.TryGetComponent<Shelf>(out Shelf shelf))
        {
            return shelf;
        }
        if (obj.TryGetComponent<Rug>(out Rug rug))
        {
            return rug;
        }
        return null;
    }

    private Dictionary<Vector3Int,Object> getAdjacentObjects(Vector3Int newPlacement)
    {
        List<Vector3Int> adjacentTiles = new List<Vector3Int>();
        Dictionary<Vector3Int, Object> adjacentObjectsAndPositions = new Dictionary<Vector3Int, Object>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                adjacentTiles.Add(newPlacement + new Vector3Int(i, 0, j));
            }
        } 
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            if (_tilesWithObjects2.ContainsKey(adjacentTiles[i]))
            {
                adjacentObjectsAndPositions.Add(adjacentTiles[i], _tilesWithObjects2.GetValueOrDefault(adjacentTiles[i]));
            }
        }
        Debug.Log("Nº de objetos adjacentes: " + adjacentObjectsAndPositions.Count);
        return adjacentObjectsAndPositions;
    }

    private bool compareProbabilities(Dictionary<ObjectTypes, int[]> probabilities, Dictionary<Vector3Int,Object> adjacentObjects,Vector3Int newPlacement)
    {
        bool canPlace = true;
        int placementProbability = 0;
        foreach(Vector3Int pos in adjacentObjects.Keys)
        {
            Object adjacentObj = adjacentObjects.GetValueOrDefault(pos);
            ObjectTypes objectType = convertObjectToObjectType(adjacentObj);
            int relativePositionIndex = convertCoordinatesToRelativePosition(pos, newPlacement);
            switch (relativePositionIndex)
            {
                case 0:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[0];
                    break;
                case 1:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[1];
                    break;
                case 2:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[2];
                    break;
                case 3:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[3];
                    break;
                case 4:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[4];
                    break;
                case 5:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[5];
                    break;
                case 6:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[6];
                    break;
                case 7:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[7];
                    break;
                case 8:
                    placementProbability = probabilities.GetValueOrDefault(objectType)[8];
                    break;
                default:
                    Debug.Log("Alguma coisa deu errado");
                    break;
            }
            Debug.Log("Probabilidade: " + placementProbability);
            int random = Random.Range(1, 101);
            Debug.Log("Random number: " + random);
            if (random > placementProbability)
            {
                canPlace = false;
                break;
            }
        }
        return canPlace;
    }

    private ObjectTypes convertObjectToObjectType(Object obj)
    {
        if (obj.TryGetComponent<Chair>(out Chair chair))
        {
            return ObjectTypes.Chair;
        }
        if (obj.TryGetComponent<Table>(out Table table))
        {
            return ObjectTypes.Table;
        }
        if (obj.TryGetComponent<Shelf>(out Shelf shelf))
        {
            return ObjectTypes.Shelf;
        }
        if (obj.TryGetComponent<Rug>(out Rug rug))
        {
            return ObjectTypes.Rug;
        }
        return ObjectTypes.Default;
    }
    private int convertCoordinatesToRelativePosition(Vector3Int adjacentPosition, Vector3Int newPlacement)
    {
        if (adjacentPosition == newPlacement)
        {
            return 0;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(0, 0, 1))
        {
            return 1;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(1, 0, 1))
        {
            return 2;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(1, 0, 0))
        {
            return 3;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(1, 0, -1))
        {
            return 4;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(0, 0, -1))
        {
            return 5;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(-1, 0, -1))
        {
            return 6;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(-1, 0, 0))
        {
            return 7;
        } else
        if (adjacentPosition == newPlacement + new Vector3Int(-1, 0, 1))
        {
            return 8;
        } else
        {
            return -1;
        }
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

    private List<Vector3Int> checkAvailableArea2(Vector3Int newPlacement, int[] adjacentPositionProbabilities)
    {
        List<Vector3Int> nonAvailableTiles = new List<Vector3Int>();
        Vector3Int adjacentPositionsUsed = new Vector3Int();
        for (int i = 0; i<adjacentPositionProbabilities.Length; i++)
        {
            return null;
        }
        return null;
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
