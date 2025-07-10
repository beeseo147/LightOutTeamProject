    // Script by Marcelli Michele

using System.Linq;
using UnityEngine;

public class PadLockPassword : MonoBehaviour
{
    MoveRuller _moveRull;

    public int[] _numberPassword = {0,0,0,0};

    private void Awake()
    {
        _moveRull = FindFirstObjectByType<MoveRuller>();
    }

    public void Password()
    {
        // 현재 패스워드 값이 _numberPassword와 일치하는지 확인
        if (_moveRull._numberArray.SequenceEqual(_numberPassword))
        {
            PadLockOpen padLockOpen = FindFirstObjectByType<PadLockOpen>();
            padLockOpen.OpenLock();
            // Here enter the event for the correct combination
            Debug.Log("Password correct");
            // Es. Below the for loop to disable Blinking Material after the correct password
            for (int i = 0; i < _moveRull._rullers.Count; i++)
            {
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
            }

        }
    }
}
