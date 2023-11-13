using System;
using UnityEngine;

[RequireComponent(typeof(SoldierMover))]
public class Soldier : MonoBehaviour
{
    [SerializeField] private SoldierHands _hands;

    private SoldierMover _mover;

    private MilitaryBase _base;

    public bool IsWorking { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<SoldierMover>();
    }

    public void Init(MilitaryBase militaryBase)
    {
        _base = militaryBase;
    }

    public void Move(Resource resource)
    {
        resource.Target();
        IsWorking = true;
        _mover.MoveTo(resource.transform.position, new Action(PickUp));
        _hands.SetResource(resource);
    }

    public void PickUp()
    {
        if (_hands.TryPickUp() == false)
            return;

        _mover.MoveTo(_base.transform.position, new Action(Give));
    }

    public void Give()
    {
        if (_hands.TryGive(out Resource resource) == false)
            return;

        IsWorking = false;
        _base.Take(resource);
    }
}
