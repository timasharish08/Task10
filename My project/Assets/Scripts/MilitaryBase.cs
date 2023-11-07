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

    [SerializeField] private MilitaryBase _copyPrefab;
    [SerializeField] private int _copyCost;
    [SerializeField] private Flag _flag;

    private List<Soldier> _soldiers;

    private List<Resource> _scanedResources;
    private int _resourceCount;

    private void Awake()
    {
        _soldiers = new List<Soldier>();
        _scanedResources = new List<Resource>();
    }

    private void OnEnable()
    {
        foreach (Resource resource in _scanedResources)
            resource.Targeted += OnResourceTargeted;
    }

    private void Start()
    {
        for (int i = 0; i < _startSoldiersCount; i++)
            Spawn();

        StartCoroutine(Scaning(new WaitForSeconds(_scanDelay)));
    }

    private void OnDisable()
    {
        foreach (Resource resource in _scanedResources)
            resource.Targeted -= OnResourceTargeted;
    }

    public void OnResourceTargeted(Resource resource)
    {
        resource.Targeted -= OnResourceTargeted;
        _scanedResources.Remove(resource);
    }

    public void PlaceFlag(Vector3 position)
    {
        _flag.Place(position);
    }

    public void Take(Resource resource)
    {
        _resourceCount++;
        resource.Destroy();
        TrySpawn();
        TryGiveJob();
    }

    private void TrySpawn()
    {
        if (_flag.gameObject.activeSelf)
        {
            if (_resourceCount < _copyCost)
                return;

            _resourceCount -= _copyCost;
            Copy();
        }
        else if (_resourceCount >= _spawnCost)
        {
            _resourceCount -= _spawnCost;
            Spawn();
        }
    }

    private void Copy()
    {
        _flag.Remove();
        Instantiate(_copyPrefab, _flag.transform.position, Quaternion.identity);
    }

    private void Spawn()
    {
        Soldier soldier = Instantiate(_prefab, transform.position, Quaternion.identity);
        soldier.Init(this);
        _soldiers.Add(soldier);
    }

    private void TryGiveJob()
    {
        var soldiers = _soldiers.Where(solder => solder.IsWorking == false);

        foreach (Soldier soldier in soldiers)
        {
            if (_scanedResources.Count <= 0)
                return;

            _scanedResources[0].Targeted -= OnResourceTargeted;
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
            {
                _scanedResources.Add(resource);
                resource.Targeted += OnResourceTargeted;
            }
        }

        TryGiveJob();
        yield return wait;
        StartCoroutine(Scaning(wait));
    }
}
