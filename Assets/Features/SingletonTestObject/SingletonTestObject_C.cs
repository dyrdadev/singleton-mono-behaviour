using System;
using DyrdaDev.Singleton;
using UnityEngine;

public class SingletonTestObject_C : SingletonMonoBehaviour<SingletonTestObject_C>
{
    SingletonTestObject_C()
    {
        SingletonTestObject_C.PersistOnSceneLoad = false;
    }
}