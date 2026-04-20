using UnityEngine;

public class WinchTarget : MonoBehaviour
{
	private SpriteRenderer _spriteRenderer;

	private bool lookedForTransformView;

	private PhotonTransformView _tView;

	[HideInInspector]
	public bool DynamicTarget;

	public SpriteRenderer spriteRenderer
	{
		get
		{
			if (_spriteRenderer == null)
			{
				_spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return _spriteRenderer;
		}
	}

	public PhotonTransformView tView
	{
		get
		{
			if (!lookedForTransformView)
			{
				_tView = GetComponentInParent<PhotonTransformView>();
				lookedForTransformView = true;
			}
			return _tView;
		}
	}
}
