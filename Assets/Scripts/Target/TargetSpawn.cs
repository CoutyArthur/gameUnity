using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    [SerializeField] GameObject _spawnArea;
    [SerializeField] GameObject _target;
    [SerializeField] float _heightOffset = 5f;
    [SerializeField, Range(0.2f, 1f)] float widthScale = 1f;
    [SerializeField, Range(0.2f, 1f)] float heightScale = 1f;
    [SerializeField, Range(0.2f, 1f)] float depthScale = 1f;

    public void spawnTarget()
    {
        Bounds b = _spawnArea.GetComponent<Renderer>().bounds;
        float width = b.max.x - b.min.x;
        float height = b.max.y - b.min.y;
        float depth = b.max.z - b.min.z;

        float minX = b.center.x - width * (widthScale / 2);
        float maxX = b.center.x + width * (widthScale / 2);
        float minY = b.center.y - height * (heightScale / 2);
        float maxY = b.center.y + height * (heightScale / 2);
        float minZ = b.center.z - depth * (depthScale / 2);
        float maxZ = b.center.z + depth * (depthScale / 2);

        Vector3 position = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
        );

        Instantiate(_target, position, Quaternion.identity);
    }
}
