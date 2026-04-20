using UnityEngine;

public class ColorPicker : MonoBehaviour
{
	public Texture2D ColorImage;

	public RectTransform ColorPickerRect;

	public RectTransform Handler;

	public Color ResultColor;

	private float width;

	private float height;

	private void Start()
	{
		width = ColorPickerRect.rect.width;
		height = ColorPickerRect.rect.height;
	}

	public void HandlerMoved()
	{
		Texture2D colorImage = ColorImage;
		Vector3 localPosition = Handler.localPosition;
		float u = localPosition.x / width;
		Vector3 localPosition2 = Handler.localPosition;
		Color pixelBilinear = colorImage.GetPixelBilinear(u, localPosition2.y / height);
		Vector3 localPosition3 = Handler.localPosition;
		localPosition3.x = Mathf.Clamp(localPosition3.x, 0f, width);
		localPosition3.y = Mathf.Clamp(localPosition3.y, 0f, height);
		Handler.localPosition = localPosition3;
	}
}
