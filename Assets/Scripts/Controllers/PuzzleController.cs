using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPuzzle
{
    bool IsActive   { get; }
    bool IsCleared  { get; }
    event UnityAction<IPuzzle> OnCleared;
}

public class PuzzleController : MonoBehaviour, IPuzzle
{
    [Tooltip("Clear Conditions List")]
    [SerializeField] List<MonoBehaviour> conditions;

    public bool IsActive => true;
    public bool IsCleared { get; private set; }
    public event UnityAction<IPuzzle> OnCleared;

    void Update()
    {
        if (IsCleared)
            return;

        foreach (var c in conditions)
            if (c is ICondition cond && !cond.IsSolved)
                return;

        Clear();
    }

    void Clear()
    {
        IsCleared = true;
        OnCleared?.Invoke(this);   // 구독자 호출
    }
}

public interface ICondition { bool IsSolved { get; } }
