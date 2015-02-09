using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;




	// export data struct!

	// Material ------------------------------------------
[StructLayout(LayoutKind.Sequential),Serializable]
public class XXTexture_io
{
	[MarshalAsAttribute(UnmanagedType.LPStr)]
		public string mFileName;
		public int mIndex;
		public int mType;
}

[StructLayout(LayoutKind.Sequential),Serializable]
public class XXTextureAR_io
{
		public int mCount;
		public IntPtr mpTexture_buf; // XXTexture_io* mpTexture_buf; // a array to XXTexture_io
}

[StructLayout(LayoutKind.Sequential),Serializable]
public class ConnectedTexture
{
		public int m_texture_usage;
		public int m_textureio_index;
}

[StructLayout(LayoutKind.Sequential),Serializable]
public class XXMaterial_io
{
	[MarshalAsAttribute(UnmanagedType.LPStr) ]
		public string mName;
		public int mType;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] mAmbient;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] mDiffuse;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] mEmissive;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] mSpecular;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] mReflection;
		public float mTransparencyFactor;
		public float mSpecularPower;
		public float mShininess;
		public float mReflectionFactor;
		public int mConnectedTexture_num;
		public IntPtr mTextureIndexes ;  //ConnectedTexture* mTextureIndexes; Array

}

[StructLayout(LayoutKind.Sequential),Serializable]
public class XXMaterialAR_io
{
		public int mCount;
		public IntPtr mpMaretial_buf; //Array//  XXMaterial_io* mpMaretial_buf;
}

	// Skeleton ------------------------------------------------
[StructLayout(LayoutKind.Sequential),Serializable]
public class XXJoint_io
{
		public int id;
		public int parent_id;
	[MarshalAsAttribute(UnmanagedType.LPStr)]
		public string pName;  
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] PosationLoca ; //XXVector3 PosationLoca;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=4)]
		public float[] RotationLoca;// XXVector4 RotationLoca;
	[MarshalAsAttribute(UnmanagedType.ByValArray,SizeConst=3)]
		public float[] ScaleLoca; //XXVector3 ScaleLoca;

}

[StructLayout(LayoutKind.Sequential),Serializable]
public class XXSkeleton_io
{
		public IntPtr mJoint_array; // XXJoint_io* mJoint_array;
		public int mCount;
		public int mStructSize_joint; // used to check if there is error in made sequence struct in C sharp
}
	

	// Geometry ------------------------------------------------
[StructLayout(LayoutKind.Sequential),Serializable]
public class SubMesh
{
		public int IndexOffset;
		public int TriangleCount;
		public int Material_index;
}

[StructLayout(LayoutKind.Sequential),Serializable]
public class XXMesh_Data
{
	[MarshalAsAttribute(UnmanagedType.LPStr)]
		public string mName;
		public int mHasNormal;
		public int mHasUV;

		public int mVertex_count;
		public int mVertex_size; // =VERTEX_STRIDE
		public IntPtr mVertices_buf; //Array  // float * mVertices_buf; //VERTEX_STRIDE

		public int mNormal_size; //=NORMAL_STRIDE
		public IntPtr mNormals_buf; // float * mNormals_buf;

		public int mUv_Size; // = UV_STRIDE
		public IntPtr mUVs_buf; // float * mUVs_buf;

		public int mIndex_count;
		public IntPtr mIndices_buf;  // unsigned int* mIndices_buf;

		public int mSubMeshes_count;
		public IntPtr mSubmeshes_buf;  // SubMesh* mSubmeshes_buf;

		public int mMaterials_count;
		public IntPtr mpMaretial_buf ; // XXMaterial_io* mpMaretial_buf;
}

	// the whole file data
[StructLayout(LayoutKind.Sequential),Serializable]
public class XXFile_All_Data
{
		//mesh
		public int mMeshCount;
		public IntPtr mMesh_buf; //  XXMesh_Data** mMesh_buf{ 0 }; // XXMesh_Data point array
		public IntPtr mTexture_ario; // XXTextureAR_io* mTexture_ario{ 0 };
		//skeleton
		public IntPtr mSkeleton;  //XXSkeleton_io* mSkeleton{ 0 };
}
	// <<!32X64
public class IntPtr_Itor<TRealType>{
	IntPtr mPtr;
	int mPtr_int;
	int mTypeSize;
	public IntPtr_Itor(IntPtr ptr){
		mPtr=ptr;
		mPtr_int = ptr.ToInt32();
		mTypeSize = Marshal.SizeOf(typeof(TRealType));
	}
    public TRealType MoveAndRead(int count)
    {
		TRealType tmp = (TRealType) Marshal.PtrToStructure( new IntPtr(mPtr_int),typeof(TRealType) );
		mPtr_int+=count*mTypeSize;
		return tmp;
	}
    public TRealType GetAt(int lindex)
    {
		return (TRealType) Marshal.PtrToStructure( new IntPtr(mPtr_int+lindex*mTypeSize),typeof(TRealType) );
	}
    public TRealType Get()
    {
		return (TRealType) Marshal.PtrToStructure( mPtr,typeof(TRealType) );
	}

    public void Move(int count)
    {
		mPtr_int+=count*mTypeSize;
		mPtr = new IntPtr(mPtr_int);
	}
}

public class XXUtilT<TRealType>
{
    public static TRealType ConverPTR(IntPtr ptr){
        return (TRealType)Marshal.PtrToStructure(ptr, typeof(TRealType));
    }


}

public class XXUtil
{
    public static Vector3 GetVector3(float[] data)
    {
        return new Vector3(data[0] * 0.01f, data[1] * 0.01f, data[2] * 0.01f);
    }
}
// !32X64>>


//<<<<<<<<<<<<<~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// function !~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

public class IFbxFile{
	[DllImport("XXFbxDll")]
	public static extern int XXLoad(string fname);

	[DllImport("XXFbxDll")]
	public static extern int XXGet_All_Data(int file_id, out IntPtr outData); //XXGet_All_Data(int file_id, XXFile_All_Data** outData);

	[DllImport("XXFbxDll")]
	public static extern void XXClose(int file_id);
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// function end !~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~>>>>>>>>>>>>>>


public class fbxInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
