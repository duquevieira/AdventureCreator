using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{

    public bool Placed { get; private set; }

    // Returns the position of the object
    public Vector3 getStartPosition()
    {
        return this.transform.position;
    }
    // Events that happen when an object is placed
    public virtual void Place()
    {
        Placed = true;
        // Invoke events on placement
    }

    // Rotate the object
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
    }



}
