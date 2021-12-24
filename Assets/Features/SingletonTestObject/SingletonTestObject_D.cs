using System;
using UnityEngine;
using DyrdaDev.Singleton;

public class SingletonTestObject_D : SingletonMonoBehaviour<SingletonTestObject_D>
{
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("Inside overwritten Awake.");
    }
}