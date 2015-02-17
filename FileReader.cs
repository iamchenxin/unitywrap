using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;


public class XXFileReader { 
    int mFile_id;
    XXFile_All_Data mFileData;
    XXJoint_io[] mJointArray;
    public void Open(string FileName){
        mFile_id = IFbxFile.XXLoad(FileName);
        IntPtr loutData = new IntPtr();
        IFbxFile.XXGet_All_Data(mFile_id,out loutData);

        IntPtr_Itor<XXFile_All_Data> lpData = new IntPtr_Itor<XXFile_All_Data>(loutData);
        mFileData = lpData.Get();
    }
    public void Close(){
        IFbxFile.XXClose(mFile_id);
    }

    public XXFileReader() { }
    ~XXFileReader()
    {
        Close();
    }
    public XXJoint_io[] GetSkeleton(){
        if(mJointArray!=null){return mJointArray;} // if already load just return

        XXSkeleton_io lSkeleton = (XXSkeleton_io)Marshal.PtrToStructure(mFileData.mSkeleton, typeof(XXSkeleton_io));
        int lJointCount = lSkeleton.mCount;
        mJointArray = new XXJoint_io[lJointCount];
        IntPtr_Itor<XXJoint_io> lJointPtr= new IntPtr_Itor<XXJoint_io>(lSkeleton.mJoint_array) ;
        
        for (int i = 0; i < lJointCount; i++){
           mJointArray[i]=lJointPtr.GetAt(i);
        }
        return mJointArray;
    }
    public void LoadMesh(){

    }
}
public class SkeletonTest  {
    XXFileReader mxxfile;

    public SkeletonTest()
    {
        mxxfile = new XXFileReader();
    }
    GameObject CreateGameObject(string name)
    {
        XX_Tools.CubeMaker cMaker = new XX_Tools.CubeMaker();
        GameObject go = new GameObject(name);
        go.AddComponent<MeshRenderer>();
        var mt = new Material(Shader.Find("Diffuse"));
        mt.color = Color.white;
        go.renderer.material = mt;
        var acmf = (MeshFilter)go.AddComponent<MeshFilter>();
        acmf.mesh = cMaker.Make24Cube(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.03f, 0.08f, 0.03f), 0.1f);

        
        return go;
    }
    public void ShowSkeleton()
    {

        XXJoint_io[] rJoint_Array = mxxfile.GetSkeleton();
        int lJointCount = rJoint_Array.Length;
        GameObject[] lGOJointArray = new GameObject[lJointCount];

       
        for (int i = 0; i < lJointCount; i++){
            lGOJointArray[i] = CreateGameObject(rJoint_Array[i].pName);
            
        }

        GameObject lContainer = new GameObject("skeleton");  
        lGOJointArray[0].transform.parent = lContainer.transform;
        lGOJointArray[0].transform.localPosition = XXUtil.GetVector3(rJoint_Array[0].PosationLoca);
        for (int i = 1; i < lJointCount; i++){
            lGOJointArray[i].transform.parent = lGOJointArray[ rJoint_Array[i].parent_id ].transform;
            lGOJointArray[i].transform.localPosition = XXUtil.GetVector3(rJoint_Array[i].PosationLoca);
        }
        
    }

    public void ShowSkeleton_flat()
    {
        XXJoint_io[] rJoint_Array = mxxfile.GetSkeleton();
        int lJointCount = rJoint_Array.Length;
        GameObject[] lGOJointArray = new GameObject[lJointCount];

       
        for (int i = 0; i < lJointCount; i++){
            lGOJointArray[i] = CreateGameObject(rJoint_Array[i].pName);
            lGOJointArray[i].transform.localPosition = XXUtil.GetVector3(rJoint_Array[i].PosationLoca);
        }

        GameObject lContainer = new GameObject("skeleton");  
        lGOJointArray[0].transform.parent = lContainer.transform;

        for (int i = 1; i < lJointCount; i++){
            lGOJointArray[i].transform.parent = lContainer.transform;
        }
    }
    public void Read(string Filename)
    {
        mxxfile.Open(Filename);
    }



}
