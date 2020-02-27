using UnityEngine;
using System.Diagnostics;
using System;
using System.IO;

namespace KirinUtil {
    public class ProcessManager:MonoBehaviour {

        //private static ProcessApp instance;
        private Process process;

        /*static ProcessApp Instance {

            get {
                if (instance == null) {
                    GameObject obj = new GameObject("ProcessManager");
                    instance = obj.AddComponent<ProcessApp>();
                }
                return instance;
            }
        }*/


        //----------------------------------
        //  外部App
        //----------------------------------
        #region App Run&Exit
        // [isRelative] 相対パスで実行するかどうか
        public void Run(string path, bool workingDirectoryOn, bool minimize = false) {
            process = new Process();
            if (workingDirectoryOn) {
                process.StartInfo.FileName = Path.GetFileName(path);
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
            } else {
                process.StartInfo.FileName = path;
            }

            // exit event
            process.EnableRaisingEvents = true;
            process.Exited += ProcessExited;
            if (minimize)
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            // run
            process.Start();
        }

        public void Exit() {
            if (process == null)
                return;

            if (process.HasExited == false) {
                process.CloseMainWindow();
                process.Dispose();
                process = null;
            }
        }


        private void ProcessExited(object sender, System.EventArgs e) {
            process.Dispose();
            process = null;
        }

        private void OnDestroy() {
            Exit();
        }
        #endregion

        #region Only Run
        // [isRelative] 相対パスで実行するかどうか
        public void RunApp(string path, bool workingDirectoryOn, bool minimize = false) {
            process = new Process();

            if (workingDirectoryOn) {
                process.StartInfo.FileName = Path.GetFileName(path);
                process.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
            } else {
                process.StartInfo.FileName = path;
            }

            if (minimize)
                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            process.Start();
        }
        #endregion

        #region Only exit

        public void ExitApp(string processName) {
            Process[] ps = Process.GetProcesses();

            foreach (Process p in ps) {
                try {
                    //プロセス名を出力する
                    if (p.ProcessName == processName) {
                        print("ExitApp: " + p.ProcessName);

                        p.CloseMainWindow();
                        p.Dispose();
                    }
                } catch (Exception ex) {
                    print("ExitApp Error: " + ex.Message);
                }
            }
        }

        #endregion

        public bool IsRunning(string processName) {
            Process[] ps = Process.GetProcesses();
            bool isRunning = false;

            foreach (Process p in ps) {
                try {
                    //プロセス名を出力する
                    if (p.ProcessName == processName) {
                        isRunning = true;
                        break;
                    }
                } catch (Exception ex) {
                    print("IsRunning Error: " + ex.Message);
                }
            }

            return isRunning;

        }
        

        //----------------------------------
        //  コマンドライン
        //----------------------------------
        #region commandline
        public void RunCommand(string exePath, bool workingDirectoryOn, string option) {
            print("[exePath] " + exePath);
            print("[option] " + option);

            string workingDirectory = "";
            if (workingDirectoryOn) {
                workingDirectory = Path.GetDirectoryName(exePath);
                exePath = Path.GetFileName(exePath);
            }

            var info = new ProcessStartInfo(exePath, option);
            if(workingDirectoryOn) info.WorkingDirectory = workingDirectory;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;

            Process processCommand = new Process();
            processCommand.StartInfo = info;

            processCommand.EnableRaisingEvents = true;
            processCommand.ErrorDataReceived += new DataReceivedEventHandler(ProcessCommandErrorDataReceived);
            processCommand.OutputDataReceived += new DataReceivedEventHandler(ProcessCommandOutputDataReceived);
            processCommand.Exited += new EventHandler(ProcessCommandExited);

            processCommand.Start();

            processCommand.BeginOutputReadLine();
            processCommand.BeginErrorReadLine();
        }


        private void ProcessCommandErrorDataReceived(object sender, DataReceivedEventArgs e) {
            print("ProcessCommandErrorDataReceived: " + e.Data);
        }

        private void ProcessCommandOutputDataReceived(object sender, DataReceivedEventArgs e) {
            Process thisProcess = (Process)sender;
            thisProcess.Dispose();
            thisProcess = null;
            print("ProcessCommandOutputDataReceived: " + e.Data);
        }

        private void ProcessCommandExited(object sender, EventArgs e) {
            Process thisProcess = (Process)sender;
            thisProcess.Dispose();
            thisProcess = null;

            print("ProcessCommandExited");
        }
        #endregion

    }
}