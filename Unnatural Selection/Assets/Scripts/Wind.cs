using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    private static Wind m_Singleton;
    public static Wind GetSingleton() {
        return m_Singleton;
    }

    private void Awake() {
        if (m_Singleton != null) {
            DestroyImmediate(gameObject);
            return;
        }
        m_Singleton = this;

        DontDestroyOnLoad(this);
    }
}
