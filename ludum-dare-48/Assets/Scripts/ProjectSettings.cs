using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectSettings", menuName = "Project/Project Settings")]
public class ProjectSettings : ScriptableObject
{
    public CatCommonSettings catCommonSettings;
}