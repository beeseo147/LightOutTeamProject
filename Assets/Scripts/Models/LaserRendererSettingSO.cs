using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(menuName = "Laser/Renderer Setting")]
public class LaserRendererSettingSO : ScriptableObject
{
    [SerializeField] private Color color;
    [SerializeField] private float width;
    [SerializeField] [Range(1f, 200f)] private float emissionAmount;

    public void Apply(LineRenderer lineRenderer)
    {
        lineRenderer.useWorldSpace = true;
        string path = "Materials/M_Laser";
        lineRenderer.material = Resources.Load<Material>(path);
        lineRenderer.material.color = color;
        lineRenderer.material.EnableKeyword("_EMISSION");
        lineRenderer.material.SetColor("_EmissionColor", color * emissionAmount);
        //lineRenderer.startColor = color;
        lineRenderer.startWidth = width;
        //lineRenderer.endColor = color;
        lineRenderer.endWidth = width;
    }
}
