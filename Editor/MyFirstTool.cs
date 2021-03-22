using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyFirstTool : EditorWindow //on hérite des méthodes des outils
{
    [MenuItem("MyTools/LevelDesign/GroundCreator")]//on l'ajoute dans les onglets de Unity
    public static void ShowWindow()
    {
        GetWindow(typeof(MyFirstTool));//on ouvre la fenêtre
    }

    private GameObject _obj;
    private Vector2Int _dimensions;

    private void OnGUI() //Update
    {
        _obj = (GameObject)EditorGUILayout.ObjectField(new GUIContent("GroundTile"), _obj, typeof(GameObject), false); //on récupère l'objet à placer pour le sol

        EditorGUILayout.BeginHorizontal(); //on place les fields les uns à côté des autres
        _dimensions.x = EditorGUILayout.IntField(_dimensions.x);//on récupère le x de dimensions
        _dimensions.y = EditorGUILayout.IntField(_dimensions.y);//on récupère le y de dimensions
        EditorGUILayout.EndHorizontal(); //fin des fields en horizontal

        if(GUILayout.Button(new GUIContent("Create ground")))//on ajoute un bouton
        {
            GameObject tempObj = new GameObject("Ground");//on créé un parent
            tempObj.transform.position = Vector3.zero;//qu'on met en 0.0.0

            for (int i = 0; i < _dimensions.x; i++)
            {
                for (int j = 0; j < _dimensions.y; j++)
                {
                    Instantiate(_obj, new Vector3(i, 0, j), Quaternion.identity, tempObj.transform);//on créé des objets à chaque positions des dimensions
                }
            }

            //GameObject newObject = Instantiate(_obj, Vector3.zero, Quaternion.identity, tempObj.transform) as GameObject;  // instatiate the object
            //newObject.transform.localScale = new Vector3(_dimensions.x,1, _dimensions.y);
        }
    }
}