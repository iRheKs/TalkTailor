using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
namespace TextEditor
{
    
    public enum ConnectionPointType { In, Out }

    public class ConnectionPoint
    {
        public bool conected;
        public int index;

        public Rect rect;

        public ConnectionPointType type;

        public Node node;

        public GUIStyle style;

        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
        {
            this.node = node;
            this.type = type;
            this.style = style;
            this.OnClickConnectionPoint = OnClickConnectionPoint;
            rect = new Rect(0, 0, 10f, 10f);
        }

        public void Draw(Node node, float initialY = 0, float offset = 0)
        {

            switch (type)
            {
                case ConnectionPointType.In:
                    rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
                    rect.x = node.rect.x - rect.width + 8f;
                    break;

                case ConnectionPointType.Out:
                    rect.y = node.rect.y + initialY + (17.5f * offset);
                    rect.x = node.rect.x + node.rect.width - 8f;
                    break;
            }

            //rect.size *= scale;

            if (GUI.Button(rect, "", style))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint(this);
                }
            }
        }
    }
}