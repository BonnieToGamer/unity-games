using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static IEnumerator SmoothMove(this Vector2 origin, Vector2 target, float speed, int maxIterations, System.Action<Vector2> callback, System.Action finished = null)
    {
        int iterations = 0;
        while (origin != target)
        {
            origin = Vector2.Lerp(origin, target, speed * Time.deltaTime);
            callback(origin);

            if (++iterations > maxIterations)
                origin = target;

            yield return null;
        }
        finished?.Invoke();
    }

    public static IEnumerator Spring(this Vector2 origin, Vector2 target, float stiffness, int maxIterations, System.Action<Vector2> callback, float friction = 0.999f)
    {
        Vector2 previousPosition = Vector2.zero;
        int iterations = 0;

        while (origin != target)
        {
            // f = k * x
            Vector2 positionBeforeUpdate = origin;
            Vector2 distance = target - origin;
            Vector2 springForce = distance * stiffness;

            origin += (origin - previousPosition + springForce) * friction;

            previousPosition = positionBeforeUpdate;

            if (++iterations > maxIterations)
                origin = target;

            callback(origin);

            yield return null;
        }
    }

    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
}