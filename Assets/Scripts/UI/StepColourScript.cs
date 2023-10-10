using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepColourScript : MonoBehaviour
{

    [SerializeField]
    private Color _storyColour;
    [SerializeField]
    private Color _itemColour;

    void FixedUpdate()
    {
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;
    }

    public void ToggleStepColor()
    {
        gameObject.GetComponent<Image>().color = _itemColour;
    }
}
