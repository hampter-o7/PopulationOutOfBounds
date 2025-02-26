using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private float lightTransitionTimeInSeconds = 2;
    private readonly List<Light2D> lights = new();
    private bool isLightActive = true;

    public void ToggleLights()
    {
        isLightActive = !isLightActive;
        StartCoroutine(FadeLights());
    }

    private void Start()
    {
        Light2D[] foundLights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
        lights.AddRange(foundLights);
        foreach (var light in foundLights) light.intensity = light.lightType == Light2D.LightType.Global ? 1 : 0;
    }

    private IEnumerator FadeLights()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lightTransitionTimeInSeconds)
        {
            float t = elapsedTime / lightTransitionTimeInSeconds;
            foreach (Light2D light in lights) light.intensity = Mathf.Lerp(light.lightType == Light2D.LightType.Global ? isLightActive ? 0 : 1 : isLightActive ? 1 : 0, light.lightType == Light2D.LightType.Global ? isLightActive ? 1 : 0 : isLightActive ? 0 : 1, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        foreach (Light2D light in lights) light.intensity = (light.lightType == Light2D.LightType.Global) ? (1 - (isLightActive ? 0 : 1)) : isLightActive ? 0 : 1;
    }
}
