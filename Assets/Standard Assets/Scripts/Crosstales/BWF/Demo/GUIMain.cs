using Crosstales.BWF.Manager;
using Crosstales.BWF.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crosstales.BWF.Demo
{
	[HelpURL("https://www.crosstales.com/media/data/assets/badwordfilter/api/class_crosstales_1_1_b_w_f_1_1_demo_1_1_g_u_i_main.html")]
	public class GUIMain : MonoBehaviour
	{
		public bool AutoTest = true;

		public bool AutoReplace;

		public bool ReplaceLeet = true;

		public bool SimpleCheck = true;

		public float IntervalCheck = 0.5f;

		public float IntervalReplace = 0.5f;

		public InputField Text;

		public Text OutputText;

		public Text BadWordList;

		public Text BadWordCounter;

		public Text Name;

		public Text Version;

		public Text Scene;

		public Toggle TestEnabled;

		public Toggle ReplaceEnabled;

		public Toggle Badword;

		public Toggle Domain;

		public Toggle Capitalization;

		public Toggle Punctuation;

		public InputField BadwordReplaceChars;

		public InputField DomainReplaceChars;

		public InputField CapsTrigger;

		public InputField PuncTrigger;

		public Toggle LeetReplace;

		public Toggle SimpleCheckToggle;

		public Image BadWordListImage;

		public Color32 GoodColor = new Color32(0, byte.MaxValue, 0, 192);

		public Color32 BadColor = new Color32(byte.MaxValue, 0, 0, 192);

		public ManagerMask BadwordManager = ManagerMask.BadWord;

		public ManagerMask DomManager = ManagerMask.Domain;

		public ManagerMask CapsManager = ManagerMask.Capitalization;

		public ManagerMask PuncManager = ManagerMask.Punctuation;

		public List<string> Sources = new List<string>(30);

		private List<string> badWords = new List<string>();

		private float elapsedTimeCheck;

		private float elapsedTimeReplace;

		private bool tested;

		public void Start()
		{
			BadWordManager.isReplaceLeetSpeak = ReplaceLeet;
			if (!ReplaceLeet && LeetReplace != null)
			{
				LeetReplace.isOn = false;
			}
			BadWordManager.isSimpleCheck = SimpleCheck;
			if (!SimpleCheck && SimpleCheckToggle != null)
			{
				SimpleCheckToggle.isOn = false;
			}
		}

		public void Update()
		{
			elapsedTimeCheck += Time.deltaTime;
			elapsedTimeReplace += Time.deltaTime;
			if (AutoTest && !AutoReplace && elapsedTimeCheck > IntervalCheck)
			{
				Test();
				elapsedTimeCheck = 0f;
			}
			if (AutoReplace && elapsedTimeReplace > IntervalReplace)
			{
				Replace();
				elapsedTimeReplace = 0f;
			}
			if (BadwordReplaceChars != null)
			{
				BadWordManager.ReplaceCharacters = BadwordReplaceChars.text;
			}
			if (DomainReplaceChars != null)
			{
				DomainManager.ReplaceCharacters = DomainReplaceChars.text;
			}
			int result;
			if (CapsTrigger != null)
			{
				CapitalizationManager.CharacterNumber = ((!int.TryParse(CapsTrigger.text, out result)) ? 2 : ((result <= 2) ? 2 : result));
				CapsTrigger.text = CapitalizationManager.CharacterNumber.ToString();
			}
			if (PuncTrigger != null)
			{
				PunctuationManager.CharacterNumber = ((!int.TryParse(PuncTrigger.text, out result)) ? 2 : ((result <= 2) ? 2 : result));
				PuncTrigger.text = PunctuationManager.CharacterNumber.ToString();
			}
			if (tested)
			{
				if (badWords.Count > 0)
				{
					BadWordList.text = string.Join(Environment.NewLine, badWords.ToArray());
					BadWordListImage.color = BadColor;
				}
				else
				{
					BadWordList.text = "No bad words found";
					BadWordListImage.color = GoodColor;
				}
			}
			if (BadWordCounter != null)
			{
				BadWordCounter.text = badWords.Count.ToString();
			}
			if (OutputText != null)
			{
				OutputText.text = BWFManager.Mark(Text.text, badWords);
			}
		}

		public void TestChanged(bool val)
		{
			AutoTest = val;
		}

		public void ReplaceChanged(bool val)
		{
			AutoReplace = val;
		}

		public void BadwordChanged(bool val)
		{
			BadwordManager = (val ? ManagerMask.BadWord : ManagerMask.None);
		}

		public void DomainChanged(bool val)
		{
			DomManager = (val ? ManagerMask.Domain : ManagerMask.None);
		}

		public void CapitalizationChanged(bool val)
		{
			CapsManager = (val ? ManagerMask.Capitalization : ManagerMask.None);
		}

		public void PunctuationChanged(bool val)
		{
			PuncManager = (val ? ManagerMask.Punctuation : ManagerMask.None);
		}

		public void LeetChanged(bool val)
		{
			BadWordManager.isReplaceLeetSpeak = val;
		}

		public void SimpleChanged(bool val)
		{
			BadWordManager.isSimpleCheck = val;
		}

		public void FullscreenChanged(bool val)
		{
			Screen.fullScreen = val;
		}

		public void Test()
		{
			tested = true;
			badWords = BWFManager.GetAll(Text.text, BadwordManager | DomManager | CapsManager | PuncManager, Sources.ToArray());
		}

		public void Replace()
		{
			tested = true;
			string testString = "fuck it";
			testString = BWFManager.ReplaceAll(testString, ManagerMask.All);
			MonoBehaviour.print(testString);
		}

		public void OpenAssetURL()
		{
			Application.OpenURL("https://www.assetstore.unity3d.com/#!/list/42213-crosstales?aid=1011lNGT&pubref=BWF PRO");
		}

		public void OpenCTURL()
		{
			Application.OpenURL("https://www.crosstales.com");
		}

		public void Quit()
		{
			if (!Application.isEditor)
			{
				Application.Quit();
			}
		}
	}
}
