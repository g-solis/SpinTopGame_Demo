using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUtils : MonoBehaviour
{
    public static Vector2 RandomPointOnCircleEdge(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        return new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
    }

    public static float Vector2Range(Vector2 range)
    {
        return Random.Range(range.x,range.y);
    }

    public static int Vector2Range(Vector2Int range)
    {
        return Random.Range(range.x,range.y + 1);
    }
}
