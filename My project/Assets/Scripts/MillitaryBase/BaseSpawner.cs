using UnityEngine;

[RequireComponent(typeof(MilitaryBase))]
public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Soldier _soldierPrefab;
    [SerializeField] private int _soldierCost;
    [SerializeField] private int _startSoldiersCount;

    [SerializeField] private MilitaryBase _basePrefab;
    [SerializeField] private int _baseCost;

    private MilitaryBase _base;
    private int _resources;

    private void Awake()
    {
        _base = GetComponent<MilitaryBase>();
    }

    private void OnEnable()
    {
        _base.ResourceTaked += OnResourceTaked;
    }

    private void Start()
    {
        for (int i = 0; i < _startSoldiersCount; i++)
            Spawn();
    }

    private void OnDisable()
    {
        _base.ResourceTaked -= OnResourceTaked;
    }

    private void OnResourceTaked()
    {
        _resources++;

        if (_base.Flag.gameObject.activeSelf)
        {
            if (_resources < _baseCost)
                return;

            _resources -= _baseCost;
            SpawnBase();
        }
        else if (_resources >= _soldierCost)
        {
            _resources -= _soldierCost;
            Spawn();
        }
    }

    private void SpawnBase()
    {
        _base.Flag.Remove();
        Instantiate(_basePrefab, _base.Flag.transform.position, Quaternion.identity);
    }

    private void Spawn()
    {
        _base.AddSoldier(Instantiate(_soldierPrefab, transform.position, Quaternion.identity));
    }
}
