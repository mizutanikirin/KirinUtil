using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class separatorDrawer:DecoratorDrawer
{
	SeparatorAttribute separatorAttribute { get { return ((SeparatorAttribute)attribute); } }


	public override void OnGUI(Rect _position)
	{
		if(separatorAttribute.title == "")
		{
			_position.height = 1;
			_position.y += 19;
			GUI.Box(_position, "");
		} else
		{
			Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(separatorAttribute.title));
			float separatorWidth = (_position.width - textSize.x) / 2.0f - 5.0f;
			_position.y += 19;

			InitStyles();
			GUI.Box(new Rect(_position.xMin, _position.yMin, separatorWidth, 1), "", currentStyle);
			GUI.Label(new Rect(_position.xMin + separatorWidth + 5.0f, _position.yMin - 8.0f, textSize.x, 20), separatorAttribute.title);
			GUI.Box(new Rect(_position.xMin + separatorWidth + 10.0f + textSize.x, _position.yMin, separatorWidth, 1), "", currentStyle);
		}
	}

	public override float GetHeight()
	{
		return 41.0f;
	}

	private GUIStyle currentStyle = null;
	private void InitStyles()
	{
		if (currentStyle == null)
		{
			currentStyle = new GUIStyle(GUI.skin.box);
			currentStyle.normal.background = MakeTex(2, 2, new Color(0.4f, 0.4f, 0.4f, 1f));

        }
	}

	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i)
		{
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}
}
#endif