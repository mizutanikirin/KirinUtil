using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KirinUtil {
    public class KRNFile:MonoBehaviour {


        //----------------------------------
        //  OpenTextFile
        //----------------------------------
        #region OpenTextFile
        public string OpenTextFile(string filePath) {
            print("OpenTextFile: " + filePath);

            FileInfo fi = new FileInfo(filePath);
            string returnSt = "";

            try {
                using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8)) {
                    returnSt = sr.ReadToEnd();
                }
            } catch (Exception e) {
                print(e.Message);
                returnSt = "";
            }

            return returnSt;
        }
        #endregion

        //----------------------------------
        //  WriteTextFile
        //----------------------------------
        public void WriteTextFile(string _filePath, string _contents, bool addWrite, string encode = "UTF-8") {
            print("WriteTextFile: " + _filePath);

            StreamWriter sw;
            Encoding encoding = Encoding.GetEncoding(encode);

            try {
                sw = new StreamWriter(_filePath, addWrite, encoding);
                sw.Write(_contents);
                sw.Close();
            } catch (Exception e) {
                print(e.Message);
            }
        }


        //----------------------------------
        //  AllDelFile
        //----------------------------------
        // 参考: http://kan-kikuchi.hatenablog.com/entry/DirectoryProcessor
        public void AllDelFile(string dirPath) {
            if (!Directory.Exists(dirPath)) return;

            // ディレクトリ以外の全ファイルを削除
            string[] filePaths = Directory.GetFiles(dirPath);
            foreach (string filePath in filePaths) {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            // ディレクトリの中のディレクトリも再帰的に削除
            string[] directoryPaths = Directory.GetDirectories(dirPath);
            foreach (string directoryPath in directoryPaths) {
                AllDelFile(directoryPath);
            }
        }

        //----------------------------------
        //  GetAllFilePath
        //----------------------------------
        #region GetAllFilePath
        public List<string> GetAllFilePath(string dirPath) {
            List<string> returnfilePath = new List<string>();

            returnfilePath = GetAllFilePaths(dirPath, returnfilePath);

            return returnfilePath;
        }

        private List<string> GetAllFilePaths(string dirPath, List<string> nowList) {
            List<string> returnfilePath = new List<string>();

            for (int i = 0; i < nowList.Count; i++) {
                returnfilePath.Add(nowList[i]);
            }

            if (!Directory.Exists(dirPath)) return returnfilePath;

            // ディレクトリ以外の全ファイルを削除
            string[] filePaths = Directory.GetFiles(dirPath);
            foreach (string filePath in filePaths) {
                returnfilePath.Add(filePath);
            }

            //ディレクトリの中のディレクトリも再帰的に削除
            string[] directoryPaths = Directory.GetDirectories(dirPath);
            foreach (string directoryPath in directoryPaths) {
                returnfilePath = GetAllFilePaths(directoryPath, returnfilePath);
            }

            return returnfilePath;
        }
        #endregion


        //----------------------------------
        //  get now time file name
        //----------------------------------
        #region GetNowFileName
        public string GetNowFileName(string extention) {
            string fileName = "";

            fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (extention != "")
                fileName += "." + extention;

            return fileName;
        }

        public string GetNowFileName(string format, string extention) {
            string fileName = "";

            fileName = DateTime.Now.ToString(format);
            if (extention != "")
                fileName += "." + extention;

            return fileName;
        }
        #endregion


        //----------------------------------
        //  Get Special Folder Path
        //----------------------------------
        public string GetSpecialFolderPath(SpecialFolder folder) {
            string thisPath = "";

            if (folder == SpecialFolder.Desktop)
                thisPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            else if (folder == SpecialFolder.MyDocuments)
                thisPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            else if (folder == SpecialFolder.ProgramFiles)
                thisPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            else if (folder == SpecialFolder.Startup)
                thisPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            else if (folder == SpecialFolder.System)
                thisPath = Environment.GetFolderPath(Environment.SpecialFolder.System);

            return thisPath;
        }

		//----------------------------------
		//  SaveXml
		//----------------------------------
		public void SaveXml(string xmlContents, string[] tagName, string[] value, string filePath) {
			
			string[] strArray = xmlContents.Split("\n"[0]);
			string _contents = string.Empty;
			for (int i = 0; i < tagName.Length; i++) {
				
				string str1 = "<" + tagName[i] + ">";
				string str2 = "</" + tagName[i] + ">";
				string str3 = string.Empty;

				for (int j = 0; j < strArray.Length; j++) {
					
					if (strArray[j].IndexOf(str1) != -1) {
						int num = CountChar(strArray[j], " "[0]);
						string empty = string.Empty;
						for (int k = 0; k < num; k++)
							empty += " ";
						strArray[j] = empty + str1 + value[i] + str2;
					}

					str3 = str3 + strArray[j] + "\n";
				}
				_contents = str3;
			}
			if (!(xmlContents != _contents)) return;

            string lastWord = _contents.Substring(_contents.Length - 1, 1);
            if (lastWord == "\n") {
                _contents = _contents.Substring(0, _contents.Length - 1);
            }

            xmlContents = _contents;
			WriteTextFile(filePath, _contents, false);
		}

		public int CountChar(string s, char c) {
			return s.Length - s.Replace(c.ToString(), string.Empty).Length;
		}
    }
}
