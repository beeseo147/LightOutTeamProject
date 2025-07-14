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
        //Material originalMat = Resources.Load<Material>(path);
        //Material matInstance = new Material(originalMat);

        //matInstance.color = color.linear;
        //matInstance.EnableKeyword("_EMISSION");
        //matInstance.SetColor("_EmissionColor", color.linear * emissionAmount);
        //lineRenderer.material = matInstance;
        lineRenderer.material.color = color.linear;
        lineRenderer.material.EnableKeyword("_EMISSION");
        lineRenderer.material.SetColor("_EmissionColor", color.linear * emissionAmount);
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}
