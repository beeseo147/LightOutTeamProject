using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationUI : MonoBehaviour
{
    // 대화 내용을 저장할 리스트
    [SerializeField] private List<string> conversationList = new List<string>();

    // 대화 내용을 표시할 Text UI (TextMeshPro 사용 시 TMP_Text로 변경)
    [SerializeField] private Text conversationText;

    // 현재 대화 인덱스
    private int currentIndex = 0;

    // 대화창을 호출해서 보여주는 함수
    public void ShowConversation(int index)
    {
        if (index >= 0 && index < conversationList.Count)
        {
            conversationText.text = conversationList[index];
            currentIndex = index;
            // 필요하다면 대화창 활성화
            gameObject.SetActive(true);
        }
    }

    // 다음 대화로 넘기는 함수 (옵션)
    public void ShowNextConversation()
    {
        if (currentIndex + 1 < conversationList.Count)
        {
            ShowConversation(currentIndex + 1);
        }
    }
}
