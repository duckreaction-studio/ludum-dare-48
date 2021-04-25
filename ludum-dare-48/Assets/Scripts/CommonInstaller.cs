using ModestTree.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CommonInstaller : MonoInstaller
{
    [Preserve]
    public override void InstallBindings()
    {
        Debug.Log("Install common");
        Container.Bind<SceneService>().FromComponentInHierarchy(false).AsSingle();
    }
}
