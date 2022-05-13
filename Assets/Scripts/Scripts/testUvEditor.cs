﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testUvEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
    Mesh mesh = GetComponent<MeshFilter>().mesh;
    Vector3[] vertices = mesh.vertices;
    Vector2[] uvs = new Vector2[vertices.Length];

    for (int i = 0; i < uvs.Length; i++)
    {
      uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
    }
    mesh.uv = uvs;
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
