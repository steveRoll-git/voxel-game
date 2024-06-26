﻿using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelGame;

internal record AttributeDefinition(VertexAttribute Attribute, int Size);

internal class VertexFormat(params AttributeDefinition[] attributes)
{
    public readonly uint numComponents = (uint)attributes.Sum(a => a.Size);
    public uint Stride => numComponents * sizeof(float);
    public readonly AttributeDefinition[] attributes = attributes;
}
