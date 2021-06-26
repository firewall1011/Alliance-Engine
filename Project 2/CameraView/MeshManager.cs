﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutorial;

namespace World_3D.CameraView
{

    public struct MeshData
    {
        public string modelPath;
        public string texturePath;

        public MeshData(string modelPath, string texturePath)
        {
            this.modelPath = modelPath;
            this.texturePath = texturePath;
        }
    }

    public static class MeshManager
    {
        private static Dictionary<MeshType, Mesh> loadedMeshes = new();
        private static Dictionary<MeshType, MeshData> meshPaths = new() {
            { MeshType.Bear, new MeshData("..\\..\\..\\Models\\bear.obj", "..\\..\\..\\Models\\textures\\wizardTowerDiff.png") },
        };
            


        public static Mesh GetMesh(MeshType meshType)
        {

            Mesh returnMesh;
            
            if(! loadedMeshes.TryGetValue(meshType, out returnMesh)) {
                returnMesh = CreateMesh(meshType);

                loadedMeshes[meshType] = returnMesh;
            }

            if(returnMesh == null)
            {
            
            }

            return returnMesh;

        }

        private static Mesh CreateMesh(MeshType meshType)
        {
            MeshData md;
            Mesh newMesh;
            
            if(meshPaths.TryGetValue(meshType, out md))
            {
                newMesh = new Mesh(md.modelPath, md.texturePath);
            }
            else
            {
                throw new Exception($"Model {meshType} not found");
            }

            return newMesh;
        }
    }
}
