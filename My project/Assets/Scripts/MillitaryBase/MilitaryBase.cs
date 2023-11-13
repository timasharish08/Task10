using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MilitaryBase : MonoBehaviour
{
    public event UnityAction ResourceTaked;

    [SerializeField] private float _scanDelay;

    [SerializeField] private Flag _flag;

    private List<Soldier> _soldiers;

    private List<Resource> _scanedResources;

    public Flag Flag => _flag;

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

    public void AddSoldier(Soldier soldier)
    {
        soldier.Init(this);
        _soldiers.Add(soldier);
    }

    public void PlaceFlag(Vector3 position)
    {
        _flag.Place(position);
    }

    public void Take(Resource resource)
    {
        resource.Destroy();
        ResourceTaked.Invoke();
        TryGiveJob();
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

    private IEnumerator Scaning(WaitForSeconds delay)
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
        yield return delay;
        StartCoroutine(Scaning(delay));
    }
}
