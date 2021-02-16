using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for WordPreview
/// </summary>
public class WordPreview
{
    public static void Priview(string inFilePath,string newName, string outDirPath = "")
    {
        object missingType = Type.Missing;
        object readOnly = true;
        object isVisible = false;
        object documentFormat = 8;
       // string randomName = DateTime.Now.Ticks.ToString();
        string randomName = newName;
        object htmlFilePath = outDirPath + randomName + ".html";
        string directoryPath = outDirPath + randomName + "_files";

        object filePath = inFilePath;
        //Open the word document in background
        ApplicationClass applicationclass = new ApplicationClass();
        applicationclass.Documents.Open(ref filePath,
                                        ref readOnly,
                                        ref missingType, ref missingType, ref missingType,
                                        ref missingType, ref missingType, ref missingType,
                                        ref missingType, ref missingType, ref isVisible,
                                        ref missingType, ref missingType, ref missingType,
                                        ref missingType, ref missingType);
        applicationclass.Visible = false;
        Document document = applicationclass.ActiveDocument;

        //Save the word document as HTML file
        document.SaveAs(ref htmlFilePath, ref documentFormat, ref missingType,
                        ref missingType, ref missingType, ref missingType,
                        ref missingType, ref missingType, ref missingType,
                        ref missingType, ref missingType, ref missingType,
                        ref missingType, ref missingType, ref missingType,
                        ref missingType);

        //Close the word document
        document.Close(ref missingType, ref missingType, ref missingType);

        #region Read the Html File as Byte Array and Display it on browser
        //byte[] bytes;
        //using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
        //{
        //    BinaryReader reader = new BinaryReader(fs);
        //    bytes = reader.ReadBytes((int)fs.Length);
        //    fs.Close();
        //}
        //p.Response.BinaryWrite(bytes);
        //p.Response.Flush();
        //p.Response.End();
        #endregion

        //Process process = new Process();
        //process.StartInfo.UseShellExecute = true;
        //process.StartInfo.FileName = htmlFilePath.ToString();
        //process.Start();

        #region Delete the Html File and Diretory
        //File.Delete(htmlFilePath.ToString());
        //foreach (string file in Directory.GetFiles(directoryPath))
        //{
        //    File.Delete(file);
        //}
        //Directory.Delete(directoryPath);
        #endregion
    }
}