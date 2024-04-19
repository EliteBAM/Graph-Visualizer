using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class EdgeAnimator : MonoBehaviour
{

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public IEnumerator StartToEndAnimationRoutine(float animationDuration)
    {
        float startTime = Time.time;

        Vector3 startPosition = lineRenderer.GetPosition(0);
        Vector3 endPosition = lineRenderer.GetPosition(1);

        Vector3 currentPosition = startPosition;
        while (currentPosition != endPosition)
        {
            float t = (Time.time - startTime) / animationDuration;
            currentPosition = Vector3.Lerp(startPosition, endPosition, t);

            lineRenderer.SetPosition(1, currentPosition);

            yield return null;
        }
    }

    public IEnumerator EndToStartAnimationRoutine(float animationDuration)
    {
        float startTime = Time.time;

        Vector3 startPosition = lineRenderer.GetPosition(1);
        Vector3 endPosition = lineRenderer.GetPosition(0);

        Vector3 currentPosition = startPosition;
        while (currentPosition != endPosition)
        {
            float t = (Time.time - startTime) / animationDuration;
            currentPosition = Vector3.Lerp(startPosition, endPosition, t);

            lineRenderer.SetPosition(0, currentPosition);

            yield return null;
        }
    }

}
