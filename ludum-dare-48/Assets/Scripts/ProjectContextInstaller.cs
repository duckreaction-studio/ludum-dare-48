using DuckReaction.Audio;
using DuckReaction.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("Install project");
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameEvent>();
        Container.Bind<AudioService>().FromComponentInNewPrefabResource("MainAudioService").AsSingle().NonLazy();
    }
}
