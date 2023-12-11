using UnityEngine;
using UnityEditor;
public class IsometricSceneSorter : EditorWindow
{
    [SerializeField]
    private int layerIndex = 0;

    [SerializeField]
    private bool sortOnPlay = false;

    [MenuItem("Window/Isometric Sprite Sorting", false, 10000)]  
    static void ShowWindow()
    {
        EditorWindow.GetWindow<IsometricSceneSorter>();
    }

  
    void OnEnable()
    {
        this.titleContent.text = "Isometric Sort";
        this.minSize = new Vector2(100, 60);
        this.maxSize = new Vector2(4000,60);
        EditorApplication.playmodeStateChanged += OnPlayModeChanged;
    }

    //This callback is to sort them when the Play button is pressed in the editor (If enabled by the user)
    private void OnPlayModeChanged()
    {
        if (sortOnPlay)
        {
            SortSpritesRenderers();
        }
    }
    void OnGUI()
    {
        using (new EditorGUILayout.VerticalScope())
        {
          
            //This dropdown is used to select which Sorting Layer will be affected
            if (SortingLayer.layers.Length > 0 )
            { 
                string[] sortingLayerNames = new string[SortingLayer.layers.Length];
                for (int i = 0; i < sortingLayerNames.Length; i++)
                {
                    sortingLayerNames[i] = SortingLayer.layers[i].name;
                }
                layerIndex = EditorGUILayout.Popup("Layer", layerIndex, sortingLayerNames);
            }

          
            //This Button runs the main method to sort the sprites given their Y value
            if (GUILayout.Button("Sort Sprites Layer"))
            {
                SortSpritesRenderers();
            }
            sortOnPlay = GUILayout.Toggle(sortOnPlay, "Automatically Sort on Play");
            GUILayout.Space(5);
            //This is the contact label, remove this if its annoying. (You should also adjust the minSize and maxSize Y to 50)
            if (GUILayout.Button("More info at: <color=blue><b>www.codeartist.mx</b></color>", GetLinkStyle()))
            {
                Application.OpenURL("https://codeartist.mx/tutorials/isometric-sorting/");
            }
        }
    }
    private void SortSpritesRenderers()
    {
        //Check if the selcted layer is valid, if its not throw an error
        if (layerIndex >= SortingLayer.layers.Length)
        {
            Debug.LogError("<color=red><b> Sprite Sorting Failed!! </b></color> The selected Layer is not valid or does not exist anymore.");            
        }
        else
        { 
            SpriteRenderer[] spriteRenderers= GameObject.FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[]; //Look for all the SpriteRenderers in the scene
            SortingLayer selectedLayer = SortingLayer.layers[layerIndex]; //Assigned the selected sprite layer
            int sortedSpritesCount = 0; 
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                if (spriteRenderers[i].sortingLayerID == selectedLayer.id)
                { 
                    spriteRenderers[i].sortingOrder = (int)(spriteRenderers[i].gameObject.transform.position.y* IsometricSprite.PrecisionValue); 
                    sortedSpritesCount++;
                }
            }

            if (sortedSpritesCount > 0)
            {
                Debug.Log("<color=green><b>" + sortedSpritesCount + "</b></color> Sprites sorted in Layer --> <color=green><b>" + selectedLayer.name + "</b></color>");
            }
            else
            {
                Debug.Log("No sprites found using Layer --> <color=green><b>" + selectedLayer.name + "</b></color>");
            }
        }
    }

    //Ignore this, its just to have a clickable link in the editor window
    private GUIStyle GetLinkStyle()
    {
        GUIStyle s = new GUIStyle();
        RectOffset b = s.border;
        b.left = 0;
        b.top = 0;
        b.right = 0;
        b.bottom = 0;
        s.alignment = TextAnchor.MiddleCenter;
        return s;
    }
}