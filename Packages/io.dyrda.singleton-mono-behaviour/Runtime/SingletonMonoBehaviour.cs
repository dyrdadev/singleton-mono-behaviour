using UnityEngine;

namespace DyrdaIo
{
    namespace Singleton
    {
        [DisallowMultipleComponent]
        public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
        {
            private const string messagePrefix = "<b>— SingletonMonoBehaviour:</b> ";
            private const string messageSuffix = "\n";

            // THE instance.
            private static volatile T instance;

            /// <summary>
            ///     Our lock for THE instance.
            /// </summary>
            private static readonly object instanceLock = new object();

            /// <summary>
            ///     Unity overloads the == operator and it is quite slow. So instead of (instance != null)
            ///     we use a flag for comparison. For further details see the following Unity blog article:
            ///     http://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
            /// </summary>
            private static bool instantiated;

            /// <summary>
            ///     The first instance reaching the lock on Awake sets it own id to this so it is set as current instance.
            /// </summary>
            private static int preferredInstanceId = -1;

            /// <summary>
            ///     Indicator that the instance is destroyed as it is the case on application quit.
            /// </summary>
            private static bool instanceDestroyed;

            /// <summary>
            ///     The current instance of the singleton class.
            /// </summary>
            public static T Instance
            {
                get
                {
                    lock (instanceLock)
                    {
                        if (instantiated)
                        {
                            return instance;
                        }

                        if (instanceDestroyed == false)
                        {
                            InitializeInstance();
                            return instance;
                        }
                        else
                        {
                            WarnMessage(
                                $"'{typeof(T)}' is destroyed. This may be because the application has already been closed. Therefore no new object is created. Returning null.");
                            return null;
                        }
                    }
                }
                private set
                {
                    instance = value;
                    instantiated = true;
                    if (persistOnSceneLoad)
                    {
                        // Prepare the persistence.
                        if (instance.transform.parent != null)
                        {
                            LogMessage(
                                $"The instance {instance.name} is not a root object. Persistence using DontDestroyOnLoad, however, is only possible for root objects. Updating the scene graph position of the instance.");
                            instance.transform.SetParent(null);
                        }

                        // Ensure a persistent instance.
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
            }

            /// <summary>
            ///     Log wrapper for all debug logs of this class.
            /// </summary>
            /// <param name="message"></param>
            private static void LogMessage(string message)
            {
                Debug.LogFormat($"{messagePrefix}{message}{messageSuffix}");

            }

            private static void WarnMessage(string message)
            {
                Debug.LogWarningFormat($"{messagePrefix}{message}{messageSuffix}");
            }

            /// <summary>
            ///     Destroy an instance.
            /// </summary>
            /// <param name="_instance"></param>
            private static void DestroyInstance(Object _instance)
            {
                // Check if it is already deleted. (Because currently, additional instances delete themselves
                // and are deleted by the current instance initialization If true, the object is not null
                // but == works because of unity's overload of the operator.
                if (_instance == null == false)
                {
                    WarnMessage($"Deleting additional instance of '{typeof(T)}' attached to '{_instance.name}'.");

                    Destroy(_instance);
                }
            }

            private static void InitializeInstance()
            {
                // Find all [active / active and inactive] instances in the scene.
                var objects = considerInactive
                    ? Resources.FindObjectsOfTypeAll(typeof(T))
                    : FindObjectsOfType(typeof(T));

                if (objects == null || objects.Length < 1)
                {
                    // There is no instance in the scene...

                    var missingInstanceMessage =
                        $"An instance of '{typeof(T)}' is needed in the scene, but there is none. ";

                    if (createInstanceIfNotPresent)
                    {
                        var singleton = new GameObject();
                        singleton.name = $"{typeof(T)} – Autogenerated Singleton";
                        Instance = singleton.AddComponent<T>();

                        missingInstanceMessage +=
                            $"'{singleton.name}' was created{(persistOnSceneLoad ? " as persistent Game Object." : ".")}";
                    }

                    missingInstanceMessage += "Due to the singleton settings no new instance is created.";
                    WarnMessage(missingInstanceMessage);
                }
                else if (objects.Length >= 1)
                {
                    // There is at least one instance in the scene...

                    // ... so we always take the preferred instance or the first one:
                    if (preferredInstanceId != -1)
                    {
                        for (var i = 0; i < objects.Length; i++)
                            if ((objects[i] as T).GetInstanceID() == preferredInstanceId)
                                Instance = objects[i] as T;
                    }
                    else
                    {
                        Instance = objects[0] as T;
                    }
                }
            }

            private void Awake()
            {
                if (loadLazy)
                    return;
                lock (instanceLock)
                {
                    if (instantiated == false)
                    {
                        preferredInstanceId = GetInstanceID();
                        var doSomethingToInitializeTheInstance = Instance.name;
                    }
                    else
                    {
                        if (instance == this == false) DestroyInstance(this);
                    }
                }
            }

            protected virtual void OnDestroy()
            {
                // Are we destroying the current instance?
                if (instantiated && GetInstanceID() == instance.GetInstanceID())
                {
                    //This should only be the case on application quit.
                    WarnMessage(
                        $"Deleting <b>current</b> instance of '{typeof(T)}' attached to '{instance.name}'. This should only be the case on application quit.");

                    // Prevent recreations.
                    instanceDestroyed = true;
                }
            }

            #region Settings

            /// <summary>
            ///     Should we wait with finding the correct instance until a script tries to access it? If set to false, we load the
            ///     instance on Awake.
            /// </summary>
            public static bool loadLazy = false;

            /// <summary>
            ///     If true, we create a new object if there is no instance in the scene.
            /// </summary>
            public static bool createInstanceIfNotPresent = true;

            /// <summary>
            ///     If true, we also consider inactive elements in the scene when we search for an instance.
            /// </summary>
            public static bool considerInactive = false;

            /// <summary>
            ///     If true, we do not destroy the instance on scene loads.
            /// </summary>
            public static bool persistOnSceneLoad = true;

            #endregion
        }
    }
}
