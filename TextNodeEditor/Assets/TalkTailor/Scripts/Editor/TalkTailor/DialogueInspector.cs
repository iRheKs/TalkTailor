using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace TextEditor
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueInspector : Editor
    {
        Dialogue trg;
        TextNodeWindow win;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (serializedObject == null)
            {
                return;
            }

            trg = (Dialogue)target;

            EditorGUI.BeginChangeCheck();

            if (trg.isDialogue)
            {
                if (GUILayout.Button("Edit"))
                {
                    if(!trg.IsFirst())
                        DLLWrapper.SwapDialogue(trg.pointer);
                    else if (trg.IsFirst())
                    {
                        trg.Second();
                        trg.pointer = DLLWrapper.LoadDialogue(trg.path);
                    }
                    if (win == null)
                        win = (TextNodeWindow)EditorWindow.GetWindow(typeof(TextNodeWindow));
                    win.OpenWindow(trg);
                }
            }
            else
            {
                if (GUILayout.Button("Init"))
                {
                    Initialize();
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(trg);
        }
        private void Initialize()
        {
            trg.path = EditorUtility.OpenFilePanel("New Dialogue", "", "xml");
            int index = trg.path.LastIndexOf('/');
            if (index >= 0)
            {
                string substring = trg.path.Substring(index);
                trg.dialogueName = substring.Substring(1, substring.Length - 5);
                trg.CreateDialogue();
            }
        }
    }
}