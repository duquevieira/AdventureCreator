using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    private Text colliderText;
    [SerializeField]
    private Text dialogText;
    [SerializeField]
    private Text requirementText;

    public void setupText(string collidertxt, string dialogtxt, string requirementtxt)
    {
        colliderText.text = collidertxt;
        dialogText.text = dialogtxt;
        requirementText.text = requirementtxt;
    }

}
