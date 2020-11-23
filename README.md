# Singleton Mono Behaviour

Another Singleton MonoBehaviour implementation for Unity. 

> 🧪 **EXPERIMENTAL** This project is experimental. It is still under development. It may be unstable. It is not optimized and largely untested . Do **not** use this project in critical projects. 

## Introduction

This package provides an implementation of a singleton MonoBehaviour. Thus, it is related to concrete component instances on GameObjects in the scene. As MonoBehaviour, the derived classes have access to all Unity lifecycle methods.

## Features

## Quick Start

So you have a class that currently inherits from ```MonoBehaviour``` and you want it to be a Singleton, then just follow these steps:

1. Include the using directive to the ```SingletonMonoBehaviour``` class: ```using DyrdaIo.Singleton;```.
2. Let the class in question inherit from ```SingletonMonoBehaviour<T>``` instead of MonoBehaviour. ```T``` is the type of your concrete class that will be the concrete ```SingletonMonoBehaviour```.
3. Done. Now you can access the Singleton via the ```Instance``` member of the concrete Singleton class.

### GameData Example

Here is an example for a ```GameData``` class with a ```score``` member as SingeltonMonoBehaviour.
The preparation and implementation of the Singleton MonoBehaviour:

```
using UnityEngine;
using DyrdaIo.Singleton;

public class GameData : SingletonMonoBehaviour<GameData>
{
   public int score;
   
   // Remaining implementation of the class.
}
```

Now, you can access the ```score``` member as follows:
```
GameData.Instance.score
```


## Install the package

You can install this package with unity's [package manager](https://docs.unity3d.com/Manual/PackagesList.html). Just add a new package with the git HTTPS URL to the "UPM" branch of this repository: https://github.com/Zughiko/singleton-mono-behaviour.git#upm

You can do this by using the Package Manager window or the manifest.json directly:

1. **Installing from a Git URL using the Package Manager window.** Open the Package Manager window. Click "+", then "Add package from git URL" and enter the git URL from above. You can find more information [here](https://docs.unity3d.com/Manual/upm-ui-giturl.html).
2. **Installing from a Git URL using the manifest.json.** You can add a new entry to the manifest.json file in the ``Packages`` folder of your unity project: ```"io.dyrda.singleton.singleton-mono-behaviour": "https://github.com/Zughiko/singleton-mono-behaviour.git#upm"```. You can find more information [here](https://docs.unity3d.com/Manual/upm-git.html).
 
