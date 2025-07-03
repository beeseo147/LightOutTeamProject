using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyModel : MonoBehaviour
{
    public event System.Action<bool> OnGameStartAvailable;
    public event System.Action<string> OnPlayerNameChanged;
    
    private bool isGameStartAvailable;
    private string playerName;
    
    public bool IsGameStartAvailable 
    { 
        get => isGameStartAvailable; 
        set 
        { 
            isGameStartAvailable = value; 
            OnGameStartAvailable?.Invoke(value);
        } 
    }
    
    public string PlayerName 
    { 
        get => playerName; 
        set 
        { 
            playerName = value; 
            OnPlayerNameChanged?.Invoke(value);
        } 
    }
}
