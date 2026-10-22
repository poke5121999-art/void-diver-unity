using System.Collections.Generic;
using UnityEngine;

public class MoodSceneSetup : MonoBehaviour
{
	[Header("플레이 시 활성화할 오브젝트/컴포넌트")]
	[SerializeField]
	private List<GameObject> _enableGameObjects;

	[SerializeField]
	private List<Behaviour> _enableComponents;

	[Header("플레이 시 비활성화할 오브젝트/컴포넌트")]
	[SerializeField]
	private List<GameObject> _disableGameObjects;

	[SerializeField]
	private List<Behaviour> _disableComponents;
}
