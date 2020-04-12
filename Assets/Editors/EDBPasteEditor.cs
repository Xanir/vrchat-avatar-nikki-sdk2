#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
	
public class EDBEditor : EditorWindow {
	private static int columnWidth = 300;
    private GameObject oldModel;
	private GameObject newModel;
	private DynamicBone copyBone;

	[MenuItem("Window/EDB Paste Editor")]
	static void Init() {
		EditorWindow.GetWindow<EDBEditor>();
	}
	
	void copyComponents() {
		Transform[] oldModelChildrens = oldModel.GetComponentsInChildren<Transform>();
		Transform[] newModelChildrens = newModel.GetComponentsInChildren<Transform>();
		foreach (Transform c_collide in oldModelChildrens)
		{
			DynamicBoneCollider db_col;
			if((db_col = (c_collide.gameObject.GetComponent<DynamicBoneCollider>())) != null) {
				foreach (Transform e_collide in newModelChildrens)
				{
					if(e_collide.gameObject.name == c_collide.gameObject.name) {
						DynamicBoneCollider col_value = e_collide.gameObject.AddComponent<DynamicBoneCollider>();
						col_value.m_Bound = db_col.m_Bound;
						col_value.m_Center = db_col.m_Center;
						col_value.m_Direction = db_col.m_Direction;
						col_value.m_Height = db_col.m_Height;
						col_value.m_Radius = db_col.m_Radius;
					}
				}
			}
		}
		foreach (Transform oldModelTransform in oldModelChildrens)
		{
			if((copyBone = (oldModelTransform.gameObject.GetComponent("DynamicBone") as DynamicBone)) != null) {
				foreach (Transform newModelTransform in newModelChildrens) {
					if(newModelTransform.name == oldModelTransform.name) {
						DynamicBone curr = newModelTransform.gameObject.AddComponent<DynamicBone>();
						List<DynamicBoneColliderBase> copyColliders = new List<DynamicBoneColliderBase>();
						foreach (Transform colliderNew in newModelChildrens)
						{
							DynamicBoneCollider tempCollider;
							if((tempCollider = (colliderNew.gameObject.GetComponent("DynamicBoneCollider") as DynamicBoneCollider)) != null) {
								foreach (DynamicBoneCollider item in copyBone.m_Colliders)
								{
									if(item.gameObject.name == tempCollider.gameObject.name) {
										copyColliders.Add(tempCollider as DynamicBoneColliderBase);
									}
								}
							}
						}
						curr.m_Colliders = copyColliders;
						curr.m_Damping = copyBone.m_Damping;
						curr.m_DampingDistrib = copyBone.m_DampingDistrib;
						curr.m_DistanceToObject = copyBone.m_DistanceToObject;
						curr.m_DistantDisable = copyBone.m_DistantDisable;
						curr.m_Elasticity = copyBone.m_Elasticity;
						curr.m_ElasticityDistrib = copyBone.m_ElasticityDistrib;
						curr.m_EndLength = copyBone.m_EndLength;
						curr.m_EndOffset = copyBone.m_EndOffset;
						List<Transform> excludeTransforms = new List<Transform>();
						foreach (Transform trans1 in copyBone.m_Exclusions)
						{
							foreach (Transform trans2 in newModelChildrens)
							{
								if(trans1.name == trans2.name) {
									excludeTransforms.Add(trans2);
								}
							}
						}
						curr.m_Exclusions = excludeTransforms;
						curr.m_Force = copyBone.m_Force;
						curr.m_FreezeAxis = copyBone.m_FreezeAxis;
						curr.m_Gravity = copyBone.m_Gravity;
						curr.m_Inert = copyBone.m_Inert;
						curr.m_InertDistrib = copyBone.m_InertDistrib;
						curr.m_Radius = copyBone.m_Radius;
						curr.m_RadiusDistrib = copyBone.m_RadiusDistrib;
						curr.m_ReferenceObject = copyBone.m_ReferenceObject;
						curr.m_Root = newModelTransform;
						curr.m_Stiffness = copyBone.m_Stiffness;
						curr.m_StiffnessDistrib = copyBone.m_StiffnessDistrib;
						curr.m_UpdateMode = copyBone.m_UpdateMode;
						curr.m_UpdateRate = copyBone.m_UpdateRate;
					}
				}
			}
		}
	}

    void OnGUI()
	{
		GUILayout.Label("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", EditorStyles.boldLabel);
		GUILayout.Label("Easy Dynamic Bones Paste", EditorStyles.boldLabel);
		GUILayout.Label("▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬", EditorStyles.boldLabel);

        GUILayout.Label("", GUILayout.Width(columnWidth));

        GUILayout.Label("Original Model:", GUILayout.Width(columnWidth));
        oldModel = (GameObject)EditorGUILayout.ObjectField(oldModel, typeof(GameObject), true, GUILayout.Width(columnWidth));

        GUILayout.Label("", GUILayout.Width(columnWidth));

        GUILayout.Label("New Model:", GUILayout.Width(columnWidth));
        newModel = (GameObject)EditorGUILayout.ObjectField(newModel, typeof(GameObject), true, GUILayout.Width(columnWidth));

        GUILayout.Label("", GUILayout.Width(columnWidth));

		if (GUILayout.Button("Copy", GUILayout.Width(60)))
        {
            if (oldModel != null && newModel != null & oldModel.name == "Armature" && newModel.name == "Armature")
            {
            	copyComponents();
            	this.ShowNotification(new GUIContent("Copied!"));
            } else {
            	this.ShowNotification(new GUIContent("Incorrect Armature?"));
            }
        }
    }
}
#endif