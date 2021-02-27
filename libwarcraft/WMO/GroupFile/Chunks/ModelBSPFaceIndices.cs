﻿//
//  ModelBSPFaceIndices.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.GroupFile.Chunks
{
    /// <summary>
    /// ExtendedData chunk for BSP face indices.
    /// </summary>
    public class ModelBSPFaceIndices : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// The RIFF chunk signature of this chunk.
        /// </summary>
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOBR";

        /// <summary>
        /// Gets the list of face indices contained in this chunk.
        /// </summary>
        public List<ushort> BSPFaceIndices { get; } = new List<ushort>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBSPFaceIndices"/> class.
        /// </summary>
        public ModelBSPFaceIndices()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBSPFaceIndices"/> class.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        public ModelBSPFaceIndices(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <summary>
        /// Deserialzes the provided binary data of the object. This is the full data block which follows the data
        /// signature and data block length.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            while (ms.Position < ms.Length)
            {
                BSPFaceIndices.Add(br.ReadUInt16());
            }
        }

        /// <summary>
        /// Gets the static data signature of this data block type.
        /// </summary>
        /// <returns>A string representing the block signature.</returns>
        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                foreach (var bspFaceIndex in BSPFaceIndices)
                {
                    bw.Write(bspFaceIndex);
                }
            }

            return ms.ToArray();
        }
    }
}
