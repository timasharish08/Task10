using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MilitaryBase : MonoBehaviour
{
    [SerializeField] private Soldier _prefab;
    [SerializeField] private int _spawnCost;
    [SerializeField] private int _startSoldiersCount;

    [SerializeField] private float _scanDelay;

    private List<Soldier> _soldiers;

    private List<Resource> _scanedResources;
    private int _resourceCount;

    private void Awake()
    {
        _soldiers = new List<Soldier>();
        _scanedResources = new List<Resource>();
    }

    private void Start()
    {
        for (int i = 0; i < _startSoldiersCount; i++)
            Spawn();

        StartCoroutine(Scaning(new WaitForSeconds(_scanDelay)));
    }

    public void Take(Resource resource)
    {
        _resourceCount++;
        resource.Destroy();
        TrySpawn();
        TryGiveJob();
    }

    private void Spawn()
    {
        Soldier soldier = Instantiate(_prefab, transform.position, Quaternion.identity);
        soldier.Init(this);
        _soldiers.Add(soldier);
    }

    private void TrySpawn()
    {
        if (_resourceCount < _spawnCost)
            return;

        _resourceCount -= _spawnCost;
        Spawn();
    }

    private void TryGiveJob()
    {
        var soldiers = _soldiers.Where(solder => solder.IsWorking == false);

        foreach (Soldier soldier in soldiers)
        {
            if (_scanedResources.Count <= 0)
                return;

            soldier.Move(_scanedResources[0]);
            _scanedResources.RemoveAt(0);
        }
    }

    private IEnumerator Scaning(WaitForSeconds wait)
    {
        Resource[] resources = FindObjectsOfType<Resource>();

        foreach (Resource resource in resources)
        {
            if (_scanedResources.FirstOrDefault(scaned => scaned == resource) == null
                && resource.IsTarget == false && resource.gameObject.activeSelf)
                _scanedResources.Add(resource);
        }

        TryGiveJob();
        yield return wait;
        StartCoroutine(Scaning(wait));
    }
}
