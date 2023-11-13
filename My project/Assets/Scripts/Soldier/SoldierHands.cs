using UnityEngine;

public class SoldierHands : MonoBehaviour
{
    public Resource Resource { get; private set; }

    public void SetResource(Resource resource)
    {
        Resource = resource;
    }

    public bool TryPickUp()
    {
        if (Resource == null)
            return false;

        Resource.transform.parent = transform;
        Resource.transform.position = transform.position;
        return true;
    }

    public bool TryGive(out Resource resource)
    {
        if (Resource == null)
        {
            resource = null;
            return false;
        }

        Resource.transform.parent = null;
        resource = Resource;
        Resource = null;
        return true;
    }
}
