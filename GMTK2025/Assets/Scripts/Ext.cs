using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Ext
{
    public static TInterface GetInterfaceComponent<TInterface>(this GameObject obj)
    {
        return obj.GetComponents<MonoBehaviour>().OfType<TInterface>().FirstOrDefault();
    }
    public static TInterface GetInterfaceComponent<TInterface>(this Component obj)
    {
        return obj.GetComponents<MonoBehaviour>().OfType<TInterface>().FirstOrDefault();
    }
    public static TInterface[] GetInterfaceComponents<TInterface>(this GameObject obj)
    {
        return obj.GetComponents<MonoBehaviour>().OfType<TInterface>().ToArray();
    }
    public static TInterface[] GetInterfaceComponents<TInterface>(this Component obj)
    {
        return obj.GetComponents<MonoBehaviour>().OfType<TInterface>().ToArray();
    }
    public static TInterface GetInterfaceComponentInChildren<TInterface>(this GameObject obj)
    {
        return obj.GetComponentsInChildren<MonoBehaviour>().OfType<TInterface>().FirstOrDefault();
    }
    public static TInterface GetInterfaceComponentInChildren<TInterface>(this Component obj)
    {
        return obj.GetComponentsInChildren<MonoBehaviour>().OfType<TInterface>().FirstOrDefault();
    }
    public static TInterface[] GetInterfaceComponentsInChildren<TInterface>(this GameObject obj)
    {
        return obj.GetComponentsInChildren<MonoBehaviour>().OfType<TInterface>().ToArray();
    }
    public static TInterface[] GetInterfaceComponentsInChildren<TInterface>(this Component obj)
    {
        return obj.GetComponentsInChildren<MonoBehaviour>().OfType<TInterface>().ToArray();
    }
    public static bool SceneExistsInBuildSettings(string sceneName)
    {
        return GetAllScenePathsInBuild().Any(scenePath =>
        {
            string sceneNameInBuild = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            return (sceneNameInBuild == sceneName);
        });
    }
    private static IEnumerable<string> GetAllScenePathsInBuild()
    {
        int count = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < count; i++)
        {
            yield return UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
        }
    }
    public static Vector2 RandomPointOnUnitCircle()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector2 pointOnUnitCircle = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return pointOnUnitCircle;
    }
}
