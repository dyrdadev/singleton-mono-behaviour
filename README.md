# Singleton Mono Behaviour

> Another Singleton ```MonoBehaviour``` implementation for Unity. 

> 🧪 **EXPERIMENTAL** This project is experimental. It is still under development. It may be unstable. It is not optimized and largely untested . Do **not** use this project in critical projects. 

This package for Unity provides an implementation of the Singleton pattern for ```MonoBehaviour``` instances. The package introduces the ```SingletonMonoBehaviour``` class. With this class, we can create concrete ```MonoBehaviour``` components that can be added to game objects in the scene and behave in the same way as classic ```MonoBehaviour``` components – just as a singleton.

The class implements the most common features described for ```MonoBehaviour``` singletons, such as persistence, handling of multiple instances and performance optimizations. Check out the [Features](#features) section for a complete overview of the features.

> 💭 **A short comment of Daniel on the Singleton pattern:** At this point we do not want to enter the discussion about whether the Singleton pattern is good or bad. Whether it is more good than bad, or vice versa, depends on the concrete use case. This should be evaluated on a case-by-case basis. So don't get discouraged if somebody says that Singleton is an antipattern. In this radical statement, I do not think this is true. I know many situations in which a Singleton offers a beautiful solution. At the same time, however, it is true that one can easily drift into a suboptimal code structure. So use the pattern very consciously and be aware of the consequences. Make yourself fully aware of what you are doing. If you are unsure, a singleton will probably lead to suboptimal code. If you are looking for a good alternative to Singletons, **"Dependency Injection"** seems to be a good approach. There are some solutions of "Dependency Injection" available for Unity.

If you want to read more about the singleton pattern, check out [Game Programming Patterns - Singleton](http://gameprogrammingpatterns.com/singleton.html) by Robert Nystrom.

## Quick Start

Install the package as described [below](#install-the-package). Now you may find yourself in the following situation: You have a class that currently inherits from ```MonoBehaviour``` and you want it to be a Singleton. Then follow these steps:

1. **Preparation:** Open the script of the class which should be a singleton. To use the class ```SingletonMonoBehaviour``` in your script, you have to include the "DyrdaDev.Singleton" using directive. At the beginning of your script, insert the following line: ```using DyrdaDev.Singleton;```.
2. **Implementation of the concrete Singleton:** Change the superclass from which the class in question inherits from ```MonoBehaviour``` to ```SingletonMonoBehaviour<T>```. ```T``` is the type of your original class in question that will be the concrete ```SingletonMonoBehaviour```.
3. **Accessing the Singleton from everywhere:** Done. Now you can access the Singleton via the ```Instance``` property of the concrete Singleton class.

**Example:** Here is an example for a ```GameData``` class with a ```Score``` property. The ```GameData``` class is supposed to be a singleton.  The result of the steps as described above looks like this:

```C#
using UnityEngine;
using DyrdaDev.Singleton;

public class GameData : SingletonMonoBehaviour<GameData>
{
   public int Score;
   
   // ...
}
```

Now, you can access the ```Score``` property via ```GameData.Instance.Score``` from any script in your project that has access to the ```GameData``` class.

## Features

Our Singleton implementation offers the following features:

#### Event Functions
```SingletonMonoBehaviour``` inherits from ```MonoBehaviour```. This means, we work with concrete ```MonoBehaviour``` components on objects in the scene – just as a singleton. As consequence of the inheritance of   ```MonoBehaviour```, the derived classes of ```SingletonMonoBehaviour``` have access to all Unity event functions such as ```Awake```, ```Start```, ```Update``` or ```FixedUpdate```. Please note: ```Awake``` and ```OnDestroy``` are used by the base singleton class. When using these classes, use the ```overwrite``` keyword and make sure to call the base method. For example:

```C#
using System;
using UnityEngine;
using DyrdaDev.Singleton;

public class SingletonTest : SingletonMonoBehaviour<SingletonTest>
{
    protected override void Awake()
    {
        base.Awake();
        // ...
    }
}
```

#### Persistence
We work with real ```MonoBehaviour``` components and ```GameObjects``` – in other words, with components and objects in the scene. Singletons should usually be persistent for the runtime of an application, even between scene loads. Our code will take care of this for you if you like. You can control the behavior with the ```PersistOnSceneLoad``` property. If ```PersistOnSceneLoad``` is true, we do not destroy the instance when loading scenes with the ```DontDestroyOnLoad``` functionality. Our implementation also takes care of some preparations for this. The default value for ```PersistOnSceneLoad``` is ```true```. You can set the static field ``PersistOnSceneLoad`` in the implementation of your concrete ``SingletonMonoBehaviour`` class so that everything behaves according to your requirements.

#### Exactly one Singleton, not more
We make sure that there is always only one instance of the singleton in the scene. Further instances of a singleton are automatically destroyed.

#### Exactly one Singleton, not less
There should always be an instance of a Singleton class. Our implementation can automatically create one for you. The ```CreateInstanceIfNotPresent``` property states whether we create a new object if there is no instance in the scene. The defaut value for ```CreateInstanceIfNotPresent``` is ```true```. You can set the static field ```CreateInstanceIfNotPresent``` in the implementation of your concrete ```SingletonMonoBehaviour``` class so that everything behaves according to your requirements.

#### Inactive Singletons
When looking for instances in the scene, we can include inactive objects or ignore them. If the ```ConsiderInactiveInstances``` property is ```true```, we also consider inactive instances in the scene when we search for an instance. The default value for ```ConsiderInactiveInstances``` is ```false```. You can set the static field ```ConsiderInactiveInstances``` in the implementation of your concrete ```SingletonMonoBehaviour``` class so that everything behaves according to your requirements.

#### Thread Safety
This implementation is (/ should be) thread-safe. It uses a lock object for instance access and implements some other optimizations. Please note: There are other solutions out there with different approaches that may better suit your needs. (Compare for example [this article by Jon Skeet](https://csharpindepth.com/Articles/Singleton)) Also note that accessing Unity's internal features from parallel threads can lead to errors and inconsistencies. You should be clear about what you are doing if you want to work with threads and ```MonoBehaviour```.

#### Loading on Demand
With the ```Singleton-MonoBehaviour``` we work with ```GameObjects``` in the scene. So we have to find the one instance that is our current singleton instance. With the property ```LoadOnDemand``` you can decide whether we should find the correct instance at ```Awake``` or later on demand when a script tries to access the instance. If ```LoadOnDemand``` is true, we wait to find the instance until a script tries to access the instance. The default value for ```LoadOnDemand``` is ```false```. You can set the static field ```CreateInstanceIfNotPresent``` in the implementation of your concrete ```SingletonMonoBehaviour``` class so that everything behaves according to your requirements. Keep in mind that initialization requires resources and may result in some frame drops if the "onDemand" scenario occurs during performance-critical situations. 

#### Performance
Every access to the current instance requires some checks. These checks are a bottleneck because they are executed every time any piece of code accesses the singleton. Our implementation uses a flag to check if an instance already exists. This is better performing than a common approach used in many implementations based on the ```==``` operators. This is the case because Unity overloads the ```==``` operator. Unity's overloaded ```==``` operator is quite slow. You can read more about this topic in [this Unity blog article](http://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/).

#### Logging
The solution outputs debug logs whenever something "interesting" happens. The "OnDestroy" warning can be muted by setting the ```MuteOnDestroyWarning``` property to true.


## Install the Package

You can install this package with unity's [package manager](https://docs.unity3d.com/Manual/PackagesList.html). Simply add a new package with the git-HTTPS URL to the version you want to install in the form ```https://github.com/DyrdaDev/singleton-mono-behaviour.git#{version}```, where ```{version}``` is the actual version of the release you want to install. For example, if you want to install version ```0.0.7``` of this package, you can refer to ```https://github.com/DyrdaDev/singleton-mono-behaviour.git#0.0.7```.

You can do this by using the Package Manager window or the manifest.json directly:

1. **Installing from a Git URL using the Package Manager window.** Open the Package Manager window. Click "+", then "Add package from git URL" and enter the git URL from above. You can find more information [here](https://docs.unity3d.com/Manual/upm-ui-giturl.html).
2. **Installing from a Git URL using the manifest.json.** You can add a new entry to the manifest.json file in the ``Packages`` folder of your unity project: ```"dev.dyrda.singleton.singleton-mono-behaviour": "https://github.com/dyrdadev/singleton-mono-behaviour.git#upm"```. You can find more information [here](https://docs.unity3d.com/Manual/upm-git.html).

## License

This package is licensed under a MIT license. See the [LICENSE](/LICENSE.md) file for details. 

## Contribute

This project was created by [Daniel Dyrda](https://dyrda.digital).

> Daniel: _If you want to support me and my projects, you can follow me on [github (DyrdaDev)](https://github.com/DyrdaDev) and [twitter (@daniel_dyrda)](https://twitter.com/daniel_dyrda). Just come by and say hello, I would love to hear how you use the project._

If you want to contribute to this project, you are welcome to do so. Just write me and we will find a way to collaborate.
