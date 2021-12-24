#region

using UnityEngine;

#endregion

namespace DyrdaDev
{
    namespace Singleton
    {
        [DisallowMultipleComponent]
        public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
        {
            #region Configuration

            /// <summary>
            /// Should we wait with finding the correct instance until a script tries to access it? If set to false,
            /// we load the instance on Awake.
            /// </summary>
            public static bool LoadLazy = false;

            /// <summary>
            /// If true, we create a new object if there is no instance in the scene.
            /// </summary>
            public static bool CreateInstanceIfNotPresent = true;

            /// <summary>
            /// If true, we also consider inactive elements in the scene when we search for an instance.
            /// </summary>
            public static bool ConsiderInactive = false;

            /// <summary>
            /// If true, we do not destroy the instance on scene loads.
            /// </summary>
            public static bool PersistOnSceneLoad = true;

            #endregion

            #region Internal

            private const string MessagePrefix = "<b>— SingletonMonoBehaviour:</b> ";
            private const string MessageSuffix = "\n";

            // THE instance.
            private static volatile T _instance = null;

            /// <summary>
            /// Our lock for THE instance.
            /// </summary>
            private static readonly object InstanceLock = new object();

            /// <summary>
            /// We use a flag for comparison to avoid the == operator. (It is slow because Unity overloads the operator.)
            /// </summary>
            private static bool _instantiated = false;

            /// <summary>
            /// The first instance reaching the lock on Awake sets it own id to this so it is set as current instance.
            /// </summary>
            private static int _preferredInstanceId = -1;

            /// <summary>
            /// Indicator that the instance is destroyed as it is the case on application quit.
            /// </summary>
            private static bool _instanceDestroyed = false;

            #endregion

            /// <summary>
            /// Log wrapper for all debug logs of this class.
            /// </summary>
            /// <param name="message">Message to be logged.</param>
            private static void LogMessage(string message)
            {
                Debug.LogFormat($"{MessagePrefix}{message}{MessageSuffix}");
            }

            /// <summary>
            /// Log Warn wrapper for all debug logs of this class.
            /// </summary>
            /// <param name="message">Message to be logged.</param>
            private static void WarnMessage(string message)
            {
                Debug.LogWarningFormat($"{MessagePrefix}{message}{MessageSuffix}");
            }

            /// <summary>
            /// The current instance of the singleton class.
            /// </summary>
            public static T Instance
            {
                get
                {
                    lock (InstanceLock)
                    {
                        if (_instantiated)
                        {
                            // We have an instance...

                            return _instance;
                        }
                        else if (_instanceDestroyed == false)
                        {
                            // We have no instance but we can initialize one...

                            InitializeInstance();
                            return _instance;
                        }
                        else
                        {
                            // We do not have an instance and we are not allowed to create one...

                            WarnMessage(
                                $"'{typeof(T)}' is destroyed. This may be because the application has already been " +
                                $"closed. Therefore no new object is created. Returning null.");
                            return null;
                        }
                    }
                }
                protected set
                {
                    _instance = value;

                    if (PersistOnSceneLoad)
                    {
                        // Prepare the persistence.
                        if (_instance.transform.parent != null)
                        {
                            LogMessage($"The instance {_instance.name} is not a root object. Persistence using " +
                                       $"DontDestroyOnLoad, however, is only possible for root objects. Updating " +
                                       $"the scene graph position of the instance.");
                            _instance.transform.SetParent(null);
                        }

                        // Make the instance persistent.
                        DontDestroyOnLoad(_instance.gameObject);
                    }

                    _instantiated = true;
                }
            }

            /// <summary>
            /// Initialize the instance.
            /// </summary>
            private static void InitializeInstance()
            {
                // Find all [active / active and inactive] instances in the scene.
                var objects = ConsiderInactive
                    ? Resources.FindObjectsOfTypeAll(typeof(T))
                    : FindObjectsOfType(typeof(T));

                if (objects == null || objects.Length < 1)
                {
                    // There is no instance in the scene...

                    var missingInstanceMessage =
                        $"An instance of '{typeof(T)}' is needed in the scene, but there is none. ";

                    if (CreateInstanceIfNotPresent)
                    {
                        Instance = new GameObject().AddComponent<T>();
                        Instance.name = $"{typeof(T)} – Autogenerated Singleton";

                        missingInstanceMessage +=
                            $"'{Instance.name}' was created{(PersistOnSceneLoad ? " as persistent Game Object." : ".")}";
                    }
                    else
                    {
                        missingInstanceMessage += "Due to the singleton settings no new instance is created.";
                    }

                    WarnMessage(missingInstanceMessage);
                }
                else if (objects.Length >= 1)
                {
                    // There is at least one instance in the scene...

                    // ... so we always take the preferred instance or the first one:
                    if (_preferredInstanceId != -1)
                    {
                        for (var i = 0; i < objects.Length; i++)
                        {
                            if ((objects[i] as T).GetInstanceID() == _preferredInstanceId)
                            {
                                Instance = objects[i] as T;
                            }
                        }
                    }
                    else
                    {
                        Instance = objects[0] as T;
                    }
                }
            }

            /// <summary>
            /// Destroy an instance.
            /// </summary>
            /// <param name="instance">Object that should be destroyed</param>
            private static void DestroyInstance(Object instance)
            {
                // Check if it is already deleted. (If true, the object is not null but == works because of
                // unity's overload of the operator.)
                if (instance == null == false)
                {
                    WarnMessage($"Deleting additional instance of '{typeof(T)}' attached to '{instance.name}'.");

                    Destroy(instance);
                }
            }

            private void Awake()
            {
                if (LoadLazy == false)
                {
                    lock (InstanceLock)
                    {
                        if (_instantiated == false)
                        {
                            // This is the current instance. We set the preferred instance to the id of this and trigger
                            // the default initialize instance behavior so we can make sure that every scenario actual
                            // executes the same code.
                            _preferredInstanceId = GetInstanceID();
                            var doSomethingToInitializeTheInstance = Instance.name;
                        }
                        else
                        {
                            if (_instance == this == false)
                            {
                                DestroyInstance(this);
                            }
                        }
                    }
                }
            }

            protected virtual void OnDestroy()

            {
                if (_instantiated && GetInstanceID() == _instance.GetInstanceID())
                {
                    // We are destroying the current instance...

                    // ... this should only be the case on application quit.
                    WarnMessage(
                        $"Deleting <b>current</b> instance of '{typeof(T)}' attached to '{_instance.name}'. This " +
                        $"should only be the case on application quit.");

                    // Prevent recreations.
                    _instanceDestroyed = true;
                }
            }
        }
    }
}
