using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    [SerializeField] GameObject _spawnArea;
    [SerializeField] GameObject _target;
    [SerializeField] float _heightOffset = 5f;

    public void spawnTarget()
    {
        Bounds b = _spawnArea.GetComponent<Renderer>().bounds;

        Vector3 position = new Vector3(
            Random.Range(b.min.x, b.max.x),
            b.max.y + _heightOffset,
            Random.Range(b.min.z, b.max.z)
        );

        Instantiate(_target, position, Quaternion.identity);
    }
}
