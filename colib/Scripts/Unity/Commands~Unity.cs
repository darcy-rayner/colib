using System;
using UnityEngine;

namespace CoLib 
{

public static partial class Cmd
{
    public static CommandDelegate Log(string text)
    {
        return Cmd.Do( () => Debug.Log(text) );
    }

    public static CommandDelegate LogError(string text)
    {
        return Cmd.Do (() => Debug.LogError(text));
    }

    public static CommandDelegate LogWarning(string text)
    {
        return Cmd.Do (() => Debug.LogWarning (text));
    }

    public static CommandDelegate LogException(Exception e)
    {
        return Cmd.Do (() => Debug.LogException (e));
    }

    public static CommandDelegate Enable(MonoBehaviour behaviour, bool isEnabled = true)
    {
        return Cmd.Do (() => behaviour.enabled = isEnabled);
    }

    public static CommandDelegate SetActive(GameObject gm, bool isActive)
    {
        return Cmd.Do (() => gm.SetActive (isActive));
    }

    public static CommandDelegate SendMessage(GameObject gm, string eventName, object obj  = null, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
    {
        return Cmd.Do( () => gm.SendMessage (eventName, obj, options));
    }

}

}

