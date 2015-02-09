using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class FileReader  {

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
    void ShowSkeleton(XXSkeleton_io lSkeleton)
    {
        
   //     cMaker.Make24Cube()
        int lJointCount = lSkeleton.mCount;
        GameObject[] lJointArray = new GameObject[lJointCount];
        IntPtr_Itor<XXJoint_io> lJointPtr= new IntPtr_Itor<XXJoint_io>(lSkeleton.mJoint_array) ;
        for (int i = 0; i < lJointCount; i++)
        {
            XXJoint_io ljoint = lJointPtr.GetAt(i);
            lJointArray[i] = CreateGameObject(ljoint.pName);
            
        }

        GameObject lContainer = new GameObject("skeleton");
        lJointArray[0].transform.parent = lContainer.transform;
        for (int i = 1; i < lJointCount; i++)
        {
            XXJoint_io ljoint = lJointPtr.GetAt(i);
            lJointArray[i].transform.parent = lJointArray[ljoint.parent_id].transform;
            lJointArray[i].transform.localPosition = XXUtil.GetVector3(ljoint.PosationLoca);
        }
    }
    public void Read(string Filename)
    {
        int lFileid = IFbxFile.XXLoad(Filename);
        IntPtr loutData = new IntPtr();
        IFbxFile.XXGet_All_Data(lFileid,out loutData);

        IntPtr_Itor<XXFile_All_Data> lpData = new IntPtr_Itor<XXFile_All_Data>(loutData);
        XXFile_All_Data lFileData = lpData.Get();

        

        Debug.Log("hehehehehe");
        Debug.Log(lFileData.mMeshCount);

        XXSkeleton_io lSkeleton = (XXSkeleton_io)Marshal.PtrToStructure(lFileData.mSkeleton, typeof(XXSkeleton_io));

        ShowSkeleton(lSkeleton);


        IFbxFile.XXClose(lFileid);

    }



}
