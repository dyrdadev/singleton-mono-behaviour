# Singleton Mono Behaviour

Another Singleton MonoBehaviour implementation for Unity. 

> 🧪 **EXPERIMENTAL** This project is experimental. It is still under development. It may be unstable. It is not optimized and largely untested . Do **not** use this project in critical projects. 

## Introduction

This package provides an implementation of the Singleton pattern for ```MonoBehaviour``` instances. Thus, we work with concrete ```MonoBehaviour``` component instances on ```GameObjects``` in the scene. As ```MonoBehaviour```, the derived classes have access to all Unity lifecycle methods.

## Features

- **Performance:** Every access to the current instance requires some checks. These checks are a bottleneck because they are executed every time any piece of code accesses the singleton. Our implementation uses a flag to check if an instance already exists. This is better performing than a common approach used in many implementations based on the == operators. (Because Unity overloads the ==-operator and it is quite slow).
- **Thread Safety:** This implementation is (/ should be) thread-safe. It uses a lock object for instance access and implements some other optimizations.
- **Persistence:** We work with real ```MonoBehaviour``` components and ```GameObjects``` – in other words with components and objects in the scene. Singletons should usually be persistent for the runtime of an application, even between scene loads. Our code will take care of this for you if you like. You can control the behavior with the ```peristOnLoad``` property. If p```ersistOnSceneLoad``` is true, we do not destroy the instance when loading scenes with the ```DontDestroyOnLoad``` functionality. Our implementation also takes care of some preparations for this. The default value for ```persistOnSceneLoad``` is ```true```. You can set the static field ``persistOnSceneLoad`` in the implementation of your concrete ``SingeltonMonoBehaviour`` class so that everything behaves according to your requirements.
- **Exactly one Singleton, not more.:** We make sure that there is always only one instance of the singleton in the scene. Further instances of a singleton are automatically destroyed.
- **Exactly one Singleton, not less:** There should always be an instance of a Singleton class. Our implementation can autoamtically create one for you. The ```createInstanceIfNotPresent``` property states whether we create a new object if there is no instance in the scene. The defaut value for ```createInstanceIfNotPresent``` is ```true```. You can set the static field ```createInstanceIfNotPresent``` in the implementation of your concrete ```SingeltonMonoBehaviour``` class so that everything behaves according to your requirements.
- **Inactive Singletons:** When looking for instances in the scene, we can include inactive objects or ignore them. If the ```considerInactive``` property is ```true```, we also consider inactive elements in the scene when we search for an instance. The default value for ```considerInactive``` is ```false```. You can set the static field ```considerInactive``` in the implementation of your concrete ```SingeltonMonoBehaviour``` class so that everything behaves according to your requirements.
- **Lazy load:** With the ```Singleton-MonoBehaviour``` we work with ```GameObjects``` in the scene. So we have to find the one instance that is our current singleton instance. With the property ```loadLazy``` you can decide whether we should find the correct instance at ```Awake``` or later on demand when a script tries to access the instance. If ```loadLazy``` is true, we wait to find the instance until a script tries to access the instance. The default value for ```loadLazy``` is ```false```. You can set the static field ```createInstanceIfNotPresent``` in the implementation of your concrete ```SingeltonMonoBehaviour``` class so that everything behaves according to your requirements.

## Quick Start

You have a class that currently inherits from ```MonoBehaviour``` and you want it to be a Singleton, then just follow these steps:

1. **Preparation:** Include the using directive to the ```SingletonMonoBehaviour``` class in your script: ```using DyrdaIo.Singleton;```.
2. **Implementation of the concrete Singelton:** Let the class in question inherit from ```SingletonMonoBehaviour<T>``` instead of ```MonoBehaviour```. ```T``` is the type of your original class in question that will be the concrete ```SingletonMonoBehaviour```.
3. **Accessing the Singleton from everywhere:** Done. Now you can access the Singleton via the ```Instance``` member of the concrete Singleton class.

### Example

Here is an example for a ```GameData``` class with a ```score``` member as SingeltonMonoBehaviour.
The preparation and implementation of a ```MonoBehaviour``` as Singleton:

```
using UnityEngine;
using DyrdaIo.Singleton;

public class GameData : SingletonMonoBehaviour<GameData>
{
   public int score;
   
   // Remaining implementation of the class.
}
```

Now, you can access the ```score``` member via ```GameData.Instance.score```


## Install the package

You can install this package with unity's [package manager](https://docs.unity3d.com/Manual/PackagesList.html). Just add a new package with the git HTTPS URL to the "UPM" branch of this repository: https://github.com/Zughiko/singleton-mono-behaviour.git#upm

You can do this by using the Package Manager window or the manifest.json directly:

1. **Installing from a Git URL using the Package Manager window.** Open the Package Manager window. Click "+", then "Add package from git URL" and enter the git URL from above. You can find more information [here](https://docs.unity3d.com/Manual/upm-ui-giturl.html).
2. **Installing from a Git URL using the manifest.json.** You can add a new entry to the manifest.json file in the ``Packages`` folder of your unity project: ```"io.dyrda.singleton.singleton-mono-behaviour": "https://github.com/Zughiko/singleton-mono-behaviour.git#upm"```. You can find more information [here](https://docs.unity3d.com/Manual/upm-git.html).
 
