using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun; // 추가
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
//현주옥 작성
//김동균 수정
[System.Serializable]
public class DialogueSegment
{
    public float startTime;
    public float endTime;
    public string speaker;
    [TextArea(3, 5)]
    public string dialogue;
}

public class DialogueFirst : MonoBehaviourPun, ICheckPeopleEvent
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Transform dialoguePanel;

    [Header("Dialogue Data")]
    [SerializeField] private DialogueSegment[] dialogueSegments;

    [Header("Settings")]
    [SerializeField] private bool autoStart = false;
    [SerializeField] private bool loopDialogue = false;
    [SerializeField] private KeyCode triggerKey = KeyCode.F1;
    [SerializeField] private bool useFadeTransition = true;
    [SerializeField] private float fadeSpeed = 0.3f; // 빠른 페이드
    private bool initialized = false;
    //keycode = 확인용으로 추가해둔 거라 실제 사용에는 안씀
    // Private variables
    private bool isPlaying = false;
    private int currentSegmentIndex = 0;
    private Coroutine dialogueCoroutine;
    private float dialogueStartTime;
    private Coroutine fadeCoroutine;
    private bool dialogueStarted = false;
    XROrigin xROrigin;
    Camera myCamera;
    void Start()
    {
        // Disable panel at start
        if (dialoguePanel != null)
        {
            dialoguePanel.gameObject.SetActive(false);
        }
        if (autoStart)
        {
            StartCoroutine(AutoStartDialogueWhenReady());
        }
    }

    void Update()
    {
        if (dialoguePanel != null)
        {
            // Update dialogue if playing
            if (isPlaying)
            {
                UpdateDialogue();
            }
            // Dialogue UI가 항상 카메라 앞에 오도록(고정/추적)
            
            if (myCamera != null)
            {
                Vector3 uiPos = myCamera.transform.position + myCamera.transform.forward * 1.2f;
                dialoguePanel.transform.position = uiPos;
                dialoguePanel.LookAt(myCamera.transform.position); // 카메라를 바라보도록 회전
            }
        }
    }
    IEnumerator AutoStartDialogueWhenReady()
    {
        PlayerSetup myPlayer = null;
        Camera myCamera = null;

        // 내 PlayerSetup이 생성될 때까지 대기
        while (myPlayer == null)
        {
            foreach (var ps in GameObject.FindObjectsByType<PlayerSetup>(FindObjectsSortMode.None))
            {
                if (ps.transform.GetComponent<PhotonView>().IsMine)
                {
                    myPlayer = ps;
                    break;
                }
            }
            yield return null;
        }

        // 내 XR Origin, 내 Camera가 생성될 때까지 대기
        while (xROrigin == null || myCamera == null)
        {
            xROrigin = myPlayer.GetComponentInChildren<XROrigin>(true);
            if (xROrigin != null)
                myCamera = xROrigin.GetComponentInChildren<Camera>(true);
            yield return null;
        }

        if (!dialogueStarted)
        {
            StartDialogue();
            dialogueStarted = true;
        }
    }
    void StartDialogue()
    {
        if (audioSource == null || dialogueText == null)
        {
            Debug.LogError("AudioSource or DialogueText is not assigned!");
            return;
        }
        // Enable dialogue panel
        if (dialoguePanel != null)
        {
            dialoguePanel.gameObject.SetActive(true);
        }

        isPlaying = true;
        currentSegmentIndex = 0;
        dialogueStartTime = Time.time;
        audioSource.PlayOneShot(audioSource.clip);

        // Start dialogue coroutine
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);
        dialogueCoroutine = StartCoroutine(DialogueCoroutine());

        Debug.Log("Dialogue started!");
    }

    void StopDialogue()
    {
        isPlaying = false;

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
            dialogueCoroutine = null;
        }

        // Clear text
        if (dialogueText != null)
            dialogueText.text = "";

        // Disable dialogue panel
        if (dialoguePanel != null)
        {
            dialoguePanel.gameObject.SetActive(false);
        }

        Debug.Log("Dialogue stopped!");
    }

    void UpdateDialogue()
    {
        float currentTime = Time.time - dialogueStartTime;

        // Find current segment
        for (int i = 0; i < dialogueSegments.Length; i++)
        {
            if (currentTime >= dialogueSegments[i].startTime &&
                currentTime <= dialogueSegments[i].endTime)
            {
                if (currentSegmentIndex != i)
                {
                    currentSegmentIndex = i;
                    DisplayDialogue(dialogueSegments[i]);
                    Debug.Log($"Switched to segment {i}: {dialogueSegments[i].speaker} - {dialogueSegments[i].dialogue}");
                }
                return;
            }
        }

        // If no segment found, clear text
        if (dialogueText.text != "")
        {
            dialogueText.text = "";
            Debug.Log("No segment found, cleared text");
        }
    }

    void DisplayDialogue(DialogueSegment segment)
    {
        if (dialogueText != null)
        {
            string fullText = $"{segment.speaker} {segment.dialogue}";

            if (useFadeTransition)
            {
                // Stop previous fade effect
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);

                // Start new fade effect
                fadeCoroutine = StartCoroutine(FadeTransition(fullText));
            }
            else
            {
                dialogueText.text = fullText;
            }

            Debug.Log($"Displaying: {fullText}");
        }
        else
        {
            Debug.LogError("DialogueText is null!");
        }
    }

    IEnumerator DialogueCoroutine()
    {
        // Wait for audio clip duration
        yield return new WaitForSeconds(audioSource.clip.length);

        // Handle loop
        if (loopDialogue)
        {
            StartDialogue();
        }
        else
        {
            StopDialogue();
        }
    }

    // Public methods for external control
    public void PlayDialogue()
    {
        StartDialogue();
    }

    public void PauseDialogue()
    {
        StopDialogue();
    }

    public void SetDialogueSegments(DialogueSegment[] newSegments)
    {
        dialogueSegments = newSegments;
    }

    // Method to manually set current dialogue segment
    public void SetCurrentSegment(int segmentIndex)
    {
        if (segmentIndex >= 0 && segmentIndex < dialogueSegments.Length)
        {
            currentSegmentIndex = segmentIndex;
            DisplayDialogue(dialogueSegments[segmentIndex]);
        }
    }

    // Fade transition coroutine
    IEnumerator FadeTransition(string newText)
    {
        // Fade out current text
        Color originalColor = dialogueText.color;
        float alpha = originalColor.a;

        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Change text
        dialogueText.text = newText;

        // Fade in new text
        while (alpha < originalColor.a)
        {
            alpha += Time.deltaTime / fadeSpeed;
            dialogueText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure final color is correct
        dialogueText.color = originalColor;
    }
    // ICheckPeopleEvent 인터페이스 구현
    public void OnBothSelected()
    {
        print("OnBothSelected 실행");
        // 네트워크 전체에 대화 실행
        PlayDialogueNetwork();
    }

    // 이미 구현된 네트워크 동기화 함수
    public void PlayDialogueNetwork()
    {
        print("PlayDialogueNetwork 실행");
        photonView.RPC(nameof(RPC_StartDialogue), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_StartDialogue()
    {
        StartDialogue();
    }
}