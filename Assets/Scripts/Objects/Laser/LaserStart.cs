using Photon.Pun;
using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(LineRenderer))]
//[RequireComponent(typeof(PhotonView))]
public class LaserStart : MonoBehaviour
{
    [Header("Laser On / Off")]
    [SerializeField] bool isOn = true; // True : Drawing, False : Nothing

    [Header("Laser Setting")]
    [SerializeField] LaserRendererSettingSO laserRendererSetting;
    [SerializeField] LaserTarget laserTarget;
    LineRenderer lineRenderer;
    List<Vector3> hitPoints = new();

    //LaserTarget laserTarget;
    [SerializeField] int maxBounceCount = 10;
    const float farDistance = 300f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserRendererSetting.Apply(lineRenderer);
    }

    void FixedUpdate()
    {
        if (isOn)
        {
            RecalculatePath();
            DrawLaser();
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    /// <summary>레이저 경로를 다시 계산하며 pts 리스트를 채운다</summary>
    void RecalculatePath()
    {
        hitPoints.Clear();
        Vector3 start = transform.position;
        hitPoints.Add(start);

        ShootBeamRecur(start, transform.forward, 0);
    }
    /// <summary>LineRenderer에 pts 리스트를 그린다</summary>
    [PunRPC]
    void DrawLaser()
    {
        lineRenderer.positionCount = hitPoints.Count;
        lineRenderer.SetPositions(hitPoints.ToArray());
    }

    void ShootBeamRecur(Vector3 origin, Vector3 dir, int depth)
    {
        if (maxBounceCount <= depth)
            return;

        int defaultLayer = LayerMask.NameToLayer("Default");
        int mirrorLayer = LayerMask.NameToLayer("Mirror");
        int targetLayer = LayerMask.NameToLayer("LaserTarget");
        int hitMask = (1 << mirrorLayer) | (1 << targetLayer) | (1 << defaultLayer);

        if (Physics.Raycast(origin, dir, out var hit, farDistance, hitMask))
        {
            hitPoints.Add(hit.point);
            int layerHit = hit.collider.gameObject.layer;

            // 1) Mirror Hit : Reflection Repeat..
            if (layerHit == mirrorLayer)
            {
                laserTarget.DeActivate();

                //Debug.Log($"Mirror Hit, depth {depth}");
                Vector3 nextOrigin = hit.point + hit.normal * 0.001f; // 1 mm offset
                Vector3 nextDir = Vector3.Reflect(dir, hit.normal);
                ShootBeamRecur(nextOrigin, nextDir, depth + 1);
            }

            // 2) LaserTarget Hit : Finish Puzzle and Recursion
            else if (layerHit == targetLayer)
            {
                laserTarget.Activate();
                //Debug.Log($"Target Hit, depth {depth}");
                return;
            }
        }
        // 3) No Hit
        else
        {
            laserTarget.DeActivate();

            //Debug.Log($"No Hit, depth {depth}");
            hitPoints.Add(origin + dir * farDistance);
        }
    }
}
