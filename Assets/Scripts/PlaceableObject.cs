using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{

    public bool placed { get; private set; }

    public Vector3 getStartPosition()
    {
        return this.transform.position;
    }
    public virtual void Place()
    {
        placed = true;
        // Invoke events on placement
    }

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }



}
