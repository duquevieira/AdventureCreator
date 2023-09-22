using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int _selectedObjectIndex;
    private Quaternion _rotation;
    private Vector2Int _size;
    private Vector3 _structureRotatedPosition;
    string Name;
    Grid Grid;
    PreviewSystem PreviewSystem;
    ObjectsDataBase Database;
    GridData ObjData;
    ObjectPlacer ObjectPlacer;

    public PlacementState(string name, Grid grid, PreviewSystem previewSystem, ObjectsDataBase database, GridData objData, ObjectPlacer objectPlacer)
    {
        Name = name;
        Grid = grid;
        PreviewSystem = previewSystem;
        Database = database;
        ObjData = objData;
        ObjectPlacer = objectPlacer;

        _structureRotatedPosition = new Vector3(0f, 0.1f, 0f);
        _selectedObjectIndex = -1;
        _selectedObjectIndex = Database.objectsDatabase.FindIndex(data => data.Name == name);
        _size = Database.objectsDatabase[_selectedObjectIndex].Size;
        _rotation = Database.objectsDatabase[_selectedObjectIndex].Prefab.transform.rotation;

        if (_selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(Database.objectsDatabase[_selectedObjectIndex].Prefab, _size);
        }
        else
            throw new System.Exception($"No object with name {name}");
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos);
        _structureRotatedPosition = GetPositionOfRotatedStructure(_rotation, Grid.CellToWorld(gridPos));
        ObjectData.ObjectTypes objType = Database.objectsDatabase[_selectedObjectIndex].Types;
        if (objType == ObjectData.ObjectTypes.Wall)
        {
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
            PreviewSystem.UpdatePreviewPosition(_structureRotatedPosition, placementValidity);
        }
        else if (objType == ObjectData.ObjectTypes.Prop)
        {
            float _maxY = GetMaximumHeightAt(gridPos);
            Vector3 _maxHeightPos = Grid.GetCellCenterWorld(gridPos) + new Vector3(0,_maxY,0);
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
            PreviewSystem.UpdatePreviewPosition(_maxHeightPos, placementValidity);
        } else
        {
            PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
            PreviewSystem.UpdatePreviewPosition(Grid.GetCellCenterWorld(gridPos), placementValidity);
        }
    }

    public void EndState()
    {
        PreviewSystem.StopShowingPreview();
    }
    public void OnAction(Vector3Int gridPos)
    {
        int index;
        bool placementValidity = CheckPlacementValidity(gridPos);
        if (!placementValidity)
            return;

        //GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);
        ObjectData.ObjectTypes objType = Database.objectsDatabase[_selectedObjectIndex].Types;
        if (objType== ObjectData.ObjectTypes.Animation)
        {
            //Fazer alguma coisa para mostrar que colocou a animacao no NPC
            //Colocar a animacao no NPC
        } else
        {
            if (objType == ObjectData.ObjectTypes.Wall)
            {
                index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, _structureRotatedPosition, _rotation, objType);
                PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), true);
                PreviewSystem.UpdatePreviewPosition(_structureRotatedPosition, true);
            }
            else if (objType == ObjectData.ObjectTypes.Prop)
            {
                float _maxY = GetMaximumHeightAt(gridPos);
                Vector3 _maxHeightPos = Grid.GetCellCenterWorld(gridPos) + new Vector3(0, _maxY, 0);
                index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, _maxHeightPos, _rotation, objType);
                PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), placementValidity);
                PreviewSystem.UpdatePreviewPosition(_maxHeightPos, placementValidity);
            }
            else if (objType == ObjectData.ObjectTypes.WallProp)
            {
                index = 0;
                Debug.Log("TODO");
            }
            else
            {
                index = ObjectPlacer.PlaceObject(Database.objectsDatabase[_selectedObjectIndex].Prefab, Grid.GetCellCenterWorld(gridPos), _rotation, objType);
                PreviewSystem.UpdateCursorPosition(Grid.CellToWorld(gridPos), false);
                PreviewSystem.UpdatePreviewPosition(Grid.GetCellCenterWorld(gridPos), false);
            }
            ObjData.AddObjectAt(gridPos, _size, Database.objectsDatabase[_selectedObjectIndex].Name, index, objType);
        }
        
    }
    public Quaternion Rotate()
    {
        ObjectData.ObjectTypes objType = Database.objectsDatabase[_selectedObjectIndex].Types;
        //GridData selectedData = GetSelectedData(Database.objectsDatabase[_selectedObjectIndex].Types);
        if (objType != ObjectData.ObjectTypes.Wall)
        {
            _rotation = PreviewSystem.RotatePreviewCenter();
            Vector2Int _newSize = PreviewSystem.RotateCursor(_size);
            _size = _newSize;
        }
        else
        {
            Transform transform = PreviewSystem.RotatePreviewEdges();
            _rotation = transform.rotation;
            _structureRotatedPosition = transform.position;
            _structureRotatedPosition += new Vector3(0, -0.06f, 0);
            Vector2Int _newSize = PreviewSystem.RotateCursor(_size);
            _size = _newSize;
        }
        return _rotation;
    }
    public void Drag()
    {
    }

    private Vector3 GetPositionOfRotatedStructure(Quaternion rotation, Vector3 gridPos)
    {
        switch (Mathf.Round(rotation.eulerAngles.y))
        {
            case 90:
                return gridPos;
            case 180:
                return gridPos += new Vector3Int(0, 0, 1);
            case 270:
                return gridPos += new Vector3Int(1, 0, 1);
            case 0:
                return gridPos += new Vector3Int(1, 0, 0);
            default:
                return gridPos;
        }
    }
   
    private bool CheckPlacementValidity(Vector3Int gridPos)
    {
        //GridData selectedData = GetSelectedData(Database.objectsDatabase[selectedObjectIndex].Types);
        ObjectData.ObjectTypes objType = Database.objectsDatabase[_selectedObjectIndex].Types;
        /*if (selectedData == StructureData)
            return true;            
        else
            return selectedData.CanPlacedObjectAt(gridPos, _size);*/
        return ObjData.CanPlacedObjectAt(gridPos, _size, objType);
    }

    private float GetMaximumHeightAt(Vector3Int gridPos)
    {
        if (ObjData.CheckObjectsAt(gridPos))
        {
            int _objectIndex = Database.objectsDatabase.FindIndex(data => data.Name == ObjData.getHighestObjectNameAt(gridPos));
            GameObject obj = Database.objectsDatabase[_objectIndex].Prefab;
            float _y;
            Renderer _renderer = obj.GetComponent<Renderer>();
            if (_renderer == null)
            {
                _renderer = obj.GetComponentInChildren<Renderer>();
                _y = _renderer.bounds.size.y + obj.transform.position.y;
            }
            else
            {
                _y = _renderer.bounds.size.y + obj.transform.position.y;
            }
            return _y;
        }
        else
            return 0.1f;
        
    }


}
