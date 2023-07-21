using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSelectedUpdater : MonoBehaviour
{   
    private const string SELECTED_SAVE = "SELECTED SAVE: ";
    [SerializeField]
    private LoadList _loadList;

    void Update() {
        if(!string.IsNullOrEmpty(_loadList.SelectedSave)) {
            this.GetComponent<TextMeshProUGUI>().text = SELECTED_SAVE + _loadList.SelectedSave;
        }
    }
}
