using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;




namespace Midget
{
	/// <summary>
	/// Utilities class with functions responsible for movement
	/// </summary>
	public class MovementFunctions
	{
		private MovementFunctions()
		{}


		public static Quaternion GetRotationFromCursor(System.Drawing.Point pt, float fTrackBallRadius)
		{
			System.Drawing.Rectangle rc = new System.Drawing.Rectangle(0,0,1024,768);
			float xpos = (((2.0f * pt.X) / (rc.Right-rc.Left)) - 1);
			float ypos = (((2.0f * pt.Y) / (rc.Bottom-rc.Top)) - 1);
			float sz;

			if (xpos == 0.0f && ypos == 0.0f)
				return new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

			float d2 = (float)Math.Sqrt(xpos*xpos + ypos*ypos);

			if (d2 < fTrackBallRadius * 0.70710678118654752440) // Inside sphere
				sz = (float)Math.Sqrt(fTrackBallRadius*fTrackBallRadius - d2*d2);
			else                                                 // On hyperbola
				sz = (fTrackBallRadius*fTrackBallRadius) / (2.0f*d2);

			// Get two points on trackball's sphere
			Vector3 p1 = new Vector3(xpos, ypos, sz);
			Vector3 p2 = new Vector3(0.0f, 0.0f, fTrackBallRadius);

			// Get axis of rotation, which is cross product of p1 and p2
			Vector3 axis = Vector3.Cross(p1,p2);

			// Calculate angle for the rotation about that axis
			float t = Vector3.Length(Vector3.Subtract(p2,p1)) / (2.0f*fTrackBallRadius);
			if (t > +1.0f) t = +1.0f;
			if (t < -1.0f) t = -1.0f;
			float fAngle = (float)(2.0f * Math.Asin(t));

			// Convert axis to quaternion
			return Quaternion.RotationAxis(axis, fAngle);

		}

		/// <summary>
		/// Returns a quaternion for the rotation implied by the window's cursor position
		/// </summary>
		public static Quaternion GetRotationFromCursor(System.Windows.Forms.Control control, float fTrackBallRadius)
		{
			System.Drawing.Point pt = System.Windows.Forms.Cursor.Position;
			System.Drawing.Rectangle rc = control.ClientRectangle;
			pt = control.PointToClient(pt);
			float xpos = (((2.0f * pt.X) / (rc.Right-rc.Left)) - 1);
			float ypos = (((2.0f * pt.Y) / (rc.Bottom-rc.Top)) - 1);
			float sz;

			if (xpos == 0.0f && ypos == 0.0f)
				return new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

			float d2 = (float)Math.Sqrt(xpos*xpos + ypos*ypos);

			if (d2 < fTrackBallRadius * 0.70710678118654752440) // Inside sphere
				sz = (float)Math.Sqrt(fTrackBallRadius*fTrackBallRadius - d2*d2);
			else                                                 // On hyperbola
				sz = (fTrackBallRadius*fTrackBallRadius) / (2.0f*d2);

			// Get two points on trackball's sphere
			Vector3 p1 = new Vector3(xpos, ypos, sz);
			Vector3 p2 = new Vector3(0.0f, 0.0f, fTrackBallRadius);

			// Get axis of rotation, which is cross product of p1 and p2
			Vector3 axis = Vector3.Cross(p1,p2);

			// Calculate angle for the rotation about that axis
			float t = Vector3.Length(Vector3.Subtract(p2,p1)) / (2.0f*fTrackBallRadius);
			if (t > +1.0f) t = +1.0f;
			if (t < -1.0f) t = -1.0f;
			float fAngle = (float)(2.0f * Math.Asin(t));

			// Convert axis to quaternion
			return Quaternion.RotationAxis(axis, fAngle);
		}




		/// <summary>
		/// Returns a quaternion for the rotation implied by the window's cursor position
		/// </summary>
		public static Quaternion GetRotationFromCursor(System.Windows.Forms.Control control)
		{
			return GetRotationFromCursor(control, 1.0f);
		}
	}
}
