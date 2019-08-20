using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using FMB_Tools;
namespace TextEditor
{
    public class TextNodeWindow : EditorWindow
    {

        #region VARIABLES
        private DialogueEditor dialogue;
        private Dialogue d;

        private EditorSection nameSection;
        private EditorSection dialogueBodySection;
        private EditorSection nodeInspectorSection;
        private List<EditorSection> sections;

        float currentInspectorWidth;
        Rect cursorChangeRect;
        bool resize;
        Node currentNodeSelected;
        NodeInfoInspector infoInspector;

        string dialogueName;
        #endregion

        [MenuItem("Window/Talk Tailor Window")]
        private static void OpenWindow()
        {
            TextNodeWindow window = GetWindow<TextNodeWindow>();
            window.titleContent = new GUIContent("Talk Tailor Window");
            window.Show();
        }

        private void OnEnable()
        {
            sections = new List<EditorSection>();
            nameSection = new EditorSection(new Rect(0, 0, position.width, 50), new Color(0.5f, 0.5f, 0.5f, 1));
            nodeInspectorSection = new EditorSection(new Rect(position.width - 300, nameSection.GetRect().height, 300, position.height - nameSection.GetRect().height), new Color(0.4f, 0.4f, 0.4f, 1));
            nodeInspectorSection.minWidth = 200;
            nodeInspectorSection.maxWidth = 400;
            nodeInspectorSection.Hide();
            dialogueBodySection = new EditorSection(new Rect(0, nameSection.GetRect().height, position.width - nodeInspectorSection.GetRect().width, position.height - nameSection.GetRect().height), new Color(0.2f, 0.2f, 0.2f));

            sections.Add(nameSection);
            sections.Add(nodeInspectorSection);
            sections.Add(dialogueBodySection);
        }

        private void OnDisable()
        {
            if (d != null && dialogue != null)
            {
                Save();
            }
        }

        private void OnGUI()
        {
            DrawSections();
            if (dialogue != null)
            {
                dialogue.DrawDialogue(dialogueBodySection.GetRect());

                dialogue.ProcessNodeEvents(Event.current);
                ProcessEvents(Event.current);
            }
            if (GUI.changed)
                Repaint();
        }


        public void OpenWindow(Dialogue dObject)
        {
            d = dObject;
            dialogue = dObject.GetEditor();
            titleContent = new GUIContent("Dialogue Editor");
            dialogue.Initialize(dialogueBodySection.GetRect(), OnNodeSelected, OnAreaClicked);
            dialogueName = d.GetName();
            Show();
        }

        #region DRAW
        private void DrawSections()
        {
            nameSection.SetRect(new Rect(0, 0, position.width, 35));
            nodeInspectorSection.SetRect(new Rect(position.width - 300, nameSection.GetRect().height, 300, position.height - nameSection.GetRect().height));
            dialogueBodySection.SetRect(new Rect(0, nameSection.GetRect().height, position.width - nodeInspectorSection.GetRect().width, position.height - nameSection.GetRect().height));

            foreach (var item in sections)
            {
                GUI.DrawTexture(item.GetRect(), item.GetTexture());
            }
            
            DrawDialogueInfo();
            DrawNodeInfo();
        }

        private void DrawDialogueInfo()
        {
            GUILayout.BeginArea(nameSection.GetRect());
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                if (dialogue == null)
                {
                    d = (Dialogue)EditorGUILayout.ObjectField(new GUIContent("Dialogue to edit"), d, typeof(Dialogue), false);
                    if (d != null && d.isDialogue)
                    {
                        dialogue = d.GetEditor();
                        dialogue.Initialize(dialogueBodySection.GetRect(), OnNodeSelected, OnAreaClicked);
                        dialogueName = d.GetName();
                        DLLWrapper.SwapDialogue(d.pointer);
                    }
                    else if (d != null && !d.isDialogue)
                    {
                        //Debug.LogError("Dialogue asset must be initialized, not unity error");
                        //Debug.LogAssertion("Dialogue asset must be initialized, not unity assertion");
                        Debug.LogWarning("Dialogue asset must be initialized");
                        //Debug.Log("Ahora en serio esto es mio propio no algo de unity");
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(dialogueName);

                    if (GUILayout.Button("Save", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(200)))
                    {
                        Save();
                    }
                    if (GUILayout.Button("Export", EditorStyles.miniButtonRight, GUILayout.MaxWidth(200)))
                    {
                        Export();
                    }
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        public void DrawNodeInfo()
        {
            if (currentNodeSelected != null)
            {
                GUILayout.BeginArea(nodeInspectorSection.GetRect());
                {

                    EditorGUILayout.BeginVertical();

                    infoInspector = (NodeInfoInspector)Editor.CreateEditor(currentNodeSelected.infoDrawer);
                    infoInspector.OnWindowGUI();

                    EditorGUILayout.EndVertical();

                    GUILayout.EndArea();
                }
            }
        }
        #endregion

        #region EVENT_MANAGEMENT
        private void ProcessEvents(Event e)
        {
            dialogue.Drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 2)
                    {
                        dialogue.OnDrag(e.delta);
                    }
                    break;
                case EventType.MouseUp:
                    resize = false;
                    break;
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add node"), false, () => dialogue.OnClickAddNode(mousePosition - dialogueBodySection.GetRect().position));
            genericMenu.ShowAsContext();
        }
        #endregion

        private void ResizeNodeInspector()
        {
            if (cursorChangeRect.Contains(Event.current.mousePosition))
            {
                resize = true;
            }
            if (resize)
            {
                currentInspectorWidth = Event.current.mousePosition.x;
                if (position.width - currentInspectorWidth <= nodeInspectorSection.maxWidth && position.width - currentInspectorWidth >= nodeInspectorSection.minWidth)
                {

                }
                cursorChangeRect.Set(currentInspectorWidth, cursorChangeRect.y, cursorChangeRect.width, cursorChangeRect.height);
            }
            GUI.changed = true;
        }

        #region ACTIONS
        private void OnAreaClicked(DialogueEditor dialogue)
        {
            nodeInspectorSection.Hide();
        }

        private void OnNodeSelected(Node node)
        {
            nodeInspectorSection.Unhide();
            currentNodeSelected = node;
        }
        #endregion

        private void Export()
        {
            string path = null;
            path = EditorUtility.SaveFilePanel("Export Dialogue", "", dialogueName, "xml");
            d.path = path;
            DLLWrapper.ExportDialogue(path);
        }

        private void Save()
        {
            DLLWrapper.SaveDialogue();
        }
    }
}
