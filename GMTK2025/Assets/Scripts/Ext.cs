using System;
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
    public static T MaxOf<T>(this IEnumerable<T> list, Func<T, long> valueOfT)
    {
        long max = long.MinValue;
        T maxElement = default(T);
        bool hasElements = false;
        foreach (var item in list)
        {
            hasElements = true;
            long l = valueOfT(item);
            if (l > max)
            {
                max = l;
                maxElement = item;
            }
        }
        if (!hasElements) { throw new Exception($"{nameof(MaxOf)} was called with an empty enumerable."); }
        return maxElement;
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
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        Vector2 pointOnUnitCircle = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return pointOnUnitCircle;
    }
}
