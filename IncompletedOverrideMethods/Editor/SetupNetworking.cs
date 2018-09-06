using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

public class SetupNetworking : EditorWindow {

	[MenuItem("Invector/Multiplayer/Create Network Manager")]
    private static void M_NetworkManager()
    {
        GameObject _networkManager = new GameObject("NetworkManager");
        _networkManager.AddComponent<NetworkManager>();
        _networkManager.AddComponent<NetworkManagerHUD>();
    }

    //# ------------------------------------------------------------ #

    GameObject _player = null;
    GUISkin skin;
    Vector2 rect = new Vector2(400, 180);
    Vector2 max_rect = new Vector2(400, 500);
    Editor playerPreview;
    bool generated = false;

    [MenuItem("Invector/Multiplayer/Make Player Multiplayer Compatible")]
    private static void M_MakePlayerMultiplayer()
    {
        GetWindow<SetupNetworking>("UNet - Make Player Multiplayer Compatible");
        
    }

    void PlayerPreview()
    {
        GUILayout.FlexibleSpace();

        if (_player != null)
        {
            playerPreview.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(360, 300), "window");
        }
    }

    private void OnGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;

        this.minSize = rect;
        this.maxSize = max_rect;
        this.titleContent = new GUIContent("UNet: Multiplayer", null, "Adds multiplayer support to your player.");
        GUILayout.BeginVertical("Add Multiplayer Compatiblity", "window");
        GUILayout.Space(35);

        GUILayout.BeginVertical("box");

        if (!_player)
            EditorGUILayout.HelpBox("Input your target player prefab to add multiplayer support. If you don't use a prefab the wrong object will be added to the network manager at the end.", MessageType.Info);

        _player = EditorGUILayout.ObjectField("Player Prefab", _player, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;

        if (GUI.changed && _player != null)
        {
            playerPreview = Editor.CreateEditor(_player);
        }
        if (_player != null && _player.GetComponent<SetupLocalPlayer>() != null && generated == false)
        {
            EditorGUILayout.HelpBox("This gameObject already contains the component \"SetupLocalPlayer\". Adding support again will reset it's values to default.", MessageType.Warning);
        }
        else if (_player != null && _player.GetComponent<SetupLocalPlayer>() != null && generated == true)
        {
            EditorGUILayout.HelpBox("Great, your done! Now your player's movements will be synced across the network. Note: This player has been added the the \"NetworkManager\" Gameobject.", MessageType.Info);
        }
        GUILayout.EndVertical();

        if (_player != null)
        {
            GUILayout.BeginHorizontal("box");
            PlayerPreview();
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Add Multiplayer Support"))
            {
                generated = true;
                M_SetupMultiplayer();
            }
        }
    }

    void M_SetupMultiplayer()
    {
        foreach (MonoBehaviour script in _player.GetComponents(typeof(MonoBehaviour)))
        {
            script.enabled = false;
        }
        if (_player.GetComponent<SetupLocalPlayer>() == null)
        {
            _player.AddComponent<SetupLocalPlayer>();
        }
        if (_player.GetComponent<NetworkIdentity>() == null)
        {
            _player.AddComponent<NetworkIdentity>();
        }
        if (_player.GetComponent<NetworkAnimator>() == null)
        {
            _player.AddComponent<NetworkAnimator>();
        }
        _player.GetComponent<SetupLocalPlayer>().enabled = true;

        _player.GetComponent<SetupLocalPlayer>().lerpRate = 90.0f;
        _player.GetComponent<SetupLocalPlayer>().positionLerp = 45.0f;
        _player.GetComponent<SetupLocalPlayer>().rotationLerp = 90.0f;
        _player.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        _player.GetComponent<NetworkIdentity>().enabled = true;

         NetworkAnimator na = _player.GetComponent<NetworkAnimator>();
        Animator playerAnim = _player.GetComponent<Animator>();
        na.enabled = true;
        na.animator = playerAnim;
        
        for (int i=0; i < 33; i++)
        {
            na.SetParameterAutoSend(i, true);
        }

        GameObject nm = GameObject.Find("NetworkManager");
        if (nm == null)
        {
            M_NetworkManager();
            nm = GameObject.Find("NetworkManager");
        }
        nm.GetComponent<NetworkManager>().playerPrefab = _player;
    }
}
