using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;

// Photon 네트워크를 사용하여 두 플레이어가 서로 동시에 오브젝트를 들고 있는지 여부를 확인하는 스크립트
// 두 플레이어가 해당 오브젝트를 동시에 들고있다면 DialogueFirst 스크립트를 통해 대화를 시작할 수 있도록 함
public class CheckPeopleManager : MonoBehaviourPunCallbacks
{
    public static CheckPeopleManager Instance;

    // 오브젝트별 상태 및 참조
    private Dictionary<string, bool> selectedDict = new();
    private Dictionary<string, CheckPeople> peopleDict = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 모든 CheckPeople 등록
        foreach (var cp in FindObjectsOfType<CheckPeople>())
        {
            peopleDict[cp.objectKey] = cp;
            selectedDict[cp.objectKey] = false;
        }
    }

    public void SetObjectSelected(string objectKey, bool selected)
    {
        selectedDict[objectKey] = selected;

        // 네트워크 Custom Properties로 동기화
        var props = new ExitGames.Client.Photon.Hashtable();
        props[objectKey] = selected;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);

        // objectKey 그룹만 체크
        CheckGroupSelectedAndInvoke(objectKey);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach (var key in propertiesThatChanged.Keys)
        {
            string objKey = key as string;
            if (objKey != null && selectedDict.ContainsKey(objKey))
                selectedDict[objKey] = (bool)propertiesThatChanged[objKey];
        }
        CheckBothSelectedAndInvoke();
    }

    private void CheckBothSelectedAndInvoke()
    {
        // 모든 오브젝트가 잡힌 상태인지 체크
        bool allSelected = true;
        foreach (var sel in selectedDict.Values)
            if (!sel) allSelected = false;

        if (allSelected)
        {
            // 각 오브젝트의 ICheckPeopleEvent 실행
            foreach (var cp in peopleDict.Values)
            {
                cp.eventHandler?.OnBothSelected();
            }
        }
    }

    private void CheckGroupSelectedAndInvoke(string groupKey)
    {
        print($"{groupKey} isSelected: {selectedDict[groupKey]}");
        // groupKey에 해당하는 오브젝트들만 체크
        bool allSelected = true;
        foreach (var cp in peopleDict.Values)
        {
            if (cp.objectKey == groupKey && !selectedDict[cp.objectKey])
                allSelected = false;
        }

        if (allSelected)
        {
            foreach (var cp in peopleDict.Values)
            {
                if (cp.objectKey == groupKey)
                    cp.eventHandler?.OnBothSelected();
                print("OnBothSelected 실행");
            }
        }
    }
}
