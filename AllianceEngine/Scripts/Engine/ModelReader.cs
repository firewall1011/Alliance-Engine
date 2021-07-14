﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AllianceEngine
{
    public static class ModelReader
    {
        public struct MeshObjectData
        {
            public Vector3[] vertices;
            public Vector2[] uvs;
            public int[] vertexIndices;
            public int[] uvIndices;
            public Texture texture;
        }

        public static List<MeshObjectData> ReadAll(string filepath, string mtlFile = null, string texturesFolder = null)
        {

            Dictionary<string, Texture> textures = ReadTextures(mtlFile, texturesFolder);

            using (StreamReader file = File.OpenText(filepath))
            {
                List<MeshObjectData> meshes = new();

                while (!file.EndOfStream)
                {
                    List<Vector3> vertices = new();
                    List<Vector2> uvs = new();
                    List<int> vertexIndices = new();
                    List<int> uvIndices = new();
                    string textureName = "";
                    bool meshEmpty = true;

                    string line = file.ReadLine();

                    if (line == null)
                        break;

                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();

                        if (line == null)
                            break;

                        if (line.StartsWithOptimized("o "))
                        {
                            if (meshEmpty == false && textureName != "") //if we have info to fill the mesh
                            {
                                meshes.Add(new MeshObjectData()
                                {
                                    vertices = vertices.ToArray(),
                                    uvs = uvs.ToArray(),
                                    vertexIndices = vertexIndices.ToArray(),
                                    uvIndices = uvIndices.ToArray(),
                                    texture = textures[textureName]
                                });

                                meshEmpty = true;
                                vertices = new();
                                uvs = new();
                                vertexIndices = new();
                                uvIndices = new();
                                textureName = "";
                            }
                        }
                        else if (line.StartsWithOptimized("v "))
                        {
                            string[] verticesText = line[2..].Split(' ');
                            Vector3 newVertex = Vector3.Zero;

                            if (Single.TryParse(verticesText[0], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out float axis))
                                newVertex.X = axis;
                            if (Single.TryParse(verticesText[1], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out axis))
                                newVertex.Y = axis;
                            if (Single.TryParse(verticesText[2], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out axis))
                                newVertex.Z = axis;

                            vertices.Add(newVertex);

                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("vt "))
                        {
                            string[] uvsText = line[3..].Split(' ');
                            Vector2 newUv = Vector2.Zero;

                            if (float.TryParse(uvsText[0], NumberStyles.Float, CultureInfo.InvariantCulture,
                                out float axis))
                                newUv.X = axis;
                            if (float.TryParse(uvsText[1], NumberStyles.Float, CultureInfo.InvariantCulture, out axis))
                                newUv.Y = axis;

                            uvs.Add(newUv);

                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("f "))
                        {
                            string[] faces = line[2..].Split(' ');

                            foreach (var comb in faces)
                            {
                                string[] indices = comb.Split('/');
                                if (int.TryParse(indices[0], out int vertexIndex))
                                    vertexIndices.Add(vertexIndex);
                                if (int.TryParse(indices[1], out int uvIndex))
                                    uvIndices.Add(uvIndex);
                            }

                            meshEmpty = false;
                        }
                        else if (line.StartsWithOptimized("usemtl "))
                        {
                            textureName = line.Split(' ').Last();
                        }
                    }

                    meshes.Add(new MeshObjectData()
                    {
                        vertices = vertices.ToArray(),
                        uvs = uvs.ToArray(),
                        vertexIndices = vertexIndices.ToArray(),
                        uvIndices = uvIndices.ToArray(),
                        texture = textures[textureName]
                    });
                }

                return meshes;
            }

            throw new FileNotFoundException("Could not find file with path " + filepath);

        }

        private static Dictionary<string, Texture> ReadTextures(string mtlFile, string texturesFolder)
        {
            Dictionary<string, Texture> textures = new();

            using (StreamReader file = File.OpenText(mtlFile))
            {
                string mtlName = "";
                string txtImgName = "";
                
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();

                    if (line == null)
                        break;

                    if (line.StartsWith("newmtl "))
                    {
                        mtlName = line.Split(' ')[1];
                    }
                    else if (line.StartsWith("map_Kd "))
                    {
                        
                        txtImgName = line.Split(" ").Last();

                        if (txtImgName.Contains("\\"))
                        {
                            txtImgName = line.Split("\\").Last();    
                        }

                        string path = Path.Join(texturesFolder, txtImgName);
                        Texture newTxt = new Texture(path);

                        textures.Add(mtlName, newTxt);
                    }
                }

                return textures;
            }
            
            throw new FileNotFoundException("Could not find file with path " + mtlFile);
        }
    }
}
