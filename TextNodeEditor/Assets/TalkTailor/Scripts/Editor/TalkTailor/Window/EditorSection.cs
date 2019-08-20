﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FMB_Tools
{
	public class EditorSection
	{

		private Rect sectionRect;
		private Texture2D sectionTexture;

        private bool hidden = false;

        public int minWidth, maxWidth;

		public EditorSection (Rect rect, Color color)
		{
			sectionRect = rect;

			sectionTexture = new Texture2D (1, 1);
			sectionTexture.SetPixel (0, 0, color);
			sectionTexture.Apply ();
		}

		public EditorSection (Rect rect, Texture2D texture)
		{
			sectionRect = rect;
			sectionTexture = texture;

		}

		public Rect GetRect ()
		{
			return sectionRect;
		}

		public void SetRect (float width, float height)
		{
			sectionRect.width = width;
			sectionRect.height = height;
		}

		public void SetRect (Rect rect)
		{
            if (!hidden)
            {
                sectionRect = rect;
            }else
            {
                sectionRect = new Rect();
            }
		}

		public void SetRect (float x, float y, float width, float height)
		{
            if (!hidden)
            {
                sectionRect.x = x;
                sectionRect.y = y;
                sectionRect.width = width;
                sectionRect.height = height;
            }
            else
            {
                sectionRect = new Rect();
            }
		}

		public void SetTexture (Texture2D texture)
		{
			sectionTexture = texture;
		}

		public Texture2D GetTexture ()
		{
			return sectionTexture;
		}

		public void RefreshTextureColor ()
		{
			sectionTexture.Apply ();
		}

		public Color GetTextureColor (int x, int y)
		{
			return sectionTexture.GetPixel (x, y);
		}

        public void Hide()
        {
            hidden = true;
            GUI.changed = true;
        }
        public void Unhide()
        {
            hidden = false;
            GUI.changed = true;
        }
	}
}
