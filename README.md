# Singleton Mono Behaviour

> Another Singleton ```MonoBehaviour``` implementation for Unity. 

> 🧪 **EXPERIMENTAL** This project is experimental. It is still under development. It may be unstable. It is not optimized and largely untested . Do **not** use this project in critical projects. 

This package for Unity provides an implementation of the Singleton pattern for ```MonoBehaviour``` instances: the ```SingletonMonoBehaviour``` class. This means we work with concrete ```MonoBehaviour``` components on objects in the scene – as a singleton. 

> 💭 **A short comment of Daniel on the Singleton pattern:** At this point we do not want to enter the discussion about whether the Singleton pattern is good or bad. Probably the answer will be: neither, anyway. It lies somewhere in between. Whether it is more good than bad, or vice versa, depends on the concrete use case. This should be evaluated by yourself on a case-by-case basis. So don't get discouraged if somebody says that Singleton is an antipattern. In this radical statement, I do not think this is true. I know many situations in which a Singleton offers a beautiful solution. At the same time, however, it is true that one can easily drift into a suboptimal code structure. So use the pattern very consciously and be aware of the consequences. Make yourself fully aware of what you are doing. If you are unsure, a singleton will probably lead to suboptimal code. If you are looking for a good alternative to Singletons, **"Dependency Injection"** seems to be a good approach. There are some solution approaches of "Dependency Injection" available for Unity.

## Quick Start

You have a class that currently inherits from ```MonoBehaviour``` and you want it to be a Singleton, then just follow these steps:

1. **Preparation:** Include the using directive to the ```SingletonMonoBehaviour``` class in your script: ```using DyrdaIo.Singleton;```.
2. **Implementation of the concrete Singleton:** Let the class in question inherit from ```SingletonMonoBehaviour<T>``` instead of ```MonoBehaviour```. ```T``` is the type of your original class in question that will be the concrete ```SingletonMonoBehaviour```.
3. **Accessing the Singleton from everywhere:** Done. Now you can access the Singleton via the ```Instance``` member of the concrete Singleton class.

**Example:** Here is an example for a ```GameData``` class with a ```score``` member as ```SingletonMonoBehaviour```.
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

## Features

Our Singleton implementation offers the following features:

#### Event Functions
```SingletonMonoBehaviour``` inherits from ```MonoBehaviour```. This menas, we work with concrete ```MonoBehaviour``` components on objects in the scene – just as a singleton. As consequence of the inheritance of   ```MonoBehaviour```, the derived classes of ```SingletonMonoBehaviour``` have access to all Unity event functions such as ```Awake```, ```Start```, ```Update``` or ```FixedUpdate```.
#### Performance
Every access to the current instance requires some checks. These checks are a bottleneck because they are executed every time any piece of code accesses the singleton. Our implementation uses a flag to check if an instance already exists. This is better performing than a common approach used in many implementations based on the == operators. (Because Unity overloads the ==-operator and it is quite slow).
#### Thread Safety
This implementation is (/ should be) thread-safe. It uses a lock object for instance access and implements some other optimizations.
#### Persistence
We work with real ```MonoBehaviour``` components and ```GameObjects``` – in other words with components and objects in the scene. Singletons should usually be persistent for the runtime of an application, even between scene loads. Our code will take care of this for you if you like. You can control the behavior with the ```peristOnLoad``` property. If p```ersistOnSceneLoad``` is true, we do not destroy the instance when loading scenes with the ```DontDestroyOnLoad``` functionality. Our implementation also takes care of some preparations for this. The default value for ```persistOnSceneLoad``` is ```true```. You can set the static field ``persistOnSceneLoad`` in the implementation of your concrete ``SingletonMonoBehaviour`` class so that everything behaves according to your requirements.
#### Exactly one Singleton, not more
We make sure that there is always only one instance of the singleton in the scene. Further instances of a singleton are automatically destroyed.
#### Exactly one Singleton, not less
There should always be an instance of a Singleton class. Our implementation can autoamtically create one for you. The ```createInstanceIfNotPresent``` property states whether we create a new object if there is no instance in the scene. The defaut value for ```createInstanceIfNotPresent``` is ```true```. You can set the static field ```createInstanceIfNotPresent``` in the implementation of your concrete ```SingletonMonoBehaviour``` class so that everything behaves according to your requirements.
#### Inactive Singletons
When looking for instances in the scene, we can include inactive objects or ignore them. If the ```considerInactive``` property is ```true```, we also consider inactive elements in the scene when we search for an instance. The default value for ```considerInactive``` is ```false```. You can set the static field ```considerInactive``` in the implementation of your concrete ```SingletonMonoBehaviour``` class so that everything behaves according to your requirements.
#### Lazy load
With the ```Singleton-MonoBehaviour``` we work with ```GameObjects``` in the scene. So we have to find the one instance that is our current singleton instance. With the property ```loadLazy``` you can decide whether we should find the correct instance at ```Awake``` or later on demand when a script tries to access the instance. If ```loadLazy``` is true, we wait to find the instance until a script tries to access the instance. The default value for ```loadLazy``` is ```false```. You can set the static field ```createInstanceIfNotPresent``` in the implementation of your concrete ```SingletonMonoBehaviour``` class so that everything behaves according to your requirements.

## Install the Package

You can install this package with unity's [package manager](https://docs.unity3d.com/Manual/PackagesList.html). Simply add a new package with the git-HTTPS URL to the version you want to install in the form "https://github.com/dyrdaio/singleton-mono-behaviour.git#{version}", where {version} is the actual version of the release you want to install. For example, if you want to install version "0.0.2" of this package, you can refer to https://github.com/dyrdaio/singleton-mono-behaviour.git#0.0.2.

You can do this by using the Package Manager window or the manifest.json directly:

1. **Installing from a Git URL using the Package Manager window.** Open the Package Manager window. Click "+", then "Add package from git URL" and enter the git URL from above. You can find more information [here](https://docs.unity3d.com/Manual/upm-ui-giturl.html).
2. **Installing from a Git URL using the manifest.json.** You can add a new entry to the manifest.json file in the ``Packages`` folder of your unity project: ```"io.dyrda.singleton.singleton-mono-behaviour": "https://github.com/dyrdaio/singleton-mono-behaviour.git#upm"```. You can find more information [here](https://docs.unity3d.com/Manual/upm-git.html).

## License

This package is licensed under a MIT license. See the [LICENSE](/LICENSE) file for details.

## Support

This project was created by [Daniel Dyrda](https://dyrda.io). If you want to support me and my projects, you can follow me on [github (dyrdaio)](https://github.com/dyrdaio) and [twitter (@dyrdaio)](https://twitter.com/dyrdaio). Just come by and say hello, I would love to hear how you use the project.

## Contribute

This project was developed by [Daniel Dyrda](https://dyrda.io). If you want to contribute to this project, you are welcome to do so. Just write me and we will find a way to collaborate.
