    // Script by Marcelli Michele

using System.Linq;
using UnityEngine;
using Photon.Pun;
public class PadLockPassword : MonoBehaviourPun
{
    MoveRuller _moveRull;
    public static event System.Action OnPasswordSuccess;
    public int[] _numberPassword = {0,0,0,0};

    private void Awake()
    {
        _moveRull = GetComponent<MoveRuller>();
        if(_moveRull == null)
        {
            Debug.LogError("MoveRuller component not found on this object");
        }
    }

    public void Password()
    {
        // 현재 패스워드 값이 _numberPassword와 일치하는지 확인
        if (_moveRull._numberArray.SequenceEqual(_numberPassword))
        {
            print($"GameObject : {gameObject.name} Password correct");
            PadLockOpen padLockOpen = GetComponent<PadLockOpen>();
            padLockOpen.OpenLock();
            // Here enter the event for the correct combination
            //Debug.Log("Password correct");
            // Es. Below the for loop to disable Blinking Material after the correct password
            for (int i = 0; i < _moveRull._rullers.Count; i++)
            {
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
            }
            GetComponent<AudioSource>().Play();
            OnPasswordSuccess?.Invoke();
        }
        
    }
    
}
