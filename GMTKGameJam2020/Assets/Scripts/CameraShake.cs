using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void ShakeCameraHard()
    {
        StartCoroutine("ShakeHard");
    }

    IEnumerator ShakeHard()
    {
        float intensity = 2.5f;
        float freq = 5.5f;
        float duration = 0.5f;
        while(t < duration)
        {
            //Debug.Log("Shake " + t);

            float x = (Mathf.PerlinNoise(Time.time * freq, 0) - 0.5f) * intensity;
            float y = (Mathf.PerlinNoise(0, Time.time * freq) - 0.5f) * intensity;

            x = Mathf.Lerp(0, x, t / duration);
            y = Mathf.Lerp(0, y, t / duration);

            t += Time.deltaTime;

            transform.localPosition = new Vector3(x, y, -15);

            yield return new WaitForEndOfFrame();
        }

        t = 0f;
        transform.localPosition = new Vector3(0, 0, -15);

        yield return null;
    }
}

