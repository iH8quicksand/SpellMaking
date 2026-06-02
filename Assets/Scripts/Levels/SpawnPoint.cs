using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public enum SpawnName
    {
        RED, GREEN, BONE
    }

    public SpawnName kind;
    public float spawnRadius = 1.8f; 

    public Vector3 GetRandomPosition()
    {
        Vector2 offset = Random.insideUnitCircle * spawnRadius;
        return transform.position + new Vector3(offset.x, offset.y, 0);
    }
}
