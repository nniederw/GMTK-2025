using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Room")]
public class Room : ScriptableObject
{
    public string SceneName;
    public Vector2 BottomLeft;//todo change into list of valid space
    public Vector2 TopRight;
    private void Awake()
    {
        var loadable = Application.CanStreamedLevelBeLoaded(SceneName);
        if (!loadable) { throw new System.Exception($"Assigned Scene {SceneName} of {name} {nameof(Room)} can't be loaded."); }
    }
}