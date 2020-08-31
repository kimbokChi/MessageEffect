﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MessageEditor : EditorWindow
{
    private const int ODD = 1;

    private float mLetterSpacing = 20f;
    private int mFontSize = 26;
    private string mName;
    private string mMessage;
    private Transform mParent;

    private Font  mFont;
    private Color mColor = Color.white;

    [MenuItem("MessageEditor/Create Unsettled")]
    private static void Init()
    {
        MessageEditor window = EditorWindow.GetWindow(typeof(MessageEditor)) as MessageEditor;

        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Object Name", EditorStyles.label);
        mName = GUILayout.TextField(mName);

        GUILayout.Space(8f);

        GUILayout.Label("Message", EditorStyles.label);
        mMessage = EditorGUI.TextField(new Rect(2.5f, 63.5f, EditorGUIUtility.currentViewWidth - 7f, 18f), mMessage);

        GUILayout.Space(21f);

        GUILayout.Label("Font", EditorStyles.label);
        mFont = (Font)EditorGUILayout.ObjectField(mFont, typeof(Font), true);

        GUILayout.Label("Parent Object", EditorStyles.label);
        mParent = (Transform)EditorGUILayout.ObjectField(mParent, typeof(Transform), true);

        GUILayout.Label("Letter Spacing", EditorStyles.label);
        mLetterSpacing = EditorGUILayout.FloatField(mLetterSpacing);

        GUILayout.Label("Font Size", EditorStyles.label);
        mFontSize = EditorGUILayout.IntField(mFontSize);

        GUILayout.Label("Color", EditorStyles.label);
        mColor = EditorGUILayout.ColorField(mColor);

        if (GUILayout.Button("Create!") && !EditorApplication.isPlaying) {
            Create();
        }
    }
    private void Create()
    {
        GameObject newObject = new GameObject(mName, typeof(RectTransform), typeof(UnsettledText));

        Undo.RegisterCreatedObjectUndo(newObject, mName);

        newObject.transform.parent = mParent;
        newObject.transform.localPosition = Vector3.zero;

        if (newObject.TryGetComponent(out UnsettledText text)) {
            text.SetMessage(mMessage);
        }
        float charOffset = (mMessage.Length & ODD).Equals(ODD) ? 0f : mLetterSpacing * 0.5f;

        for (int i = 0; i < mMessage.Length; i++)
        {
            GameObject createChar = CreateUnSettledChar(mMessage[i]);

            createChar.transform.parent = newObject.transform;
            createChar.transform.localPosition = new Vector2((-mMessage.Length / 2 + i) * mLetterSpacing + charOffset, 0);
        }
    }
    private GameObject CreateUnSettledChar(char letter)
    {
        string name = $"Character[{letter}]";

        GameObject newObject = new GameObject(name, typeof(RectTransform), typeof(Text), typeof(UnsettledChar));

        Undo.RegisterCreatedObjectUndo(newObject, name);

        if (newObject.TryGetComponent(out Text text)) 
        {
            text.text = letter.ToString();

            text.alignment = TextAnchor.MiddleCenter;

            text.fontSize = mFontSize; text.font = mFont;

            text.color = mColor;
        }
        if (newObject.TryGetComponent(out UnsettledChar unsettled)) {
            unsettled.Setting(6, 5f);
        }
        return newObject;
    }
}
