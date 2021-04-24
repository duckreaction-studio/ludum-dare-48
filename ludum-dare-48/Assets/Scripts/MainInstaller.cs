using Core;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField]
    GameObject _catPrefab;

    public override void InstallBindings()
    {
        Container.BindFactory<int, Vector3, Cat, Cat.Factory>().FromPoolableMemoryPool<int, Vector3, Cat, Cat.Pool>(
    x => x.WithInitialSize(20).FromComponentInNewPrefab(_catPrefab).UnderTransformGroup("CatPool")
);
    }
}