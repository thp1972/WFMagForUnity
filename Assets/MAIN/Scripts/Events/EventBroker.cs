using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBroker 
{
    public static event Action<int> OnGetTotalPages;
    public static void TriggerOnGetTotalPages(int tp)
    {
        OnGetTotalPages?.Invoke(tp);
    }

    public static event Action<int> OnGetTotalNumbers;
    public static void TriggerOnGetTotalNumbers(int tn)
    {
        OnGetTotalNumbers?.Invoke(tn);
    }

    public static event Action<int> OnGoToPage;
    public static void TriggerOnGoToPage(int page)
    {
        OnGoToPage?.Invoke(page);
    }

    public static event Action<bool> OnSetCursor;
    public static void TriggerOnSetCursor(bool set)
    {
        OnSetCursor?.Invoke(set);
    }
}
