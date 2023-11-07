using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InteractInput : MonoBehaviour
{
    private Camera _camera;

    private MilitaryBase _selected;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out MilitaryBase militaryBase))
                _selected = militaryBase;
            else if (_selected != null)
                _selected.PlaceFlag(hit.point);
        }
    }
}
