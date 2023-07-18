using UnityEngine;

public class Load : MonoBehaviour, MenuObject
{
    public Camera MainCamera;
    public GameObject LoadMenu;
    public GameObject MainCanvas;
    public GameObject LoadCanvas;
    private static Vector3 LOAD_POS = new Vector3(250, 0, -75);
    private static Vector3 MAIN_POS = new Vector3(0, 0, -75);

    public void runFunction() {
        LoadCanvas.SetActive(true);
        LoadMenu.SetActive(true);
        MainCanvas.SetActive(false);
        MainCamera.transform.position = LOAD_POS;
    }

    public void returnFunction() {
        MainCanvas.SetActive(true);
        LoadMenu.SetActive(false);
        LoadCanvas.SetActive(false);
        MainCamera.transform.position = MAIN_POS;
    }
}
