using UnityEngine;
using System.Collections;

public class SimplePressAnimation : MonoBehaviour
{
    private const float ROTATE_SPEED = 360f;

    public void PlayAnimation()
    {
        StartCoroutine(SpinOnce());
    }

    private IEnumerator SpinOnce()
    {
        float targetAngle = transform.eulerAngles.y + 360f;
        float elapsedTime = 0f;
        float startAngle = transform.eulerAngles.y;

        while (elapsedTime < 1f)
        {
            float currentAngle = Mathf.Lerp(startAngle, targetAngle, elapsedTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }
}
