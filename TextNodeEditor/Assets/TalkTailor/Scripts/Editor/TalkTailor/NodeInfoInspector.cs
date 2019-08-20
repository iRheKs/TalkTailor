using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TextEditor
{
    [CustomEditor(typeof(NodeInfoDrawer))]
    public class NodeInfoInspector : Editor
    {
        NodeInfoDrawer trg;

        SerializedProperty nodeID;
        SerializedProperty nodeName;
        SerializedProperty lines;
        SerializedProperty answers;
        SerializedProperty linesLength;
        SerializedProperty answersLength;

        private void Init()
        {
            nodeID = serializedObject.FindProperty("info").FindPropertyRelative("ID");
            nodeName = serializedObject.FindProperty("info").FindPropertyRelative("nodeName");
            lines = serializedObject.FindProperty("info").FindPropertyRelative("lines");
            answers = serializedObject.FindProperty("info").FindPropertyRelative("answers");
            linesLength = serializedObject.FindProperty("info").FindPropertyRelative("lineNumber");
            answersLength = serializedObject.FindProperty("info").FindPropertyRelative("answerNumber");
        }

        public void OnWindowGUI()
        {
            //DrawDefaultInspector();
            serializedObject.Update();

            trg = (NodeInfoDrawer)target;

            Init();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(nodeName, new GUIContent("Node Name"));
            if (EditorGUI.EndChangeCheck())
            {
                DLLWrapper.ChangeNodeName(nodeID.intValue, nodeName.stringValue);
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Lines"), EditorStyles.boldLabel);
            EditorGUILayout.Space();

            for (int i = 0; i < lines.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(lines.GetArrayElementAtIndex(i), GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    DLLWrapper.ChangeLine(nodeID.intValue, i, lines.GetArrayElementAtIndex(i).stringValue);
                }
                if (GUILayout.Button(new GUIContent("-", "Delete line"), EditorStyles.miniButton, GUILayout.Width(20f)))
                {
                    lines.DeleteArrayElementAtIndex(i);
                    DLLWrapper.DeleteLine(nodeID.intValue, i);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button(new GUIContent("+", "Add new line")))
            {
                lines.arraySize += 1;
                DLLWrapper.AddLine(nodeID.intValue, " ");
            }
            linesLength.intValue = lines.arraySize;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Answers"), EditorStyles.boldLabel);
            EditorGUILayout.Space();

            for (int i = 0; i < answers.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(answers.GetArrayElementAtIndex(i).FindPropertyRelative("answerText"), GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                {
                    DLLWrapper.ChangeAnswer(nodeID.intValue, i, answers.GetArrayElementAtIndex(i).FindPropertyRelative("answerText").stringValue);
                }
                if (GUILayout.Button(new GUIContent("-", "Delete answer"), EditorStyles.miniButton, GUILayout.Width(20f)))
                {
                    answers.DeleteArrayElementAtIndex(i);
                    DLLWrapper.DeleteAnswer(nodeID.intValue, i);
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button(new GUIContent("+", "Add new answer")))
            {
                answers.arraySize += 1;
                DLLWrapper.AddAnswer(nodeID.intValue, " ");
            }
            answersLength.intValue = answers.arraySize;

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }

            EditorUtility.SetDirty(trg);
        }

    }
}