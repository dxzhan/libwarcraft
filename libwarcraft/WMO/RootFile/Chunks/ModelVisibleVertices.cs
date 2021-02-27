﻿//
//  ModelVisibleVertices.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Holds the visible vertices.
    /// </summary>
    public class ModelVisibleVertices : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOVV";

        /// <summary>
        /// Gets the visible vertices.
        /// </summary>
        public List<Vector3> VisibleVertices { get; } = new List<Vector3>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelVisibleVertices"/> class.
        /// </summary>
        public ModelVisibleVertices()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelVisibleVertices"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelVisibleVertices(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            var vertexCount = inData.Length / 12;
            for (var i = 0; i < vertexCount; ++i)
            {
                VisibleVertices.Add(br.ReadVector3());
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                foreach (var visibleVertex in VisibleVertices)
                {
                    bw.WriteVector3(visibleVertex);
                }
            }

            return ms.ToArray();
        }
    }
}
