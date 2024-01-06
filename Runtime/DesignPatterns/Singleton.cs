using UnityEngine;

[DefaultExecutionOrder(-50)]
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static object _lock = new();

    private static bool _isQuitting ;

    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null || _instance.gameObject.activeSelf == false)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("There is more than one manager of this type in this scene : " + typeof(T).Name);
                        return _instance;
                    }

                    if (_instance == null && !_isQuitting) return null;
                }

                return _instance;
            }
        }
    }

    public static void SetActive(bool _active)
    {
        Instance.gameObject.SetActive(_active);
    }

    private void OnDestroy()
    {
        OnDestroySpecific();
    }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    protected virtual void OnDestroySpecific() { }
}