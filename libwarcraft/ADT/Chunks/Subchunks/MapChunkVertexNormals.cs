﻿//
//  MapChunkVertexNormals.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.IO;
using System.Numerics;

using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCNR chunk - Holds per-vertex normals of a map chunk.
    /// </summary>
    public class MapChunkVertexNormals : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCNR";

        /// <summary>
        /// Gets or sets the normals of the high-resolution vertices.
        /// </summary>
        public List<Vector3> HighResVertexNormals { get; set; } = new List<Vector3>();

        /// <summary>
        /// Gets or sets the normals of the low-resolution vertices.
        /// </summary>
        public List<Vector3> LowResVertexNormals { get; set; } = new List<Vector3>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkVertexNormals"/> class.
        /// </summary>
        public MapChunkVertexNormals()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkVertexNormals"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MapChunkVertexNormals(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            for (var y = 0; y < 16; ++y)
            {
                if (y % 2 == 0)
                {
                    // Read a block of 9 high res normals
                    for (var x = 0; x < 9; ++x)
                    {
                        var normX = br.ReadSByte();
                        var normZ = br.ReadSByte();
                        var normY = br.ReadSByte();

                        HighResVertexNormals.Add(new Vector3(normX, normY, normZ));
                    }
                }
                else
                {
                    // Read a block of 8 low res normals
                    for (var x = 0; x < 8; ++x)
                    {
                        var normX = br.ReadSByte();
                        var normZ = br.ReadSByte();
                        var normY = br.ReadSByte();

                        LowResVertexNormals.Add(new Vector3(normX, normY, normZ));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }
    }
}
