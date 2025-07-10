using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RullerDial : MonoBehaviour
{
    public int rullerIndex;
    public MoveRuller moveRuller;

    private XRBaseInteractable interactable;
    private bool isHeld = false;

    private float lastSnappedAngle = 0f;
    private float startAngle = 0f; // 잡기 시작할 때의 각도
    private int lastNumber = 0;

    // 18도 이상 회전시 36도로 스냅하기 위한 임계값
    private const float SNAP_THRESHOLD = 18f;
    private const float SNAP_STEP = 36f;

    void Start()
    {
        moveRuller = FindFirstObjectByType<MoveRuller>();
        interactable = GetComponent<XRBaseInteractable>();

        // XR Interaction 이벤트 리스너 등록
        interactable.selectEntered.AddListener(_ =>
        {
            isHeld = true;
            // 잡기 시작할 때의 각도 저장 (SignedAngle 사용)
            startAngle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.right);
            lastSnappedAngle = startAngle;
            // 현재 상태와 일치하도록 내부 상태 갱신
            
        });

        interactable.selectExited.AddListener(_ =>
        {
            isHeld = false;
            //SnapToNearestStep(); // 손 뗐을 때 각도 보정
            print($"[{rullerIndex}] 손 떼기 - 최종 각도: {Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.right):F2}");
            SyncStateWithTransform();
        });

        // 초기값 동기화
        SyncStateWithTransform();
    }

    void Update()
    {
        if (!isHeld) return;

        // Vector3.SignedAngle을 사용해 정확한 회전 각도 계산
        float currentAngle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.right);
        
        // 각도 차이 계산
        float angleDelta = Mathf.DeltaAngle(lastSnappedAngle, currentAngle);
        // 18도 이상 회전했는지 확인
        if (Mathf.Abs(angleDelta) >= SNAP_THRESHOLD && !isHeld)
        {
            // 회전 방향 결정 (양수: 시계방향, 음수: 반시계방향)
            int direction = angleDelta > 0 ? 1 : -1;
            
            // 36도 단위로 스냅
            float snapAngle = lastSnappedAngle + (direction * SNAP_STEP);
            
            // 각도 정규화 (-180 ~ 180도 범위)
            if (snapAngle > 180f) snapAngle -= 360f;
            if (snapAngle < -180f) snapAngle += 360f;
            
            // 트랜스폼 적용 (SignedAngle 기준으로 변환)
            //transform.localEulerAngles = new Vector3(snapAngle < 0 ? 360f + snapAngle : snapAngle, 0, 0);
            lastSnappedAngle = snapAngle;
            
            // 숫자 업데이트 (시계방향 회전시 숫자 증가)
            lastNumber += direction;
            if (lastNumber > 9) lastNumber = 0;
            if (lastNumber < 0) lastNumber = 9;
            
            // MoveRuller의 배열 업데이트
            moveRuller._numberArray[rullerIndex] = lastNumber;
            
            //// 이펙트 활성화 (선택된 룰러 표시)
            //UpdateRullerSelection();
        }
    }

    void SnapToNearestStep()
    {
        // 손을 뗐을 때 가장 가까운 36도 단위로 보정
        float currentAngle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.right);
        float snappedAngle = Mathf.Round(currentAngle / SNAP_STEP) * SNAP_STEP;
        
        // 각도 정규화 (-180 ~ 180도 범위)
        if (snappedAngle > 180f) snappedAngle -= 360f;
        if (snappedAngle < -180f) snappedAngle += 360f;
        
        // 트랜스폼 적용 (SignedAngle 기준으로 변환)
        transform.localEulerAngles = new Vector3(snappedAngle < 0 ? 360f + snappedAngle : snappedAngle, 0, 0);
        lastSnappedAngle = snappedAngle;
    }

    // 외부에서 호출 가능하게 public
    public void SyncStateWithTransform()
    {
        float currentAngle = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.left);

        
        float offsetAngle = currentAngle;

        int steps = Mathf.RoundToInt(offsetAngle / 36f) % 10;
        if (steps < 0) steps += 10;
        lastNumber = (steps+1)%10;

        print($"[{rullerIndex}] SyncState - currentAngle: {currentAngle:F2}, offsetAngle: {offsetAngle:F2}, steps: {steps}, lastNumber: {lastNumber}");
        moveRuller._numberArray[rullerIndex] = lastNumber;
        lastSnappedAngle = currentAngle;
    }

    //// 룰러 선택 상태 업데이트
    //void UpdateRullerSelection()
    //{
    //    // 현재 룰러를 선택된 상태로 설정
    //    moveRuller._changeRuller = rullerIndex;
        
    //    // 모든 룰러의 이펙트 업데이트
    //    for (int i = 0; i < moveRuller._rullers.Count; i++)
    //    {
    //        var emission = moveRuller._rullers[i].GetComponent<PadLockEmissionColor>();
    //        if (emission != null)
    //        {
    //            emission._isSelect = (i == rullerIndex);
    //            emission.BlinkingMaterial();
    //        }
    //    }
    //}
}