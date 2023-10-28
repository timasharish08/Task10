using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private float _delay;

    private WaitForSeconds wait;

    private void Awake()
    {
        wait = new WaitForSeconds(_delay);
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
        yield return wait;
        _resource.transform.position = transform.position;
        _resource.gameObject.SetActive(true);
    }
}
