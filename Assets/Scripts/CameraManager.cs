using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static Vector3 STARTPOS = new Vector3(0f, 1.5f, 0f);
    private static Vector3 LOADPOS = new Vector3(1.5f, 0.875f, 1.65f);
    private static float ANIMDURATION = 1f;
    private static float STARTROT = 13.405f;
    private static float LOADROT = 0f;

    private float time = 0f;

    [SerializeField]
    private SceneSwitcher sceneSwitcher;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject LoadText;

    public void MoveToLoad()
    {
        time = 0f;
        sceneSwitcher.disableMenu();
        LoadText.SetActive(false);
        StartCoroutine(MoveCameraCoroutine(STARTPOS, LOADPOS, STARTROT, LOADROT, time, "load"));
    }

    public void MoveToStart()
    {
        time = 0f;
        sceneSwitcher.disableMenu();
        LoadText.SetActive(true);
        StartCoroutine(MoveCameraCoroutine(LOADPOS, STARTPOS, LOADROT, STARTROT,time, "start"));
    }

    private IEnumerator MoveCameraCoroutine(Vector3 startPos, Vector3 endPos, float startRot, float endRot, float time, string type)
    {
        Quaternion startQuat = Quaternion.Euler(startRot, 0f, 0f);
        Quaternion endQuat = Quaternion.Euler(endRot, 0f, 0f);

        while (time < ANIMDURATION)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / ANIMDURATION);
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startQuat, endQuat, t);
            yield return null;
        }

        mainCamera.transform.position = endPos;
        if(type.Equals("load"))
        {
            sceneSwitcher.loadMenu();
        } else
        {
            sceneSwitcher.mainMenu();
        }
    }
}
