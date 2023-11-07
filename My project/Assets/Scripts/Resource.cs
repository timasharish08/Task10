using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public event UnityAction Destroyed;
    public event UnityAction<Resource> Targeted;

    public bool IsTarget { get; private set; }

    public void Destroy()
    {
        IsTarget = false;
        gameObject.SetActive(false);
        Destroyed.Invoke();
    }

    public void Target()
    {
        IsTarget = true;
        Targeted?.Invoke(this);
    }
}
