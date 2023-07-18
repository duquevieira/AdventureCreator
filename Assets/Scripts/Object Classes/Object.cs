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

    public void UpdateAvailableTopAreas(AvailableArea a, float objMinX, float objMaxX, float objMinZ, float objMaxZ)
    {
        /*float newMinX;
        float newMaxX;
        float newMinZ;
        float newMaxZ;
        if (objMinX > a.getMinX())
            newMinX = objMinX;
        else
            newMinX = a.getMinX();

        if (objMaxX < a.getMaxX())
            newMaxX = objMaxX;
        else
            newMaxX = a.getMaxX();

        if (objMinZ> a.getMinZ())
            newMinZ = objMinZ;
        else
            newMinZ = a.getMinZ();

        if (objMaxZ> a.getMaxZ())
            newMaxZ = objMaxZ;
        else
            newMaxZ = a.getMaxZ();

        AvailableArea n1 = new AvailableArea(a.getMinX(),newMinX,a.getMinZ(),newMinZ);
        AvailableArea n2 = new AvailableArea(newMaxX, a.getMaxX(), newMaxZ, a.getMaxZ());*/
        AvailableArea n1 = new AvailableArea(a.getMinX(), objMinX, a.getMinZ(), a.getMaxZ());
        AvailableArea n2 = new AvailableArea(objMaxX, a.getMaxX(), a.getMinZ(), a.getMaxZ());
        AvailableArea n3 = new AvailableArea(a.getMinX(), a.getMaxX(), a.getMinZ(), objMinZ);
        AvailableArea n4 = new AvailableArea(a.getMinX(), a.getMaxX(), objMaxZ, a.getMaxZ());
        OnTopAvailableAreas.Remove(a);
        OnTopAvailableAreas.Add(n1);
        OnTopAvailableAreas.Add(n2);
        OnTopAvailableAreas.Add(n3);
        OnTopAvailableAreas.Add(n4);
        if (a.getMinX() == objMinX)
            OnTopAvailableAreas.Remove(n1);
        if (objMaxX == a.getMaxX())
            OnTopAvailableAreas.Remove(n2);
        if (a.getMinZ()== objMinZ)
            OnTopAvailableAreas.Remove(n3);
        if (a.getMaxZ()== objMaxZ)
            OnTopAvailableAreas.Remove(n4);
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


