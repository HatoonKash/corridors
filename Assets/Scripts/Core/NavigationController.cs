using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class NavigationController : MonoBehaviour {

    public Vector3 TargetPosition { get; set; } = Vector3.zero;

    public NavMeshPath CalculatedPath { get; private set; }

    public float arrivedDistance = 1f;
    public UnityEvent onDestinationReached;

    private void Start() {
        CalculatedPath = new NavMeshPath();
    }

    private void Update() {
        if (TargetPosition != Vector3.zero) {
            NavMesh.CalculatePath(transform.position, TargetPosition, NavMesh.AllAreas, CalculatedPath);
            if (Vector3.Distance(transform.position, TargetPosition) <= arrivedDistance)
            {
                onDestinationReached?.Invoke();
                Debug.Log("Destination Reached");
                TargetPosition = Vector3.zero;
                CalculatedPath.ClearCorners();
            }
        }
        
    }
}
