using UnityEngine;

public class Configurations : MonoBehaviour {

    [Header("Physics Settings")]
    [SerializeField] private float defaultMaxAngularSpeed;
    [SerializeField] private Vector3 gravity;
    [SerializeField] private bool autoSyncTransform;
    [SerializeField] private int defaultSolverIterations;
    [SerializeField] private int defaultSolverVelocityIterations;

    private void Start() {
        float defaultMaxAngularSpeed = Physics.defaultMaxAngularSpeed;
        Vector3 gravity =  Physics.gravity;
        bool autoSyncTransform = Physics.autoSyncTransforms;
        int defaultSolverIterations = Physics.defaultSolverIterations;
        int defaultSolverVelocityIterations = Physics.defaultSolverVelocityIterations;

        print($"-=> {defaultMaxAngularSpeed} - {gravity} - {autoSyncTransform} - {defaultSolverIterations} - {defaultSolverVelocityIterations}");
    }
}
