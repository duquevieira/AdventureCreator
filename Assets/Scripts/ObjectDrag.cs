using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{

    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - PlacementSystem.getMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 pos = PlacementSystem.getMouseWorldPosition() + offset;
        transform.position = PlacementSystem.current.SnapCoordinateToGrid(pos);
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(this.gameObject);
        }
    }
}
