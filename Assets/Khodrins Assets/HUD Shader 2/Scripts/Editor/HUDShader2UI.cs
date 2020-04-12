using UnityEngine;
using UnityEditor;
using System;

public class HUDShader2UI : ShaderGUI {

	private string MipMapError = "The supllied Texture has MipMaps enabled. This will most likely lead to wired Artifacts. Please disable \"Generate Mip Maps\" in the Texture Import settings.";
	private string[] options = new string[]
	{
		"Seconds","Minutes","Hours","FPS","X Position","Y Position","Z Position","Light Intensity","Light Alpha", 
	};
	private static string EDITORFOLDER = "/Khodrins Assets/HUD Shader 2/Scripts/Editor/";

	public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties)
	{
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();

		Texture2D logo = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets"+EDITORFOLDER+"Logo-Khodrin-hud-shader.png", typeof(Texture2D));
		GUI.DrawTexture (GUILayoutUtility.GetRect (300,100), logo, ScaleMode.ScaleToFit);

		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal();

		if (GUI.Button (GUILayoutUtility.GetRect (100, 20), "Help")) {
			HUDShader2Window helpWindow = (HUDShader2Window)EditorWindow.GetWindow (typeof(HUDShader2Window), true, "Help");
			helpWindow.Show ();
		}

		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

		//Background
		GUILayout.BeginVertical("HelpBox");
		GUILayout.Label("Base Settings");
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical("GroupBox");

		MaterialProperty _BackgroundImage = ShaderGUI.FindProperty("_BackgroundImage", properties);
		_BackgroundImage.textureValue = (Texture2D)EditorGUILayout.ObjectField ("Background Texture", _BackgroundImage.textureValue, typeof (Texture2D), false);

		MaterialProperty _BackgroundColor = ShaderGUI.FindProperty("_BackgroundColor", properties);
		_BackgroundColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Background Color"), _BackgroundColor.colorValue, false, true, false, null);

		MaterialProperty _BackgroundEmission = ShaderGUI.FindProperty("_BackgroundEmission", properties);
		_BackgroundEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Background Emission"), _BackgroundEmission.colorValue,  false, false, true, null);

		MaterialProperty _LODDistance = ShaderGUI.FindProperty ("_LODDistance", properties);
		_LODDistance.floatValue = EditorGUILayout.Slider ("Culling Distance", _LODDistance.floatValue, 0, 50);

		MaterialProperty _DistanceEffectsEmission = ShaderGUI.FindProperty ("_DistanceEffectsEmission", properties);
		_DistanceEffectsEmission.floatValue = EditorGUILayout.Toggle ("Culling Turns off Background Emission", _DistanceEffectsEmission.floatValue == 1)?1:0;

		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();

		//Digital Clock
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _TimeDigitalEnable = ShaderGUI.FindProperty("_TimeDigitalEnable", properties);
		_TimeDigitalEnable.floatValue = EditorGUILayout.Toggle (_TimeDigitalEnable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Digital Time Display", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_TimeDigitalEnable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _TimeDigitalScaleOffset = ShaderGUI.FindProperty ("_TimeDigitalScaleOffset", properties);
			Vector4 ScaleOffset = _TimeDigitalScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_TimeDigitalScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _Digits = ShaderGUI.FindProperty ("_Digits", properties);
			Texture2D TEX_Digits = (Texture2D)EditorGUILayout.ObjectField ("Digits*", _Digits.textureValue, typeof(Texture2D), false);
			_Digits.textureValue = TEX_Digits;
			if (TEX_Digits != null && TEX_Digits.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _Blink = ShaderGUI.FindProperty ("_Blink", properties);
			Texture2D TEX_Blink = (Texture2D)EditorGUILayout.ObjectField ("Blink", _Blink.textureValue, typeof(Texture2D), false);
			_Blink.textureValue = TEX_Blink;
			if (TEX_Blink != null && TEX_Blink.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _BlinkInterval = ShaderGUI.FindProperty ("_BlinkInterval", properties);
			_BlinkInterval.floatValue = EditorGUILayout.IntField ("Blink Interval", (int)_BlinkInterval.floatValue);

			MaterialProperty _TimeScale = ShaderGUI.FindProperty ("_TimeScale", properties);
			_TimeScale.floatValue = EditorGUILayout.FloatField ("Time Scale*", _TimeScale.floatValue);

			MaterialProperty _TimeOffset = ShaderGUI.FindProperty ("_TimeOffset", properties);
			_TimeOffset.floatValue = EditorGUILayout.FloatField ("Time Offset*", _TimeOffset.floatValue);

			MaterialProperty _DigitalWatchColor = ShaderGUI.FindProperty("_DigitalWatchColor", properties);
			_DigitalWatchColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _DigitalWatchColor.colorValue, false, true, false, null);

			MaterialProperty _DigitalWatchEmission = ShaderGUI.FindProperty("_DigitalWatchEmission", properties);
			_DigitalWatchEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _DigitalWatchEmission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Digital Milliseconds
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _TimeMillisecondsEnable = ShaderGUI.FindProperty("_TimeMillisecondsEnable", properties);
		_TimeMillisecondsEnable.floatValue = EditorGUILayout.Toggle (_TimeMillisecondsEnable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Digital Time Milliseconds", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_TimeMillisecondsEnable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _TimeMillisecondsScaleOffset = ShaderGUI.FindProperty ("_TimeMillisecondsScaleOffset", properties);
			Vector4 ScaleOffset = _TimeMillisecondsScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_TimeMillisecondsScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _Digits = ShaderGUI.FindProperty ("_Digits", properties);
			Texture2D TEX_Digits = (Texture2D)EditorGUILayout.ObjectField ("Digits*", _Digits.textureValue, typeof(Texture2D), false);
			_Digits.textureValue = TEX_Digits;
			if (TEX_Digits != null && TEX_Digits.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _TimeScale = ShaderGUI.FindProperty ("_TimeScale", properties);
			_TimeScale.floatValue = EditorGUILayout.FloatField ("Time Scale*", _TimeScale.floatValue);

			MaterialProperty _TimeOffset = ShaderGUI.FindProperty ("_TimeOffset", properties);
			_TimeOffset.floatValue = EditorGUILayout.FloatField ("Time Offset*", _TimeOffset.floatValue);

			MaterialProperty _DigitalMillisecondsColor = ShaderGUI.FindProperty("_DigitalMillisecondsColor", properties);
			_DigitalMillisecondsColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _DigitalMillisecondsColor.colorValue, false, true, false, null);

			MaterialProperty _DigitalMillisecondsEmission = ShaderGUI.FindProperty("_DigitalMillisecondsEmission", properties);
			_DigitalMillisecondsEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _DigitalMillisecondsEmission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Analog Clock
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _AnalogTimeEnable = ShaderGUI.FindProperty("_AnalogTimeEnable", properties);
		_AnalogTimeEnable.floatValue = EditorGUILayout.Toggle (_AnalogTimeEnable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Analog Time Display", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_AnalogTimeEnable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _AnalogTimeScaleOffset = ShaderGUI.FindProperty ("_AnalogTimeScaleOffset", properties);
			Vector4 ScaleOffset = _AnalogTimeScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_AnalogTimeScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _HourHand = ShaderGUI.FindProperty ("_HourHand", properties);
			Texture2D TEX_HourHand = (Texture2D)EditorGUILayout.ObjectField ("Hour Hand", _HourHand.textureValue, typeof(Texture2D), false);
			_HourHand.textureValue = TEX_HourHand;
			if (TEX_HourHand != null && TEX_HourHand.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _HourHandColor = ShaderGUI.FindProperty("_HourHandColor", properties);
			_HourHandColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Hour Hand Color"), _HourHandColor.colorValue, false, true, false, null);

			MaterialProperty _HourHandEmission = ShaderGUI.FindProperty("_HourHandEmission", properties);
			_HourHandEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Hour Hand Emission"), _HourHandEmission.colorValue,  false, false, true, null);

			MaterialProperty _MinuteHand = ShaderGUI.FindProperty ("_MinuteHand", properties);
			Texture2D TEX_MinuteHand = (Texture2D)EditorGUILayout.ObjectField ("Minute Hand", _MinuteHand.textureValue, typeof(Texture2D), false);
			_MinuteHand.textureValue = TEX_MinuteHand;
			if (TEX_MinuteHand != null && TEX_MinuteHand.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _MinuteHandColor = ShaderGUI.FindProperty("_MinuteHandColor", properties);
			_MinuteHandColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Minute Hand Color"), _MinuteHandColor.colorValue, false, true, false, null);

			MaterialProperty _MinuteHandEmission = ShaderGUI.FindProperty("_MinuteHandEmission", properties);
			_MinuteHandEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Minute Hand Emission"), _MinuteHandEmission.colorValue,  false, false, true, null);

			MaterialProperty _SecondHand = ShaderGUI.FindProperty ("_SecondHand", properties);
			Texture2D TEX_SecondHand = (Texture2D)EditorGUILayout.ObjectField ("Second Hand", _SecondHand.textureValue, typeof(Texture2D), false);
			_SecondHand.textureValue = TEX_SecondHand;
			if (TEX_SecondHand != null && TEX_SecondHand.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _SecondHandColor = ShaderGUI.FindProperty("_SecondHandColor", properties);
			_SecondHandColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Second Hand Color"), _SecondHandColor.colorValue, false, true, false, null);

			MaterialProperty _SesondHandEmission = ShaderGUI.FindProperty("_SesondHandEmission", properties);
			_SesondHandEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Second Hand Emission"), _SesondHandEmission.colorValue,  false, false, true, null);

			MaterialProperty _SecondHandSteps = ShaderGUI.FindProperty ("_SecondHandSteps", properties);
			_SecondHandSteps.floatValue = EditorGUILayout.Slider ("Second Hand Steps*", _SecondHandSteps.floatValue, 1, 100);

			MaterialProperty _TimeScale = ShaderGUI.FindProperty ("_TimeScale", properties);
			_TimeScale.floatValue = EditorGUILayout.FloatField ("Time Scale*", _TimeScale.floatValue);

			MaterialProperty _TimeOffset = ShaderGUI.FindProperty ("_TimeOffset", properties);
			_TimeOffset.floatValue = EditorGUILayout.FloatField ("Time Offset*", _TimeOffset.floatValue);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Analog Seconds
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _AnalogSecondsEnable = ShaderGUI.FindProperty("_AnalogSecondsEnable", properties);
		_AnalogSecondsEnable.floatValue = EditorGUILayout.Toggle (_AnalogSecondsEnable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Analog Seconds", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_AnalogSecondsEnable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _AnalogSecondsScaleOffset = ShaderGUI.FindProperty ("_AnalogSecondsScaleOffset", properties);
			Vector4 ScaleOffset = _AnalogSecondsScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_AnalogSecondsScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _StandaloneSecondHand = ShaderGUI.FindProperty ("_StandaloneSecondHand", properties);
			Texture2D TEX_StandaloneSecondHand = (Texture2D)EditorGUILayout.ObjectField ("Hour Hand", _StandaloneSecondHand.textureValue, typeof(Texture2D), false);
			_StandaloneSecondHand.textureValue = TEX_StandaloneSecondHand;
			if (TEX_StandaloneSecondHand != null && TEX_StandaloneSecondHand.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _StandaloneSecondColor = ShaderGUI.FindProperty("_StandaloneSecondColor", properties);
			_StandaloneSecondColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _StandaloneSecondColor.colorValue, false, true, false, null);

			MaterialProperty _StandaloneSecondEmission = ShaderGUI.FindProperty("_StandaloneSecondEmission", properties);
			_StandaloneSecondEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _StandaloneSecondEmission.colorValue,  false, false, true, null);

			MaterialProperty _SecondHandSteps = ShaderGUI.FindProperty ("_SecondHandSteps", properties);
			_SecondHandSteps.floatValue = EditorGUILayout.Slider ("Second Hand Steps*", _SecondHandSteps.floatValue, 1, 100);

			MaterialProperty _TimeScale = ShaderGUI.FindProperty ("_TimeScale", properties);
			_TimeScale.floatValue = EditorGUILayout.FloatField ("Time Scale*", _TimeScale.floatValue);

			MaterialProperty _TimeOffset = ShaderGUI.FindProperty ("_TimeOffset", properties);
			_TimeOffset.floatValue = EditorGUILayout.FloatField ("Time Offset*", _TimeOffset.floatValue);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//FPS Digital
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _DigitalFPSEnable = ShaderGUI.FindProperty("_DigitalFPSEnable", properties);
		_DigitalFPSEnable.floatValue = EditorGUILayout.Toggle (_DigitalFPSEnable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Digital FPS", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_DigitalFPSEnable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _DigitalFPSScaleOffset = ShaderGUI.FindProperty ("_DigitalFPSScaleOffset", properties);
			Vector4 ScaleOffset = _DigitalFPSScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_DigitalFPSScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _Digits = ShaderGUI.FindProperty ("_Digits", properties);
			Texture2D TEX_Digits = (Texture2D)EditorGUILayout.ObjectField ("Digits*", _Digits.textureValue, typeof(Texture2D), false);
			_Digits.textureValue = TEX_Digits;
			if (TEX_Digits != null && TEX_Digits.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _FPSColor = ShaderGUI.FindProperty("_FPSColor", properties);
			_FPSColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _FPSColor.colorValue, false, true, false, null);

			MaterialProperty _FPSEmission = ShaderGUI.FindProperty("_FPSEmission", properties);
			_FPSEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _FPSEmission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Worldpos X
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _DigitalWorldPositionXEnabled = ShaderGUI.FindProperty("_DigitalWorldPositionXEnabled", properties);
		_DigitalWorldPositionXEnabled.floatValue = EditorGUILayout.Toggle (_DigitalWorldPositionXEnabled.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Digital World X Position", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_DigitalWorldPositionXEnabled.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _DigitalWorldPositionXScaleOffset = ShaderGUI.FindProperty ("_DigitalWorldPositionXScaleOffset", properties);
			Vector4 ScaleOffset = _DigitalWorldPositionXScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_DigitalWorldPositionXScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _Digits = ShaderGUI.FindProperty ("_Digits", properties);
			Texture2D TEX_Digits = (Texture2D)EditorGUILayout.ObjectField ("Digits*", _Digits.textureValue, typeof(Texture2D), false);
			_Digits.textureValue = TEX_Digits;
			if (TEX_Digits != null && TEX_Digits.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _Signs = ShaderGUI.FindProperty ("_Signs", properties);
			Texture2D TEX_Signs = (Texture2D)EditorGUILayout.ObjectField ("Signs*", _Signs.textureValue, typeof(Texture2D), false);
			_Signs.textureValue = TEX_Signs;
			if (TEX_Signs != null && TEX_Signs.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _World_Pos_X_Scale = ShaderGUI.FindProperty ("_World_Pos_X_Scale", properties);
			_World_Pos_X_Scale.floatValue = EditorGUILayout.FloatField ("Scale", _World_Pos_X_Scale.floatValue);

			MaterialProperty _World_Pos_X_Offset = ShaderGUI.FindProperty ("_World_Pos_X_Offset", properties);
			_World_Pos_X_Offset.floatValue = EditorGUILayout.FloatField ("Offset", _World_Pos_X_Offset.floatValue);

			MaterialProperty _WorldPosXColor = ShaderGUI.FindProperty("_WorldPosXColor", properties);
			_WorldPosXColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _WorldPosXColor.colorValue, false, true, false, null);

			MaterialProperty _WorldPosXEmission = ShaderGUI.FindProperty("_WorldPosXEmission", properties);
			_WorldPosXEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _WorldPosXEmission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Worldpos Y
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _DigitalWorldPositionYEnabled = ShaderGUI.FindProperty("_DigitalWorldPositionYEnabled", properties);
		_DigitalWorldPositionYEnabled.floatValue = EditorGUILayout.Toggle (_DigitalWorldPositionYEnabled.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Digital World Y Position", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_DigitalWorldPositionYEnabled.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _DigitalWorldPositionYScaleOffset = ShaderGUI.FindProperty ("_DigitalWorldPositionYScaleOffset", properties);
			Vector4 ScaleOffset = _DigitalWorldPositionYScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_DigitalWorldPositionYScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _Digits = ShaderGUI.FindProperty ("_Digits", properties);
			Texture2D TEX_Digits = (Texture2D)EditorGUILayout.ObjectField ("Digits*", _Digits.textureValue, typeof(Texture2D), false);
			_Digits.textureValue = TEX_Digits;
			if (TEX_Digits != null && TEX_Digits.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _Signs = ShaderGUI.FindProperty ("_Signs", properties);
			Texture2D TEX_Signs = (Texture2D)EditorGUILayout.ObjectField ("Signs*", _Signs.textureValue, typeof(Texture2D), false);
			_Signs.textureValue = TEX_Signs;
			if (TEX_Signs != null && TEX_Signs.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _World_Pos_Y_Scale = ShaderGUI.FindProperty ("_World_Pos_Y_Scale", properties);
			_World_Pos_Y_Scale.floatValue = EditorGUILayout.FloatField ("Scale", _World_Pos_Y_Scale.floatValue);

			MaterialProperty _World_Pos_Y_Offset = ShaderGUI.FindProperty ("_World_Pos_Y_Offset", properties);
			_World_Pos_Y_Offset.floatValue = EditorGUILayout.FloatField ("Offset", _World_Pos_Y_Offset.floatValue);

			MaterialProperty _WorldPosYColor = ShaderGUI.FindProperty("_WorldPosYColor", properties);
			_WorldPosYColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _WorldPosYColor.colorValue, false, true, false, null);

			MaterialProperty _WorldPosYEmission = ShaderGUI.FindProperty("_WorldPosYEmission", properties);
			_WorldPosYEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _WorldPosYEmission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Worldpos Z
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _DigitalWorldPositionZEnabled = ShaderGUI.FindProperty("_DigitalWorldPositionZEnabled", properties);
		_DigitalWorldPositionZEnabled.floatValue = EditorGUILayout.Toggle (_DigitalWorldPositionZEnabled.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Digital World Z Position", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_DigitalWorldPositionZEnabled.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _DigitalWorldPositionZScaleOffset = ShaderGUI.FindProperty ("_DigitalWorldPositionZScaleOffset", properties);
			Vector4 ScaleOffset = _DigitalWorldPositionZScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			_DigitalWorldPositionZScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _Digits = ShaderGUI.FindProperty ("_Digits", properties);
			Texture2D TEX_Digits = (Texture2D)EditorGUILayout.ObjectField ("Digits*", _Digits.textureValue, typeof(Texture2D), false);
			_Digits.textureValue = TEX_Digits;
			if (TEX_Digits != null && TEX_Digits.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _Signs = ShaderGUI.FindProperty ("_Signs", properties);
			Texture2D TEX_Signs = (Texture2D)EditorGUILayout.ObjectField ("Signs*", _Signs.textureValue, typeof(Texture2D), false);
			_Signs.textureValue = TEX_Signs;
			if (TEX_Signs != null && TEX_Signs.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			MaterialProperty _World_Pos_Z_Scale = ShaderGUI.FindProperty ("_World_Pos_Z_Scale", properties);
			_World_Pos_Z_Scale.floatValue = EditorGUILayout.FloatField ("Scale", _World_Pos_Z_Scale.floatValue);

			MaterialProperty _World_Pos_Z_Offset = ShaderGUI.FindProperty ("_World_Pos_Z_Offset", properties);
			_World_Pos_Z_Offset.floatValue = EditorGUILayout.FloatField ("Offset", _World_Pos_Z_Offset.floatValue);

			MaterialProperty _WorldPosZColor = ShaderGUI.FindProperty("_WorldPosZColor", properties);
			_WorldPosZColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _WorldPosZColor.colorValue, false, true, false, null);

			MaterialProperty _WorldPosZEmission = ShaderGUI.FindProperty("_WorldPosZEmission", properties);
			_WorldPosZEmission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _WorldPosZEmission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Custom Display 1
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _OpenInput1Enable = ShaderGUI.FindProperty("_OpenInput1Enable", properties);
		_OpenInput1Enable.floatValue = EditorGUILayout.Toggle (_OpenInput1Enable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Custom Display 1", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_OpenInput1Enable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _OpenInput1ScaleOffset = ShaderGUI.FindProperty ("_OpenInput1ScaleOffset", properties);
			Vector4 ScaleOffset = _OpenInput1ScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			MaterialProperty _OpenInput1Input = ShaderGUI.FindProperty ("_OpenInput1Input", properties);
			_OpenInput1Input.floatValue = EditorGUILayout.Popup("Input", (int)_OpenInput1Input.floatValue, options);

			_OpenInput1ScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _OpenInput1Range = ShaderGUI.FindProperty ("_OpenInput1Range", properties);
			Vector4 RangeFlipbook = _OpenInput1Range.vectorValue;
			Vector2 MinMax = new Vector2 (RangeFlipbook.x, RangeFlipbook.y);

			MaterialProperty _OpenInput1Sheet = ShaderGUI.FindProperty ("_OpenInput1Sheet", properties);
			Texture2D TEX_OpenInput1Sheet = (Texture2D)EditorGUILayout.ObjectField ("Spritesheet", _OpenInput1Sheet.textureValue, typeof(Texture2D), false);
			_OpenInput1Sheet.textureValue = TEX_OpenInput1Sheet;
			if (TEX_OpenInput1Sheet != null && TEX_OpenInput1Sheet.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			Vector2 RowsCols = new Vector2 (RangeFlipbook.z, RangeFlipbook.w);

			MinMax = EditorGUILayout.Vector2Field ("Min/Max", MinMax);
			RowsCols = EditorGUILayout.Vector2Field ("Cols/Rows", RowsCols);

			_OpenInput1Range.vectorValue = new Vector4 (MinMax.x, MinMax.y, RowsCols.x, RowsCols.y);

			MaterialProperty _OpenInput1NumFrames = ShaderGUI.FindProperty ("_OpenInput1NumFrames", properties);
			_OpenInput1NumFrames.floatValue = EditorGUILayout.IntField ("Number of Frames", (int)_OpenInput1NumFrames.floatValue);

			MaterialProperty _OpenInput1Color = ShaderGUI.FindProperty("_OpenInput1Color", properties);
			_OpenInput1Color.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _OpenInput1Color.colorValue, false, true, false, null);

			MaterialProperty _OpenInput1Emission = ShaderGUI.FindProperty("_OpenInput1Emission", properties);
			_OpenInput1Emission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _OpenInput1Emission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Custom Display 2
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _OpenInput2Enable = ShaderGUI.FindProperty("_OpenInput2Enable", properties);
		_OpenInput2Enable.floatValue = EditorGUILayout.Toggle (_OpenInput2Enable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Custom Display 2", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_OpenInput2Enable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _OpenInput2ScaleOffset = ShaderGUI.FindProperty ("_OpenInput2ScaleOffset", properties);
			Vector4 ScaleOffset = _OpenInput2ScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			MaterialProperty _OpenInput2Input = ShaderGUI.FindProperty ("_OpenInput2Input", properties);
			_OpenInput2Input.floatValue = EditorGUILayout.Popup("Input", (int)_OpenInput2Input.floatValue, options);

			_OpenInput2ScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _OpenInput2Range = ShaderGUI.FindProperty ("_OpenInput2Range", properties);
			Vector4 RangeFlipbook = _OpenInput2Range.vectorValue;
			Vector2 MinMax = new Vector2 (RangeFlipbook.x, RangeFlipbook.y);

			MaterialProperty _OpenInput2Sheet = ShaderGUI.FindProperty ("_OpenInput2Sheet", properties);
			Texture2D TEX_OpenInput2Sheet = (Texture2D)EditorGUILayout.ObjectField ("Spritesheet", _OpenInput2Sheet.textureValue, typeof(Texture2D), false);
			_OpenInput2Sheet.textureValue = TEX_OpenInput2Sheet;
			if (TEX_OpenInput2Sheet != null && TEX_OpenInput2Sheet.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			Vector2 RowsCols = new Vector2 (RangeFlipbook.z, RangeFlipbook.w);

			MinMax = EditorGUILayout.Vector2Field ("Min/Max", MinMax);
			RowsCols = EditorGUILayout.Vector2Field ("Cols/Rows", RowsCols);

			_OpenInput2Range.vectorValue = new Vector4 (MinMax.x, MinMax.y, RowsCols.x, RowsCols.y);

			MaterialProperty _OpenInput2NumFrames = ShaderGUI.FindProperty ("_OpenInput2NumFrames", properties);
			_OpenInput2NumFrames.floatValue = EditorGUILayout.IntField ("Number of Frames", (int)_OpenInput2NumFrames.floatValue);

			MaterialProperty _OpenInput2Color = ShaderGUI.FindProperty("_OpenInput2Color", properties);
			_OpenInput2Color.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _OpenInput2Color.colorValue, false, true, false, null);

			MaterialProperty _OpenInput2Emission = ShaderGUI.FindProperty("_OpenInput2Emission", properties);
			_OpenInput2Emission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _OpenInput2Emission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Custom Display 3
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _OpenInput3Enable = ShaderGUI.FindProperty("_OpenInput3Enable", properties);
		_OpenInput3Enable.floatValue = EditorGUILayout.Toggle (_OpenInput3Enable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Custom Display 3", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_OpenInput3Enable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _OpenInput3ScaleOffset = ShaderGUI.FindProperty ("_OpenInput3ScaleOffset", properties);
			Vector4 ScaleOffset = _OpenInput3ScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			MaterialProperty _OpenInput3Input = ShaderGUI.FindProperty ("_OpenInput3Input", properties);
			_OpenInput3Input.floatValue = EditorGUILayout.Popup("Input", (int)_OpenInput3Input.floatValue, options);

			_OpenInput3ScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _OpenInput3Range = ShaderGUI.FindProperty ("_OpenInput3Range", properties);
			Vector4 RangeFlipbook = _OpenInput3Range.vectorValue;
			Vector2 MinMax = new Vector2 (RangeFlipbook.x, RangeFlipbook.y);

			MaterialProperty _OpenInput3Sheet = ShaderGUI.FindProperty ("_OpenInput3Sheet", properties);
			Texture2D TEX_OpenInput3Sheet = (Texture2D)EditorGUILayout.ObjectField ("Spritesheet", _OpenInput3Sheet.textureValue, typeof(Texture2D), false);
			_OpenInput3Sheet.textureValue = TEX_OpenInput3Sheet;
			if (TEX_OpenInput3Sheet != null && TEX_OpenInput3Sheet.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			Vector2 RowsCols = new Vector2 (RangeFlipbook.z, RangeFlipbook.w);

			MinMax = EditorGUILayout.Vector2Field ("Min/Max", MinMax);
			RowsCols = EditorGUILayout.Vector2Field ("Cols/Rows", RowsCols);

			_OpenInput3Range.vectorValue = new Vector4 (MinMax.x, MinMax.y, RowsCols.x, RowsCols.y);

			MaterialProperty _OpenInput3NumFrames = ShaderGUI.FindProperty ("_OpenInput3NumFrames", properties);
			_OpenInput3NumFrames.floatValue = EditorGUILayout.IntField ("Number of Frames", (int)_OpenInput3NumFrames.floatValue);

			MaterialProperty _OpenInput3Color = ShaderGUI.FindProperty("_OpenInput3Color", properties);
			_OpenInput3Color.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _OpenInput3Color.colorValue, false, true, false, null);

			MaterialProperty _OpenInput3Emission = ShaderGUI.FindProperty("_OpenInput3Emission", properties);
			_OpenInput3Emission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _OpenInput3Emission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Custom Display 4
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _OpenInput4Enable = ShaderGUI.FindProperty("_OpenInput4Enable", properties);
		_OpenInput4Enable.floatValue = EditorGUILayout.Toggle (_OpenInput4Enable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Custom Display 4", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_OpenInput4Enable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _OpenInput4ScaleOffset = ShaderGUI.FindProperty ("_OpenInput4ScaleOffset", properties);
			Vector4 ScaleOffset = _OpenInput4ScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			MaterialProperty _OpenInput4Input = ShaderGUI.FindProperty ("_OpenInput4Input", properties);
			_OpenInput4Input.floatValue = EditorGUILayout.Popup("Input", (int)_OpenInput4Input.floatValue, options);

			_OpenInput4ScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _OpenInput4Range = ShaderGUI.FindProperty ("_OpenInput4Range", properties);
			Vector4 RangeFlipbook = _OpenInput4Range.vectorValue;
			Vector2 MinMax = new Vector2 (RangeFlipbook.x, RangeFlipbook.y);

			MaterialProperty _OpenInput4Sheet = ShaderGUI.FindProperty ("_OpenInput4Sheet", properties);
			Texture2D TEX_OpenInput4Sheet = (Texture2D)EditorGUILayout.ObjectField ("Spritesheet", _OpenInput4Sheet.textureValue, typeof(Texture2D), false);
			_OpenInput4Sheet.textureValue = TEX_OpenInput4Sheet;
			if (TEX_OpenInput4Sheet != null && TEX_OpenInput4Sheet.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			Vector2 RowsCols = new Vector2 (RangeFlipbook.z, RangeFlipbook.w);

			MinMax = EditorGUILayout.Vector2Field ("Min/Max", MinMax);
			RowsCols = EditorGUILayout.Vector2Field ("Cols/Rows", RowsCols);

			_OpenInput4Range.vectorValue = new Vector4 (MinMax.x, MinMax.y, RowsCols.x, RowsCols.y);

			MaterialProperty _OpenInput4NumFrames = ShaderGUI.FindProperty ("_OpenInput4NumFrames", properties);
			_OpenInput4NumFrames.floatValue = EditorGUILayout.IntField ("Number of Frames", (int)_OpenInput4NumFrames.floatValue);

			MaterialProperty _OpenInput4Color = ShaderGUI.FindProperty("_OpenInput4Color", properties);
			_OpenInput4Color.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _OpenInput4Color.colorValue, false, true, false, null);

			MaterialProperty _OpenInput4Emission = ShaderGUI.FindProperty("_OpenInput4Emission", properties);
			_OpenInput4Emission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _OpenInput4Emission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Custom Display 5
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _OpenInput5Enable = ShaderGUI.FindProperty("_OpenInput5Enable", properties);
		_OpenInput5Enable.floatValue = EditorGUILayout.Toggle (_OpenInput5Enable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Custom Display 5", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_OpenInput5Enable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _OpenInput5ScaleOffset = ShaderGUI.FindProperty ("_OpenInput5ScaleOffset", properties);
			Vector4 ScaleOffset = _OpenInput5ScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			MaterialProperty _OpenInput5Input = ShaderGUI.FindProperty ("_OpenInput5Input", properties);
			_OpenInput5Input.floatValue = EditorGUILayout.Popup("Input", (int)_OpenInput5Input.floatValue, options);

			_OpenInput5ScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _OpenInput5Range = ShaderGUI.FindProperty ("_OpenInput5Range", properties);
			Vector4 RangeFlipbook = _OpenInput5Range.vectorValue;
			Vector2 MinMax = new Vector2 (RangeFlipbook.x, RangeFlipbook.y);

			MaterialProperty _OpenInput5Sheet = ShaderGUI.FindProperty ("_OpenInput5Sheet", properties);
			Texture2D TEX_OpenInput5Sheet = (Texture2D)EditorGUILayout.ObjectField ("Spritesheet", _OpenInput5Sheet.textureValue, typeof(Texture2D), false);
			_OpenInput5Sheet.textureValue = TEX_OpenInput5Sheet;
			if (TEX_OpenInput5Sheet != null && TEX_OpenInput5Sheet.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			Vector2 RowsCols = new Vector2 (RangeFlipbook.z, RangeFlipbook.w);

			MinMax = EditorGUILayout.Vector2Field ("Min/Max", MinMax);
			RowsCols = EditorGUILayout.Vector2Field ("Cols/Rows", RowsCols);

			_OpenInput5Range.vectorValue = new Vector4 (MinMax.x, MinMax.y, RowsCols.x, RowsCols.y);

			MaterialProperty _OpenInput5NumFrames = ShaderGUI.FindProperty ("_OpenInput5NumFrames", properties);
			_OpenInput5NumFrames.floatValue = EditorGUILayout.IntField ("Number of Frames", (int)_OpenInput5NumFrames.floatValue);

			MaterialProperty _OpenInput5Color = ShaderGUI.FindProperty("_OpenInput5Color", properties);
			_OpenInput5Color.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _OpenInput5Color.colorValue, false, true, false, null);

			MaterialProperty _OpenInput5Emission = ShaderGUI.FindProperty("_OpenInput5Emission", properties);
			_OpenInput5Emission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _OpenInput5Emission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Custom Display 6
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _OpenInput6Enable = ShaderGUI.FindProperty("_OpenInput6Enable", properties);
		_OpenInput6Enable.floatValue = EditorGUILayout.Toggle (_OpenInput6Enable.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Custom Display 6", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_OpenInput6Enable.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _OpenInput6ScaleOffset = ShaderGUI.FindProperty ("_OpenInput6ScaleOffset", properties);
			Vector4 ScaleOffset = _OpenInput6ScaleOffset.vectorValue;
			Vector2 Scale = new Vector2 (ScaleOffset.x, ScaleOffset.y);
			Vector2 Offset = new Vector2 (ScaleOffset.z, ScaleOffset.w);

			Scale = EditorGUILayout.Vector2Field ("Scale", Scale);
			Offset = EditorGUILayout.Vector2Field ("Offset", Offset);

			MaterialProperty _OpenInput6Input = ShaderGUI.FindProperty ("_OpenInput6Input", properties);
			_OpenInput6Input.floatValue = EditorGUILayout.Popup("Input", (int)_OpenInput6Input.floatValue, options);

			_OpenInput6ScaleOffset.vectorValue = new Vector4 (Scale.x, Scale.y, Offset.x, Offset.y);

			MaterialProperty _OpenInput6Range = ShaderGUI.FindProperty ("_OpenInput6Range", properties);
			Vector4 RangeFlipbook = _OpenInput6Range.vectorValue;
			Vector2 MinMax = new Vector2 (RangeFlipbook.x, RangeFlipbook.y);

			MaterialProperty _OpenInput6Sheet = ShaderGUI.FindProperty ("_OpenInput6Sheet", properties);
			Texture2D TEX_OpenInput6Sheet = (Texture2D)EditorGUILayout.ObjectField ("Spritesheet", _OpenInput6Sheet.textureValue, typeof(Texture2D), false);
			_OpenInput6Sheet.textureValue = TEX_OpenInput6Sheet;
			if (TEX_OpenInput6Sheet != null && TEX_OpenInput6Sheet.mipmapCount > 1) {
				EditorGUILayout.HelpBox(MipMapError, MessageType.Warning);
			}

			Vector2 RowsCols = new Vector2 (RangeFlipbook.z, RangeFlipbook.w);

			MinMax = EditorGUILayout.Vector2Field ("Min/Max", MinMax);
			RowsCols = EditorGUILayout.Vector2Field ("Cols/Rows", RowsCols);

			_OpenInput6Range.vectorValue = new Vector4 (MinMax.x, MinMax.y, RowsCols.x, RowsCols.y);

			MaterialProperty _OpenInput6NumFrames = ShaderGUI.FindProperty ("_OpenInput6NumFrames", properties);
			_OpenInput6NumFrames.floatValue = EditorGUILayout.IntField ("Number of Frames", (int)_OpenInput6NumFrames.floatValue);

			MaterialProperty _OpenInput6Color = ShaderGUI.FindProperty("_OpenInput6Color", properties);
			_OpenInput6Color.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), _OpenInput6Color.colorValue, false, true, false, null);

			MaterialProperty _OpenInput6Emission = ShaderGUI.FindProperty("_OpenInput6Emission", properties);
			_OpenInput6Emission.colorValue = EditorGUILayout.ColorField(new GUIContent("Emission"), _OpenInput6Emission.colorValue,  false, false, true, null);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		//Testmode
		GUILayout.BeginVertical("HelpBox");

		GUILayout.BeginHorizontal ();
		MaterialProperty _Testmode = ShaderGUI.FindProperty("_Testmode", properties);
		_Testmode.floatValue = EditorGUILayout.Toggle (_Testmode.floatValue == 1, GUILayout.Width(16))?1:0;
		EditorGUILayout.LabelField ("Testmode", GUILayout.Width(250));
		GUILayout.EndHorizontal ();

		if (_Testmode.floatValue == 1) {

			GUILayout.BeginHorizontal ();
			GUILayout.BeginVertical ("GroupBox");

			MaterialProperty _TimeTestvalue = ShaderGUI.FindProperty ("_TimeTestvalue", properties);
			_TimeTestvalue.floatValue = EditorGUILayout.FloatField ("Time in Seconds", _TimeTestvalue.floatValue);

			MaterialProperty _FPS_Testvalue = ShaderGUI.FindProperty ("_FPS_Testvalue", properties);
			_FPS_Testvalue.floatValue = EditorGUILayout.FloatField ("FPS", _FPS_Testvalue.floatValue);

			MaterialProperty _World_Pos_X_Testvalue = ShaderGUI.FindProperty ("_World_Pos_X_Testvalue", properties);
			_World_Pos_X_Testvalue.floatValue = EditorGUILayout.FloatField ("World Position X", _World_Pos_X_Testvalue.floatValue);

			MaterialProperty _World_Pos_Y_Testvalue = ShaderGUI.FindProperty ("_World_Pos_Y_Testvalue", properties);
			_World_Pos_Y_Testvalue.floatValue = EditorGUILayout.FloatField ("World Position Y", _World_Pos_Y_Testvalue.floatValue);

			MaterialProperty _World_Pos_Z_Testvalue = ShaderGUI.FindProperty ("_World_Pos_Z_Testvalue", properties);
			_World_Pos_Z_Testvalue.floatValue = EditorGUILayout.FloatField ("World Position Z", _World_Pos_Z_Testvalue.floatValue);

			MaterialProperty _Light_Intensity_Testvalue = ShaderGUI.FindProperty ("_Light_Intensity_Testvalue", properties);
			_Light_Intensity_Testvalue.floatValue = EditorGUILayout.FloatField ("Light Intensity", _Light_Intensity_Testvalue.floatValue);

			MaterialProperty _Light_Alpha_Testvalue = ShaderGUI.FindProperty ("_Light_Alpha_Testvalue", properties);
			_Light_Alpha_Testvalue.floatValue = EditorGUILayout.FloatField ("Light Alpha", _Light_Alpha_Testvalue.floatValue);

			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();

		}
		GUILayout.EndVertical ();

		EditorGUILayout.LabelField ("* This Inputs are used in different Displays.");
	}
}