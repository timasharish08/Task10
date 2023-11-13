using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private float _spawnDelay;

    private WaitForSeconds _delay;

    private void Awake()
    {
        _delay = new WaitForSeconds(_spawnDelay);
    }

    private void OnEnable()
    {
        _resource.Destroyed += OnResourceDestroyed;
    }

    private void OnDisable()
    {
        _resource.Destroyed -= OnResourceDestroyed;
    }

    public void OnResourceDestroyed()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return _delay;

        _resource.transform.parent = transform;
        _resource.transform.position = transform.position;
        _resource.gameObject.SetActive(true);
    }
}
