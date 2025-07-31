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
    public static Vector2 RandomPointOnUnitCircle()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector2 pointOnUnitCircle = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return pointOnUnitCircle;
    }
}
