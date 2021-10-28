using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void Load(int index)
    {
        StartCoroutine(LevelManager.LoadAsynchronously(index));
    }
}