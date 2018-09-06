 using UnityEngine;
using UnityEditor;
using System.IO;                                    //for modifying files (reading and writing)
using System.Collections.Generic;                   //For Lists

public class ModifyScripts : EditorWindow
{
    public class M_FileData
    {
        public bool exists = false;
        public string path = "";
    }
    public class M_Additions
    {
        public string target = "";
        public string add = "";
        public string nextline = "";
        public M_FileAddtionType type = M_FileAddtionType.NewLine;
    }

    #region Editor Variables
    GUISkin skin;
    Vector2 rect = new Vector2(400, 180);
    Vector2 maxrect = new Vector2(400, 180);
    private bool _executed = false;
    public enum M_FileAddtionType { Replace, NewLine, InsertLine }
    private int _index = 0;
    #endregion

    #region List Of Available Files 
    M_FileData _aiAnimator;
    M_FileData _aiMotor;
    M_FileData _aiWeaponsControl;
    M_FileData _meleeClickToMove;
    M_FileData _meleeCombatInput;
    M_FileData _thirdPersonAnimator;
    M_FileData _character;
    M_FileData _itemManager;
    M_FileData _shooterManager;
    M_FileData _bowControl;
    M_FileData _monoBehavior;
    #endregion

    [MenuItem("Invector/Multiplayer/Add Multiplayer To Invector Scripts")]
    private static void M_ChangeScripts()
    {
        GetWindow<ModifyScripts>("UNet - Modify Scripts");
    }
    private void OnGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;

        this.minSize = rect;
        this.maxSize = maxrect;
        this.titleContent = new GUIContent("UNet: Multiplayer", null, "Adds multiplayer support to Invector scripts.");
        GUILayout.BeginVertical("Add Multiplayer Compatiblity To Scripts", "window");
        GUILayout.Space(35);

        GUILayout.BeginVertical("box");
        if (_executed == false)
        {
            EditorGUILayout.HelpBox("This will modify all relevant Invector scripts by adding needed lines to their scripts. This will make these script send their needed data across the network. Note: There is no automated process to undo these changes. If you wish to undo look at the \"InvectorScriptChanges.txt\" file that will contain a list of changes.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Done! The invector scripts will now be synced across the network. If you would like to undo these changes look at the InvectorScriptChanges.txt file to see what lines were added to what files.", MessageType.Info);
        }
        GUILayout.EndVertical();
        if (_executed == false)
        {
            if (GUILayout.Button("Add Multiplayer Lines"))
            {
                _executed = true;
                M_AddMultiplayerToScripts();
            }
        }
    }

    void M_AddMultiplayerToScripts()
    {
        M_MonoBehavior();
        M_AIAnimator();
        M_AIMotor();
        M_AIWeaponsControl();
        M_MeleeClickToMoveCS();
        M_MeleeCombatInputCS();
        M_ThirdPersonAnimatorCS();
        M_Character();
        M_ItemManager();
        M_ShooterManager();
        M_BowControl();
    }

    #region Individual Files Modification Instructions
    void M_AIAnimator()
    {
        _aiAnimator = FileExists("v_AIAnimator.cs", Application.dataPath);
        if (_aiAnimator.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[4];
            adding[0] = new M_Additions();
            adding[0].target = "animator.SetTrigger(\"WeakAttack\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"WeakAttack\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "animator.SetTrigger(\"TriggerRecoil\");";
            adding[2].add = "GetComponent<NetworkAnimator>().SetTrigger(\"TriggerRecoil\");";
            adding[2].type = M_FileAddtionType.Replace;
            adding[3] = new M_Additions();
            adding[3].target = "animator.SetTrigger(\"ResetState\");";
            adding[3].add = "GetComponent<NetworkAnimator>().SetTrigger(\"ResetState\");";
            adding[3].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_aiAnimator.path, newlines);
        }
    }
    void M_AIMotor()
    {
        _aiMotor = FileExists("v_AIMotor.cs", Application.dataPath);
        if (_aiMotor.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[2];
            adding[0] = new M_Additions();
            adding[0].target = "animator.SetTrigger(\"ResetState\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"ResetState\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;

            newlines.AddRange(adding);
            ModifyFile(_aiMotor.path, newlines);
        }
    }
    void M_AIWeaponsControl()
    {
        _aiWeaponsControl = FileExists("v_AIWeaponsControl.cs", Application.dataPath);
        if (_aiWeaponsControl.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[2];
            adding[0] = new M_Additions();
            adding[0].target = "ai.animator.SetTrigger(\"EquipItem\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"EquipItem\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;

            newlines.AddRange(adding);
            ModifyFile(_aiWeaponsControl.path, newlines);
        }
    }
    void M_MeleeClickToMoveCS()
    {
        _meleeClickToMove = FileExists("vMeleeClickToMove.cs", Application.dataPath);
        if (_meleeClickToMove.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[4];
            adding[0] = new M_Additions();
            adding[0].target = "animator.SetTrigger(\"WeakAttack\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"WeakAttack\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "cc.animator.SetTrigger(\"TriggerRecoil\");";
            adding[2].add = "GetComponent<NetworkAnimator>().SetTrigger(\"TriggerRecoil\");";
            adding[2].type = M_FileAddtionType.Replace;
            adding[3] = new M_Additions();
            adding[3].target = "cc.animator.SetTrigger(\"ResetState\");";
            adding[3].add = "GetComponent<NetworkAnimator>().SetTrigger(\"ResetState\");";
            adding[3].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_meleeClickToMove.path, newlines);
        }
    }
    void M_MeleeCombatInputCS()
    {
        _meleeCombatInput = FileExists("vMeleeCombatInput.cs", Application.dataPath);
        if (_meleeCombatInput.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[4];

            adding[0] = new M_Additions();
            adding[0].target = "if (meleeManager == null) meleeManager = GetComponent<vMeleeManager>();";
            adding[0].add = "if (meleeManager == null) meleeManager = GetComponent<MultiplayerMeleeManager>();";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "if (meleeManager == null) return;";
            adding[1].add = "if (isLocalPlayer == false) return;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "{";
            adding[2].add = "    if (isLocalPlayer == false) return;";
            adding[2].nextline = "cc.lockRotation = false;";
            adding[2].type = M_FileAddtionType.InsertLine;
            adding[3] = new M_Additions();
            adding[3].target = "{";
            adding[3].add = "    if (isLocalPlayer == false) return;";
            adding[3].nextline = "cc.animator.ResetTrigger(\"WeakAttack\");";
            adding[3].type = M_FileAddtionType.InsertLine;

            newlines.AddRange(adding);
            ModifyFile(_meleeCombatInput.path, newlines);
        }
    }
    void M_ThirdPersonAnimatorCS()
    {
        _thirdPersonAnimator = FileExists("vThirdPersonAnimator.cs", Application.dataPath);
        if (_thirdPersonAnimator.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[2];
            adding[0] = new M_Additions();
            adding[0].target = "animator.SetTrigger(\"IdleRandomTrigger\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"IdleRandomTrigger\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;

            newlines.AddRange(adding);
            ModifyFile(_thirdPersonAnimator.path, newlines);
        }
    }
    void M_Character()
    {
        _character = FileExists("vCharacter.cs", Application.dataPath);
        if (_character.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[5];
            adding[0] = new M_Additions();
            adding[0].target = "if (triggerRecoilHash.isValid) animator.SetTrigger(triggerRecoilHash);";
            adding[0].add = "if (triggerRecoilHash.isValid) GetComponent<NetworkAnimator>().SetTrigger(triggerRecoilHash);";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "if (triggerReactionHash.isValid) animator.SetTrigger(triggerReactionHash);";
            adding[2].add = "if (triggerReactionHash.isValid) GetComponent<NetworkAnimator>().SetTrigger(triggerReactionHash);";
            adding[2].type = M_FileAddtionType.Replace;
            adding[3] = new M_Additions();
            adding[3].target = "if (triggerResetStateHash.isValid) animator.SetTrigger(triggerResetStateHash);";
            adding[3].add = "if (triggerResetStateHash.isValid) GetComponent<NetworkAnimator>().SetTrigger(triggerResetStateHash);";
            adding[3].type = M_FileAddtionType.Replace;
            adding[4] = new M_Additions();
            adding[4].target = "if (triggerReactionHash.isValid) animator.SetTrigger(triggerReactionHash);";
            adding[4].add = "if (triggerReactionHash.isValid) GetComponent<NetworkAnimator>().SetTrigger(triggerReactionHash);";
            adding[4].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_character.path, newlines);
        }
    }
    void M_ItemManager()
    {
        _itemManager = FileExists("vItemManager.cs", Application.dataPath);
        if (_itemManager.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[3];
            adding[0] = new M_Additions();
            adding[0].target = "animator.SetTrigger(\"EquipItem\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"EquipItem\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "animator.SetTrigger(\"ResetState\");";
            adding[2].add = "GetComponent<NetworkAnimator>().SetTrigger(\"ResetState\");";
            adding[2].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_itemManager.path, newlines);
        }
    }
    void M_ShooterManager()
    {
        _shooterManager = FileExists("vShooterManager.cs", Application.dataPath);
        if (_shooterManager.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[3];
            adding[0] = new M_Additions();
            adding[0].target = "animator.SetTrigger(\"Reload\");";
            adding[0].add = "GetComponent<NetworkAnimator>().SetTrigger(\"Reload\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "if (animator) animator.SetTrigger(\"Shoot\");";
            adding[2].add = "if (animator) GetComponent<NetworkAnimator>().SetTrigger(\"Shoot\");";
            adding[2].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_shooterManager.path, newlines);
        }
    }
    void M_BowControl()
    {
        _bowControl = FileExists("vBowControl.cs", Application.dataPath);
        if (_bowControl.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[4];
            adding[0] = new M_Additions();
            adding[0].target = "if (animator) animator.SetTrigger(\"Spring\");";
            adding[0].add = "if (animator) GetComponent<NetworkAnimator>().SetTrigger(\"Spring\");";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "if (animator) animator.SetTrigger(\"UnSpring\");";
            adding[2].add = "if (animator) GetComponent<NetworkAnimator>().SetTrigger(\"UnSpring\");";
            adding[2].type = M_FileAddtionType.Replace;
            adding[3] = new M_Additions();
            adding[3].target = "if (animator) animator.SetTrigger(\"Shot\");";
            adding[3].add = "if (animator) GetComponent<NetworkAnimator>().SetTrigger(\"Shot\");";
            adding[3].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_bowControl.path, newlines);
        }
    }
    void M_MonoBehavior()
    {
        _monoBehavior = FileExists("vMonoBehaviour.cs", Application.dataPath);
        if (_monoBehavior.exists == true)
        {
            List<M_Additions> newlines = new List<M_Additions>();
            M_Additions[] adding = new M_Additions[3];
            adding[0] = new M_Additions();
            adding[0].target = "public  class vMonoBehaviour : MonoBehaviour";
            adding[0].add = "public  class vMonoBehaviour : NetworkBehaviour";
            adding[0].type = M_FileAddtionType.Replace;
            adding[1] = new M_Additions();
            adding[1].target = "using UnityEngine;";
            adding[1].add = "using UnityEngine.Networking;";
            adding[1].type = M_FileAddtionType.NewLine;
            adding[2] = new M_Additions();
            adding[2].target = "public class vMonoBehaviour : MonoBehaviour";
            adding[2].add = "public class vMonoBehaviour : NetworkBehaviour";
            adding[2].type = M_FileAddtionType.Replace;

            newlines.AddRange(adding);
            ModifyFile(_monoBehavior.path, newlines);
        }
    }
    #endregion

    #region Modification Logic
    M_FileData FileExists(string filename, string directory)
    {
        M_FileData data = new M_FileData();
        DirectoryInfo dir = new DirectoryInfo(directory);
        foreach (FileInfo file in dir.GetFiles("*.cs"))
        {
            if (file.Name == filename)
            {
                data.exists = true;
                data.path = file.ToString();
                return data;
            }
        }
        foreach(string subDir in Directory.GetDirectories(directory))
        {
            data = FileExists(filename, subDir);
            if (data.exists == true)
            {
                return data;
            }
        }
        data.exists = false;
        data.path = "";
        return data;
    }
    void ModifyFile(string filepath, List<M_Additions> additions)
    {
        List<string> lines = new List<string>(System.IO.File.ReadAllLines(filepath));
        List<string> modified = new List<string>();
        bool added = false;

        for (int i=0; i < lines.Count; i++)
        {
            foreach (M_Additions item in additions)
            {
                if (lines[i].Trim().Equals(item.target))
                {
                    string space;
                    switch (item.type)
                    {
                        case M_FileAddtionType.NewLine:
                            added = true;
                            modified.Add(lines[i]);
                            if (!lines[i + 1].Trim().Equals(item.add))              //prevent this code from adding the same line in twice
                            {
                                space = lines[i].Split(item.target[0])[0];          //Get spaces
                                modified.Add(space + item.add);
                            }
                            break;
                        case M_FileAddtionType.Replace:
                            added = true;
                            if (!lines[i].Trim().Equals("//"+item.target))         //prevent this code from adding the same line in twice
                            {
                                space = lines[i].Split(item.target[0])[0];         //Get spaces
                                modified.Add(space+"//"+lines[i].Trim());          //Comment out the target line
                            }
                            else
                            {
                                modified.Add(lines[i]);
                            }
                            if (!lines[i+1].Trim().Equals(item.add))               //prevent this code from adding the same line in twice
                            {
                                space = lines[i].Split(item.target[0])[0];         //Get spaces
                                modified.Add(space + item.add);                    //Add new line
                            }
                            break;
                        case M_FileAddtionType.InsertLine:
                            _index = i + 1;
                            if (lines[_index].Trim() == "")
                                _index += 1;
                            if (lines[_index].Trim().Equals(item.nextline))
                            {
                                added = true;
                                space = lines[i].Split(item.target[0])[0];         //Get spaces
                                modified.Add(lines[i]);
                                modified.Add(space+item.add);
                            }
                            break;
                    }
                }
            }
            if (added == false)
            {
                modified.Add(lines[i]);
            }
            else
            {
                added = false;
            }
            
        }
        
        using (StreamWriter writer = new StreamWriter(filepath, false))
        {
            foreach (string line in modified)
            {
                writer.WriteLine(line);
            }
        }
    }
    #endregion
}
