using UnityEngine;

public class Configurations : MonoBehaviour {

    [Header("Physics Settings")]
    [SerializeField] private float defaultMaxAngularSpeed;
    [SerializeField] private Vector3 gravity;
    [SerializeField] private bool autoSyncTransform;
    [SerializeField] private int defaultSolverIterations;
    [SerializeField] private int defaultSolverVelocityIterations;

    private void Start() {
        Physics.defaultMaxAngularSpeed = defaultMaxAngularSpeed;
        Physics.gravity = gravity;
        Physics.autoSyncTransforms = autoSyncTransform;
        Physics.defaultSolverIterations = defaultSolverIterations;
        Physics.defaultSolverVelocityIterations = defaultSolverVelocityIterations;
    }
}
