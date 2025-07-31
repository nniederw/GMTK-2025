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
}
