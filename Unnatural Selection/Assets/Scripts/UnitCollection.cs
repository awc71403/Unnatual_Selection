using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCollection : MonoBehaviour
{
    #region Variables
    private static UnitCollection m_Singleton;
    public static UnitCollection GetSingleton() {
        return m_Singleton;
    }

    [SerializeField]
    public GameObject[] insectFaction;

    [SerializeField]
    public GameObject[] mechFaction;

    [SerializeField]
    public GameObject[] rockFaction;

    [SerializeField]
    public GameObject[] shadowFaction;
    #endregion

    #region Initialization
    public void Awake() {
        // Singleton makes sure there is only one of this object
        if (m_Singleton != null) {
            DestroyImmediate(gameObject);
            return;
        }
        m_Singleton = this;

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    #region Helper
    public GameObject[] FactionPicker(int picker) {
        switch (picker) {
            case 0:
                return insectFaction;
            case 1:
                return mechFaction;
            case 2:
                return rockFaction;
            case 3:
                return shadowFaction;
        }
        return insectFaction;
    }
    #endregion
}
