using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application = UnityEngine.Application;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditorInternal;
#endif


/// <summary>
/// This store multiple data related to the game like the type of Ammo, the list of levels etc...
/// When access in the editor, if it don't exit it create an instance in the Resources folder, otherwise it load it
/// for modification.
/// 
/// Esto almacena múltiples datos relacionados con el juego como el tipo de Munición, la lista de niveles etc...
/// Al acceder en el editor, si no sale crea una instancia en la carpeta Resources, de lo contrario la carga
/// para su modificación.
/// </summary>
public class GameDatabase : ScriptableObject
{
#if UNITY_EDITOR
    static GameDatabase()
    {
        EditorApplication.playModeStateChanged += change =>
        {
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                var db = GameDatabase.Instance;
     
            }
        };
    }
    
#endif
    
    public static GameDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                var db = Resources.Load<GameDatabase>("GameDatabase");

                if (db == null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        db = CreateInstance<GameDatabase>();

                        if (!System.IO.Directory.Exists(Application.dataPath + "/Resources"))
                            AssetDatabase.CreateFolder("Assets", "Resources");
                        
                        AssetDatabase.CreateAsset(db, "Assets/Resources/GameDatabase.asset");
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        Debug.LogError("Game Database couldn't be found.");
                        return null;
                    }
#else
                    Debug.LogError("Game Database couldn't be found.");
                    return null;
#endif
                }

                instance = db;
            }

            return instance;
        }
    }

    static GameDatabase instance;

    public AmmoDatabase ammoDatabase;
    
}

[System.Serializable]
public class AmmoDatabase
{
    public int maxId = 0;
    public Queue<int> freeID = new Queue<int>();

    [System.Serializable]
    public class Entry
    {
        public string name;
        public int id;
    }

    public Entry[] entries;

#if  UNITY_EDITOR
    public Entry AddEntry(string name)
    {
        Entry e = new Entry();
        
        if(freeID.Count > 0)
            e.id = freeID.Dequeue();
        else
        {
            e.id = maxId;
            maxId++;
        }

        e.name = name;

        ArrayUtility.Add(ref entries, e);

        return e;
    }
    
    public void RemoveEntry(Entry e)
    {
        freeID.Enqueue(e.id);
        ArrayUtility.Remove(ref entries, e);
    }
#endif

    public Entry GetEntry(string name)
    {
        return entries.First(entry => entry.name == name);
    }

    public Entry GetEntry(int id)
    {
        return entries.First(entry => entry.id == id);
    }
}


#if UNITY_EDITOR

public class DBEditor : EditorWindow
{
    GameDatabase db;

    int m_EditedCategory = 0;
    string[] m_Categories = new[] { /*"Episodes",*/ "Ammo Type" };

    ReorderableList[] m_ReorderableLists = new ReorderableList[0];
    SerializedObject m_SerializedObject;
    
    [MenuItem("TDBR/Game Database")]
    static void Open()
    {
        GetWindow<DBEditor>();
    }

    void OnEnable()
    {
        db = GameDatabase.Instance;
        m_SerializedObject = new SerializedObject(db);

    }

    void OnGUI()
    {
        m_EditedCategory = GUILayout.Toolbar(m_EditedCategory, m_Categories);

        switch (m_EditedCategory)
        {
            case 0 :
                AmmoDatabaseEditor();
                break;
            default:
                break;
        }
    }

    void AmmoDatabaseEditor()
    { 
        AmmoDatabase.Entry todelete = null;
        for (int i = 0; i < db.ammoDatabase.entries.Length; ++i)
        {
            GUILayout.BeginHorizontal();
            
            EditorGUI.BeginChangeCheck();
            db.ammoDatabase.entries[i].name = GUILayout.TextField(db.ammoDatabase.entries[i].name);
            if(EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(db);
            
            if (GUILayout.Button("-", GUILayout.Width(64)))
            {
                todelete = db.ammoDatabase.entries[i];
            }
            GUILayout.EndHorizontal();
        }

        if (todelete != null)
        {
            ArrayUtility.Remove(ref db.ammoDatabase.entries, todelete);
        }

        if (GUILayout.Button("Add Ammo Type"))
        {
            db.ammoDatabase.AddEntry("Ammo");
            EditorUtility.SetDirty(db);
        }
    }
}
#endif
