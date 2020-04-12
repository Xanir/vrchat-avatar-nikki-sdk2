using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HUDShader2Window : EditorWindow {

	private static string EDITORFOLDER = "/Khodrins Assets/HUD Shader 2/Scripts/Editor/";
	private static string UPDATEURL = "http://khodrin.com/online_unity_assets/hud_shader_2/";
	private static bool[] foldouts;
	private static HUDShader2HelpCollection help = null;
	private static HUDShader2HelpCollection helpUpdate = null;

	private Vector2 scrollPos;

	public static HUDShader2Window Instance;

	 
	public static void ShowWindow () {
		EditorWindow.GetWindow (typeof(HUDShader2Window));
	}

	public void Awake () {
		loadDataFromJson ();
		Instance = this;
	}

	private static void loadDataFromJson () {
		if (File.Exists (Application.dataPath + EDITORFOLDER + "help.json")) {
			string helpJson = File.ReadAllText (Application.dataPath + EDITORFOLDER + "help.json");
			help = JsonUtility.FromJson<HUDShader2HelpCollection> (helpJson);
			initializeFoldouts ();
		}
	}

	private static void initializeFoldouts () {
		foldouts = new bool[help.faqs.Length];
		for (int i = 0; i < help.faqs.Length; i++) {
			foldouts [i] = false;
		}
	}

	private static void updateFAQ () {
		LoadText (UPDATEURL + "help.json");
	}

	//Text Loading
	private static WWW www;

	private static void LoadText(string url)
	{
		www = new WWW(url);
		#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
			EditorApplication.update = MyUpdateText;
		else
			WaitForDownloadText();
		#else
		WaitForDownloadText();
		#endif
	}

	#if UNITY_EDITOR
	private static void MyUpdateText ()
	{
		if (www.isDone)
		{
			EditorApplication.update = null;
			LoadCompletedText();
		}
	}
	#endif

	private static IEnumerator WaitForDownloadText()
	{
		yield return www;
		LoadCompletedText();
	}

	private static void LoadCompletedText()
	{
		checkFaqUpdate(www.text);
	}

	private static void checkFaqUpdate (string content) {
		helpUpdate = JsonUtility.FromJson<HUDShader2HelpCollection> (content);
		if (helpUpdate.revision > help.revision) {
			proccessUpdate ();
		} else {
			Debug.Log ("No Update available.");
		}
	}
	//End Text Loading

	//Image Loading
	private static WWW wwwImage;
	private static List<string> images;

	private static void proccessUpdate () {
		images = new List<string> ();

		for (int i = 0; i < helpUpdate.faqs.Length; i++) {
			HUDShader2HelpFaq faq = helpUpdate.faqs [i];
			for (int j = 0; j < faq.answer.Length; j++) {
				HUDShader2HelpFaqContent content = faq.answer [j];
				if (content.type == "image") {
					images.Add (content.content);
				}
			}
		}

		checkImageDownload ();
	}

	private static void checkImageDownload () {
		if (images.Count > 0) {
			if (File.Exists (Application.dataPath + EDITORFOLDER + images [0])) {
				images.RemoveAt (0);
				checkImageDownload ();
			} else {
				LoadImage (UPDATEURL + images [0]);
			}
		} else {
			faqUpdateFinished ();
		}
	}

	private static void LoadImage(string url)
	{
		Debug.Log (url);
		wwwImage = new WWW(url);
		#if UNITY_EDITOR
		if (!EditorApplication.isPlaying)
			EditorApplication.update = MyUpdateImage;
		else
			WaitForDownloadImage();
		#else
		WaitForDownloadImage();
		#endif
	}

	#if UNITY_EDITOR
	private static void MyUpdateImage ()
	{
		if (wwwImage.isDone)
		{
			EditorApplication.update = null;
			LoadCompletedImage();
		}
	}
	#endif

	private static IEnumerator WaitForDownloadImage()
	{
		yield return wwwImage;
		LoadCompletedImage();
	}

	private static void LoadCompletedImage()
	{
		Texture2D loadedImage = wwwImage.texture;
		byte[] bytes = loadedImage.EncodeToPNG ();
		File.WriteAllBytes ("Assets" + EDITORFOLDER + images[0], bytes);
		AssetDatabase.Refresh ();

		TextureImporter imageImporter = (TextureImporter) AssetImporter.GetAtPath("Assets" + EDITORFOLDER + images[0]);
		imageImporter.textureType = TextureImporterType.Sprite;
		imageImporter.spriteImportMode = SpriteImportMode.Single;
		imageImporter.maxTextureSize = 4096;
		imageImporter.sRGBTexture = false;
		imageImporter.mipmapEnabled = false;
		imageImporter.SaveAndReimport ();

		images.RemoveAt (0);
		checkImageDownload ();
	}

	private static void faqUpdateFinished () {
		help = helpUpdate;
		initializeFoldouts ();
		Instance.Repaint();
		File.WriteAllText (Application.dataPath + EDITORFOLDER + "help.json", JsonUtility.ToJson(help));
		Debug.Log ("Update finished");
	}
	//End Image Loading

	void OnGUI () {
		Rect r = position;

		scrollPos = GUILayout.BeginScrollView (scrollPos);

		GUIStyle nonbreakingLabelStyle;
		nonbreakingLabelStyle = new GUIStyle();
		nonbreakingLabelStyle.wordWrap = true;

		if (help != null) {

			Texture2D logo = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets"+EDITORFOLDER+"Logo-Khodrin-hud-shader.png", typeof(Texture2D));
			GUI.DrawTexture (GUILayoutUtility.GetRect (300,100), logo, ScaleMode.ScaleToFit);

			if (GUI.Button (GUILayoutUtility.GetRect (100, 20), "Look for FAQ updates")) {
				updateFAQ ();
			}

			EditorGUILayout.LabelField ("Version: "+help.version);
			EditorGUILayout.LabelField ("FAQ Revision: "+help.revision);
			RectOffset rctOff = GUI.skin.label.padding;
			rctOff.left = 5;
			nonbreakingLabelStyle.padding = rctOff;
			GUILayout.Label (help.intro, nonbreakingLabelStyle, GUILayout.MaxWidth(position.width));
			EditorGUILayout.Space ();
			rctOff = GUI.skin.label.padding;

			EditorGUILayout.LabelField ("FAQ:");

			for (int i = 0; i < help.faqs.Length; i++) {
				HUDShader2HelpFaq faq = help.faqs [i];

				GUILayout.BeginVertical("HelpBox");
				GUILayout.BeginHorizontal ();
				foldouts[i] = EditorGUILayout.Foldout (foldouts[i], faq.question);
				GUILayout.EndHorizontal ();

				if (foldouts [i]) {
					GUILayout.BeginHorizontal ();
					GUILayout.BeginVertical ("GroupBox");

					for (int j = 0; j < faq.answer.Length; j++) {
						HUDShader2HelpFaqContent content = faq.answer [j];
						switch (content.type) {
						case "text":
							GUILayout.Label (content.content, nonbreakingLabelStyle, GUILayout.MaxWidth(position.width));
							GUILayout.Space (10);
							break;
						case "image":
							Texture2D image = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets" + EDITORFOLDER + content.content, typeof(Texture2D));
							if (GUI.Button (GUILayoutUtility.GetRect (128, 128), image)) {
								HUDShader2WindowImage imageWindow = (HUDShader2WindowImage)EditorWindow.GetWindow (typeof(HUDShader2WindowImage), true, "Image");
								imageWindow.image_path = "Assets" + EDITORFOLDER + content.content;
								imageWindow.Show ();
							}
							GUILayout.Space (10);
							break;
						}
					}

					GUILayout.EndVertical ();
					GUILayout.EndHorizontal ();
				}

				GUILayout.EndVertical ();
			}

		} else {
			EditorGUILayout.HelpBox("The helpfile could not be found. Make sure the file \"help.json\" is located in \""+EDITORFOLDER+"\"", MessageType.Error);
		}

		GUILayout.EndScrollView ();
	}
}

public class HUDShader2WindowImage : EditorWindow {
	public string image_path = "";

	public static void ShowWindow () {
		EditorWindow.GetWindow (typeof(HUDShader2WindowImage));
	}

	void OnGUI () {
		Rect r = position;

		Texture2D image = (Texture2D)AssetDatabase.LoadAssetAtPath (image_path, typeof(Texture2D));
		GUI.Button (GUILayoutUtility.GetRect(position.width, position.height), image);
	}
}

[System.Serializable]
public class HUDShader2HelpCollection {
	public string intro;
	public string version;
	public int revision;

	public HUDShader2HelpFaq[] faqs;
}

[System.Serializable]
public class HUDShader2HelpFaq {
	public string question;
	public HUDShader2HelpFaqContent[] answer;
}

[System.Serializable]
public class HUDShader2HelpFaqContent {
	public string type;
	public string content;
}