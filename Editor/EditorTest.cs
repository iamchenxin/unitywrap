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

	void OnGUI()
	{
		mFileName = EditorGUILayout.TextField(mFileName);

		if (GUILayout.Button("Test"))
		{
			FileReader reader =new FileReader();
            reader.Read(mFileName);
		}
	}



	// Use this for runtime initialization
	void Start () {
	
	}

	// runtime Update is called once per frame
	void Update () {
	
	}
}