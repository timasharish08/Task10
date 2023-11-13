using System;
using System.Collections;
using UnityEngine;

public class SoldierMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _interactionRange;

    public void MoveTo(Vector3 position, Action action)
    {
        StartCoroutine(Move(position, action));
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
