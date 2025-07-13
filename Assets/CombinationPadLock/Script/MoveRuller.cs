// Script by Marcelli Michele

using System.Collections.Generic;
using UnityEngine;

public class MoveRuller : MonoBehaviour
{
    PadLockPassword _lockPassword;
    PadLockEmissionColor _pLockColor;

    [HideInInspector]
    //룰러 오브젝트 리스트
    public List <GameObject> _rullers = new List<GameObject>();
    private int _scroolRuller = 0;
    private int _changeRuller = 0;
    //정답 배열
    [HideInInspector]
    public int[] _numberArray = {0,0,0,0};

    private int _numberRuller = 0;

    //룰러를 돌릴 때마다 활성화되는 이펙션 여부
    private bool _isActveEmission = false;
    private bool bIsCorrect = false;

    void Awake()
    {
        _lockPassword = GetComponent<PadLockPassword>();
        _pLockColor = GetComponent<PadLockEmissionColor>();

        _rullers.Add(GameObject.Find("Ruller1"));
        _rullers.Add(GameObject.Find("Ruller2"));
        _rullers.Add(GameObject.Find("Ruller3"));
        _rullers.Add(GameObject.Find("Ruller4"));

        foreach (GameObject r in _rullers)
        {
            //r.transform.Rotate(-144, 0, 0, Space.Self);
        }
        // 상태 동기화
    
    }
    void Update()
    {
        //MoveRulles();
        //RotateRullers();
        //print("numberArray: " + _numberArray[0] + _numberArray[1] + _numberArray[2] + _numberArray[3]);
        if(!bIsCorrect)
        {
            _lockPassword.Password();
        }
    }

    void MoveRulles()
    {
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            _isActveEmission = true;
            _changeRuller ++;
            _numberRuller += 1;

            if (_numberRuller > 3)
            {
                _numberRuller = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            _isActveEmission = true;
            _changeRuller --;
            _numberRuller -= 1;

            if (_numberRuller < 0)
            {
                _numberRuller = 3;
            }
        }
        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;


        for (int i = 0; i < _rullers.Count; i++)
        {
            if (_isActveEmission)
            {
                if (_changeRuller == i)
                {

                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = true;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
                else
                {
                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
            }
        }

    }

    void RotateRullers()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(-_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] += 1;

            if (_numberArray[_changeRuller] > 9)
            {
                _numberArray[_changeRuller] = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] -= 1;

            if (_numberArray[_changeRuller] < 0)
            {
                _numberArray[_changeRuller] = 9;
            }
        }
    }

    // 외부에서 호출할 수 있도록 public 메서드로 변경
    public void SelectNextRuller()
    {
        _isActveEmission = true;
        _changeRuller++;
        _numberRuller++;
        if (_numberRuller > 3) _numberRuller = 0;
        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;

        UpdateRullerEmission();
    }

    public void SelectPrevRuller()
    {
        _isActveEmission = true;
        _changeRuller--;
        _numberRuller--;
        if (_numberRuller < 0) _numberRuller = 3;
        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;
        UpdateRullerEmission();
    }

    public void IncreaseCurrentRuller()
    {
        _isActveEmission = true;
        _scroolRuller = 36;
        _rullers[_changeRuller].transform.Rotate(-_scroolRuller, 0, 0, Space.Self);
        _numberArray[_changeRuller]++;
        if (_numberArray[_changeRuller] > 9) _numberArray[_changeRuller] = 0;
    }

    public void DecreaseCurrentRuller()
    {
        _isActveEmission = true;
        _scroolRuller = 36;
        _rullers[_changeRuller].transform.Rotate(_scroolRuller, 0, 0, Space.Self);
        _numberArray[_changeRuller]--;
        if (_numberArray[_changeRuller] < 0) _numberArray[_changeRuller] = 9;
    }

    // 이펙트 갱신 함수 분리
    void UpdateRullerEmission()
    {
        for (int i = 0; i < _rullers.Count; i++)
        {
            var emission = _rullers[i].GetComponent<PadLockEmissionColor>();
            emission._isSelect = (_changeRuller == i);
            emission.BlinkingMaterial();
        }
    }
}
