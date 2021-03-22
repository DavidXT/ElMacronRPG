using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class BrushTool : EditorWindow//on hérite des méthodes liées aux outils
{
    [MenuItem("MyTools/LevelDesign/BrushTool")]//on créé un onglet dans Unity pour ouvrir notre outil
    public static void ShowWindow()//méthode qui sera appelée par le clique sur l'onglet
    {
        GetWindow(typeof(BrushTool));//on ouvre la fenêtre de l'outil
    }

    private GameObject _obj;
    private float _dimensions = 1;
    private Vector3 _rotation;
    private bool _rotateToggle = false;
    public const float MaxAngle = 180;
    public const float MaxDimension = 100f;
    public const float MinDimension = 0.1f;
    private void OnGUI()//pour simplifier, c'est l'update des outils
    {
        _obj = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Object to brush"), _obj, typeof(GameObject), false);//on récupère l'objet qu'on veut peindre
        EditorGUILayout.LabelField("Rescale Object : ");
        _dimensions = EditorGUILayout.Slider(_dimensions, MinDimension, MaxDimension);
        EditorGUILayout.LabelField("Rotate Object : ");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Activate Rotation");
        _rotateToggle = EditorGUILayout.Toggle(_rotateToggle);
        EditorGUILayout.EndHorizontal();
        _rotation.x = EditorGUILayout.Slider("x :",_rotation.x, -MaxAngle, MaxAngle);
        _rotation.y = EditorGUILayout.Slider("y :",_rotation.y, -MaxAngle, MaxAngle);
        _rotation.z = EditorGUILayout.Slider("z :",_rotation.z, -MaxAngle, MaxAngle);
        
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)//on récupère l'input 
        {
            //calcul compliqué pour récupérer la position de la souris
            Vector3 tempMousePosition = Event.current.mousePosition + position.position - SceneView.lastActiveSceneView.position.position; //on récupère la position de la souris sur l'écran et on la recentre sur la fenêtre "Scene" de notre Unity
            tempMousePosition /= SceneView.lastActiveSceneView.position.size; //On resize la position de la souris pour que ça rentre dans la fenêtre "Scene"
            tempMousePosition.z = 1; //on décale le Z pour interpréter le raycast correctement
            tempMousePosition.y = 1 - tempMousePosition.y; //on inverse la position de la souris en Y car les fenêtres et le moteur interprète le Y de deux manières différentes
            Ray tempRay = SceneView.lastActiveSceneView.camera.ViewportPointToRay(tempMousePosition);//on convertit la position ainsi calculée en worldPos

            if(Physics.Raycast(tempRay, out RaycastHit tempHit))
            {
                if(tempHit.collider != null)
                {
                    Vector3 tempPosCollider = tempHit.collider.transform.position;
                    Vector3 tempPosObject = Vector3.zero;
                    tempPosObject = tempPosCollider + tempHit.normal;
                    if (_rotateToggle)
                    {
                        GameObject tempObj = Instantiate(_obj, tempPosObject, Quaternion.Euler(_rotation.x, _rotation.y, _rotation.z), tempHit.collider.transform.parent);//on instantie un objet sur le point de contact dans la scene, sur la position de la souris
                        tempObj.transform.localScale = new Vector3(_dimensions, _dimensions, _dimensions);
                    }
                    else
                    {
                        GameObject tempObj = Instantiate(_obj, tempPosObject, Quaternion.identity, tempHit.collider.transform.parent);//on instantie un objet sur le point de contact dans la scene, sur la position de la souris
                        tempObj.transform.localScale = new Vector3(_dimensions, _dimensions, _dimensions);
                    }
                }
                else
                {
                    GameObject tempObj = Instantiate(_obj, tempHit.point, Quaternion.identity, tempHit.collider.transform.parent);//on instantie un objet sur le point de contact dans la scene, sur la position de la souris
                    tempObj.transform.localScale = new Vector3(_dimensions, _dimensions, _dimensions);
                }

            }
        }
    }
}