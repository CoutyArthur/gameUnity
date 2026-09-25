using UnityEngine;

public class TargetSpawn : MonoBehaviour
{
    [SerializeField] GameObject _target;
    [SerializeField] int xMin;
    [SerializeField] int xMax;
    [SerializeField] int zMin;
    [SerializeField] int zMax;

    public void spawnTarget()
    {
        Vector3 position = new Vector3 (Random.Range(xMin, xMax), 5, Random.Range(zMin, zMax));
        GameObject t = Instantiate(_target, position , Quaternion.identity);
    }

}
