using UnityEngine;
using System.Collections;
using System;

public class FileReader  {

    public void Read(string Filename)
    {
        int lFileid = IFbxFile.XXLoad(Filename);
        IntPtr loutData = new IntPtr();
        IFbxFile.XXGet_All_Data(lFileid,out loutData);

        IntPtr_Itor<XXFile_All_Data> lpData = new IntPtr_Itor<XXFile_All_Data>(loutData);
        XXFile_All_Data lFileData = lpData.Get();


        Debug.Log("hehehehehe");
        Debug.Log(lFileData.mMeshCount);

        IFbxFile.XXClose(lFileid);

    }

}
