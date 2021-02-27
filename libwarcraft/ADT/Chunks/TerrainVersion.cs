﻿//
//  TerrainVersion.cs
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
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MVER Chunk - Contains the ADT version.
    /// </summary>
    public class TerrainVersion : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MVER";

        /// <summary>
        /// Gets or sets the ADT version.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainVersion"/> class.
        /// </summary>
        public TerrainVersion()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainVersion"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainVersion(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            Version = br.ReadUInt32();
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Gets the size of the data contained in this chunk.
        /// </summary>
        /// <returns>The size.</returns>
        public static uint GetSize()
        {
            return 4;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(Version);
            }

            return ms.ToArray();
        }
    }
}
