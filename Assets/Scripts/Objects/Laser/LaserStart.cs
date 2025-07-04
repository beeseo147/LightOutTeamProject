using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(LineRenderer))]
public class LaserStart : MonoBehaviour
{
    /* ---------- Status ---------- */
    [Header("Laser On / Off")]
    [SerializeField] bool isOn = true; // True : Drawing, False : Nothing

    /* Holding ... */
    //bool isMoved;
    //Vector3 prevPos;
    //Quaternion prevRot;

    /* ---------- Laser Setting ---------- */
    [SerializeField] LaserRendererSettingSO laserRendererSetting;
    LineRenderer lineRenderer;
    List<Vector3> hitPoints = new();

    LaserTarget laserTarget;
    [SerializeField] int maxBounceCount = 5;
    const float farDistance = 300f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserRendererSetting.Apply(lineRenderer);
        //prevPos = transform.position;
    }

    void FixedUpdate()
    {
        //UpdateMoveFlag();

        if (isOn)
        {
            //if (isMoved || 0 == hitPoints.Count)
            //if (0 == hitPoints.Count)
                RecalculatePath();

            DrawLaser();
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    /* Holding ... */
    /// <summary>현재 프레임에 오브젝트가 이동했는지 판단하여 isMoved 갱신</summary>
    //void UpdateMoveFlag()
    //{
    //    /* ---------- Checking for movement ---------- */
    //    Vector3 curPos = transform.position;
    //    const float posEps = 0.0001f; // ~ 0.1mm distance OK 
    //    bool posMoved = (curPos - prevPos).sqrMagnitude > posEps * posEps;

    //    /* ---------- Checking for rotation ---------- */
    //    Quaternion curRot = transform.rotation;
    //    const float rotEps = 0.1f; // ~ 0.1°distance OK
    //    bool rotMoved = Quaternion.Angle(curRot, prevRot) > rotEps;

    //    isMoved = posMoved || rotMoved;

    //    /* ---------- Save for next frame comparison ---------- */
    //    prevPos = curPos;
    //    prevRot = curRot;
    //}
    /// <summary>레이저 경로를 다시 계산하며 pts 리스트를 채운다</summary>
    void RecalculatePath()
    {
        hitPoints.Clear();
        Vector3 start = transform.position;
        hitPoints.Add(start);

        ShootBeamRecur(start, transform.forward, 0);
    }
    /// <summary>LineRenderer에 pts 리스트를 그린다</summary>
    void DrawLaser()
    {
        lineRenderer.positionCount = hitPoints.Count;
        lineRenderer.SetPositions(hitPoints.ToArray());
    }

    void ShootBeamRecur(Vector3 origin, Vector3 dir, int depth)
    {
        if (maxBounceCount <= depth)
            return;

        int mirrorLayer = LayerMask.NameToLayer("Mirror");
        int targetLayer = LayerMask.NameToLayer("LaserTarget");
        int hitMask = (1 << mirrorLayer) | (1 << targetLayer);
        if (Physics.Raycast(origin, dir, out var hit, farDistance, hitMask))
        {
            hitPoints.Add(hit.point);
            int layerHit = hit.collider.gameObject.layer;

            // 1) Mirror Hit : Reflection Repeat..
            if (layerHit == mirrorLayer)
            {
                Debug.Log($"Mirror Hit, depth {depth}");
                Vector3 nextOrigin = hit.point + hit.normal * 0.001f; // 1 mm offset
                Vector3 nextDir = Vector3.Reflect(dir, hit.normal);
                ShootBeamRecur(nextOrigin, nextDir, depth + 1);
            }

            // 2) LaserTarget Hit : Finish Puzzle and Recursion
            else if (layerHit == targetLayer)
            {
                Debug.Log($"Target Hit, depth {depth}");
                return;
            }
        }
        // 3) No Hit
        else
        {
            Debug.Log($"No Hit, depth {depth}");
            hitPoints.Add(origin + dir * farDistance);
        }
    }
}
