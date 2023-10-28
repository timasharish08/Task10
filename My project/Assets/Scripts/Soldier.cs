using System;
using System.Collections;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _interactionRange;

    [SerializeField] private Transform _hands;

    private MilitaryBase _base;
    private Resource _resource;

    public bool IsWorking { get; private set; }

    public void Init(MilitaryBase militaryBase)
    {
        _base = militaryBase;
    }

    public void Move(Resource resource)
    {
        resource.Target();
        IsWorking = true;
        StartCoroutine(Move(resource.transform.position, new Action(PickUp)));
        _resource = resource;
    }

    private void PickUp()
    {
        if (_resource == null)
            return;

        _resource.transform.parent = transform;
        _resource.transform.position = _hands.position;
        StartCoroutine(Move(_base.transform.position, new Action(Give)));
    }

    private void Give()
    {
        if (_resource == null)
            return;

        _resource.transform.parent = null;
        IsWorking = false;
        _base.Take(_resource);
    }

    private IEnumerator Move(Vector3 position, Action action)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (Vector3.Distance(transform.position, position) > _interactionRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, _speed * Time.deltaTime);
            yield return wait;
        }

        action.Invoke();
    }
}
