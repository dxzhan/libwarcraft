//
//  ModelGroup.cs
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
using Warcraft.ADT.Chunks;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.WMO.GroupFile.Chunks;

namespace Warcraft.WMO.GroupFile
{
    /// <summary>
    /// This class describes the structure of a model group, that is, the actual bulk data of a world model.
    /// </summary>
    public class ModelGroup : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the version of the model group.
        /// </summary>
        public TerrainVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the group data of the model. This is where all of the actual data is stored.
        /// </summary>
        public ModelGroupData GroupData { get; set; }

        /// <summary>
        /// Gets or sets the name of the model group.
        /// </summary>
        public string? Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the descriptive name of the model group.
        /// </summary>
        public string? DescriptiveName
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGroup"/> class.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        public ModelGroup(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            Version = br.ReadIFFChunk<TerrainVersion>();
            GroupData = br.ReadIFFChunk<ModelGroupData>();
        }

        /// <summary>
        /// Gets the position of the model group, relative to the model root.
        /// </summary>
        /// <returns>The group's position.</returns>
        public Vector3 GetPosition()
        {
            return GetBoundingBox().GetCenterCoordinates();
        }

        /// <summary>
        /// Gets the bounding box of the model group.
        /// </summary>
        /// <returns>The bounding box of the group.</returns>
        public Box GetBoundingBox()
        {
            return GroupData.BoundingBox;
        }

        /// <summary>
        /// Gets the offset to the name of this group inside the name block.
        /// </summary>
        /// <returns>An offset into the name block.</returns>
        public uint GetInternalNameOffset()
        {
            return GroupData.GroupNameOffset;
        }

        /// <summary>
        /// Gets the offset to the descriptive name of this group inside the name block.
        /// </summary>
        /// <returns>An offset into the name block.</returns>
        public uint GetInternalDescriptiveNameOffset()
        {
            return GroupData.DescriptiveGroupNameOffset;
        }

        /// <summary>
        /// Gets the vertex positions contained in this model group.
        /// </summary>
        /// <returns>A list of vertex positions.</returns>
        public IEnumerable<Vector3> GetVertices()
        {
            return GroupData.Vertices!.Vertices;
        }

        /// <summary>
        /// Gets the vertex normals contained in this model group.
        /// </summary>
        /// <returns>A list of vertex normals.</returns>
        public IEnumerable<Vector3> GetNormals()
        {
            return GroupData.Normals!.Normals;
        }

        /// <summary>
        /// Gets the texture coordinates for the vertices contained in this model group.
        /// </summary>
        /// <returns>A list of texture coordinates.</returns>
        public IEnumerable<Vector2> GetTextureCoordinates()
        {
            return GroupData.TextureCoordinates!.TextureCoordinates;
        }

        /// <summary>
        /// Gets the vertex indices contained in this model group.
        /// </summary>
        /// <returns>A list of the vertex indices.</returns>
        public IEnumerable<ushort> GetVertexIndices()
        {
            return GroupData.VertexIndices!.VertexIndices;
        }

        /// <summary>
        /// Gets the render batches contained in this model group.
        /// </summary>
        /// <returns>A list of the render batches.</returns>
        public IEnumerable<RenderBatch> GetRenderBatches()
        {
            return GroupData.RenderBatches!.RenderBatches;
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
                bw.WriteIFFChunk(Version);
                bw.WriteIFFChunk(GroupData);
            }

            return ms.ToArray();
        }
    }
}
