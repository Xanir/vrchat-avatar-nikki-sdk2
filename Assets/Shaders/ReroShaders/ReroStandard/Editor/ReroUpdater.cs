//Thanks, Azami
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.Net.Security;

public class ReroUpdater : MonoBehaviour
{

    [MenuItem("ReroUpdater/Check for Updates")]
    private static void NewMenuOption()
    {
        string currentVersion = System.IO.File.ReadAllText("Assets\\ReroShaders\\ReroStandard\\Editor\\Version.txt");
        string newVersion = string.Empty;

        using (WebClient client = new WebClient())
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            newVersion = client.DownloadString("https://pastebin.com/raw/Pg2dWaEu");
        }

        if (currentVersion == newVersion)
            EditorUtility.DisplayDialog("Update Shader?",
                "You are up to date.", "Okay");

        else if (EditorUtility.DisplayDialog("Update Shader?",
                "There is a new update available\nCurrent: " + currentVersion + "\nNew: " + newVersion + "\n\nDo you want to update?", "Yes", "No"))
        {
            Debug.Log("Downloading all the new files.");

            Application.OpenURL("https://vrcmods.com/download.php?file=4382");

            Debug.Log("Done!");
        }
        else
            Debug.Log("Not actually downloading anything.");
    }
}
