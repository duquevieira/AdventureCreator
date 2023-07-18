using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object: MonoBehaviour
{
    [HideInInspector] public Dictionary<ObjectTypes, int[]> Probabilities;
    //[HideInInspector] public enum AdjacentPositions { Top, TopLeft,TopRight, Right,Left,On,BottomLeft,Bottom,BottomRight };
    [HideInInspector] public enum ObjectTypes { Chair, Shelf, Table, Rug, Prop, Wall, Default};

    [HideInInspector] public List<AvailableArea> OnTopAvailableAreas;

    private void Awake()
    {
        OnTopAvailableAreas = new List<AvailableArea>();
        AvailableArea a = new AvailableArea(
            this.GetComponent<Collider>().transform.position.x - (this.GetComponent<Collider>().bounds.size.x / 2),
            this.GetComponent<Collider>().transform.position.x + (this.GetComponent<Collider>().bounds.size.x / 2),
            this.GetComponent<Collider>().transform.position.z - (this.GetComponent<Collider>().bounds.size.z / 2),
            this.GetComponent<Collider>().transform.position.z + (this.GetComponent<Collider>().bounds.size.z / 2));
        OnTopAvailableAreas.Add(a);
    }

    public List<AvailableArea> getOnTopAvailableAreas()
    {
        return OnTopAvailableAreas;
    }

    public void UpdateAvailableTopAreas(AvailableArea areaUsed, float objMinX, float objMaxX, float objMinZ, float objMaxZ)
    {
        AvailableArea newArea1 = new AvailableArea(areaUsed.getMinX(), objMinX, areaUsed.getMinZ(), areaUsed.getMaxZ());
        AvailableArea newArea2 = new AvailableArea(objMaxX, areaUsed.getMaxX(), areaUsed.getMinZ(), areaUsed.getMaxZ());
        AvailableArea newArea3 = new AvailableArea(areaUsed.getMinX(), areaUsed.getMaxX(), areaUsed.getMinZ(), objMinZ);
        AvailableArea newArea4 = new AvailableArea(areaUsed.getMinX(), areaUsed.getMaxX(), objMaxZ, areaUsed.getMaxZ());
        OnTopAvailableAreas.Remove(areaUsed);
        OnTopAvailableAreas.Add(newArea1);
        OnTopAvailableAreas.Add(newArea2);
        OnTopAvailableAreas.Add(newArea3);
        OnTopAvailableAreas.Add(newArea4);
        if (areaUsed.getMinX() == objMinX)
            OnTopAvailableAreas.Remove(newArea1);
        if (objMaxX == areaUsed.getMaxX())
            OnTopAvailableAreas.Remove(newArea2);
        if (areaUsed.getMinZ()== objMinZ)
            OnTopAvailableAreas.Remove(newArea3);
        if (areaUsed.getMaxZ()== objMaxZ)
            OnTopAvailableAreas.Remove(newArea4);
    }

    public virtual Dictionary<ObjectTypes, int[]> getProbabilities()
    {
        return Probabilities;
    }
    /*public virtual int[] getAdjacentPositionsProbabilities(ObjectTypes objectType)
    {
        return Probabilities.GetValueOrDefault(objectType);
    }*/

    public virtual void setProbabilities()
    {
        Probabilities = new Dictionary<ObjectTypes, int[]>();
    }

}

public class AvailableArea
{
    private float _minX, _maxX, _minZ, _maxZ;

    public AvailableArea(float minX, float maxX, float minZ, float maxZ)
    {
        _minX = minX;
        _maxX = maxX;
        _minZ = minZ;
        _maxZ = maxZ;
    }

    public float getMinX() { return _minX; }
    public float getMaxX() { return _maxX; }
    public float getMinZ() { return _minZ; }
    public float getMaxZ() { return _maxZ; }

}


