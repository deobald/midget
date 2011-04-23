using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Midget
{	

	/// <summary>
	/// Image file formats that can be saved too and are supported by midget
	/// </summary>
	public enum  FileFormat 
	{
		bmp, gif, jpg, png, tiff, avi
	}

	
	/// <summary>
	/// Class to store all the Rendering Settings for Rendering to a file
	/// </summary>
	public class FileRenderSettings
	{
		private FileFormat fileFormat = FileFormat.bmp;
		private string filePath = Application.UserAppDataPath;
		private string fileName = "untitled";
		private string fileExtension = "bmp";
		private int startFrame = 1;
		private int endFrame = 1;
		private int width = 640;
		private int height = 480;
		private Midget.Cameras.MidgetCamera camera;

		public FileRenderSettings()
		{
		}

		public FileFormat FileFormat
		{
			get { return fileFormat; }
			set 
			{ 
				fileFormat = value;
 
				// configure file format
				fileExtension = fileFormat.ToString();
			}
		}

		public string FilePath
		{
			get { return filePath; }
			set { filePath = value; }
		}

		public string FileName
		{
			get { return fileName; }
			set { fileName = value; }
		}

		public int StartFrame
		{
			get { return startFrame; }
			set { startFrame = value; }
		}

		public int EndFrame
		{
			get { return endFrame; }
			set { endFrame = value; }
		}

		public int Width
		{
			get { return width; }
			set { width = value;}
		}

		public int Height
		{
			get { return height; }
			set { height = value; }
		}

		public Midget.Cameras.MidgetCamera Camera
		{
			get { return camera; }
			set { camera = value; }
		}
	}
}
