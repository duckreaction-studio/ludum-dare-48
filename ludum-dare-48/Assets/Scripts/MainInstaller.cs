using Core;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    GameObject _catPrefab;

    public override void InstallBindings()
    {
        Container.Bind<CatSpawner>().FromComponentInHierarchy().AsSingle();
        Container.Bind<CatBowl>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameState>().FromComponentInHierarchy().AsSingle();

        Container.BindFactory<int, Vector3, Cat, Cat.Factory>().FromPoolableMemoryPool<int, Vector3, Cat, Cat.Pool>(
    x => x.WithInitialSize(20).FromComponentInNewPrefab(_catPrefab).UnderTransformGroup("CatPool")
);
    }
}