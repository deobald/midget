using System;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;


namespace Midget.Materials
{
	/// <summary>
	/// Material class for IObject3D
	/// </summary>
	/// 

	[Serializable()]
	public class MidgetMaterial
	{
		[NonSerialized]
		private Microsoft.DirectX.Direct3D.Material material;
		[NonSerialized]
		private Microsoft.DirectX.Direct3D.Texture texture;
		private string name;
		private string texturePath = "";
		private Color ambient;
		private Color diffuse;
		private Color emissive;
		private Color specular;
		private float specularSharpness; 

		public MidgetMaterial()
		{
			material = new Material();

			ambient = material.Ambient;
			diffuse = material.Diffuse;
			emissive = material.Emissive;
			specular = material.Specular;
			specularSharpness = material.SpecularSharpness;
		}

		public MidgetMaterial (string name, string texturePath, Color ambient, Color diffuse, Color emissive,
			Color specular, float specularSharpness)
		{
			this.name = name;
			this.texturePath = texturePath;
			this.ambient = ambient;
			this.diffuse = diffuse;
			this.emissive = emissive;
			this.specular = specular;
			this.specularSharpness = specularSharpness;
		}
		
		public void Initialize (Device device)
		{
			material = new Material();
			material.Ambient = ambient;
			material.Diffuse = diffuse;
			material.Emissive = emissive;
			specular = material.Specular;
			specularSharpness = material.SpecularSharpness;
			
			texture = null;

			if(texturePath.Length > 0)
			{
				try
				{
					texture = TextureLoader.FromFile(device,texturePath);
				}
				catch
				{
					System.Windows.Forms.MessageBox.Show("Texture could not be loaded");
				}
			}
		}
		

		public Color Ambient
		{
			get { return material.Ambient; }
			set {ambient = value; 
				material.Ambient = value;}
		}

		public Color Diffuse
		{
			get { return material.Diffuse; }
			set 
			{
				diffuse = value; 
				material.Diffuse = value; 
			}
		}

		public Color Emissive
		{
			get { return material.Emissive; }
			set { emissive = value;
				material.Emissive = value; }
		}

		public Color Specular
		{
			get { return material.Specular; }
			set { specular = value;
				material.Specular = value; }
		}

		public float SpecularSharpness
		{
			get { return material.SpecularSharpness; }
			set { specularSharpness = value;
				material.SpecularSharpness = value; }
		}
		
		public Microsoft.DirectX.Direct3D.Material Material
		{
			get { return material; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		
		public void SetTexture(Texture newTexture)
		{
			texture = newTexture;
		}

		public Microsoft.DirectX.Direct3D.Texture Texture
		{
			get { return texture; }
			set { texture = value; }
		}

		public string TexturePath
		{
			get { return texturePath; }
			set {texturePath = value; }
		}
	}
}
