using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorTest : EditorWindow {

   
	[MenuItem("Window/EditorTest")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(EditorTest));
	}


	string mFileName;
	int mFileid;
    SkeletonTest ske;

	void OnGUI()
	{
		mFileName = EditorGUILayout.TextField(mFileName);

		if (GUILayout.Button("load"))
		{
            if(ske==null){ske = new SkeletonTest();}
            ske.Read(mFileName);
		}
        if (GUILayout.Button("skeleton"))
        {
            if(ske!=null){
                ske.ShowSkeleton();
            }
           
        }
        if (GUILayout.Button("Flat_skeleton"))
        {
            if(ske!=null){
                ske.ShowSkeleton_flat();
            }
        }
	}



	// Use this for runtime initialization
	void Start () {
	
	}

	// runtime Update is called once per frame
	void Update () {
	
	}
}