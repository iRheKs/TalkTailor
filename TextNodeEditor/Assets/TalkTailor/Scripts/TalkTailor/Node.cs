using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace TextEditor
{
#if UNITY_EDITOR
    [System.Serializable]
    public class Node
    {
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isSelected;

        public NodeInfo info;
        public NodeInfoDrawer infoDrawer;

        public ConnectionPoint inPoint;
        public List<ConnectionPoint> outPoint;

        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;
        
        private Texture2D selectedBackGround, defaultBackGround;

        public Action<Node> OnRemoveNode;
        public Action<Node> OnNodeSelected;

        Action<ConnectionPoint> OnCLickOut;
        Action<ConnectionPoint> OnCLickIn;
        GUIStyle outPointS;

        [SerializeField]
        private Vector2 mouseOffset;
        [SerializeField]
        private float scale;
        //private float minScale = 0.5f, maxScale = 3.0f;
        [SerializeField]
        private float initialHeigth;

        public Node(Vector2 position, float width, float height, Vector2 mouseOffset, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
        {
            CreateStyles();

            outPoint = new List<ConnectionPoint>();
            rect = new Rect(position.x, position.y, width, height);
            initialHeigth = height;
            this.mouseOffset = mouseOffset;
            //rect.size *= 0.5f;
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
            OnCLickIn = OnClickInPoint;
            OnCLickOut = OnClickOutPoint;
            outPointS = outPointStyle;
            defaultNodeStyle = style;
            OnRemoveNode = OnClickRemoveNode;
            info = new NodeInfo();
            SetUp(inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
        }

        private void CreateStyles()
        {
            defaultBackGround = new Texture2D(1, 1);
            defaultBackGround.SetPixel(0, 0, new Color(0.3f, 0.3f, 0.3f, 1));
            selectedBackGround = new Texture2D(1, 1);
            selectedBackGround.SetPixel(0, 0, new Color(0.5f, 0.5f, 0.5f, 1));
            defaultBackGround.Apply();
            selectedBackGround.Apply();

            defaultNodeStyle = new GUIStyle();
            defaultNodeStyle.normal.background = defaultBackGround;

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = selectedBackGround;

            style = defaultNodeStyle;
        }

        public void SetUp(GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
        {
            CreateStyles();
            OnCLickIn = OnClickInPoint;
            OnCLickOut = OnClickOutPoint;
            OnRemoveNode = OnClickRemoveNode;
            outPointS = outPointStyle;
            infoDrawer = ScriptableObject.CreateInstance<NodeInfoDrawer>();
            infoDrawer.info = info;
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnCLickIn);
            outPoint = new List<ConnectionPoint>();
            for(int i = 0; i < info.answerNumber; i++)
            {
                outPoint.Add(new ConnectionPoint(this, ConnectionPointType.Out, outPointS, OnCLickOut));
            }
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw(int scale)
        {
            Resize();
            GUI.Box(rect, title, style);
            rect.size *= scale;
            DrawInside();
            DrawConnectionPoints();
        }

        private void DrawInside()
        {
            string auxString = "";
            GUILayout.BeginArea(rect);
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(info.nodeName, EditorStyles.whiteBoldLabel);
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                for (int i = 0; i < info.lineNumber && i < 3; i++)
                {
                    if (i < 2 || info.lineNumber == 3)
                    {
                        if (info.lines[i].Length > 24)
                        {
                            auxString = info.lines[i].Substring(0, 24) + "..";
                        }
                        else
                        {
                            auxString = info.lines[i];
                        }
                        EditorGUILayout.LabelField(auxString);
                    }
                    else
                    {
                        EditorGUILayout.LabelField("...");
                    }
                }

                if (info.lineNumber == 0)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }

                for (int i = 0; i < info.answerNumber; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (info.answers[i].answerText.Length > 10)
                    {
                        auxString = info.answers[i].answerText.Substring(0, 10) + "..";
                    }
                    else
                    {
                        auxString = info.answers[i].answerText;
                    }
                    EditorGUILayout.LabelField(auxString, GUILayout.MaxWidth(100));
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                GUILayout.EndArea();
            }

        }

        private void DrawConnectionPoints()
        {
            inPoint.Draw(this);

            if (outPoint.Count < info.answerNumber)
            {
                AddAnswerPoint();
            }
            float initialY = CalculateYFirstAnswer();
            for (int i = 0; i < info.answerNumber; i++)
            {
                outPoint[i].Draw(this, initialY, i);
            }
        }

        private void AddAnswerPoint()
        {
            ConnectionPoint point = new ConnectionPoint(this, ConnectionPointType.Out, outPointS, OnCLickOut);
            point.index = outPoint.Count;
            outPoint.Add(point);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition - mouseOffset))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            selectedNodeStyle.normal.background = selectedBackGround;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            defaultNodeStyle.normal.background = defaultBackGround;
                            style = defaultNodeStyle;
                        }
                    }
                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition - mouseOffset))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }

        private void Resize()
        {
            rect.height = initialHeigth;
            int totalOffset = info.lineNumber + info.answerNumber;
            float lineHeigth = EditorGUIUtility.singleLineHeight;
            if (totalOffset >= 3)
            {
                rect.height += lineHeigth * (totalOffset - 2) + 10;
            }
        }

        private float CalculateYFirstAnswer()
        {
            float res = 0;
            if (info.lineNumber > 1)
            {

                res = (EditorGUIUtility.singleLineHeight * (info.lineNumber + 1)) + EditorGUIUtility.standardVerticalSpacing * 2 * (info.lineNumber);
            }
            else
            {
                res = (EditorGUIUtility.singleLineHeight * 2) + 7.5f;
            }

            return res;
        }
    }

    public class NodeInfoDrawer : ScriptableObject
    {
        public NodeInfo info;
    }

#endif

    [Serializable]
    public class NodeInfo
    {
        public int ID;
        public int lineNumber, answerNumber;
        public string nodeName;
        public List<string> lines = new List<string>();
        public List<Answer> answers = new List<Answer>();
    }

    [Serializable]
    public class Answer
    {
        public string answerText;
        public int connection;
    }
}