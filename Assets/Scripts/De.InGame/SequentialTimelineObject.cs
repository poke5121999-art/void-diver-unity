using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class SequentialTimelineObject : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _timelineObjects;

	private void Start()
	{
		PlaySequenceAsync().Forget();
	}

	private async UniTaskVoid PlaySequenceAsync()
	{
		foreach (GameObject timelineObject in _timelineObjects)
		{
			if (timelineObject != null)
			{
				timelineObject.SetActive(false);
			}
		}
		for (int i = 0; i < _timelineObjects.Length; i++)
		{
			GameObject go = _timelineObjects[i];
			if (!(go == null))
			{
				PlayableDirector director = go.GetComponent<PlayableDirector>();
				go.SetActive(true);
				if (director != null && director.duration > 0.0)
				{
					await WaitForDirectorAsync(director);
				}
				go.SetActive(false);
			}
		}
		Object.Destroy(gameObject);
	}

	private static async UniTask WaitForDirectorAsync(PlayableDirector director)
	{
		await UniTask.Yield();
		await UniTask.WaitWhile(() => director != null && director.state == PlayState.Playing);
	}
}
