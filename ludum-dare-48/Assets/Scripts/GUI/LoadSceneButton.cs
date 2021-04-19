using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GUI
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField]
        string[] _scenesToUnload;

        [SerializeField]
        string[] _scenesToLoad;

        [Inject]
        SceneService _sceneService;

        public void LoadScenes()
        {
            _sceneService.StartSceneTransition(_scenesToUnload, _scenesToLoad);
        }
    }
}