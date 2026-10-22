using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class MiniMapSnapshot : MonoBehaviour
{
	public MiniMapSettings Setting;

	public Color GizmoColor;

	public string TextureName;

	public string TextureDir => null;

	public string TexturePath => null;
}
