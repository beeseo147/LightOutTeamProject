using UnityEngine;
using UnityEngine.XR;

public class PuzzleInteractor : MonoBehaviour
{
    public GameObject puzzlePrefab; // ���� ���� ������
    public Transform zoomSpawnPoint; // VR ī�޶� �� ���� ��ġ
    public XRNode inputSource = XRNode.RightHand;

    private GameObject currentZoomedObject;
    private bool isInteracting;

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        bool triggerPressed;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed) && triggerPressed)
        {
            if (!isInteracting)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 5f))
                {
                    if (hit.collider.CompareTag("Puzzle")) 
                    {
                        ShowZoomedPuzzle(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                CloseZoomedPuzzle();
            }
        }
    }

    void ShowZoomedPuzzle(GameObject original)
    {
        isInteracting = true;

        currentZoomedObject = Instantiate(puzzlePrefab, zoomSpawnPoint.position, zoomSpawnPoint.rotation);
        currentZoomedObject.transform.localScale *= 2.0f;
    }

    void CloseZoomedPuzzle()
    {
        isInteracting = false;
        Destroy(currentZoomedObject);
    }
}
