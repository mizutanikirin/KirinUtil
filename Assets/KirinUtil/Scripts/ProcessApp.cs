using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class ProcessApp : MonoBehaviour {

	private static ProcessApp instance;
	private static Process process;

	static ProcessApp Instance {

		get {
			if( instance == null ) {
				GameObject obj = new GameObject("ProcessManager");
				instance = obj.AddComponent<ProcessApp>();
			}
			return instance;
		}
	}


	public static void Run(string path)
	{
		process = new Process();
		process.StartInfo.FileName = path;

		// exit event
		process.EnableRaisingEvents = true;
		process.Exited += ProcessExited;

		// run
		process.Start();
	}

	public static void Exit(){
		if (process.HasExited == false){
			process.CloseMainWindow();
			process.Dispose();
			process = null;
		}
	}


	private static void ProcessExited(object sender, System.EventArgs e)
	{
		process.Dispose();
		process = null;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
