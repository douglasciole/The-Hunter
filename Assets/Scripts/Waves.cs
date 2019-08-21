using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditorInternal;
#endif
using UnityEngine.UI;


[System.Serializable]
public struct MobWave
{
    public GameObject Prefab;
    public int Level;
    public float yPosition;
    public float yVariationMin;
    public float yVariationMax;
}

public class Waves : MonoBehaviour {

    
    public int acttualLevel = 0;
    int waveSize = 4;
    public int actualWaveSize = 2;


    int waveNumberOfMonsters = 0;
    [HideInInspector]
    public List<MobWave> WaveMonsters = new List<MobWave>();
    public List<int> mobsIndex = new List<int>();

    public Animator anim;
    public Text levelDisplay;
    public Text levelDisplayShadow;

    // Use this for initialization
    void Start()
    {
        Enemy.OnDie += enemyDie;
        for (int i = 0; i < WaveMonsters.Count; i++)
        {
            mobsIndex.Add(i);
        }
        finishWave();
    }

    public MobWave getSpawn()
    {
        List<int> intsTentados = new List<int>(mobsIndex);
        
        if (WaveMonsters.Count > 0) {
            int i = Random.Range(0, intsTentados.Count);

            while (WaveMonsters[i].Level > actualWaveSize)
            {
                intsTentados.RemoveAt(i);
                i = Random.Range(0, intsTentados.Count);
            }

            waveNumberOfMonsters += 1;
            actualWaveSize -= WaveMonsters[i].Level;
            return WaveMonsters[i];
        }

        Debug.LogError("Sem mobs cadastrados ou com níveis baixos");
        return new MobWave();
    }

    public void finishWave()
    {
        acttualLevel += 1;
        waveSize = Mathf.FloorToInt((waveSize / 2) * 3);
        actualWaveSize = waveSize;
        waveNumberOfMonsters = 0;

        levelDisplay.text = acttualLevel.ToString();
        levelDisplayShadow.text = acttualLevel.ToString();

        if (anim)
            anim.Play("LevelUp");
    }

    public void enemyDie()
    {
        waveNumberOfMonsters -= 1;
        if (waveNumberOfMonsters <= 0 && actualWaveSize <= 0)
        {
            finishWave();
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(Waves))]
public class LevelDataEditor : Editor
{
    private ReorderableList list;

    private void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("WaveMonsters"),
                true, true, true, true);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        EditorGUILayout.Space();

        list.elementHeight = EditorGUIUtility.singleLineHeight * 5f;
        list.drawElementCallback =
        (Rect rect, int index, bool isActive, bool isFocused) => {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width - 70, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Prefab"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Level"), GUIContent.none);


            EditorGUI.Slider(
                new Rect(rect.x, rect.y + 2 + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("yPosition"), -4.6f, 3.7f);

            EditorGUI.Slider(
                new Rect(rect.x, rect.y + 4 + (EditorGUIUtility.singleLineHeight * 2), rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("yVariationMin"), 0, 5f);

            EditorGUI.Slider(
                new Rect(rect.x, rect.y + 6 + (EditorGUIUtility.singleLineHeight * 3), rect.width, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("yVariationMax"), 0, 5f);

        };

        list.DoLayoutList();

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
