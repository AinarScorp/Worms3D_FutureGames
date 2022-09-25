using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    bool _isAvailable;

    public bool IsAvailable => _isAvailable;

    public Vector3 UseSpawnPoint()
    {
        _isAvailable = false;
        return transform.position;
    }

    public void ResetSpawnPoint() => _isAvailable = false;
}
