using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class MiniMapSnapshot : MonoBehaviour
{
	public MiniMapSettings Setting;

	public Color GizmoColor = new Color(0f, 1f, 0f, 0.25f);

	public string TextureName;

	public string TextureDir => Setting.TextureDir;

	public string TexturePath => Setting.TextureDir + "/" + TextureName + ".png";
}
