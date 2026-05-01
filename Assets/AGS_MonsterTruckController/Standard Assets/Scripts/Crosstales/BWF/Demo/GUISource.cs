using Crosstales.BWF.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.BWF.Demo
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_g_u_i_source.html")]
	public class GUISource : MonoBehaviour
	{
		public GameObject ItemPrefab;

		public GameObject Target;

		public Scrollbar Scroll;

		public GUIMain GuiMain;

		public int ColumnCount = 1;

		public Vector2 SpaceWidth = new Vector2(8f, 8f);

		public Vector2 SpaceHeight = new Vector2(8f, 8f);

		public void Start()
		{
			StartCoroutine(buildLanguageList());
		}

		private IEnumerator buildLanguageList()
		{
			if (!(ItemPrefab != null) || !(Scroll != null))
			{
				yield break;
			}
			while (!BWFManager.isReady)
			{
				yield return null;
			}
			RectTransform rowRectTransform = ItemPrefab.GetComponent<RectTransform>();
			RectTransform containerRectTransform = Target.GetComponent<RectTransform>();
			for (int num = Target.transform.childCount - 1; num >= 0; num--)
			{
				Transform child = Target.transform.GetChild(num);
				child.SetParent(null);
				UnityEngine.Object.Destroy(child.gameObject);
			}
			List<Source> items = BWFManager.Sources(ManagerMask.BadWord);
			float width = containerRectTransform.rect.width / (float)ColumnCount - (SpaceWidth.x + SpaceWidth.y) * (float)ColumnCount;
			float height = rowRectTransform.rect.height - (SpaceHeight.x + SpaceHeight.y);
			int rowCount = items.Count / ColumnCount;
			if (rowCount > 0 && items.Count % rowCount > 0)
			{
				rowCount++;
			}
			float scrollHeight = height * (float)rowCount;
			RectTransform rectTransform = containerRectTransform;
			Vector2 offsetMin = containerRectTransform.offsetMin;
			rectTransform.offsetMin = new Vector2(offsetMin.x, (0f - scrollHeight) / 2f);
			RectTransform rectTransform2 = containerRectTransform;
			Vector2 offsetMax = containerRectTransform.offsetMax;
			rectTransform2.offsetMax = new Vector2(offsetMax.x, scrollHeight / 2f);
			int i = 0;
			for (int j = 0; j < items.Count; j++)
			{
				if (j % ColumnCount == 0)
				{
					i++;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(ItemPrefab);
				gameObject.name = Target.name + " item at (" + j + "," + i + ")";
				gameObject.transform.SetParent(Target.transform);
				gameObject.transform.localScale = Vector3.one;
				gameObject.GetComponent<SourceEntry>().Source = items[j];
				gameObject.GetComponent<SourceEntry>().GuiMain = GuiMain;
				RectTransform component = gameObject.GetComponent<RectTransform>();
				float x = (0f - containerRectTransform.rect.width) / 2f + (width + SpaceWidth.x) * (float)(j % ColumnCount) + SpaceWidth.x * (float)ColumnCount;
				float y = containerRectTransform.rect.height / 2f - (height + SpaceHeight.y) * (float)i;
				component.offsetMin = new Vector2(x, y);
				Vector2 offsetMin2 = component.offsetMin;
				x = offsetMin2.x + width;
				Vector2 offsetMin3 = component.offsetMin;
				y = offsetMin3.y + height;
				component.offsetMax = new Vector2(x, y);
			}
			Scroll.value = 1f;
		}
	}
}
