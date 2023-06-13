using JetBrains.Annotations;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static Object;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

// www.youtube.com/watch?v=rKp9fWvmIww&t=342s

// Problemas: Destroy nao funciona; so calcula posicoes a volta e nao embaixo ou em cima; as posicoes a volta nao dependem da relacao entre 2 objetos (exemplo: mesa nao pode ter nada na diagonal agora, mas poderia so ter prateleiras na diagonal

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Current;
    public GridLayout GridLayout;
    private Grid _grid;
    [SerializeField] private PlacementUI _placementUI;
    [SerializeField] private SwitchCreateMode switchMode;
    //[SerializeField] private ClickMenuSlot _clickMenuSlot;
    [SerializeField] private Tilemap _mainTileMap;
    [SerializeField] private GameObject floor;
    [SerializeField] private List<GameObject> _structureObjects;
    [SerializeField] public List<GameObject> _mainObjects;
    [SerializeField] private List<GameObject> _extraObjects;
    [SerializeField] private int width = 5;
    [SerializeField] private int height = 5;
    private PlaceableObject _objectToPlace;
    public List<GameObject> _objectsInScene;
    private List<Vector3Int> _availableTiles;
    private Dictionary<Vector3Int, List<Object>> _tilesWithObjects;
    private Vector3 clickedTile;
    private GameObject _selectedObject;
    
    private void Awake()
    {
        Current = this;
        _grid = GridLayout.GetComponent<Grid>();
        _objectsInScene = new List<GameObject>();
        _availableTiles = new List<Vector3Int>();
        _tilesWithObjects = new Dictionary<Vector3Int, List<Object>>();
    }

    private void Start()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new Vector3(i, 0.001f, j);
                var cloneObj = Instantiate(floor, pos, Quaternion.identity);
                cloneObj.name = cloneObj.name.Split("(")[0]; 
                _objectsInScene.Add(cloneObj);

            }
        }
        SpawnStructure();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DestroyAllObjects();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 pos = new Vector3(i, 0.001f, j);
                    var cloneObj = Instantiate(floor, pos, Quaternion.identity);
                    cloneObj.name = cloneObj.name.Split("(")[0];
                    _objectsInScene.Add(cloneObj);

                }
            }
            SpawnStructure();
            SpawnMainObjects();
            SpawnExtras();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (switchMode.currentMode == SwitchCreateMode.CreateMode.MapMode)
            {
                getClickedTile();
                AddObjectManually();
            }  
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
    
    public void SetSelectedObject(GameObject obj)
    {
        if (obj != null)
        {
            _selectedObject = obj;
            _selectedObject.transform.localScale = Vector3.one;
            _selectedObject.layer = 0;
        } else
        {
            _selectedObject = null;
        }
        
    }
   
    public void AddObjectManually()
    {
        if (_selectedObject!= null)
        {
            var cloneObj = Instantiate(_selectedObject, clickedTile, Quaternion.identity);
            cloneObj.name = cloneObj.name.Split("(")[0];
            _objectsInScene.Add(cloneObj);
            List<Object> objectsInOneTile = getObjectsInOneTile(convertFloatPosToTile(clickedTile));
            if (objectsInOneTile == null)
            {
                objectsInOneTile = new List<Object>();

            }
            objectsInOneTile.Add(getObjectType(cloneObj));
            _tilesWithObjects.Remove(convertFloatPosToTile(clickedTile));
            _tilesWithObjects.Add(convertFloatPosToTile(clickedTile), objectsInOneTile);
        }
    }
    public Vector3 getClickedTile()
    {
        Vector3 mousePos = getMouseWorldPosition();
        Vector3 pos = SnapCoordinateToGrid(mousePos);
        clickedTile = pos;
        return clickedTile;
    }
    private void ResetAvailableTiles()
    {
        for (int i = 0; i < width-1; i++)
        {
            for (int j = 0; j < height-1; j++)
            {
                _availableTiles.Add(new Vector3Int(i, 0, j));
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
        Vector3Int cellPos = _mainTileMap.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);

        return position;
    }
    public void DestroyAllObjects()
    {
        List<Transform> childs = new List<Transform>();
        for (int j = 0; j < _objectsInScene.Count; j++)
        {
            GameObject.Destroy(_objectsInScene[j]);
        }
        _tilesWithObjects.Clear();
        ResetAvailableTiles();
    }
    private void SpawnStructure()
    {
        Vector3Int position;
        int counter = 0;
        int min;
        int max;
        Quaternion rotation = Quaternion.identity;
        while (counter < 4)
        {
            switch (counter)
            {
                case 0:
                    min = 0;
                    max = height;
                    break;
                case 1:
                    min = 0;
                    max = width;
                    break;
                case 2:
                    min = -1;
                    max = width - 1;
                    break;
                case 3:
                    min = 1;
                    max = height + 1;
                    break;
                default:
                    min = 0;
                    max = 0;
                    break;

            }
            for (int i = min; i < max; i++)
            {
                switch (counter)
                {
                    case 0: //left
                        position = new Vector3Int(-1, 0, i);
                        rotation = Quaternion.Euler(0f, 90f, 0f);
                        break;
                    case 1: //down
                        position = new Vector3Int(i, 0, 0);
                        rotation = Quaternion.identity;
                        break;
                    case 2: //up
                        position = new Vector3Int(i, 0, height);
                        rotation = Quaternion.Euler(0f, -180f, 0f);
                        break;
                    case 3: //right
                        position = new Vector3Int(width - 1, 0, i);
                        rotation = Quaternion.Euler(0f, 270f, 0f);
                        break;
                    default:
                        position = new Vector3Int();
                        rotation = new Quaternion();
                        break;
                }
     
                List<int> objectsToTryIndexes = new List<int>();
                for (int j = 0; j < _structureObjects.Count; j++)
                {
                    objectsToTryIndexes.Add(j);
                }
                int numberObjectsAvailable = _structureObjects.Count;
                bool placedObject = false;
                while (numberObjectsAvailable > 0 && !placedObject)
                {
                    int objectIndex = Random.Range(0, numberObjectsAvailable);
                    GameObject obj = _structureObjects[objectsToTryIndexes[objectIndex]];
                    Debug.Log("Objeto: " + obj);
                    Dictionary<ObjectTypes, int[]> objectProbabilities = getObjectProbabilities(obj);
                    Dictionary<Vector3Int, List<Object>> adjacentObjects = getAdjacentObjects(position);
                    bool canPlace = compareProbabilities(objectProbabilities, adjacentObjects, position);
                    if (canPlace)
                    {
                        //Vector3 position = SnapCoordinateToGrid(randomTile);
                        var cloneObj = Instantiate(_structureObjects[objectsToTryIndexes[objectIndex]], position, rotation);
                        cloneObj.name = cloneObj.name.Split("(")[0];
                        _objectsInScene.Add(cloneObj);
                        //_availableTiles.Remove(randomTile);
                        List<Object> objectsInOneTile = getObjectsInOneTile(position);
                        if (objectsInOneTile == null)
                        {
                            objectsInOneTile = new List<Object>();

                        }
                        objectsInOneTile.Add(getObjectType(obj));
                        _tilesWithObjects.Remove(position);
                        _tilesWithObjects.Add(position, objectsInOneTile);
                        placedObject = true;
                        Debug.Log("Coloquei");
                    }
                    else
                    {
                        // try another object
                        numberObjectsAvailable--;
                        objectsToTryIndexes.RemoveAt(objectIndex);
                        Debug.Log("Nao coloquei");
                        if (numberObjectsAvailable == 0)
                        {
                            //_availableTiles.Remove(randomTile);
                            Debug.Log("Nothing placed on this tile");
                        }
                    }
                }
            }
            counter++;
        }
        
        Debug.Log("There is no more available tiles to place the object");
    }
    private void SpawnMainObjects()
    {
        //DestroyAllObjects();
        ResetAvailableTiles();
        while (_availableTiles.Count > 0)
        {
            GenerationProcess(_mainObjects);
        }
        Debug.Log("There is no more available tiles to place the object");
    }
    private void SpawnExtras()
    {
        //DestroyAllObjects();
        ResetAvailableTiles();
        while (_availableTiles.Count > 0)
        {
            GenerationProcess(_extraObjects);
        }
        Debug.Log("There is no more available tiles to place the object");
    }

    private void GenerationProcess(List<GameObject> listObjects) {
        List<int> objectsToTryIndexes = new List<int>();
        for (int i = 0; i < listObjects.Count; i++)
        {
            objectsToTryIndexes.Add(i);
        }
        int numberObjectsAvailable = listObjects.Count;
        bool placedObject = false;
        Vector3Int randomTile = _availableTiles[Random.Range(0, _availableTiles.Count)];
        Debug.Log("RandomTile: " + randomTile);
        while (numberObjectsAvailable > 0 && !placedObject)
        {
            int objectIndex = Random.Range(0, numberObjectsAvailable);
            GameObject obj = listObjects[objectsToTryIndexes[objectIndex]];
            Debug.Log("Objeto: " + obj);
            Dictionary<ObjectTypes, int[]> objectProbabilities = getObjectProbabilities(obj);
            Dictionary<Vector3Int, List<Object>> adjacentObjects = getAdjacentObjects(randomTile);
            bool canPlace = compareProbabilities(objectProbabilities, adjacentObjects, randomTile);
            if (canPlace)
            {
                List<Object> objectsInOneTile = getObjectsInOneTile(randomTile);
                Vector3 position = SnapCoordinateToGrid(randomTile);
                if (convertObjectToObjectType(getObjectType(obj)) == ObjectTypes.Prop)
                {
                    position.y = 0.5f;
                }
                var cloneObj = Instantiate(listObjects[objectsToTryIndexes[objectIndex]], position, Quaternion.identity);
                cloneObj.name = cloneObj.name.Split("(")[0];
                _objectsInScene.Add(cloneObj);
                //_availableTiles.Remove(randomTile);
                
                if (objectsInOneTile == null)
                {
                    objectsInOneTile = new List<Object>();

                }
                objectsInOneTile.Add(getObjectType(obj));
                _tilesWithObjects.Remove(randomTile);
                _tilesWithObjects.Add(randomTile, objectsInOneTile);
                placedObject = true;
                Debug.Log("Coloquei");
            }
            else
            {
                // try another object
                numberObjectsAvailable--;
                objectsToTryIndexes.RemoveAt(objectIndex);
                Debug.Log("Nao coloquei");
                if (numberObjectsAvailable == 0)
                {
                    _availableTiles.Remove(randomTile);
                    Debug.Log("Nothing placed on this tile");
                }
            }
        }
    }

    private float getObjectHeight(List<Object> objects)
    {
        float height = 0f;
        for (int i = 0; i<objects.Count; i++)
        { 
            if (objects[0].gameObject.transform.lossyScale.y > height)
            {
                height = objects[0].gameObject.transform.localScale.y;
            }
        }
        return height;
    }
    private List<Object> getObjectsInOneTile(Vector3Int tile)
    {
        return _tilesWithObjects.GetValueOrDefault(tile);
    }

    private Dictionary<ObjectTypes, int[]> getObjectProbabilities(GameObject obj)
    {
        Dictionary<ObjectTypes, int[]> objectProbabilities = new Dictionary<ObjectTypes, int[]>();
        if (obj.TryGetComponent<Chair>(out Chair chair))
        {
            chair.setProbabilities();
            objectProbabilities = chair.getProbabilities();
        }
        if (obj.TryGetComponent<Table>(out Table table))
        {
            table.setProbabilities();
            objectProbabilities = table.getProbabilities();
        }
        if (obj.TryGetComponent<Shelf>(out Shelf shelf))
        {
            shelf.setProbabilities();
            objectProbabilities = shelf.getProbabilities();
        }
        if (obj.TryGetComponent<Rug>(out Rug rug))
        {
            rug.setProbabilities();
            objectProbabilities = rug.getProbabilities();
        }
        if (obj.TryGetComponent<Prop>(out Prop prop))
        {
            prop.setProbabilities();
            objectProbabilities = prop.getProbabilities();
        }
        if (obj.TryGetComponent<Wall>(out Wall wall))
        {
            wall.setProbabilities();
            objectProbabilities = wall.getProbabilities();
        }
        return objectProbabilities;
    }

    private Object getObjectType(GameObject obj)
    {
        if (obj.TryGetComponent<Chair>(out Chair chair))
        {
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
        if (obj.TryGetComponent<Prop>(out Prop prop))
        {
            return prop;
        }
        if (obj.TryGetComponent<Wall>(out Wall wall))
        {
            return wall;
        }
        return null;
    }

    private Dictionary<Vector3Int, List<Object>> getAdjacentObjects(Vector3Int newPlacement)
    {
        List<Vector3Int> adjacentTiles = new List<Vector3Int>();
        Dictionary<Vector3Int, List<Object>> adjacentObjectsAndPositions = new Dictionary<Vector3Int, List<Object>>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                adjacentTiles.Add(newPlacement + new Vector3Int(i, 0, j));
            }
        }
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            if (_tilesWithObjects.ContainsKey(adjacentTiles[i]))
            {
                adjacentObjectsAndPositions.Add(adjacentTiles[i], _tilesWithObjects.GetValueOrDefault(adjacentTiles[i]));
            }
        }
        Debug.Log("N de objetos adjacentes: " + adjacentObjectsAndPositions.Count);
        return adjacentObjectsAndPositions;
    }

    private bool compareProbabilities(Dictionary<ObjectTypes, int[]> probabilities, Dictionary<Vector3Int, List<Object>> adjacentObjects, Vector3Int newPlacement)
    {
        bool canPlace = false;
        if (adjacentObjects.Count == 0)
        {
            canPlace = true;
        }
        int placementProbability = 0;
        foreach (Vector3Int pos in adjacentObjects.Keys)
        {
            List<Object> adjacentObjInPosition = adjacentObjects[pos];
            for (int i = 0; i < adjacentObjInPosition.Count; i++)
            {
                ObjectTypes objectType = convertObjectToObjectType(adjacentObjInPosition[i]);
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
                if (placementProbability != -1)
                {
                    if (random > placementProbability)
                    {
                        canPlace = false;
                        return canPlace;
                    }
                    else
                    {
                        canPlace = true;
                    }
                }
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
        if (obj.TryGetComponent<Prop>(out Prop prop))
        {
            return ObjectTypes.Prop;
        }
        if (obj.TryGetComponent<Wall>(out Wall wall))
        {
            return ObjectTypes.Wall;
        }
        return ObjectTypes.Default;
    }

    public Vector3Int convertFloatPosToTile(Vector3 pos)
    {
        Vector3Int convertedPos = new Vector3Int(Mathf.CeilToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
        return convertedPos;
    }
    private int convertCoordinatesToRelativePosition(Vector3Int adjacentPosition, Vector3Int newPlacement)
    {
        if (adjacentPosition == newPlacement)
        {
            return 0;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(0, 0, 1))
        {
            return 1;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(1, 0, 1))
        {
            return 2;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(1, 0, 0))
        {
            return 3;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(1, 0, -1))
        {
            return 4;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(0, 0, -1))
        {
            return 5;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(-1, 0, -1))
        {
            return 6;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(-1, 0, 0))
        {
            return 7;
        }
        else
        if (adjacentPosition == newPlacement + new Vector3Int(-1, 0, 1))
        {
            return 8;
        }
        else
        {
            return -1;
        }
    }

}
