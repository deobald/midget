using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Reflection;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Diagnostics;

namespace Midget
{
	/// <summary>
	/// Class responsible for handling all the rendering to files
	/// </summary>
	public class FileRenderThread
	{
		private FileRenderSettings renderSettings;
		private HiddenRenderSurface renderSurface;
		private Thread thread;
		private int frameCount;
		private string tempDirectory;
	

		/// <summary>
		/// EventHandler to informing anything that is tracking progress that a frame was rendererd
		/// </summary>
		public EventHandler FrameRendered;

		public EventHandler ThreadFinishedEvent;



		public FileRenderThread(IObject3D scene, DeviceManager dm, FileRenderSettings initRenderSettings)
		{
			this.renderSettings = initRenderSettings;

			renderSurface = new HiddenRenderSurface(dm, scene, renderSettings.Width, renderSettings.Height);
			
			frameCount = renderSettings.StartFrame;
		
			thread = new Thread(new ThreadStart(this.Run));

			// setup temp directory in case the user chooses to save to a file format other than bitmap
			tempDirectory = Application.StartupPath + thread.GetHashCode().ToString();
			
			// if not a bitmap make the temp directory
			if(renderSettings.FileFormat != FileFormat.bmp)
				Directory.CreateDirectory(tempDirectory);
		}
		

		public void Pause()
		{
			//Thread.Sleep(Thread.);
		}

		public void Resume()
		{
			thread.Interrupt();
		}

		public void Reset()
		{
			if(!thread.IsAlive)
			{
				thread = null;
				thread = new Thread(new ThreadStart(this.Run));
			}
		}
		
		private void Run() 
		{
			// setup camera
			renderSurface.Camera = renderSettings.Camera;

			// render all the frames in the scene
			for(int i = renderSettings.StartFrame; i <= renderSettings.EndFrame; ++i)
			{
				// render 
				renderSurface.Render(i);
				
				this.HandleFileSaving();

				// if there something listening for render updates
				if(FrameRendered != null)
					this.FrameRendered(null,null);
			}

			// advise everybody that the thread has finished
			if(ThreadFinishedEvent != null)
				this.ThreadFinishedEvent(null,null);
		
			// if it wasn't a bitmap do file conversion
			if(renderSettings.FileFormat == FileFormat.avi)
			{
				DoAviFileConversion();
			}
			else if(renderSettings.FileFormat != FileFormat.bmp)
			{
				CleanUpTempDirectory();
			}
		}

		private void HandleFileSaving()
		{
			
			string destination;

			// if file format is bitmap save directly to output directory
			if(renderSettings.FileFormat == FileFormat.bmp)
				destination = renderSettings.FilePath + renderSettings.FileName + frameCount.ToString() + "." + renderSettings.FileFormat.ToString();
				// else save to temporary directory
			else
				destination = tempDirectory + "\\" + renderSettings.FileName + frameCount.ToString() + ".bmp";
			
			// save file
			SurfaceLoader.Save(destination, ImageFileFormat.Bmp, renderSurface.RenderTarget);


			// if it as an image other than BMP do conversion
			if(renderSettings.FileFormat != FileFormat.bmp &&renderSettings.FileFormat != FileFormat.avi)
				DoImageFileConversion();

			// increment frameCount
			++frameCount;
		}
		
		private void DoImageFileConversion()
		{
			ImageFormat format;

			if(renderSettings.FileFormat == FileFormat.gif)
				format = ImageFormat.Gif;
			else if(renderSettings.FileFormat == FileFormat.jpg)
				format = ImageFormat.Jpeg;
			else if(renderSettings.FileFormat == FileFormat.png)
				format = ImageFormat.Png;
			else 
				format = ImageFormat.Tiff;

			// open image
			Image tempImage = Image.FromFile(tempDirectory + "\\" + renderSettings.FileName + frameCount.ToString() + ".bmp");

			tempImage.Save(renderSettings.FilePath + renderSettings.FileName + frameCount.ToString() + "." + format.ToString(),format);
		
			//File.Delete(tempDirectory + "\\" + renderSettings.FileName + frameCount.ToString() + ".bmp");
		}

		private void DoAviFileConversion()
		{

			Process process = new Process();
			process.StartInfo.FileName  = Application.StartupPath + @"\bmp2avi.exe";
			process.StartInfo.Arguments = '\"' + tempDirectory + "\" " + renderSettings.FileName + ".bmp" + " \"" +
				renderSettings.FilePath + '\\' + renderSettings.FileName + ".avi\" " + renderSettings.StartFrame.ToString() + 
				" " + renderSettings.EndFrame.ToString();
			process.StartInfo.CreateNoWindow = false;
			process.StartInfo.UseShellExecute = false;

			process.Start();
			process.WaitForExit();

			CleanUpTempDirectory();
		}

		private void CleanUpTempDirectory()
		{
			// clean out directory
			//Directory.Delete(tempDirectory, true);
		}

		public Thread Thread
		{
			get { return thread; }
		}

		public FileRenderSettings RenderSettings
		{ 
			get { return renderSettings; }
		}
	}
}
