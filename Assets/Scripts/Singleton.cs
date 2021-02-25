using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// The instance.
    /// </summary>
    private static T instance;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if ((object)instance == null)
            {
                instance = FindObjectOfType<T>();
                if ((object)instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Awake()
    {
        if ((object)instance == null)
        {
            Debug.Log("Created!" + this.name);
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Destroyed! " + (object)instance.name);
            Destroy(gameObject);
        }
    }

    #endregion

}
