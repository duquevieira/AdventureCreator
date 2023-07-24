using System.Collections;
using UnityEngine;

public class SimplePressAnimation : MonoBehaviour
{
    private const float ROTATE_SPEED = 360f;
    public void PlayAnimation() {
        StartCoroutine(SpinOnce());
    }

    private IEnumerator SpinOnce() {
        float targetAngle = transform.eulerAngles.y + 360f;
        float elapsedTime = 0f;

        while(elapsedTime < 1f) {
            float currentAngle = Mathf.Lerp(transform.eulerAngles.y, targetAngle, elapsedTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }
}
