using System;
using DyrdaDev.Singleton;
using UnityEngine;

public class SingletonTestObject_B : SingletonMonoBehaviour<SingletonTestObject_B>
{
    private void Start()
    {
        Debug.Log("Testing the other singelton class: " + SingletonTestObject.Instance.name);
    }
}