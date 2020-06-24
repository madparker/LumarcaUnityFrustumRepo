using System.IO;
using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {
	
	// Take a shot immediately
	IEnumerator Start() {
		return UploadPNG("MattTestImage");
	}

	public IEnumerator UploadPNG(string imageName) {
		// We should only read the screen buffer after rendering is complete
		yield return new WaitForEndOfFrame();
		
		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);
		
		// For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(Application.dataPath + "/../" + imageName + ".png", bytes);
		
		
		// Create a Web Form
//		WWWForm form = new WWWForm();
//		form.AddField("frameCount", Time.frameCount.ToString());
//		form.AddBinaryData("fileUpload", bytes);
//		
//		// Upload to a cgi script
//		WWW w = new WWW("http://localhost/cgi-bin/env.cgi?post", form);
//		yield return w;
//		if (w.error != null)
//			print(w.error);
//		else
//			print("Finished Uploading Screenshot");
	}
}