using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MoveTexture : MonoBehaviour
{
    [System.Serializable]
    public class MaterialScrollSettings
    {
        public enum ScrollMode
        {
            Smooth,
            Step
        }

        [Tooltip("Material to scroll.")]
        public Material material;

        [Tooltip("Type of scrolling (Smooth or Step-based).")]
        public ScrollMode scrollMode = ScrollMode.Smooth;

        [Tooltip("Scroll direction (X = horizontal, Y = vertical). Use 0 to disable movement on an axis.")]
        public Vector2 direction = Vector2.right;

        [Header("Smooth Scroll Settings")]
        [Tooltip("Speed of smooth scrolling (units per second).")]
        public float smoothSpeed = 0.5f;

        [Header("Step Scroll Settings")]
        [Tooltip("Time (in seconds) between each texture step.")]
        public float stepInterval = 0.2f;

        [Tooltip("Offset size per step (in texture space).")]
        public float stepSize = 0.25f;

        [HideInInspector] public Vector2 currentOffset;
        [HideInInspector] public float stepTimer;
    }

    [Tooltip("List of materials and their scroll settings.")]
    public MaterialScrollSettings[] materialsToScroll;

    private void LateUpdate()
    {
        foreach (var matData in materialsToScroll)
        {
            if (matData.material == null)
                continue;

            switch (matData.scrollMode)
            {
                case MaterialScrollSettings.ScrollMode.Smooth:
                    HandleSmoothScroll(matData);
                    break;

                case MaterialScrollSettings.ScrollMode.Step:
                    HandleStepScroll(matData);
                    break;
            }
        }
    }

    private void HandleSmoothScroll(MaterialScrollSettings matData)
    {
        if (matData.direction == Vector2.zero)
            return;

        Vector2 baseOffset = matData.material.GetTextureOffset("_MainTex");
        Vector2 newOffset = baseOffset;

        // Update only active axes
        if (matData.direction.x != 0)
            newOffset.x += matData.direction.x * (matData.smoothSpeed * Time.deltaTime);
        if (matData.direction.y != 0)
            newOffset.y += matData.direction.y * (matData.smoothSpeed * Time.deltaTime);

        matData.material.SetTextureOffset("_MainTex", newOffset);
        matData.currentOffset = newOffset;
    }

    private void HandleStepScroll(MaterialScrollSettings matData)
    {
        if (matData.direction == Vector2.zero)
            return;

        matData.stepTimer += Time.deltaTime;

        if (matData.stepTimer >= matData.stepInterval)
        {
            matData.stepTimer = 0f;

            Vector2 baseOffset = matData.material.GetTextureOffset("_MainTex");
            Vector2 newOffset = baseOffset;

            // Update only active axes
            if (matData.direction.x != 0)
                newOffset.x += matData.direction.x * matData.stepSize;
            if (matData.direction.y != 0)
                newOffset.y += matData.direction.y * matData.stepSize;

            matData.material.SetTextureOffset("_MainTex", newOffset);
            matData.currentOffset = newOffset;
        }
    }

    private void OnDisable()
    {
        foreach (var matData in materialsToScroll)
        {
            if (matData.material == null)
                continue;

            matData.material.SetTextureOffset("_MainTex", matData.currentOffset);
            matData.stepTimer = 0f;
        }
    }

    private void OnDestroy()
    {
        foreach (var matData in materialsToScroll)
        {
            if (matData.material == null)
                continue;

            matData.material.SetTextureOffset("_MainTex", Vector2.zero);
            matData.stepTimer = 0f;
        }
    }
}
